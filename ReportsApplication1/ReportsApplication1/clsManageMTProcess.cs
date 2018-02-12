using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Utilities;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;



namespace ReportsApplication1
{
    class clsManageMTProcess
    {
        // Get connection string
        private string Conn = null;
        // Create DB Logger
        private Logging Log = null;
        // ConfigTable Access
        private ConfigTable Conf = null;

        // Use Test DB flag
        private bool UseTestDB = false;

        // Worker Thread object we are running under
        public static WorkerThread WT = null;

        // RefID for AppStats association
        private static string refID = string.Empty;


        private int MTMaxRecsPerProc = 3000;
        private int MTMaxParallelProcs = 2;

        // Count of total rows in Batch / Run
        private int rowCount = 0;

        // Count of Processes to start
        private int procCount = 0;

        // Save Batch, Run, LetterType
        private string Batch = string.Empty;
        private string Run = string.Empty;
        private string Lettertype = string.Empty;

        // Lock process start
        private object LockProcessStart = new object();
        private int interlockedWorkerDone = 0;
        private int interlockedWorkerStarted = 0;
        private object LockLogging = new object();
        private object LockWT = new object();


        // Process Status
        class ProcStat
        {
            public int ProcNum = 0;
            public string RecStart = string.Empty;
            public string RecEnd = string.Empty;
            public bool Started = false;
            public bool Ended = false;
            public bool Error = false;
            public string Batch = string.Empty;
            public string Run = string.Empty;
            public string LetterType = string.Empty;
            public int ReturnCode = 0;
            public string RunTime = string.Empty;
        }

        // Process Status array, process Task array, process work queue
        private ProcStat[] processStatus = null;
        private Task[] processTasks = null;
        //private Process[] taskProcess = null;
        private ConcurrentQueue<ProcStat> processWorkQueue = new ConcurrentQueue<ProcStat>();



        // Constructors
        public clsManageMTProcess()
        {
            Conn = DbAccess.GetConnectionString();
            UseTestDB = DbAccess.UseTestDB;

            Conf = new ConfigTable(Conn);
            Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";

            Log = new Logging(Conn, "AppLog");
            Log.SourceBase = "clsManageMTProcess";

            MTMaxRecsPerProc = Conf.GetInt("MTMaxRecsPerProc", 3000);
            MTMaxParallelProcs = Conf.GetInt("MTMaxParallelProcs", 2);
        }

        // Constructor passing Worker Thread
        public clsManageMTProcess(WorkerThread wt) : this()
        {
            WT = wt;
        }


        // Generate PDFs using Multithread started processes
        public void MTProcGenerateIndividualPDFs(string batch, string run, string lettertype, DataTable dt, string refid)
        {
            Batch = batch;
            Run = run;
            Lettertype = lettertype;
            refID = refid;

            Log.Info(string.Format("Start MTProcGenerateIndividualPDFs for Batch: {0}  Run: {1} in {2}", batch, run,
                                    (DbAccess.UseTestDB) ? "TEST" : "PROD"));

            MTCalcRecsToProcess(dt);
            StartMTProcs();

            Log.Info(string.Format("End MTProcGenerateIndividualPDFs for Batch: {0}  Run: {1} in {2}", batch, run,
                                    (DbAccess.UseTestDB) ? "TEST" : "PROD"));
        }


        // Process DataTable of records for batch / run and allocate ranges of records
        // for each process
        private void MTCalcRecsToProcess(DataTable dt)
        {
            // Calc number of processes needed for all records
            rowCount = dt.Rows.Count;
            procCount = rowCount / MTMaxRecsPerProc;
            procCount += (rowCount % MTMaxRecsPerProc == 0) ? 0 : 1;

            // Create array of process status classes
            processStatus = new ProcStat[procCount];

            // For each process status in the array fill in values and presortid ranges
            for (int i = 0; i < processStatus.Length; i++)
            {
                // Create and Number the process
                processStatus[i] = new ProcStat();
                processStatus[i].ProcNum = i;

                // Get presortIDs for start and end of ranges
                int ndx = MTMaxRecsPerProc * i;
                processStatus[i].RecStart = dt.Rows[ndx]["MPresortID"].ToString();
                ndx += MTMaxRecsPerProc - 1;
                if (ndx > rowCount - 1)
                    ndx = rowCount - 1;
                processStatus[i].RecEnd = dt.Rows[ndx]["MPresortID"].ToString();

                // Initialize the rest of the process status
                processStatus[i].Started = false;
                processStatus[i].Ended = false;
                processStatus[i].Error = false;
                processStatus[i].ReturnCode = 0;
                processStatus[i].Batch = Batch;
                processStatus[i].Run = Run;
                processStatus[i].LetterType = Lettertype;
                processStatus[i].RunTime = string.Empty;
            }

            var tasks = (MTMaxParallelProcs > processStatus.Length) ? processStatus.Length : MTMaxParallelProcs;
            Log.Info(string.Format("MT Individual PDFs: {0} Worker runs, {1} in parallel", procCount, tasks));

            if (WT != null)
                WT.ReportProgress(-2, string.Format("MT Individual PDFs: {0} Worker runs, {1} in parallel",
                                                     procCount, tasks));
        }


        // Start the MultiThreaded Processes
        private void StartMTProcs()
        {

            // First Queue all the Process Status objects for multithread processing
            for (int i = 0; i < processStatus.Length; i++)
                processWorkQueue.Enqueue(processStatus[i]);

            // Create the Task Array based on the max number of parallel processes config parm
            //  but no more than total processes needed.
            var tasks = (MTMaxParallelProcs > processStatus.Length) ? processStatus.Length : MTMaxParallelProcs;
            processTasks = new Task[tasks];

            // Create the Process array like the task array and init
            //taskProcess = new Process[MTMaxParallelProcs];
            //for (int i = 0; i < taskProcess.Length; i++) taskProcess[i] = null;

            if (WT != null)
            {
                string text = string.Format("{0} Workers Done of {1}", interlockedWorkerDone, procCount);
                WT.ReportProgress(-1, text);
            }

            // For each of the max parallel processes, start a Task(thread) to process the queue
            for (int i = 0; i < processTasks.Length; i++)
            {
                // for each Task start the thread as a lambda
                processTasks[i] = Task.Factory.StartNew(() =>
                {
                    // while there are items in the queue process them
                    while (processWorkQueue.Count > 0)
                    {
                        // Get next item from queue
                        ProcStat procStatusItem;
                        bool success = processWorkQueue.TryDequeue(out procStatusItem);

                        // If got item from queue process it on our task number
                        if (success)
                        {
                            // If cancel pending don't start another process
                            if ((WT != null) && (WT.CancellationPending))
                                continue;

                            Interlocked.Increment(ref interlockedWorkerStarted);

                            RunProcess(procStatusItem);

                            Interlocked.Increment(ref interlockedWorkerDone);

                            if (WT != null)
                            {
                                string text = string.Format("Workers Done {0} of {1}", interlockedWorkerDone, procCount);
                                int pcrecsdone = (interlockedWorkerDone * 100) / (procCount);
                                LockedWT(pcrecsdone, text);
                            }
                        }
                    }

                    // No more items in queue, just end the Task
                },
                TaskCreationOptions.LongRunning);
            }


            // Worker Thread, Wait for all Tasks to finish, catch any exceptions
            try
            {
                while (!Task.WaitAll(processTasks, 1000))
                {
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("RunProcess: Error: {0}", ex.Message));
                Log.Error(string.Format("RunProcess: Error: {0}", ex.StackTrace));
                if (ex.InnerException != null)
                {
                    Log.Error(string.Format("RunProcess: Inner Error: {0}", ex.InnerException.Message));
                    Log.Error(string.Format("RunProcess: Inner Error: {0}", ex.InnerException.StackTrace));
                }
            }
            finally
            {
                // For each process status in the array Log status
                for (int i = 0; i < processStatus.Length; i++)
                {
                    ProcStat ps = processStatus[i];
                    string s = string.Format("Worker: {0}, RC: {1}, Time: {2}", ps.ProcNum, ps.ReturnCode, ps.RunTime);
                    Log.Info(s);
                }

            }
        }


        // Start and wait for completion of an external process on a Task thread
        private int RunProcess(ProcStat ps)
        {
            int code = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Process proc = new Process();


            // Build command line arguments
            string parm = string.Format("Close=Auto Num={0} Start={1} End={2} Batch={3} Run={4} TestProd={5} RefID={6}",
                                        ps.ProcNum.ToString().Trim(), ps.RecStart.Trim(), 
                                        ps.RecEnd.Trim(), ps.Batch.Trim(), ps.Run.Trim(),
                                        (UseTestDB) ? "TEST" : "PROD", refID.Trim());
            if (!string.IsNullOrEmpty(ps.LetterType))
                parm = parm + string.Format(", Letter={0}", ps.LetterType.Trim());

            LockedLog("I", string.Format("RunProcess {0}: Starting: {1}", ps.ProcNum, parm));

            // Start and wait for completion of External process
            try
            {
                ps.Started = true;

                // Configure the process using the StartInfo properties.
                proc.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "ProcessWorker.exe");
                proc.StartInfo.Arguments = parm;
                proc.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                //process.StartInfo.UseShellExecute = false;

                // Start the external process
                lock (LockProcessStart)
                {
                    proc.Start();

                    Thread.Sleep(3000);
                }

                // Wait here for the process to exit.
                proc.WaitForExit();

                // Retrieve the exit code
                code = proc.ExitCode;
                LockedLog("I", string.Format("ProcessWorker: {0}, Exit Code {1}", ps.ProcNum.ToString().Trim(), code));

                ps.ReturnCode = code;
                ps.Ended = true;
            }
            catch (Exception ex)
            {
                LockedLog("E", string.Format("RunProcess {0}: Error: {1}", ps.ProcNum, ex.Message));
                LockedLog("E", string.Format("RunProcess {0}: Error: {1}", ps.ProcNum, ex.StackTrace));
                if (ex.InnerException != null)
                {
                    LockedLog("E", string.Format("RunProcess {0}: Inner Error: {1}", ps.ProcNum, ex.InnerException.Message));
                    LockedLog("E", string.Format("RunProcess {0}: Inner Error: {1}", ps.ProcNum, ex.InnerException.StackTrace));
                }
            }
            finally
            {
                proc.Close();
                sw.Stop();
                ps.RunTime = sw.Elapsed.ToString(@"hh\:mm\:ss\.f");
                string text = string.Format("RunProcess {0}: Ending: Time: {1}", ps.ProcNum, sw.Elapsed.ToString(@"hh\:mm\:ss\.f"));
                LockedLog("I", text);
            }

            return (code);   
        }


        /// <summary>
        /// Serializes access to the Logging function as it is not Multi-thread safe
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        private void LockedLog(string level, string msg)
        {
            lock (LockLogging)
            {
                Log.Log(level, msg);
            }
        }


        /// <summary>
        /// Serializes access to the BackgroundWorker Object just in case :)
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="text"></param>
        private void LockedWT(int progress, string text)
        {
            if (WT == null)
                return;

            lock (LockWT)
            {
                WT.ReportProgressInvoke(progress, text);
            }
        }

    }
}
