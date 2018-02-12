using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Utilities;
using System.ComponentModel;
using System.Windows.Forms;



namespace ReportsApplication1
{
    class clsMove
    {
        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Log = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;

        private static string strrootfolder = "\\\\Cobmain\\usacms\\PODFO\\Output";
        private static string srtLPrint = "\\\\cobmaina\\L_Drive\\PRINT\\AUTO\\";
        private static string strMailDatUpload = "\\\\cobmaina\\production\\USA Programs\\MailDat\\Postal One! Repository Folders\\Mail";

        public static WorkerThread WT = null;

        // Record Statistics in AppStats Table
        private static AppStats stats = null;
        private static string appName = string.Empty;
        private static int recordID = 0;
        public static string refID = string.Empty;

        private static int TotalErrorCount = 0;
        private static List<string> ErrorMessages = null;
        private static Stopwatch totalruntime = null;



        /// <summary>
        /// Constructor
        /// </summary>
        static clsMove()
        {
            Conn = DbAccess.GetConnectionString();
            UseTestDB = DbAccess.UseTestDB;
            Conf = new ConfigTable(Conn);
            Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";
            Log = new Logging(Conn, "AppLog");
            Log.SourceBase = "MoveToProduction";

            strrootfolder = Conf.GetString("RootOutputFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output");
            srtLPrint = Conf.GetString("LPrintPath", "\\\\cobmaina\\L_Drive\\PRINT\\AUTO\\");
            strMailDatUpload = Conf.GetString("MailDatUpload", "\\\\cobmaina\\production\\USA Programs\\MailDat\\Postal One! Repository Folders\\Mail");
        }


        /// <summary>
        /// Move Files to Production
        /// </summary>
        /// <param name="batch">Batch number for the process</param>
        /// <param name="run">Run number for the process</param>
        public static void MoveToProduction(string batch, string run)
        {
            int mergedPDFsCount = 0;
            int postageFileCount = 0;
            int JobTicketeFileCount = 0;
            int MailDatFileCount = 0;
            string copyfilefrom = string.Empty;
            string copyfileto = string.Empty;
            string copyfiletype = string.Empty;
            int totalOutputFileCount = 0;
            int PDFOuptutFileCount = 0;
            int mailDatFileCount = 0;
            int postagefileOutCount = 0;
            int jobTicketOutCount = 0;

            totalruntime = new Stopwatch();
            totalruntime.Start();

            ErrorMessages = new List<string>();
            TotalErrorCount = 0;


            // Init AppStats info
            stats = new AppStats(DbAccess.GetConnectionString(), "AppStats");
            if (string.IsNullOrEmpty(refID))
                refID = string.Format("{0:yyyyMMddHHmmssff}", DateTime.Now);
            recordID = 0;
            appName = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);

            // Record Statistics in AppStats Table
            stats.SetDefaults = true;
            stats.SetField("RefID", refID);
            stats.SetField("AppName", (string.IsNullOrEmpty(appName) ? "ReportsApplication1" : appName));
            stats.SetField("AppSection", "MoveToProduction");
            stats.SetField("Batch", batch);
            stats.SetField("Run", run);
            stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
            int? ret = stats.InsertRecord();
            if (ret == null)
            {
                string s = string.Format("Error Inserting AppStats record: {0}", stats.Message);
                Log.Error(s);
                TotalErrorCount++;
                ErrorMessages.Add(s);
            }
            else
                recordID = (int)ret;
            try
            {
                if ((WT != null) && (WT.CancellationPending))
                {
                    Log.Error("MoveToProduction already canceled");
                    TotalErrorCount++;
                    stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                    stats.SetField("ErrorCount", TotalErrorCount);
                    stats.SetField("ErrorMessages", "MoveToProduction already canceled");
                    stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                    if (!stats.UpdateRecord())
                    {
                        Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                    }
                    return;
                }

                Log.Info(string.Format("MoveToProduction starting for Batch: {0}, Run: {1}", batch, run));
                string strpdffolder = strrootfolder + "\\MergedPDFs\\" + batch + run + "\\";
                string srtpostagefolder = strrootfolder + "\\Postage Statements";
                string srtJobTicketfolder = strrootfolder + "\\Job Tickets";
                string srtMailDatfolder = strrootfolder + "\\Mail-Dat\\" + batch + run + "\\";
                //handle each folder individually as each folder gets saved differently.
                // ------------------------------------------------------------
                // Get merged PDFs, copy to LPrint
                copyfiletype = "MergedPDFs";
                string[] pdffiles = Directory.GetFiles(strpdffolder);
                mergedPDFsCount = pdffiles.Length;

                string copyfile;
                for (int i = 0; i < pdffiles.Length; i++)
                {
                    copyfile = pdffiles[i].Remove(0, pdffiles[i].LastIndexOf("\\") + 1);
                    copyfilefrom = pdffiles[i];
                    copyfileto = srtLPrint + copyfile;
                    if (!File.Exists(srtLPrint + copyfile))
                    {
                        Log.Info(string.Format("Moving file for Batch: {0}, Run: {1},  File: {2}", batch, run, srtLPrint + copyfile));
                        File.Copy(pdffiles[i], srtLPrint + copyfile);
                    }
                    else
                    {
                        Log.Info(string.Format("File found, already in production. File: {2} Batch: {0}, Run: {1}", batch, run, srtLPrint + copyfile));
                    }
                    totalOutputFileCount++;
                    PDFOuptutFileCount++;
                }

                // ------------------------------------------------------------
                // Copy Postage Statements to LPrint
                copyfiletype = "PostageStatements";
                string[] postagefiles = Directory.GetFiles(srtpostagefolder);
                postageFileCount = postagefiles.Length;

                for (int i = 0; i < postagefiles.Length; i++)
                {
                    copyfile = postagefiles[i].Remove(0, postagefiles[i].LastIndexOf("\\") + 1);
                    if (copyfile == "PODFO_POSTAGE_" + batch + run + ".PDF")
                    {
                        copyfilefrom = postagefiles[i];
                        copyfileto = srtLPrint + copyfile;
                        if (!File.Exists(srtLPrint + copyfile))
                        {
                            Log.Info(string.Format("Moving file for Batch: {0}, Run: {1},  File: {2}", batch, run, srtLPrint + copyfile));
                            File.Copy(postagefiles[i], srtLPrint + copyfile);
                        }
                        else
                        {
                            Log.Info(string.Format("File found, already in production. File: {2} Batch: {0}, Run: {1}", batch, run, srtLPrint + copyfile));
                        }

                        totalOutputFileCount++;
                        postagefileOutCount++;
                        break;
                    }
                }

                // ------------------------------------------------------------
                // Copy Job Ticket to LPrint 
                copyfiletype = "JobTicket";
                string[] JobTicketfiles = Directory.GetFiles(srtJobTicketfolder);
                JobTicketeFileCount = JobTicketfiles.Length;

                for (int i = 0; i < JobTicketfiles.Length; i++)
                {
                    copyfile = JobTicketfiles[i].Remove(0, JobTicketfiles[i].LastIndexOf("\\") + 1);
                    if (copyfile == "PODFO-" + batch + run + ".pdf")
                    {
                        copyfilefrom = JobTicketfiles[i];
                        copyfileto = srtLPrint + copyfile;                        
                        if (!File.Exists(srtLPrint + copyfile))
                        {
                            Log.Info(string.Format("Moving file for Batch: {0}, Run: {1},  File: {2}", batch, run, srtLPrint + copyfile));
                            File.Copy(JobTicketfiles[i], srtLPrint + copyfile);
                        }
                        else
                        {
                            Log.Info(string.Format("File found, already in production. File: {2} Batch: {0}, Run: {1}", batch, run, srtLPrint + copyfile));
                        }

                        totalOutputFileCount++;
                        jobTicketOutCount++;
                        break;
                    }
                }

                // ------------------------------------------------------------
                // Copy Mail Dat files to Mail-Dat upload
                copyfiletype = "Mail-Dat";
                string[] MailDatfiles = Directory.GetFiles(srtMailDatfolder);
                MailDatFileCount = MailDatfiles.Length;

                for (int i = 0; i < MailDatfiles.Length; i++)
                {
                    copyfile = MailDatfiles[i].Remove(0, MailDatfiles[i].LastIndexOf("\\") + 1);
                    copyfilefrom = MailDatfiles[i];
                    copyfileto = strMailDatUpload + "\\" + copyfile;
                    File.Copy(MailDatfiles[i], strMailDatUpload + "\\" + copyfile);
                    totalOutputFileCount++;
                    mailDatFileCount++;
                }

            }
            catch (Exception ex)
            {
                // Format and log exception
                string s = string.Format("Error clsMove {0}: Batch: {1}  Run: {2} : {3}",
                                         copyfiletype, batch, run, ex.Message);
                Log.Error(s);
                Log.Error(string.Format("Error on Copy {0} from: '{1}' to: '{2}'", 
                                        copyfiletype, copyfilefrom, copyfileto));
                ErrorMessages.Add(s);
                TotalErrorCount++;

                // Format and log inner exception if any
                string inner = string.Empty;
                if (ex.InnerException != null)
                {
                    inner = string.Format("InnerEx.Message: {0}", ex.InnerException.Message);
                    Log.Error(inner);
                }

                // Send Email message of exception
                clsEmail.EmailMessage(string.Format("Error from PODFO Batch {0} run {1} {2}",
                                                    batch, run, (UseTestDB) ? " TESTING" : ""),
                                      string.Format("Error in clsMove.MoveToProduction: {0} \r\n" +
                                                    "Copy {1} from: '{2}' to: '{3}' \r\n {4}",
                                                    ex.Message, copyfiletype, copyfilefrom, copyfileto, inner));
            }
            finally
            {
                totalruntime.Stop();
                string s = string.Format("clsMove: MergedPDFs: {0} of {1}, PostageFiles: {2} of {3}, JobTicketFiles: {4} of {5}, " +
                                         "MailDatFiles: {6} of {7}, RunTime: {8}",
                                         PDFOuptutFileCount, mergedPDFsCount, postagefileOutCount, postageFileCount,
                                         jobTicketOutCount, JobTicketeFileCount, mailDatFileCount, MailDatFileCount,
                                         totalruntime.Elapsed.ToString(@"hh\:mm\:ss\.f"));
                Log.Info(s);
                // Limit Error messages string for DB
                string errorMessages = string.Empty;
                if (ErrorMessages.Count > 0)
                {
                    errorMessages = ErrorMessages.Aggregate((a, b) => a + ", " + b);
                    if (errorMessages.Length > 1023)
                        errorMessages = errorMessages.Substring(0, 1023);
                }
                // Record Statistics in AppStats Table
                Process procObj = Process.GetCurrentProcess();
                stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                stats.SetField("ErrorCount", TotalErrorCount);
                stats.SetField("ErrorMessages", errorMessages);
                stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                stats.SetField("AppNotes", s);
                stats.SetField("MaxMemUsedMB", (int)(procObj.PeakVirtualMemorySize64 / 1048576L));
                stats.SetField("InputCount1", mergedPDFsCount + postageFileCount + JobTicketeFileCount + MailDatFileCount);
                stats.SetField("OutputCount1", totalOutputFileCount);
                stats.SetField("OutputCount2", PDFOuptutFileCount);
                stats.SetField("ProcessTimeSecs1", totalruntime.Elapsed.TotalSeconds);
                stats.SetField("TotalRunTimeSecs", totalruntime.Elapsed.TotalSeconds);
                stats.SetField("AppCount3", mailDatFileCount);
                bool rcstats = stats.UpdateRecord();
                if (!rcstats)
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }

            }
        }

    }
}
