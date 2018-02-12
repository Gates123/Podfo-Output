
Imports System.IO
Imports Utilities

Public Module basUnzipMBPPDR

    ' Get connection string
    Private Conn As String = Nothing
    ' Create DB Logger
    Private Logger As Logging = Nothing
    ' Create Config table access
    Private Conf As ConfigTable = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False


    Private mstrDownLoadFolderName As String = "\\Cobmain\usacms\PODFO\Downloads\PROCESS"
    Private mstrUnzipMPDFolder
    Public dtMBPPDRDateToUnzip As DateTime


    Sub New()
        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB
        Logger = New Logging(Conn, "AppLog")
        Conf = New ConfigTable(Conn)
        mstrDownLoadFolderName = GetParm("strdirectory", "\\Cobmain\usacms\PODFO\Downloads\PROCESS")
    End Sub

    Public Function UnzipMBPPDRFiles() As Integer
        Try

            Dim fleFiles As String()
            Dim strCurrentFile As String
            Dim intZipID As Integer

            intZipID = -1


            'today's downloads will be in today's folder
            mstrDownLoadFolderName = mstrDownLoadFolderName & "\" & dtMBPPDRDateToUnzip.ToString("MM-dd-yyyy") & "\"


            ''P:\COB\DATA\WCPOD
            fleFiles = Directory.GetFiles(mstrDownLoadFolderName)

            'loop through the zip files
            For Each strCurrentFile In fleFiles

                If UCase(Mid(strCurrentFile, strCurrentFile.Length - 3)) = ".ZIP" Then


                    Dim objFile As New FileInfo(strCurrentFile)


                    If InStr(objFile.Name.ToUpper, "MBPPRD") > 0 Then



                        WriteToLogfile("Checking for zip file: " & objFile.Name)
                        'only process if zip file has not been processed previously
                        If Check_Zip_FileName_Is_Good(objFile.Name) = True Then

                            'ONLY NEED FOLDER IF WE ARE UNZIPPING
                            mstrUnzipMPDFolder = mstrDownLoadFolderName & "Processed_MBPPRD\"
                            If Directory.Exists(mstrUnzipMPDFolder) = False Then
                                Directory.CreateDirectory(mstrUnzipMPDFolder)
                            End If


                            WriteToLogfile("Extracting zip file: " & objFile.Name)
                            'unzip file in PREPROCESS FOLDER
                            Extract(mstrDownLoadFolderName, strCurrentFile)


                            WriteToLogfile("Inserting zip file: " & objFile.Name)
                            intZipID = Insert_ZipFile(objFile.Name)




                        End If
                    End If
                End If

            Next
            UnzipMBPPDRFiles = intZipID

        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basUnzipMBPPDR.UnzipMBPPDRFiles" & ex.Message)
        End Try

    End Function
    Public Function UnzipMBPPDRFiles(ByVal dtMBPPDRDateToUnzip As DateTime) As Integer
        Try

            Dim fleFiles As String()
            Dim strCurrentFile As String
            Dim intZipID As Integer

            intZipID = -1


            'today's downloads will be in today's folder
            mstrDownLoadFolderName = mstrDownLoadFolderName & "\" & dtMBPPDRDateToUnzip.ToString("MM-dd-yyyy") & "\"


            ''P:\COB\DATA\WCPOD
            fleFiles = Directory.GetFiles(mstrDownLoadFolderName)

            'loop through the zip files
            For Each strCurrentFile In fleFiles

                If UCase(Mid(strCurrentFile, strCurrentFile.Length - 3)) = ".ZIP" Then


                    Dim objFile As New FileInfo(strCurrentFile)


                    If InStr(objFile.Name.ToUpper, "MBPPRD") > 0 Then



                        WriteToLogfile("Checking for zip file: " & objFile.Name)
                        'only process if zip file has not been processed previously
                        If Check_Zip_FileName_Is_Good(objFile.Name) = True Then

                            'ONLY NEED FOLDER IF WE ARE UNZIPPING
                            mstrUnzipMPDFolder = mstrDownLoadFolderName & "Processed_MBPPRD\"
                            If Directory.Exists(mstrUnzipMPDFolder) = False Then
                                Directory.CreateDirectory(mstrUnzipMPDFolder)
                            End If


                            WriteToLogfile("Extracting zip file: " & objFile.Name)
                            'unzip file in PREPROCESS FOLDER
                            Extract(mstrDownLoadFolderName, strCurrentFile)


                            WriteToLogfile("Inserting zip file: " & objFile.Name)
                            intZipID = Insert_ZipFile(objFile.Name)




                        End If
                    End If
                End If

            Next
            UnzipMBPPDRFiles = intZipID

        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basUnzipMBPPDR.UnzipMBPPDRFiles" & ex.Message)
        End Try

    End Function
    Private Function Extract(ByVal strDirectoryForUnzipping As String, ByVal strFullZipFileName As String) As Integer

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
            myProcess.StartInfo.Arguments = " x " & """" & strFullZipFileName & """" & " -o" & """" & mstrUnzipMPDFolder & """" & " -y"
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            myProcess.StartInfo.CreateNoWindow = False
            myProcess.StartInfo.UseShellExecute = False
            myProcess.Start()

            myProcess.WaitForExit()
            intExitCode = myProcess.ExitCode

            If intExitCode = 0 Then




                Dim strExtractedFiles As String() = Directory.GetFiles(mstrUnzipMPDFolder)
                intLetterCount = strExtractedFiles.Length
                Return intLetterCount


            End If


        Catch ex As Exception
            'make sure dll in folder
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basUnzipMBPPDR.Extract" & ex.Message)
            Throw New System.Exception("Error extracting", ex)

        Finally
            ' zipStream.Close()
            'File.Delete(_Filename)
        End Try
    End Function
    Private Function Insert_ZipFile(ByVal strZipFileName As String) As Integer
        Try


            'Dim ConnectionMain As New Connection
            'Dim conn As SqlClient.SqlConnection = ConnectionMain.getConnection
            Dim cmd As New SqlClient.SqlCommand
            Dim rd As SqlClient.SqlDataReader


            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "USP_Insert_PODMBPPRDZipFile"
            cmd.CommandTimeout = 0
            cmd.Connection = DbAccess.GetConnection()
            cmd.Parameters.AddWithValue("@P_MBPPRDZIPName", strZipFileName)

            rd = cmd.ExecuteReader

            If rd.Read() = True Then

                'get recid
                Insert_ZipFile = rd.Item(0)

            Else
                Insert_ZipFile = -1

            End If
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basUnzipMBPPDR.Insert_ZipFile" & ex.Message)
            Throw New System.Exception("Error inserting Zip into db", ex)
        End Try

    End Function
    Private Function Check_Zip_FileName_Is_Good(ByVal strZipFileName As String) As Boolean
        Try


            'Dim ConnectionMain As New Connection
            'Dim conn As SqlClient.SqlConnection = ConnectionMain.getConnection
            Dim cmd As New SqlClient.SqlCommand
            Dim rd As SqlClient.SqlDataReader


            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "USP_POD_MBPPRD_CHECK_ZIP_FILENAME"
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
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basUnzipMBPPDR.Check_Zip_FileName_Is_Good" & ex.Message)
            Throw New System.Exception("Error checking zip file name", ex)
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
