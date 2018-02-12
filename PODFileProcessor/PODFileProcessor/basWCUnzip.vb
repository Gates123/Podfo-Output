Imports System.IO
Imports System.Data.SqlClient
Imports Utilities


Module basWCUnzip

    ' Get connection string
    Private Conn As String = Nothing
    ' Create DB Logger
    Private Logger As Logging = Nothing
    ' Create Config table access
    Private Conf As ConfigTable = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False

    Public mstrMainWCProcessFolder As String = "\\Cobmain\usacms\PODFO\Downloads\WC_ZIPFILES\PROCESSED\"
    Public mstrDownLoadFolderName As String = "\\Cobmain\usacms\PODFO\Downloads\PROCESS"
    Public mstrWCPODProcessedFolder As String
    Public mstrWCPODPreprocessFolder As String

    Public mstrWCPODPreprocessArchiveFolder As String
    Public dtWCDateToUnzip As DateTime


    Sub New()
        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB
        Logger = New Logging(Conn, "AppLog")
        Conf = New ConfigTable(Conn)
        mstrMainWCProcessFolder = GetParm("mstrMainWCProcessFolder", "\\Cobmain\usacms\PODFO\Downloads\WC_ZIPFILES\PROCESSED\")
        mstrDownLoadFolderName = GetParm("strdirectory", "\\Cobmain\usacms\PODFO\Downloads\PROCESS")
    End Sub

    Public Sub UnzipWCFiles()
        Try
            Dim fleFiles As String()
            Dim strCurrentFile As String
            Dim intZipID As Integer = -1


            'today's downloads will be in today's folder
            mstrDownLoadFolderName = mstrDownLoadFolderName & "\" & dtWCDateToUnzip.ToString("MM-dd-yyyy") & "\"


            ''P:\COB\DATA\WCPOD
            fleFiles = Directory.GetFiles(mstrDownLoadFolderName)

            'loop through the zip files
            For Each strCurrentFile In fleFiles

                If UCase(Mid(strCurrentFile, strCurrentFile.Length - 3)) = ".ZIP" Then


                    Dim objFile As New FileInfo(strCurrentFile)


                    If InStr(objFile.Name.ToUpper, "PEARSON") > 0 Then

                        mstrWCPODPreprocessFolder = mstrDownLoadFolderName & "WC_FILES\"
                        WriteToLogfile("Checking for zip file: " & objFile.Name)
                        'only process if zip file has not been processed previously
                        If Check_Zip_FileName_Is_Good(objFile.Name) = True Then

                            'ONLY NEED FOLDER IF WE ARE UNZIPPING

                            If Directory.Exists(mstrWCPODPreprocessFolder) = False Then
                                Directory.CreateDirectory(mstrWCPODPreprocessFolder)

                            End If


                            WriteToLogfile("Extracting zip file: " & objFile.Name)
                            'unzip file in PREPROCESS FOLDER
                            If Extract(mstrDownLoadFolderName, strCurrentFile, intZipID) < 1 Then

                                Exit Sub

                            End If


                            WriteToLogfile("Inserting zip file: " & objFile.Name)
                            intZipID = Insert_ZipFile(objFile.Name)


                        End If

                        If (intZipID <> -1) Then


                            mstrWCPODProcessedFolder = mstrDownLoadFolderName & "Procssed_WC\"
                            If Directory.Exists(mstrWCPODProcessedFolder) = False Then
                                Directory.CreateDirectory(mstrWCPODProcessedFolder)

                            End If

                            'PROCESS-PREPROCESS FOLDER CONVERT DOC TO PDF AND CREATE EXCEL DOCUMENT
                            WriteToLogfile("ConvertDocToPDF")
                            'MessageBox.Show("Calling ConvertDocToPDF")
                            ConvertDocToPDF(mstrWCPODPreprocessFolder, intZipID)
                            'MessageBox.Show("Done ConvertDocToPDF")
                            WriteToLogfile("ConvertDocToPDF Done")
                        End If

                        'LAST STEP DELETE PREPROCESS FOLDER
                        'Dim objDirectory As New DirectoryInfo(mstrWCPODPreprocessFolder)
                        'objDirectory.Delete()


                    End If 'PEARSON CHECK CONTAINS WC FILES

                End If ' ZIP CHECK

            Next
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basWCUnzip.UnzipWCFiles" & ex.Message)
        End Try

        '  MessageBox.Show("done")

    End Sub
  
    Public Function Extract(ByVal strDirectoryForUnzipping As String, ByVal strFullZipFileName As String, ByVal intTrnHdrID As Integer) As Integer

        Try
            Dim myProcess As Process = New System.Diagnostics.Process()
            Dim progPath As String
            Dim intExitCode As Integer
            Dim FILEINFO As New FileInfo(strFullZipFileName)
            'Dim strFolderName As String = mstrWCPODPreprocessFolder
            Dim intLetterCount As Integer

            progPath = Application.StartupPath

            ' Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High
            myProcess.StartInfo.FileName = progPath & "\7za.exe"
            myProcess.StartInfo.Arguments = " x " & """" & strFullZipFileName & """" & " -o" & """" & mstrWCPODPreprocessFolder & """" & " -y"
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            myProcess.StartInfo.CreateNoWindow = False
            myProcess.StartInfo.UseShellExecute = False
            myProcess.Start()

            myProcess.WaitForExit()
            intExitCode = myProcess.ExitCode

            If intExitCode = 0 Then

                Dim strExtractedFiles As String() = Directory.GetFiles(mstrWCPODPreprocessFolder)
                intLetterCount = strExtractedFiles.Length
                Return intLetterCount

            Else
                ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "zip 7 returned an exit code of " & intExitCode.ToString)
                Return -1
            End If



        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basWCUnzip.Extract" & ex.Message)

            Throw New System.Exception("Unable to extract zip", ex)

        Finally
            ' zipStream.Close()
            'File.Delete(_Filename)
        End Try
    End Function
    Private Function Insert_ZipFile(ByVal strZipFileName As String) As Integer
        Try
            'Dim ConnectionMain As New Connection
            ' Dim conn As SqlClient.SqlConnection = ConnectionMain.getConnection
            Dim cmd As New SqlClient.SqlCommand
            Dim rd As SqlClient.SqlDataReader


            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UPS_Insert_PODWCZipFile"
            cmd.CommandTimeout = 0
            cmd.Connection = DbAccess.GetConnection()
            cmd.Parameters.AddWithValue("@P_WCZipFileName", strZipFileName)

            rd = cmd.ExecuteReader

            If rd.Read() = True Then

                'get recid
                Insert_ZipFile = rd.Item(0)

            Else
                Insert_ZipFile = -1

            End If
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basWCUnzip.Insert_ZipFile" & ex.Message)
        End Try

    End Function
    Private Function Check_Zip_FileName_Is_Good(ByVal strZipFileName As String) As Boolean
        Try

      
            'Dim ConnectionMain As New Connection
            'Dim conn As SqlClient.SqlConnection = ConnectionMain.getConnection
            Dim cmd As New SqlClient.SqlCommand
            Dim rd As SqlClient.SqlDataReader


            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "USP_POD_WC_CHECK_ZIP_FILENAME"
            cmd.CommandTimeout = 0
            cmd.Connection = DbAccess.GetConnection()
            cmd.Parameters.AddWithValue("@P_ZIP_FILENAME", strZipFileName)

            rd = cmd.ExecuteReader

            If rd.Read() = True Then

                If rd.Item("Deleted").ToString <> "1" Then
                    Check_Zip_FileName_Is_Good = False
                Else
                    Check_Zip_FileName_Is_Good = True

                End If
            Else
                Check_Zip_FileName_Is_Good = True

            End If

        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basWCUnzip.Check_Zip_FileName_Is_Good" & ex.Message)
        End Try
    End Function


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
