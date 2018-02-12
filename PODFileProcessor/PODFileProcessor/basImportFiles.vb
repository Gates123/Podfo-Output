Imports System.IO
Imports System.Data.SqlClient
Imports Utilities

Module basImportFiles

    ' Get connection string
    Private Conn As String = Nothing
    ' Create DB Logger
    Private Logger As Logging = Nothing
    ' Create Config table access
    Private Conf As ConfigTable = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False

    Public strMainProcessFolder As String = "\\Cobmain\usacms\PODFO\Downloads\Process\"
    Public strWCLiveProductionFolder As String = "\\Cobmain\usacms\PODFO\Downloads\WC_PDFs\"
    Public dtImportFilesDate As DateTime


    Sub New()
        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB
        Logger = New Logging(Conn, "AppLog")
        Conf = New ConfigTable(Conn)
        strMainProcessFolder = GetParm("strdirectory", "\\Cobmain\usacms\PODFO\Downloads\Process\")
        strWCLiveProductionFolder = GetParm("strWCLiveProductionFolder", "\\Cobmain\usacms\PODFO\Downloads\WC_PDFs\")
    End Sub


    Public Sub ImportAllFiles()
        Try
            Dim intBatchid As Integer = 0
            Dim intFileTypeID As Integer
            Dim objFile As FileInfo
            Dim intInportFileID As Integer
            Dim strFiles As String()
            Dim strCurrentFile As String
            Dim strMBPProcessFolder As String
            Dim strWCProcessFolder As String
            Dim oMainConnection As SqlConnection
            'Dim objConn As New Connection


            'open connection to database
            oMainConnection = DbAccess.GetConnection


            'create folder locations
            strMainProcessFolder = strMainProcessFolder & dtImportFilesDate.ToString("MM-dd-yyyy") & "\"
            strMBPProcessFolder = strMainProcessFolder & "Processed_MBPPRD\"
            strWCProcessFolder = strMainProcessFolder & "Procssed_WC\"


            'LOOP THROUGH MAIN PROCESSING FOLDER TO IMPORT FILES
            strFiles = Directory.GetFiles(strMainProcessFolder)

            If strFiles.Length > 0 Then

                'loop through each file 
                For Each strCurrentFile In strFiles

                    objFile = New FileInfo(strCurrentFile)


                    'determine type and import using parser for that type
                    Select Case objFile.Name.Substring(0, 3).ToUpper

                        Case "DIS", "MMA"
                            WriteToLogfile("Importing: " & objFile.Name & " as DIS")
                            intFileTypeID = GetFileTypeID(oMainConnection, "DIS")
                            intInportFileID = InsertImportFile(oMainConnection, objFile.Name, intBatchid, intFileTypeID)
                            ParseDISFile(oMainConnection, objFile.FullName, intInportFileID, intBatchid, "01")
                            MarkBadRecords(intInportFileID)
                        Case "ACO"
                            WriteToLogfile("Importing: " & objFile.Name & " as ACO")
                            intFileTypeID = GetFileTypeID(oMainConnection, "ACO")
                            intInportFileID = InsertImportFile(oMainConnection, objFile.Name, intBatchid, intFileTypeID)
                            ParseACOFile(oMainConnection, objFile.FullName, intInportFileID, intBatchid, "01")
                            MarkBadRecords(intInportFileID)
                        Case "CPC"
                            WriteToLogfile("Importing: " & objFile.Name & " as CPC")
                            intFileTypeID = GetFileTypeID(oMainConnection, "CPC")
                            intInportFileID = InsertImportFile(oMainConnection, objFile.Name, intBatchid, intFileTypeID)
                            ParseCPCFile(oMainConnection, objFile.FullName, intInportFileID, intBatchid, "01")
                            MarkBadRecords(intInportFileID)
                        Case "MBP", "NGD"
                            WriteToLogfile("Importing: " & objFile.Name & " as ENT")
                            intFileTypeID = GetFileTypeID(oMainConnection, "ENT")
                            intInportFileID = InsertImportFile(oMainConnection, objFile.Name, intBatchid, intFileTypeID)
                            ParseENTFile(oMainConnection, objFile.FullName, intInportFileID, intBatchid, "01")
                            MarkBadRecords(intInportFileID)
                    End Select


                Next


            End If

            

            'check for any WC- files does folder exist
            If Directory.Exists(strWCProcessFolder) Then
                'IF ANYTHING IS IN THE WC FOLDER PROCESS IT
                If Directory.GetFiles(strWCProcessFolder).Length > 0 Then

                    WriteToLogfile("Importing Folder: " & strWCProcessFolder & " as wc")
                    intFileTypeID = GetFileTypeID(oMainConnection, "WC")

                    ParseWCFiles(oMainConnection, strWCProcessFolder, intFileTypeID, intBatchid, "01", strWCLiveProductionFolder)
                End If

            End If

            'check if there are any MBP files -- does folder exist
            If Directory.Exists(strMBPProcessFolder) = True Then
                'IF ANYTHING IS IN THE MBP XML FOLDER PROCESS IT
                If Directory.GetFiles(strMBPProcessFolder).Length > 0 Then
                    WriteToLogfile("Importing Folder: " & strMBPProcessFolder & " as MBP")
                    intFileTypeID = GetFileTypeID(oMainConnection, "MBP")
                    intInportFileID = InsertImportFile(oMainConnection, strMBPProcessFolder, intBatchid, intFileTypeID)
                    ParseMBPFiles(oMainConnection, strMBPProcessFolder, intInportFileID, intBatchid, "01")
                    MarkBadRecords(intInportFileID)
                End If
            End If
            oMainConnection.Close()
            oMainConnection = Nothing
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basImportFiles.ImportAllFiles" & ex.Message)
        End Try
    End Sub


    Private Sub MarkBadRecords(ByVal intImportFileID As Integer)
        Try
            Dim cmdSQL As New SqlCommand

            'Dim conn As New Connection


            With cmdSQL

                .CommandText = "usp_Mark_Bad_Records_Basedon_ImportFiles"
                .CommandType = CommandType.StoredProcedure
                .Connection = DbAccess.GetConnection
                .Parameters.AddWithValue("@P_ImportFileID", intImportFileID)
                .ExecuteNonQuery()

            End With


            cmdSQL.Dispose()
        Catch ex As Exception
            'Throw New System.Exception("Error in MarkBadRecords", ex)
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basImportFiles.MarkBadRecords" & ex.Message)
        End Try
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
