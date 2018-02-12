Imports System.Configuration
Imports Utilities
Imports System.ComponentModel
Imports System.IO



Public Class Form1

    Private WithEvents WT As WorkerThread = New WorkerThread()
    Private Running As Boolean = False

    Private strBatch As String = ""
    Private strRun As String = ""
    Private testmode As Boolean = False

    ' Get connection string
    Private Conn As String = String.Empty

    ' Create DB Logger
    Private Log As Logging = Nothing

    Private refID As String = String.Empty



    ' Button Click does nothing
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    End Sub


    ' Form load
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ' Get the application's application domain.
        Dim currentDomain As AppDomain = AppDomain.CurrentDomain

        ' Define a handler for unhandled exceptions.
        AddHandler currentDomain.UnhandledException, AddressOf MYExnHandler

        ' Define a handler for unhandled exceptions for threads behind forms.
        AddHandler Application.ThreadException, AddressOf MYThreadHandler


        ' Check for -PROD or -TEST to select database
        For Each p As String In Environment.GetCommandLineArgs
            If (p.ToUpper() = "-TEST") Then testmode = True
            If (p.ToUpper() = "-PROD") Then testmode = False
        Next
        ConfigurationManager.AppSettings.Set("UseTestDB", If(testmode, "True", "False"))
        DbAccess.Close()

        Conn = DbAccess.GetConnectionString()
        Log = New Logging(Conn, "AppLog")
        Log.SourceBase = "PODFOAutomated"


        Try
            ' Show Version Number and EXE File date in Title Bar
            Dim Ver As String, BDate As String
            Ver = My.Application.Info.Version.ToString
            BDate = File.GetLastWriteTime(Reflection.Assembly.GetExecutingAssembly.Location).ToString()
            Me.Text = String.Format("PODFOAutomated - Version:{0}   File Date: {1}", Ver, BDate)
            Log.Info(Me.Text)

            ' Handle parms (-B xxx -R xxx [-TEST] [RefID=yyyyMMddHHmmssff])
            If (Environment.GetCommandLineArgs.Count <> 1) Then
                If (Environment.GetCommandLineArgs(1).ToString() = "-B") Then
                    strBatch = Environment.GetCommandLineArgs(2).ToString()
                End If
                If (Environment.GetCommandLineArgs(3).ToString() = "-R") Then
                    strRun = Environment.GetCommandLineArgs(4).ToString()
                End If
            End If

            ' Get Parm [RefID=yyyyMMddHHmmssff] if any, or default to Datetime.Now
            Dim cl As CmdLine = New CmdLine()
            cl.ParseCommandLineArgsForKVP()
            refID = cl.GetCmdLineArg("RefID")

            If (String.IsNullOrEmpty(refID)) Then
                refID = String.Format("{0:yyyyMMddHHmmssff}", DateTime.Now)
            End If

            Log.Info(String.Format("PODFO Automated Starting for Batch: {0}, Run: {1} in {2} mode, RefID: {3}",
                                   strBatch, strRun, If(testmode, "TEST", "PROD"), refID))

            ' If either the batch or run is missing, error and close the app
            If (String.IsNullOrEmpty(strBatch) Or (String.IsNullOrEmpty(strRun))) Then
                Dim s = "Missing Batch (-B) or Run (-R) in Application command line parameters"
                Log.Error(s)
                LogToFile(s)
                Close()
            End If

            WT.WorkerReportsProgress = True
            WT.WorkerSupportsCancellation = True
            ReportsApplication1.MainForm.WT = WT
            ReportsApplication1.MainForm.refID = refID

            lblBatch.Text = strBatch
            lblRun.Text = strRun
            lblDB.Text = If(testmode, "TEST", "PROD")
            lblStep.Text = String.Empty
            lblStatus.Text = "Initializing"
            Button1.Text = "Processing"

            'MsgBox(strBatch & strRun)
            'Dim dtDemo As DateTime = New DateTime(2014, 5, 10)
            'Set all fields to true so program will start from beginning.


            'If (strRun = "" And strBatch = "") Then
            '    PODFileProcessor.basImport.ProcessTodaysFiles(DateTime.Now, True, True, strBatch, strRun)
            'End If

            'If (strRun = "02") Then 'run will only be 02 if there are records for a second run
            '    ' MessageBox.Show("START MERGE")
            '    Dim form1 = New ReportsApplication1.MainForm(strBatch, "01", True, True, True, True, False, False)
            '    Dim form2 = New ReportsApplication1.MainForm(strBatch, "02", True, True, True, True, False, False)
            'Else
            '    Dim form1 = New ReportsApplication1.MainForm(strBatch, "01", True, True, True, True, False, False)
            'End If

            ' MessageBox.Show("Done")

        Catch ex As Exception
            Dim s = String.Format("Error: (0)", ex.Message)
            Log.Error(s)
            MessageBox.Show(s)

        Finally
        End Try

    End Sub


    Private Sub Form1_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        Dim success As Boolean = False

        Try
            Running = True
            WT.RunWorkerSync()
            Button1.Text = "Done"
            Log.Info("PODFO Automated Ending")
            Running = False
            success = True

        Catch ex As Exception
            success = False

        End Try

        ' 2015-04-14 JCH  Auto Close window upon completion
        If success Then
            Close()
        End If

    End Sub


    ' Custom background (UI) worker thread processing
    Private Sub ApplicationWork()

        ' Get the parm structure with a thread safe copy of UI control information
        'Dim parm As UIValues = CType(sender, UIValues)
        'WT.ReportProgress(-2, String.Format("ReRun Range"))
        'Dim form = New ReportsApplication1.MainForm(parm.PODBatch, parm.PODRunID, parm.CBNonWcPDFS,
        '                                            parm.cbWCpdfs, parm.tbStart, parm.tbEnd)


        ''MsgBox(strBatch & strRun)
        ''Dim dtDemo As DateTime = New DateTime(2014, 5, 10)
        ''Set all fields to true so program will start from beginning.

        Try
            ' Don't do this, moved to a SQL Job
            ' Execute sp_updatestats to optimize performance on PODFO database
            ' Once a week
            'If (DateTime.Now.DayOfWeek = DayOfWeek.Friday) Then
            '    Log.Info(String.Format("sp_updatestats Begining"))
            '    DbAccess.ExecuteNonQuery("sp_updatestats", CommandType.StoredProcedure)
            '    Log.Info(String.Format("sp_updatestats Ended"))
            'End If

            ''If (strRun = "" And strBatch = "") Then
            ''    PODFileProcessor.basImport.ProcessTodaysFiles(DateTime.Now, True, True, strBatch, strRun)
            ''End If

            If (strRun = "02") Then 'run will only be 02 if there are records for a second run(wc letters)
                ' MessageBox.Show("START MERGE")
                Dim form1 = New ReportsApplication1.MainForm(strBatch, "01", True, True, True, True, True, True)
                Dim form2 = New ReportsApplication1.MainForm(strBatch, "02", True, True, True, True, False, True)
            Else
                Dim form1 = New ReportsApplication1.MainForm(strBatch, "01", True, True, True, True, True, True)
            End If

            ' Create Summary Report and email it
            ReportsApplication1.clsGenerateLetters.CreateSummaryReportAndEmail(ReportsApplication1.clsGenerateLetters.refID, strBatch)


        Catch ex As Exception
            Dim s = String.Format("PODFO Automated ApplicationWork Exception: {0}", ex.Message)
            Log.Error(s)
            MessageBox.Show(s)

        Finally

        End Try

    End Sub


    ' Worker Thread completed - runs on worker thread, needs BeginInvoke
    Private Sub WT_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles WT.RunWorkerCompleted

        ' The background process is complete. We need to inspect
        ' our response to see if an error occurred, a cancel was
        ' requested or if we completed successfully.  
        If e.Cancelled Then
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Cancelled")
            lblStatus.Text = "Process Cancelled"

            ' Check to see if an error occurred in the background process.
        ElseIf e.Error IsNot Nothing Then
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Error while performing background operation")
            lblStatus.Text = "Error while performing background operation."
            MessageBox.Show(String.Format("Error in worker task: {0}", e.Error.Message))
        Else
            ' Everything completed normally.
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Completed")
            lblStatus.Text = "Process Completed"
        End If

        Running = False
        Application.DoEvents()


        'Change the status of the buttons on the UI accordingly
        'frmParent.BeginInvoke(Sub() ControlsEnable(True))
        ' BeginInvoke(Sub() ControlsEnable(True))
        'BeginInvoke(New Action(Of Boolean)(AddressOf ControlsEnable), True)
        'ControlsEnable(True)

    End Sub


    ' Worker Thread progress changed - runs on worker thread, BeginInvoke needed
    Private Sub WT_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles WT.ProgressChanged

        ' This method runs on the worker thread. 
        Dim s As String = CType(e.UserState, String)

        If (e.ProgressPercentage >= 0) Then
            pbProgress.Value = e.ProgressPercentage
            lblPercent.Text = String.Format("{0}%", e.ProgressPercentage)
        End If
        Application.DoEvents()

        If Not (String.IsNullOrEmpty(s)) Then
            Select Case e.ProgressPercentage
                Case -1                         ' Main Status Bar Text
                    lblStatus.Text = s
                Case -2                         ' Step
                    lblStep.Text = s
                Case -3                         ' R/M
                    lblRPM.Text = s
                Case Else                       ' Normal percent and all others
                    lblStatus.Text = s
            End Select
        End If
        Application.DoEvents()

    End Sub


    ' Worker Thread perform work, runs on the worker thread
    Private Sub WT_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles WT.DoWork
        ' This method runs on the background thread. 

        ' Get the BackgroundWorker object that raised this event. 
        Dim worker As WorkerThread
        worker = CType(sender, WorkerThread)

        ' Get the parm object and call the main method. 
        'Dim parm As UIValues = CType(e.Argument, UIValues)

        ' btnProcess_Click_Work(parm, e)

        'If (ReportsApplication1.clsGenerateLetters.BGWCanceled) Then
        '    e.Cancel = True
        'End If

        ApplicationWork()

        If (worker.CancellationPending) Then
            e.Cancel = True
        End If

    End Sub


    ' UI Cancel button clicked
    'Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click

    '    Dim dr As DialogResult = MessageBox.Show("Are you sure you want to Cancel", "Are you Sure", MessageBoxButtons.YesNo)
    '    If dr = DialogResult.Yes Then
    '        'BGW.CancelAsync()
    '        WT.CancelAsync()
    '    End If

    'End Sub


    Private Sub Form1_Closing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

    End Sub



    ' Application unhandled exception handler (catch all)
    Private Sub MYExnHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Dim EX As Exception
        EX = CType(e.ExceptionObject, Exception)

        LogToFile(DateTime.Now.ToString())
        LogToFile("Unhandled Exception Event")
        LogToFile(EX.Message)
        LogToFile(EX.StackTrace)
        If (EX.InnerException IsNot Nothing) Then
            LogToFile(EX.InnerException.Message)
            LogToFile(EX.InnerException.StackTrace)
        End If
    End Sub


    ' Application Thread unhandled exception handler (catch all)
    Private Sub MYThreadHandler(ByVal sender As Object, ByVal e As Threading.ThreadExceptionEventArgs)
        Dim EX As Exception
        EX = e.Exception

        LogToFile(DateTime.Now.ToString())
        LogToFile("Thread Exception Event")
        LogToFile(EX.Message)
        LogToFile(EX.StackTrace)
        If (EX.InnerException IsNot Nothing) Then
            LogToFile(EX.InnerException.Message)
            LogToFile(EX.InnerException.StackTrace)
        End If
    End Sub


    ' Write log msg string to file
    Public Sub LogToFile(strToWrite As String)
        Dim s = Date.Now.ToString() & " " & strToWrite
        Using writer As StreamWriter = New StreamWriter("c:\DeleteMe\PODFOMain.Critical.Log.txt", True)
            writer.WriteLine(strToWrite)
        End Using
    End Sub

End Class
