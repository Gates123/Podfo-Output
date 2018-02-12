using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using ReportsApplication1;
using Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using ceTe;
using ceTe.DynamicPDF.Merger;
using ceTe.DynamicPDF;
using QREncoder;
namespace ReportsApplication1
{

    class clsJobTicket
    {
        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Log = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;

        private static string strOutputFolder = "\\\\Cobmain\\usacms\\PODFO\\Output\\Job Tickets\\";
        private static string ConnPMS = "Server=COBDATA1;Database=PrintMailScan;User ID=ieq;Password=Ramius123;Trusted_Connection=False";
        private static string ConnSage = "Server=usamailer;Database=Sage_Inventory;User ID=USASql;Password=Usa@12345678;Trusted_Connection=False";
        public static WorkerThread WT = null;

        // Record Statistics in AppStats Table
        private static AppStats stats = null;
        private static string appName = string.Empty;
        private static int recordID = 0;
        public static string refID = string.Empty;

        private static int TotalErrorCount = 0;
        private static List<string> ErrorMessages = null;
        private static Stopwatch totalruntime = null;
        private static int totalOutputFileCount = 0;



        /// <summary>
        /// Constructor
        /// </summary>
        static clsJobTicket()
        {
            Conn = DbAccess.GetConnectionString();
            UseTestDB = DbAccess.UseTestDB;
            Conf = new ConfigTable(Conn);
            Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";
            Log = new Logging(Conn, "AppLog");
            Log.SourceBase = "CreateJobTicket";

            strOutputFolder = Conf.GetString("JobTicketOutputFolder", "\\\\Cobmain\\usacms\\PODFO\\Output\\Job Tickets\\");
            ConnPMS = Conf.GetString("ConnPMS", "Server=COBDATA1;Database=PrintMailScan;User ID=ieq;Password=Ramius123;Trusted_Connection=False");
            ConnSage = Conf.GetString("ConnSage", "Server=usamailer;Database=Sage_Inventory;User ID=USASql;Password=Usa@12345678;Trusted_Connection=False");
        }


        /// <summary>
        /// Create the Job Ticket for the Batch / Run
        /// </summary>
        /// <param name="batch">Batch number for process</param>
        /// <param name="run">Run number for process</param>
        public static void CreateJobTicket(string batch, string run) 
        {
            totalruntime = new Stopwatch();
            totalruntime.Start();
            totalOutputFileCount = 0;
            ErrorMessages = new List<string>();
            int mailScheduleID; 

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
            stats.SetField("AppSection", "CreateJobTicket");
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

            // If already canceled
            if ((WT != null) && (WT.CancellationPending))
            {
                Log.Error("CreateJobTicket already canceled");
                TotalErrorCount++;
                stats.SetField("Status", (TotalErrorCount == 0) ? "OK" : "ERRORS");
                stats.SetField("ErrorCount", TotalErrorCount);
                stats.SetField("ErrorMessages", "CreateJobTicket already canceled");
                stats.SetField("TestProd", (UseTestDB) ? "TEST" : "PROD");
                if (!stats.UpdateRecord())
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }
                return;
            }

            Log.Info(string.Format("CreateJobTicket for Batch {0}, Run {1}", batch, run));

            string strPDFName = "PODFO-" + batch+run;
            var log = new clsLog();
            log.mstrLogFileLocation = batch + run + ".txt";

            // Build document folder name for job ticket template
            string strRootFolder = string.Empty;
            strRootFolder = System.IO.Directory.GetCurrentDirectory().ToLower();
            strRootFolder +=  "\\documents\\";
            log.WriteToLogfile("Job ticket template location = " + strRootFolder);

            string strJob = "PODFO";
            int intRun = Convert.ToInt16(run);
            SqlConnection conn = DbAccess.GetConnection();
            StreamReader srReader;
            string strFile;
            StreamWriter swWriter;
            string strJobTicketName = "";
            FileInfo fileinfo = new FileInfo(strPDFName);
            Dictionary<string, int> itemCodeDic = new Dictionary<string, int>();
            string itemCode = string.Empty;
            int packets = 0;
            int images = 0;

            try
            {
                //Get record count and image count
                var DA = new clsGetBatchSort();


                //update this query 
                SqlDataAdapter adapter = new SqlDataAdapter(DA.dsBatch_Sort(batch, run));
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                packets = ds.Tables[0].Rows.Count;
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    images += Convert.ToInt32(dr["PageCount"]);
                }

                // Get Must Mail Date
                SqlDataAdapter adapter2 = new SqlDataAdapter(DA.dsGet_MailDate(batch, run));
                DataSet ds2 = new DataSet();
                adapter2.Fill(ds2);
                DateTime dtMustMailDate = Convert.ToDateTime(ds2.Tables[0].Rows[0][0].ToString());
                string Postage = string.Empty;
                if(intRun == 1)
               Postage = ds2.Tables[0].Rows[0]["Postage"].ToString();

                //open the job ticket template, read in the entire file, close the template
                srReader = File.OpenText(strRootFolder + "PODFOJOBTICKET.rtf");
                strFile = srReader.ReadToEnd();
                srReader.Close();


                strJobTicketName = strOutputFolder + fileinfo.Name + ".rtf";

                swWriter = File.CreateText(strJobTicketName);

                //replace the variable data in the job ticket

                if (intRun == 1)
                {
                    strFile = strFile.Replace("$RANGE$", "1 to 5");
                    if (packets >= 200)
                    {
                        itemCode = Conf.GetString("10_Envelope_IND", "1102000");
                        strFile = strFile.Replace("NVLP", string.Concat("#10 PODFO Window Envelope IND ", itemCode));
                        strFile = strFile.Replace("$ENVELOPE2$", "#10 PODFO Window Envelope IND");
                    }
                    else
                    {
                        itemCode = Conf.GetString("10_Envelope_ND", "1103000");
                        strFile = strFile.Replace("NVLP", string.Concat("#10 PODFO Window Envelope NI", itemCode));
                        strFile = strFile.Replace("$ENVELOPE2$", "#10 PODFO Window Envelope NI");
                    }
                }
                else if (intRun == 2)
                {
                    strFile = strFile.Replace("$RANGE$", "6 to 75");
                    if (packets >= 200)
                    {
                        itemCode = Conf.GetString("9x12_Envelope_IND", "1191000");
                        strFile = strFile.Replace("NVLP", string.Concat("PODFO #9 X 12 Window IND", itemCode));
                        strFile = strFile.Replace("$ENVELOPE2$", "PODFO #9 X 12 Window IND");
                    }
                    else
                    {
                        itemCode = Conf.GetString("9x12_Envelope_ND", "1193000");
                        strFile = strFile.Replace("NVLP", string.Concat("PODFO #9 X 12 Window NI", itemCode));
                        strFile = strFile.Replace("$ENVELOPE2$", "PODFO #9 X 12 Window NI");
                    }

                }

                itemCodeDic.Add(itemCode, packets);
                itemCodeDic.Add( Conf.GetString("PaperItemCode", "2085011"), images / 2);
                mailScheduleID = InsertIntoPrintMailScan(strJob + batch.ToString() + "-R0" + intRun.ToString(), packets, batch, run, dtMustMailDate, Postage);
                strFile = strFile.Replace("NPACKS", packets.ToString());
                strFile = strFile.Replace("$PACKETS$", packets.ToString());
                strFile = strFile.Replace("$IMAGECOUNT$", images.ToString());
                strFile = strFile.Replace("PKTS", packets.ToString());

                //add up records for other thing

                strFile = strFile.Replace("$RUN$", "0" + intRun.ToString());
                strFile = strFile.Replace("$BATCH$", batch);
                strFile = strFile.Replace("$FULLJOBTYPE$", strJob + " POD LETTERS");
                strFile = strFile.Replace("$MAILDATE$", dtMustMailDate.ToString("MM/dd/yyyy"));                
                strFile = strFile.Replace("$PDFNAME$", strPDFName + ".pdf");
                strFile = strFile.Replace("StockBarcode", itemCode);
                // Write Job Ticket File
                swWriter.WriteLine(strFile);
                swWriter.Flush();
                swWriter.Close();
                totalOutputFileCount++;
               


                ConvertJobTicketToPdf(strJobTicketName,string.Concat( strJob , batch.ToString() , "-R0" + intRun.ToString(),"-",packets.ToString(),";" , mailScheduleID), itemCode);
                foreach (string itemKey in itemCodeDic.Keys)
                {
                    InsertItemUsageIntoSage_Inventory(strJob, itemCodeDic[itemKey], batch, run, itemKey);
                }
                
            }
            catch (Exception ex)
            {
                string s = string.Format("Error in Batch: {0}, Run: {1} for clsJobTicket: {2}", batch, run, ex.Message);
                clsEmail.EmailMessage(string.Format("Error from PODFO Batch {0} run {1} {2}",
                                      batch, run, (UseTestDB) ? " TESTING" : ""), s);
                Log.Error(s);
                ErrorMessages.Add(s);
                TotalErrorCount++;

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
                totalruntime.Stop();

                string s = string.Format("clsJobTicket: {0} Job Tickets, for {1} records, and {2} pages, RunTime: {3} ",
                                         totalOutputFileCount, packets, images, totalruntime.Elapsed.ToString(@"hh\:mm\:ss\.f"));
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
                stats.SetField("ExpectedCountOut", 1);
                stats.SetField("InputCount1", packets);
                stats.SetField("InputCount2", images);
                stats.SetField("OutputCount1", totalOutputFileCount);
                //stats.SetField("OutputCount2", PDFOuptutFileCount);
                //stats.SetField("InputTimeSecs1", loadtime.Elapsed.TotalSeconds);
                stats.SetField("ProcessTimeSecs1", totalruntime.Elapsed.TotalSeconds);
                //stats.SetField("OutputTimeSecs1", PDFtime.Elapsed.TotalSeconds);
                stats.SetField("TotalRunTimeSecs", totalruntime.Elapsed.TotalSeconds);
                //stats.SetField("AppCount3", mailDatFileCount);
                bool rcstats = stats.UpdateRecord();
                if (!rcstats)
                {
                    Log.Error(string.Format("Error Updating AppStats record: {0}", stats.Message));
                }
            }
        }

        private static string ConvertJobTicketToPdf(string FullFileName, string JobName, string StockCode)
        {
            FileInfo objWordDoc = new FileInfo(FullFileName);
            string pdfFullName = FullFileName.Replace(".rtf", " .pdf");
            Microsoft.Office.Interop.Word.Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document oMainDoc = oWord.Documents.Add(FullFileName);
            oMainDoc.SaveAs(pdfFullName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
            oMainDoc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            objWordDoc.Delete();

            return BarcodeJobTicketPdf(pdfFullName, JobName, StockCode);
         
        }
        private static string BarcodeJobTicketPdf(string FullFileName, string JobName, string StockCode)
        {
            string pdfFullName = FullFileName.Replace(" .pdf", ".pdf");
            ceTe.DynamicPDF.Document.AddLicense("DPS70NEDJGMGEGWKOnLLQb4SjhbTTJhXnkpf9bj8ZzxFH+FFxctoPX+HThGxkpidUCHJ5b88fg4oUJSHiRBggzHdghUgkkuIvoag");
            var doc = new ceTe.DynamicPDF.Document();
            var page = new ceTe.DynamicPDF.Page();
            
            MergeDocument MyDocJobTicket = new MergeDocument();
            PdfDocument pdfTemplate = new PdfDocument(FullFileName);
            var qrCode = new ceTe.DynamicPDF.PageElements.Image(Encode.QR(JobName), 300, 50);
            qrCode.Height = 90;
            qrCode.Width = 90;

            MyDocJobTicket.Append(pdfTemplate);
            MyDocJobTicket.Pages[0].Dimensions.SetMargins(0);
            
            MyDocJobTicket.Pages[0].Elements.Add(qrCode);
            qrCode = new ceTe.DynamicPDF.PageElements.Image(Encode.QR(StockCode), 50, 405);
            qrCode.Height = 38;
            qrCode.Width = 38;
            MyDocJobTicket.Pages[0].Elements.Add(qrCode);
            MyDocJobTicket.FormFlattening = FormFlatteningOptions.Default;
            MyDocJobTicket.Draw(pdfFullName);
            MyDocJobTicket = null;
            FileInfo fi = new FileInfo(FullFileName);
            fi.Delete();
            return pdfFullName;
        }

        /// <summary>
        /// Insert Job into Print Mail Scan database for production
        /// </summary>
        /// <param name="JobName"></param>
        /// <param name="Qty"></param>
        /// <param name="strBatch"></param>
        /// <param name="strRun"></param>
        /// <param name="MailDate"></param>
        public static int InsertIntoPrintMailScan(string JobName, int Qty, string strBatch, string strRun, DateTime MailDate, string Postage)
        {
            int mailScheduleID;
            try
            {
               
                SqlCommand cmdSql = new SqlCommand();
                //   Dim conn As New CONNECTION
                SqlConnection connPMS = new SqlConnection(ConnPMS);
                connPMS.Open();

                //setup command
                cmdSql.CommandText = "ADD_TO_PRINTMAILSCAN_WITH_POSTAGE_AND_ACCOUNT";
                cmdSql.CommandType = CommandType.StoredProcedure;
                cmdSql.Connection = connPMS;
                //'set the parameters
                cmdSql.Parameters.AddWithValue("@P_JobName", "PODFO" + strBatch + "R" + strRun);
                cmdSql.Parameters.AddWithValue("@P_QTY", Qty);
                cmdSql.Parameters.AddWithValue("@P_MAILDATE", MailDate.ToString("MM/dd/yyyy").ToString());
                cmdSql.Parameters.AddWithValue("@P_TYPE", "P");

                if (Postage == string.Empty || Postage == "")
                    cmdSql.CommandText = "ADD_TO_PRINTMAILSCAN";
                else
                {
                    cmdSql.Parameters.AddWithValue("@P_POSTAGE", Postage);
                    cmdSql.Parameters.AddWithValue("@P_Account", "1");
                }
                cmdSql.CommandTimeout = 60;
                mailScheduleID = Convert.ToInt32(cmdSql.ExecuteScalar());

               UpdateBatchMailScheduleID(strBatch, strRun, mailScheduleID);
                //set the parameters
                //cmdSql.ExecuteNonQuery();
               
                //execute the command

                //clean up
                cmdSql.Dispose();
                //      conn = Nothing
                connPMS = null;
            }
            catch (Exception ex)
            {
                clsEmail.EmailMessage(string.Format("Error from PODFO Batch {0} Run {1} {2}", strBatch, strRun, (UseTestDB) ? " TESTING" : ""),
                                      "clsJobTicket.InsertIntoPrintMailScan " + ex.Message);
                mailScheduleID = 0;
            }
            return mailScheduleID;
        }
        /// <summary>
        /// Insert Job into JobItemUsage database for inventory tracking
        /// </summary>
        /// <param name="JobName"></param>
        /// <param name="Qty"></param>
        /// <param name="strBatch"></param>
        /// <param name="strRun"></param>
        /// <param name="ItemCode"></param>
        public static void InsertItemUsageIntoSage_Inventory(string JobName, int Qty, string strBatch, string strRun, string ItemCode)
        {

            try
            {

                SqlCommand cmdSql = new SqlCommand();
                //   Dim conn As New CONNECTION
                SqlConnection connSage = new SqlConnection(ConnSage);

                connSage.Open();

                //setup command
                cmdSql.CommandText = "Insert_JobItemUsage";
                cmdSql.CommandType = CommandType.StoredProcedure;
                cmdSql.Connection = connSage;
                //'set the parameters
                cmdSql.Parameters.AddWithValue("@Project", JobName);
                cmdSql.Parameters.AddWithValue("@Batch", strBatch);
                cmdSql.Parameters.AddWithValue("@Run", strRun);
                cmdSql.Parameters.AddWithValue("@JobName", JobName);
                cmdSql.Parameters.AddWithValue("@ItemCode", ItemCode);
                cmdSql.Parameters.AddWithValue("@CountUsed", Qty);

                cmdSql.CommandTimeout = 60;
                //set the parameters
                cmdSql.ExecuteNonQuery();

                //execute the command

                //clean up
                cmdSql.Dispose();
                connSage = null;
            }
            catch (Exception ex)
            {
                clsEmail.EmailMessage(string.Format("Error from PODFO Batch {0} Run {1} {2}", strBatch, strRun, (UseTestDB) ? " TESTING" : ""),
                                      "clsJobTicket.InsertIntoPrintMailScan " + ex.Message);
            }
        }
        public static void UpdateBatchMailScheduleID(string strBatch, string strRun, int MailScheduleJobId)
        {
            SqlConnection conn = DbAccess.GetConnection();
            SqlCommand cmdSql = new SqlCommand();
            cmdSql.CommandText = string.Concat("UPDATE PODBatch SET MailScheduleID = ", MailScheduleJobId,
                " WHERE PODBatch = ", strBatch, " AND PODRunID = ", strRun);
            cmdSql.CommandTimeout = 0;
            cmdSql.CommandType = CommandType.Text;
            cmdSql.Connection = conn;
            cmdSql.ExecuteNonQuery();
        }
      
    }
}
