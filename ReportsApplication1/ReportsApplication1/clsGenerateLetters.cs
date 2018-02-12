using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;
using System.Data.SqlClient;
using ceTe;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using ceTe.DynamicPDF.Merger;
using Utilities;
using System.Diagnostics;
using System.Linq;
using System.Threading;


namespace ReportsApplication1
{
    public class clsGenerateLetters
    {
        public static WorkerThread WT = null;
        public static bool FromProcessWorker = false;
        public static string ProcessWorkerNumber = string.Empty;


        public static int RecordsCount = 0;
        public static int RecordsDone = 0;

        // Report Progress every "n" records
        private static int ReportProgressCount = 10;

        // Report Time To Go every "m" records
        private static int ReportTTGCount = 100;

        // Stopwatch for saving PDF files in the report processing
        private static Stopwatch pdftime = null;

        private static bool UseExternalProcess = true;

        private static int retryCount = 20;
        private static int retryDelay = 10000;


        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Log = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Record Statistics in AppStats Table
        private static AppStats stats = null;
        private static string appName = string.Empty;
        private static int recordID = 0;
        public static string refID = string.Empty;

        // Use Test DB flag
        private static bool UseTestDB = false;

        private static string mstrFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs";
        //private const string mstrMergeFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\MergedPDFs";
      
        // public ReportViewer viewer = new ReportViewer();

        private static Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource();
        //  private static  Enums.LetterTypes LetterTypes = new Enums.LetterTypes();
        private static Dictionary<int,string> rptFileName = new Dictionary<int,string>()
        { 
            {1,"ReportsApplication1.DIS.DISDMA_MAENG.rdlc"},
            {2,"ReportsApplication1.DIS.DISDMA_MASPA.rdlc"},
            {3,"ReportsApplication1.DIS.DISDPD_PDAENG.rdlc"},
            {4,"ReportsApplication1.DIS.DISDPD_PDASPA.rdlc"},
            {7,"ReportsApplication1.DIS.DISOPT_PDPENG.rdlc"},
            {8,"ReportsApplication1.DIS.DISOPT_PDPSPA.rdlc"},
            {9,"ReportsApplication1.ENT.ENTENNGD.rdlc"},
            {10,"ReportsApplication1.ENT.ENTENMBP.rdlc"},
            {11,"ReportsApplication1.ACO.ACOINAENG.rdlc"},
            {12,"ReportsApplication1.ACO.ACOINASP.rdlc"},
            {15,"ReportsApplication1.ACO.ACOINRSAENG.rdlc"},
            {16,"ReportsApplication1.ACO.ACOINRSASPA.rdlc"},
            {17,"ReportsApplication1.ACO.ACOINUENG.rdlc"},
            {18,"ReportsApplication1.ACO.ACOINUSPA.rdlc"},
            {19,"ReportsApplication1.ACO.ACOOUTAENG.rdlc"},
            {20,"ReportsApplication1.ACO.ACOOUTASPA.rdlc"},
            {21,"ReportsApplication1.ACO.ACOOUTUENG.rdlc"},
            {22,"ReportsApplication1.ACO.ACOOUTUSPA.rdlc"},
            {23,"ReportsApplication1.CPC.CPCOUTENG.rdlc"},
            {24,"ReportsApplication1.CPC.CPCOUTSPA.rdlc"},
            {25,"ReportsApplication1.CPC.CPCOUTRENG.rdlc"},
            {26,"ReportsApplication1.CPC.CPCOUTRSPA.rdlc"},
            {29,"ReportsApplication1.MBP.MBP_DA_ENG.rdlc"},
            {30,"ReportsApplication1.MBP.MBP_DA_SPAN.rdlc"},
            {31,"ReportsApplication1.MBP.MBP_RAEP_ENG.rdlc"},
            {32,"ReportsApplication1.MBP.MBP_RAEP_SPAN.rdlc"},
            {33,"ReportsApplication1.MBP.MBP_RAWE_ENG.rdlc"},
            {34,"ReportsApplication1.MBP.MBP_RAWE_SPAN.rdlc"},
            {35,"ReportsApplication1.MBP.MBP_AE_ENG.rdlc"},
            {36,"ReportsApplication1.MBP.MBP_AE_SPAN.rdlc"},
            {37,"ReportsApplication1.MBP.MBP_NE_ENG.rdlc"},
            {39,"ReportsApplication1.MBP.MBP_RS_ENG.rdlc"},
            {40,"ReportsApplication1.MBP.MBP_RS_SPAN.rdlc"},
            {41,"ReportsApplication1.MBP.MBP_PREP_ENG.rdlc"},
            {42,"ReportsApplication1.MBP.MBP_PREP_SPAN.rdlc"},
            {43,"ReportsApplication1.MBP.MBP_PRWE_ENG.rdlc"},
            {44,"ReportsApplication1.MBP.MBP_PRWE_SPAN.rdlc"},
            {10031,"ReportsApplication1.ENT.ENTENNGD_SPAN.rdlc"},
            {10032,"ReportsApplication1.ENT.ENTENMBP_SPAN.rdlc"},
            {10033,"ReportsApplication1.ACO.ACOINSAENG.rdlc"},
              {10034,"ReportsApplication1.ACO.ACOINSASPA.rdlc"}
        };
    //// Array of Report file names by letter type
    //private static string[] rptFileName = 
    //                                  {  /*  0 */ String.Empty,
    //                                     /*  1 */ "ReportsApplication1.DIS.DISDMA_MAENG.rdlc",
    //                                     /*  2 */ "ReportsApplication1.DIS.DISDMA_MASPA.rdlc",
    //                                     /*  3 */ "ReportsApplication1.DIS.DISDPD_PDAENG.rdlc",
    //                                     /*  4 */ "ReportsApplication1.DIS.DISDPD_PDASPA.rdlc",
    //                                     /*  5 */ String.Empty,
    //                                     /*  6 */ String.Empty,
    //                                     /*  7 */ "ReportsApplication1.DIS.DISOPT_PDPENG.rdlc",
    //                                     /*  8 */ "ReportsApplication1.DIS.DISOPT_PDPSPA.rdlc",
    //                                     /*  9 */ "ReportsApplication1.ENT.ENTENNGD.rdlc",
    //                                     /* 10 */ "ReportsApplication1.ENT.ENTENMBP.rdlc",
    //                                     /* 11 */ "ReportsApplication1.ACO.ACOINAENG.rdlc",
    //                                     /* 12 */ "ReportsApplication1.ACO.ACOINASP.rdlc",
    //                                     /* 13 */ String.Empty,
    //                                     /* 14 */ String.Empty,
    //                                     /* 15 */ "ReportsApplication1.ACO.ACOINRSAENG.rdlc",
    //                                     /* 16 */ "ReportsApplication1.ACO.ACOINRSASPA.rdlc",
    //                                     /* 17 */ "ReportsApplication1.ACO.ACOINUENG.rdlc",
    //                                     /* 18 */ "ReportsApplication1.ACO.ACOINUSPA.rdlc",
    //                                     /* 19 */ "ReportsApplication1.ACO.ACOOUTAENG.rdlc",
    //                                     /* 20 */ "ReportsApplication1.ACO.ACOOUTASPA.rdlc",
    //                                     /* 21 */ "ReportsApplication1.ACO.ACOOUTUENG.rdlc",
    //                                     /* 22 */ "ReportsApplication1.ACO.ACOOUTUSPA.rdlc",
    //                                     /* 23 */ "ReportsApplication1.CPC.CPCOUTENG.rdlc",
    //                                     /* 24 */ "ReportsApplication1.CPC.CPCOUTSPA.rdlc",
    //                                     /* 25 */ "ReportsApplication1.CPC.CPCOUTRENG.rdlc",
    //                                     /* 26 */ "ReportsApplication1.CPC.CPCOUTRSPA.rdlc",
    //                                     /* 27 */ String.Empty,
    //                                     /* 28 */ String.Empty,
    //                                     /* 29 */ "ReportsApplication1.MBP.MBP_DA_ENG.rdlc",
    //                                     /* 30 */ String.Empty,
    //                                     /* 31 */ "ReportsApplication1.MBP.MBP_RAEP_ENG.rdlc",
    //                                     /* 32 */ String.Empty,
    //                                     /* 33 */ "ReportsApplication1.MBP.MBP_RAWE_ENG.rdlc",
    //                                     /* 34 */ String.Empty,
    //                                     /* 35 */ "ReportsApplication1.MBP.MBP_AE_ENG.rdlc",
    //                                     /* 36 */ String.Empty,
    //                                     /* 37 */ "ReportsApplication1.MBP.MBP_NE_ENG.rdlc",
    //                                     /* 38 */ String.Empty,
    //                                     /* 39 */ "ReportsApplication1.MBP.MBP_RS_ENG.rdlc",
    //                                     /* 40 */ "ReportsApplication1.MBP.MBP_RS_SPAN.rdlc",
    //                                     /* 41 */ "ReportsApplication1.MBP.MBP_PREP_ENG.rdlc",
    //                                     /* 42 */ String.Empty,
    //                                     /* 43 */ "ReportsApplication1.MBP.MBP_PRWE_ENG.rdlc",
    //         /* 10031 */  "ReportsApplication1.ENT.ENTENNGD_SPAN.rdlc",
    //         /* 10032 */ "ReportsApplication1.ENT.ENTENMBP_SPAN.rdlc",
    //                                };



        // Record counts, one per letter type
        //private static int[] counts = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //                               0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        //                               0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        private static Dictionary<int, int> counts = new Dictionary<int, int>()
        {
            {1,0}, {2,0}, {3,0}, {4,0}, {7,0}, {8,0}, {9,0}, {10,0}, {11,0}, {12,0}, {15,0},{16,0},{17,0},
            {18,0},{19,0},{20,0},{21,0},{22,0},{23,0},{24,0},{25,0},{26,0},{29,0},{30,0},{31,0},{32,0},{33,0},{34,0},{35,0},{36,0},
            {37,0},{39,0},{40,0},{41,0},{42,0},{43,0},{44,0},{10031,0},{10032,0},{10033,0},{10034,0}
        };

        private static int RecoveredTimeoutErrorRecCount = 0;
        private static int TotalErrorCount = 0;
        private static List<string> ErrorMessages = null;


        /// <summary>
        /// Constructor
        /// </summary>
        static clsGenerateLetters()
        {
            Conn = DbAccess.GetConnectionString();
            UseTestDB = DbAccess.UseTestDB;

            Log = new Logging(Conn, "AppLog");
            Log.SourceBase = string.Format("GenerateIndividualPDFs {0}", ProcessWorkerNumber);

            Conf = new ConfigTable(Conn);
            Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";

            mstrFilePath = Conf.GetString("mstrFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs");
            retryCount = Conf.GetInt("RetryCount", 20);
            retryDelay = Conf.GetInt("RetryDelay", 10000);
            //ReportViewerResetCount = Conf.GetInt("ReportViewerResetCount", 500);
            ReportProgressCount = Conf.GetInt("ReportProgressCount", 10);
            UseExternalProcess = Conf.GetBool("UseExternalProcess", true);

            pdftime = new Stopwatch();
        }



        /// <summary>
        /// Generate Individual PDFs
        /// </summary>
        /// <param name="batch">Batch number</param>
        /// <param name="run">Run number</param>
        /// <param name="DSpodfo">DataSet for PODFO</param>
        /// <param name="Ta">Table Adapter for data</param>
        /// <param name="bs">BindingSource</param>
        /// <param name="intStart">Range Start</param>
        /// <param name="intEnd">Range End</param>
        /// <param name="strLetterType">Letter Type</param>
        public static void GenerateIndividualPDFs(string batch, string run, PODFODataSet1 DSpodfo, 
                                                  PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter Ta,
                                                  BindingSource bs, int intStart = 0, int intEnd = 0, string strLetterType = "")
        {
            Stopwatch loadtime = new Stopwatch();
            Stopwatch rpttime = new Stopwatch();
            Stopwatch totalruntime = new Stopwatch();

            DataTable dtMailInfo = null;

            pdftime.Reset();
            totalruntime.Start();

            RecoveredTimeoutErrorRecCount = 0;
            TotalErrorCount = 0;
            ErrorMessages = new List<string>();

            string rptName = string.Empty;
            int MPresortID = 0;
            int desiredCount = 0;
            string appnotes = string.Empty;

            //int TimeoutErrorRecCount = 0;
            int countOfLetterIdIsTooBig = 0;
            Log.SourceBase = string.Format("GenerateIndividualPDFs {0}", ProcessWorkerNumber);

            // Init AppStats info
            stats = new AppStats(DbAccess.GetConnectionString(), "AppStats");
            if (string.IsNullOrEmpty(refID))
                refID = string.Format("{0:yyyyMMddHHmmssff}", DateTime.Now);
            recordID = 0;
            appName = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);


            // Record Statistics in AppStats Table
            stats.SetDefaults = true;
            stats.SetField("RefID", refID);
            stats.SetField("AppName", (string.IsNullOrEmpty(appName)?"ReportsApplication1":appName));
            stats.SetField("AppSection", "GenerateIndividualPDFs");
            stats.SetField("SubAppNumber", ProcessWorkerNumber);
            stats.SetField("Batch", batch);
            stats.SetField("Run", run);
            stats.SetField("RangeStart", intStart);
            stats.SetField("RangeEnd", intEnd);
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


            reportDataSource.Name = "DataSet1";
            reportDataSource.Value = bs;
            int count;
            //SqlDataAdapter adapter;
           
            var batchinfo = new clsBatchInfo();
          
            ReportViewer report = new ReportViewer();

            report.LocalReport.DataSources.Add(reportDataSource);


            // Check for Cancel request already done
            if ((WT != null) && (WT.CancellationPending))
            {
                Log.Error("RunAutomated already canceled");
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

            // Make folder for PDFs if it doesn't exist
            if (!Directory.Exists(mstrFilePath + "\\" + batch + run))
            {
                DirectoryInfo di = Directory.CreateDirectory(mstrFilePath + "\\" + batch + run);
            }


            // Report Progress % and Loading list of Letters for Batch/Run
            if (WT != null)
            {
                WT.ReportProgress(0, string.Format("Loading List of Letters for Batch/Run"));
            }


            // Load the Mail Info records for the range of letters for the Batch / Run
            // Can cause Timeouts from SQL
            loadtime.Start();
            Exception lastex = null;

            bool rc = GetDataForLettersWithRetry(batch, run, intStart, intEnd, strLetterType, out dtMailInfo, out lastex);
            if (!rc || dtMailInfo == null)
            {
                string s = string.Format("Error SQL exceded retries loading Batch/Run letter list: {0}", lastex.Message);
                Log.Error(s);
                TotalErrorCount++;
                ErrorMessages.Add(s);

                // Show Error
                if (WT != null)
                    WT.ReportProgress(-1, s);

                clsEmail.EmailMessage(string.Format("Error from PODFO.MakePDFs {0}", (UseTestDB) ? "TESTING" : ""), s);

                stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                stats.SetField("ErrorCount", TotalErrorCount);
                stats.SetField("ErrorMessages", s);
                stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                if (!stats.UpdateRecord())
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }
                return;
            }
            count = dtMailInfo.Rows.Count;
            loadtime.Stop();


            int i = 0;
            int LetterTypeID = 0;

            report.ProcessingMode = ProcessingMode.Local;


            // Setup for instrumenting code
            DateTime lastRPMTime = new DateTime();
            int RPMCount = 0;
            //int TimeoutErrorRecCount = 0;
            DateTime lastTTGTime = new DateTime();
            int TTGCount = 0;
            string TTGStr = string.Empty;
            //count = ds.Tables[0].Rows.Count;
            RecordsCount = count;
            RecordsDone = 0;
            lastRPMTime = DateTime.Now;
            lastTTGTime = DateTime.Now;
            RPMCount = 0;

            // check for letters by Letter type and UseExternalProcess, if so ignore UseExternalProcess
            if ((!string.IsNullOrEmpty(strLetterType)) && UseExternalProcess)
            {
                Log.Error(string.Format("Generate individual letters by Letter type and UseExternalProcess " +
                                        "are not compatible, ignoring UseExternalProcess"));
                UseExternalProcess = false;
            }


            // If using External Process Worker call it and return from here
            if (!FromProcessWorker && UseExternalProcess)
            {
                clsManageMTProcess mp = new clsManageMTProcess(WT);
                //mp.MTProcGenerateIndividualPDFs(batch, run, strLetterType, ds.Tables[0], refID);
                mp.MTProcGenerateIndividualPDFs(batch, run, strLetterType, dtMailInfo, refID);

                // Count PDF files created
                var TotalPDFCount = Directory.EnumerateFiles(mstrFilePath + "\\" + batch + run, "*.PDF").Where(x => x.ToUpper().Contains(".PDF")).Count();
                Log.Info(string.Format("GenerateIndividualPDFs: Total PDFs: {0}", TotalPDFCount));

                string sql = "SELECT count(*) AS 'Count' FROM [PODFODEV2].[dbo].[PODMailingInfo] " +
                             "WHERE PODBatchID = @Batch AND PODRunID = @Run AND MPresortID > 0 AND PODMasterLetterTypeId < 44";
                SqlParameter[] sp = new SqlParameter[2];
                sp[0] = new SqlParameter("@Batch", batch);
                sp[1] = new SqlParameter("@Run", run);
                desiredCount = DbAccess.ExecuteScalar(sql, CommandType.Text, sp);
                Log.Info(string.Format("GenerateIndividualPDFs: Desired PDF Count: {0}", desiredCount));

                totalruntime.Stop();

                appnotes = string.Format("Individual Recs in DB {0}, desired count: {1}, PDF Files in folder: {2}",
                                         RecordsCount, desiredCount, TotalPDFCount);

                stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                stats.SetField("ErrorCount", TotalErrorCount);
                stats.SetField("ErrorMessages", "");
                stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                stats.SetField("AppNotes", appnotes);
                stats.SetField("ExpectedCountOut", desiredCount);
                stats.SetField("OutputCount1", TotalPDFCount);
                stats.SetField("TotalRunTimeSecs", totalruntime.Elapsed.TotalSeconds);
                if (!stats.UpdateRecord())
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }

                return;
            }



            // Show 0% done and 0 record count done
            if (WT != null)
                WT.ReportProgress(0, string.Format("Recs Done {0} of {1}", RecordsDone, RecordsCount));


            // Bind DataTable to report viewer 
            //DataTable tempdt = ds.Tables[0].Clone();      // Clone DataTable schema
            DataTable dt = dtMailInfo.Clone();              // Clone DataTable schema
            report.LocalReport.DataSources.Clear();
            report.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));


            // For each record in the database to be processed, create report
            //foreach (DataRow dr in ds.Tables[0].Rows)
            foreach (DataRow drMailInfo in dtMailInfo.Rows)
            {
                i++;

                // Copy next row to empty data table for report viewer to use as a single record table
                dt.Rows.Clear();
                dt.ImportRow(drMailInfo);

                try
                {
                    // Get letter type number
                    LetterTypeID = Convert.ToInt32(dt.Rows[0][2]);
                    // Get Presort ID for letter
                    MPresortID = Convert.ToInt32(dt.Rows[0]["MPresortID"]);

                    // If lettertypeID is bigger than we have RDLC files for, count and ignore
                    if (LetterTypeID ==  10029 || LetterTypeID == 10030)// if (LetterTypeID >  43)
                    {
                        countOfLetterIdIsTooBig++;
                        continue;
                    }

                    // Get Report file (rdlc) name
                    rptName = rptFileName[LetterTypeID];

                    // Render report as PDF and Export to file on disk
                    rpttime.Start();
                    report.LocalReport.ReportEmbeddedResource = rptName;
                    ExportReport(MPresortID, batch, run, report);
                    rpttime.Stop();
                }
                catch (Exception ex)
                {
                    string s2 = "Error from PODFO Batch " + batch + run + " PresortID: " + MPresortID.ToString();
                    Log.Error(s2);
                    Log.Error(ex.Message);

                    // Format and log inner exception if any
                    string inner = string.Empty;
                    if (ex.InnerException != null)
                    {
                        inner = string.Format("InnerEx.Message: {0}", ex.InnerException.Message);
                        Log.Error(inner);
                    }

                    TotalErrorCount++;
                    ErrorMessages.Add(string.Format("{0} : {1}", s2, ex.Message));
                }

                // Count records processed
                RecordsDone++;
                RPMCount++;
                TTGCount++;
                counts[LetterTypeID]++;
                

                // If Worker Thread, create notifications
                if (WT != null)
                {
                    if ((WT != null) && (WT.CancellationPending))
                    {
                        Log.Error("GenerateIndividualPDFs: Canceled by Worker Thread Request (UI)");
                        break;
                    }

                    // Estimate Time to Completion (Time To Go)
                    if (TTGCount % ReportTTGCount == 0)
                    {
                        var rpmlong = TTGCount / (DateTime.Now - lastTTGTime).TotalMinutes;
                        lastTTGTime = DateTime.Now;
                        TTGCount = 0;
                        var ttgmin = (RecordsCount - RecordsDone) / rpmlong;
                        var ttghms = TimeSpan.FromMinutes(ttgmin);
                        TTGStr = ttghms.ToString(@"hh\:mm\:ss\.f");
                    }

                    // Report Background worker thread status
                    int pcrecsdone = (RecordsDone * 100) / (RecordsCount);
                    if (RecordsDone % ReportProgressCount == 0)
                    {
                        // Report Progress % and count of recs processed
                        WT.ReportProgress(pcrecsdone, string.Format("Recs Done {0} of {1}    Time left: {2}",
                                                                    RecordsDone, RecordsCount, TTGStr));

                        // Report records per minute
                        var rpm = RPMCount / (DateTime.Now - lastRPMTime).TotalMinutes;
                        if (WT != null)
                            WT.ReportProgress(-3, string.Format("R/M: {0:#.##}", rpm));
                        lastRPMTime = DateTime.Now;
                        RPMCount = 0;
                    }
                }

                #region "OLD - Switch by record type"

                    // OLD - Switch by record type
                //switch (LetterTypeID)
                //{
                //    case 1:                                      
                //{
                //    //DataSet set = new DataSet("test");
                //    //MPresortID
                //    //DSpodfo.

                //    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDMA_MAENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 2:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDMA_MASPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                   
                //    case 3:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDPD_PDAENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 4:
                //{
                //    //pODFODataSet1.Clear();
                //    try
                //    {
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    }
                //    catch (Exception ex)
                //    {
                //        clsEmail.EmailMessage("Error from PODFO Batch " + batch + run, "Generating DIS letter - " + ex.Message);
                //        //continue;
                //        //MessageBox.Show(ex.Message);
                //        //return "";
                //    }
                //    //this.USP_SELECT_DIS_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetDIS.USP_SELECT_DIS_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    //reportDataSource.Value = this.uSPSelectDISBindingSource;

                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDPD_PDASPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 7:
                //{
                //    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));                    
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISOPT_PDPENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 8:
                //{
                //    try
                //    {
                //        Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    }
                //    catch (Exception ex)
                //    {
                //        clsEmail.EmailMessage("Error from PODFO Batch " + batch + run, "Generating DIS letter - " + ex.Message);
                //        //continue;
                //    }
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISOPT_PDPSPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //     case 9:
                //{
                //    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENNGD.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //} 
                //    case 10:
                //{
                //    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENMBP.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 11:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINAENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 12:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINASP.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 15:
                //{
                //    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINRSAENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 16:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINRSASPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 17:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINUENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 18:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINUSPA.rdlc";
                //   ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 19:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTAENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 20:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTASPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 21:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTUENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 22:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTUSPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 23:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 24:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTSPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 25:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 26:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRSPA.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 29:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_DA_ENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 31:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAEP_ENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 33:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAWE_ENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 35:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_AE_ENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 37:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_NE_ENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 39:
                //{
                //    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_ENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 40:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_SPAN.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 41:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PREP_ENG.rdlc";
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
                //    case 43:
                //{
                //     Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PRWE_ENG.rdlc";                   
                //    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //    continue;
                //}
               
                //}
                //}
                //catch (Exception ex)
                //{
                //    try
                //    {
                //        switch (LetterTypeID)
                //        {
                //            case 1:
                //                {
                //                    DataSet set = new DataSet("test");
                //                    //MPresortID
                //                    //DSpodfo.

                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDMA_MAENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 2:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDMA_MASPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }

                //            case 3:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDPD_PDAENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 4:
                //                {
                //                    //pODFODataSet1.Clear();
                //                    try
                //                    {
                //                        Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    }
                //                    catch (Exception ex2)
                //                    {
                //                        clsEmail.EmailMessage("Error from PODFO Batch " + batch + run, "Generating DIS letter - " + ex2.Message);
                //                        //continue;
                //                        //MessageBox.Show(ex.Message);
                //                        //return "";
                //                    }
                //                    //this.USP_SELECT_DIS_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetDIS.USP_SELECT_DIS_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(batch), run,  Convert.ToInt32(dr["MPresortID"]));
                //                    //reportDataSource.Value = this.uSPSelectDISBindingSource;

                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDPD_PDASPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 7:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISOPT_PDPENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 8:
                //                {
                //                    try
                //                    {
                //                        Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    }
                //                    catch (Exception ex2)
                //                    {
                //                        clsEmail.EmailMessage("Error from PODFO Batch " + batch + run, "Generating DIS letter - " + ex2.Message);
                //                        //continue;
                //                    }
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISOPT_PDPSPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 9:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENNGD.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 10:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENMBP.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 11:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINAENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 12:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINASP.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 15:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINRSAENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 16:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINRSASPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 17:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINUENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 18:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINUSPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 19:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTAENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 20:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTASPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 21:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTUENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 22:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTUSPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 23:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 24:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTSPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 25:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 26:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRSPA.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 29:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_DA_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 31:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAEP_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 33:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAWE_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 35:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_AE_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 37:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_NE_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 39:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 40:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_SPAN.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 41:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PREP_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }
                //            case 43:
                //                {
                //                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                //                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PRWE_ENG.rdlc";
                //                    ExportReport(Convert.ToInt32(dr["MPresortID"]), batch, run, report);
                //                    continue;
                //                }

                //        }
                //    }
                //    catch (Exception ex3)
                //    {
                //        clsEmail.EmailMessage("Error from PODFO Batch " + batch + run, " PresortID: " + dr["MPresortID"] + "clsGenerateLetters " + ex3.Message);
                //        continue;
                //    }
                //    //MessageBox.Show(ex.Message);
                //    //return "";

                //}
                    #endregion

            } // End Foreach


            // Records Done, Collect and log statistics of processing
            try
            {
                // Count PDF files created
                var PDFCount = 0;
                var PDFCountfmt = string.Empty;
                PDFCount = Directory.EnumerateFiles(mstrFilePath + "\\" + batch + run, "*.PDF").Where(x => x.ToUpper().Contains(".PDF")).Count();
                if (!FromProcessWorker)
                {
                    PDFCountfmt = string.Format(", PDFs: {0}", PDFCount);

                    string sql = "SELECT count(*) AS 'Count' FROM [PODFODEV2].[dbo].[PODMailingInfo] " +
                                 "WHERE PODBatchID = @Batch AND PODRunID = @Run AND MPresortID > 0 AND PODMasterLetterTypeId < 44";
                    SqlParameter[] sp = new SqlParameter[2];
                    sp[0] = new SqlParameter("@Batch", batch);
                    sp[1] = new SqlParameter("@Run", run);
                    desiredCount = DbAccess.ExecuteScalar(sql, CommandType.Text, sp);
                    Log.Info(string.Format("GenerateIndividualPDFs: Desired PDF Count: {0}", desiredCount));
                }

                // Show 100% done
                if (WT != null)
                    WT.ReportProgress(100, string.Format("Recs Done {0} of {1}", RecordsDone, RecordsCount));

                // Log Statistics
                string s3 = string.Format(
                            "GenerateIndividualPDFs: Count: {0}, Load: {1}, Rpt: {2}, PDF: {3}, Ignored: {4}, RecoveredTimeouts: {5}{6}",
                            count, loadtime.Elapsed.ToString(@"hh\:mm\:ss\.f"),
                            rpttime.Elapsed.ToString(@"hh\:mm\:ss\.f"), pdftime.Elapsed.ToString(@"hh\:mm\:ss\.f"),
                            countOfLetterIdIsTooBig, RecoveredTimeoutErrorRecCount, PDFCountfmt);
                Log.Info(s3);

                Process procObj = Process.GetCurrentProcess();
                s3 = string.Format("Peak Working Set: {0} MB, Peak Paged Memory Size: {1} MB, Peak Virtual Memory Size: {2} MB",
                                    procObj.PeakWorkingSet64 / 1048576L, procObj.PeakPagedMemorySize64 / 1048576L,
                                    procObj.PeakVirtualMemorySize64 / 1048576L);
                Log.Info(s3);
                FormatCounts();
                totalruntime.Stop();


                // Limit Error messages string for DB
                string errorMessages = string.Empty;
                if (ErrorMessages.Count > 0)
                {
                    errorMessages = ErrorMessages.Aggregate((a, b) => a + ", " + b);
                    if (errorMessages.Length > 1023)
                        errorMessages = errorMessages.Substring(0, 1023);
                }

                appnotes = string.Format("Individual Recs Done {0} of {1}, desired: {2}, PDFs in folder: {3}, " +
                                         "Load: {4}, Rpt: {5}, Total: {6}",
                                         RecordsDone, RecordsCount, desiredCount, PDFCount,
                                         loadtime.Elapsed.ToString(@"hh\:mm\:ss\.f"), 
                                         rpttime.Elapsed.ToString(@"hh\:mm\:ss\.f"),
                                         totalruntime.Elapsed.ToString(@"hh\:mm\:ss\.f"));

                // Record Statistics in AppStats Table
                stats.SetField("Status", (TotalErrorCount == 0)?"OK":"ERRORS");
                stats.SetField("ErrorCount", TotalErrorCount);
                stats.SetField("ErrorMessages", errorMessages);
                stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                stats.SetField("AppNotes", appnotes);
                stats.SetField("MaxMemUsedMB", (int)(procObj.PeakVirtualMemorySize64 / 1048576L));
                stats.SetField("ExpectedCountOut", desiredCount);
                stats.SetField("InputCount1", count);
                stats.SetField("RejectCount1", countOfLetterIdIsTooBig);
                stats.SetField("OutputCount1", PDFCount);
                stats.SetField("OutputCount2", RecordsDone);
                stats.SetField("InputTimeSecs1", loadtime.Elapsed.TotalSeconds);
                stats.SetField("ProcessTimeSecs1", rpttime.Elapsed.TotalSeconds);
                stats.SetField("OutputTimeSecs1", pdftime.Elapsed.TotalSeconds);
                stats.SetField("TotalRunTimeSecs", totalruntime.Elapsed.TotalSeconds);
                stats.SetField("AppCount1", RecoveredTimeoutErrorRecCount);
                bool rcstats = stats.UpdateRecord();
                if (!rcstats)
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }
            }
            catch (Exception ex)
            {
                string s = string.Format("Error gathering and logging statistics: {0}", ex.Message);
                Log.Error(s);
                
                // Format and log inner exception if any
                string inner = string.Empty;
                if (ex.InnerException != null)
                {
                    inner = string.Format("InnerEx.Message: {0}", ex.InnerException.Message);
                    Log.Error(inner);
                }
            }
            finally
            {
                Log.Info("End GenerateIndividualPDFs");
            }
        }


        #region "TEST Code to Generate one of each letter type"
        public static void GenerateIndividualPDFsTEST(string batch, string run, PODFODataSet1 DSpodfo, PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter Ta, BindingSource bs, int intStart = 0, int intEnd = 0, string strLetterType = "")
        {
            reportDataSource.Name = "DataSet1";
            reportDataSource.Value = bs;
            int count;
            SqlDataAdapter adapter;

            var batchinfo = new clsBatchInfo();

            ReportViewer report = new ReportViewer();

            report.LocalReport.DataSources.Add(reportDataSource);


            if (!Directory.Exists(mstrFilePath + "\\" + batch + run))
            {
                DirectoryInfo di = Directory.CreateDirectory(mstrFilePath + "\\" + batch + run);
            }
            var DA = new clsGetBatchSort();
            if (intEnd == 0 && intStart == 0 && strLetterType == "")
            {
                //this dataAdapter will print the full set
                adapter = new SqlDataAdapter(DA.dsBatch_Sort(batch, run));

            }
            else if (strLetterType != "")
            {
                //This dataAdapter prints for a single letter type
                adapter = new SqlDataAdapter(DA.dsBatch_Sort(batch, run, strLetterType));
            }
            else if (intEnd != 0)
            {
                //use this dataAdapterr to fill to print a set range
                adapter = new SqlDataAdapter(DA.dsBatch_Sort(batch, run, intStart, intEnd));
            }
            else
            {
                //this dataAdapter will print the full set
                adapter = new SqlDataAdapter(DA.dsBatch_Sort(batch, run));
            }

            DataSet ds = new DataSet();
            adapter.Fill(ds);
            count = ds.Tables[0].Rows.Count;

            int i = 0;
            int LetterTypeID = 0;

            report.ProcessingMode = ProcessingMode.Local;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                i++;
                try
                {
                    LetterTypeID = Convert.ToInt32(dr[2]);
                  

                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDMA_MAENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                      
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDMA_MASPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDPD_PDAENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                        Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                             

                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISDPD_PDASPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISOPT_PDPENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                        Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                              
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.DIS.DISOPT_PDPSPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENNGD.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENMBP.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINAENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINASP.rdlc";
                    ExportReport(i, batch, run, report);
                              
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINRSAENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINRSASPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINUENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOINUSPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;

                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTAENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTASPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTUENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ACO.ACOOUTUSPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTSPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRSPA.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_DA_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAEP_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAWE_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_AE_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_NE_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_SPAN.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PREP_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PRWE_ENG.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENMBP_SPAN.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, Convert.ToInt32(dr["MPresortID"]));
                    report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.ENT.ENTENNGD_SPAN.rdlc";
                    ExportReport(i, batch, run, report);
                    i++;
                    break;
                              
                }
                catch (Exception ex)
                {
                    clsEmail.EmailMessage(string.Format("Error from PODFO Batch {0} run {1} {2}", batch, run, (UseTestDB) ? " TESTING" : ""),
                                            "clsGenerateLetters " + ex.Message);
                    continue;
                    //MessageBox.Show(ex.Message);
                    //return "";
                }
            }

        }
        #endregion


        #region "GetDataByPresortIDWithRetry - not used"
        /// <summary>
        /// Get the data record by PresortID with retry
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="run"></param>
        /// <param name="MPresortID"></param>
        /// <param name="DSpodfo"></param>
        /// <param name="Ta"></param>
        /// <returns></returns>
        //private static bool GetDataByPresortIDWithRetry(string batch, string run, int MPresortID, PODFODataSet1 DSpodfo, 
        //                                                PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter Ta,
        //                                                out Exception lastex)
        //{
        //    // Try to read the specific letter data by PresortID with error retry
        //    bool fillOK = true;
        //    Exception lastEX = null;


        //    // Read the record for a unique letter, if error retry with delay
        //    for (int i = 0; i < retryCount; i++)
        //    {
        //        try
        //        {
        //            Ta.Fill(DSpodfo.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(batch), run, MPresortID);
        //            fillOK = true;
        //            break;
        //        }
        //        catch (Exception ex)
        //        {
        //            fillOK = false;
        //            lastEX = ex;
        //            string s2 = string.Format("Error: Try: {0} of {1}, accessing Ta.Fill from PODFO Batch {2} Run: {3} PresortID: {4}",
        //                                       i, retryCount, batch, run, MPresortID);
        //            Log.Error(s2);
        //            Log.Error(string.Format("Ex.Message: {0}", ex.Message));
        //            if (ex.InnerException != null)
        //                Log.Error(string.Format("InnerEx.Message: {0}", ex.InnerException.Message));
        //            if (WT != null)
        //                WT.ReportProgress(-3, string.Format("Retry {0} of Ta.Fill, max {1}", i, retryCount));

        //            Thread.Sleep(retryDelay);
        //        }
        //    }

        //    lastex = lastEX;
        //    return (fillOK);
        //}
        #endregion


        /// <summary>
        /// Get the MailInfo records for letters with timeout retry
        /// </summary>
        /// <param name="batch">Batch number</param>
        /// <param name="run">Run number</param>
        /// <param name="intStart">Range start</param>
        /// <param name="intEnd">Range end</param>
        /// <param name="strLetterType">Letter Type</param>
        /// <param name="dtout">Returned data table</param>
        /// <param name="lastex">Returned last exception</param>
        /// <returns>OK or exception (T/F)</returns>
        private static bool GetDataForLettersWithRetry(string batch, string run, int intStart, int intEnd, string strLetterType,
                                                       out DataTable dtout, out Exception lastex)
        {
            // Try to read the records for Individual letters with error retry
            bool fillOK = true;
            Exception lastEX = null;
            DataTable dt = null;
            int start = intStart;
            int end = intEnd;
            string sp = "USP_Select_Batch_Address_To_Sort_Record_Range";
            bool uselettertype = false;
            List<SqlParameter> parameters = null;
            SqlParameter[] pa = null;


            // Check for full run and set very large range
            if (intEnd == 0 && intStart == 0)
            {
                start = 1;
                end = 100000000;
            }

            // Check for Run by letter type
            if (!string.IsNullOrEmpty(strLetterType))
            {
                uselettertype = true;
                sp = "USP_Select_Batch_Address_To_Sort_Record_Range_LetterType";
            }


            // Read the records for WC letters, if error retry with delay
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    // Build Stored Procedure parameters
                    parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@P_PODBatchID", batch));
                    parameters.Add(new SqlParameter("@P_PODRunID", run));
                    parameters.Add(new SqlParameter("@P_MPresortIDStart", start));
                    parameters.Add(new SqlParameter("@P_MPresortIDEnd", end));
                    if (uselettertype)
                        parameters.Add(new SqlParameter("@P_LetterType", strLetterType));
                    pa = parameters.ToArray();

                    dt = DbAccess.ExecuteQueryDataTable(sp, CommandType.StoredProcedure, pa);

                    fillOK = true;
                    break;
                }
                catch (Exception ex)
                {
                    fillOK = false;
                    dt = null;
                    lastEX = ex;
                    pa = null;
                    string s2 = string.Format("Error: Try: {0} of {1}, accessing SQL from PODFO Batch {2} Run: {3}",
                                                i, retryCount, batch, run);
                    Log.Error(s2);
                    TotalErrorCount++;
                    ErrorMessages.Add(string.Format("{0} : {1}", s2, ex.Message));

                    Log.Error(string.Format("Error Message: {0}", ex.Message));
                    if (ex.InnerException != null)
                        Log.Error(string.Format("Inner Error Message: {0}", ex.InnerException.Message));

                    if (WT != null)
                        WT.ReportProgress(-3, string.Format("Retry {0} of Ta.Fill, max {1}", i, retryCount));

                    Thread.Sleep(retryDelay);
                    RecoveredTimeoutErrorRecCount++;
                }
            }

            lastex = lastEX;
            dtout = dt;
            return (fillOK);
        }


        /// <summary>
        /// Export a report from report viewer to a PDF file
        /// </summary>
        /// <param name="PreSortID">PreSortID number</param>
        /// <param name="batch">Batch number</param>
        /// <param name="run">Run number</param>
        /// <param name="Report">ReportViewer report object</param>
        private static void ExportReport(int PreSortID, string batch, string run, ReportViewer Report)
        {      
            Report.RefreshReport();
          
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            string fmt = "00000000";


            // Use ReportViewer to produce a PDF file as a byte array
            byte[] bytes = Report.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            // Detect and log errors and warnings if errors
            if (warnings.Any(w => w.Severity == Severity.Error))
            {
                foreach (var w in warnings)
                {
                    var s = string.Format("Export Report: {0}: {1}", (w.Severity == Severity.Error) ? "Error" : "Warning", w.Message);
                    if (w.Severity == Severity.Error)
                        Log.Error(s);
                    else
                        Log.Warn(s);
                }
            }

            // Write PDF file returned by reportviewer from the byte array
            pdftime.Start();                
            string filename = mstrFilePath + "\\" + batch + run + "\\" + PreSortID.ToString(fmt) + ".PDF";
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);      
            }
            pdftime.Stop();
        }
       

        /// <summary>
        /// Format and log the counts by letter type
        /// </summary>
        private static void FormatCounts()
        {
            StringBuilder sb = new StringBuilder();
            int j = 0;
            var num = counts.Where(x => x.Value != 0).Count();

            sb.Append("Counts: ");
            for (int i = 0; i < counts.Count; i++)
            {
                if (counts[i] > 0)
                {
                    if (j >= 8)
                    {
                        Log.Info(sb.ToString());
                        sb.Clear();
                        sb.Append("Counts: ");
                        j = 0;
                    }

                    sb.AppendFormat("[{0}] : {1}  ", i, counts[i]);
                    j++;
                }
            }
            if (j > 0)
                Log.Info(sb.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="refid"></param>
        /// <param name="batch"></param>
        /// <param name="run"></param>
        public static void CreateSummaryReportAndEmail(string refid, string batch)
        {

            string query =
                    "Select a.refid AS 'RefID', " +
                    "  CONVERT(varchar(10), a.[DateTime], 101) as 'Date', " +
                    "  a.[Batch], a.[Run], " +
                    "  CASE  WHEN e.ErrCount > 0 THEN 'ERRORS' ELSE a.[status] END as 'Status', " +
                    "  e.ErrCount as 'Errors', " +
                    "  CONVERT(varchar, DATEADD(ms, DateDiff(ms, a.datetime, m.datetime), 0) + (m.TotalRunTimeSecs/100000), 108) as 'Total Time', " +
                    "  a.outputcount1 as 'Ind Count', w.OutputCount2 as 'WC Count', " +
                    "  a.OutputCount1+w.OutputCount2 as 'Total Count', " +
                    "  r.inputcount1 as 'Merged Files', r.outputcount2 as 'Merged Pages', " +
                    "  CONVERT(varchar, CAST(a.[datetime] AS time), 100) as 'Start', " +
                    "  CONVERT(varchar, CAST(DATEADD(ss, m.TotalRunTimeSecs, m.datetime) AS time), 100) AS 'End', " +
                    "  CONVERT(varchar, CAST(s.AppTime1 AS time), 100) AS 'SortStart', " +
	                "  CONVERT(varchar, DATEADD(ms, DateDiff(ms, s.AppTime1, s.AppTime2), 0), 108) AS 'Sort Time' " +
                    "FROM appstats as a " +
                    "  left join appstats as m on (a.RefID = m.RefID and m.AppSection = 'MoveToProduction' and " +
				    "				 a.batch = m.batch and a.run = m.run) " +
	                "  left join appstats as r on (a.RefID = r.RefID and r.AppSection = 'MergePDFs' and " +
				    "				a.batch = r.batch and a.run = r.run) " +
	                "  left join appstats as w on (a.RefID = w.RefID and w.AppSection = 'GenerateLettersWC' and " +
				    "				a.batch = w.batch and a.run = w.run) " +
	                "  left join (Select refid, batch, run, sum(ErrorCount) as 'ErrCount'  " +
	                "             from appstats " + 
                    "             group by refid, batch, run) as e on " + 
			        "                (a.RefID = e.RefID and a.batch = e.batch and a.run = e.run) " +
	                "  left join appstats AS s ON (a.RefID = s.RefID and s.AppSection = 'ProcessTodaysFiles' and " +
					"                              a.batch = s.batch and a.run = s.run) " +
                    "WHERE a.[AppSection] = 'GenerateIndividualPDFs' and a.SubAppNumber = '' " +
                    "      AND a.refid = @RefID ";

            SqlParameter[] parms = { new SqlParameter("@RefID", refID) };

            try
            {
                //DataTable dt = DbAccess.ExecuteQueryDataTable(query, CommandType.Text, parms);
                string html = DbAccess.ExecuteQueryToHTMLTable(query, CommandType.Text, parms);
                string s = string.Format("PODFO POD Letters Processing Summary - Batch {0} {1} {2}", batch, DateTime.Now.ToShortDateString(),
                                         (UseTestDB) ? "TESTING" : "");

                string[] Toemail = new string[] { "James@USAImages.net" };
                string te = Conf.GetString("SummaryReportEmail", "James@USAImages.net");
                if (!string.IsNullOrEmpty(te))
                {
                    Toemail = te.Split(',');
                    AutoEmail.EmailClass Email = new AutoEmail.EmailClass();
                    Email.SendEmailtoContacts(s, html, "usa_apps@unitedsystems.net", Toemail, "ReportsApplication1", null, true);
                }
            }
            catch (Exception ex)
            {
                string s = string.Format("CreateSummaryReportAndEmail Error: {0}", ex.Message);
                Log.Error(s);
                if (ex.InnerException != null)
                {
                    string inner = string.Format("InnerException: {0}", ex.InnerException.Message);
                    Log.Error(inner);
                }
            }

        }

    }
}
