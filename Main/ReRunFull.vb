Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports Utilities
Imports System.ComponentModel


Public Class ReRunFull
    Inherits System.Windows.Forms.Form

    Dim frmParent As Main = Nothing
    Private WithEvents BGW As BackgroundWorker = New BackgroundWorker()
    Private UIState As UIValues = New UIValues()
    Private tabCal As Boolean = False
    Private BGWRunning As Boolean = False

    Private WithEvents WT As WorkerThread = New WorkerThread()

    ' Get connection string
    Private Conn As String = String.Empty

    ' Create DB Logger
    Private Log As Logging = Nothing

    ' Use Test DB flag
    Private UseTestDB As Boolean = False



    Structure UIValues

        Dim Tab As String
        Dim PODBatch As String
        Dim PODRunID As String
        Dim CreationDate As Date

        Dim CBImport As Boolean
        Dim cbBatch As Boolean
        Dim CBNonWcPDFS As Boolean
        Dim cbWCpdfs As Boolean
        Dim cbJobTicket As Boolean
        Dim cbMerge As Boolean
        Dim cbEmailReport As Boolean
        Dim cbMoveToProdcution As Boolean

        Dim cbCALENDARimport As Boolean
        Dim cbCALENDARnewBatch As Boolean
        Dim rerunCalendarSelectionStart As Date
        Dim cbCALENDARmakeNonWCpdfs As Boolean
        Dim cbCALENDARmakeWCpdfs As Boolean
        Dim cbCALENDARmerge As Boolean
        Dim cbCALENDARjobticket As Boolean
        Dim cbCALENDARemailReport As Boolean
        Dim cbCALENDARmoveToProduction As Boolean

    End Structure


    ' Copy UI values to Parm structure
    Private Sub GetUIValues(tab As String)

        UIState.Tab = tab

        UIState.PODBatch = dvBatches.SelectedRows(0).Cells("Batch").Value
        UIState.PODRunID = dvBatches.SelectedRows(0).Cells("Run").Value
        UIState.CreationDate = dvBatches.SelectedRows(0).Cells("File Date").Value
        UIState.CBImport = CBImport.Checked
        UIState.cbBatch = cbBatch.Checked
        UIState.CBNonWcPDFS = CBNonWcPDFS.Checked
        UIState.cbWCpdfs = cbWCpdfs.Checked
        UIState.cbJobTicket = cbJobTicket.Checked
        UIState.cbMerge = cbMerge.Checked
        UIState.cbEmailReport = cbEmailReport.Checked
        UIState.cbMoveToProdcution = cbMoveToProdcution.Checked

        UIState.cbCALENDARimport = cbCALENDARimport.Checked
        UIState.cbCALENDARnewBatch = cbCALENDARnewBatch.Checked
        UIState.rerunCalendarSelectionStart = rerunCalendar.SelectionStart
        UIState.cbCALENDARmakeNonWCpdfs = cbCALENDARmakeNonWCpdfs.Checked
        UIState.cbCALENDARmakeWCpdfs = cbCALENDARmakeWCpdfs.Checked
        UIState.cbCALENDARmerge = cbCALENDARmerge.Checked
        UIState.cbCALENDARjobticket = cbCALENDARjobticket.Checked
        UIState.cbCALENDARemailReport = cbCALENDARemailReport.Checked
        UIState.cbCALENDARmoveToProduction = cbCALENDARmoveToProduction.Checked

    End Sub



    ' On form loading
    Private Sub ReRunFull_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

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

        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB

        Log = New Logging(Conn, "AppLog")
        Log.SourceBase = String.Format("ReRunFull")


        Try
            cbBatch.Enabled = False
            CBImport.Enabled = False
            cbCALENDARimport.Enabled = False
            cbCALENDARnewBatch.Enabled = False
            cbCALENDARsort.Enabled = False

            'Dim conn As New CONNECTION
            Dim myAdapter As New SqlDataAdapter("SELECT  PODBatch AS 'Batch', PODRunID AS 'Run', FileDate as 'File Date', CreationDate AS 'Creation Date', LetterCount AS 'Letter Count', " +
                    "       MustMailDate, MailDate, PrintDate, StatusSentDate " +
                    "FROM PODBatch WHERE (Deleted IS NULL OR Deleted <> 'Y') ORDER  BY PODBatchID DESC ", DbAccess.GetConnection())
            Dim mySqlCommandBuilder As New SqlCommandBuilder(myAdapter)
            Dim myDataSet As New DataSet

            'fill the dataset
            myAdapter.Fill(myDataSet)
            dvBatches.DataSource = myDataSet.Tables(0)

            If (myDataSet.Tables(0).Rows.Count() = 0) Then
                MessageBox.Show("No Data found in PODBatch")
            End If
            'conn = Nothing
            myAdapter.Dispose()
            myDataSet.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "error")

        Finally
            'Cursor = cur
        End Try

        'MsgBox(Main.Text)

    End Sub

#Region "BGW Event Handlers"
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
            frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage)
        End If

        If Not (String.IsNullOrEmpty(s)) Then
            Select Case e.ProgressPercentage
                Case -1                         ' Main Status Bar Text
                    frmParent.ToolStripStatusLabel.Text = s
                Case -2                         ' ReRunFull Step
                    If (tabCal) Then
                        lblStepCal.Text = s
                    Else
                        lblStep.Text = s
                    End If
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
        If (parm.Tab = "GO") Then
            btnProcess_DoWork_Click(parm, e)
        ElseIf (parm.Tab = "GOCalendar") Then
            btnProcessCalanderReRun_DoWork_Click(parm, e)
        End If

        'If (ReportsApplication1.clsGenerateLetters.BGWCanceled) Then
        'e.Cancel = True
        'End If

        If (worker.CancellationPending) Then
            e.Cancel = True
        End If

    End Sub
#End Region




    ' Worker Thread completed - runs on worker thread, needs invoke
    Private Sub WT_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles WT.RunWorkerCompleted

        ' The background process is complete. We need to inspect
        ' our response to see if an error occurred, a cancel was
        ' requested or if we completed successfully.  
        If e.Cancelled Then
            frmParent.ToolStripStatusLabel.Text = "Process Cancelled"
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Cancelled")

            ' Check to see if an error occurred in the background process.
        ElseIf e.Error IsNot Nothing Then
            frmParent.ToolStripStatusLabel.Text = "Error while performing background operation."
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Error while performing background operation")
            MessageBox.Show(String.Format("Error in background task: {0}", e.Error.Message))
        Else
            ' Everything completed normally.
            frmParent.ToolStripStatusLabel.Text = "Process Completed"
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Completed")
        End If
        Application.DoEvents()

        BGWRunning = False

        'Change the status of the buttons on the UI accordingly
        'frmParent.BeginInvoke(Sub() ControlsEnable(True))
        ControlsEnable(True)
        Application.DoEvents()

    End Sub


    ' Worker Thread progress changed - runs on worker thread
    Private Sub WT_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles WT.ProgressChanged

        ' This method runs on the worker thread, invoke needed 
        Dim s As String = CType(e.UserState, String)

        If (e.ProgressPercentage >= 0) Then
            frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage)
            frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage)
            'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage))
        End If

        If Not (String.IsNullOrEmpty(s)) Then
            Select Case e.ProgressPercentage
                Case -1                         ' Main Status Bar Text
                    frmParent.ToolStripStatusLabel.Text = s
                    'frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = s)
                Case -2                         ' ReRunFull Step
                    If (tabCal) Then
                        lblStepCal.Text = s
                        'lblStepCal.BeginInvoke(Sub() lblStepCal.Text = s)
                    Else
                        lblStep.Text = s
                        'lblStep.BeginInvoke(Sub() lblStep.Text = s)
                    End If
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


    ' Worker Thread progress changed - runs on task worker thread, Invoke needed
    Private Sub WT_ProgressChangedInvoke(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles WT.ProgressChangedInvoke

        ' This method runs on the worker thread, invoke needed 
        Dim s As String = CType(e.UserState, String)

        If (e.ProgressPercentage >= 0) Then
            'frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage
            frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage)
            'frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage)
            frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage))
        End If

        If Not (String.IsNullOrEmpty(s)) Then
            Select Case e.ProgressPercentage
                Case -1                         ' Main Status Bar Text
                    'frmParent.ToolStripStatusLabel.Text = s
                    frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = s)
                Case -2                         ' ReRunFull Step
                    If (tabCal) Then
                        'lblStepCal.Text = s
                        lblStepCal.BeginInvoke(Sub() lblStepCal.Text = s)
                    Else
                        'lblStep.Text = s
                        lblStep.BeginInvoke(Sub() lblStep.Text = s)
                    End If
                Case -3
                    'frmParent.ToolStripRPM.Text = s
                    frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripRPM.Text = s)
                Case Else                       ' Normal percent and all others
                    'frmParent.ToolStripStatusLabel.Text = s
                    frmParent.StatusStrip.BeginInvoke(Sub() frmParent.ToolStripStatusLabel.Text = s)
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
        If (parm.Tab = "GO") Then
            btnProcess_DoWork_Click(parm, e)
        ElseIf (parm.Tab = "GOCalendar") Then
            btnProcessCalanderReRun_DoWork_Click(parm, e)
        End If

        'If (ReportsApplication1.clsGenerateLetters.BGWCanceled) Then
        'e.Cancel = True
        'End If

        If (worker.CancellationPending) Then
            e.Cancel = True
        End If

    End Sub



    ' UI Cancel button clicked
    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click,
                                                                                        btnProcessCalanderReRunCancel.Click
        Dim dr As DialogResult = MessageBox.Show("Are you sure you want to Cancel", "Are you Sure", MessageBoxButtons.YesNo)
        If dr = DialogResult.Yes Then
            'BGW.CancelAsync()
            WT.CancelAsync()
        End If
    End Sub


    ' Prevent this form from closing
    Private Sub ReRunFull_Closing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        If BGWRunning Then
            e.Cancel = True
        End If

    End Sub


    ' UI Go button clicked - Start background worker
    Private Sub btnProcess_Click(sender As System.Object, e As System.EventArgs) Handles btnProcess.Click

        'frmParent.SetText("Processing")
        GetUIValues("GO")
        tabCal = False
        ControlsEnable(False)

        Log.Info(String.Format("Starting ReRunFull App Version: {0}", Application.ProductVersion))

        ' Start the asynchronous operation.
        BGWRunning = True
        'BGW.RunWorkerAsync(UIState)
        'WT.RunWorkerAsync(UIState)
        WT.RunWorkerSync(UIState)
        'btnProcess_DoWork_Click(UIState, e)

    End Sub


    ' Custom background worker thread processing
    Private Sub btnProcess_DoWork_Click(sender As System.Object, e As System.EventArgs)

        ' Get the parm structure with a thread safe copy of UI control information
        Dim parm As UIValues = CType(sender, UIValues)

        ReportsApplication1.MainForm.refID = String.Format("{0:yyyyMMddHHmmssff}", DateTime.Now)

        If (Not String.IsNullOrEmpty(parm.PODBatch)) Then
            If (parm.CBImport = True OrElse parm.cbBatch = True) Then
                'DirectCast(Me.MdiParent, Main).SetText("ProcessTodaysFiles")
                'frmParent.ToolStripStatusLabel.Text = "ProcessTodaysFiles"
                'BGW.ReportProgress(-2, String.Format("ProcessTodaysFiles"))
                WT.ReportProgress(-2, String.Format("ProcessTodaysFiles"))

                PODFileProcessor.basImport.ProcessTodaysFiles(parm.CreationDate, parm.CBImport, parm.cbBatch, parm.PODBatch, parm.PODRunID)
            End If

            If (parm.CBNonWcPDFS = True) OrElse (parm.cbWCpdfs = True) OrElse (parm.cbMerge = True) OrElse (parm.cbJobTicket = True) _
                OrElse (parm.cbEmailReport = True) OrElse (parm.cbMoveToProdcution = True) Then

                'BGW.ReportProgress(-2, String.Format("ProcessReportsApplication"))
                WT.ReportProgress(-2, String.Format("ProcessReportsApplication"))

                'if there is a new batch we need to do both runs.
                If (parm.cbBatch = True And parm.PODRunID = "02") Then 'run will only be 02 if there are records for a second run
                    'DirectCast(Me.MdiParent, Main).SetText("Creating and Merging PDFs for 01")
                    'frmParent.ToolStripStatusLabel.Text = "Creating and Merging PDFs for 01"
                    Dim form = New ReportsApplication1.MainForm(parm.PODBatch, "01", parm.CBNonWcPDFS, parm.cbWCpdfs,
                                                                parm.cbMerge, parm.cbJobTicket, parm.cbEmailReport,
                                                                parm.cbMoveToProdcution)
                    'DirectCast(Me.MdiParent, Main).SetText("Creating and Merging PDFs for 02")
                    'frmParent.ToolStripStatusLabel.Text = "Creating and Merging PDFs for 02"
                    Dim form2 = New ReportsApplication1.MainForm(parm.PODBatch, "02", parm.CBNonWcPDFS, parm.cbWCpdfs,
                                                                 parm.cbMerge, parm.cbJobTicket, parm.cbEmailReport,
                                                                 parm.cbMoveToProdcution)
                Else
                    'frmParent.ToolStripStatusLabel.Text = "Creating PDFs"
                    Dim form2 = New ReportsApplication1.MainForm(parm.PODBatch, parm.PODRunID, parm.CBNonWcPDFS, parm.cbWCpdfs,
                                                                 parm.cbMerge, parm.cbJobTicket, parm.cbEmailReport,
                                                                 parm.cbMoveToProdcution)
                End If

                ' Create Summary Report and email it
                If parm.cbEmailReport Then
                    ReportsApplication1.clsGenerateLetters.CreateSummaryReportAndEmail(ReportsApplication1.clsGenerateLetters.refID, parm.PODBatch)
                End If

            End If

        End If

    End Sub


    ' UI Go button clicked - Start background worker
    Private Sub btnProcessCalanderReRun_Click(sender As System.Object, e As System.EventArgs) Handles btnProcessCalanderReRun.Click

        'frmParent.SetText("Processing")
        GetUIValues("GOCalendar")
        tabCal = True
        ControlsEnable(False)

        ' Start the asynchronous operation.
        BGWRunning = True
        'BGW.RunWorkerAsync(UIState)
        'WT.RunWorkerAsync(UIState)
        WT.RunWorkerSync(UIState)

        'btnProcessCalanderReRun_DoWork_Click(UIState, e)

    End Sub


    ' Custom background worker thread processing
    Private Sub btnProcessCalanderReRun_DoWork_Click(sender As System.Object, e As System.EventArgs)

        ' Get the parm structure with a thread safe copy of UI control information
        Dim parm As UIValues = CType(sender, UIValues)


        If (parm.cbCALENDARnewBatch) Then
            'DirectCast(Me.MdiParent, Main).SetText("ProcessTodaysFiles")
            PODFileProcessor.basImport.ProcessTodaysFiles(parm.rerunCalendarSelectionStart, parm.cbCALENDARimport,
                                                          parm.cbCALENDARnewBatch, parm.PODBatch, parm.PODRunID)

            If parm.PODRunID = "02" Then 'run will only be 02 if there are records for a second run
                'DirectCast(Me.MdiParent, Main).SetText("Creating and Merging PDFs for 01")
                Dim form = New ReportsApplication1.MainForm(parm.PODBatch, "01", parm.cbCALENDARmakeNonWCpdfs,
                                                            parm.cbCALENDARmakeWCpdfs, parm.cbCALENDARmerge,
                                                            parm.cbCALENDARjobticket, parm.cbCALENDARemailReport,
                                                            parm.cbCALENDARmoveToProduction)
                'DirectCast(Me.MdiParent, Main).SetText("Creating and Merging PDFs for 02")
                Dim form2 = New ReportsApplication1.MainForm(parm.PODBatch, "02", parm.cbCALENDARmakeNonWCpdfs,
                                                             parm.cbCALENDARmakeWCpdfs, parm.cbCALENDARmerge,
                                                             parm.cbCALENDARjobticket, parm.cbCALENDARemailReport,
                                                             parm.cbCALENDARmoveToProduction)
            Else
                'DirectCast(Me.MdiParent, Main).SetText("Creating and Merging PDFs for 01")
                Dim form = New ReportsApplication1.MainForm(parm.PODBatch, "01", parm.cbCALENDARmakeNonWCpdfs,
                                                            parm.cbCALENDARmakeWCpdfs, parm.cbCALENDARmerge,
                                                            parm.cbCALENDARjobticket, parm.cbCALENDARemailReport,
                                                            parm.cbCALENDARmoveToProduction)
            End If

            ' Create Summary Report and email it
            If parm.cbEmailReport Then
                ReportsApplication1.clsGenerateLetters.CreateSummaryReportAndEmail(ReportsApplication1.clsGenerateLetters.refID, parm.PODBatch)
            End If

        Else
            'MessageBox.Show("Must create a new batch in order to process from the Calendar. " &
            ' "If the job has already been processed use the gridview or check new batch.")
        End If

    End Sub


    ' enable or disable UI controls
    Private Sub ControlsEnable(enable As Boolean)
        If (Not enable) Then
            frmParent.ToolsToolStripMenuItem.Enabled = False
            btnProcess.Enabled = False
            btnCancel.Enabled = True
            btnProcessCalanderReRunCancel.Enabled = True
            btnProcessCalanderReRun.Enabled = False
            CBImport.Enabled = False
            cbBatch.Enabled = False
            CBNonWcPDFS.Enabled = False
            cbWCpdfs.Enabled = False
            cbMerge.Enabled = False
            cbJobTicket.Enabled = False
            cbEmailReport.Enabled = False
            cbMoveToProdcution.Enabled = False
            cbCALENDARimport.Enabled = False
            cbCALENDARnewBatch.Enabled = False
            cbCALENDARmakeNonWCpdfs.Enabled = False
            cbCALENDARmakeWCpdfs.Enabled = False
            cbCALENDARmerge.Enabled = False
            cbCALENDARjobticket.Enabled = False
            cbCALENDARemailReport.Enabled = False
            cbCALENDARmoveToProduction.Enabled = False
            frmParent.cbProdTest.Enabled = False
        Else
            frmParent.ToolsToolStripMenuItem.Enabled = True
            btnProcess.Enabled = True
            btnCancel.Enabled = False
            btnProcessCalanderReRunCancel.Enabled = False
            btnProcessCalanderReRun.Enabled = True
            CBImport.Enabled = True
            cbBatch.Enabled = True
            CBNonWcPDFS.Enabled = True
            cbWCpdfs.Enabled = True
            cbMerge.Enabled = True
            cbJobTicket.Enabled = True
            cbEmailReport.Enabled = True
            cbMoveToProdcution.Enabled = True
            cbCALENDARimport.Enabled = True
            cbCALENDARnewBatch.Enabled = True
            cbCALENDARmakeNonWCpdfs.Enabled = True
            cbCALENDARmakeWCpdfs.Enabled = True
            cbCALENDARmerge.Enabled = True
            cbCALENDARjobticket.Enabled = True
            cbCALENDARemailReport.Enabled = True
            cbCALENDARmoveToProduction.Enabled = True
            frmParent.cbProdTest.Enabled = True
        End If
    End Sub

   
End Class

