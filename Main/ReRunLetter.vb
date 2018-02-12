Imports System.Data.SqlClient
Imports System.IO
Imports System.Data.OleDb
Imports System.Data
Imports System.ComponentModel

Imports Utilities

Public Class ReRunLetter

    Dim frmParent As Main = Nothing
    Private WithEvents BGW As BackgroundWorker = New BackgroundWorker()
    Private UIState As UIValues = New UIValues()
    Private BGWRunning As Boolean = False

    Private WithEvents WT As WorkerThread = New WorkerThread()


    Structure UIValues

        Dim PODBatch As String
        Dim PODRunID As String
        Dim rbACO As Boolean
        Dim rbWC As Boolean
        Dim rbMBP As Boolean
        Dim rbENT As Boolean
        Dim rbDIS As Boolean
        Dim rbCPC As Boolean
        Dim mstrLetterType As String

    End Structure


    ' Copy UI values to Parm structure
    Private Sub GetUIValues()

        UIState.PODBatch = dvBatches.SelectedRows(0).Cells("PODBatch").Value
        UIState.PODRunID = dvBatches.SelectedRows(0).Cells("PODRunID").Value
        UIState.rbACO = rbACO.Checked
        UIState.rbWC = rbWC.Checked
        UIState.rbMBP = rbMBP.Checked
        UIState.rbENT = rbENT.Checked
        UIState.rbDIS = rbDIS.Checked
        UIState.rbCPC = rbCPC.Checked

    End Sub


    ' On form loading
    Private Sub ReRunLetter_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Dim cur As Cursor = Cursor
        ' Cursor = Cursors.WaitCursor
        frmParent = DirectCast(Me.MdiParent, Main)
        lblStep.Text = String.Empty

        'BGW.WorkerReportsProgress = True
        'BGW.WorkerSupportsCancellation = True
        'ReportsApplication1.MainForm.BGW = BGW

        WT.WorkerReportsProgress = True
        WT.WorkerSupportsCancellation = True
        ReportsApplication1.MainForm.WT = WT
        ReportsApplication1.MainForm.refID = String.Format("{0:yyyyMMddHHmmssff}", DateTime.Now)



        Try
            Dim myAdapter As New SqlDataAdapter("SELECT PODBatch, PODRunID, CreationDate, LetterCount, ImageCount, MustMailDate, MailDate, PrintDate, StatusSentDate " &
                                                " FROM     PODBatch order by PODBatchID desc ", DbAccess.GetConnection())
            Dim mySqlCommandBuilder As New SqlCommandBuilder(myAdapter)
            Dim myDataSet As New DataSet

            'fill the dataset
            myAdapter.Fill(myDataSet)
            dvBatches.DataSource = myDataSet.Tables(0)

            'conn = Nothing
            myAdapter.Dispose()
            'Dim conn As New CONNECTION
            myDataSet.Dispose()

        Catch ex As Exception
            MessageBox.Show("Exception: {0}", ex.Message)
        Finally
            'Cursor = cur
        End Try
    End Sub


#Region "BGW Event Handler"
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

        btnProcess_Click_1_Work(parm, e)

        'If (ReportsApplication1.clsGenerateLetters.BGWCanceled) Then
        '    e.Cancel = True
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
            'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Cancelled")
            frmParent.ToolStripStatusLabel.Text = "Process Cancelled"

            ' Check to see if an error occurred in the background process.
        ElseIf e.Error IsNot Nothing Then
            'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripStatusLabel.Text = "Error while performing background operation")
            frmParent.ToolStripStatusLabel.Text = "Error while performing background operation"
            MessageBox.Show(String.Format("Error in background task: {0}", e.Error.Message))
        Else
            ' Everything completed normally.
            'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripStatusLabel.Text = "Process Completed")
            frmParent.ToolStripStatusLabel.Text = "Process Completed"
        End If

        BGWRunning = False
        Application.DoEvents()

        'Change the status of the buttons on the UI accordingly
        'frmParent.Invoke(Sub() ControlsEnable(True))
        ControlsEnable(True)

    End Sub


    ' Worker Thread progress changed - runs on worker thread, Invoke needed
    Private Sub WT_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles WT.ProgressChanged

        ' This method runs on the main UI thread. 
        Dim s As String = CType(e.UserState, String)

        If (e.ProgressPercentage >= 0) Then
            'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage)
            frmParent.ToolStripProgressBar1.Value = e.ProgressPercentage
            'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage))
            frmParent.ToolStripPercent.Text = String.Format("{0}%", e.ProgressPercentage)
        End If
        Application.DoEvents()

        If Not (String.IsNullOrEmpty(s)) Then
            Select Case e.ProgressPercentage
                Case -1                         ' Main Status Bar Text
                    'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripStatusLabel.Text = s)
                    frmParent.ToolStripStatusLabel.Text = s
                Case -2                         ' Step
                    'lblStep.Invoke(Sub() lblStep.Text = s)
                    lblStep.Text = s
                Case -3
                    'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripRPM.Text = s)
                    frmParent.ToolStripRPM.Text = s
                Case Else                       ' Normal percent and all others
                    'frmParent.StatusStrip.Invoke(Sub() frmParent.ToolStripStatusLabel.Text = s)
                    frmParent.ToolStripStatusLabel.Text = s
            End Select
        End If
        Application.DoEvents()

    End Sub


    ' Worker Thread perform work, runs on the worker thread
    Private Sub WT_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles WT.DoWork
        ' This method runs on the background thread. 

        ' Get the WorkerThread object that raised this event. 
        Dim worker As WorkerThread
        worker = CType(sender, WorkerThread)

        ' Get the parm object and call the main method. 
        Dim parm As UIValues = CType(e.Argument, UIValues)

        btnProcess_Click_1_Work(parm, e)

        'If (ReportsApplication1.clsGenerateLetters.BGWCanceled) Then
        '    e.Cancel = True
        'End If

        If (worker.CancellationPending) Then
            e.Cancel = True
        End If

    End Sub


    ' Go Clicked on form
    Private Sub btnProcess_Click_1(sender As System.Object, e As System.EventArgs) Handles btnProcess.Click

        'frmParent.SetText("Processing")
        GetUIValues()
        ControlsEnable(False)

        ' Start the asynchronous operation.
        BGWRunning = True
        'BGW.RunWorkerAsync(UIState)
        'WT.RunWorkerAsync(UIState)
        WT.RunWorkerSync(UIState)

    End Sub


    ' Custom Worker thread Processing
    Private Sub btnProcess_Click_1_Work(sender As System.Object, e As System.EventArgs)

        ' Get the parm structure with a thread safe copy of UI control information
        Dim parm As UIValues = CType(sender, UIValues)

        'BGW.ReportProgress(-2, String.Format("ReRun Letter"))
        WT.ReportProgress(-2, String.Format("ReRun Letter"))

        Dim form = New ReportsApplication1.MainForm(parm.PODBatch, parm.PODRunID, parm.mstrLetterType)

        'MsgBox("Done")

    End Sub


    ' Letter Type radio button clicked to change selection
    Private Sub rb_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rbACO.CheckedChanged,
                                                                                          rbWC.CheckedChanged,
                                                                                          rbMBP.CheckedChanged,
                                                                                          rbENT.CheckedChanged,
                                                                                          rbDIS.CheckedChanged,
                                                                                          rbCPC.CheckedChanged
        'mstrLetterType = CType(sender, RadioButton).Text
        UIState.mstrLetterType = CType(sender, RadioButton).Text
    End Sub


    ' Cancel button clicked
    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click

        Dim dr As DialogResult = MessageBox.Show("Are you sure you want to Cancel", "Are you Sure", MessageBoxButtons.YesNo)
        If dr = DialogResult.Yes Then
            'BGW.CancelAsync()
            WT.CancelAsync()
        End If

    End Sub


    Private Sub ReRunLetter_Closing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

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
            rbACO.Enabled = False
            rbWC.Enabled = False
            rbMBP.Enabled = False
            rbENT.Enabled = False
            rbDIS.Enabled = False
            rbCPC.Enabled = False
            frmParent.cbProdTest.Enabled = False
        Else
            frmParent.ToolsToolStripMenuItem.Enabled = True
            btnProcess.Enabled = True
            btnCancel.Enabled = False
            rbACO.Enabled = True
            rbWC.Enabled = True
            rbMBP.Enabled = True
            rbENT.Enabled = True
            rbDIS.Enabled = True
            rbCPC.Enabled = True
            frmParent.cbProdTest.Enabled = True
        End If
    End Sub


End Class