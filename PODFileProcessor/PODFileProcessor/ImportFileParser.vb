
Imports System.IO
Imports System.Xml
Imports System.Data.SqlClient
Imports Excel = Microsoft.Office.Interop.Excel
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports System.Data.OleDb

Module ImportFileParser
    Public Sub ParseACOFile(ByRef objMainConn As SqlConnection, ByVal strFileName As String, intImportFileID As Integer, intBatch As Integer, strRun As String)
        Try
            Dim srReader As StreamReader = File.OpenText(strFileName)
            Dim strLine As String
            Dim intCurrentField As Integer = 0
            Dim strHICN As String = ""
            Dim strMailName As String = ""
            Dim strFirstName As String = "'"
            Dim strMiddleName As String = ""
            Dim strLastName As String = ""
            Dim strAdd1 As String = ""
            Dim strAdd2 As String = ""
            Dim strAdd3 As String = ""
            Dim strAdd4 As String = ""
            Dim strAdd5 As String = ""
            Dim strCity As String = ""
            Dim strState As String = ""
            Dim strZip As String = ""
            Dim strPlus4 As String = ""
            Dim strCityStateZip As String = ""
            Dim strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE As String = ""
            Dim strACO_LEGAL_NAME As String = ""
            Dim strPUBLICATION_LANGUAGE_PREFERENCE As String = ""
            Dim strDATA_SHARING As String = ""
            Dim strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE As String = ""
            Dim strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR As String = ""
            Dim strRowID As String = ""
            Dim strLanguage As String = ""
            Dim strSubType As String = ""
            Dim intMasterLetterType As Integer
            Dim intPageCount As Integer
            Dim dblWeight As Double
            Dim dblThickness As Double
            Dim strKey As String = ""
            Dim strValue As String = ""
            Dim dtACO As DataTable
            Dim blnImport As Boolean = False


            'Create Datatable for records
            dtACO = CreateACODataTable()


            'read start record
            strLine = srReader.ReadLine



            Do While Not IsNothing(strLine)

                strKey = ""
                strValue = ""

                'get string value pair
                ReturnKeyValue(strLine, strKey, strValue)

                Select Case strKey

                    Case "RECORD_STARTS"
                        'start of record
                        'strLine = srReader.ReadLine

                    Case "HICN"
                        strHICN = strValue.Trim
                    Case "FIRST_NAME"
                        strFirstName = strValue.Trim
                    Case "MIDDLE_NAME"
                        strMiddleName = strValue.Trim
                    Case "LAST_NAME"
                        strLastName = strValue.Trim
                    Case "ADDRESS_LINE_1"
                        strAdd1 = strValue.Trim
                    Case "ADDRESS_LINE_2"
                        strAdd2 = strValue.Trim
                    Case "ADDRESS_LINE_3"
                        strAdd3 = strValue.Trim
                    Case "CITY"
                        strCity = strValue.Trim
                    Case "STATE"
                        strState = strValue.Trim
                    Case "ZIP_CODE"
                        strZip = strValue.Trim
                    Case "ZIP_CODE_PLUS_4"
                        strPlus4 = strValue.Trim
                    Case "BENEFICIARY_DATA_SHARING_EFFECTIVE_DATE"
                        strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE = strValue.Trim
                    Case "ACO_LEGAL_NAME"
                        strACO_LEGAL_NAME = strValue.Trim
                    Case "PUBLICATION_LANGUAGE_PREFERENCE"
                        strPUBLICATION_LANGUAGE_PREFERENCE = strValue.Trim
                    Case "DATA_SHARING"
                        strDATA_SHARING = strValue.Trim
                    Case "BENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE"
                        strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE = strValue.Trim
                    Case "BENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR"
                        strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR = strValue.Trim
                    Case "ROW_ID"
                        strRowID = strValue.Trim

                    Case "RECORD_ENDS"
                        'end of record import this record

                        'determine Language
                        If strPUBLICATION_LANGUAGE_PREFERENCE.Trim.ToUpper = "ENGLISH" Then
                            strLanguage = "ENG"
                        Else
                            strLanguage = "SPAN"
                        End If

                        '   If strRowID = "7-A6SYLWQ" Then Stop

                        'determine letter subtype
                        strSubType = FindACOLetterType(strACO_LEGAL_NAME, strDATA_SHARING, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR)

                        'get master letter type id from dbs
                        intMasterLetterType = GetLetterTypeID(objMainConn, "ACO", strSubType, strLanguage, intPageCount, dblWeight, dblThickness)


                        'concat mailname
                        strMailName = strFirstName
                        If strMiddleName.Trim.Length <> 0 Then
                            strMailName = strMailName & " " & strMiddleName
                        End If
                        strMailName = strMailName & " " & strLastName


                        'concat CityStateZip
                        strCityStateZip = strCity & ", " & strState & " " & strZip

                        If strPlus4.Trim.Length > 0 Then
                            strCityStateZip = strCityStateZip & "-" & strPlus4
                        End If

                        'insert record into db
                        'InsertPODACOLETTERINFO(strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE, strACO_LEGAL_NAME, _
                        '                      strDATA_SHARING, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR, strRowID, intMasterLetterType, intImportFileID, intBatch, strRun)

                        'insert values into datatable
                        ACOInsertIntoDT(dtACO, strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE, strACO_LEGAL_NAME, _
                                             strDATA_SHARING, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR, strRowID, intMasterLetterType, intImportFileID, intBatch, strRun, intPageCount, dblWeight, dblThickness)

                        blnImport = True

                        'clear fields
                        strHICN = ""
                        strMailName = ""
                        strFileName = ""
                        strMiddleName = ""
                        strLastName = ""
                        strAdd1 = ""
                        strAdd2 = ""
                        strAdd3 = ""
                        strAdd4 = ""
                        strAdd5 = ""
                        strCity = ""
                        strState = ""
                        strZip = ""
                        strPlus4 = ""
                        strCityStateZip = ""
                        strCity = ""
                        strState = ""
                        strZip = ""
                        strPlus4 = ""
                        strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE = ""
                        strACO_LEGAL_NAME = ""
                        strPUBLICATION_LANGUAGE_PREFERENCE = ""
                        strDATA_SHARING = ""
                        strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE = ""
                        strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR = ""
                        strRowID = ""
                        intMasterLetterType = -1




                End Select

                'read next field
                strLine = srReader.ReadLine
            Loop

            srReader.Close()
            srReader.Dispose()

            'case of zero lenght file
            If blnImport = True Then
                'bulk insert datatable
                ACOBulkInsert(dtACO, objMainConn)
            End If

            dtACO.Dispose()
        Catch ex As Exception
            Throw New System.Exception("Error parsing ACO files", ex)
        End Try
    End Sub
    Public Sub ParseDISFile(ByRef objMainConn As SqlConnection, ByVal strFileName As String, intImportFileID As Integer, intBatch As Integer, strRun As String)
        Try

            Dim intLastRecord As Integer = 17
            Dim srReader As StreamReader = File.OpenText(strFileName)
            Dim strLine As String
            Dim intCurrentField As Integer = 0
            Dim strCurrentDate As String = ""
            Dim strLetterType As String = ""
            Dim strHICN As String = ""
            Dim strMailName As String = ""
            Dim strAdd1 As String = ""
            Dim strAdd2 As String = ""
            Dim strAdd3 As String = ""
            Dim strAdd4 As String = ""
            Dim strAdd5 As String = ""
            Dim strCityStateZip As String = ""
            Dim strStartDate As String = ""
            Dim strAcitivityId As String = ""
            Dim strPlanName As String = ""
            Dim strTerminationDate As String = ""
            Dim strLanguage As String = ""
            Dim strCreateBy As String = ""
            Dim strRowID As String = ""
            Dim strContractorDefined1 As String = ""
            Dim strContractorDefined2 As String = ""
            Dim intMasterLetterType As Integer
            Dim intPageCount As Integer
            Dim dblWeight As Double
            Dim dblThickness As Double
            Dim strKey As String = ""
            Dim strValue As String = ""
            Dim dtDIS As DataTable
            Dim blnImport As Boolean = False

            'Create Datatable for records
            dtDIS = CreateDISDataTable()

            'read start record
            strLine = srReader.ReadLine



            Do While Not IsNothing(strLine)

                strKey = ""
                strValue = ""

                'get string value pair
                ReturnKeyValue(strLine, strKey, strValue)


                Select Case strKey

                    Case "RECORD_STARTS"
                        'start of record
                        'strLine = srReader.ReadLine
                    Case "CURRENT_DATE"
                        strCurrentDate = strValue.Trim
                    Case "LETTER_TYPE"
                        strLetterType = strValue.Trim
                    Case "HICN"
                        strHICN = strValue.Trim
                    Case "BENEFICIARY_FULL_NAME"
                        strMailName = strValue.Trim
                    Case "STREET_ADDRESS"
                        strAdd1 = strValue.Trim
                    Case "STREET_ADDRESS2"
                        strAdd2 = strValue.Trim
                    Case "CITY_STATE_ZIP"
                        strCityStateZip = strValue.Trim
                    Case "CALL_START_DATE_TIME"
                        strStartDate = strValue.Trim
                    Case "ACTIVITY_ID"
                        strAcitivityId = strValue.Trim
                    Case "PLAN_NAME"
                        strPlanName = strValue.Trim
                    Case "TERMINATION_DATE"
                        strTerminationDate = strValue.Trim
                    Case "LANGUAGE_PREFERENCE"
                        strLanguage = strValue.Trim
                    Case "CREATED_BY"
                        strCreateBy = strValue.Trim
                    Case "ROW_ID"
                        strRowID = strValue.Trim
                    Case "CONTRACTOR_DEFINED_1"
                        strContractorDefined1 = strValue.Trim
                    Case "CONTRACTOR_DEFINED_2"
                        strContractorDefined1 = strValue.Trim

                    Case "RECORD_ENDS"
                        'end of record import this record


                        Dim strLangAbrev As String

                        If strLanguage.ToUpper.Trim = "ENGLISH" Or strLanguage.ToUpper.Trim = "ENG" Then
                            strLangAbrev = "ENG"
                        Else
                            strLangAbrev = "SPAN"
                        End If
                        'get the letter type id
                        intMasterLetterType = GetLetterTypeID(objMainConn, "DIS", strLetterType.Trim.ToUpper, strLangAbrev, intPageCount, dblWeight, dblThickness)

                        'insert into db
                        'InsertPODDISLETTERINFO(strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strCurrentDate, strStartDate, strAcitivityId, _
                        '                       strPlanName, strTerminationDate, strCreateBy, strContractorDefined1, strContractorDefined2, intMasterLetterType, intImportFileID, intBatch, strRun)

                        DISInsertIntoDT(dtDIS, strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strCurrentDate, strStartDate, strAcitivityId, _
                                              strPlanName, strTerminationDate, strCreateBy, strContractorDefined1, strContractorDefined2, intMasterLetterType, intImportFileID, intBatch, strRun, intPageCount, dblWeight, dblThickness)



                        blnImport = True
                        'clear fields
                        strCurrentDate = ""
                        strLetterType = ""
                        strHICN = ""
                        strMailName = ""
                        strAdd1 = ""
                        strAdd2 = ""
                        strAdd3 = ""
                        strAdd4 = ""
                        strAdd5 = ""
                        strCityStateZip = ""
                        strStartDate = ""
                        strAcitivityId = ""
                        strPlanName = ""
                        strTerminationDate = ""
                        strLanguage = ""
                        strCreateBy = ""
                        strRowID = ""
                        strContractorDefined1 = ""
                        strContractorDefined2 = ""
                        intMasterLetterType = -1


                        'set counter back to zero
                        intCurrentField = 0


                End Select

                'read next field
                strLine = srReader.ReadLine
            Loop

            srReader.Close()

            'case of zero lenght file
            If blnImport = True Then
                'bulk insert datatable
                DISBulkInsert(dtDIS, objMainConn)
            End If

            dtDIS.Dispose()

        Catch ex As Exception
            Throw New System.Exception("Error parsing DIS files", ex)
        End Try
    End Sub
    Public Sub ParseCPCFile(ByRef objMainConn As SqlConnection, ByVal strFileName As String, intImportFileID As Integer, intBatch As Integer, strRun As String)
        Try
            Dim srReader As StreamReader = File.OpenText(strFileName)
            Dim strLine As String
            Dim strHICN As String = ""
            Dim strMailName As String = ""
            Dim strFirstName As String = "'"
            Dim strMiddleName As String = ""
            Dim strLastName As String = ""
            Dim strAdd1 As String = ""
            Dim strAdd2 As String = ""
            Dim strAdd3 As String = ""
            Dim strAdd4 As String = ""
            Dim strAdd5 As String = ""
            Dim strCity As String = ""
            Dim strState As String = ""
            Dim strZip As String = ""
            Dim strPlus4 As String = ""
            Dim strCityStateZip As String = ""
            Dim strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE As String = ""
            Dim strCPC_PRACTICE_NAME As String = ""
            Dim strPUBLICATION_LANGUAGE_PREFERENCE As String = ""
            Dim strDATA_SHARING As String = ""
            Dim strRowID As String = ""
            Dim strLanguage As String = ""
            Dim strSubType As String = ""
            Dim intMasterLetterType As Integer
            Dim intPageCount As Integer
            Dim dblWeight As Double
            Dim dblThickness As Double
            Dim strKey As String = ""
            Dim strValue As String = ""
            Dim dtCPC As DataTable
            Dim blnImport As Boolean = False

            'Create Datatable for records
            dtCPC = CreateCPCDataTable()

            'read start record
            strLine = srReader.ReadLine



            Do While Not IsNothing(strLine)

                strKey = ""
                strValue = ""

                'get string value pair
                ReturnKeyValue(strLine, strKey, strValue)

                Select Case strKey

                    Case "RECORD_STARTS"
                        'start of record
                        'strLine = srReader.ReadLine

                    Case "HICN"
                        strHICN = strValue.Trim
                    Case "FIRST_NAME"
                        strFirstName = strValue.Trim
                    Case "MIDDLE_NAME"
                        strMiddleName = strValue.Trim
                    Case "LAST_NAME"
                        strLastName = strValue.Trim
                    Case "ADDRESS_LINE_1"
                        strAdd1 = strValue.Trim
                    Case "ADDRESS_LINE_2"
                        strAdd2 = strValue.Trim
                    Case "ADDRESS_LINE_3"
                        strAdd3 = strValue.Trim
                    Case "CITY"
                        strCity = strValue.Trim
                    Case "STATE"
                        strState = strValue.Trim
                    Case "ZIP_CODE"
                        strZip = strValue.Trim
                    Case "ZIP_CODE_PLUS_4"
                        strPlus4 = strValue.Trim
                    Case "BENEFICIARY_DATA_SHARING_EFFECTIVE_DATE"
                        strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE = strValue.Trim
                    Case "CPC_PRACTICE_NAME"
                        strCPC_PRACTICE_NAME = strValue.Trim
                    Case "PUBLICATION_LANGUAGE_PREFERENCE"
                        strPUBLICATION_LANGUAGE_PREFERENCE = strValue.Trim
                    Case "DATA_SHARING"
                        strDATA_SHARING = strValue.Trim
                    Case "ROW_ID"
                        strRowID = strValue.Trim

                    Case "RECORD_ENDS"
                        'end of record insert into databse


                        'determine Language
                        If strPUBLICATION_LANGUAGE_PREFERENCE.Trim.ToUpper = "ENGLISH" Then
                            strLanguage = "ENG"
                        Else
                            strLanguage = "SPAN"
                        End If

                        'determine letter subtype
                        If strDATA_SHARING.Trim.ToUpper = "Y" Then
                            strSubType = "OUTR"
                        Else
                            strSubType = "OUT"
                        End If

                        'get master letter type id from dbs
                        intMasterLetterType = GetLetterTypeID(objMainConn, "CPC", strSubType, strLanguage, intPageCount, dblWeight, dblThickness)


                        'concat mailname
                        strMailName = strFirstName
                        If strMiddleName.Trim.Length <> 0 Then
                            strMailName = strMailName & " " & strMiddleName
                        End If
                        strMailName = strMailName & " " & strLastName


                        'concat CityStateZip
                        strCityStateZip = strCity & ", " & strState & " " & strZip

                        If strPlus4.Trim.Length > 0 Then
                            strCityStateZip = strCityStateZip & "-" & strPlus4
                        End If

                        'insert record into db
                        'InsertPODCPCLETTERINFO(strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE, strCPC_PRACTICE_NAME, _
                        '                      strDATA_SHARING, strRowID, intMasterLetterType, intImportFileID, intBatch, strRun)


                        CPCInsertIntoDT(dtCPC, strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE, strCPC_PRACTICE_NAME, _
                                              strDATA_SHARING, strRowID, intMasterLetterType, intImportFileID, intBatch, strRun, intPageCount, dblWeight, dblThickness)


                        blnImport = True

                        'clear fields
                        strHICN = ""
                        strMailName = ""
                        strFileName = ""
                        strMiddleName = ""
                        strLastName = ""
                        strAdd1 = ""
                        strAdd2 = ""
                        strAdd3 = ""
                        strAdd4 = ""
                        strAdd5 = ""
                        strCity = ""
                        strState = ""
                        strZip = ""
                        strPlus4 = ""
                        strCityStateZip = ""
                        strCity = ""
                        strState = ""
                        strZip = ""
                        strPlus4 = ""
                        strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE = ""
                        strCPC_PRACTICE_NAME = ""
                        strPUBLICATION_LANGUAGE_PREFERENCE = ""
                        strDATA_SHARING = ""
                        strRowID = ""
                        intMasterLetterType = -1




                End Select

                'read next field
                strLine = srReader.ReadLine
            Loop

            srReader.Close()

            'case of zero lenght file
            If blnImport = True Then
                'bulk insert datatable
                CPCBulkInsert(dtCPC, objMainConn)
            End If
            dtCPC.Dispose()
        Catch ex As Exception
            Throw New System.Exception("Error parsing CPC files", ex)
        End Try
    End Sub
    Public Sub ParseENTFile(ByRef objMainConn As SqlConnection, ByVal strFileName As String, intImportFileID As Integer, intBatch As Integer, strRun As String)
        Try
            Dim srReader As StreamReader = File.OpenText(strFileName)
            Dim strLine As String
            Dim strHICN As String = ""
            Dim strMailName As String = ""
            Dim strFirstName As String = "'"
            Dim strMiddleName As String = ""
            Dim strLastName As String = ""
            Dim strAdd1 As String = ""
            Dim strAdd2 As String = ""
            Dim strAdd3 As String = ""
            Dim strAdd4 As String = ""
            Dim strAdd5 As String = ""
            Dim strCity As String = ""
            Dim strState As String = ""
            Dim strZip As String = ""
            Dim strPlus4 As String = ""
            Dim strCityStateZip As String = ""
            Dim strPartAEntitlementStartDate As String = ""
            Dim strPartBEntitlementStartDate As String = ""
            Dim strCurrentDate As String = ""
            Dim strSystem As String = ""
            Dim strLanguage As String = ""
            Dim strRowID As String = ""
            Dim strSubType As String = ""
            Dim intMasterLetterType As Integer
            Dim intPageCount As Integer
            Dim dblWeight As Double
            Dim dblThickness As Double
            Dim dtENT As DataTable
            Dim blnImport As Boolean = False
            'Create Datatable for records
            dtENT = CreateENTDataTable()

            'read start record
            strLine = srReader.ReadLine
            Do While Not IsNothing(strLine)

                If strLine.Length = 240 Then



                    strHICN = strLine.Substring(0, 11).Trim
                    strFirstName = strLine.Substring(11, 15).Trim
                    strMiddleName = strLine.Substring(26, 1).Trim
                    strLastName = strLine.Substring(27, 24).Trim
                    strAdd1 = strLine.Substring(51, 22).Trim
                    strAdd2 = strLine.Substring(73, 22).Trim
                    strAdd3 = strLine.Substring(95, 22).Trim
                    strAdd4 = strLine.Substring(117, 22).Trim
                    strAdd5 = strLine.Substring(139, 22).Trim
                    strCity = strLine.Substring(161, 19).Trim
                    strState = strLine.Substring(180, 2).Trim
                    strZip = strLine.Substring(182, 5).Trim
                    strPlus4 = strLine.Substring(187, 4).Trim
                    strPartAEntitlementStartDate = strLine.Substring(191, 10).Trim
                    strPartBEntitlementStartDate = strLine.Substring(201, 10).Trim
                    strLanguage = strLine.Substring(211, 1).Trim
                    strRowID = strLine.Substring(212, 15).Trim
                    strCurrentDate = strLine.Substring(227, 10).Trim
                    strSystem = strLine.Substring(237, 3).Trim


                    ''determine Language
                    'If strLanguage.Trim.ToUpper = "E" Then
                    '    strLanguage = "ENG"
                    'Else
                    '    strLanguage = "SPAN"
                    'End If

                    'currently always english
                    strLanguage = "ENG"

                    'get master letter type id from dbs
                    intMasterLetterType = GetLetterTypeID(objMainConn, "ENT", strSystem, strLanguage, intPageCount, dblWeight, dblThickness)


                    'concat mailname
                    strMailName = strFirstName
                    If strMiddleName.Trim.Length <> 0 Then
                        strMailName = strMailName & " " & strMiddleName
                    End If
                    strMailName = strMailName & " " & strLastName


                    'concat CityStateZip
                    strCityStateZip = strCity & ", " & strState & " " & strZip

                    If strPlus4.Trim.Length > 0 Then
                        strCityStateZip = strCityStateZip & "-" & strPlus4
                    End If

                    'insert record into db
                    'InsertPODENTLETTERINFO(strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strPartAEntitlementStartDate, strPartBEntitlementStartDate, _
                    '                      strCurrentDate, strSystem, strRowID, intMasterLetterType, intImportFileID, intBatch, strRun)


                    ENTInsertIntoDT(dtENT, strHICN, strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strPartAEntitlementStartDate, strPartBEntitlementStartDate, _
                                         strCurrentDate, strSystem, strRowID, intMasterLetterType, intImportFileID, intBatch, strRun, intPageCount, dblWeight, dblThickness)


                    blnImport = True

                    'clear fields
                    strHICN = ""
                    strMailName = ""
                    strFileName = ""
                    strMiddleName = ""
                    strLastName = ""
                    strAdd1 = ""
                    strAdd2 = ""
                    strAdd3 = ""
                    strAdd4 = ""
                    strAdd5 = ""
                    strCity = ""
                    strState = ""
                    strZip = ""
                    strPlus4 = ""
                    strCityStateZip = ""
                    strCity = ""
                    strState = ""
                    strZip = ""
                    strPlus4 = ""
                    strPartAEntitlementStartDate = ""
                    strPartBEntitlementStartDate = ""
                    strCurrentDate = ""
                    strSystem = ""
                    strRowID = ""
                    intMasterLetterType = -1


                End If



                'read next field
                strLine = srReader.ReadLine
            Loop

            srReader.Close()

            'case of zero lenght file
            If blnImport = True Then
                ENTBulkInsert(dtENT, objMainConn)
            End If

            dtENT.Dispose()
        Catch ex As Exception
            Throw New System.Exception("Error parsing ENT files", ex)
        End Try
    End Sub
    Public Sub ParseMBPFiles(ByRef objMainConn As SqlConnection, ByVal strFolder As String, intImportFileID As Integer, intBatch As Integer, strRun As String)
        Try
            Dim objXMLDoc As New XmlDocument
            Dim objNodeList As XmlNodeList
            Dim objNode As XmlNode
            Dim objChildNode As XmlNode
            Dim objInnerChildNode As XmlNode
            Dim strFirstName As String = ""
            Dim strMiddleName As String = ""
            Dim strLastName As String = ""
            Dim strMailingAddress As String = ""
            Dim strPassword As String = ""
            Dim strRegDate As String = ""
            Dim strLanguage As String = ""
            Dim strPatternedPassword As String = ""
            Dim strCSRREsetPasword As String = ""
            Dim strAuthorizedRepRelationship As String = ""
            Dim strAuthorizedRepName As String = ""
            Dim strFiles As String() = Directory.GetFiles(strFolder, "*.xml")
            Dim strFileName As String
            Dim strMailName As String = ""
            Dim strAddressParts As String()
            Dim strCityStateZip As String = ""
            Dim strAdd1 As String = ""
            Dim strAdd2 As String = ""
            Dim strAdd3 As String = ""
            Dim strAdd4 As String = ""
            Dim strAdd5 As String = ""
            Dim intMasterLetterType As Integer
            Dim strSubType As String
            Dim dtMBP As DataTable
            Dim intPageCount As Integer
            Dim dblWeight As Double
            Dim dblThickness As Double
            Dim objFileinfo As FileInfo
            Dim intDash As Integer
            Dim intDot As Integer
            Dim strCustomerID As String
            Dim blnImport As Boolean = False


            'Create Datatable for records
            dtMBP = CreateMBPDataTable()

            For Each strFileName In strFiles

                objFileinfo = New FileInfo(strFileName)
                intDash = InStr(objFileinfo.Name, "_")
                intDot = InStr(objFileinfo.Name, ".")

                If intDash <> 0 Then
                    strCustomerID = objFileinfo.Name.Substring(intDash, (intDot - intDash) - 1)

                End If

                objXMLDoc.Load(strFileName)

                'select node list
                objNodeList = objXMLDoc.SelectNodes("/REGISTRANT")

                For Each objNode In objNodeList
                    ' strFirstName = objNode.Item("FIRSTNAME").InnerText
                    For Each objChildNode In objNode.ChildNodes


                        Select Case objChildNode.Name
                            Case "FIRSTNAME"
                                strFirstName = objChildNode.InnerText.Trim
                            Case "MIDDLENAME"
                                strMiddleName = objChildNode.InnerText.Trim
                            Case "LASTNAME"
                                strLastName = objChildNode.InnerText.Trim
                            Case "MAILINGADDRESS"
                                strMailingAddress = objChildNode.InnerText.Trim
                            Case "PASSWORD"
                                strPassword = objChildNode.InnerText.Trim
                            Case "REGISTRATIONDATE"
                                strRegDate = objChildNode.InnerText.Trim
                            Case "LANGUAGE"
                                strLanguage = objChildNode.InnerText.Trim
                            Case "PATTERNEDPASSWORD"
                                strPatternedPassword = objChildNode.InnerText.Trim
                            Case "CSRRESETPASSWORD"
                                strCSRREsetPasword = objChildNode.InnerText.Trim
                            Case "AUTHORIZEDREP"
                                For Each objInnerChildNode In objChildNode
                                    Select Case objInnerChildNode.Name
                                        Case "RELATIONSHIP"
                                            strAuthorizedRepRelationship = objInnerChildNode.InnerText.Trim
                                        Case "NAME"
                                            strAuthorizedRepName = objInnerChildNode.InnerText.Trim
                                    End Select
                                Next objInnerChildNode 'inner child node
                        End Select

                    Next objChildNode

                    'concat mailname
                    strMailName = strFirstName
                    If strMiddleName.Trim.Length <> 0 Then
                        strMailName = strMailName & " " & strMiddleName
                    End If
                    strMailName = strMailName & " " & strLastName

                    strAddressParts = strMailingAddress.Split(",")

                    Dim intAddressMax As Integer = strAddressParts.Length - 1

                    Dim intIndex As Integer

                    For intIndex = 0 To intAddressMax

                        If intIndex = intAddressMax Then

                            strCityStateZip = strAddressParts(intIndex)

                        Else

                            Select Case intIndex
                                Case 0
                                    strAdd1 = strAddressParts(intIndex)
                                Case 1
                                    strAdd2 = strAddressParts(intIndex)
                                Case 2
                                    strAdd3 = strAddressParts(intIndex)
                                Case 3
                                    strAdd4 = strAddressParts(intIndex)
                                Case Else

                                    If strAdd5.Trim.Length > 0 Then
                                        strAdd5 = strAdd5 & " " & strAddressParts(intIndex)
                                    Else
                                        strAdd5 = strAddressParts(intIndex)

                                    End If


                            End Select

                        End If


                    Next 'intindex


                    strSubType = FindMBPLetterSubType(strCSRREsetPasword, strAuthorizedRepName, strPatternedPassword)


                    Select Case strLanguage.Trim.ToUpper
                        Case "ENG", "ENGLISH"
                            strLanguage = "ENG"
                        Case Else
                            strLanguage = "SPAN"
                    End Select


                    'get master letter type id from dbs
                    intMasterLetterType = GetLetterTypeID(objMainConn, "MBP", strSubType, strLanguage, intPageCount, dblWeight, dblThickness)



                    MBPInsertIntoDT(dtMBP, "", strMailName, strAdd1, strAdd2, strAdd3, strAdd4, strAdd5, strCityStateZip, strPassword, strRegDate, _
                                          strPatternedPassword, strCSRREsetPasword, strAuthorizedRepRelationship, strAuthorizedRepName, intMasterLetterType, intImportFileID, intBatch, strRun, intPageCount, dblWeight, dblThickness, strCustomerID)


                    blnImport = True
                    'CLEANUP

                    strFirstName = ""
                    strMiddleName = ""
                    strLastName = ""
                    strMailingAddress = ""
                    strPassword = ""
                    strRegDate = ""
                    strLanguage = ""
                    strPatternedPassword = ""
                    strCSRREsetPasword = ""
                    strAuthorizedRepRelationship = ""
                    strAuthorizedRepName = ""
                    strMailName = ""
                    strCityStateZip = ""
                    strAdd1 = ""
                    strAdd2 = ""
                    strAdd3 = ""
                    strAdd4 = ""
                    strAdd5 = ""


                Next


            Next 'next file

            'case of zero lenght file
            If blnImport = True Then
                MBPBulkInsert(dtMBP, objMainConn)
            End If

            dtMBP.Dispose()
        Catch ex As Exception
            Throw New System.Exception("Error parsing MBP files", ex)
        End Try

    End Sub
    Public Sub ParseWCFiles(ByRef objMainConn As SqlConnection, ByVal strFolder As String, intFileTypeID As Integer, intBatch As Integer, strRun As String, strWCLiveProductionFolder As String)
        Try
            Dim strFiles As String()
            Dim strCurrentaXLSFile As String
            Dim objCSVDataset As New DataSet
            Dim strSelectFrom As String = "Select * From [Sheet1$]"
            Dim objConn As OleDb.OleDbConnection
            Dim objFileReader As OleDb.OleDbDataReader
            Dim objCommand As OleDb.OleDbCommand
            Dim objFile As FileInfo
            'Dim strImportId As String
            Dim strLetterFileName As String
            Dim intRecipientCount As Integer
            Dim strHICN As String
            Dim strName As String
            Dim strAddressLine1 As String
            Dim strAddressLine2 As String
            Dim strAddressLine3 As String
            Dim strAddressLine4 As String = ""
            Dim strAddressLine5 As String = ""
            Dim strCity As String
            Dim strState As String
            Dim strZipcode As String
            Dim strCityStateZip As String
            Dim strLOB As String
            Dim strContract As String
            Dim strInternal As String
            Dim strDocumentID As String
            Dim strDocumentCreatedDate As String
            Dim strActivityID As String
            Dim strMainframeCorrespondenceID As String
            Dim strCorrespondenceCaseID As String
            Dim strCMSForeignAddressIndicator As String
            Dim intRecordID As Integer = 0
            Dim intMasterLetterTypeid As Integer = 10029
            Dim dtWC As DataTable
            Dim intPageCount As Integer
            Dim dblWeight As Double
            Dim dblThickness As Double
            Dim intLastUnderScore As String
            Dim strLetterTrackingID As String
            Dim intPODWCZipFileID As Integer
            Dim intImportFileID As Integer
            Dim strCurrentPDF As String
            'Dim objCurrentPDF As FileInfo
            Dim strWCFolder As String
            Dim blnImport As Boolean = False

            'Dim myCommand As OleDbDataAdapter
            'Dim dtSet As System.Data.DataSet
            'Dim row As DataRow

            WriteToLogfile("Creating CreateWCDataTable")
            'Create datatable for inserting records
            dtWC = CreateWCDataTable()

            'Connect to folder


            'get a list of CSV files
            strFiles = Directory.GetFiles(strFolder, "*.xlsx")




            'loop through csv files
            For Each strCurrentaXLSFile In strFiles

                'objConn = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" & _
                '                           "Data Source=" & strCurrentaXLSFile & ";" & _
                '                            "Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1""")

                ''  objConn = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; Data Source='" & strCurrentaXLSFile & "'; Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1'")
                'myCommand = New System.Data.OleDb.OleDbDataAdapter _
                '    ("select * from [Sheet1$]", objConn)
                'myCommand.TableMappings.Add("Table", "Table")
                'dtSet = New System.Data.DataSet

                'myCommand.Fill(dtSet)

                'MessageBox.Show("Calling ace")
                WriteToLogfile("Calling Ace")
                objConn = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" & _
                                           "Data Source=" & strCurrentaXLSFile & ";" & _
                                            "Extended Properties=""Excel 12.0 Xml;HDR=YES;IMEX=1""")



                objFile = New FileInfo(strCurrentaXLSFile)

                objCommand = New OleDb.OleDbCommand
                objCommand.CommandText = strSelectFrom
                objCommand.CommandType = CommandType.Text
                objCommand.Connection = objConn

                ''open connection
                WriteToLogfile("objConn.Open()")
                'MessageBox.Show("objConn.Open()")
                objConn.Open()
                objFileReader = objCommand.ExecuteReader
                'MessageBox.Show("objConn.Open() done")
                intImportFileID = InsertImportFile(objMainConn, objFile.Name, intBatch, intFileTypeID)

                Do While objFileReader.Read
                    'get record information
                    intPODWCZipFileID = objFileReader.Item("ZipFileID").ToString.Trim
                    strLetterFileName = objFileReader.Item("Filename").ToString.Trim
                    strCurrentPDF = strFolder & "\" & intPODWCZipFileID.ToString & "-" & strLetterFileName & ".PDF"
                    'check to see if file exits
                    If File.Exists(strCurrentPDF) Then

                        'increase record count by one
                        intRecordID = intRecordID + 1

                        intLastUnderScore = strLetterFileName.LastIndexOf("_")

                        If intLastUnderScore <> 0 Then
                            strLetterTrackingID = strLetterFileName.Substring(intLastUnderScore + 1)
                        Else
                            strLetterTrackingID = ""
                        End If

                        'get record information
                        With objFileReader
                            intRecipientCount = .Item("Recipient Count")
                            strHICN = .Item("HICN").ToString
                            strName = .Item("Name").ToString
                            strAddressLine1 = .Item("Address Line 1").ToString
                            strAddressLine2 = .Item("Address Line 2").ToString
                            strAddressLine3 = .Item("Address Line 3").ToString
                            strCity = .Item("City").ToString
                            strState = .Item("State").ToString
                            strZipcode = .Item("Zipcode").ToString
                            strLOB = .Item("LOB").ToString
                            strContract = .Item("Contract").ToString
                            strInternal = .Item("Internal?").ToString
                            strDocumentID = .Item("Document ID").ToString
                            strDocumentCreatedDate = .Item("Document Created Date").ToString
                            strActivityID = .Item("Activity ID").ToString
                            strMainframeCorrespondenceID = .Item("Mainframe Correspondence ID").ToString
                            strCorrespondenceCaseID = .Item("Correspondence Case ID").ToString
                            strCMSForeignAddressIndicator = .Item("CMS Foreign Address Indicator").ToString
                            intPageCount = .Item("Page Count")
                            strWCFolder = .Item("WCFolder").ToString

                            If intPageCount > 10 Then
                                strRun = "02"
                            Else
                                strRun = "01"
                            End If

                        End With


                        'concat CityStateZip
                        strCityStateZip = strCity & ", " & strState & " " & strZipcode



                        If intPageCount > 0 Then

                            intMasterLetterTypeid = 10029

                        Else

                            'mark record as bad missing pdf
                            intMasterLetterTypeid = 10030

                        End If

                        WCInsertIntoDT(dtWC, strHICN, strName, strAddressLine1, strAddressLine2, strAddressLine3, strAddressLine4, strAddressLine5, strCityStateZip, strLetterFileName, intRecipientCount, _
                                       strLOB, strContract, strInternal, strDocumentID, strDocumentCreatedDate, strActivityID, strMainframeCorrespondenceID, strCorrespondenceCaseID, _
                                       strCMSForeignAddressIndicator, intMasterLetterTypeid, intImportFileID, intBatch, strRun, intPageCount, dblWeight, dblThickness, strLetterTrackingID, intPODWCZipFileID, strWCFolder)


                        blnImport = True
                        'need to make a copy of this PDF for live Production
                        'objCurrentPDF = New FileInfo(strCurrentPDF)
                        'objCurrentPDF.CopyTo(strWCLiveProductionFolder & objCurrentPDF.Name)

                    End If


                Loop

                objFileReader.Close()
                objCommand.Dispose()
                objConn.Close()
                objConn.Dispose()



            Next


            'case of zero lenght file
            If blnImport = True Then
                'bulk insert
                WriteToLogfile("WCBulkInsert")
                WCBulkInsert(dtWC, objMainConn)
            End If

            dtWC.Dispose()
        Catch ex As Exception
            WriteToLogfile("Error parsing WC Files: ex " & ex.Message)
            Throw New System.Exception("Error parsing WC Files", ex)
        End Try
    End Sub
    Private Function FindACOLetterType(strACO_LEGAL_NAME As String, strDATA_SHARING As String, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR As String) As String
        Dim strType As String = ""

        If strACO_LEGAL_NAME.Trim.Length > 0 Then


            If strACO_LEGAL_NAME.Trim.ToUpper = "** DE-ALIGNED **" Then

                If strDATA_SHARING.Trim.ToUpper = "N" Then
                    If strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR.Trim.Length = 0 Then
                        strType = "OUTU "
                    End If
                Else
                    strType = "UNKNOWN"
                End If

            ElseIf strDATA_SHARING.Trim.Length > 0 Then
                'ina,inrsa, outa



                If strDATA_SHARING.Trim.ToUpper = "Y" Then

                    If strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR.Trim.Length = 0 Then
                        'ina,
                        strType = "INA"
                    Else
                        'unknown
                        strType = "UNKNOWN"
                    End If

                Else ' N

                    If strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR.Trim.Length = 0 Then

                        'outa
                        strType = "OUTA"
                    Else
                        'unkown
                        strType = "UNKNOWN"
                    End If

                End If
            Else

                If strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR.Trim.ToUpper = "N" Then

                    'inrsa
                    strType = "INRSA"
                Else
                    'unknown
                    strType = "UNKNOWN"
                End If
            End If

        Else

            'inu, outu
            If strDATA_SHARING.Trim.ToUpper = "Y" Then
                If strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR.Trim.Length = 0 Then
                    'inu
                    strType = "INU"
                Else
                    'unkown
                    strType = "UNKNOWN"
                End If
            Else 'N
                If strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR.Trim.Length = 0 Then
                    'outu
                    strType = "OUTU"
                Else
                    'unknown
                    strType = "UNKNOWN"
                End If

            End If


        End If
        'This is to ensure that the program doesn't crash and also to mark the record as BAD
        If strType = "" Then strType = "UNKNOWN"
        Return strType

    End Function
    Private Function FindMBPLetterSubType(strCSRRestPassword As String, strAuthorizedRepName As String, strPatternedPassword As String) As String
        Dim strSubType As String = ""

        Select Case strCSRRestPassword
            Case "DEACTIVATE"
                strSubType = "DA"
            Case "REACTIVATE"

                If strPatternedPassword.Trim.Length > 0 Then
                    strSubType = "RAEP"
                Else
                    strSubType = "RAWE"
                End If
            Case "Y"
                If strPatternedPassword.Trim.Length > 0 Then
                    strSubType = "PREP"
                Else
                    strSubType = "PRWE"
                End If
            Case Else

                If strAuthorizedRepName.Trim.ToUpper = "MYMEDICARE ADMIN" Then
                    strSubType = "NE"
                ElseIf strAuthorizedRepName.Trim.ToUpper = "1-800-CSR" Then
                    strSubType = "AE"
                Else
                    strSubType = "RS"
                End If


        End Select

        If strSubType = "" Then strSubType = "UNKNOWN"
        Return strSubType

    End Function

    Private Function ReturnKeyValue(ByVal strRecord As String, ByRef strKey As String, ByRef strValue As String) As Boolean
        Dim intEqualSign As Integer = InStr(strRecord, "=")



        If intEqualSign = 0 Then
            strKey = strRecord.ToUpper.Trim
            If strKey = "RECORD_ENDS" Or strKey = "RECORD_STARTS" Then
                strValue = -1
                ReturnKeyValue = True

            Else
                strKey = "ERROR"
                ReturnKeyValue = False
            End If

        Else

            strKey = strRecord.Substring(0, intEqualSign - 1).ToUpper
            strValue = strRecord.Substring(intEqualSign)
            ReturnKeyValue = True

        End If

    End Function
End Module
