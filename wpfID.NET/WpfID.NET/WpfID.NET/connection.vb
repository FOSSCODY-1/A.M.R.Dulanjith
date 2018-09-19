Imports MySql.Data.MySqlClient
Public Class connection
    Public Shared Function con() As MySqlConnection
        Dim sqlcon As New MySqlConnection
        sqlcon.ConnectionString = "server=den1.mysql6.gear.host;user id=foss01;password=Rr75!E4oF?uA;persistsecurityinfo=True;database=foss01;sslmode=None"

        Try
            sqlcon.Open()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Unable to connect database", vbOKOnly, MessageBoxImage.Error)
            Exit Function
        End Try

        Return sqlcon
    End Function

End Class
