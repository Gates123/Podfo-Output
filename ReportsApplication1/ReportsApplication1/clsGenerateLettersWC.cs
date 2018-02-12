using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using ceTe;
using ceTe.DynamicPDF;
using Utilities;
using System.Threading;
using System.Diagnostics;


namespace ReportsApplication1
{
    public class clsGenerateLettersWC
    {
        public static bool blFoundFont = true;
        //static CONNECTION con = new CONNECTION();

        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Log = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;


        private static string ceTeLicense = string.Empty;
        private static string strIndiviualPDFPath = "\\\\Cobmain\\USACMS\\PODFO\\Output\\IndividualPDFs\\";
        private static string strPODProcessFolder = "\\\\Cobmain\\USACMS\\PODFO\\Downloads\\WC_ZIPFILES\\PROCESSED\\";

        private static int retryCount = 20;
        private static int retryDelay = 10000;

        // BGW Report Progress every "n" records
        private static int ReportProgressCount = 10;

        public static WorkerThread WT = null;

        // Record Statistics in AppStats Table
        private static AppStats stats = null;
        private static string appName = string.Empty;
        private static int recordID = 0;
        public static string refID = string.Empty;

        private static int fileNotFoundCount = 0;
        private static int TotalErrorCount = 0;
        private static List<string> ErrorMessages = null;

        private static Stopwatch rpttime = null;
        private static Stopwatch PDFtime = null;


        /// <summary>
        /// Constructor
        /// </summary>
        static clsGenerateLettersWC()
        {
            Conn = DbAccess.GetConnectionString();
            Log = new Logging(Conn, "AppLog");
            Log.SourceBase = "MakePDFs";
            UseTestDB = DbAccess.UseTestDB;
            Conf = new ConfigTable(Conn);
            Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";

            retryCount = Conf.GetInt("RetryCount", 20);
            retryDelay = Conf.GetInt("RetryDelay", 10000);

            strIndiviualPDFPath = Conf.GetString("mstrFilePath", "\\\\Cobmain\\USACMS\\PODFO\\Output\\IndividualPDFs\\");
            strPODProcessFolder = Conf.GetString("PODProcessFolder", "\\\\Cobmain\\USACMS\\PODFO\\Downloads\\WC_ZIPFILES\\PROCESSED\\");
            ReportProgressCount = Conf.GetInt("ReportProgressCount", 10);
            ceTeLicense = Conf.GetString("ceTeLicense", "DPS70NEDJGMGEGWKOnLLQb4SjhbTTJhXnkpf9bj8ZzxFH+FFxctoPX+HThGxkpidUCHJ5b88fg4oUJSHiRBggzHdghUgkkuIvoag");
            // If TEST mode then force PROD folder for WC_ZIPFILES\\PROCESSED\ as this is where the data is for the WC letters
            if (UseTestDB)
            {
                //strPODProcessFolder = "\\\\Cobmain\\USACMS\\PODFO\\Downloads\\WC_ZIPFILES\\PROCESSED\\";
                //strPODProcessFolder = Conf.GetString("PODProcessFolder",
                //                                     "\\\\Cobmain\\USACMS\\PODFO\\Downloads\\WC_ZIPFILES\\PROCESSED\\", "PODFOReports");
            }
            if (!strIndiviualPDFPath.EndsWith("\\"))
            {
                strIndiviualPDFPath = strIndiviualPDFPath + "\\";
            }

        }


        /// <summary>
        /// Make WC PDFs
        /// </summary>
        /// <param name="batch">Batch number</param>
        /// <param name="run">Run number</param>
        /// <param name="intStart">Range Start</param>
        /// <param name="intEnd">Range End</param>
        public static void MakePDFs(string batch, string run, int intStart = 0, int intEnd = 0)
        {
            int RecordsCount = 0;
            int RecordsDone = 0;
            DateTime lastRPMTime = DateTime.Now;
            int RPMCount = 0;
            Stopwatch totalruntime = new Stopwatch();
            Stopwatch loadtime = new Stopwatch();
            rpttime = new Stopwatch();
            PDFtime = new Stopwatch();
            int TimeoutErrorRecCount = 0;
            totalruntime.Start();

            Log.SourceBase = "WCMakePDFs";
            TotalErrorCount = 0;
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
            stats.SetField("AppSection", "GenerateLettersWC");
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



            if ((WT != null) && (WT.CancellationPending))
            {
                Log.Warn("MakePDFs already canceled");
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
                WT.ReportProgress(-3, string.Format("R/M: {0:#.##}", 0.0));
                WT.ReportProgress(0, string.Format("Loading Batch / Run rows"));
            }

            Log.Info(string.Format("Start MakePDFs for Batch: {0}  Run: {1} in {2}", batch, run,
                        (DbAccess.UseTestDB) ? "TEST" : "PROD"));

            try
            {

                // Insure Output Directory exists for the PDFs
                if (!Directory.Exists(strIndiviualPDFPath + "\\" + batch + run))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strIndiviualPDFPath + "\\" + batch + run);
                }

                //string mConnectionString = "Data Source=usasql;Initial Catalog=PODFO;Integrated Security=True;";
                //SqlConnection conn = new SqlConnection(mConnectionString);
                SqlConnection conn = DbAccess.GetConnection();
                SqlCommand cmd;
                //conn.Open();

                //Get all rows from the table based on the batch and run
                loadtime.Start();
                if (intStart == 0 && intStart == 0)
                {
                    cmd = new SqlCommand("USP_SELECT_WC_LETTERS_FOR_BATCH_RUN");
                }
                else
                {
                    cmd = new SqlCommand("USP_SELECT_WC_LETTERS_FOR_BATCH_RUN_RANGE");
                    cmd.Parameters.AddWithValue("@P_Start_MPresortID", intStart);
                    cmd.Parameters.AddWithValue("@P_End_MPresortID", intEnd);
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@P_PODBatchID", batch);
                cmd.Parameters.AddWithValue("@P_PODRunID", run);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                // Can timeout
                //ds = new DataSet();
                //adapter.Fill(ds);
                DataSet ds = null;
                Exception lastex = null;
                bool rc = GetDataForWCLettersWithRetry(batch, run, adapter, out ds, out lastex);
                if (!rc || ds == null)
                {
                    string s = string.Format("MakePDFs: Error SQL exceded retries loading letter data: {0}", lastex.Message);
                    Log.Error(s);
                    // Show Error
                    if (WT != null)
                        WT.ReportProgress(-1, s);
                    clsEmail.EmailMessage(string.Format("Error from PODFO.MakePDFs {0}", (UseTestDB) ? "TESTING" : ""), s);

                    TotalErrorCount++;
                    TimeoutErrorRecCount++;
                    stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                    stats.SetField("ErrorCount", TotalErrorCount);
                    stats.SetField("ErrorMessages", s);
                    stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                    stats.SetField("AppCount1", TimeoutErrorRecCount);
                    if (!stats.UpdateRecord())
                    {
                        Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                    }
                    return;
                }

                RecordsCount = ds.Tables[0].Rows.Count;
                loadtime.Stop();

                lastRPMTime = DateTime.Now;
                RPMCount = 0;
                fileNotFoundCount = 0;

                if (WT != null)
                    WT.ReportProgress(0, string.Format("Recs Done {0} of {1}", RecordsDone, RecordsCount));

                rpttime.Start();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //create the report
                    CreateWCReport(dr);

                    RecordsDone++;
                    RPMCount++;

                    if (WT != null)
                    {
                        // Check if cancel pending, set canceled flag
                        if (WT.CancellationPending)
                        {
                            Log.Warn("MakePDFs: Canceled by Background Worker Request");
                            TotalErrorCount++;
                            ErrorMessages.Add("MakePDFs: Canceled by Background Worker Request");
                            break;
                        }

                        // Report Background worker thread status
                        int pcrecsdone = (RecordsDone * 100) / (RecordsCount);
                        if (RecordsDone % ReportProgressCount == 0)
                        {
                            // Report Progress % and count of recs processed
                            WT.ReportProgress(pcrecsdone, string.Format("Recs Done {0} of {1}", RecordsDone, RecordsCount));

                            // Report records per minute
                            var rpm = RPMCount / (DateTime.Now - lastRPMTime).TotalMinutes;
                            WT.ReportProgress(-3, string.Format("R/M: {0:#.##}", rpm));
                            lastRPMTime = DateTime.Now;
                            RPMCount = 0;
                        }
                    }
                }
                rpttime.Stop();
            }

            catch (Exception ex)
            {
                var log = new clsLog();
                log.mstrLogFileLocation = batch + run + ".txt";
                log.WriteToLogfile("clsGenerateLettersWC.MakePDFs " + ex.Message);
                string s = string.Format("Error: clsGenerateLettersWC.MakePDFs Batch: {0}, Run: {1} : {2}", batch, run, ex.Message);
                Log.Error(s);
                ErrorMessages.Add(s);
                TotalErrorCount++;

                clsEmail.EmailMessage(string.Format("Error from PODFO {0}", (UseTestDB) ? "TESTING" : ""),
                                      "clsGenerateLettersWC.MakePDFs " + ex.Message);
            }

            finally
            {
                // Show 100% done
                if (WT != null)
                    WT.ReportProgress(100, string.Format("Recs Done {0} of {1}", RecordsDone, RecordsCount));

                string s2 = string.Format("MakePDFs: Record count: {0}, Files not found count: {1}",
                                         RecordsCount, fileNotFoundCount);
                Log.Info(s2);

                if (fileNotFoundCount > 0)
                {
                    string msg = string.Format("Error: clsGenerateLettersWC.MakePDFs: There were {0} WCZip files not found",
                                             fileNotFoundCount);
                    Log.Error(msg);
                    clsEmail.EmailMessage(string.Format("Error from PODFO {0}", (UseTestDB) ? "TESTING" : ""), msg);
                }

                // Count PDF files created
                var TotalPDFCount = Directory.EnumerateFiles(strIndiviualPDFPath + batch + run, "*.PDF").Where(x => x.ToUpper().Contains(".PDF")).Count();
                Log.Info(string.Format("MakePDFs: Total PDFs in BatchRun Folder: {0}", TotalPDFCount));
                totalruntime.Stop();


                // Limit Error messages string for DB
                string errorMessages = string.Empty;
                if (ErrorMessages.Count > 0)
                {
                    errorMessages = ErrorMessages.Aggregate((a, b) => a + ", " + b);
                    if (errorMessages.Length > 1023)
                        errorMessages = errorMessages.Substring(0, 1023);
                }

                string appnotes = string.Format("WC Recs Done {0} of {1}, files not found: {2}, PDF Files in folder: {3}",
                                                RecordsDone, RecordsCount, fileNotFoundCount, TotalPDFCount);

                // Record Statistics in AppStats Table
                Process procObj = Process.GetCurrentProcess();
                stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                stats.SetField("ErrorCount", TotalErrorCount);
                stats.SetField("ErrorMessages", errorMessages);
                stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                stats.SetField("AppNotes", appnotes);
                stats.SetField("MaxMemUsedMB", (int)(procObj.PeakVirtualMemorySize64 / 1048576L));
                stats.SetField("ExpectedCountOut", RecordsCount);
                stats.SetField("InputCount1", RecordsCount);
                stats.SetField("OutputCount1", TotalPDFCount);
                stats.SetField("OutputCount2", RecordsDone);
                stats.SetField("InputTimeSecs1", loadtime.Elapsed.TotalSeconds);
                stats.SetField("ProcessTimeSecs1", rpttime.Elapsed.TotalSeconds);
                stats.SetField("OutputTimeSecs1", PDFtime.Elapsed.TotalSeconds);
                stats.SetField("TotalRunTimeSecs", totalruntime.Elapsed.TotalSeconds);
                stats.SetField("AppCount2", fileNotFoundCount);
                bool rcstats = stats.UpdateRecord();
                if (!rcstats)
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }
            Log.Info(string.Format("End MakePDFs for Batch: {0}, Run: {1}", batch, run));
            }

        }


        
        /// <summary>
        ///  Get the data records for WC letters with retry
        /// </summary>
        /// <param name="batch">batch number</param>
        /// <param name="run">run number</param>
        /// <param name="adapter">adapter to access data</param>
        /// <param name="dsout">created dataset</param>
        /// <param name="lastex">last exception</param>
        /// <returns>Data retrieved OK (T/F)</returns>        
        private static bool GetDataForWCLettersWithRetry(string batch, string run, SqlDataAdapter adapter, out DataSet dsout, out Exception lastex)
        {
            // Try to read the records for WC letters with error retry
            bool fillOK = true;
            Exception lastEX = null;
            DataSet ds = null;


            // Read the records for WC letters, if error retry with delay
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    ds = new DataSet();
                    adapter.Fill(ds);
                    fillOK = true;
                    break;
                }
                catch (Exception ex)
                {
                    fillOK = false;
                    ds.Dispose();
                    ds = null;
                    lastEX = ex;
                    string s2 = string.Format("Error: Try: {0} of {1}, accessing SQL from MakePDFs Batch {2} Run: {3}",
                                               i, retryCount, batch, run);
                    Log.Error(s2);
                    Log.Error(string.Format("Ex.Message: {0}", ex.Message));
                    if (ex.InnerException != null)
                        Log.Error(string.Format("InnerEx.Message: {0}", ex.InnerException.Message));
                    if (WT != null)
                        WT.ReportProgress(-3, string.Format("Retry {0} of Ta.Fill, max {1}", i, retryCount));

                    Thread.Sleep(retryDelay);
                }
            }

            lastex = lastEX;
            dsout = ds;
            return (fillOK);
        }


        /// <summary>
        /// Create WC letter
        /// </summary>
        /// <param name="dr">data row of WC letter</param>
        private static void CreateWCReport(DataRow dr)
        {

            try
            {
                ceTe.DynamicPDF.Document.AddLicense(ceTeLicense);
                //string strIndiviualPDFPath = "\\\\Cobmain\\USACMS\\PODFO\\Output\\IndividualPDFs\\";
                //string strPODProcessFolder = "\\\\Cobmain\\USACMS\\PODFO\\Downloads\\WC_ZIPFILES\\PROCESSED\\";
                string strMpresortID = Convert.ToString((Int32)dr["MPresortID"]);
                string strRun = (string)dr["PODRunID"];
                string strBatchRun = Convert.ToString((decimal)dr["PODBatchID"]) + (string)dr["PODRunID"]; 
                string strFileName = strIndiviualPDFPath + strBatchRun + "\\\\" + strMpresortID.PadLeft(8, '0') + ".PDF";
           
                string strBarcode = (string)dr["BarCodes"];
           
                string[] strBarcodes = strBarcode.Split(',');
                int intPDFPageCount;
                int intEvenPages;
                int intBarCode;
                string strZipFileID = Convert.ToString((decimal)dr["PODWCZipFileID"]);
                string strPDFName = (string)dr["LETTER_FILENAME"];
                string strPDFNameandPath = strPODProcessFolder + strZipFileID + "-" + strPDFName.Trim() + ".pdf";


                if (File.Exists(strPDFNameandPath))
                {

                    ceTe.DynamicPDF.Merger.MergeDocument pdf = new ceTe.DynamicPDF.Merger.MergeDocument(strPDFNameandPath);

                    intPDFPageCount = pdf.Pages.Count - 1;

                    //get Address Page
                    ceTe.DynamicPDF.Page CurrentPage = pdf.Pages[0];
                    CurrentPage.Dimensions.LeftMargin = 2;
                    if (strRun == "01")
                    {
                        AddBarCode(ref CurrentPage, strBarcodes[0], false, true);
                    }
                    else
                    {
                        AddBarCode(ref CurrentPage, strBarcodes[0], true, true);
                    }

                    //set up address string
                    string strAddress;
                    string strEndorse = (string)dr["MEndorse"]; //.ToString.Replace("*", "")
                    string strKeyline = (string)dr["MKeyLine"];
                    string strTray = Convert.ToString((Int32)dr["MTray"]);
                    string strPackage = Convert.ToString((Int32)dr["MPackage"]);
                    string strPageCount = Convert.ToString((decimal)dr["PageCount"]);
                    string strPresortID = Convert.ToString((Int32)dr["MPresortID"]);

                    strEndorse = strKeyline + System.Environment.NewLine + "***" + strEndorse.Replace("*", "").Trim() +
                                 " R:" + strPresortID + " T:" + strTray + " P:" + strPackage + " PC:" + strPageCount + " F: " + strBatchRun;

                    strAddress = strEndorse + System.Environment.NewLine;
                    strAddress = strAddress + (string)dr["mailname"] + System.Environment.NewLine;
                    strAddress = strAddress + (string)dr["SatoriAdd1"] + System.Environment.NewLine;


                    //CHECK SECOND ADDRESS LINE
                    string strAdd2 = (string)dr["SatoriAdd2"];
                    if (strAdd2.Trim().Length > 0)
                    {
                        strAddress = strAddress + strAdd2 + System.Environment.NewLine;
                    }

                    //ADD CITY STATE ZIP            
                    strAddress = strAddress + (string)dr["SatoriCityStateZip"] + System.Environment.NewLine;


                    if (strRun == "01")
                    {
                        //create address label 
                        ceTe.DynamicPDF.PageElements.Label MyAddressLabel = new ceTe.DynamicPDF.PageElements.Label(strAddress, 80, 90, 612, 70, ceTe.DynamicPDF.Font.TimesRoman, 10, TextAlign.Left);
                        CurrentPage.Elements.Add(MyAddressLabel);

                        //ADD IMB
                        ceTe.DynamicPDF.PageElements.BarCoding.IntelligentMailBarCode IMBBarcode = new ceTe.DynamicPDF.PageElements.BarCoding.IntelligentMailBarCode((string)dr["MBarcodeNumber"], 80, 80);
                        CurrentPage.Elements.Add(IMBBarcode);

                    }
                    else
                    {
                        ceTe.DynamicPDF.PageElements.Label MyAddressLabel = new ceTe.DynamicPDF.PageElements.Label(strAddress, 320, 100, 612, 70, ceTe.DynamicPDF.Font.TimesRoman, 10, TextAlign.Left);
                        CurrentPage.Elements.Add(MyAddressLabel);
                        //ADD IMB
                        ceTe.DynamicPDF.PageElements.BarCoding.IntelligentMailBarCode IMBBarcode = new ceTe.DynamicPDF.PageElements.BarCoding.IntelligentMailBarCode((string)dr["MBarcodeNumber"], 320, 169);
                        CurrentPage.Elements.Add(IMBBarcode);

                    }


                    //add barcode to remaining pages
                    for (intEvenPages = 2; intEvenPages < intPDFPageCount; intEvenPages += 2)
                    {
                        intBarCode = intEvenPages / 2;

                        //add barcode
                        CurrentPage = pdf.Pages[intEvenPages];

                        if (strRun == "01")
                        {
                            AddBarCode(ref CurrentPage, strBarcodes[intBarCode], false, false);
                        }
                        else
                        {
                            AddBarCode(ref CurrentPage, strBarcodes[intBarCode], true, false);
                        }


                    }


                    // pdf.Pages.Add(blankPage)
                    pdf.FormFlattening = ceTe.DynamicPDF.Merger.FormFlatteningOptions.Default;
                    pdf.CompressionLevel = 0;
                    pdf.PdfVersion = ceTe.DynamicPDF.PdfVersion.v1_3;

                    PDFtime.Start();
                    pdf.Draw(strFileName);
                    PDFtime.Stop();

                    pdf = null;
                    CurrentPage = null;
                }//file exits check
                else
                    fileNotFoundCount++;

            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("clsGenerateLettersWC.CreateWCReport " + ex.Message);
                
            }
        

        }


        /// <summary>
        /// Add a Barcode to the PDF page
        /// </summary>
        /// <param name="objCurrentPage">current ceTe page to add to</param>
        /// <param name="strBarcode">barcode string</param>
        /// <param name="blnFlat"></param>
        /// <param name="blnAddressPage"></param>
        private static void AddBarCode(ref ceTe.DynamicPDF.Page objCurrentPage, string strBarcode, bool blnFlat, bool blnAddressPage)
        {
            var log = new clsLog();

           
            //set up barcode
            ceTe.DynamicPDF.Text.OpenTypeFont MyFont;
            try
            {

                string strFontName = "";
                float flX;
                float flY;
                float flFontSize;

                if (blnFlat == true)
                {
                    if (blnAddressPage == true)
                    {
                        flX = 180;
                        flY = 705;
                    }
                    else
                    {
                        flX = 180;
                        flY = 755;

                    }
                    flFontSize = 36.0F;
                }
                else
                {
                    if (blnAddressPage == true)
                    {
                        flX = 0;
                        flY = 197;
                    }
                    else
                    {
                        flX = 0;
                        flY = 227;
                    }
                    flFontSize = 40.0F;
                }

                //set up barcode
                ceTe.DynamicPDF.PageElements.Forms.TextField MyBarcodeBack = 
                                new ceTe.DynamicPDF.PageElements.Forms.TextField("BarcodeBack", flX, flY - 15, 290, 50);
                MyBarcodeBack.TextAlign = Align.Center;
                MyBarcodeBack.BackgroundColor = RgbColor.White;
                MyBarcodeBack.DefaultValue = "";

                ceTe.DynamicPDF.PageElements.Forms.TextField MyBarcode = 
                                new ceTe.DynamicPDF.PageElements.Forms.TextField("Barcode", flX, flY, 250, 40);

                MyBarcode.TextAlign = Align.Center;
                //if (strFontName != "Code3903.TTF")
                //{
                //    strFontName = "C3903.TTF";
                //}
                strFontName = "C3903.TTF";

                //if (IsFontInstalled(strFontName) == false)
                //{

                //    strFontName = "C3903.TTF";
                //    log.WriteToLogfile("IsFontInstalled(strFontName) = false" + strFontName);
                //}
                if (blFoundFont == false)
                {
                    strFontName = "Code3903.TTF";

                }
                try
                {
                    MyFont = new ceTe.DynamicPDF.Text.OpenTypeFont(strFontName);
                }
                catch
                {
                    blFoundFont = false;
                    //strFontName = "C3903.TTF";
                    strFontName = "Code3903.TTF";
                    MyFont = new ceTe.DynamicPDF.Text.OpenTypeFont(strFontName);
                }
                MyBarcode.BackgroundColor = RgbColor.White;
                //MyBarcode.BorderStyle = ceTe.DynamicPDF.PageElements.Forms.BorderStyle.Solid;
                MyBarcode.Font = MyFont;
                MyBarcode.FontSize = flFontSize;

                if (blnFlat == false)
                {
                    MyBarcode.Rotate = 90;
                    MyBarcodeBack.Rotate = 90;
                }

                //set text equal to first barcode
                MyBarcode.DefaultValue = "*" + strBarcode + "*";

                //add barcode to page
                objCurrentPage.Elements.Add(MyBarcodeBack);
                objCurrentPage.Elements.Add(MyBarcode);


                if (blnFlat == true)
                {
                    if (blnAddressPage == true)
                    {
                        flX = 200;
                        //flY = 702;
                        flY = 699;
                    }
                    else
                    {
                        flX = 200;
                        //flY = 752;
                        flY = 749;
                    }
                }
                else
                {
                    if (blnAddressPage == true)
                    {
                        flX = 40;
                        flY = 225;
                    }
                    else
                    {
                        flX = 40;
                        flY = 255;
                    }
                }

                //Create readable barcode
                ceTe.DynamicPDF.PageElements.Forms.TextField MyreadableBarcode = new ceTe.DynamicPDF.PageElements.Forms.TextField("Barcode2", flX, flY, 200, 8);
                MyreadableBarcode.TextAlign = Align.Center;
                MyreadableBarcode.BackgroundColor = RgbColor.White;
                //MyreadableBarcode.BorderStyle = ceTe.DynamicPDF.PageElements.Forms.BorderStyle.Solid;

                ceTe.DynamicPDF.Text.OpenTypeFont MyFont2 = new ceTe.DynamicPDF.Text.OpenTypeFont("Times.TTF");
                MyreadableBarcode.Font = MyFont2;
                MyreadableBarcode.FontSize = 6;
                if (blnFlat == false)
                {
                    MyreadableBarcode.Rotate = 270;
                }

                //set text equal to first barcode
                MyreadableBarcode.DefaultValue = "*" + strBarcode + "*";

                //add readable barcode to page
                objCurrentPage.Elements.Add(MyreadableBarcode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating WC report // AddBarCode", ex);
            }

        }


        /// <summary>
        /// Is a font installed in Windows
        /// </summary>
        /// <param name="fontName">name of font to look for</param>
        /// <returns></returns>
        private static bool IsFontInstalled(string fontName)
        {
            using (var testFont = new System.Drawing.Font(fontName, 8))
            {
                return 0 == string.Compare(
                  fontName,
                  testFont.OriginalFontName,
                  StringComparison.InvariantCultureIgnoreCase);
            }
        }

    }
}
