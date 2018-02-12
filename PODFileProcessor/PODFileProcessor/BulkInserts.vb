Imports System.Data.SqlClient

Module BulkInserts

    Public Sub ACOBulkInsert(ByRef dtACO As DataTable, ByRef ojbMainConn As SqlConnection)
        Try

            Using bulkcopy As SqlBulkCopy = New SqlBulkCopy(ojbMainConn)
                bulkcopy.DestinationTableName = "ACOTempTable"
                With bulkcopy.ColumnMappings

                    .Add(New SqlBulkCopyColumnMapping("PODImportFilesID", "PODImportFilesID"))
                    .Add(New SqlBulkCopyColumnMapping("PODMasterLetterTypeId", "PODMasterLetterTypeId"))
                    .Add(New SqlBulkCopyColumnMapping("HICN", "HICN"))
                    .Add(New SqlBulkCopyColumnMapping("MAILNAME", "MAILNAME"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd1", "OrigAdd1"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd2", "OrigAdd2"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd3", "OrigAdd3"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd4", "OrigAdd4"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd5", "OrigAdd5"))
                    .Add(New SqlBulkCopyColumnMapping("OrigCityStateZip", "OrigCityStateZip"))
                    .Add(New SqlBulkCopyColumnMapping("PODBatchID", "PODBatchID"))
                    .Add(New SqlBulkCopyColumnMapping("PODRunID", "PODRunID"))
                    .Add(New SqlBulkCopyColumnMapping("EffectiveDate", "EffectiveDate"))
                    .Add(New SqlBulkCopyColumnMapping("LegalName", "LegalName"))
                    .Add(New SqlBulkCopyColumnMapping("DataSharing", "DataSharing"))
                    .Add(New SqlBulkCopyColumnMapping("SharingEffectiveDate", "SharingEffectiveDate"))
                    .Add(New SqlBulkCopyColumnMapping("SharingPreferenceIndicator", "SharingPreferenceIndicator"))
                    .Add(New SqlBulkCopyColumnMapping("RowID", "RowID"))
                    .Add(New SqlBulkCopyColumnMapping("PageCount", "PageCount"))
                    .Add(New SqlBulkCopyColumnMapping("Weight", "Weight"))
                    .Add(New SqlBulkCopyColumnMapping("Thickness", "Thickness"))

                End With


                bulkcopy.WriteToServer(dtACO)



            End Using

            ProcessTempTable("USP_Process_ACOTempTable", ojbMainConn)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from BulkInserts.ACOBulkInsert" & ex.Message)
        End Try
    End Sub

    Public Sub DISBulkInsert(ByRef dtDIS As DataTable, ByRef ojbMainConn As SqlConnection)
        Try

      
        Using bulkcopy As SqlBulkCopy = New SqlBulkCopy(ojbMainConn)
            bulkcopy.DestinationTableName = "DISTempTable"
            With bulkcopy.ColumnMappings

                .Add(New SqlBulkCopyColumnMapping("PODImportFilesID", "PODImportFilesID"))
                .Add(New SqlBulkCopyColumnMapping("PODMasterLetterTypeId", "PODMasterLetterTypeId"))
                .Add(New SqlBulkCopyColumnMapping("HICN", "HICN"))
                .Add(New SqlBulkCopyColumnMapping("MAILNAME", "MAILNAME"))
                .Add(New SqlBulkCopyColumnMapping("OrigAdd1", "OrigAdd1"))
                .Add(New SqlBulkCopyColumnMapping("OrigAdd2", "OrigAdd2"))
                .Add(New SqlBulkCopyColumnMapping("OrigAdd3", "OrigAdd3"))
                .Add(New SqlBulkCopyColumnMapping("OrigAdd4", "OrigAdd4"))
                .Add(New SqlBulkCopyColumnMapping("OrigAdd5", "OrigAdd5"))
                .Add(New SqlBulkCopyColumnMapping("OrigCityStateZip", "OrigCityStateZip"))
                .Add(New SqlBulkCopyColumnMapping("PODBatchID", "PODBatchID"))
                .Add(New SqlBulkCopyColumnMapping("PODRunID", "PODRunID"))
                .Add(New SqlBulkCopyColumnMapping("CurrentDate", "CurrentDate"))
                .Add(New SqlBulkCopyColumnMapping("CallStartTime", "CallStartTime"))
                .Add(New SqlBulkCopyColumnMapping("ActivityID", "ActivityID"))
                .Add(New SqlBulkCopyColumnMapping("PlanName", "PlanName"))
                .Add(New SqlBulkCopyColumnMapping("TeminationDate", "TeminationDate"))
                .Add(New SqlBulkCopyColumnMapping("CreateBy", "CreateBy"))
                .Add(New SqlBulkCopyColumnMapping("ContractorDefine1", "ContractorDefine1"))
                .Add(New SqlBulkCopyColumnMapping("ContractorDefine2", "ContractorDefine2"))
                .Add(New SqlBulkCopyColumnMapping("PageCount", "PageCount"))
                .Add(New SqlBulkCopyColumnMapping("Weight", "Weight"))
                .Add(New SqlBulkCopyColumnMapping("Thickness", "Thickness"))
            End With


            bulkcopy.WriteToServer(dtDIS)
        End Using

        ProcessTempTable("USP_Process_DISTempTable", ojbMainConn)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from BulkInserts.DISBulkInsert" & ex.Message)
        End Try
    End Sub
    Public Sub CPCBulkInsert(ByRef dtCPC As DataTable, ByRef ojbMainConn As SqlConnection)
        Try
            Using bulkcopy As SqlBulkCopy = New SqlBulkCopy(ojbMainConn)
                bulkcopy.DestinationTableName = "CPCTempTable"
                With bulkcopy.ColumnMappings

                    .Add(New SqlBulkCopyColumnMapping("PODImportFilesID", "PODImportFilesID"))
                    .Add(New SqlBulkCopyColumnMapping("PODMasterLetterTypeId", "PODMasterLetterTypeId"))
                    .Add(New SqlBulkCopyColumnMapping("HICN", "HICN"))
                    .Add(New SqlBulkCopyColumnMapping("MAILNAME", "MAILNAME"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd1", "OrigAdd1"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd2", "OrigAdd2"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd3", "OrigAdd3"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd4", "OrigAdd4"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd5", "OrigAdd5"))
                    .Add(New SqlBulkCopyColumnMapping("OrigCityStateZip", "OrigCityStateZip"))
                    .Add(New SqlBulkCopyColumnMapping("PODBatchID", "PODBatchID"))
                    .Add(New SqlBulkCopyColumnMapping("PODRunID", "PODRunID"))
                    .Add(New SqlBulkCopyColumnMapping("SharingEffectiveDate", "SharingEffectiveDate"))
                    .Add(New SqlBulkCopyColumnMapping("PracticeName", "PracticeName"))
                    .Add(New SqlBulkCopyColumnMapping("DataSharing", "DataSharing"))
                    .Add(New SqlBulkCopyColumnMapping("RowID", "RowID"))
                    .Add(New SqlBulkCopyColumnMapping("PageCount", "PageCount"))
                    .Add(New SqlBulkCopyColumnMapping("Weight", "Weight"))
                    .Add(New SqlBulkCopyColumnMapping("Thickness", "Thickness"))
                End With


                bulkcopy.WriteToServer(dtCPC)
            End Using

            ProcessTempTable("USP_Process_CPCTempTable", ojbMainConn)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from BulkInserts.CPCBulkInsert" & ex.Message)
        End Try
    End Sub
    Public Sub ENTBulkInsert(ByRef dtENT As DataTable, ByRef ojbMainConn As SqlConnection)
        Try
            Using bulkcopy As SqlBulkCopy = New SqlBulkCopy(ojbMainConn)
                bulkcopy.DestinationTableName = "ENTTempTable"
                With bulkcopy.ColumnMappings

                    .Add(New SqlBulkCopyColumnMapping("PODImportFilesID", "PODImportFilesID"))
                    .Add(New SqlBulkCopyColumnMapping("PODMasterLetterTypeId", "PODMasterLetterTypeId"))
                    .Add(New SqlBulkCopyColumnMapping("HICN", "HICN"))
                    .Add(New SqlBulkCopyColumnMapping("MAILNAME", "MAILNAME"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd1", "OrigAdd1"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd2", "OrigAdd2"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd3", "OrigAdd3"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd4", "OrigAdd4"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd5", "OrigAdd5"))
                    .Add(New SqlBulkCopyColumnMapping("OrigCityStateZip", "OrigCityStateZip"))
                    .Add(New SqlBulkCopyColumnMapping("PODBatchID", "PODBatchID"))
                    .Add(New SqlBulkCopyColumnMapping("PODRunID", "PODRunID"))
                    .Add(New SqlBulkCopyColumnMapping("PartAStartDate", "PartAStartDate"))
                    .Add(New SqlBulkCopyColumnMapping("PartBStartDate", "PartBStartDate"))
                    .Add(New SqlBulkCopyColumnMapping("RowID", "RowID"))
                    .Add(New SqlBulkCopyColumnMapping("CurrentDate", "CurrentDate"))
                    .Add(New SqlBulkCopyColumnMapping("SystemLetter", "SystemLetter"))
                    .Add(New SqlBulkCopyColumnMapping("PageCount", "PageCount"))
                    .Add(New SqlBulkCopyColumnMapping("Weight", "Weight"))
                    .Add(New SqlBulkCopyColumnMapping("Thickness", "Thickness"))
                End With


                bulkcopy.WriteToServer(dtENT)
            End Using

            ProcessTempTable("USP_Process_ENTTempTable", ojbMainConn)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from BulkInserts.ENTBulkInsert" & ex.Message)
        End Try
    End Sub
    Public Sub MBPBulkInsert(ByRef dtMBP As DataTable, ByRef ojbMainConn As SqlConnection)
        Try
            Using bulkcopy As SqlBulkCopy = New SqlBulkCopy(ojbMainConn)

                bulkcopy.DestinationTableName = "MBPTempTable"
                With bulkcopy.ColumnMappings

                    .Add(New SqlBulkCopyColumnMapping("PODImportFilesID", "PODImportFilesID"))
                    .Add(New SqlBulkCopyColumnMapping("PODMasterLetterTypeId", "PODMasterLetterTypeId"))
                    .Add(New SqlBulkCopyColumnMapping("HICN", "HICN"))
                    .Add(New SqlBulkCopyColumnMapping("MAILNAME", "MAILNAME"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd1", "OrigAdd1"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd2", "OrigAdd2"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd3", "OrigAdd3"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd4", "OrigAdd4"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd5", "OrigAdd5"))
                    .Add(New SqlBulkCopyColumnMapping("OrigCityStateZip", "OrigCityStateZip"))
                    .Add(New SqlBulkCopyColumnMapping("PODBatchID", "PODBatchID"))
                    .Add(New SqlBulkCopyColumnMapping("PODRunID", "PODRunID"))
                    .Add(New SqlBulkCopyColumnMapping("PassWord", "PassWord"))
                    .Add(New SqlBulkCopyColumnMapping("RegistrationDate", "RegistrationDate"))
                    .Add(New SqlBulkCopyColumnMapping("PatternedPassword", "PatternedPassword"))
                    .Add(New SqlBulkCopyColumnMapping("CSRRestPassword", "CSRRestPassword"))
                    .Add(New SqlBulkCopyColumnMapping("AuthorizeRepRelationShip", "AuthorizeRepRelationShip"))
                    .Add(New SqlBulkCopyColumnMapping("AuthorizeRepName", "AuthorizeRepName"))
                    .Add(New SqlBulkCopyColumnMapping("PageCount", "PageCount"))
                    .Add(New SqlBulkCopyColumnMapping("Weight", "Weight"))
                    .Add(New SqlBulkCopyColumnMapping("Thickness", "Thickness"))
                    .Add(New SqlBulkCopyColumnMapping("CustomerID", "CustomerID"))

                End With

                bulkcopy.BulkCopyTimeout = 0
                bulkcopy.WriteToServer(dtMBP)
            End Using

            ProcessTempTable("USP_Process_MBPTempTable", ojbMainConn)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from BulkInserts.MBPBulkInsert" & ex.Message)
        End Try
    End Sub
    Public Sub WCBulkInsert(dtWC As DataTable, ByRef ojbMainConn As SqlConnection)
        Try
            Using bulkcopy As SqlBulkCopy = New SqlBulkCopy(ojbMainConn)
                bulkcopy.DestinationTableName = "WCTempTable"
                With bulkcopy.ColumnMappings

                    .Add(New SqlBulkCopyColumnMapping("PODImportFilesID", "PODImportFilesID"))
                    .Add(New SqlBulkCopyColumnMapping("PODMasterLetterTypeId", "PODMasterLetterTypeId"))
                    .Add(New SqlBulkCopyColumnMapping("HICN", "HICN"))
                    .Add(New SqlBulkCopyColumnMapping("MAILNAME", "MAILNAME"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd1", "OrigAdd1"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd2", "OrigAdd2"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd3", "OrigAdd3"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd4", "OrigAdd4"))
                    .Add(New SqlBulkCopyColumnMapping("OrigAdd5", "OrigAdd5"))
                    .Add(New SqlBulkCopyColumnMapping("OrigCityStateZip", "OrigCityStateZip"))
                    .Add(New SqlBulkCopyColumnMapping("PODBatchID", "PODBatchID"))
                    .Add(New SqlBulkCopyColumnMapping("PODRunID", "PODRunID"))
                    .Add(New SqlBulkCopyColumnMapping("LETTER_FILENAME", "LETTER_FILENAME"))
                    .Add(New SqlBulkCopyColumnMapping("RecipientCount", "RecipientCount"))
                    .Add(New SqlBulkCopyColumnMapping("LOB", "LOB"))
                    .Add(New SqlBulkCopyColumnMapping("Contract", "Contract"))
                    .Add(New SqlBulkCopyColumnMapping("Internal", "Internal"))
                    .Add(New SqlBulkCopyColumnMapping("DocumentID", "DocumentID"))
                    .Add(New SqlBulkCopyColumnMapping("DocumentCreatedDate", "DocumentCreatedDate"))
                    .Add(New SqlBulkCopyColumnMapping("ActivityID", "ActivityID"))
                    .Add(New SqlBulkCopyColumnMapping("MainframeCorrespondenceID", "MainframeCorrespondenceID"))
                    .Add(New SqlBulkCopyColumnMapping("CorrespondenceCaseID", "CorrespondenceCaseID"))
                    .Add(New SqlBulkCopyColumnMapping("CMSForeignAddressIndicator", "CMSForeignAddressIndicator"))
                    .Add(New SqlBulkCopyColumnMapping("PageCount", "PageCount"))
                    .Add(New SqlBulkCopyColumnMapping("Weight", "Weight"))
                    .Add(New SqlBulkCopyColumnMapping("Thickness", "Thickness"))
                    .Add(New SqlBulkCopyColumnMapping("LetterTrackingID", "LetterTrackingID"))
                    .Add(New SqlBulkCopyColumnMapping("PODWCZipFileID", "PODWCZipFileID"))
                    .Add(New SqlBulkCopyColumnMapping("WCFolder", "WCFolder"))


                End With


                bulkcopy.WriteToServer(dtWC)



            End Using

            ProcessTempTable("USP_Process_WCTempTable", ojbMainConn)
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from BulkInserts.WCBulkInsert" & ex.Message)
        End Try
    End Sub
    Public Sub ProcessTempTable(strProcess As String, ByRef ojbMainConn As SqlConnection)
        Try
            Dim cmdSQL As New SqlCommand

            With cmdSQL
                .CommandText = strProcess
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 0
                .Connection = ojbMainConn
                .ExecuteNonQuery()
                .Dispose()
            End With
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from BulkInserts.ProcessTempTable" & ex.Message)
        End Try
    End Sub
End Module
