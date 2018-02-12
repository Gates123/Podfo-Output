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

namespace ReportsApplication1
{
    class clsArchiveBatch
    {

        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Logger = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;

        private static string strBatchPathP = "\\\\Cobmain\\usacms\\PODFO\\Output\\mergedPDFs\\";

        /// <summary>
        /// Constructor
        /// </summary>
        static clsArchiveBatch()
        {
            Conn = DbAccess.GetConnectionString();
            Conf = new ConfigTable(Conn);
            Logger = new Logging(Conn, "AppLog");
            UseTestDB = DbAccess.UseTestDB;

            strBatchPathP = GetParm("mstrMergeFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\mergedPDFs\\");
        }


        public static void Archive(string strBatch, string strRun)
        {
            //string strFilePath = "\\\\Cobmain\\usacms\\PODFO\\Output\\mergedPDFs";
            string strBatchPath = strBatchPathP + strBatch + strRun;
            if (!Directory.Exists(strBatchPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(strBatchPath);
            }

            Log("I", string.Format("Archive Start for Batch: {0} Run: {1}", strBatch, strRun));
            string[] fileEntries = Directory.GetFiles(strBatchPath);
            string archive = strBatchPath + "\\Archive " + DateTime.Now.ToString("MM-dd-yyyy HH mm ss");


            if (!Directory.Exists(archive))
            {
                DirectoryInfo di = Directory.CreateDirectory( archive);
            }

            foreach (string Fi in fileEntries)
            {
                File.Copy(Fi,  archive + "\\" + Fi.Remove(0, Fi.LastIndexOf("\\") + 1));
            }
            foreach (string Fi in fileEntries)
            {
                File.Delete(Fi);
            }

            Log("I", string.Format("Archive End for Batch: {0} Run: {1}, Count: {2}", 
                                   strBatch, strRun, fileEntries.Length.ToString()));

        }


        /// <summary>
        /// Log messages
        /// </summary>
        /// <param name="level">Level, e.g. I, W, E</param>
        /// <param name="msg">Message text</param>
        private static void Log(string level, string msg)
        {
            Logger.Log(level, "ReportsApplication", msg);
        }


        /// <summary>
        /// Get parameter from Config table in DB
        /// </summary>
        /// <param name="parm">Parm value desired</param>
        /// <param name="def">default value if no parm</param>
        /// <returns>Parm value or default</returns>
        private static string GetParm(string parm, string def)
        {
            string value = string.Empty;
            string group = (UseTestDB) ? "PODFOReports.Test" : "PODFOReports";
            value = Conf.Get(group, parm);
            if (string.IsNullOrEmpty(value))
                value = def;
            return (value);
        }

    }

}
