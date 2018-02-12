using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Threading;
using Utilities;
using ReportsApplication1;



namespace ProcessWorker
{
    public partial class ProcessWorker : Form
    {
        // Get connection string
        private string Conn = null;
        // Create DB Logger
        private Logging Log = null;
        // ConfigTable Access
        private ConfigTable Conf = null;

        // Use Test DB flag
        private bool UseTestDB = false;


        private int ExitCode = -1;

        private BackgroundWorker BGW = new BackgroundWorker();

        // Cmdline parms
        private string workerNumber = string.Empty;
        private string start = string.Empty;
        private string end = string.Empty;
        private string batch = string.Empty;
        private string run = string.Empty;
        private string letterType = string.Empty;
        private string testProd = string.Empty;
        private string refID = string.Empty;

        private bool autoclose = false;
        private bool autoCloseInvoked = false;

        private WorkerThread WT = new WorkerThread();


        public ProcessWorker()
        {
            InitializeComponent();
            //this.Shown += ProcessWorker_Shown;
        }


        // Write log msg string to file
        private void LogToFile(string strToWrite)
        {
            var s = DateTime.Now.ToString() + " " + strToWrite;
            var fname = string.Empty;

            string host = System.Environment.MachineName;
            //if (host.StartsWith("PROG-05") || host.StartsWith("sabertooth"))
            if (Directory.Exists("c:\\DeleteMe\\"))
                fname = "c:\\DeleteMe\\ProcessWorker.Critical.Log.txt";
            else
                fname = "c:\\Program Files\\ProcessWorker.Critical.Log.txt";


            
           // else
              //  fname = "c:\\Program Files\\ProcessWorker.Critical.Log.txt";

            using (StreamWriter writer = new StreamWriter(fname, true))
            {
                writer.WriteLine(strToWrite);
            }
        }


        // Form Load
        private void ProcessWorker_Load(object sender, EventArgs e)
        {
            this.Shown += ProcessWorker_Shown;

            try
            {
                LogToFile("ProcessWorker:ProcessWorker_Load Starting");

                Conn = DbAccess.GetConnectionString();
                UseTestDB = DbAccess.UseTestDB;

                Conf = new ConfigTable(Conn);
                Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";

                Log = new Logging(Conn, "AppLog");

                // Get cmdline parms
                ParseCommandLineArgs();
                Log.SourceBase = "ProcessWorker" + "." + workerNumber;
                Log.Info(Environment.CommandLine);

                // Show Version Number and EXE File date in Title Bar
                string Ver, BDate;
                Ver = Application.ProductVersion.ToString();
                BDate = File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
                this.Text = String.Format("ProcessWorker #{0} - Version:{1}   File Date: {2}", workerNumber, Ver, BDate);

                // Handle TEST / PROD mode
                string s = (testProd == "PROD") ? "False" : "True";
                ConfigurationManager.AppSettings.Set("UseTestDB", s);

                // Set connection string in Application settings
                Properties.Settings.Default["PODFOConnectionString"] = DbAccess.GetConnectionString();
                DbAccess.Close();

                // Setup Form Controls
                lblWorkerNum.Text = string.Format("# {0}", workerNumber);
                lblBatch.Text = batch;
                lblRun.Text = run;
                lblStart.Text = start;
                lblEnd.Text = end;
                lblTestProd.Text = testProd;

                // Setup background worker and start processing
                //BGW.WorkerReportsProgress = true;
                //BGW.DoWork += new DoWorkEventHandler(bw_DoWork);
                //BGW.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                //BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                //MainForm.BGW = BGW;

                // Setup worker threadand start processing
                WT.WorkerReportsProgress = true;
                WT.WorkerSupportsCancellation = true;
                WT.DoWork += wt_DoWork;
                WT.ProgressChanged += wt_ProgressChanged;
                WT.RunWorkerCompleted += wt_RunWorkerCompleted;
                MainForm.WT = WT;
                MainForm.refID = refID;

                // Setup clsGenerateLetters to know it's running from ProcessWorker so it doesn't recurse
                clsGenerateLetters.FromProcessWorker = true;
                clsGenerateLetters.ProcessWorkerNumber = workerNumber;

                // Start Background Thread
                //BGW.RunWorkerAsync();
                //WT.RunWorkerAsync();
                //WT.RunWorkerSync();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error in Form Load {0}", ex.Message));
                Log.Error(string.Format("Error in Form Load {0}", ex.StackTrace));
                if (ex.InnerException != null)
                {
                    Log.Error(string.Format("Error in Form Load {0}", ex.InnerException.Message));
                    Log.Error(string.Format("Error in Form Load {0}", ex.InnerException.StackTrace));
                }
            }
        }


        private void ProcessWorker_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            WT.RunWorkerSync();
        }


        // Form Closing, return status code
        private void ProcessWorker_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!autoCloseInvoked)
            //{
            //    DialogResult dr = MessageBox.Show("Are you sure you want to cancel?", "Are you Sure",
            //                                      MessageBoxButtons.YesNoCancel);
            //    if (dr != DialogResult.Yes)
            //        e.Cancel = true;
            //}
            Environment.ExitCode = ExitCode;
        }


        // Parse the command line arguments as key=value pairs
        private void ParseCommandLineArgs()
        {
            var sa = Environment.GetCommandLineArgs();

            // For each key=value pair, split and assign value 
            foreach (var a in sa)
            {
                var kv = a.Split('=');
                if (kv.Length == 2)
                {
                    switch (kv[0])
                    {
                        case "Num": workerNumber = kv[1]; break;
                        case "Batch": batch = kv[1]; break;
                        case "Run": run = kv[1]; break;
                        case "Start": start = kv[1]; break;
                        case "End": end = kv[1]; break;
                        case "Letter": letterType = kv[1]; break;
                        case "TestProd": testProd = kv[1]; break;
                        case "Close": if (kv[1] == "Auto") autoclose = true; break;
                        case "RefID": refID = kv[1]; break;
                    }
                }
            }
        }

        #region "BackgroundWorker Thread (Not used)"

        // "Backgroundworker" on it's thread
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            BGW.ReportProgress(-2, "Reports Application");

            // Start the ReportsApplication on the background worker thread
            var form = new ReportsApplication1.MainForm(batch, run, true, false, Convert.ToInt32(start), Convert.ToInt32(end));
        }


        // Background worker Progress Event on UI thread
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string msg = (string) e.UserState;

            if (e.ProgressPercentage >= 0)
            {
                //pbProgress.Value = e.ProgressPercentage;
                //lblPercent.Text = String.Format("{0}%", e.ProgressPercentage);
            }

            //if (!string.IsNullOrEmpty(msg))
            //{
            //    switch (e.ProgressPercentage)
            //    {
            //        case -1: lblStatus.Text = msg; break;
            //        case -2: lblStep.Text = msg; break;
            //        case -3: lblRPM.Text = msg; break;
            //        default: lblStatus.Text = msg; break;
            //    }
            //}
        }


        // Background worker event on UI thread
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                lblStatus.Text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                lblStatus.Text = ("Error: " + e.Error.Message);
                Log.Error("Error: " + e.Error.Message);
            }

            else
            {
                lblStatus.Text = "Done!";
            }

            if (autoclose)
            {
                autoCloseInvoked = true;
                Close();
            }
        }

        #endregion


        // "WorkerThread" on it's thread
        private void wt_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerThread worker = sender as WorkerThread;

            //BGW.ReportProgress(-2, "Reports Application");
            WT.ReportProgress(-2, "Reports Application");
            Application.DoEvents();

            // Start the ReportsApplication on the background worker thread
            var form = new ReportsApplication1.MainForm(batch, run, true, false, Convert.ToInt32(start), Convert.ToInt32(end));
        }


        // WorkerThread Progress Event on worker thread
        private void wt_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string msg = (string)e.UserState;

            if (e.ProgressPercentage >= 0)
            {
                if (pbProgress.InvokeRequired)
                    pbProgress.BeginInvoke(new Action(() => pbProgress.Value = e.ProgressPercentage));
                else
                    pbProgress.Value = e.ProgressPercentage;
                if (lblPercent.InvokeRequired)
                    lblPercent.BeginInvoke(new Action(() => lblPercent.Text = String.Format("{0}%", e.ProgressPercentage)));
                else
                    lblPercent.Text = String.Format("{0}%", e.ProgressPercentage);
                Application.DoEvents();
            }

            if (!string.IsNullOrEmpty(msg))
            {
                switch (e.ProgressPercentage)
                {
                    case -1:
                        if (lblStatus.InvokeRequired)
                            lblStatus.BeginInvoke(new Action(() => lblStatus.Text = msg));
                        else
                            lblStatus.Text = msg;
                        break;
                    case -2:
                        if (lblStep.InvokeRequired)
                            lblStep.BeginInvoke(new Action(() => lblStep.Text = msg));
                        else
                            lblStep.Text = msg;
                        break;
                    case -3:
                        if (lblRPM.InvokeRequired)
                            lblRPM.BeginInvoke(new Action(() => lblRPM.Text = msg));
                        else
                            lblRPM.Text = msg;
                        break;
                    default:
                        if (lblStatus.InvokeRequired)
                            lblStatus.BeginInvoke(new Action(() => lblStatus.Text = msg));
                        else
                            lblStatus.Text = msg;
                        break;
                }
                Application.DoEvents();
            }
        }


        // Worker Thread event on worker thread
        private void wt_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                if (lblStatus.InvokeRequired)
                    lblStatus.BeginInvoke(new Action(() => lblStatus.Text = "Canceled!"));
                else
                    lblStatus.Text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                if (lblStatus.InvokeRequired)
                    lblStatus.BeginInvoke(new Action(() => lblStatus.Text = "Error: " + e.Error.Message));
                else
                    lblStatus.Text = "Error: " + e.Error.Message;
                Log.Error("Error: " + e.Error.Message);
            }

            else
            {
                if (lblStatus.InvokeRequired)
                    lblStatus.BeginInvoke(new Action(() => lblStatus.Text = "Done!"));
                else
                    lblStatus.Text = "Done!";
            }
            Application.DoEvents();


            if (autoclose)
            {
                autoCloseInvoked = true;
                //lblStatus.BeginInvoke(new Action(() => lblStatus.Text = "Canceled!"));

                this.BeginInvoke(new Action(() => this.Close()));
            }
        }


    }
}
