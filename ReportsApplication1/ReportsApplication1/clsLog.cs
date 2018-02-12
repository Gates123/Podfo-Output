using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Utilities;


namespace ReportsApplication1
{
    
    public class clsLog
    {
        public string mstrLogFileLocation = "";
        public static string mstrRootLogFileLocation = "\\\\Cobmain\\usacms\\PODFO\\Output\\Log\\";


        // Get connection string
        private static string Conn = null;
        // Create DB Logger
        private static Logging Logger = null;
        // ConfigTable Access
        private static ConfigTable Conf = null;

        // Use Test DB flag
        private static bool UseTestDB = false;

        

        /// <summary>
        /// Constructor
        /// </summary>
        static clsLog()
        {
            Conn = DbAccess.GetConnectionString();
            Conf = new ConfigTable(Conn);
            Logger = new Logging(Conn, "AppLog");
            UseTestDB = DbAccess.UseTestDB;

            mstrRootLogFileLocation = GetParm("LogFileFolder", "\\\\Cobmain\\usacms\\PODFO\\Output\\Log\\");
        }


        public void WriteToLogfile(string strToWrite)
        {
            StreamWriter wrLog;

            wrLog = File.AppendText(mstrRootLogFileLocation + mstrLogFileLocation);
            wrLog.WriteLine(strToWrite);
            wrLog.Flush();
            wrLog.Close();

            Log("V", strToWrite);
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
