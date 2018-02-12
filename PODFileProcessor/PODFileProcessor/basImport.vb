Imports System.IO
Imports Utilities

Public Module basImport
    'Public Sub ProcessTodaysFiles(dtDateToProcess As DateTime, ByVal blImport As Boolean, ByVal blNewBatch As Boolean, ByVal intBatch As Integer, ByVal intRun As Integer)



    '    Dim strdirectory As String = "\\Cobmain\usacms\PODFO\Downloads\Process\" & dtDateToProcess.ToString("MM-dd-yyyy") & "\"
    '    Dim intBatchId As Integer = intBatch
    '    dtMBPPDRDateToUnzip = dtDateToProcess
    '    dtWCDateToUnzip = dtDateToProcess
    '    dtFileDate = dtDateToProcess
    '    dtImportFilesDate = dtDateToProcess

    '    mstrLogFileLocation = strdirectory & "Processing Log.txt"



    '    If Directory.Exists(strdirectory) Then



    '        'Unzip the MBPPDR files
    '        WriteToLogfile("Unzipping MBPPDR files")
    '        UnzipMBPPDRFiles()

    '        'Unzip and Process WC Files
    '        WriteToLogfile("Unzipping WC files")
    '        UnzipWCFiles()

    '        'Import The Files
    '        If (blImport = True) Then
    '            WriteToLogfile("Importing Files")
    '            ImportAllFiles()
    '        End If

    '        'Batch Cass and Sort
    '        If (blNewBatch = True) Then
    '            WriteToLogfile("Cassing and Sorting Files")
    '            intBatch = BatchCassAndPresort(intBatch, intRun)
    '        End If


    '        'Dim form = New ReportsApplication1.MainForm(intBatchId, "01")
    '        ' Dim form2 = New ReportsApplication1.MainForm(intBatchId, "02")
    '    End If



    'End Sub

    ' Get connection string
    Private Conn As String = Nothing
    ' Create DB Logger
    Private Logger As Logging = Nothing
    ' Create Config table access
    Private Conf As ConfigTable = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False

    Private strdirectoryP As String = Nothing


    Sub New()
        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB
        Logger = New Logging(Conn, "AppLog")
        Conf = New ConfigTable(Conn)
        strdirectoryP = GetParm("strdirectory", "\\Cobmain\usacms\PODFO\Downloads\Process\")
    End Sub


    Public Sub ProcessTodaysFiles(dtDateToProcess As DateTime, ByVal blImport As Boolean, ByVal blNewBatch As Boolean,
                                  ByRef strBatch As String, ByRef strRun As String)

        Dim strdirectory As String = strdirectoryP & dtDateToProcess.ToString("MM-dd-yyyy") & "\"
        'Dim intBatchId As Integer
        'Dim intRun As Integer
        dtMBPPDRDateToUnzip = dtDateToProcess
        dtWCDateToUnzip = dtDateToProcess
        dtFileDate = dtDateToProcess
        dtImportFilesDate = dtDateToProcess

        mstrLogFileLocation = strdirectory & "Processing Log.txt"
        '   MessageBox.Show("START")
        Log("I", "ProcessTodaysFiles Starting")

        If Directory.Exists(strdirectory) Then


            'Unzip the MBPPDR files
            WriteToLogfile("Unzipping MBPPDR files")
            ' MessageBox.Show("Unzipping MBPPDR files")
            UnzipMBPPDRFiles()

            'Unzip and Process WC Files
            ' MessageBox.Show("Unzipping WC files")
            WriteToLogfile("Unzipping WC files")
            UnzipWCFiles()

            If (blImport = True) Then
                'Import The Files

                '     MessageBox.Show("Importing Files")
                WriteToLogfile("Importing Files")
                ImportAllFiles()
            End If

            'Batch Cass and Sort
            'MessageBox.Show("Cassing and Sorting Files")
            WriteToLogfile("Cassing and Sorting Files")
            If (blNewBatch = True) Then
                strBatch = BatchCassAndPresort(strRun)
                ' strBatch = intBatchId
            End If


            If (strRun = 2) Then
                strRun = "02"
            Else
                strRun = "01"
            End If
        End If

        Log("I", "ProcessTodaysFiles Ending")


    End Sub

    ''' <summary>
    ''' Log messages
    ''' </summary>
    ''' <param name="level">Level, e.g. I, W, E</param>
    ''' <param name="msg">Message text</param>
    ''' <remarks></remarks>
    Private Sub Log(ByRef level As String, ByRef msg As String)
        Logger.Log(level, "PODFileProcessor", msg)
    End Sub


    ''' <summary>
    ''' Get parameter from Config table in DB
    ''' </summary>
    ''' <param name="parmname">Parm value desired</param>
    ''' <param name="def">default value if no parm</param>
    ''' <returns>Parm value or default</returns>
    ''' <remarks></remarks>
    Private Function GetParm(ByRef parmname As String, ByRef def As String) As String
        Dim value As String = String.Empty
        Dim group As String = If(UseTestDB, "PODFileProcessor.Test", "PODFileProcessor")

        value = Conf.Get(group, parmname)
        If (String.IsNullOrEmpty(value)) Then value = def
        Return (value)
    End Function

End Module
