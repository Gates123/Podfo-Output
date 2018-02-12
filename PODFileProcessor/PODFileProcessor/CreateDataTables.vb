Imports System.Data.SqlClient
Imports System.Globalization
Module CreateDataTables

    Public Function CreateACODataTable() As DataTable
        Try
            Dim dsACO As New DataSet
            dsACO.Locale = CultureInfo.InvariantCulture
            Dim dtACO As DataTable

            dtACO = dsACO.Tables.Add("ACOLETTER")

            With dtACO.Columns
                .Add("PODImportFilesID", Type.GetType("System.Decimal"))
                .Add("PODMasterLetterTypeId", Type.GetType("System.Decimal"))
                .Add("HICN", Type.GetType("System.String"))
                .Add("MAILNAME", Type.GetType("System.String"))
                .Add("OrigAdd1", Type.GetType("System.String"))
                .Add("OrigAdd2", Type.GetType("System.String"))
                .Add("OrigAdd3", Type.GetType("System.String"))
                .Add("OrigAdd4", Type.GetType("System.String"))
                .Add("OrigAdd5", Type.GetType("System.String"))
                .Add("OrigCityStateZip", Type.GetType("System.String"))
                .Add("PODBatchID", Type.GetType("System.Decimal"))
                .Add("PODRunID", Type.GetType("System.String"))
                .Add("EffectiveDate", Type.GetType("System.String"))
                .Add("LegalName", Type.GetType("System.String"))
                .Add("DataSharing", Type.GetType("System.String"))
                .Add("SharingEffectiveDate", Type.GetType("System.String"))
                .Add("SharingPreferenceIndicator", Type.GetType("System.String"))
                .Add("RowID", Type.GetType("System.String"))
                .Add("PageCount", Type.GetType("System.Decimal"))
                .Add("Weight", Type.GetType("System.Decimal"))
                .Add("Thickness", Type.GetType("System.Decimal"))
            End With

            Return dtACO
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from CreateDataTables.CreateACODataTable" & ex.Message)
        End Try
    End Function
    Public Function CreateDISDataTable() As DataTable
        Try
            Dim dsDIS As New DataSet
            dsDIS.Locale = CultureInfo.InvariantCulture
            Dim dtDIS As DataTable

            dtDIS = dsDIS.Tables.Add("DISLETTER")

            With dtDIS.Columns

                .Add("PODImportFilesID", Type.GetType("System.Decimal"))
                .Add("PODMasterLetterTypeId", Type.GetType("System.Decimal"))
                .Add("HICN", Type.GetType("System.String"))
                .Add("MAILNAME", Type.GetType("System.String"))
                .Add("OrigAdd1", Type.GetType("System.String"))
                .Add("OrigAdd2", Type.GetType("System.String"))
                .Add("OrigAdd3", Type.GetType("System.String"))
                .Add("OrigAdd4", Type.GetType("System.String"))
                .Add("OrigAdd5", Type.GetType("System.String"))
                .Add("OrigCityStateZip", Type.GetType("System.String"))
                .Add("PODBatchID", Type.GetType("System.Decimal"))
                .Add("PODRunID", Type.GetType("System.String"))
                .Add("CurrentDate", Type.GetType("System.String"))
                .Add("CallStartTime", Type.GetType("System.String"))
                .Add("ActivityID", Type.GetType("System.String"))
                .Add("PlanName", Type.GetType("System.String"))
                .Add("TeminationDate", Type.GetType("System.String"))
                .Add("CreateBy", Type.GetType("System.String"))
                .Add("ContractorDefine1", Type.GetType("System.String"))
                .Add("ContractorDefine2", Type.GetType("System.String"))
                .Add("PageCount", Type.GetType("System.Decimal"))
                .Add("Weight", Type.GetType("System.Decimal"))
                .Add("Thickness", Type.GetType("System.Decimal"))
            End With

            Return dtDIS
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from CreateDataTables.CreateDISDataTable" & ex.Message)
        End Try
    End Function
    Public Function CreateCPCDataTable() As DataTable
        Try
            Dim dsCPC As New DataSet
            dsCPC.Locale = CultureInfo.InvariantCulture
            Dim dtCPC As DataTable

            dtCPC = dsCPC.Tables.Add("CPCLETTER")

            With dtCPC.Columns

                .Add("PODImportFilesID", Type.GetType("System.Decimal"))
                .Add("PODMasterLetterTypeId", Type.GetType("System.Decimal"))
                .Add("HICN", Type.GetType("System.String"))
                .Add("MAILNAME", Type.GetType("System.String"))
                .Add("OrigAdd1", Type.GetType("System.String"))
                .Add("OrigAdd2", Type.GetType("System.String"))
                .Add("OrigAdd3", Type.GetType("System.String"))
                .Add("OrigAdd4", Type.GetType("System.String"))
                .Add("OrigAdd5", Type.GetType("System.String"))
                .Add("OrigCityStateZip", Type.GetType("System.String"))
                .Add("PODBatchID", Type.GetType("System.Decimal"))
                .Add("PODRunID", Type.GetType("System.String"))
                .Add("SharingEffectiveDate", Type.GetType("System.String"))
                .Add("PracticeName", Type.GetType("System.String"))
                .Add("DataSharing", Type.GetType("System.String"))
                .Add("RowID", Type.GetType("System.String"))
                .Add("PageCount", Type.GetType("System.Decimal"))
                .Add("Weight", Type.GetType("System.Decimal"))
                .Add("Thickness", Type.GetType("System.Decimal"))
            End With

            Return dtCPC
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from CreateDataTables.CreateCPCDataTable" & ex.Message)
        End Try
    End Function
    Public Function CreateENTDataTable() As DataTable
        Try
            Dim dsENT As New DataSet
            dsENT.Locale = CultureInfo.InvariantCulture
            Dim dtENT As DataTable

            dtENT = dsENT.Tables.Add("ENTLETTER")

            With dtENT.Columns

                .Add("PODImportFilesID", Type.GetType("System.Decimal"))
                .Add("PODMasterLetterTypeId", Type.GetType("System.Decimal"))
                .Add("HICN", Type.GetType("System.String"))
                .Add("MAILNAME", Type.GetType("System.String"))
                .Add("OrigAdd1", Type.GetType("System.String"))
                .Add("OrigAdd2", Type.GetType("System.String"))
                .Add("OrigAdd3", Type.GetType("System.String"))
                .Add("OrigAdd4", Type.GetType("System.String"))
                .Add("OrigAdd5", Type.GetType("System.String"))
                .Add("OrigCityStateZip", Type.GetType("System.String"))
                .Add("PODBatchID", Type.GetType("System.Decimal"))
                .Add("PODRunID", Type.GetType("System.String"))
                .Add("PartAStartDate", Type.GetType("System.String"))
                .Add("PartBStartDate", Type.GetType("System.String"))
                .Add("RowID", Type.GetType("System.String"))
                .Add("CurrentDate", Type.GetType("System.String"))
                .Add("SystemLetter", Type.GetType("System.String"))
                .Add("PageCount", Type.GetType("System.Decimal"))
                .Add("Weight", Type.GetType("System.Decimal"))
                .Add("Thickness", Type.GetType("System.Decimal"))
            End With

            Return dtENT
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from CreateDataTables.CreateENTDataTable" & ex.Message)
        End Try
    End Function
    Public Function CreateMBPDataTable() As DataTable
        Try
            Dim dsMBP As New DataSet
            dsMBP.Locale = CultureInfo.InvariantCulture
            Dim dtMBP As DataTable

            dtMBP = dsMBP.Tables.Add("MBPLETTER")

            With dtMBP.Columns

                .Add("PODImportFilesID", Type.GetType("System.Decimal"))
                .Add("PODMasterLetterTypeId", Type.GetType("System.Decimal"))
                .Add("HICN", Type.GetType("System.String"))
                .Add("MAILNAME", Type.GetType("System.String"))
                .Add("OrigAdd1", Type.GetType("System.String"))
                .Add("OrigAdd2", Type.GetType("System.String"))
                .Add("OrigAdd3", Type.GetType("System.String"))
                .Add("OrigAdd4", Type.GetType("System.String"))
                .Add("OrigAdd5", Type.GetType("System.String"))
                .Add("OrigCityStateZip", Type.GetType("System.String"))
                .Add("PODBatchID", Type.GetType("System.Decimal"))
                .Add("PODRunID", Type.GetType("System.String"))
                .Add("PassWord", Type.GetType("System.String"))
                .Add("RegistrationDate", Type.GetType("System.String"))
                .Add("PatternedPassword", Type.GetType("System.String"))
                .Add("CSRRestPassword", Type.GetType("System.String"))
                .Add("AuthorizeRepRelationShip", Type.GetType("System.String"))
                .Add("AuthorizeRepName", Type.GetType("System.String"))
                .Add("PageCount", Type.GetType("System.Decimal"))
                .Add("Weight", Type.GetType("System.Decimal"))
                .Add("Thickness", Type.GetType("System.Decimal"))
                .Add("CustomerID", Type.GetType("System.String"))
            End With

            Return dtMBP
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from CreateDataTables.CreateMBPDataTable" & ex.Message)
        End Try
    End Function
    Public Function CreateWCDataTable() As DataTable
        Try
            Dim dsWC As New DataSet
            dsWC.Locale = CultureInfo.InvariantCulture
            Dim dtWc As DataTable

            dtWc = dsWC.Tables.Add("WCLETTER")

            With dtWc.Columns

                .Add("PODImportFilesID", Type.GetType("System.Decimal"))
                .Add("PODMasterLetterTypeId", Type.GetType("System.Decimal"))
                .Add("HICN", Type.GetType("System.String"))
                .Add("MAILNAME", Type.GetType("System.String"))
                .Add("OrigAdd1", Type.GetType("System.String"))
                .Add("OrigAdd2", Type.GetType("System.String"))
                .Add("OrigAdd3", Type.GetType("System.String"))
                .Add("OrigAdd4", Type.GetType("System.String"))
                .Add("OrigAdd5", Type.GetType("System.String"))
                .Add("OrigCityStateZip", Type.GetType("System.String"))
                .Add("PODBatchID", Type.GetType("System.Decimal"))
                .Add("PODRunID", Type.GetType("System.String"))
                .Add("LETTER_FILENAME", Type.GetType("System.String"))
                .Add("RecipientCount", Type.GetType("System.Decimal"))
                .Add("LOB", Type.GetType("System.String"))
                .Add("Contract", Type.GetType("System.String"))
                .Add("Internal", Type.GetType("System.String"))
                .Add("DocumentID", Type.GetType("System.String"))
                .Add("DocumentCreatedDate", Type.GetType("System.String"))
                .Add("ActivityID", Type.GetType("System.String"))
                .Add("MainframeCorrespondenceID", Type.GetType("System.String"))
                .Add("CorrespondenceCaseID", Type.GetType("System.String"))
                .Add("CMSForeignAddressIndicator", Type.GetType("System.String"))
                .Add("PageCount", Type.GetType("System.Decimal"))
                .Add("Weight", Type.GetType("System.Decimal"))
                .Add("Thickness", Type.GetType("System.Decimal"))
                .Add("LetterTrackingID", Type.GetType("System.String"))
                .Add("PODWCZipFileID", Type.GetType("System.Decimal"))
                .Add("WCFolder", Type.GetType("System.String"))
            End With

            Return dtWc
        Catch ex As Exception
            ReportsApplication1.clsEmail.EmailMessage("Error in PODFO", "Error from CreateDataTables.CreateWCDataTable" & ex.Message)
        End Try
    End Function
End Module
