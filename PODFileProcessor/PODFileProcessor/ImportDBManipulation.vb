Imports System.Data.SqlClient
Imports System.IO

Public Module ImportDBManipulation

    Public mstrLogFileLocation As String


    Public Function GetFileTypeID(ByRef objMainConn As SqlConnection, strType As String) As Integer

        Try

            Dim cmdSql As New SqlCommand
            Dim rdSql As SqlDataReader

            With cmdSql
                .CommandText = "USP_Select_PODMasterFileTypeID"
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@P_Type", strType)
                .Connection = objMainConn

                rdSql = cmdSql.ExecuteReader

                If rdSql.Read = True Then
                    GetFileTypeID = rdSql.Item("PODMasterFileTypeID")
                Else
                    GetFileTypeID = -1
                End If
            End With

            cmdSql.Dispose()
            rdSql.Close()

        Catch ex As Exception
            GetFileTypeID = -1
            Throw New ApplicationException("Error from GetFileTypeID " & ex.Message)
        End Try

    End Function
    Public Function GetLetterTypeID(ByRef objMainConn As SqlConnection, strType As String, strSubType As String, strLanguage As String, ByRef intPageCount As Integer, ByRef dblWeight As Double, ByRef dblThickness As Double) As Integer
        Try
            Dim cmdSql As New SqlCommand

            Dim rdSql As SqlDataReader

            With cmdSql
                .CommandText = "USP_Select_PODMasterLetterTypeID"
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@P_lettertype", strType)
                .Parameters.AddWithValue("@P_SubType", strSubType)
                .Parameters.AddWithValue("@P_Language", strLanguage)

                .Connection = objMainConn
                rdSql = cmdSql.ExecuteReader

                If rdSql.Read = True Then
                    GetLetterTypeID = rdSql.Item("PODMasterLetterTypeID")
                    intPageCount = rdSql.Item("PageCount")
                    dblWeight = rdSql.Item("Weight")
                    dblThickness = rdSql.Item("Thickness")
                Else
                    GetLetterTypeID = 10030
                End If
            End With

            cmdSql.Dispose()
            rdSql.Close()
        Catch ex As Exception
            GetLetterTypeID = -1
            Throw New ApplicationException(ex.Message)
        End Try

    End Function
    Public Function InsertImportFile(ByRef objMainConn As SqlConnection, ByVal strFileName As String, ByVal intPODBatchID As Integer, ByVal intPODMasterfileTypeID As Integer) As Integer
        Try
            Dim cmdSql As New SqlCommand
            Dim rdSql As SqlDataReader

            With cmdSql
                .CommandText = "USP_INSERT_IMPORT_FILE"
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@P_ImportFileName", strFileName)
                .Parameters.AddWithValue("@P_PODBatchID", intPODBatchID)
                .Parameters.AddWithValue("@P_POdMasterFileTypeID", intPODMasterfileTypeID)
                .Connection = objMainConn

                rdSql = cmdSql.ExecuteReader

                If rdSql.Read = True Then
                    InsertImportFile = rdSql.Item(0)
                Else
                    InsertImportFile = -1
                End If
            End With

            cmdSql.Dispose()
            rdSql.Close()
        Catch ex As Exception
            InsertImportFile = -1
            Throw New ApplicationException(ex.Message)
        End Try
    End Function

    Public Sub WriteToLogfile(ByVal strToWrite As String)
        Dim wrLog As StreamWriter
        wrLog = File.AppendText(mstrLogFileLocation)
        wrLog.WriteLine(strToWrite & "                " & DateTime.Now())
        wrLog.Flush()
        wrLog.Close()
    End Sub


End Module
