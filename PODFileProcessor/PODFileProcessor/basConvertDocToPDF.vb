Imports Microsoft.Office.Interop
Imports System.IO
Imports OfficeOpenXml
Imports OfficeOpenXml.Table
Imports ceTe
Imports Utilities

Module basConvertDocToPDF

    ' Get connection string
    Private Conn As String = Nothing
    ' Create DB Logger
    Private Logger As Logging = Nothing
    ' Create Config table access
    Private Conf As ConfigTable = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False

    Private ceTeLicense As String = Nothing


    Sub New()
        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB
        Logger = New Logging(Conn, "AppLog")
        Conf = New ConfigTable(Conn)
        ceTeLicense = GetParm("ceTeLicense", "DPS70NEDJGMGEGWKOnLLQb4SjhbTTJhXnkpf9bj8ZzxFH+FFxctoPX+HThGxkpidUCHJ5b88fg4oUJSHiRBggzHdghUgkkuIvoag")
    End Sub


    Public Sub ConvertDocToPDF(ByVal strFolderName As String, ByVal intZipID As Integer)
        Try

            ceTe.DynamicPDF.Document.AddLicense(ceTeLicense)

            Dim strSelectFrom As String = "Select * From "
            Dim strCSVFiles As String() = Directory.GetFiles(strFolderName, "*.csv")
            Dim strCurrentCSVFile As String
            Dim objFile As FileInfo
            Dim objExcelFile As FileInfo
            Dim objConn As OleDb.OleDbConnection
            Dim objCommand As OleDb.OleDbCommand
            Dim objDataset As New DataSet
            Dim objDataRow As DataRow
            Dim objDataAdapter As OleDb.OleDbDataAdapter
            Dim objExcelPackage As ExcelPackage
            Dim objWorksheet As ExcelWorksheet
            Dim strFileName As String
            Dim objDataColumn As DataColumn
            Dim intColumn As Integer = 1
            Dim intRow As Integer = 1
            Dim strLetterFileName As String
            Dim oMainDoc As Word.Document
            Dim oWord As Word.Application
            Dim strPDFFolder As String = strFolderName & "PDFs\"
            Dim intLetterCount As Integer
            Dim objNewPDF As FileInfo
            Dim intPageCount As Integer
            Dim strMissingPDFNames As String = ""
            'MessageBox.Show("start")
            If Directory.Exists(strPDFFolder) = False Then

                Directory.CreateDirectory(strPDFFolder)

            End If

            oWord = New Word.Application

            'Connect to folder
            'MessageBox.Show("about to hit jet call")

            objConn = New OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" & _
                                                "Data Source=" & strFolderName & ";" & _
                                                 "Extended Properties=""text;HDR=YES;FMT=Delimited""")

            'open connection
            objConn.Open()


            'MessageBox.Show("objConn.Open() done")
            ''loop through csv files
            For Each strCurrentCSVFile In strCSVFiles




                objFile = New FileInfo(strCurrentCSVFile)
                strFileName = objFile.Name.Substring(0, objFile.Name.Length - 4)




                'create excel file to hold records

                If File.Exists(mstrWCPODProcessedFolder & strFileName & ".xlsx") Then
                    File.Delete(mstrWCPODProcessedFolder & strFileName & ".xlsx")
                End If
                objExcelFile = New FileInfo(mstrWCPODProcessedFolder & strFileName & ".xlsx")
                objExcelPackage = New ExcelPackage(objExcelFile)
                objWorksheet = objExcelPackage.Workbook.Worksheets.Add("Sheet1")


                'setup command to read CSV
                objCommand = New OleDb.OleDbCommand
                objCommand.CommandText = strSelectFrom & "[" & objFile.Name & "]"
                objCommand.CommandType = CommandType.Text
                objCommand.Connection = objConn
                objDataAdapter = New OleDb.OleDbDataAdapter(objCommand)

                objDataAdapter.Fill(objDataset)

                'create header column
                For Each objDataColumn In objDataset.Tables(0).Columns

                    objWorksheet.Cells(intRow, intColumn).Value = objDataColumn.ColumnName
                    intColumn = intColumn + 1

                Next
                WriteToLogfile("  objWorksheet.Cells(intRow, intColumn).Value = Page Count")
                objWorksheet.Cells(intRow, intColumn).Value = "Page Count"
                objWorksheet.Cells(intRow, intColumn + 1).Value = "ZipFileID"
                objWorksheet.Cells(intRow, intColumn + 2).Value = "WCFolder"
                WriteToLogfile(" objWorksheet.Cells DONE")
                For Each objDataRow In objDataset.Tables(0).Rows

                    strLetterFileName = objDataRow.Item("Filename").ToString.Trim

                    'if file exits open
                    If File.Exists(strFolderName & "\" & strLetterFileName) Then

                        intRow = intRow + 1

                        intLetterCount = intLetterCount + 1

                        Dim objWordDoc As New FileInfo(strFolderName & "\" & strLetterFileName)

                        'open document in word
                        oMainDoc = oWord.Documents.Add(strFolderName & "\" & strLetterFileName)




                        objFile = New FileInfo(strFolderName & "\" & strLetterFileName)
                        strFileName = objFile.Name & ".PDF"


                        Dim strPDFNameandPath As String = strPDFFolder & strFileName

                        'save as pdf

                        oMainDoc.SaveAs(strPDFNameandPath, Word.WdSaveFormat.wdFormatPDF)




                        'DELETE DOCUMENT
                        objWordDoc.Delete()


                        'get pdf pagecount
                        Dim pdf As New ceTe.DynamicPDF.Merger.MergeDocument(strPDFNameandPath)
                        intPageCount = pdf.Pages.Count


                        'insert blank pages every other page
                        Dim intIndex As Integer
                        Dim BlankPage As ceTe.DynamicPDF.Page = New ceTe.DynamicPDF.Page
                        Dim intPages As Integer = intPageCount * 2
                        For intIndex = 1 To intPages Step 2

                            '  Dim intPageNumber As Integer = intIndex + (intIndex - 1)

                            pdf.Pages.Insert(intIndex, BlankPage)

                        Next



                        Dim AddressPage As ceTe.DynamicPDF.Page = New ceTe.DynamicPDF.Page()

                        'front address page
                        pdf.Pages.Insert(0, AddressPage)

                        'back of address page
                        pdf.Pages.Insert(0, AddressPage)



                        If pdf.Pages.Count Mod 2 <> 0 Then


                            pdf.Pages.Add(BlankPage)
                            pdf.FormFlattening = DynamicPDF.Merger.FormFlatteningOptions.Default
                            pdf.CompressionLevel = 0
                            pdf.PdfVersion = ceTe.DynamicPDF.PdfVersion.v1_3

                        End If
                        intPageCount = pdf.Pages.Count


                        'get old file info
                        Dim objOldPDF As New FileInfo(strPDFNameandPath)
                        'draw new pdf in the processed folder

                        pdf.Draw(mstrWCPODProcessedFolder & intZipID.ToString & "-" & objOldPDF.Name)



                        'delete old pdf
                        objOldPDF.Delete()

                        'COPY NEW PDF TO MAIN PROCESSING FOLDER
                        objNewPDF = New FileInfo(mstrWCPODProcessedFolder & intZipID.ToString & "-" & objOldPDF.Name)


                        objNewPDF.CopyTo(mstrMainWCProcessFolder & objNewPDF.Name, True)

                    Else

                        If strLetterFileName.Trim.ToUpper <> "FILENAME" Then
                            intRow = intRow + 1
                            intPageCount = -1
                            If strMissingPDFNames.Trim.Length > 0 Then
                                strMissingPDFNames = strMissingPDFNames & "," & strLetterFileName.Trim
                            Else
                                strMissingPDFNames = strLetterFileName
                            End If
                        End If
                    End If


                    'update pagecount
                    With objDataRow

                        'Don't re-add the header rows
                        If strLetterFileName.Trim.ToUpper <> "FILENAME" Then

                            objWorksheet.Cells(intRow, 1).Value = .Item("Filename").ToString
                            objWorksheet.Cells(intRow, 2).Value = .Item("Recipient Count").ToString
                            objWorksheet.Cells(intRow, 3).Value = .Item("HICN").ToString
                            objWorksheet.Cells(intRow, 4).Value = .Item("Name").ToString
                            objWorksheet.Cells(intRow, 5).Value = .Item("Address Line 1").ToString
                            objWorksheet.Cells(intRow, 6).Value = .Item("Address Line 2").ToString
                            objWorksheet.Cells(intRow, 7).Value = .Item("Address Line 3").ToString
                            objWorksheet.Cells(intRow, 8).Value = .Item("City").ToString
                            objWorksheet.Cells(intRow, 9).Value = .Item("State").ToString
                            objWorksheet.Cells(intRow, 10).Value = .Item("Zipcode").ToString
                            objWorksheet.Cells(intRow, 11).Value = .Item("LOB").ToString
                            objWorksheet.Cells(intRow, 12).Value = .Item("Contract").ToString
                            objWorksheet.Cells(intRow, 13).Value = .Item("Internal?").ToString
                            objWorksheet.Cells(intRow, 14).Value = .Item("Document ID").ToString
                            objWorksheet.Cells(intRow, 15).Value = .Item("Document Created Date").ToString
                            objWorksheet.Cells(intRow, 16).Value = .Item("Activity ID").ToString
                            objWorksheet.Cells(intRow, 17).Value = .Item("Mainframe Correspondence ID").ToString
                            objWorksheet.Cells(intRow, 18).Value = .Item("Correspondence Case ID").ToString
                            objWorksheet.Cells(intRow, 19).Value = .Item("CMS Foreign Address Indicator").ToString
                            objWorksheet.Cells(intRow, 20).Value = intPageCount
                            objWorksheet.Cells(intRow, 21).Value = intZipID
                            objWorksheet.Cells(intRow, 22).Value = mstrWCPODProcessedFolder

                        End If
                    End With





                    If intLetterCount > 200 Then
                        oWord.Quit()
                        oWord = Nothing
                        oWord = New Word.Application

                        intLetterCount = 1
                    End If
                Next

                objExcelPackage.Save()

                objExcelPackage.Dispose()
                objWorksheet = Nothing
                objFile = Nothing
                objExcelFile = Nothing
                objCommand.Dispose()
                objDataAdapter.Dispose()

                'Dim objCSVFile As New FileInfo(strCurrentCSVFile)
                'objCSVFile.MoveTo(mstrWCPODPreprocessArchiveFolder & objCSVFile.Name)

                If strMissingPDFNames.Trim.Length > 0 Then
                    ReportsApplication1.clsEmail.EmailMessage("Error Possible in PODFO", "There seem to be some missing PDF(s) : " & vbCrLf & strMissingPDFNames.Replace(",", vbCrLf))
                End If

            Next
            'MessageBox.Show("Done with convert Doc to PDF")
            oMainDoc = Nothing
            oWord.Quit()
            oWord = Nothing
        Catch ex As Exception
            '    MessageBox.Show("Error from basConvertDocToPDF.ConvertDocToPDF" & ex.Message)
            'MessageBox.Show("Error " & ex.Message)
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from basConvertDocToPDF.ConvertDocToPDF" & ex.Message)
        End Try




    End Sub


    Private Sub Trythis()
        'ceTe.DynamicPDF.Document.AddLicense("DPS70NEDJGMGEGWKOnLLQb4SjhbTTJhXnkpf9bj8ZzxFH+FFxctoPX+HThGxkpidUCHJ5b88fg4oUJSHiRBggzHdghUgkkuIvoag")
        ceTe.DynamicPDF.Document.AddLicense(ceTeLicense)
        Dim strPDFNameandPath As String = "Q:\Pam\000000\PODFO\PDF\00882_QL BCRC Referral - B_062614_7-9Y4HHWG.pdf"

        strPDFNameandPath = GetParm("strPDFNameandPath", "Q:\Pam\000000\PODFO\PDF\00882_QL BCRC Referral - B_062614_7-9Y4HHWG.pdf")

        Dim pdf As New ceTe.DynamicPDF.Merger.MergeDocument(strPDFNameandPath)
        Dim blankPage As ceTe.DynamicPDF.Page = New ceTe.DynamicPDF.Page()
        blankPage.Dimensions.LeftMargin = 2
        Dim barcode As DynamicPDF.PageElements.BarCoding.Code39 = New DynamicPDF.PageElements.BarCoding.Code39("test", 0, 500, 36)
        barcode.Angle = 270
        blankPage.Elements.Add(barcode)


        pdf.Pages.Add(blankPage)
        pdf.FormFlattening = DynamicPDF.Merger.FormFlatteningOptions.Default
        pdf.CompressionLevel = 0
        pdf.PdfVersion = ceTe.DynamicPDF.PdfVersion.v1_3
        Dim strTempFile As String = strPDFNameandPath & "-2.pdf"
        Dim objNewPDF As New FileInfo(strTempFile)
        pdf.Draw(strTempFile)


        pdf = Nothing
        blankPage = Nothing
        barcode = Nothing

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
