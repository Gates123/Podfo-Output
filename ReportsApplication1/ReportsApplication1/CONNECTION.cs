using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ReportsApplication1
{
    
    class CONNECTION
    {
        private string mConnectionString = "Data Source=usasql;Initial Catalog=PODFOLIVE;Integrated Security=True;";
        public void New(string ConnectionString)
        {
            mConnectionString = ConnectionString;
        }

        public static void New() 
        {
            //mConnectionString = "Provider=SQLOLEDB;Data Source=COBDATA1;Initial Catalog=IEQNEW;Integrated Security=SSPI;"
            //        mConnectionString = "Data Source=COBDATA1;Initial Catalog=IEQNEW;Integrated Security=SSPI;"
            //if (TestMode == true) {
            //    mConnectionString = "Server=COBDATA1;Database=CELLTEST;User ID=ieq;Password=Ramius123;Trusted_Connection=False";
            //} else {
            //    mConnectionString = "Server=COBDATA1;Database=CELLERLAW;User ID=ieq;Password=Ramius123;Trusted_Connection=False";
            //}
        }

        public System.Data.SqlClient.SqlConnection getConnection()
        {
            SqlConnection oConnection = new SqlConnection(mConnectionString);
            oConnection.Open();
            return oConnection;
        }

    }
}
