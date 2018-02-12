Public Class clsLetterTypes
    Public mstrLetterType As String
    Private mintLetterTypeID As Integer
    Private mstrLetterSubtype As String
    Private mstrLetterLanguage As String

    Public Property LetterType As String

        Get
            Return mstrLetterType
        End Get

        Set(ByVal value As String)
            mstrLetterType = value
        End Set
    End Property

    Public Property LetterTypeID As Integer

        Get
            Return mintLetterTypeID
        End Get

        Set(ByVal value As Integer)
            mintLetterTypeID = value
        End Set
    End Property

    Public Property LetterSubtype As String

        Get
            Return mstrLetterSubtype
        End Get

        Set(ByVal value As String)
            mstrLetterSubtype = value
        End Set
    End Property

    Public Property LetterLanguage As String

        Get
            Return mstrLetterLanguage
        End Get

        Set(ByVal value As String)
            mstrLetterLanguage = value
        End Set
    End Property
End Class
