Imports System.Data.SqlClient
Imports System.IO
Imports Utilities

Module basBatchCassPresort

    ' Get connection string
    Private Conn As String = Nothing
    ' Create DB Logger
    Private Logger As Logging = Nothing
    ' Create Config table access
    Private Conf As ConfigTable = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False

    Public strPostageStatementFolder As String = "\\Cobmain\usacms\PODFO\Output\Postage Statements\"
    Public strMailDatFolder As String = "\\Cobmain\usacms\PODFO\Output\Mail-Dat\"
    Public dtFileDate As DateTime
    Public mcolLetterTypes As New colLetterTypes


    Sub New()
        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB
        Logger = New Logging(Conn, "AppLog")
        Conf = New ConfigTable(Conn)
        strPostageStatementFolder = GetParm("strPostageStatementFolder", "\\Cobmain\usacms\PODFO\Output\Postage Statements\")
        strMailDatFolder = GetParm("strMailDatFolder", "\\Cobmain\usacms\PODFO\Output\Mail-Dat\")
    End Sub


    Public Function BatchCassAndPresort(ByRef strRun As Integer) As Integer
        Dim intBatchId As Integer
        Try



            Dim oMainConnection As SqlConnection
            'Dim objConn As New Connection


            'open connection to database
            oMainConnection = DbAccess.GetConnection()
            LoadLetterTypes()
            '
            Dim blAuto As Boolean = True
            intBatchId = SelectRunsForBatch(oMainConnection, strRun, blAuto)


            If intBatchId <> -1 Then


                SortRun(oMainConnection, intBatchId, "01")
                SortRun(oMainConnection, intBatchId, "02")
                'If (intRun = 2) Then
                'running this for run 02 should do nothing i think.
                'SortRun(oMainConnection, intBatchId, "02")
                'End If

            End If
            Return intBatchId
            'oMainConnection.Close()
            'oMainConnection.Dispose()
        Catch ex As Exception
            'MessageBox.Show(ex.Message & "batchCassAndPresort")
            ReportsApplication1.clsEmail.EmailMessage("Error from PODFO Batch " & intBatchId & strRun, "Error from batchCassAndPresort.BatchCassAndPresort " & ex.Message)
        End Try

    End Function


    'Public Function BatchCassAndPresort(ByVal intBatch As Integer, ByVal intRun As Integer) As Integer
    '    Try


    '        Dim intBatchId As Integer
    '        Dim oMainConnection As SqlConnection
    '        Dim objConn As New Connection


    '        'open connection to database
    '        oMainConnection = objConn.getConnection
    '        LoadLetterTypes()
    '        intBatch = SelectRunsForBatch(oMainConnection, intRun, False)


    '        If intBatch <> -1 Then


    '            SortRun(oMainConnection, intBatch, intRun)

    '        End If
    '        Return intBatch
    '        oMainConnection.Close()
    '        oMainConnection.Dispose()
    '    Catch ex As Exception
    '        ReportsApplication1.clsEmail.EmailMessage("ERROR FROM PODFO", "Error from batchCassAndPresort.BatchCassAndPresort " & ex.Message)

    '    End Try

    'End Function


    Private Function MailDate() As DateTime
        Dim intDaysToAdd As Integer

        'Select Case dtFileDate.DayOfWeek
        '    Case DayOfWeek.Sunday
        '        intDaysToAdd = 3
        '    Case DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday
        '        intDaysToAdd = 2
        '    Case DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday
        '        intDaysToAdd = 4
        'End Select
        Select Case dtFileDate.DayOfWeek
            Case DayOfWeek.Sunday
                intDaysToAdd = 2
            Case DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday
                intDaysToAdd = 1
            Case DayOfWeek.Friday, DayOfWeek.Saturday
                intDaysToAdd = 3
        End Select
        Return DateAdd(DateInterval.Day, intDaysToAdd, dtFileDate)
    End Function


    Private Sub SortRun(ByRef ojbMainConnection As SqlConnection, ByVal intBatchID As Integer, ByVal strRun As String)
        Try

            Dim cmdSQL As New SqlCommand
            Dim objSatori As New colSatoriAddresses
            Dim dsLettersToSort As New DataSet
            Dim daLettersToSort As SqlDataAdapter
            Dim dtMaildate As DateTime

            With cmdSQL
                .CommandText = "USP_Select_MailingInfo_Presort"
                .Connection = ojbMainConnection
                .Parameters.AddWithValue("@P_PODBatchID", intBatchID)


                .Parameters.AddWithValue("@P_PODRunID", strRun)
                .CommandType = CommandType.StoredProcedure

            End With

            'set dataadpater to command
            daLettersToSort = New SqlDataAdapter(cmdSQL)

            'use adapter to fill dataset
            daLettersToSort.Fill(dsLettersToSort)



            Dim intMax As Integer = dsLettersToSort.Tables(0).Rows.Count - 1
            Dim intIndex As Integer
            Dim ObjCurrentAddress As clsSatoriAddress
            If intMax >= 0 Then
                For intIndex = 0 To intMax

                    ObjCurrentAddress = objSatori.Add(dsLettersToSort.Tables(0).Rows(intIndex).Item("PODMailingInfoID"))


                    With dsLettersToSort.Tables(0).Rows(intIndex)

                        ObjCurrentAddress.LetterType = mcolLetterTypes(.Item("PODMasterLetterTypeId").ToString).LetterType

                        Dim strAddressline3 As String = .Item("OrigAdd3").ToString

                        If strAddressline3.Trim.Length = 0 Then



                            ObjCurrentAddress.OriginalAddress1 = .Item("OrigAdd1").ToString.Trim
                            ObjCurrentAddress.OriginalAddress2 = .Item("OrigAdd2").ToString.Trim
                            If StringHasNumber(ObjCurrentAddress.OriginalAddress1) = True Then
                                ObjCurrentAddress.HasNumberInAddress = True
                            ElseIf StringHasNumber(ObjCurrentAddress.OriginalAddress1) = True Then
                                ObjCurrentAddress.HasNumberInAddress = True
                            Else
                                ObjCurrentAddress.HasNumberInAddress = False
                            End If

                        Else

                            Dim strAddressLine1 As String = ""
                            Dim strAddressline2 As String = ""
                            Dim strExtra As String = ""
                            Dim blnHasNumberInAddress As Boolean

                            CreateAddressLineOneAndTwo(dsLettersToSort.Tables(0).Rows(intIndex), strAddressLine1, strAddressline2, strExtra, blnHasNumberInAddress)

                            ObjCurrentAddress.OriginalAddress1 = strAddressLine1
                            ObjCurrentAddress.OriginalAddress2 = strAddressline2
                            ObjCurrentAddress.ExtraAddress = strExtra
                            ObjCurrentAddress.HasNumberInAddress = blnHasNumberInAddress

                        End If

                        ObjCurrentAddress.OriginalLastLine = .Item("OrigCityStateZip").ToString.Trim
                    End With

                Next

                With objSatori
                    .JobNameToDisplay = "PODTEST"
                    .MailDate = Now

                    If .Count > 200 Then
                        If strRun = "01" Then
                            .SatoriTemplateName = "PODFO-MIXED-10s"
                        Else
                            .SatoriTemplateName = "PODFO-MIXED-FLATS"
                        End If

                    Else
                        .SatoriTemplateName = "RCMIXMAILMETEREDIMB"
                    End If

                    .SortFileNameAndPath = strPostageStatementFolder & "PODFO_POSTAGE_" & intBatchID.ToString.Trim & strRun & ".PDF"
                    .MailDatDirectory = strMailDatFolder
                    .MixedWeightMailing = False
                    .ShowSatoriScreen = True
                    .SatoriServerIP = GetParm("SatoriServerIP", "10.0.0.107:5150")
                    dtMaildate = MailDate()
                    .MailDate = dtMaildate
                End With


                objSatori.SatoriCassAndSort(intBatchID, strRun)


                dsLettersToSort.Tables(0).PrimaryKey = New DataColumn() {dsLettersToSort.Tables(0).Columns("PODMailingInfoID")}

                Dim drCurrentRow As DataRow

                Try



                    For Each ObjCurrentAddress In objSatori

                        If dsLettersToSort.Tables(0).Rows.Contains(ObjCurrentAddress.RecordID) Then
                            drCurrentRow = dsLettersToSort.Tables(0).Rows.Find(ObjCurrentAddress.RecordID)

                            With ObjCurrentAddress

                                If .ExtraAddress.Trim.Length = 0 Then

                                    drCurrentRow.Item("SatoriAdd1") = .MailAddress1
                                    drCurrentRow.Item("SatoriAdd2") = .MailAddress2
                                    drCurrentRow.Item("SatoriAdd3") = ""
                                Else

                                    drCurrentRow.Item("SatoriAdd1") = .ExtraAddress
                                    drCurrentRow.Item("SatoriAdd2") = .MailAddress1
                                    drCurrentRow.Item("SatoriAdd3") = .MailAddress2

                                End If

                                drCurrentRow.Item("SatoriCityStateZip") = .MailLastLine
                                drCurrentRow.Item("MDPC") = .MailDeliveryPointCode
                                drCurrentRow.Item("MADD_ERR") = .MailErrorCode
                                drCurrentRow.Item("MTray") = .MailTrayNumber
                                drCurrentRow.Item("MPackage") = .MailPackageNumber
                                drCurrentRow.Item("MBarcode") = .MailIntelligentMailBarCode
                                drCurrentRow.Item("MBarcodeNumber") = .MailIntelligentMailBarCodeNumber
                                drCurrentRow.Item("MEndorse") = .MailEndoresementLine
                                drCurrentRow.Item("MPresortID") = .MailPresortID
                                drCurrentRow.Item("MKeyline") = .MailKeyline
                            End With

                        End If
                    Next


                Catch ex As Exception

                    Throw New System.Exception("Error from basBatchCassPresort.SortRun.Loop. Error during sort " & ex.Message)

                End Try

                Dim CMD As New SqlClient.SqlCommandBuilder(daLettersToSort)

                daLettersToSort.AcceptChangesDuringFill = True

                daLettersToSort.UpdateCommand = CMD.GetUpdateCommand
                daLettersToSort.Update(dsLettersToSort)

                CreateBarcodesForMailing(ojbMainConnection, intBatchID, strRun)

                UpdateBatchRunMailDate(ojbMainConnection, strRun, intBatchID, dtMaildate)

            End If
        Catch ex As Exception
            Throw New System.Exception("Error from basBatchCassPresort.SortRun. Error during sort " & ex.Message)
        End Try

    End Sub


    Private Sub CreateAddressLineOneAndTwo(ByVal drRow As DataRow, ByRef strAddress1 As String, ByRef strAddress2 As String, ByRef strAddressExtra As String, ByRef blnHasNumber As Boolean)


        'check if address3 has numbers 
        If StringHasNumber(drRow.Item("OrigAdd3").ToString.Trim) = True Then

            blnHasNumber = True

            'add1 and 2 are extras
            strAddressExtra = drRow.Item("OrigAdd1").ToString.Trim & " " & drRow.Item("OrigAdd2").ToString.Trim

            '3 is the main address line
            strAddress1 = drRow.Item("OrigAdd3").ToString.Trim

            'add any remaing to address line 2
            If drRow.Item("OrigAdd4").ToString.Trim.Length > 0 Then
                strAddress2 = drRow.Item("OrigAdd4").ToString.Trim
            End If
            If drRow.Item("OrigAdd5").ToString.Trim.Length > 0 Then
                strAddress2 = strAddress2 & " " & drRow.Item("OrigAdd5").ToString.Trim
            End If

        Else 'Address 3 becomes part of the extra

            If StringHasNumber(drRow.Item("OrigAdd4").ToString.Trim) = True Then
                blnHasNumber = True

                'add1,2 and 3 are extras
                strAddressExtra = drRow.Item("OrigAdd1").ToString.Trim & " " & drRow.Item("OrigAdd2").ToString.Trim & " " & drRow.Item("OrigAdd3").ToString.Trim

                '3 is the main address line
                strAddress1 = drRow.Item("OrigAdd4").ToString.Trim

                'add any remaing to address line 2
                If drRow.Item("OrigAdd5").ToString.Trim.Length > 0 Then
                    strAddress2 = drRow.Item("OrigAdd5").ToString.Trim
                End If

            Else 'address 4 is added to extra

                If StringHasNumber(drRow.Item("OrigAdd5").ToString.Trim) = True Then
                    blnHasNumber = True

                    'add1,2 and 3 are extras
                    strAddressExtra = drRow.Item("OrigAdd1").ToString.Trim & " " & drRow.Item("OrigAdd2").ToString.Trim & " " & drRow.Item("OrigAdd3").ToString.Trim & " " & drRow.Item("OrigAdd4").ToString.Trim

                    '3 is the main address line
                    strAddress1 = drRow.Item("OrigAdd5").ToString.Trim

                    strAddress2 = ""

                Else 'there are no numbers so it's all bad

                    blnHasNumber = False
                    strAddressExtra = ""
                    strAddress1 = drRow.Item("OrigAdd1").ToString.Trim
                    strAddress2 = drRow.Item("OrigAdd2").ToString.Trim & " " & drRow.Item("OrigAdd3").ToString.Trim & " " & drRow.Item("OrigAdd4").ToString.Trim & " " & drRow.Item("OrigAdd5").ToString.Trim

                End If

            End If 'address line 4 check



        End If 'address line 3 check




    End Sub


    Private Function StringHasNumber(ByVal strToCheck As String) As Boolean
        Dim Element As Char

        For Each Element In strToCheck

            If IsNumeric(Element) Then
                Return True
            End If
        Next

        Return False

    End Function


    Private Function CreateNewBatch(ByVal strRun As String) As Integer
        Try
            Dim cmdSQL As New SqlCommand
            Dim rdReader As SqlDataReader
            'Dim conn As New Connection


            With cmdSQL
                .CommandText = "USP_INSERT_NEW_BATCH"
                .CommandType = CommandType.StoredProcedure
                .Connection = DbAccess.GetConnection()
                .Parameters.AddWithValue("@P_PODRunID", "01")
                rdReader = .ExecuteReader

            End With

            If rdReader.Read = True Then

                CreateNewBatch = rdReader.Item(0)

            Else

                CreateNewBatch = -1

            End If

            rdReader.Close()
            cmdSQL.Dispose()
            rdReader = Nothing
        Catch ex As Exception
            Throw New System.Exception("Error creating batch", ex)
        End Try

    End Function


    Private Sub InsertNewBatchRun(ByVal intBatch As Integer, ByVal strRun As String)
        Try
            Dim cmdSQL As New SqlCommand

            'Dim conn As New Connection


            With cmdSQL

                .CommandText = "USP_INSERT_NEWRUN"
                .CommandType = CommandType.StoredProcedure
                .Connection = DbAccess.GetConnection()
                .Parameters.AddWithValue("@P_PODRunID", strRun)
                .Parameters.AddWithValue("@P_PODBatchID", intBatch)
                .ExecuteNonQuery()

            End With




            cmdSQL.Dispose()
        Catch ex As Exception
            Throw New System.Exception("Error inserting Batch", ex)
        End Try
    End Sub


    Private Sub UpdateBatchRunMailDate(ByRef ojbMainConnection As SqlConnection, ByVal strRun As String,
                                       ByVal intBatch As Integer, ByVal dtMaildate As DateTime)
        Try


            Dim cmdSQL As New SqlCommand



            With cmdSQL
                .CommandText = "USP_Update_Run_MustMailDate"
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@P_PODRunID", strRun)
                .Parameters.AddWithValue("@P_PODBatch", intBatch)
                .Parameters.AddWithValue("@P_MustMailDate", dtMaildate)
                .Connection = ojbMainConnection
                .ExecuteNonQuery()

            End With


            cmdSQL.Dispose()
        Catch ex As Exception
            Throw New System.Exception("Error Update batch Run MailDate" & ex.Message)
        End Try
    End Sub


    Private Function SelectRunsForBatch(ByRef ojbMainConnection As SqlConnection, ByRef strRun As String, ByVal blAuto As Boolean) As Integer
        Try
            Dim cmdSQL As New SqlCommand
            Dim rdReader As SqlDataReader
            Dim blnFirst As Boolean = True
            'Dim strRun As String
            Dim intBatch As Integer

            With cmdSQL
                .CommandText = "SELECT PODRunID FROM dbo.PODMailingInfo GROUP BY PODRunID, PODBatchID HAVING PODBatchID = 0"
                .CommandType = CommandType.Text
                .Connection = ojbMainConnection
                rdReader = .ExecuteReader

            End With



            Do While rdReader.Read
                strRun = rdReader.Item("PODRunID")

                If (strRun = 2 And blAuto = True) Then
                    strRun = "02"
                End If
                If blnFirst = True Then
                    intBatch = CreateNewBatch(strRun)
                    blnFirst = False
                Else

                    InsertNewBatchRun(intBatch, strRun)

                End If
            Loop


            rdReader.Close()
            cmdSQL.Dispose()
            rdReader = Nothing

            Return intBatch
        Catch ex As Exception

            Throw New System.Exception("Error reading database", ex)
        End Try

    End Function


    Private Function ReturnAddressLine2(dr As DataRow) As String
        Dim strAddressLine2 As String

        strAddressLine2 = ""

        With dr

            Dim strnextAddress As String = .Item("OrigAdd2").ToString.Trim

            'add all other address lines to second line satori only allows 2 address lines
            If strnextAddress.Length > 0 Then

                strAddressLine2 = strnextAddress

                strnextAddress = .Item("OrigAdd3").ToString.Trim

                If strnextAddress.Length > 0 Then

                    strAddressLine2 = strAddressLine2 & " " & strnextAddress

                    strnextAddress = .Item("OrigAdd4").ToString.Trim

                    If strnextAddress.Length > 0 Then

                        strAddressLine2 = strAddressLine2 & " " & strnextAddress

                        strnextAddress = .Item("OrigAdd5").ToString.Trim

                        If strnextAddress.Length > 0 Then

                            strAddressLine2 = strAddressLine2 & " " & strnextAddress



                        End If

                    End If

                End If

            End If ' original address check
        End With

        Return strAddressLine2

    End Function


    Private Sub LoadLetterTypes()
        Dim cmdSQL As New SqlCommand
        Dim rdReader As SqlDataReader
        'Dim conn As New Connection


        With cmdSQL
            .CommandText = "Select * from PODMasterLetterType order by PODMasterLetterTypeId"
            .CommandType = CommandType.Text
            .Connection = DbAccess.GetConnection()
            rdReader = .ExecuteReader

        End With

        Do While rdReader.Read

            Dim strType As String = rdReader.Item("LetterType").ToString
            Dim strSubType As String = rdReader.Item("SubType").ToString
            Dim strLanguage As String = rdReader.Item("Language").ToString
            Dim strKey As String = rdReader.Item("PODMasterLetterTypeId").ToString
            mcolLetterTypes.Add(strType, strSubType, strLanguage, strKey)

        Loop

        cmdSQL.Dispose()
        rdReader.Close()
        rdReader = Nothing


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
