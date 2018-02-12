Public Class clsSatoriAddress
    Private mintRecordID As Integer
    Private mstrOriginalAddress1 As String = " "
    Private mstrOriginalAddress2 As String = " "
    Private mstrOriginalLastLine As String = " "
    Private mstrOriginalCity As String = " "
    Private mstrOriginalState As String = " "
    Private mstrOriginalZip As String = " "
    Private mstrExtraAddress As String = ""
    Private mblnHasNumberInAddress As String = False
    Private mstrLetterType As String
    Friend mstrMailName As String = ""
    Friend mstrMailAddress1 As String = ""
    Friend mstrMailAddress2 As String = ""
    Friend mstrMailCity As String = ""
    Friend mstrMailState As String = ""
    Friend mstrMailZip As String = ""
    Friend mstrMailLastLine As String = ""
    Private mstrMailDeliveryPointCode As String = " "
    Private mstrMailDeliveryPointBarcode As String = " "
    Friend mstrMailErrorCode As String = ""
    Private mstrMailCarrierRoute As String = ""
    Private mstrMailWalkSequence As String = ""
    Friend mstrMailPackageNumber As String = ""
    Friend mstrMailTrayNumber As String = ""
    Friend mstrMailEndoresementLine As String = ""
    Friend mstrMailPresortID As String = ""
    Friend mstrMailIntelligentMailBarCode As String = ""
    Friend mstrMailIntelligentMailBarCodeNumber As String = ""
    Private mstrPieceWeight As String = ""
    Private mstrPieceThickness As String = ""
    Friend mstrMailKeyline As String = ""
    Friend mstrSKey As String = ""
    Friend mintAddedOrder As Integer

    Public ReadOnly Property Skey As String
        Get
            Skey = mstrSKey
        End Get
        
    End Property
    Public Property RecordID As Integer

        Get
            Return mintRecordID
        End Get

        Set(ByVal value As Integer)
            mintRecordID = value
        End Set
    End Property
    Public Property MailName As String

        Get
            Return mstrMailName
        End Get

        Set(ByVal value As String)
            mstrMailName = value
        End Set
    End Property
    Public Property OriginalAddress1 As String

        Get
            Return mstrOriginalAddress1
        End Get

        Set(ByVal value As String)
            mstrOriginalAddress1 = value
        End Set
    End Property

    Public Property OriginalAddress2 As String

        Get
            Return mstrOriginalAddress2
        End Get

        Set(ByVal value As String)
            mstrOriginalAddress2 = value
        End Set
    End Property

    Public Property OriginalLastLine As String

        Get

            If mstrOriginalLastLine.Trim.Length > 0 Then
                Return mstrOriginalLastLine
            Else
                mstrOriginalLastLine = OriginalCity.Trim & ", " & OriginalState.Trim & " " & OriginalZip
                Return mstrOriginalLastLine
            End If
        End Get

        Set(ByVal value As String)
            mstrOriginalLastLine = value
        End Set
    End Property

    Public Property OriginalCity As String

        Get
            Return mstrOriginalCity
        End Get

        Set(ByVal value As String)
            mstrOriginalCity = value
        End Set
    End Property

    Public Property OriginalState As String

        Get
            Return mstrOriginalState
        End Get

        Set(ByVal value As String)
            mstrOriginalState = value
        End Set
    End Property

    Public Property OriginalZip As String

        Get
            Return mstrOriginalZip
        End Get

        Set(ByVal value As String)
            mstrOriginalZip = value
        End Set
    End Property
    Public Property HasNumberInAddress As Boolean
        Get
            Return mblnHasNumberInAddress
        End Get
        Set(value As Boolean)
            mblnHasNumberInAddress = value
        End Set
    End Property
    Public Property ExtraAddress As String
        Get
            Return mstrExtraAddress
        End Get
        Set(value As String)
            mstrExtraAddress = value
        End Set
    End Property
    Public Property PieceWeight As String
        Get
            Return mstrPieceWeight
        End Get
        Set(value As String)
            mstrPieceWeight = value
        End Set
    End Property
    Public Property PieceThickness As String
        Get
            Return mstrPieceThickness
        End Get
        Set(value As String)
            mstrPieceThickness = value
        End Set
    End Property
    Public ReadOnly Property MailAddress1 As String

        Get
            Return mstrMailAddress1
        End Get

    End Property

    Public ReadOnly Property MailAddress2 As String

        Get
            Return mstrMailAddress2
        End Get

    End Property

    Public ReadOnly Property MailCity As String

        Get
            Return mstrMailCity
        End Get

    End Property

    Public ReadOnly Property MailState As String

        Get
            Return mstrMailState
        End Get

    End Property

    Public ReadOnly Property MailZip As String

        Get
            Return mstrMailZip
        End Get

    End Property
    Public ReadOnly Property MailCityStateZip As String
        Get
            Return mstrMailCity & ", " & mstrMailState & " " & mstrMailZip

        End Get
    End Property

    Public ReadOnly Property MailLastLine As String

        Get
            mstrMailLastLine = MailCity.Trim & ", " & MailState.Trim & " " & MailZip
            Return mstrMailLastLine
        End Get

    End Property
    Public ReadOnly Property MailErrorCode As String
        Get
            Return mstrMailErrorCode
        End Get
       
    End Property
    Public Property MailDeliveryPointCode As String

        Get
            Return mstrMailDeliveryPointCode
        End Get

        Set(ByVal value As String)
            mstrMailDeliveryPointCode = value
        End Set
    End Property
    Public Property MailDeliveryPointBarcode As String
        Get
            Return mstrMailDeliveryPointBarcode
        End Get
        Set(value As String)
            mstrMailDeliveryPointBarcode = value
        End Set
    End Property
    Public Property MailCarrierRoute As String
        Get
            Return mstrMailCarrierRoute
        End Get
        Set(value As String)

            mstrMailCarrierRoute = value

        End Set
    End Property

    Public Property MailWalkSequence As String

        Get
            Return mstrMailWalkSequence
        End Get

        Set(ByVal value As String)
            mstrMailWalkSequence = value
        End Set
    End Property

    Public ReadOnly Property MailPackageNumber As String

        Get
            Return mstrMailPackageNumber
        End Get

      
    End Property

    Public ReadOnly Property MailTrayNumber As String

        Get
            Return mstrMailTrayNumber
        End Get

       
    End Property

    Public ReadOnly Property MailEndoresementLine As String

        Get
            Return mstrMailEndoresementLine
        End Get

       
    End Property
    Public ReadOnly Property MailKeyline As String
        Get
            Return mstrMailKeyline
        End Get
    End Property

    Public ReadOnly Property MailPresortID As String

        Get
            Return mstrMailPresortID
        End Get

     
    End Property

   

    Public ReadOnly Property MailIntelligentMailBarCode As String

        Get
            Return mstrMailIntelligentMailBarCode
        End Get

      
    End Property
    Public ReadOnly Property MailIntelligentMailBarCodeNumber As String

        Get
            Return mstrMailIntelligentMailBarCodeNumber
        End Get


    End Property
    Public Property LetterType As String
        Get
            Return mstrLetterType
        End Get
        Set(value As String)
            mstrLetterType = value
        End Set
    End Property
End Class
