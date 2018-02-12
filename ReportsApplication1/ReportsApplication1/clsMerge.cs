using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Data.SqlClient;
using ceTe;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.Merger;
using Utilities;


namespace ReportsApplication1
{
    class clsMerge
    {
        private static string strFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs";
        private static string strMergeFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\MergedPDFs";
        private static string ceTeLicense = "DPS70NEDJGMGEGWKOnLLQb4SjhbTTJhXnkpf9bj8ZzxFH+FFxctoPX+HThGxkpidUCHJ5b88fg4oUJSHiRBggzHdghUgkkuIvoag";

        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Log = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;

        public static bool BGWCanceled = false;
        public static WorkerThread WT = null;

        //private static int PagesPerMerge = 3000;
        private static int FilesPerMerge = 2500;

        // Record Statistics in AppStats Table
        private static AppStats stats = null;
        private static string appName = string.Empty;
        private static int recordID = 0;
        public static string refID = string.Empty;

        private static int TotalErrorCount = 0;
        private static List<string> ErrorMessages = null;

        private static Stopwatch totalruntime = null;
        private static Stopwatch loadtime = null;
        private static Stopwatch mergetime = null;
        private static Stopwatch PDFtime = null;



        /// <summary>
        /// Constructor
        /// </summary>
        static clsMerge()
        {
            Conn = DbAccess.GetConnectionString();
            UseTestDB = DbAccess.UseTestDB;
            Conf = new ConfigTable(Conn);
            Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";
            Log = new Logging(Conn, "AppLog");
            Log.SourceBase = "MergePDFs";

            strFilePath = Conf.GetString("mstrFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs");
            strMergeFilePath = Conf.GetString("mstrMergeFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\MergedPDFs");
            ceTeLicense = Conf.GetString("ceTeLicense", "DPS70NEDJGMGEGWKOnLLQb4SjhbTTJhXnkpf9bj8ZzxFH+FFxctoPX+HThGxkpidUCHJ5b88fg4oUJSHiRBggzHdghUgkkuIvoag");
            //PagesPerMerge = Conf.GetInt("PagesPerMerge", 3000);
            FilesPerMerge = Conf.GetInt("FilesPerMerge", 2500);
        }

     
        /// <summary>
        /// Merge the individual PDFs
        /// </summary>
        /// <param name="batch">Batch number for this process</param>
        /// <param name="run">Run number for this process</param>
        public static void MergePDFs(string batch, string run)
        {
            int intcount = 0;
            int intEnd = 0;
            int intStart = 1;
            int introwcount = 0;
            int RecsDone = 0;
            //DateTime lastRPMTime = new DateTime();
            int RPMCount = 0;
            int filesCountThisMerge = 0;

            int intDBpagecount = 0;
            int intPDFpagecount = 0;
            totalruntime = new Stopwatch();
            totalruntime.Start();
            int fileNotFoundCount = 0;
            int RecordsOuput = 0;
            loadtime = new Stopwatch();
            mergetime = new Stopwatch();
            PDFtime = new Stopwatch();
            ErrorMessages = new List<string>();


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
            stats.SetField("AppSection", "MergePDFs");
            stats.SetField("Batch", batch);
            stats.SetField("Run", run);
            stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
            stats.SetField("AppCount3", FilesPerMerge);
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



            if ((WT != null) && (WT.CancellationPending))
            {
                Log.Error("MergePDFs already canceled");
                TotalErrorCount++;
                stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                stats.SetField("ErrorCount", TotalErrorCount);
                stats.SetField("ErrorMessages", "RunAutomated already canceled");
                stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                if (!stats.UpdateRecord())
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }
                return;
            }

            if (WT != null)
            {
                WT.ReportProgress(-2, String.Format("Merge PDFs"));
                WT.ReportProgress(-3, string.Format("R/M: {0:#.##}", 0.0));
            }


            try
            {
                Log.Info(string.Format("MergePDFs starting for Batch: {0}, Run: {1}, FilesPerMerge: {2}",
                                       batch, run, FilesPerMerge));

                var log = new clsLog();
                log.mstrLogFileLocation = batch + run + ".txt";

                loadtime.Start();
                var DA = new clsGetBatchSort();
                SqlDataAdapter adapter = new SqlDataAdapter(DA.dsBatch_Sort(batch, run));
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                introwcount = ds.Tables[0].Rows.Count;
                loadtime.Stop();


                //int LetterTypeID = 0;
                if (WT != null)
                    WT.ReportProgress(0, string.Format("Collecting page count"));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intDBpagecount += Convert.ToInt32(dr["PageCount"]);
                }


                ceTe.DynamicPDF.Document.AddLicense(ceTeLicense);
                //Dim intRecordCount As Integer = 1
                //Dim rootValue As String
                string mergePath = strMergeFilePath + "\\" + batch + run;

                if (!Directory.Exists(mergePath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(mergePath);
                }



                string[] fileEntries = Directory.GetFiles(strFilePath + "\\" + batch + run);
                string fmt = "00000000";
                //string filename = Path.Combine("C:\\PDF\\", "report" + PreSortID.ToString(fmt) + ".pdf");
                //string filename = Path.Combine(mstrFilePath + "\\" + batch + run + "\\" + PreSortID.ToString(fmt) + ".pdf");
                string filename;

                ceTe.DynamicPDF.Merger.MergeDocument myTempDoc = new ceTe.DynamicPDF.Merger.MergeDocument();
                string missingfiles = "";

                mergetime.Start();
                if (introwcount != 0) //make sure files where imported
                {

                    if (fileEntries.Length == introwcount) //make sure file count matches pdf count
                    {
                        for (int i = 1; i < fileEntries.Length + 1; i++)
                        {
                            filename = strFilePath + "\\" + batch + run + "\\" + i.ToString(fmt) + ".PDF";

                            if (fileEntries[i - 1].ToString().Equals(filename))
                            {
                                intcount += 1;
                                intEnd += 1;

                                // Merge file
                                myTempDoc.Append(filename);
                                filesCountThisMerge++;

                                //shut pdf every "n" files to avoid errors
                                //if ((myTempDoc.Pages.Count >= PagesPerMerge))
                                if (filesCountThisMerge >= FilesPerMerge)
                                    {
                                    PDFtime.Start();
                                    log.WriteToLogfile("about to combine myTempDoc " + myTempDoc.Pages.Count + " Pages");
                                    myTempDoc.CompressionLevel = 0;
                                    myTempDoc.PdfVersion = ceTe.DynamicPDF.PdfVersion.v1_3;
                                    intPDFpagecount += myTempDoc.Pages.Count;
                                    //creates pdf
                                    log.WriteToLogfile("myTempDoc.Draw(" + mergePath + "\\PODFO " + batch + run + " " + intStart + "-" + intEnd + ".Pdf)");
                                    myTempDoc.Draw(mergePath + "\\PODFO " + batch + run + " " + intStart + "-" + intEnd + ".Pdf");
                                    myTempDoc = null;
                                    GC.Collect();
                                    intStart = intEnd + 1;
                                    intcount = 0;
                                    myTempDoc = new ceTe.DynamicPDF.Merger.MergeDocument();

                                    log.WriteToLogfile("Finished Drawing");
                                    Log.Info("myTempDoc.Draw(" + mergePath + "\\PODFO " + batch + run + " " + intStart + "-" + intEnd + ".Pdf)");
                                    RecordsOuput++;
                                    PDFtime.Stop();
                                    filesCountThisMerge = 0;
                                }

                            }
                            else
                            {
                                missingfiles = missingfiles + ", " + i;
                                log.WriteToLogfile("missing file: " + i.ToString());
                                // MessageBox.Show("missing file: " + i.ToString());
                                fileNotFoundCount++;
                            }

                            RecsDone++;
                            RPMCount++;

                            // Report Background worker thread status
                            if (WT != null)
                            {
                                // Check if cancel pending, set canceled flag
                                if (WT.CancellationPending)
                                {
                                    BGWCanceled = true;
                                    break;
                                }

                                // Report Progress % and count of recs processed
                                int pcrecsdone = (RecsDone * 100) / (introwcount);
                                if (intcount % 20 == 0)
                                {
                                    WT.ReportProgress(pcrecsdone, string.Format("Recs Done {0} of {1}", RecsDone, introwcount));

                                    // Report records per minute
                                    //var rpm = RPMCount / (DateTime.Now - lastRPMTime).TotalMinutes;
                                    //BGW.ReportProgress(-3, string.Format("R/M: {0:#.##}", rpm));
                                    //lastRPMTime = DateTime.Now;
                                    //RPMCount = 0;
                                }
                            }
                        }

                        // If merge in progress, write out last merge file
                        //if ((myTempDoc.Pages.Count != PagesPerMerge))
                        if (filesCountThisMerge != FilesPerMerge)
                        {
                            PDFtime.Start();
                            log.WriteToLogfile("about to combine myTempDoc " + myTempDoc.Pages.Count + " Pages");
                            log.WriteToLogfile("myTempDoc.Draw(" + mergePath + "\\PODFO " + batch + run + " " + intStart + "-" + intEnd + ".Pdf)");
                            myTempDoc.CompressionLevel = 0;
                            myTempDoc.PdfVersion = ceTe.DynamicPDF.PdfVersion.v1_3;
                            intPDFpagecount += myTempDoc.Pages.Count;
                            //creates pdf
                            myTempDoc.Draw(mergePath + "\\PODFO " + batch + run + " " + intStart + "-" + intEnd + ".Pdf");
                            myTempDoc = null;
                            GC.Collect();
                            log.WriteToLogfile("Finished Drawing");
                            Log.Info("myTempDoc.Draw(" + mergePath + "\\PODFO " + batch + run + " " + intStart + "-" + intEnd + ".Pdf)");
                            RecordsOuput++;
                            PDFtime.Stop();
                            filesCountThisMerge = 0;
                        }

                        if (WT != null)
                            WT.ReportProgress(100, string.Format("Recs Done {0} of {1}", intEnd, introwcount));
                    }
                    else
                    {
                        //email
                        string s = string.Format("clsMerge.MergePDFs: Missing file fileEntries.Length = {0} and introwcount = {1}",
                                                 fileEntries.Length, introwcount);
                        Log.Info(s);
                        log.WriteToLogfile("Missing file fileEntries.Length = " + fileEntries.Length + " and introwcount = " + introwcount);
                        // MessageBox.Show("Missing file fileEntries.Length = " + fileEntries.Length + " and introwcount = " + introwcount);

                        if (WT != null)
                            WT.ReportProgress(100, string.Format("Record counts don't match: Files: {0},  DB Rows: {1}",
                                                                  fileEntries.Length, introwcount));
                    }

                }
                else
                    Log.Info("MergePDFs: No files to process");

            }
            catch (Exception ex)
            {
                TotalErrorCount++;
                var log = new clsLog();
                log.mstrLogFileLocation = batch + run + ".txt";
                log.WriteToLogfile("clsMerge.MergePDFs " + ex.Message + " Stack trace " + ex.StackTrace + " Inner ex " + ex.InnerException);
                clsEmail.EmailMessage(string.Format("PODFO error {0}", (UseTestDB) ? " TESTING" : ""), 
                                      "Error merging pdfs. Message: " + ex.Message + " The pdf count was " + intEnd);
                //  MessageBox.Show("Error merging pdfs. Message: " + ex.Message + " The pdf count was " + intEnd);

                string s = string.Format("clsMerge.MergePDFs Exception: {0}", ex.Message);
                Log.Error(s);
                ErrorMessages.Add(s);
                s = string.Format("clsMerge.MergePDFs Exception Inner: {0}", ex.InnerException);
                Log.Error(s);
                s = string.Format("clsMerge.MergePDFs Exception Stack: {0}", ex.StackTrace);
                Log.Error(s);
            }
            finally
            {
                totalruntime.Stop();
                mergetime.Stop();
                string s = string.Format("clsMerge.MergePDFs: Rowount: {0}, DBPageCount: {1}, Time: {2}",
                                         introwcount, intDBpagecount, totalruntime.Elapsed.ToString(@"hh\:mm\:ss\.f"));
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
                //stats.SetField("ExpectedCountOut", RecordsCount);
                stats.SetField("InputCount1", RecsDone);
                stats.SetField("OutputCount1", RecordsOuput);
                stats.SetField("OutputCount2", intDBpagecount);
                stats.SetField("InputTimeSecs1", loadtime.Elapsed.TotalSeconds);
                stats.SetField("ProcessTimeSecs1", mergetime.Elapsed.TotalSeconds);
                stats.SetField("OutputTimeSecs1", PDFtime.Elapsed.TotalSeconds);
                stats.SetField("TotalRunTimeSecs", totalruntime.Elapsed.TotalSeconds);
                stats.SetField("AppCount2", fileNotFoundCount);
                bool rcstats = stats.UpdateRecord();
                if (!rcstats)
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }

            }
        }
    }
}
