Imports System.Data.SqlClient
Imports System.Globalization
Imports Utilities


Module basBarcode

    Private Conn As String = Nothing
    'Private con As New Connection
    Private sqlcon As SqlConnection = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False

    Sub New()
        Conn = DbAccess.GetConnectionString()
        sqlcon = DbAccess.GetConnection
        UseTestDB = DbAccess.UseTestDB
    End Sub

    Public Sub CreateBarcodesForMailing(ByRef ojbMainConnection As SqlConnection, intbatch As Integer, strRun As String)
        Try


            Dim cmdsql As New SqlCommand
            Dim rdSQL As SqlDataReader
            Dim strCurrentTray As String
            Dim strTray As String = "1"
            Dim intPageCount As Integer
            Dim intBarCodeCount As Integer
            Dim intBarCodeIndex As Integer
            Dim intBarCodeMax As Integer
            Dim blnEOC As Boolean
            Dim blnEdgeMark As Boolean
            Dim strConBit As String
            Dim strWrapSequence As String
            Dim strBatch As String
            Dim strPresortID As String
            Dim intMailingInfoId As Integer
            Dim strID As String
            Dim strBarcode As String
            Dim dtDatabase As DataTable



            With cmdsql
                .CommandText = "USP_Select_MailingInfo_Postsort"
                .CommandType = CommandType.StoredProcedure
                .Connection = ojbMainConnection
                .Parameters.AddWithValue("@P_PODBatchID", intbatch)
                .Parameters.AddWithValue("@P_PODRunID", strRun)
                rdSQL = cmdsql.ExecuteReader

            End With

            'delete previous barcode 
            DeleteBarcodesFor(intbatch, strRun)


            'create barcode datatable
            dtDatabase = CreateBarCodeDataTable()

            Do While rdSQL.Read

                'get mailing info id
                intMailingInfoId = rdSQL.Item("PODMailingInfoID")

                'get record presort id
                strPresortID = rdSQL.Item("MPresortID").ToString.Trim

                'get page count
                intPageCount = rdSQL.Item("PageCount")

                'get Tray info
                strCurrentTray = rdSQL.Item("Mtray").ToString


                'check for new tray for edgemarking of envelope
                If strTray <> strCurrentTray Then

                    blnEdgeMark = True
                    strTray = strCurrentTray

                Else

                    blnEdgeMark = False

                End If


                'get total number of barcodes for this document
                intBarCodeMax = intPageCount / 2


                'loop through barcodes
                For intBarCodeIndex = 1 To intBarCodeMax

                    'count number of barcodes for wrap sequence
                    intBarCodeCount = intBarCodeCount + 1

                    If intBarCodeIndex = intBarCodeMax Then

                        blnEOC = True

                    Else

                        blnEOC = False

                    End If

                    strConBit = ConBit4(False, blnEOC, blnEdgeMark, False, False)
                    strWrapSequence = Collate(intBarCodeCount)
                    strBatch = intbatch.ToString.PadLeft(4, "0")
                    strID = strPresortID.PadLeft(10, "0")

                    strBarcode = strConBit & strWrapSequence & strBatch & strRun & strID
                    AddBarcodeToDataTable(dtDatabase, intMailingInfoId, intbatch, strRun, strBarcode, intBarCodeIndex)




                Next


            Loop 'next record



            'add barcode to database
            BARCODEBulkInsert(dtDatabase)

            'close all records-
            cmdsql.Dispose()
            rdSQL.Close()
            rdSQL = Nothing

        Catch ex As Exception
            ' ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basBarcode.CreateBarcodesForMailing" & ex.Message)
            Throw New System.Exception("Error creating bar codes" & ex.Message)
        End Try

    End Sub
    Private Sub DeleteBarcodesFor(ByVal intBatch As Integer, ByVal strrun As String)
        Try

        
        Dim cmdSQL As New SqlCommand

        With cmdSQL
            .CommandText = "Delete from PODLetterBarcode1 Where PODBatchID = " & intBatch.ToString & " AND PODRunID = '" & strrun & "'"
            .CommandType = CommandType.Text
                '  .Connection = New SqlConnection("Server=usasql;Database=PODFO;Trusted_Connection=True;")
                .Connection = sqlcon

                '.Connection.Open()
            .ExecuteNonQuery()

        End With
          Catch ex As Exception
            Throw New System.Exception("Error deleting barcodes" & ex.Message)
        End Try
    End Sub
    Public Function CreateBarCodeDataTable() As DataTable
        Try

        
        Dim dsBARCODE As New DataSet
        dsBARCODE.Locale = CultureInfo.InvariantCulture
        Dim dtBARCODE As DataTable

        dtBARCODE = dsBARCODE.Tables.Add("BARCODELETTER")

        With dtBARCODE.Columns

            .Add("PODMailingInfoID", Type.GetType("System.Decimal"))
            .Add("PODBatchID", Type.GetType("System.Decimal"))
            .Add("PODRunID", Type.GetType("System.String"))
            .Add("Barcode", Type.GetType("System.String"))
            .Add("BarcodeNumber", Type.GetType("System.Decimal"))

         

        End With

            Return dtBARCODE

         Catch ex As Exception
            Throw New System.Exception("error creating barcode datatable" & ex.Message)
        End Try

    End Function
    Public Sub BARCODEBulkInsert(ByRef dtBARCODE As DataTable)
        Try

       
            'Dim mOjbMainConn As New SqlConnection("Server=usasql;Database=PODFO;Trusted_Connection=True;")

            ' mOjbMainConn.Open()
            Dim mOjbMainConn As SqlConnection
            mOjbMainConn = sqlcon

        Using bulkcopy As SqlBulkCopy = New SqlBulkCopy(mOjbMainConn)
            bulkcopy.DestinationTableName = "PODLetterBarcode1"
            With bulkcopy.ColumnMappings

                .Add(New SqlBulkCopyColumnMapping("PODMailingInfoID", "PODMailingInfoID"))
                .Add(New SqlBulkCopyColumnMapping("PODBatchID", "PODBatchID"))
                .Add(New SqlBulkCopyColumnMapping("PODRunID", "PODRunID"))
                .Add(New SqlBulkCopyColumnMapping("Barcode", "Barcode"))
                .Add(New SqlBulkCopyColumnMapping("BarcodeNumber", "BarcodeNumber"))



            End With


            bulkcopy.WriteToServer(dtBARCODE)



        End Using


         Catch ex As Exception
            Throw New System.Exception("Error during bulk insert" & ex.Message)
        End Try

    End Sub
    Private Sub AddBarcodeToDataTable(ByRef dtBarcode As DataTable, intPODMailingInfoID As Integer, ByVal intBatch As Integer, ByVal strRun As String, ByVal strBarcode As String, ByVal intBarcodeNumber As Integer)
        'ByVal strBarcode1 As String, ByVal strBarcode2 As String, ByVal strBarcode3 As String, ByVal strBarcode4 As String, ByVal strBarcode5 As String)
        Try

            Dim drRow As DataRow

            drRow = dtBarcode.NewRow

            With drRow

                .Item("PODMailingInfoID") = intPODMailingInfoID
                .Item("PODBatchID") = intBatch
                .Item("PODRunID") = strRun
                .Item("Barcode") = strBarcode
                .Item("BarcodeNumber") = intBarcodeNumber



            End With

            dtBarcode.Rows.Add(drRow)
        Catch ex As Exception
            Throw New System.Exception("Error adding to barcode datatable" & ex.Message)
        End Try

    End Sub
    Public Function ConBit4(ByVal blnEnv As Boolean, ByVal blnEOC As Boolean, ByVal blnEdgeMark As Boolean, ByVal blnFeed1 As Boolean, ByVal blnFeed2 As Boolean) As String
        '*****************************************************************
        'This function returns back a single bit determined by the passed variables
        'It was copied from the foxpro code.  I elimnated some of the fox pro
        'code because it was unnecessary
        '
        'This bit tells the inserter whether to mark the envelope so user can see
        'where new tray starts and tells the inserter whether to add and envelope or not. 
        'As well as if it's the last page of a packet
        '
        '*****************************************************************

        Dim strChar As String = String.Empty


        ' strChar = "00"


        If blnFeed1 = True Then

            strChar = strChar & "1"

        Else

            strChar = strChar & "0"

        End If

        If blnFeed2 = True Then

            strChar = strChar & "1"

        Else

            strChar = strChar & "0"

        End If

        If blnEnv = True Then

            strChar = strChar & "1"

        Else

            strChar = strChar & "0"

        End If
        If blnEOC = True Then

            strChar = strChar & "1"

        Else

            strChar = strChar & "0"

        End If


        If blnEdgeMark = True Then

            strChar = strChar & "1"

        Else

            strChar = strChar & "0"

        End If


        Select Case strChar

            Case "00000"
                ConBit4 = "0"
            Case Is = "10000"
                ConBit4 = "1"
            Case "01000"
                ConBit4 = "2"
            Case "11000"
                ConBit4 = "3"
            Case "00100"
                ConBit4 = "4"
            Case "10100"
                ConBit4 = "5"
            Case "01100"
                ConBit4 = "6"
            Case "11100"
                ConBit4 = "7"
            Case "00010"
                ConBit4 = "8"
            Case "10010"
                ConBit4 = "9"
            Case "01010"
                ConBit4 = "A"
            Case "11010"
                ConBit4 = "B"
            Case "00110"
                ConBit4 = "C"
            Case "10110"
                ConBit4 = "D"
            Case "01110"
                ConBit4 = "E"
            Case "11110"
                ConBit4 = "F"
            Case "00001"
                ConBit4 = "G"
            Case "10001"
                ConBit4 = "H"
            Case "01001"
                ConBit4 = "I"
            Case "11001"
                ConBit4 = "J"
            Case "00101"
                ConBit4 = "K"
            Case "10101"
                ConBit4 = "L"
            Case "01101"
                ConBit4 = "M"
            Case "11101"
                ConBit4 = "N"
            Case "00011"
                ConBit4 = "O"
            Case "10011"
                ConBit4 = "P"
            Case "01011"
                ConBit4 = "Q"
            Case "11011"
                ConBit4 = "R"
            Case "00111"
                ConBit4 = "S"
            Case "10111"
                ConBit4 = "T"
            Case "01111"
                ConBit4 = "U"
            Case "11111"
                ConBit4 = "V"


        End Select

    End Function
    Public Function Collate(ByVal lngWrapSeq As Long) As String
        '*****************************************************************
        'Written by Pamela Alford 2/26/2006
        'The Inserter can uses 1 to v to count pages this function returns back the 
        'correct code for the inserter 1 to v then starts back at 1
        '*****************************************************************
        Dim lngRemainder As Long
        Dim ThisChar As String = " "

        lngRemainder = lngWrapSeq Mod 31

        Select Case lngRemainder
            Case 1
                ThisChar = "1"
            Case 2
                ThisChar = "2"
            Case 3
                ThisChar = "3"
            Case 4
                ThisChar = "4"
            Case 5
                ThisChar = "5"
            Case 6
                ThisChar = "6"
            Case 7
                ThisChar = "7"
            Case 8
                ThisChar = "8"
            Case 9
                ThisChar = "9"
            Case 10
                ThisChar = "A"
            Case 11
                ThisChar = "B"
            Case 12
                ThisChar = "C"
            Case 13
                ThisChar = "D"
            Case 14
                ThisChar = "E"
            Case 15
                ThisChar = "F"
            Case 16
                ThisChar = "G"
            Case 17
                ThisChar = "H"
            Case 18
                ThisChar = "I"
            Case 19
                ThisChar = "J"
            Case 20
                ThisChar = "K"
            Case 21
                ThisChar = "L"
            Case 22
                ThisChar = "M"
            Case 23
                ThisChar = "N"
            Case 24
                ThisChar = "O"
            Case 25
                ThisChar = "P"
            Case 26
                ThisChar = "Q"
            Case 27
                ThisChar = "R"
            Case 28
                ThisChar = "S"
            Case 29
                ThisChar = "T"
            Case 30
                ThisChar = "U"
            Case 0
                ThisChar = "V"



        End Select

        Collate = ThisChar

    End Function
End Module
