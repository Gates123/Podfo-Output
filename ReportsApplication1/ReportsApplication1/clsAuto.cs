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
    public class clsAuto
    {
        public static WorkerThread WT = null;

        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Log = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;


        /// <summary>
        /// Execute several work steps automatically based on fixed list of steps
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="run"></param>
        /// <param name="DSpodfo"></param>
        /// <param name="Ta"></param>
        /// <param name="bs"></param>
        /// <param name="countBS"></param>
        public static void RunAutomated(string batch, string run, PODFODataSet1 DSpodfo, 
                                        PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter Ta, 
                                        BindingSource bs, BindingSource countBS)
        {
            if ((WT != null) && (WT.CancellationPending))
            {
                Log.Error("RunAutomated already canceled");
                return;
            }

            var log = new clsLog();
            log.mstrLogFileLocation = batch + run + ".txt";
            log.WriteToLogfile("Starting RunAutomated");

            log.WriteToLogfile("Starting GenerateIndividualPDFs");
            if (WT != null)
                WT.ReportProgress(-2, String.Format("Make Non WC PDFs"));
            clsGenerateLetters.GenerateIndividualPDFs(batch, run, DSpodfo, Ta, bs);

            log.WriteToLogfile("Starting MakePDFs");
            if (WT != null)
                WT.ReportProgress(-2, String.Format("Make WC PDFs"));
            clsGenerateLettersWC.MakePDFs(batch, run);

            log.WriteToLogfile("Starting MergePDFs");
            if (WT != null)
                WT.ReportProgress(-2, String.Format("Merge"));
            clsMerge.MergePDFs(batch, run);

            log.WriteToLogfile("Starting CreateJobTicket");
            if (WT != null)
                WT.ReportProgress(-2, String.Format("Job Ticket"));
            clsJobTicket.CreateJobTicket(batch, run);

           // log.WriteToLogfile("Starting CreateLetterReportPDF");                    
           // clsEmail.CreateLetterReportPDF(batch, run, DSpodfo, new PODFODataSet1TableAdapters.USP_SELECT_Letter_CountTableAdapter(), countBS);
            //clsMove.MoveToProduction(batch, run);
        }


        /// <summary>
        /// Execute several work steps automatically based on true/false flag per step
        /// </summary>
        /// <param name="batch">Batch number for process</param>
        /// <param name="run">Run number for process</param>
        /// <param name="DSpodfo"></param>
        /// <param name="Ta"></param>
        /// <param name="bs"></param>
        /// <param name="countBS"></param>
        /// <param name="blMakeNonWcPDFs">Make non WC PDFs T/F</param>
        /// <param name="blMakeWcPDFs">Make WC PDFs T/F</param>
        /// <param name="blMerge">Perform Merge T/F</param>
        /// <param name="blJobTicket">Create Job Ticket T/F</param>
        /// <param name="blEmailReport">Email Report T/F</param>
        /// <param name="blMoveToProduction">Move Files to Production T/F</param>
        public static void RunAutomated(string batch, string run, PODFODataSet1 DSpodfo, 
                                        PODFODataSet1TableAdapters.USP_Select_Batch_Address_To_SortTableAdapter Ta,
                                        BindingSource bs, BindingSource countBS, bool blMakeNonWcPDFs, 
                                        bool blMakeWcPDFs, bool blMerge, bool blJobTicket, bool blEmailReport, bool blMoveToProduction)
        {
            if ((WT != null) && (WT.CancellationPending))
            {
                Log.Error("RunAutomated already canceled");
                return;
            }

            var log = new clsLog();
            log.mstrLogFileLocation = batch + run + ".txt";
            log.WriteToLogfile("Starting RunAutomated");

            if (blMakeNonWcPDFs == true)
            {
                log.WriteToLogfile("Starting GenerateIndividualPDFs");
                if (WT != null)
                    WT.ReportProgress(-2, String.Format("Make Non WC PDFs"));
                clsGenerateLetters.GenerateIndividualPDFs(batch, run, DSpodfo, Ta, bs);
            }
            if (blMakeWcPDFs == true)
            {
                log.WriteToLogfile("Starting clsGenerateLettersWC");
                if (WT != null)
                    WT.ReportProgress(-2, String.Format("Make WC PDFs"));
                clsGenerateLettersWC.MakePDFs(batch, run);
            }
            //if (blMerge == true && (blMakeNonWcPDFs == true) || blMakeWcPDFs == true)
            if (blMerge == true)
                //archive run 
                //clsArchiveBatch.Archive(batch, run);

            if (blMerge == true)
            {
                log.WriteToLogfile("Starting MergePDFs");
                if (WT != null)
                    WT.ReportProgress(-2, String.Format("Merge"));
                clsMerge.MergePDFs(batch, run);
            }
            if (blEmailReport == true)
            {
                //log.WriteToLogfile("Starting CreateLetterReportPDF");
                clsEmail.CreateLetterReportPDF(batch);
            }
            if (blJobTicket == true)
            {
                log.WriteToLogfile("Starting CreateJobTicket");
                if (WT != null)
                    WT.ReportProgress(-2, String.Format("JobT icket"));
                clsJobTicket.CreateJobTicket(batch, run);
            }

          

            if (blMoveToProduction == true)
            {
                log.WriteToLogfile("Starting MoveToProduction");
                if (WT != null)
                    WT.ReportProgress(-2, String.Format("Move To Production"));
                clsMove.MoveToProduction(batch, run);
            }
        }

    }
}
