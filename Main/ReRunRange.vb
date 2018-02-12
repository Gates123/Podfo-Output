Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports Utilities
Imports System.ComponentModel
Imports System.Configuration



Public Class ReRunRange

    Dim frmParent As Main = Nothing
    Private WithEvents BGW As BackgroundWorker = New BackgroundWorker()
    Private UIState As UIValues = New UIValues()
    Private BGWRunning As Boolean = False

    Private WithEvents WT As WorkerThread = New WorkerThread()


    Private cmdParms As CmdLine = New CmdLine()

    'Cmdline parms
    Private workerNumber As String = String.Empty
    Private start As String = String.Empty
    Private endnum As String = String.Empty
    Private batch As String = String.Empty
    Private run As String = String.Empty
    Private letterType As String = String.Empty
    Private testProd As String = String.Empty
    Private closeType As String = String.Empty

    Private autoclose As Boolean = False
    Private autoCloseInvoked As Boolean = False



    Structure UIValues
        Dim PODBatch As String
        Dim PODRunID As String
        Dim CBNonWcPDFS As Boolean
        Dim cbWCpdfs As Boolean
        Dim tbStart As String
        Dim tbEnd As String
    End Structure


    ' Copy UI values to Parm structure
    Private Sub GetUIValues()

        UIState.PODBatch = dvBatches.SelectedRows(0).Cells("PODBatch").Value
        UIState.PODRunID = dvBatches.SelectedRows(0).Cells("PODRunID").Value
        UIState.CBNonWcPDFS = CBNonWcPDFS.Checked
        UIState.cbWCpdfs = cbWCpdfs.Checked
        UIState.tbStart = tbStart.Text
        UIState.tbEnd = tbEnd.Text

    End Sub


    ' On form loading
    Private Sub ReRunRange_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        'Dim cur As Cursor = Cursor
        'Cursor = Cursors.WaitCursor
        frmParent = DirectCast(Me.MdiParent, Main)
        lblStep.Text = String.Empty

        'BGW.WorkerReportsProgress = True
        'BGW.WorkerSupportsCancellation = True
        'ReportsApplication1.MainForm.BGW = BGW

        WT.WorkerReportsProgress = True
        WT.WorkerSupportsCancellation = True
        ReportsApplication1.MainForm.WT = WT
        ReportsApplication1.MainForm.refID = String.Format("{0:yyyyMMddHHmmssff}", DateTime.Now)


        'Get cmdline parms
        'cmdParms.ParseCommandLineArgs()
        ' Fetch cmdline args as a string array
        Dim sa As String() = Environment.GetCommandLineArgs()
        ' If not commented out, parse key=value parms from the command line
       
        If (sa.Count > 1) Then


            If sa(0) <> "//" And sa(1) <> "//" Then
                cmdParms.ParseCommandLineArgsForKVP()
                workerNumber = cmdParms.GetCmdLineArg("Num")
                batch = cmdParms.GetCmdLineArg("Batch")
                run = cmdParms.GetCmdLineArg("Run")
                start = cmdParms.GetCmdLineArg("Start")
                endnum = cmdParms.GetCmdLineArg("End")
                letterType = cmdParms.GetCmdLineArg("Letter")
                testProd = cmdParms.GetCmdLineArg("TestProd")
                closeType = cmdParms.GetCmdLineArg("Close")
                If (closeType = "Auto") Then
                    autoclose = True
                End If
                ' Handle TEST / PROD mode
                If (Not String.IsNullOrEmpty(testProd)) Then
                    Dim s As String = If(testProd = "PROD", "False", "True")
                    ConfigurationManager.AppSettings.Set("UseTestDB", s)
                End If

                ' Above code sets this
                'Set connection string in Application settings
                'My.Settings.Default["PODFOConnectionString"] = DbAccess.GetConnectionString()

                DbAccess.Close()

                ' Start after form is shown
            End If
        End If




        Try
            'Dim conn As New CONNECTION
            'Dim myAdapter As New SqlDataAdapter("SELECT PODBatch, PODRunID, CreationDate, LetterCount, ImageCount, " &
            '                                    "MustMailDate, MailDate, PrintDate, StatusSentDate " &
            '                                    " FROM     PODBatch order by PODBatchID desc ", DbAccess.GetConnection())
            Dim myAdapter As New SqlDataAdapter("SELECT PODBatch, PODRunID, CreationDate, LetterCount, ImageCount, " &
                                                "MustMailDate, MailDate, PrintDate, StatusSentDate " &
                                                " FROM PODBatch ORDER BY PODBatchID desc ", DbAccess.GetConnection())
            Dim mySqlCommandBuilder As New SqlCommandBuilder(myAdapter)
            Dim myDataSet As New DataSet

            'fill the dataset
            myAdapter.Fill(myDataSet)
            dvBatches.DataSource = myDataSet.Tables(0)

            'conn = Nothing
            myAdapter.Dispose()
            myDataSet.Dispose()

        Catch ex As Exception
            MessageBox.Show("Exception: {0}", ex.Message)
        Finally
            'Cursor = cur
        End Try

    End Sub


#Region "BGW Events"
    ' Background Worker completed
    Private Sub BGW_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BGW.RunWorkerCompleted

        ' The background process is complete. We need to inspect
        ' our response to see if an error occurred, a cancel was
        ' requested or if we completed successfully.  
        If e.Cancelled Then
            frmParent.ToolStripStatusLabel.Text = "Process Cancelled"

            ' Check to see if an error occurred in the background process.
        ElseIf e.Error IsNot Nothing Then
            frmParent.ToolStripStatusLabel.Text = "Error while performing background operation."
            MessageBox.Show(String.Format("Error in background task: {0}", e.Error.Message))
        Else
            ' Everything completed normally.
            frmParent.ToolStripStatusLabel.Text = "Process Completed"
        End If

        BGWRunning = False

        'Change the status of the buttons on the UI accordingly
        ControlsEnable(True)

    End Sub


    ' Background worker progress changed
    Private Sub BGW_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles BGW.ProgressChanged

        ' This method runs on the main UI thread. 
        Dim s As String = CType(e.UserState, String)

        If (e.ProgressPercentage >= 0) Then
            frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage
        End If

        If Not (String.IsNullOrEmpty(s)) Then
            Select Case e.ProgressPercentage
                Case -1                         ' Main Status Bar Text
                    frmParent.ToolStripStatusLabel.Text = s
                Case -2                         ' Step
                    lblStep.Text = s
                Case -3
                    frmParent.ToolStripRPM.Text = s
                Case Else                       ' Normal percent and all others
                    frmParent.ToolStripStatusLabel.Text = s
            End Select
        End If

    End Sub


    ' Background worker perform work on background thread
    Private Sub BGW_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BGW.DoWork
        ' This method runs on the background thread. 

        ' Get the BackgroundWorker object that raised this event. 
        Dim worker As BackgroundWorker
        worker = CType(sender, BackgroundWorker)

        ' Get the parm object and call the main method. 
        Dim parm As UIValues = CType(e.Argument, UIValues)

        btnProcess_Click_Work(parm, e)

        'If (ReportsApplication1.clsGenerateLetters.BGWCanceled) Then
        '    e.Cancel = True
        'End If

        If (worker.CancellationPending) Then
            e.Cancel = True
        End If

    End Sub
#End Region



    ' Worker Thread completed - runs on worker thread, needs BeginInvoke
    Private Sub WT_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles WT.RunWorkerCompleted

        ' The background process is complete. We need to inspect
        ' our response to see if an error occurred, a cancel was
        ' requested or if we completed successfully.  
        If e.Cancelled Then
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Cancelled")
            frmParent.ToolStripStatusLabel.Text = "Process Cancelled"

            ' Check to see if an error occurred in the background process.
        ElseIf e.Error IsNot Nothing Then
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Error while performing background operation")
            frmParent.ToolStripStatusLabel.Text = "Error while performing background operation."
            MessageBox.Show(String.Format("Error in background task: {0}", e.Error.Message))
        Else
            ' Everything completed normally.
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Completed")
            frmParent.ToolStripStatusLabel.Text = "Process Completed"
        End If

        BGWRunning = False
        Application.DoEvents()


        'Change the status of the buttons on the UI accordingly
        'frmParent.BeginInvoke(Sub() ControlsEnable(True))
        ' BeginInvoke(Sub() ControlsEnable(True))
        'BeginInvoke(New Action(Of Boolean)(AddressOf ControlsEnable), True)
        ControlsEnable(True)

    End Sub


    ' Worker Thread progress changed - runs on worker thread, BeginInvoke needed
    Private Sub WT_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles WT.ProgressChanged

        ' This method runs on the worker thread. 
        Dim s As String = CType(e.UserState, String)

        If (e.ProgressPercentage >= 0) Then
            frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage
            frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage)
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage)
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage))
        End If
        Application.DoEvents()

        If Not (String.IsNullOrEmpty(s)) Then
            Select Case e.ProgressPercentage
                Case -1                         ' Main Status Bar Text
                    frmParent.ToolStripStatusLabel.Text = s
                    'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = s)
                Case -2                         ' Step
                    lblStep.Text = s
                    'lblStep.BeginInvoke(Sub() lblStep.Text = s)
                Case -3
                    frmParent.ToolStripRPM.Text = s
                    'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripRPM.Text = s)
                Case Else                       ' Normal percent and all others
                    frmParent.ToolStripStatusLabel.Text = s
                    'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = s)
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
        Dim parm As UIValues = CType(e.Argument, UIValues)

        btnProcess_Click_Work(parm, e)

        'If (ReportsApplication1.clsGenerateLetters.BGWCanceled) Then
        '    e.Cancel = True
        'End If

        If (worker.CancellationPending) Then
            e.Cancel = True
        End If

    End Sub



    ' UI Go button clicked - Start background worker
    Private Sub btnProcess_Click(sender As System.Object, e As System.EventArgs) Handles btnProcess.Click

        'frmParent.SetText("Processing")
        GetUIValues()
        ControlsEnable(False)

        ' Start the asynchronous operation.
        BGWRunning = True
        'BGW.RunWorkerAsync(UIState)
        'WT.RunWorkerAsync(UIState)
        WT.RunWorkerSync(UIState)

    End Sub


    ' Custom background worker thread processing
    Private Sub btnProcess_Click_Work(sender As System.Object, e As System.EventArgs)

        ' Get the parm structure with a thread safe copy of UI control information
        Dim parm As UIValues = CType(sender, UIValues)

        'BGW.ReportProgress(-2, String.Format("ReRun Range"))
        WT.ReportProgress(-2, String.Format("ReRun Range"))

        Dim form = New ReportsApplication1.MainForm(parm.PODBatch, parm.PODRunID, parm.CBNonWcPDFS,
                                                    parm.cbWCpdfs, parm.tbStart, parm.tbEnd)

    End Sub


    ' UI Cancel button clicked
    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click

        Dim dr As DialogResult = MessageBox.Show("Are you sure you want to Cancel", "Are you Sure", MessageBoxButtons.YesNo)
        If dr = DialogResult.Yes Then
            'BGW.CancelAsync()
            WT.CancelAsync()
        End If

    End Sub


    ' Prevent this form from closing
    Private Sub ReRunRange_Closing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If BGWRunning Then
            e.Cancel = True
        End If

    End Sub


    ' enable or disable UI controls
    Private Sub ControlsEnable(enable As Boolean)
        If (Not enable) Then
            frmParent.ToolsToolStripMenuItem.Enabled = False
            btnProcess.Enabled = False
            btnCancel.Enabled = True
            CBNonWcPDFS.Enabled = False
            cbWCpdfs.Enabled = False
            tbStart.Enabled = False
            tbEnd.Enabled = False
            frmParent.cbProdTest.Enabled = False
        Else
            frmParent.ToolsToolStripMenuItem.Enabled = True
            btnProcess.Enabled = True
            btnCancel.Enabled = False
            CBNonWcPDFS.Enabled = True
            cbWCpdfs.Enabled = True
            tbStart.Enabled = True
            tbEnd.Enabled = True
            frmParent.cbProdTest.Enabled = True
        End If
    End Sub


End Class