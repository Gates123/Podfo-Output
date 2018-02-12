

Public Class colLetterTypes
    Implements System.Collections.IEnumerable
    Public mCol As Collection
    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        GetEnumerator = mCol.GetEnumerator
    End Function
    Public Sub Remove(ByVal vntIndexKey As Object)
        mCol.Remove(vntIndexKey)
    End Sub
    Private Sub Class_Initialize_Renamed()
        mCol = New Collection

    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub
    Private Sub Class_Terminate_Renamed()
        mCol = Nothing
    End Sub
    Protected Overrides Sub Finalize()
        Class_Terminate_Renamed()
        MyBase.Finalize()
    End Sub
    Default Public ReadOnly Property Item(ByVal vntIndexKey As Object) As clsLetterTypes
        Get
            Item = mCol.Item(vntIndexKey)
        End Get
    End Property
    Public Function Add(ByVal strType As String, ByVal strSubType As String, ByVal strLangauage As String, Optional ByVal skey As String = "") As clsLetterTypes
        Dim objNewLetter As New clsLetterTypes


        objNewLetter.LetterLanguage = strLangauage
        objNewLetter.LetterSubtype = strSubType
        objNewLetter.LetterType = strType

        If skey.Trim.Length = 0 Then
            mCol.Add(objNewLetter)
        Else
            mCol.Add(objNewLetter, skey)
        End If
        Add = objNewLetter
        objNewLetter = Nothing
    End Function

End Class
