using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Utilities;
using System.ComponentModel;


namespace ReportsApplication1
{
   public class clsGetBatchSort
   {
       //CONNECTION con = new CONNECTION();

       // Get connection string
       private static string Conn = null;
       // Create DB Logger
       private static Logging Logger = null;
       // ConfigTable Access
       private static ConfigTable Conf = null;

       // Use Test DB flag
       private static bool UseTestDB = false;

       //public static BackgroundWorker BGW = null;
       public static WorkerThread WT = null;

       
        /// <summary>
        /// Constructor
        /// </summary>
       static clsGetBatchSort()
        {
            Conn = DbAccess.GetConnectionString();
            Conf = new ConfigTable(Conn);
            Logger = new Logging(Conn, "AppLog");
            UseTestDB = DbAccess.UseTestDB;

            //mstrFilePath = GetParm("mstrFilePath", "\\\\Cobmain\\usacms\\PODFO\\Output\\IndividualPDFs");
        }



       public SqlCommand dsBatch_Sort(string batch, string run)
        {
            try
            {
                //string mConnectionString = "Data Source=usasql;Initial Catalog=PODFO;Integrated Security=True;";
                //SqlConnection conn = new SqlConnection(mConnectionString);
                //conn.Open();
                SqlConnection conn = DbAccess.GetConnection();
                //Get all rows from the table based on the batch and run
                //SqlCommand cmd = new SqlCommand("SELECT dbo.PODMailingInfo.* FROM  dbo.PODMailingInfo WHERE (PODBatchID = " + txtBatch.Text + ") AND (PODRunID = '" + txtRun.Text + "') order by MPresortID", conn);

                SqlCommand cmd = new SqlCommand("USP_Select_Batch_Address_To_Sort_all");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@P_PODBatchID", batch);
                cmd.Parameters.AddWithValue("@P_PODRunID", run);

                return (cmd);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in dsBatch_sort", ex);
                
            }
        }

        public SqlCommand dsBatch_Sort(string batch, string run, int intStart, int intEnd)
        {
            try
            {
                //string mConnectionString = "Data Source=usasql;Initial Catalog=PODFO;Integrated Security=True;";
                //SqlConnection conn = new SqlConnection(mConnectionString);
                //conn.Open();
                SqlConnection conn = DbAccess.GetConnection();
                //Get all rows from the table based on the batch and run
                //SqlCommand cmd = new SqlCommand("SELECT dbo.PODMailingInfo.* FROM  dbo.PODMailingInfo WHERE (PODBatchID = " + txtBatch.Text + ") AND (PODRunID = '" + txtRun.Text + "') order by MPresortID", conn);
                
                SqlCommand cmd = new SqlCommand("USP_Select_Batch_Address_To_Sort_Range");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@P_PODBatchID", batch);
                cmd.Parameters.AddWithValue("@P_PODRunID", run);
                cmd.Parameters.AddWithValue("@P_Start_MPresortID", intStart);
                cmd.Parameters.AddWithValue("@P_End_MPresortID", intEnd);
                cmd.CommandTimeout = 0;
                return (cmd);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in dsBatch_sort", ex);

            }
        }

        public SqlCommand dsBatch_Sort(string batch, string run, string strLetterType)
        {
            try
            {
                //string mConnectionString = "Data Source=usasql;Initial Catalog=PODFO;Integrated Security=True;";
                //SqlConnection conn = new SqlConnection(mConnectionString);
                //conn.Open();
                SqlConnection conn = DbAccess.GetConnection();
                //Get all rows from the table based on the batch and run
                //SqlCommand cmd = new SqlCommand("SELECT dbo.PODMailingInfo.* FROM  dbo.PODMailingInfo WHERE (PODBatchID = " + txtBatch.Text + ") AND (PODRunID = '" + txtRun.Text + "') order by MPresortID", conn);
                
                SqlCommand cmd = new SqlCommand("USP_Select_Batch_Address_To_Sort_LetterType");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@P_PODBatchID", batch);
                cmd.Parameters.AddWithValue("@P_PODRunID", run);
                cmd.Parameters.AddWithValue("@P_LetterType", strLetterType);

                return (cmd);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in dsBatch_sort", ex);

            }
        }

        public SqlCommand dsGet_MailDate(string batch, string run)
        {
            try
            {
                //string mConnectionString = "Data Source=usasql;Initial Catalog=PODFO;Integrated Security=True;";
                //SqlConnection conn = new SqlConnection(mConnectionString);
                //conn.Open();
                SqlConnection conn = DbAccess.GetConnection();
                //Get all rows from the table based on the batch and run
                //SqlCommand cmd = new SqlCommand("SELECT dbo.PODMailingInfo.* FROM  dbo.PODMailingInfo WHERE (PODBatchID = " + txtBatch.Text + ") AND (PODRunID = '" + txtRun.Text + "') order by MPresortID", conn);
                
                SqlCommand cmd = new SqlCommand("USP_SELECT_POD_MAILBYDATE");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@P_PODBatch", batch);
                cmd.Parameters.AddWithValue("@P_PODRunID", run);
                

                return (cmd);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in dsBatch_sort", ex);

            }
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
