using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace ProcessWorker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Define a handler for unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += MYExnHandler;
            // Define a handler for unhandled exceptions for threads behind forms.
            Application.ThreadException += MYThreadHandler;

            //LogToFile("ProcessWorker:Main Starting");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ProcessWorker());
        }


        // Application unhandled exception handler (catch all)
        private static void MYExnHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception EX = (Exception)e.ExceptionObject;

            LogToFile(DateTime.Now.ToString());
            LogToFile("Unhandled Exception Event");
            LogToFile(EX.Message);
            LogToFile(EX.StackTrace);
            if (EX.InnerException != null)
            {
                LogToFile(EX.InnerException.Message);
                LogToFile(EX.InnerException.StackTrace);
            }
        }


        // Application Thread unhandled exception handler (catch all)
        private static void MYThreadHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception EX = (Exception)e.Exception;

            LogToFile(DateTime.Now.ToString());
            LogToFile("Thread Exception Event");
            LogToFile(EX.Message);
            LogToFile(EX.StackTrace);
            if (EX.InnerException != null)
            {
                LogToFile(EX.InnerException.Message);
                LogToFile(EX.InnerException.StackTrace);
            }
        }


        // Write log msg string to file
        private static void LogToFile(string strToWrite)
        {
            var s = DateTime.Now.ToString() + " " + strToWrite;
            using (StreamWriter writer = new StreamWriter("c:\\ProcessWorker.Critical.Log.txt", true))
            {
                writer.WriteLine(strToWrite);
            }
        }
    }
}
