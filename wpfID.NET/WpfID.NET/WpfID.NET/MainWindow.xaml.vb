﻿Imports Microsoft.Win32
Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient

Class MainWindow

    Dim lblQRclk As Integer = 0
    Dim lblSDclk As Integer = 0
    Dim lblUDclk As Integer = 0
    Dim vc = New BrushConverter()
    Dim hash As String
    Dim qrgen As New MessagingToolkit.QRCode.Codec.QRCodeEncoder

    Private Sub Rectangle_MouseDown(sender As Object, e As MouseButtonEventArgs)
        mnWindow.DragMove()
    End Sub

    Private Sub lblQR_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblQR.MouseLeftButtonDown
        lblQR.FontSize = 35
        lblSD.FontSize = 24
        lblUD.FontSize = 24
        lblQR.Foreground = vc.ConvertFrom("#FFFFFFFF")
        lblSD.Foreground = vc.ConvertFrom("#FFCFD9FF")
        lblUD.Foreground = vc.ConvertFrom("#FFCFD9FF")

        tb1.SelectedIndex = 1
        tb1.SelectedItem = ti1
        ti1.IsSelected = True
    End Sub

    Private Sub lblSD_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblSD.MouseLeftButtonDown
        lblQR.FontSize = 24
        lblSD.FontSize = 35
        lblUD.FontSize = 24
        lblQR.Foreground = vc.ConvertFrom("#FFCFD9FF")
        lblSD.Foreground = vc.ConvertFrom("#FFFFFFFF")
        lblUD.Foreground = vc.ConvertFrom("#FFCFD9FF")

        tb1.SelectedIndex = 2
        tb1.SelectedItem = ti2
        ti2.IsSelected = True

    End Sub

    Private Sub lblUD_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblUD.MouseLeftButtonDown
        lblQR.FontSize = 24
        lblSD.FontSize = 24
        lblUD.FontSize = 35
        lblQR.Foreground = vc.ConvertFrom("#FFCFD9FF")
        lblSD.Foreground = vc.ConvertFrom("#FFCFD9FF")
        lblUD.Foreground = vc.ConvertFrom("#FFFFFFFF")

        tb1.SelectedIndex = 3
        tb1.SelectedItem = ti3
        ti3.IsSelected = True
    End Sub

    Private Function hashgen()
        If txtreg.Text = "" Or txtreg.Text = " " Then
            MessageBox.Show("Please complete form before proceed!", "Warning", vbOKOnly, MessageBoxImage.Exclamation)
            txtreg.Focus()
            Exit Function
        End If

        Dim txt As String
        Dim tmpsrc() As Byte
        Dim tmphs() As Byte

        Try
            txt = txtreg.Text
            tmpsrc = ASCIIEncoding.ASCII.GetBytes(txt)
            tmphs = New MD5CryptoServiceProvider().ComputeHash(tmpsrc)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error while generating hash bytes", vbOK, MessageBoxImage.Error)
            Return 1
        End Try

        Try
            Dim i As Integer
            Dim out As New StringBuilder(tmphs.Length)
            For i = 0 To tmphs.Length - 1
                out.Append(tmphs(i).ToString("X2"))
            Next
            hash = out.ToString()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error while generating hash string", vbOK, MessageBoxImage.Error)
            Return 1
        End Try
        MessageBox.Show(hash)

        Try
            Dim ms As New IO.MemoryStream
            Dim bi As New BitmapImage
            qrgen.Encode(hash).Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            bi.BeginInit()
            bi.StreamSource = ms
            bi.EndInit()
            rechshback.Fill = vc.ConvertFrom("#FFFFFFFF")
            imgqr.Source = bi
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error while generating QR code", vbOK, MessageBoxImage.Error)
            Return 1
        End Try

        Return 0
    End Function

    Private Sub rechash_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles rechash.MouseLeftButtonDown
        hashgen()
    End Sub

    Private Sub lblhashgen_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblhashgen.MouseLeftButtonDown
        hashgen()
    End Sub
    Private Function imgSel()
        Dim dlg As New OpenFileDialog()
        dlg.InitialDirectory = "c:\"
        dlg.Filter = "Image files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All Files (*.*)|*.*"
        dlg.RestoreDirectory = True

        If dlg.ShowDialog() = True Then
            Dim selectedFileName As String = dlg.FileName
            Dim bitmap As New BitmapImage()
            bitmap.BeginInit()
            bitmap.UriSource = New Uri(selectedFileName)
            bitmap.EndInit()
            imgstd.Source = bitmap
        End If
        Return 0
    End Function

    Private Sub recbroimg_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles recbroimg.MouseLeftButtonDown
        imgSel()
    End Sub

    Private Sub lblbroimg_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblbroimg.MouseLeftButtonDown
        imgSel()
    End Sub

    Private Function sqlFacAdd()
        If txtfacID.Text = "" Or txtfacID.Text = " " Or txtfacNM.Text = "" Or txtfacNM.Text = " " Then
            MessageBox.Show("Please complete form before proceed!", "Warning", vbOKOnly, MessageBoxImage.Exclamation)
            txtfacID.Focus()
            Exit Function
        End If

        Dim msg As String()
        Dim cmd As New MySqlCommand("INSERT INTO foss01.facdet VALUES (@facid,@facnm)", connection.con())
        cmd.Parameters.Add("@facid", MySqlDbType.VarChar).Value = txtfacID.Text
        cmd.Parameters.Add("@facnm", MySqlDbType.VarChar).Value = txtfacNM.Text

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            msg = ex.Message.ToString.Split(" ")

            If msg(0) = "Duplicate" Then

                If txtfacNM.Text = "" Or txtfacNM.Text = " " Then
                    MessageBox.Show("Please enter faculty name", "Error", vbOKOnly, MessageBoxImage.Error)
                    Return 0
                End If

                Dim cmdu As New MySqlCommand("UPDATE foss01.facdet SET fac=@facnm WHERE facID=@facid", connection.con())

                cmdu.Parameters.Add("@facnm", MySqlDbType.VarChar).Value = txtfacNM.Text
                cmdu.Parameters.Add("@facid", MySqlDbType.VarChar).Value = txtfacID.Text

                Try
                    cmdu.ExecuteNonQuery()
                Catch ex2 As Exception
                    MessageBox.Show(ex2.Message, "Error while updating duplicate entry", vbOKOnly, MessageBoxImage.Error)
                    Return 0
                End Try

                MessageBox.Show("Faculty Updated!", "Successful", vbOKOnly, MessageBoxImage.Information)
            Else

                MessageBox.Show(ex.Message, "Error while updating database", vbOKOnly, MessageBoxImage.Error)
                Return 1
            End If
            Return 0
        End Try

        MessageBox.Show("Faculty Added!", "Successful", vbOKOnly, MessageBoxImage.Information)
        Return 1
    End Function

    Private Sub lblfac_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblfac.MouseLeftButtonDown
        sqlFacAdd()
    End Sub

    Private Sub recfac_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles recfac.MouseLeftButtonDown
        sqlFacAdd()
    End Sub

    Private Function sqlCorAdd()
        If txtfacID.Text = "" Or txtfacID.Text = " " Or txtcorID.Text = "" Or txtcorID.Text = " " Or txtcorNM.Text = "" Or txtcorNM.Text = " " Then
            MessageBox.Show("Please complete form before proceed!", "Warning", vbOKOnly, MessageBoxImage.Exclamation)
            txtfacID.Focus()
            Exit Function
        End If

        Dim msg As String()
        Dim cmd As New MySqlCommand("INSERT INTO foss01.courdet VALUES (@corid,@cornm,@facid)", connection.con())
        cmd.Parameters.Add("@corid", MySqlDbType.VarChar).Value = txtcorID.Text
        cmd.Parameters.Add("@cornm", MySqlDbType.VarChar).Value = txtcorNM.Text
        cmd.Parameters.Add("@facid", MySqlDbType.VarChar).Value = txtfacID.Text

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            msg = ex.Message.ToString.Split(" ")

            If msg(0) = "Duplicate" Then

                If txtfacNM.Text = "" Or txtfacNM.Text = " " Or txtfacID.Text = "" Or txtfacID.Text = " " Then
                    MessageBox.Show("Please enter details", "Error", vbOKOnly, MessageBoxImage.Error)
                    Return 0
                End If

                Dim cmdu As New MySqlCommand("UPDATE foss01.courdet SET cur=@cornm,facID=@facid WHERE curID=@corid", connection.con())

                cmdu.Parameters.Add("@corid", MySqlDbType.VarChar).Value = txtcorID.Text
                cmdu.Parameters.Add("@cornm", MySqlDbType.VarChar).Value = txtcorNM.Text
                cmdu.Parameters.Add("@facid", MySqlDbType.VarChar).Value = txtfacID.Text

                Try
                    cmdu.ExecuteNonQuery()
                Catch ex2 As Exception
                    MessageBox.Show(ex2.Message, "Error while updating duplicate entry", vbOKOnly, MessageBoxImage.Error)
                    Return 0
                End Try

                MessageBox.Show("Course Updated!", "Successful", vbOKOnly, MessageBoxImage.Information)
            Else

                MessageBox.Show(ex.Message, "Error while updating database", vbOKOnly, MessageBoxImage.Error)
                Return 1
            End If
            Return 0
        End Try

        MessageBox.Show("Course Added!", "Successful", vbOKOnly, MessageBoxImage.Information)
        Return 1
    End Function

    Private Sub lblcor_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblcor.MouseLeftButtonDown
        sqlCorAdd()

    End Sub

    Private Sub reccor_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles reccor.MouseLeftButtonDown
        sqlCorAdd()

    End Sub

    Private Function sqlDelFac()
        If txtfacID.Text = "" Then
            MessageBox.Show("Please enter faculty ID", "Error", vbOKOnly, MessageBoxImage.Error)
            Exit Function
        End If

        If MessageBox.Show("Deleting faculty will delete every course assinged to this faculty" + Environment.NewLine + "Do you want to delete this faculty?", "Warning", vbOK, MessageBoxImage.Exclamation) = vbOK Then
            Dim cmd As New MySqlCommand("DELETE FROM foss01.facdet WHERE facID=@facid", connection.con)
            cmd.Parameters.Add("@facid", MySqlDbType.VarChar).Value = txtfacID.Text

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error while deleting faculty", vbOKOnly, MessageBoxImage.Error)
                Return 1
            End Try
            MessageBox.Show("Faculty deleted!", "Successful", vbOKOnly, MessageBoxImage.Information)
        End If
    End Function

    Private Sub recfacdel_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles recfacdel.MouseLeftButtonDown
        sqlDelFac()
    End Sub

    Private Sub lblfacdel_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblfacdel.MouseLeftButtonDown
        sqlDelFac()
    End Sub
End Class
