Module DataTableInserts

    Public Sub ACOInsertIntoDT(ByRef dtACO As DataTable, strHICN As String, strMAILNAME As String, strOrigAdd1 As String, strOrigAdd2 As String, strOrigAdd3 As String, strOrigAdd4 As String, strOrigAdd5 As String, _
                                          strOrigCityStateZip As String, strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE As String, strACO_LEGAL_NAME As String, _
                                       strDATA_SHARING As String, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE As String, strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR As String, _
                                         strRowID As String, intPODMasterLetterTypeId As Integer, intPODImportFilesID As Integer, intBatch As Integer, strRun As String, intPageCount As Integer, dblWeight As Double, dblThickness As Double)

        Try
            Dim drRow As DataRow

            drRow = dtACO.NewRow

            With drRow

                .Item("PODImportFilesID") = intPODImportFilesID
                .Item("PODMasterLetterTypeId") = intPODMasterLetterTypeId
                .Item("HICN") = strHICN
                .Item("MAILNAME") = strMAILNAME
                .Item("OrigAdd1") = strOrigAdd1
                .Item("OrigAdd2") = strOrigAdd2
                .Item("OrigAdd3") = strOrigAdd3
                .Item("OrigAdd4") = strOrigAdd4
                .Item("OrigAdd5") = strOrigAdd5
                .Item("OrigCityStateZip") = strOrigCityStateZip
                .Item("PODBatchID") = intBatch
                .Item("PODRunID") = strRun
                .Item("EffectiveDate") = strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_EFFECTIVE_DATE
                .Item("LegalName") = strACO_LEGAL_NAME
                .Item("DataSharing") = strDATA_SHARING
                .Item("SharingEffectiveDate") = strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE
                .Item("SharingPreferenceIndicator") = strBENEFICIARY_SUBSTANCE_ABUSE_DATA_SHARING_PREFERENCE_INDICATOR
                .Item("RowID") = strRowID
                .Item("PageCount") = intPageCount
                .Item("Weight") = dblWeight
                .Item("Thickness") = dblThickness
            End With

            dtACO.Rows.Add(drRow)

        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from DataTableInserts.ACOInsertIntoDT" & ex.Message)
        End Try

    End Sub
    Public Sub DISInsertIntoDT(ByRef dtDIS As DataTable, strHICN As String, strMAILNAME As String, strOrigAdd1 As String, strOrigAdd2 As String, strOrigAdd3 As String, strOrigAdd4 As String, strOrigAdd5 As String, _
                                       strOrigCityStateZip As String, strCurrentDate As String, strCallStartTime As String, strActivityID As String, strPlanName As String, _
                                        strTeminationDate As String, strCreateBy As String, strContractorDefine1 As String, strContractorDefine2 As String, _
                                        intPODMasterLetterTypeId As Integer, intPODImportFilesID As Integer, intBatch As Integer, strRun As String, intPageCount As Integer, dblWeight As Double, dblThickness As Double)


        Try
            Dim drRow As DataRow

            drRow = dtDIS.NewRow

            With drRow

                .Item("PODImportFilesID") = intPODImportFilesID
                .Item("PODMasterLetterTypeId") = intPODMasterLetterTypeId
                .Item("HICN") = strHICN
                .Item("MAILNAME") = strMAILNAME
                .Item("OrigAdd1") = strOrigAdd1
                .Item("OrigAdd2") = strOrigAdd2
                .Item("OrigAdd3") = strOrigAdd3
                .Item("OrigAdd4") = strOrigAdd4
                .Item("OrigAdd5") = strOrigAdd5
                .Item("OrigCityStateZip") = strOrigCityStateZip
                .Item("PODBatchID") = intBatch
                .Item("PODRunID") = strRun
                .Item("CurrentDate") = strCurrentDate
                .Item("CallStartTime") = strCallStartTime
                .Item("ActivityID") = strActivityID
                .Item("PlanName") = strPlanName
                .Item("TeminationDate") = strTeminationDate
                .Item("CreateBy") = strCreateBy
                .Item("ContractorDefine1") = strContractorDefine1
                .Item("ContractorDefine1") = strContractorDefine2
                .Item("PageCount") = intPageCount
                .Item("Weight") = dblWeight
                .Item("Thickness") = dblThickness

            End With

            dtDIS.Rows.Add(drRow)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from DataTableInserts.DISInsertIntoDT" & ex.Message)
        End Try
    End Sub

    Public Sub CPCInsertIntoDT(ByRef dtCPC As DataTable, strHICN As String, strMAILNAME As String, strOrigAdd1 As String, strOrigAdd2 As String, strOrigAdd3 As String, strOrigAdd4 As String, strOrigAdd5 As String, _
                                          strOrigCityStateZip As String, strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE As String, strPracticeName As String, _
                                       strDATA_SHARING As String, strRowID As String, intPODMasterLetterTypeId As Integer, intPODImportFilesID As Integer, intBatch As Integer, strRun As String, intPageCount As Integer, dblWeight As Double, dblThickness As Double)

        Try

            Dim drRow As DataRow

            drRow = dtCPC.NewRow

            With drRow

                .Item("PODImportFilesID") = intPODImportFilesID
                .Item("PODMasterLetterTypeId") = intPODMasterLetterTypeId
                .Item("HICN") = strHICN
                .Item("MAILNAME") = strMAILNAME
                .Item("OrigAdd1") = strOrigAdd1
                .Item("OrigAdd2") = strOrigAdd2
                .Item("OrigAdd3") = strOrigAdd3
                .Item("OrigAdd4") = strOrigAdd4
                .Item("OrigAdd5") = strOrigAdd5
                .Item("OrigCityStateZip") = strOrigCityStateZip
                .Item("PODBatchID") = intBatch
                .Item("PODRunID") = strRun
                .Item("SharingEffectiveDate") = strBENEFICIARY_DATA_SHARING_EFFECTIVE_DATE
                .Item("PracticeName") = strPracticeName
                .Item("DataSharing") = strDATA_SHARING
                .Item("RowID") = strRowID
                .Item("PageCount") = intPageCount
                .Item("Weight") = dblWeight
                .Item("Thickness") = dblThickness

            End With

            dtCPC.Rows.Add(drRow)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from DataTableInserts.CPCInsertIntoDT" & ex.Message)
        End Try
    End Sub

    Public Sub ENTInsertIntoDT(ByRef dtENT As DataTable, strHICN As String, strMAILNAME As String, strOrigAdd1 As String, strOrigAdd2 As String, strOrigAdd3 As String, strOrigAdd4 As String, strOrigAdd5 As String, _
                                        strOrigCityStateZip As String, strPartAEntitlementStartDate As String, strPartBEntitlementStartDate As String, _
                                      strCurrentDate As String, strSystemLetter As String, strRowID As String, intPODMasterLetterTypeId As Integer, intPODImportFilesID As Integer, intBatch As Integer, strRun As String, intPageCount As Integer, dblWeight As Double, dblThickness As Double)


        Try
            Dim drRow As DataRow

            drRow = dtENT.NewRow

            With drRow

                .Item("PODImportFilesID") = intPODImportFilesID
                .Item("PODMasterLetterTypeId") = intPODMasterLetterTypeId
                .Item("HICN") = strHICN
                .Item("MAILNAME") = strMAILNAME
                .Item("OrigAdd1") = strOrigAdd1
                .Item("OrigAdd2") = strOrigAdd2
                .Item("OrigAdd3") = strOrigAdd3
                .Item("OrigAdd4") = strOrigAdd4
                .Item("OrigAdd5") = strOrigAdd5
                .Item("OrigCityStateZip") = strOrigCityStateZip
                .Item("PODBatchID") = intBatch
                .Item("PODRunID") = strRun
                .Item("PartAStartDate") = strPartAEntitlementStartDate
                .Item("PartBStartDate") = strPartBEntitlementStartDate
                .Item("RowID") = strRowID
                .Item("CurrentDate") = strCurrentDate
                .Item("SystemLetter") = strSystemLetter
                .Item("PageCount") = intPageCount
                .Item("Weight") = dblWeight
                .Item("Thickness") = dblThickness

            End With

            dtENT.Rows.Add(drRow)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from DataTableInserts.ENTInsertIntoDT" & ex.Message)
        End Try
    End Sub
    Public Sub MBPInsertIntoDT(ByRef dtMBP As DataTable, strHICN As String, strMAILNAME As String, strOrigAdd1 As String, strOrigAdd2 As String, strOrigAdd3 As String, strOrigAdd4 As String, strOrigAdd5 As String, _
                                       strOrigCityStateZip As String, strPassword As String, strRegDate As String, strPatternedPassword As String, strCSRREsetPasword As String, _
                                       strAuthorizedRepRelationship As String, strAuthorizedRepName As String, intPODMasterLetterTypeId As Integer, intPODImportFilesID As Integer, intBatch As Integer, strRun As String, intPageCount As Integer, dblWeight As Double, dblThickness As Double, ByVal strCustomerID As String)


        Try

            Dim drRow As DataRow

            drRow = dtMBP.NewRow

            With drRow

                .Item("PODImportFilesID") = intPODImportFilesID
                .Item("PODMasterLetterTypeId") = intPODMasterLetterTypeId
                .Item("HICN") = strHICN
                .Item("MAILNAME") = strMAILNAME
                .Item("OrigAdd1") = strOrigAdd1
                .Item("OrigAdd2") = strOrigAdd2
                .Item("OrigAdd3") = strOrigAdd3
                .Item("OrigAdd4") = strOrigAdd4
                .Item("OrigAdd5") = strOrigAdd5
                .Item("OrigCityStateZip") = strOrigCityStateZip
                .Item("PODBatchID") = intBatch
                .Item("PODRunID") = strRun
                .Item("PassWord") = strPassword
                .Item("RegistrationDate") = strRegDate
                .Item("PatternedPassword") = strPatternedPassword
                .Item("CSRRestPassword") = strCSRREsetPasword
                .Item("AuthorizeRepRelationShip") = strAuthorizedRepRelationship
                .Item("AuthorizeRepName") = strAuthorizedRepName
                .Item("PageCount") = intPageCount
                .Item("Weight") = dblWeight
                .Item("Thickness") = dblThickness
                .Item("CustomerID") = strCustomerID
            End With

            dtMBP.Rows.Add(drRow)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from DataTableInserts.MBPInsertIntoDT" & ex.Message)
        End Try
    End Sub
    Public Sub WCInsertIntoDT(ByRef dtWC As DataTable, strHICN As String, strMAILNAME As String, strOrigAdd1 As String, strOrigAdd2 As String, strOrigAdd3 As String, strOrigAdd4 As String, strOrigAdd5 As String, _
                                       strOrigCityStateZip As String, strLetterFileName As String, intRecipientCount As Integer, strLOB As String, strContract As String, strInternal As String, strDocumentID As String, strDocumentCreatedDate As String, strActivityID As String, _
                                           strMainframeCorrespondenceID As String, strCorrespondenceCaseID As String, strCMSForeignAddressIndicator As String, intPODMasterLetterTypeId As Integer, intPODImportFilesID As Integer, intBatch As Integer, strRun As String, intPageCount As Integer, dblWeight As Double, dblThickness As Double, ByVal strLetterTrackingID As String, ByVal intPODWCZipFileID As Integer, ByVal strWCFolder As String)
        Try
            Dim drRow As DataRow

            drRow = dtWC.NewRow

            With drRow
                .Item("PODImportFilesID") = intPODImportFilesID
                .Item("PODMasterLetterTypeId") = intPODMasterLetterTypeId
                .Item("HICN") = strHICN
                .Item("MAILNAME") = strMAILNAME
                .Item("OrigAdd1") = strOrigAdd1
                .Item("OrigAdd2") = strOrigAdd2
                .Item("OrigAdd3") = strOrigAdd3
                .Item("OrigAdd4") = strOrigAdd4
                .Item("OrigAdd5") = strOrigAdd5
                .Item("OrigCityStateZip") = strOrigCityStateZip
                .Item("PODBatchID") = intBatch
                .Item("PODRunID") = strRun
                .Item("LETTER_FILENAME") = strLetterFileName.Trim
                .Item("RecipientCount") = intRecipientCount
                .Item("LOB") = strLOB.Trim
                .Item("Contract") = strContract.Trim
                .Item("Internal") = strInternal.Trim
                .Item("DocumentID") = strDocumentID.Trim
                .Item("DocumentCreatedDate") = strDocumentCreatedDate.Trim
                .Item("ActivityID") = strActivityID.Trim
                .Item("MainframeCorrespondenceID") = strMainframeCorrespondenceID.Trim
                .Item("CorrespondenceCaseID") = strCorrespondenceCaseID.Trim
                .Item("CMSForeignAddressIndicator") = strCMSForeignAddressIndicator.Trim
                .Item("PageCount") = intPageCount
                .Item("Weight") = dblWeight
                .Item("Thickness") = dblThickness
                .Item("LetterTrackingID") = strLetterTrackingID
                .Item("PODWCZipFileID") = intPODWCZipFileID
                .Item("WCFolder") = strWCFolder

            End With

            dtWC.Rows.Add(drRow)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from DataTableInserts.WCInsertIntoDT" & ex.Message)
        End Try
    End Sub


End Module
