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


namespace ReportsApplication1
{
   public class clsEmail
    {
        private static string mstrFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs";
        private static string mstrMergeFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\MergedPDFs";
        private static string mstrEmails = "\\\\Cobmain\\usacms\\PODFO\\Output\\Emails";

        // public ReportViewer viewer = new ReportViewer();


        private static Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource();


        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Log = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;

       
        /// <summary>
        /// Constructor
        /// </summary>
        static clsEmail()
        {
            Conn = DbAccess.GetConnectionString();
            UseTestDB = DbAccess.UseTestDB;
            Conf = new ConfigTable(Conn);
            Conf.DefaultGroupName = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";
            Log = new Logging(Conn, "AppLog");
            Log.SourceBase = "clsEmail";

            mstrFilePath = Conf.GetString("mstrFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs");
            mstrMergeFilePath = Conf.GetString("mstrMergeFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\MergedPDFs");
            mstrEmails = Conf.GetString("mstrEmails", "\\\\Cobmain\\usacms\\PODFO\\Output\\Emails");
        }


        public static void EmailMessage(string strSubject, string strMessage, bool blnInternalEmail = true,
                                        string strAttachementPath = "", bool isBodyHTML = false)
        {   
            //**************************************************************************
            //Written By: Pamela Alford
            //Date: 08/10/06
            //Edited 12/3/13 By: David Gates
            //Reason: Pickup folder no longer exists changed code to implement a new method
            //***************************************************************************



            try
            {
                string[] Toemail = new string[] { };
                if (blnInternalEmail == true)
                {
                    Toemail = Conf.GetString("InternalEmail", "Programmers@UNITEDSYSTEMS.NET, Veronica@USAImages.net").Split(',');
                    //Toemail = Conf.GetString("InternalEmail", "Programmers@UNITEDSYSTEMS.NET, Veronica@USAImages.net").Split(',');
                }
               
                else
                {
                    Toemail = Conf.GetString("ExternalEmail", "rick.whitener@revelanttech.com,Tonya.Sutterfield@wellpoint.com, jeff.byrnes@revelanttech.com, samuel.jenkins@cms.hhs.gov, Stephen.Kertesz@gdit.com, Michael.gash@gdit.com, Programmers@UNITEDSYSTEMS.NET, Veronica@UNITEDSYSTEMS.NET, Bluejacket@USAImages.net").Split(',');
                }

                AutoEmail.EmailClass Email = new AutoEmail.EmailClass();
                if (strAttachementPath != "")
                {
                    System.Net.Mail.Attachment File = new System.Net.Mail.Attachment(strAttachementPath);
                    System.Net.Mail.Attachment[] AttachmentLetterReport = new System.Net.Mail.Attachment[] { File };

                    Email.SendEmailtoContacts(strSubject, strMessage, "usa_apps@unitedsystems.net", Toemail, "ReportsApplication1", AttachmentLetterReport, isBodyHTML);
                }
                else
                {
     
                    Email.SendEmailtoContacts(strSubject, strMessage, "usa_apps@unitedsystems.net", Toemail, "ReportsApplication1", null, isBodyHTML);
                }


            }
            catch (Exception ex)
            {
                //WriteToLogFile(ex.ToString);
                //MsgBox(ex.ToString)

                string s = string.Format("EmailMessage Error: {0}", ex.Message);
                Log.Error(s);
                if (ex.InnerException != null)
                {
                    string inner = string.Format("InnerException: {0}", ex.InnerException.Message);
                    Log.Error(inner);
                }
            }
        }


        public static void CreateLetterReportPDFNoLongerUsed(string batch, string run, PODFODataSet1 DSpodfo, PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter Ta, BindingSource bs)
        {
            try
            {

                reportDataSource.Name = "DataSet1";
                reportDataSource.Value = bs;
                ReportViewer report = new ReportViewer();

                report.LocalReport.DataSources.Add(reportDataSource);



                if (!Directory.Exists(mstrEmails + "\\" + batch))
                {
                    DirectoryInfo di = Directory.CreateDirectory(mstrEmails + "\\" + batch);
                }



                report.ProcessingMode = ProcessingMode.Local;


                Ta.Fill(DSpodfo.USP_SELECT_Letter_Count, Convert.ToDecimal(batch));

                report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.Reports.Reports_Letter_Count.rdlc";


                ExportReport(batch, report);
            }
            catch (Exception ex)
            {
                clsEmail.EmailMessage("PODFO error", "Error in create Letter Report PDF." + ex);
            }
        }


        public static void CreateLetterReportPDF(string batch)
        {
            try
            {
                SqlConnection conn = DbAccess.GetConnection();
                //Get all rows from the table based on the batch and run
                //SqlCommand cmd = new SqlCommand("SELECT dbo.PODMailingInfo.* FROM  dbo.PODMailingInfo WHERE (PODBatchID = " + txtBatch.Text + ") AND (PODRunID = '" + txtRun.Text + "') order by MPresortID", conn);
                SqlCommand cmd = new SqlCommand("[USP_SELECT_Letter_Count]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@P_Batch", batch);
                cmd.CommandTimeout = 0;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                string strHTMLmessage = "<table border=\"1\"><tr><th>LetterType</th><th>SubType</th><th>TotalLetterCount</th><th>ValidLettersCount</th><th>InvalidLettersCount</th>";
                int TotalLetterCount = 0;
                int TotalValidLetterCount = 0;
                int TotalInvalidLetterCount = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strHTMLmessage += "</tr><tr>";
                    strHTMLmessage += "<td>" + dr["LetterType"].ToString() + "</td>";
                    strHTMLmessage += "<td>" + dr["SubType"].ToString() + "</td>";
                    strHTMLmessage += "<td>" + dr["TotalLetterCount"].ToString() + "</td>";
                    strHTMLmessage += "<td>" + dr["ValidLettersCount"].ToString() + "</td>";
                    strHTMLmessage += "<td>" + dr["InvalidLettersCount"].ToString() + "</td>";
                    TotalLetterCount += Convert.ToInt32(dr["TotalLetterCount"]);
                    TotalValidLetterCount += Convert.ToInt32(dr["ValidLettersCount"]);
                    TotalInvalidLetterCount += Convert.ToInt32(dr["InvalidLettersCount"]);
                }
                strHTMLmessage += "</tr>";
                strHTMLmessage += "<tr>" + "<td><b>Total</b></td><td></td><td><b>" + TotalLetterCount + "</b></td>" + "<td><b>" + TotalValidLetterCount + "</b></td>" + "<td><b>" + TotalInvalidLetterCount + "</b></td></tr></table>";
                strHTMLmessage += "<p>If you notice a discrepancy with the information provided, please contact Bluejacket@USAImages.net immediately.</p>";
                //if (!Directory.Exists(mstrEmails + "\\" + batch))
                //{
                //    DirectoryInfo di = Directory.CreateDirectory(mstrEmails + "\\" + batch);
                //}


                if (UseTestDB)                
                    clsEmail.EmailMessage("TEST PODFO POD Letters Processed - Batch " + batch + " " + DateTime.Now.ToShortDateString(), strHTMLmessage, true, "", true);
                else
                    clsEmail.EmailMessage("PODFO POD Letters Processed - Batch " + batch + " " + DateTime.Now.ToShortDateString(), strHTMLmessage, false, "", true);
                
                strHTMLmessage = "";
                TotalLetterCount = 0;
                cmd.Dispose();
                cmd = null;
                ds.Dispose();
                ds = null;
                adapter.Dispose();
                adapter = null;

                cmd = new SqlCommand("[USP_Select_MailInfo_PostageAmount]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@P_Batch", batch);
                cmd.CommandTimeout = 0;

                double TotalPostage = 0;
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds);
                strHTMLmessage = "<table border=\"1\"><tr><th>LetterType</th><th>Record Count</th><th>Total Postage($)</th>";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strHTMLmessage += "</tr><tr>";
                    strHTMLmessage += "<td>" + dr["LetterType"].ToString() + "</td>";
                    strHTMLmessage += "<td>" + dr["RecordCount"].ToString() + "</td>";
                    strHTMLmessage += "<td>" + dr["PostageAmount"].ToString() + "</td>";
                    TotalLetterCount += Convert.ToInt32(dr["RecordCount"]);
                    TotalPostage += Convert.ToDouble(dr["PostageAmount"]);
                }
                strHTMLmessage += "</tr>";
                strHTMLmessage += "<tr>" + "<td><b>Total</b></td><td><b>" + TotalLetterCount + "</b></td>" + "<td><b>" + TotalPostage + "</b></td></tr></table>";
                strHTMLmessage += "<p>If you notice a discrepancy with the information provided, please contact Bluejacket@USAImages.net immediately.</p>";
                //report.LocalReport.ReportEmbeddedResource = "ReportsApplication1.Reports.Reports_Letter_Count.rdlc";
                if (UseTestDB)
                    clsEmail.EmailMessage("TEST PODFO POD Letters Total Postage - Batch " + batch + " " + DateTime.Now.ToShortDateString(), strHTMLmessage, true, "",true);
                else
                    clsEmail.EmailMessage("PODFO POD Letters Total Postage - Batch " + batch + " " + DateTime.Now.ToShortDateString(), strHTMLmessage, true, "", true);
                cmd.Dispose();
                cmd = null;
                ds.Dispose();
                ds = null;
                adapter.Dispose();
                adapter = null;

                cmd = new SqlCommand("UPDATE PODBATCH  set Postage = " + TotalPostage + " where PODBatch = " + batch + " and PODRunID = '01'");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                cmd = null;
            }
            catch (Exception ex)
            {
                string s = string.Format("EmailMessage Error: {0}", ex.Message);
                Log.Error(s);
                if (ex.InnerException != null)
                {
                    string inner = string.Format("InnerException: {0}", ex.InnerException.Message);
                    Log.Error(inner);
                }

                clsEmail.EmailMessage("PODFO error", "Error in create Letter Report PDF." + ex.Message);
            }
        }
         

        private static void ExportReport( string batch, ReportViewer Report)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] bytes = Report.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);


                string filename = mstrEmails + "\\" + batch + "\\PODFO Daily Report: " + batch + ".PDF";
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                EmailMessage("PODFO Letter Report", "The attached is the letter report for today's PODFOFiles", true, filename);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in export Report", ex);
            }
        }

    }
}
