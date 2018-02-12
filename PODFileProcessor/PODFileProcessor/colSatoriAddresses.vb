Imports System.IO
Imports Utilities

Public Class colSatoriAddresses
    Implements System.Collections.IEnumerable
    Private mCol As Collection
    Private mdtMailDate As DateTime = Now
    Private mblnShowSatoriScreen As Boolean = True
    Private mstrSatoriTemplateName As String = ""
    Private mstrJobNameToDisplay As String = ""
    Private mstrSortFileNameAndPath As String = ""
    Private mstrMailDatDirectory As String = ""
    Private mstrSatoriServerIP As String = ""
    Private mblnUseMailDat As Boolean = False
    Private mblnMixedWeight As Boolean = False
    Private mblnForceWalkSequence As Boolean = False
    Private mblnHasCassed As Boolean = False
    Private mblnHasSorted As Boolean = False
    Private mobjCopyOfCollection As colSatoriAddresses
    Private mcolBadAddress As colSatoriAddresses


    ' Get connection string
    Private Conn As String = Nothing
    ' Create DB Logger
    Private Logger As Logging = Nothing
    ' Create Config table access
    Private Conf As ConfigTable = Nothing
    ' Use test DB
    Private UseTestDB As Boolean = False

    Private mstrMailDir As String = "\\Cobmain\usacms\PODFO\Output\Mail-Dat\"

    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()

        Conn = DbAccess.GetConnectionString()
        UseTestDB = DbAccess.UseTestDB
        Logger = New Logging(Conn, "AppLog")
        Conf = New ConfigTable(Conn)

        mstrMailDir = GetParm("strMailDatFolder", "\\Cobmain\usacms\PODFO\Output\Mail-Dat\")

    End Sub

    ''' <summary>
    ''' Use satori on server instead of local machine 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SatoriServerIP As String
        Get
            Return mstrSatoriServerIP
        End Get
        Set(value As String)
            mstrSatoriServerIP = value
        End Set
    End Property

    ''' <summary>
    ''' Directory to save MAIL DAT files to. Not required if part of satori template. 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MailDatDirectory As String
        Get
            Return mstrMailDatDirectory
        End Get
        Set(value As String)
            mstrMailDatDirectory = value
        End Set
    End Property

  
    ''' <summary>
    ''' If set to true you are will be required to send walk sequence number that was provided when list was brought.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Property ForceWalkSequence As Boolean
        Get
            Return mblnForceWalkSequence
        End Get
        Set(value As Boolean)
            mblnForceWalkSequence = value
        End Set
    End Property
    ''' <summary>
    ''' If set to true you must supply wieght and thickness for each piece.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MixedWeightMailing As Boolean
        Get
            Return mblnMixedWeight
        End Get
        Set(value As Boolean)
            mblnMixedWeight = value
        End Set
    End Property
    ''' <summary>
    ''' Name and path to save Post Office paper work to.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SortFileNameAndPath As String
        Get
            Return mstrSortFileNameAndPath
        End Get
        Set(value As String)
            mstrSortFileNameAndPath = value
        End Set
    End Property
    ''' <summary>
    ''' Job name that will appear int the statement sequence number box of postage statement
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobNameToDisplay As String
        Get
            Return mstrJobNameToDisplay
        End Get
        Set(value As String)
            mstrJobNameToDisplay = value
        End Set
    End Property
    ''' <summary>
    ''' Satori template supplied must reside on machine doing sorting in order to sort correctly.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SatoriTemplateName As String
        Get
            Return mstrSatoriTemplateName
        End Get
        Set(value As String)
            mstrSatoriTemplateName = value
        End Set
    End Property
    ''' <summary>
    ''' If not supplied today's date will be used.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MailDate As DateTime
        Get
            Return mdtMailDate
        End Get
        Set(value As DateTime)
            mdtMailDate = value
        End Set
    End Property
    ''' <summary>
    ''' Sets the visibility of Satori Screens
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowSatoriScreen As Boolean
        Get
            Return mblnShowSatoriScreen
        End Get
        Set(value As Boolean)
            mblnShowSatoriScreen = value
        End Set
    End Property
    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        GetEnumerator = mCol.GetEnumerator
    End Function
    Public Sub Remove(ByVal vntIndexKey As Object)
        mCol.Remove(vntIndexKey)
    End Sub
    Private Sub Class_Initialize_Renamed()
        mCol = New Collection

    End Sub
    Private Sub Class_Terminate_Renamed()
        mCol = Nothing
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub
    Default Public ReadOnly Property Item(ByVal vntIndexKey As Object) As clsSatoriAddress
        Get
            Item = mCol.Item(vntIndexKey)
        End Get
    End Property
    Public ReadOnly Property Count() As Integer
        Get
            Return mCol.Count
        End Get
    End Property
    Public Function Add(ByVal mintRecordID As Integer) As clsSatoriAddress
        Dim objNewLetter As New clsSatoriAddress

        mCol.Add(objNewLetter, mintRecordID.ToString)
        objNewLetter.RecordID = mintRecordID
        objNewLetter.mstrSKey = mintRecordID.ToString


        Add = objNewLetter
        objNewLetter = Nothing
    End Function
    Public Function Add(ByVal objSatoriAddress As clsSatoriAddress) As clsSatoriAddress

        mCol.Add(objSatoriAddress, objSatoriAddress.Skey)
        Add = objSatoriAddress

    End Function

    Private Sub CassAddresses()


        Dim objMRTKCASSTask As MRTKTASKLib.CASSTask = New MRTKTASKLib.CASSTask
        Dim strInput As String
        Dim strOutput As String
        Try



            'format for input of original address string
            strInput = "FLD_RECORD_ID" & vbTab & _
                               "FLD_ADDRESSLINE1" & vbTab & _
                               "FLD_ADDRESSLINE2" & vbTab & _
                               "FLD_LASTLINE"

            'format for output of satori cassed address
            strOutput = "FLD_RECORD_ID" & vbTab & _
                            "FLD_ADDRESSLINE1" & vbTab & _
                            "FLD_ADDRESSLINE2" & vbTab & _
                            "FLD_CITY" & vbTab & _
                            "FLD_STATE" & vbTab & _
                            "FLD_ZIPCODE" & vbTab & _
                            "FLD_LASTLINE" & vbTab & _
                            "FLD_DPC" & vbTab & _
                            "FLD_ERRORCODE" & vbTab & _
                            "FLD_CARRIER_ROUTE" & vbTab & _
                            "FLD_DP_BARCODE" 

            'set the satori cass task properties

            If mstrSatoriServerIP.Trim.Length = 0 Then
                objMRTKCASSTask.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkMAILROOM_SERVER_LIST, "")
            Else

                objMRTKCASSTask.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkMAILROOM_SERVER_LIST, mstrSatoriServerIP)
            End If
            objMRTKCASSTask.PrepareTask()

            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctALL_CAPS, True)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctSILENT_MODE, mblnShowSatoriScreen)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctSHOW_PROGRESS, mblnShowSatoriScreen)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctHIDE_PROGRESS_AFTER_BATCH, True)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctCERTIFY_FLAG, 0)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctREVIEW_ERRORS, True)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctUPDATE_UNCORRECTED_CITYSTZIP, True)

            objMRTKCASSTask.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkINPUT_BLOCK_RECORD_COUNT, 100)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctRECORD_COUNT, Me.Count)

            objMRTKCASSTask.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkFIELD_LIST_IN, strInput)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkFIELD_LIST_OUT, strOutput)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkDELIMITER_FIELD, vbTab)
            objMRTKCASSTask.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkDELIMITER_RECORD, vbLf)

            ' Ensure that all the properties above are correct
            objMRTKCASSTask.ValidateProperties()


            Dim blnCassed As Boolean = False
            Dim intRecordCount As Integer
            Dim strAddressCass As String = ""

            'loop through address and cass them
            Dim objCurrentAddress As clsSatoriAddress

            For Each objCurrentAddress In Me

                blnCassed = False

                intRecordCount = intRecordCount + 1

                'build the string for original address
                strAddressCass = strAddressCass & BuildAddressForCass(objCurrentAddress) & vbLf


                If intRecordCount Mod 100 = 0 Then

                    'cass addresses
                    'Send address data, receive response
                    objMRTKCASSTask.Update(strAddressCass)


                    'update collection
                    UpdateObjectsWithCassInfo(strAddressCass)

                    'reset address string
                    strAddressCass = ""
                    blnCassed = True
                End If



            Next

            If blnCassed = False Then
                'Send address data, receive response
                objMRTKCASSTask.Update(strAddressCass)


                'update collection
                UpdateObjectsWithCassInfo(strAddressCass)
            End If


            objMRTKCASSTask.EndTask()

            mblnHasCassed = True
        Catch ex As Exception
            Throw New ApplicationException("Cassing Addresses Error: " & ex.Message)
        End Try

    End Sub
    Private Sub UpdateObjectsWithCassInfo(ByVal strResults As String)
        'return Satori cass string format
        'strOutput = "FLD_RECORD_ID" & vbTab & _
        '                     "FLD_ADDRESSLINE1" & vbTab & _
        '                     "FLD_ADDRESSLINE2" & vbTab & _
        '                     "FLD_CITY" & vbTab & _
        '                     "FLD_STATE" & vbTab & _
        '                     "FLD_ZIPCODE" & vbTab & _
        '                     "FLD_LASTLINE" & vbTab & _
        '                     "FLD_DPC" & vbTab & _
        '                     "FLD_ERRORCODE"
        '                     "FLD_CARRIER_ROUTE" & vbTab & _
        '                      "FLD_DP_BARCODE" 


        Dim strRecords As String() = strResults.Split(vbLf)
        Dim strCurrentRecord As String
        Dim strColumns As String()
        Dim objCurrentRecord As clsSatoriAddress


        For Each strCurrentRecord In strRecords

            strColumns = strCurrentRecord.Split(vbTab)

            'make sure not empty
            If strColumns(0).Trim.Length <> 0 Then
                objCurrentRecord = Me.Item(strColumns(0))

                With objCurrentRecord
                    .mstrMailAddress1 = strColumns(1).Trim.ToUpper
                    .mstrMailAddress2 = strColumns(2).Trim.ToUpper
                    .mstrMailCity = strColumns(3).Trim.ToUpper
                    .mstrMailState = strColumns(4).Trim.ToUpper
                    .mstrMailZip = strColumns(5).Trim.ToUpper
                    .mstrMailLastLine = strColumns(6).Trim.ToUpper
                    .MailDeliveryPointCode = strColumns(7).Trim.ToUpper
                    .mstrMailErrorCode = strColumns(8).Trim.ToUpper
                    .MailCarrierRoute = strColumns(9).Trim.ToUpper
                    .MailDeliveryPointBarcode = strColumns(10).Trim
                End With
            End If



        Next

    End Sub
    Private Function BuildAddressForCass(objAddress As clsSatoriAddress)
        'address string needs to be in this format
        'strInput = "FLD_RECORD_ID" & vbTab & _
        '                      "FLD_ADDRESSLINE1" & vbTab & _
        '                      "FLD_ADDRESSLINE2" & vbTab & _
        '                      "FLD_LASTLINE"
        Dim strAddressString As String

        With objAddress

            strAddressString = .RecordID & vbTab
            strAddressString = strAddressString & .OriginalAddress1.Trim & vbTab

            If .OriginalAddress2.Trim.Length = 0 Then
                strAddressString = strAddressString & " " & vbTab
            Else
                strAddressString = strAddressString & .OriginalAddress2.Trim & vbTab
            End If

            strAddressString = strAddressString & .OriginalLastLine.Trim

        End With

        Return strAddressString
    End Function
    Private Sub PrepareNonCassedAddresses()
        Dim objClsCurrent As clsSatoriAddress
        Dim intMissingCodes As Integer


        For Each objClsCurrent In Me

            If objClsCurrent.MailDeliveryPointCode.Trim.Length = 0 Then
                intMissingCodes = intMissingCodes + 1

            End If

            If mblnForceWalkSequence = True Then
                If objClsCurrent.MailWalkSequence.Trim.Length = 0 Then
                    Throw New ApplicationException("Must have walk sequence if Force Walk Sequence is True.")
                End If
            End If
            With objClsCurrent

                .mstrMailAddress1 = .OriginalAddress1
                .mstrMailAddress2 = .OriginalAddress2
                .mstrMailCity = .OriginalCity
                .mstrMailState = .OriginalState
                .mstrMailZip = .OriginalZip
                .mstrMailLastLine = .OriginalLastLine

            End With

        Next

        If intMissingCodes = Me.Count Then
            Throw New ApplicationException("Delivery Point Code is Required For Sorting")
        End If
    End Sub
    Private Sub SortAddresses(ByVal intBatchID As Integer, ByVal strRun As String)
        Dim _FieldDelimiter As String = vbTab
        Dim _RecordDelimiter As String = vbLf
        Dim _RecordBlockCount As Integer = 100
        Dim RT_REPORT_SEQUENCEID As Integer = 653
        Dim strSatoriInput As String
        Dim strSatoriOutput As String
        Dim objSatori As MRTKTASKLib.PresortTask = New MRTKTASKLib.PresortTask
        Dim strMailDate As String = mdtMailDate.ToString("MM/dd/yyyy")
        If (strRun.Contains("0")) Then
        Else
            strRun = "0" & strRun
        End If

        mstrMailDatDirectory = mstrMailDir & intBatchID & strRun



        Try


            'INPUT ADDRESS STRUCTURE FOR SATORI
            strSatoriInput = "FLD_RECORD_ID" & vbTab & _
                         "FLD_ADDRESSLINE1" & vbTab & _
                         "FLD_ADDRESSLINE2" & vbTab & _
                         "FLD_CITY" & vbTab & _
                         "FLD_STATE" & vbTab & _
                         "FLD_ZIPCODE" & vbTab & _
                          "FLD_DPC" & vbTab & _
                                "FLD_PIECE_WEIGHT" & vbTab & _
                                "FLD_PIECE_THICKNESS"




            'RETURNED OUTPUT STRUCTURE FROM SATORI
            strSatoriOutput = "FLD_RECORD_ID" & vbTab & _
                         "FLD_PACKAGE_NUMBER" & vbTab & _
                         "FLD_TRAY_NUMBER" & vbTab & _
                         "FLD_ENDORSEMENT_LINE" & vbTab & _
                         "FLD_PRESORT_ID" & vbTab & _
                          "FLD_IM_BARCODE" & vbTab & _
                            "FLD_IM_BARCODE_NUMERIC" & vbTab & "FLD_KEYLINE"



            If mstrSatoriServerIP.Trim.Length = 0 Then
                objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkMAILROOM_SERVER_LIST, "")
            Else

                objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkMAILROOM_SERVER_LIST, mstrSatoriServerIP)
            End If

            'Set the server and prepare the task
            objSatori.PrepareTask()



            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkTEMPLATE_NAME_TO_USE, mstrSatoriTemplateName)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptSHOW_SORT_PROGRESS, True)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptCASS_CERTIFY_FIRST, 0)
            objSatori.SetProperty(MRTKTASKLib.CASSTaskPropertyID.ctCERTIFY_FLAG, 0)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptSHOW_RECEIVE_PROGRESS, 0)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptSHOW_PAGE_SETUP, 0)
            objSatori.SetProperty(MRTKTASKLib.PRESORTWizardPropertyID.pwPRESORT_MIXED_WEIGHT_SORT, True)

            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptHIDE_SORT_PROGRESS_AFTER_SORT, True)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptFORM_NAME, "PODFO" & intBatchID.ToString.Trim & strRun)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptRECORD_COUNT, Me.Count)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptSORT_RESULTS_FLAG, 2)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptRECORD_COUNT_PER_RECEIVE, _RecordBlockCount)



            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkINPUT_BLOCK_RECORD_COUNT, _RecordBlockCount)
            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkDELIMITER_FIELD, _FieldDelimiter)
            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkDELIMITER_RECORD, _RecordDelimiter)
            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkFIELD_LIST_IN, strSatoriInput)
            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkFIELD_LIST_OUT, strSatoriOutput)

            objSatori.SetProperty(MRTKTASKLib.REPORTWizardPropertyID.rwPRINT_PRESORT_MAILER_ID, _
                                                        "PODFO" & intBatchID.ToString.Trim & strRun)
            objSatori.SetProperty(MRTKTASKLib.REPORTWizardPropertyID.rwMAILING_DATE, strMailDate)

            objSatori.SetProperty(RT_REPORT_SEQUENCEID, "PODFO" & intBatchID.ToString.Trim & strRun) 'This will set the statement sequence with the batch and the run number


            'set up mail dat file
            If Directory.Exists(mstrMailDatDirectory) = False Then

                Directory.CreateDirectory(mstrMailDatDirectory)

            End If

            ' ''set mail dat save to true
            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkPRINT_ON_SERVER, True)
            'set folder name to batch run mail dat folder
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptREPORT_FOLDER_NAME_MAILDAT, mstrMailDatDirectory)
            'rwPRINT_PRESORT_MAILER_ID
            objSatori.SetProperty(MRTKTASKLib.REPORTWizardPropertyID.rwPRINT_PRESORT_MAILER_ID, "PODFO" & intBatchID.ToString.Trim & strRun)


            objSatori.SetProperty(MRTKTASKLib.MRTKTaskPropertyID.mrtkPRINT_ON_SERVER, False)
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptUSE_MAILDAT, True)



            Trace.WriteLine("        SET FOLDER TO " & mstrMailDatDirectory.ToUpper)

            'set mail dat report name to this batch and run 
            objSatori.SetProperty(MRTKTASKLib.PRESORTTaskPropertyID.ptMAILDAT_MAILING_TITLE, "PODFO" & intBatchID.ToString.Trim & strRun)


            'objMRTKPRESORTTask.SetProperty(RT_REPORT_SEQUENCEID, "69" & Format(RCRun, "00")) 'This will set the statement sequence with the batch and the run number

            ' Ensure that all the properties above are correct
            objSatori.ValidateProperties()


            'loop through address and cass them
            Dim objCurrentAddress As clsSatoriAddress
            Dim intRecordCount As Integer
            Dim intBlock As Integer
            Dim strAddressSort As String = ""
            Dim blnSentToSatori As Boolean
            Dim intCounter As Integer
            Dim intErrorCode As Integer

            For Each objCurrentAddress In Me


                If objCurrentAddress.MailErrorCode.Trim.Length > 0 And objCurrentAddress.MailErrorCode.Trim <> "0" Then

                    intErrorCode = CInt(objCurrentAddress.MailErrorCode)

                    If intErrorCode = -412 And objCurrentAddress.LetterType.Trim = "DIS" And objCurrentAddress.HasNumberInAddress = True Then
                        intErrorCode = 0
                    End If

                    If objCurrentAddress.LetterType.Trim = "WC" And objCurrentAddress.mstrMailZip.Length > 4 Then
                        intErrorCode = 0
                    End If
                Else

                    intErrorCode = 0

                End If

                If intErrorCode < 100 Then

                    blnSentToSatori = False

                    intRecordCount = intRecordCount + 1

                    strAddressSort = strAddressSort & BuildAddressForSort(objCurrentAddress) & vbLf


                    If intRecordCount Mod 25 = 0 Then

                        'cass addresses
                        'Send address data, receive response
                        objSatori.Send(strAddressSort)
                        intBlock = intBlock + 1


                        'reset address string
                        strAddressSort = ""
                        blnSentToSatori = True
                    End If

                Else

                    'bad address undeliverable
                    objCurrentAddress.mstrMailPresortID = "-1"
                    objCurrentAddress.mstrMailTrayNumber = "-1"
                    objCurrentAddress.mstrMailPackageNumber = "-1"
                End If

            Next

            If blnSentToSatori = False Then
                objSatori.Send(strAddressSort)
                intBlock = intBlock + 1
            End If


            'sort the address
            Try

           
            objSatori.DoSort()
            objSatori.PrintReport(MRTKTASKLib.MRTKReportID.prREPORT_FILE_MAILDAT, String.Empty, 0)
            Catch ex As Exception
                Throw New ApplicationException(ex.Message & " Do Sort")
            End Try
            'get sorted data back from satori
            'loop through the number of satori loops
            For intCounter = 1 To intBlock

                'get data from satori
                objSatori.Retrieve(strAddressSort)

                'update sort info
                UpdateObjectsWithSortInfo(strAddressSort)

                strAddressSort = ""

            Next

            If Me.Count > 199 Then
                'save the satori pdfs
                objSatori.SaveReportsAsPDF(mstrSortFileNameAndPath, 0)
            End If


            'finish up task
            objSatori.EndTask()

            mblnHasSorted = True

        Catch ex As Exception
            Throw New ApplicationException(ex.Message & " Sort Addresses")
        End Try
    End Sub
    Private Function BuildAddressForSort(objAddress As clsSatoriAddress)
        'satori input string format for sorting
        'INPUT ADDRESS STRUCTURE FOR SATORI
        'strSatoriInput = "FLD_RECORD_ID" & vbTab & _
        '             "FLD_ADDRESSLINE1" & vbTab & _
        '             "FLD_ADDRESSLINE2" & vbTab & _
        '             "FLD_CITY" & vbTab & _
        '             "FLD_STATE" & vbTab & _
        '             "FLD_ZIPCODE" & vbTab & _
        '              "FLD_DPC"



        'If mblnForceWalkSequence = True Then
        '    strSatoriInput = strSatoriInput & vbTab & _
        '                "FLD_CARRIER_ROUTE" & vbTab & _
        '                "FLD_WALK_SEQUENCE_NUMBER"

        'End If

        'If mblnMixedWeight = True Then

        '    strSatoriInput = strSatoriInput & vbTab & _
        '                    "FLD_PIECE_WEIGHT" & vbTab & _
        '                    "FLD_PIECE_THICKNESS"
        'End If



        Dim strResult As String
        With objAddress

            strResult = .RecordID.ToString & vbTab
            strResult = strResult & .MailAddress1.Trim & vbTab
            strResult = strResult & .MailAddress2.Trim & vbTab
            strResult = strResult & .MailCity.Trim & vbTab
            strResult = strResult & .MailState.Trim & vbTab
            strResult = strResult & .MailZip.Trim & vbTab
            strResult = strResult & .MailDeliveryPointCode & vbTab
            strResult = strResult & .PieceWeight & vbTab
            strResult = strResult & .PieceThickness



        End With

        Return strResult

    End Function
    Private Sub UpdateObjectsWithSortInfo(ByVal strResults As String)
        'return Satori cass string format
        'strSatoriOutput = "FLD_RECORD_ID" & vbTab & _
        '             "FLD_PACKAGE_NUMBER" & vbTab & _
        '             "FLD_TRAY_NUMBER" & vbTab & _
        '             "FLD_ENDORSEMENT_LINE" & vbTab & _
        '             "FLD_PRESORT_ID" & vbTab & _
        '              "FLD_IM_BARCODE"



        Dim strRecords As String() = strResults.Split(vbLf)
        Dim strCurrentRecord As String
        Dim strColumns As String()
        Dim objCurrentRecord As clsSatoriAddress


        For Each strCurrentRecord In strRecords

            strColumns = strCurrentRecord.Split(vbTab)

            'make sure not empty
            If strColumns(0).Trim.Length <> 0 Then
                objCurrentRecord = Me.Item(strColumns(0))

                With objCurrentRecord

                    .mstrMailPackageNumber = strColumns(1).Trim.ToUpper
                    .mstrMailTrayNumber = strColumns(2).Trim.ToUpper
                    .mstrMailEndoresementLine = strColumns(3).Trim.ToUpper
                    If strColumns(4).Trim.ToUpper = "" Then
                        .mstrMailPresortID = "-1"
                        .mstrMailTrayNumber = "-1"
                    Else
                        .mstrMailPresortID = strColumns(4).Trim.ToUpper
                    End If

                    .mstrMailIntelligentMailBarCode = strColumns(5).Trim.ToUpper
                    .mstrMailIntelligentMailBarCodeNumber = strColumns(6).Trim.ToUpper
                    .mstrMailKeyline = strColumns(7).Trim.ToUpper

                End With
            End If



        Next

    End Sub


    Private Sub VerifyNonCassedRecords()
        Dim objClsCurrent As clsSatoriAddress
        Dim ZipCodeLength As Integer
        Dim intMissingCodes As Integer
        'loop through address and verify that they have zip4 and dpc because they are required for IMB
        For Each objClsCurrent In Me

            If objClsCurrent.MailDeliveryPointCode.Trim.Length = 0 Then

                intMissingCodes = intMissingCodes + 1

            End If

            If InStr(objClsCurrent.OriginalZip, "-") <> 0 Then
                ZipCodeLength = 10
            Else
                ZipCodeLength = 9
            End If

            'If objClsCurrent.OriginalZip.Length <> ZipCodeLength Then
            '    Throw New ApplicationException("Zip Code plus 4 is required for sort.")
            'End If

        Next

        If intMissingCodes = Me.Count Then
            Throw New ApplicationException("Mail delivery point code is required to sort address.")
        End If
    End Sub
    Private Sub VerifyForSorting()
        'verify setup options for sorting

        Dim objClsCurrent As clsSatoriAddress


        'a satori template must be named in order for sort to work
        If SatoriTemplateName.Trim.Length = 0 Then
            Throw New ApplicationException("Satori Template Name Required For Sorting.")
        End If

        'path must be supplied in order to save pdf
        If SortFileNameAndPath.Trim.Length = 0 Then
            Throw New ApplicationException("Sort File Name And Path required for saving sort postage PDF.")
        End If


        'if mixed weight option is selected user must supply the weight.
        If MixedWeightMailing = True Then

            For Each objClsCurrent In Me
                If objClsCurrent.PieceWeight.Trim.Length = 0 Then
                    Throw New ApplicationException("Weight is required for mixed wieght mailing sort.")
                End If
                If objClsCurrent.PieceThickness.Trim.Length = 0 Then
                    Throw New ApplicationException("Thickness is required for mixed weight mailing sort.")
                End If
            Next

        End If


        'if force walk sequence is required user must provide the walk sequence from the purchased list in order
        'for the walk sequence to work.
        If ForceWalkSequence = True Then
            For Each objClsCurrent In Me
                If objClsCurrent.MailWalkSequence.Trim.Length = 0 Then
                    Throw New ApplicationException("Walk sequence is required for force walk sequence to be applied.  This should have been supplied with mailing list.")
                End If
            Next
        End If



    End Sub
    ''' <summary>
    ''' Puts collection in presort order
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OrderAddressByMpresortID(Optional RemoveBadAddress As Boolean = True)
        Dim colNewAddress As New colSatoriAddresses
        Dim objCurrentAddress As clsSatoriAddress

        Dim strCurrentID As String
        Dim dict As New SortedList(Of Integer, Integer)
        Dim sortedDict As Dictionary(Of Integer, Integer)
        mcolBadAddress = New colSatoriAddresses

        If mblnHasSorted = False Then Exit Sub


        'copy original order
        CopyCollection()


        'populate dic with presortid for the key and the skey for the value
        For Each objCurrentAddress In Me

            If objCurrentAddress.MailPresortID = "-1" Or objCurrentAddress.MailPresortID = "" Then

                mcolBadAddress.Add(objCurrentAddress)

            Else

                strCurrentID = objCurrentAddress.MailPresortID
                dict.Add(CInt(objCurrentAddress.MailPresortID), CInt(objCurrentAddress.Skey))
            End If

        Next


        'sort the list
        sortedDict = (From entry In dict Order By entry.Key Ascending).ToDictionary(Function(pair) pair.Key, Function(pair) pair.Value)


        'loop through list and create a order collection list
        For Each kvp As KeyValuePair(Of Integer, Integer) In sortedDict


            objCurrentAddress = Me.Item(kvp.Value - 1)
            colNewAddress.Add(objCurrentAddress)


        Next kvp

        'Clear original collection
        mCol.Clear()

        'loop through ordered good address and add to collection
        For Each objCurrentAddress In colNewAddress

            Me.Add(objCurrentAddress)
        Next


        'add bad address back to output
        If RemoveBadAddress = False Then
            Dim mintRecordCount As Integer = Me.Count

            ''loop though bad address and add to end
            For Each objCurrentAddress In mcolBadAddress

                mintRecordCount = mintRecordCount + 1
                objCurrentAddress.mstrMailPresortID = mintRecordCount.ToString.Trim
                objCurrentAddress.mstrMailTrayNumber = "-1"
                objCurrentAddress.mstrMailEndoresementLine = "SNG PC"
                Me.Add(objCurrentAddress)
            Next
        End If
    End Sub
    ''' <summary>
    ''' Puts collection in order that it was entered
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub OrderAddressByKey()

        Dim objCopy As clsSatoriAddress

        'make sure there is something in the copy before trying to load it
        If mobjCopyOfCollection.Count > 0 Then

            'Clear current contents of collection
            mCol.Clear()

            'loop through copy to fill collection backup
            For Each objCopy In mobjCopyOfCollection
                Me.Add(objCopy)
            Next

        End If



    End Sub
    'Public Sub RemoveSatoriSortBadAddress()
    '    Dim objCopy As clsSatoriAddress
    '    Dim ObjWithBadAddress As New colSatoriAddresses

    '    'loop through current collection and to copy
    '    For Each objCopy In Me
    '        ObjWithBadAddress.Add(objCopy)
    '    Next

    '    mCol.Clear()

    '    For Each objCopy In ObjWithBadAddress
    '        If objCopy.mstrMailPresortID.Trim <> "-1" And objCopy.mstrMailPresortID.Trim.Length <> 0 Then
    '            Me.Add(objCopy)
    '        End If

    '    Next

    '    ObjWithBadAddress = Nothing

    'End Sub
    Private Sub CopyCollection()
        Dim objCopy As clsSatoriAddress

        'create new copy collection
        mobjCopyOfCollection = New colSatoriAddresses

        'loop through current collection and to copy
        For Each objCopy In Me
            mobjCopyOfCollection.Add(objCopy)
        Next

    End Sub

    ''' <summary>
    ''' Uses satori to cass address.  results appear in mail properties e.g. (.mailaddress1)
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SatoriCassOnly()
        'cass the address
        CassAddresses()
    End Sub
    ''' <summary>
    ''' Uses satori to sort addresses. SatoriTemplateName and SortFileNameAndPath are required.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SatoriSortOnly(ByVal intbatch As Integer, ByVal strrun As Integer)

        'if they have not used this to cass then fill
        If mblnHasCassed = False Then
            PrepareNonCassedAddresses()
        End If


        'before calling satori make sure records are okay to sort
        VerifyNonCassedRecords()
        VerifyForSorting()

        'run satori sort on addresses
        SortAddresses(intbatch, strrun)

    End Sub
    ''' <summary>
    ''' Uses satori to sort addresses. SatoriTemplateName and SortFileNameAndPath are required.  Mail address used by Satori can be found in properties that start with the word mail  
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SatoriCassAndSort(ByVal intbatch As Integer, ByVal strrun As Integer)

        'before call satori to do anything verify records are ok to sort
        VerifyForSorting()

        Try
            'Cass records before sorting

            CassAddresses()

            'sort the records
            SortAddresses(intbatch, strrun)

        Catch ex As Exception
            Throw New ApplicationException(ex.Message)
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

End Class
