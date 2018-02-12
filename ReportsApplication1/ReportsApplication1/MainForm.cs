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
using Utilities;
using ReportsApplication1.Properties;



namespace ReportsApplication1
{
    
    public partial class MainForm : Form
    {        
        // Get connection string
        private string Conn = null;
        // Create DB Logger
        private Logging Logger = null;
        // ConfigTable Access
        private ConfigTable Conf = null;

        // Use Test DB flag
        private bool UseTestDB = false;


        public string mstrFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs";
        public string mstrMergeFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\MergedPDFs";
        public int msrtintCount = 0;
        public string mstrRun = "";
        public string mstrBatch = "";
       // public ReportViewer viewer = new ReportViewer();

        Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource();
        Enums.LetterTypes LetterTypes = new Enums.LetterTypes();

        public static BackgroundWorker BGW = null;
        public static WorkerThread WT = null;
        public static string refID = string.Empty;


        public MainForm()
        {
            Conn = DbAccess.GetConnectionString();
            Conf = new ConfigTable(Conn);
            Logger = new Logging(Conn, "AppLog");
            UseTestDB = DbAccess.UseTestDB;

            mstrFilePath = GetParm("mstrFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs");
            mstrMergeFilePath = GetParm("mstrMergeFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\MergedPDFs");

            // Pass pointer to Worker Thread Object to our called modules
            clsGenerateLetters.WT = WT;
            clsAuto.WT = WT;
            clsGenerateLettersWC.WT = WT;
            clsMerge.WT = WT;
            clsMove.WT = WT;
            clsGetBatchSort.WT = WT;
            clsJobTicket.WT = WT;

            // Pass AppStats referenceID to group stats records
            clsGenerateLetters.refID = refID;
            clsGenerateLettersWC.refID = refID;
            clsMerge.refID = refID;
            clsMove.refID = refID;
            clsJobTicket.refID = refID;
        }


        public MainForm(string batch, string run, bool reprocess = false) : this()
        {
            SetSettingsConnection();

            //***Overload for Unused****
            mstrBatch = batch;
            mstrRun = run;


            //txtBatch.Text = batch;
            //txtRun.Text = run;
           // clsGenerateLettersWC.MakePDFs(mstrBatch, mstrRun);
            
            InitializeComponent();
            var log = new clsLog();
            log.mstrLogFileLocation = batch + run + ".txt";
            log.WriteToLogfile("Starting Main");
            if (reprocess == false)
            {
                clsAuto.RunAutomated(batch, run, pODFODataSet1, 
                                     new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(), 
                                     uSPSelectBatchAddressToSortBindingSource, uSP_SELECT_Letter_CountBindingSource);
            }
           // this.Close();
        }


        // Used for ReRunFull 
        public MainForm(string batch, string run, bool blMakeNonWCPDFs, bool blMakeWCPDFs,
                        bool blMerge, bool blJobTicket, bool blEmailReport, bool blMoveToProdcution)
            : this()
        {
            SetSettingsConnection();

            //***Overload for Automation****
            // we call this load method instead of calling each class because we need to pass the binding source and 
            // this was the only way I could figure out to pass that to each class.
            mstrBatch = batch;
            mstrRun = run;

            InitializeComponent();
            var log = new clsLog();
            log.mstrLogFileLocation = batch + run + ".txt";
            log.WriteToLogfile("Starting Main");
            // if (reprocess == false)
            {
                // MessageBox.Show("AUTOMATIONG GO!");
                clsAuto.RunAutomated(batch, run, pODFODataSet1,
                                     new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(),
                                     uSPSelectBatchAddressToSortBindingSource, uSP_SELECT_Letter_CountBindingSource,
                                     blMakeNonWCPDFs, blMakeWCPDFs, blMerge, blJobTicket, blEmailReport, blMoveToProdcution);
            }
            // this.Close();
        }


        // Used for ReRun Range
        public MainForm(string batch, string run, bool blMakeNonWCPDFs, bool blMakeWCPDFs, int intStart, int intEnd) : this()
        {
            SetSettingsConnection();

            //***Overload for ReRunRange****
            // we call this load method instead of calling each class because we need to pass the binding source
            // and this was the only way I could figure out to pass that to each class.
            mstrBatch = batch;
            mstrRun = run;
           // MessageBox.Show("RERUN RANGE GO!");
            InitializeComponent();
            var log = new clsLog();
            log.mstrLogFileLocation = batch + run + ".txt";
            log.WriteToLogfile("Starting Main");

            if(blMakeNonWCPDFs == true)
                clsGenerateLetters.GenerateIndividualPDFs(batch, run, pODFODataSet1, 
                                                          new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(), 
                                                          uSPSelectBatchAddressToSortBindingSource, intStart, intEnd, "");
            if(blMakeWCPDFs == true)
                clsGenerateLettersWC.MakePDFs(batch, run, intStart, intEnd);
            
        }


        // Used for Rerun Letter 
        public MainForm(string batch, string run, string strLetterType)  : this()
        {
            SetSettingsConnection();

            //we call this load method instead of calling each class because we need to pass the binding source and this was the only way I could figure out to pass that to each class.
            mstrBatch = batch;
            mstrRun = run;
           // MessageBox.Show("RERUN LETTER GO!");
            InitializeComponent();
            var log = new clsLog();
            log.mstrLogFileLocation = batch + run + ".txt";
            log.WriteToLogfile("Starting Main");
            //clsGenerateLetters.GenerateIndividualPDFsTEST(batch, run, pODFODataSet1, new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(), uSPSelectBatchAddressToSortBindingSource);
            //clsGenerateLetters.GenerateIndividualPDFs(batch, run, pODFODataSet1, 
            //                                          new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(), 
            //                                          uSPSelectBatchAddressToSortBindingSource);

            clsGenerateLetters.GenerateIndividualPDFs(batch, run, pODFODataSet1,
                                                      new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(),
                                                      uSPSelectBatchAddressToSortBindingSource, 0, 0, strLetterType);

            // what about the "WC" letter types ???
            //if (strLetterType != "WC")
            //{
            //    clsGenerateLetters.GenerateIndividualPDFs(batch, run, pODFODataSet1, new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(), uSPSelectBatchAddressToSortBindingSource, 0, 0, strLetterType);
            //}
            //else
            //{
            //    clsGenerateLettersWC.MakePDFs(batch, run);
            //}           
            
        }

        
        /// <summary>
        /// Set the "Settings PODFOConnectionString" used by table adapters to
        /// equal the DbAccess connection string.
        /// </summary>
        private void SetSettingsConnection()
        {

            string s  = DbAccess.GetConnectionString();

            Settings.Default["PODFOConnectionString"] = s;
            string s2 = (string) Settings.Default["PODFOConnectionString"];
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'PODFODataSet.PODMasterLetterType' table. You can move, or remove it, as needed.
            //this.PODMasterLetterTypeTableAdapter.Fill(this.PODFODataSet.PODMasterLetterType);            
            // this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            //reportDataSource.Name = "DataSet1";
            //reportDataSource.Value = this.uSPSelectBatchAddressToSortBindingSource;

            //   this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
        }


        //private string ExportReport(int counter, string batch, string run)
        private string ExportReport(int PreSortID, string batch, string run, ReportViewer Report)
        {
            try
            {
                Report.RefreshReport();

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                //byte[] bytes = viewer.LocalReport.Render(
                //    "PDF", null, out mimeType, out encoding, out filenameExtension,
                //     out streamids, out warnings);

                string fmt = "00000000";



                byte[] bytes = Report.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);

                //string filename = Path.Combine("C:\\PDF\\", "report" + PreSortID.ToString(fmt) + ".pdf");
                //string filename = Path.Combine(mstrFilePath + "\\" + batch + run + "\\" + PreSortID.ToString(fmt) + ".pdf");
               string filename = mstrFilePath + "\\" + batch + run + "\\" + PreSortID.ToString(fmt) + ".PDF";
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
               
                return filename;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
                return "";
            }
        }


        private string ExportReport(int PreSortID, string batch, string run)
        {
            try
            {

                this.reportViewer1.RefreshReport();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                //byte[] bytes = viewer.LocalReport.Render(
                //    "PDF", null, out mimeType, out encoding, out filenameExtension,
                //     out streamids, out warnings);

                string fmt = "00000000";



                byte[] bytes = reportViewer1.LocalReport.Render(
                   "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);

                //string filename = Path.Combine("C:\\PDF\\", "report" + PreSortID.ToString(fmt) + ".pdf");
                //string filename = Path.Combine(mstrFilePath + "\\" + batch + run + "\\" + PreSortID.ToString(fmt) + ".pdf");
                string filename = mstrFilePath + "\\" + batch + run + "\\Email" + PreSortID.ToString(fmt) + ".PDF";
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                return filename;
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
                return "";
            }
        }
           

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            
        }


        private void btGenerate_Click(object sender, EventArgs e)
        {

            clsGenerateLetters.GenerateIndividualPDFs(txtBatch.Text, txtRun.Text, pODFODataSet1, 
                        new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(), 
                        uSPSelectBatchAddressToSortBindingSource);
            
          
        }


        private void btnMerge_Click(object sender, EventArgs e)
        {
            clsMerge.MergePDFs(txtBatch.Text, txtRun.Text);

        }
        

        private void btnJobTicket_Click(object sender, EventArgs e)
        {
            try
            {
                clsJobTicket.CreateJobTicket(txtBatch.Text, txtRun.Text);
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }

        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                clsGenerateLettersWC.MakePDFs(txtBatch.Text, txtRun.Text);
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }
        }


        private void btnEmail_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var DA = new clsGetBatchSort();
            //SqlDataAdapter adapter = new SqlDataAdapter(DA.dsBatch_Sort(txtBatch.Text, txtRun.Text));
            //DataSet ds = new DataSet();
            //adapter.Fill(ds);

            //msrtintCount = ds.Tables[0].Rows.Count;
            //clsEmail.EmailMessage("PODFO File's processed", "File " + txtBatch.Text + txtRun.Text + " processed. It had " + msrtintCount + " records.", true);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }


        private void btnMove_Click(object sender, EventArgs e)
        {
            clsMove.MoveToProduction(txtBatch.Text, txtRun.Text);
        }


        private void button3_Click(object sender, EventArgs e)
        {
           // clsEmail.CreateLetterReportPDF(txtBatch.Text, txtRun.Text, pODFODataSet1,new PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter(), uSP_SELECT_Letter_CountBindingSource);
            
           //   string mConnectionString = "Data Source=usasql;Initial Catalog=PODFO;Integrated Security=True;";
           // this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
           
           // //SqlConnection conn = new SqlConnection(mConnectionString);
           // //conn.Open();

           // // //Get all rows from the table based on the batch and run
           // //SqlCommand cmd = new SqlCommand("[USP_SELECT_Letter_Count]");
           // //cmd.CommandType = CommandType.StoredProcedure;
           // //cmd.Connection = conn;
           // //cmd.Parameters.AddWithValue("@P_Batch", txtBatch.Text);
            

           // if (!Directory.Exists(mstrFilePath + "\\" + txtBatch.Text + txtRun.Text))
           // {
           //     DirectoryInfo di = Directory.CreateDirectory(mstrFilePath + "\\" + txtBatch.Text + txtRun.Text);
           // }
            

           // //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
           // //DataSet ds = new DataSet();
           // //adapter.Fill(ds);

           // reportDataSource.Value = this.uSP_SELECT_Letter_CountBindingSource;
           // reportDataSource.Name = "DataSet1";

           //// msrtintCount = ds.Tables[0].Rows.Count;


           // reportViewer1.ProcessingMode = ProcessingMode.Local;

          
           //     this.uSP_SELECT_Letter_CountTableAdapter.Fill(pODFODataSet1.USP_SELECT_Letter_Count, Convert.ToDecimal(txtBatch.Text));


           //     reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.Reports.Reports_Letter_Count.rdlc";
              
           //     this.ExportReport(1, txtBatch.Text, txtRun.Text);
                
           

        }


        private void btnImport_Click(object sender, EventArgs e)
        {

        }


        private void btnUpload_Click(object sender, EventArgs e)
        {

        }


        private void btnSort_Click(object sender, EventArgs e)
        {

        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            clsAuto.RunAutomated(txtBatch.Text, txtRun.Text, pODFODataSet1, new PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter(), uSPSelectBatchAddressToSortBindingSource, uSP_SELECT_Letter_CountBindingSource);
        }


        /// <summary>
        /// Log messages
        /// </summary>
        /// <param name="level">Level, e.g. I, W, E</param>
        /// <param name="msg">Message text</param>
        private void Log(string level, string msg)
        {
            Logger.Log(level, "ReportsApplication", msg);
        }


        /// <summary>
        /// Get parameter from Config table in DB
        /// </summary>
        /// <param name="parm">Parm value desired</param>
        /// <param name="def">default value if no parm</param>
        /// <returns>Parm value or default</returns>
        private string GetParm(string parm, string def)
        {
            string value = string.Empty;
            string group = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";
            value = Conf.Get(group, parm);
            if (string.IsNullOrEmpty(value))
                value = def;
            return (value);
        }


        /// <summary>
        /// Get parameter from Config table in DB
        /// </summary>
        /// <param name="group">parm group name</param>
        /// <param name="parm">Parm value desired</param>
        /// <param name="def">default value if no parm</param>
        /// <returns>Parm value or default</returns>
        private string GetParm(string group, string parm, string def)
        {
            string value = string.Empty;
            string grp = (UseTestDB) ? group + ".Test" : group;
            value = Conf.Get(grp, parm);
            if (string.IsNullOrEmpty(value))
                value = def;
            return (value);
        }



                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 24)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_CPC_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetCPC.USP_SELECT_CPC_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectCPCBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTSPA.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 25)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_CPC_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetCPC.USP_SELECT_CPC_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectCPCBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr[2] == 26)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_CPC_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetCPC.USP_SELECT_CPC_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectCPCBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.CPC.CPCOUTRSPA.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 29)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_DA_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 31)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAEP_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 33)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RAWE_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 35)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_AE_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 37)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_NE_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 39)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 40)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_RS_SPAN.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 41)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PREP_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
                ////else if ((decimal)dr["podmasterlettertypeid"] == 43)
                ////{
                ////    this.uSP_Select_Batch_Address_To_SortTableAdapter.Fill(pODFODataSet1.USP_Select_Batch_Address_To_Sort, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //this.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUNTableAdapter.Fill(pODFODataSetMBP.USP_SELECT_MBP_LETTERS_FOR_BATCH_RUN, Convert.ToDecimal(txtBatch.Text), txtRun.Text, i);
                ////    //reportDataSource.Value = this.uSPSelectMBPBindingSource;

                ////    this.reportViewer1.LocalReport.ReportEmbeddedResource = "ReportsApplication1.MBP.MBP_PRWE_ENG.rdlc";
                ////    this.reportViewer1.RefreshReport();
                ////    this.ExportReport(i);
                ////    continue;
                ////}
               


            }
    



        }

        
   
