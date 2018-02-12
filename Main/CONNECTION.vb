Public Class CONNECTION
    Private mConnectionString As String



    Public Sub New()

        mConnectionString = "Server=usasql;Database=PODFOLIVE;Trusted_Connection=True;"
    End Sub

    Friend Function getConnection() As SqlClient.SqlConnection

        Dim oConnection As New SqlClient.SqlConnection(mConnectionString)

        oConnection.Open()
        Return oConnection



    End Function
End Class
