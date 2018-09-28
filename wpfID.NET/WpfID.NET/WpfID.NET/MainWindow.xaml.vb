Imports Microsoft.Win32
Imports System.Security.Cryptography
Imports System.Text
Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Windows.Media.Imaging

Class MainWindow
    Dim vc = New BrushConverter()
    Dim hash As String
    Dim qrgen As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
    Dim dsFac, dsCur, dsStd As New DataSet()
    Dim qryFac, qryCur As String
    Dim facFound, curFound As Integer
    Dim selectedFileName As String
    Dim bitmap As New BitmapImage()
    Dim bi As New BitmapImage
    Dim dbimg() As Byte
    Dim ms As System.IO.MemoryStream


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
        dlg.Filter = "Image files (*.jpg)|*.jpg|PNG files (*.png)|*.png"
        dlg.RestoreDirectory = True

        If dlg.ShowDialog() = True Then
            selectedFileName = dlg.FileName

            Dim bitmap1 As New BitmapImage()
            bitmap1.BeginInit()
            bitmap1.UriSource = New Uri(selectedFileName)
            bitmap1.EndInit()

            bitmap = bitmap1
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

    Private Function sqlDelCor()
        If txtcorID.Text = "" Then
            MessageBox.Show("Please enter Course ID", "Error", vbOKOnly, MessageBoxImage.Error)
            Exit Function
        End If

        If MessageBox.Show("Do you want to delete this course?", "Warning", vbOK, MessageBoxImage.Exclamation) = vbOK Then
            Dim cmd As New MySqlCommand("DELETE FROM foss01.courdet WHERE curID=@curID", connection.con)
            cmd.Parameters.Add("@curID", MySqlDbType.VarChar).Value = txtcorID.Text

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error while deleting course", vbOKOnly, MessageBoxImage.Error)
                Return 1
            End Try
            MessageBox.Show("Course deleted!", "Successful", vbOKOnly, MessageBoxImage.Information)
        End If
    End Function

    Private Sub lblcordel_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblcordel.MouseLeftButtonDown
        sqlDelCor()
    End Sub

    Private Sub btndgv_Click(sender As Object, e As RoutedEventArgs) Handles btndgv.Click
        dsFacGen()
        dgv1.ItemsSource = dsFac.Tables(0).DefaultView
    End Sub

    Private Sub reccordel_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles reccordel.MouseLeftButtonDown
        sqlDelCor()
    End Sub

    Private Sub txtfac_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtfac.LostFocus
        If Not txtfac.Text = "" Then
            Dim i As Integer
            facFound = 0
            For i = 0 To dsFac.Tables(0).Rows.Count - 1
                If txtfac.Text = dsFac.Tables(0).Rows(i).Item(0) Then
                    facFound = 1
                    Exit Sub
                End If
            Next

            MessageBox.Show("Cannot find this Faculty ID in database." + Environment.NewLine + "Please check again", "Error", vbOKOnly, MessageBoxImage.Error)
        End If

    End Sub

    Private Sub txtcor_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtcor.LostFocus
        If Not txtcor.Text = "" Then
            Dim i As Integer
            curFound = 0
            For i = 0 To dsCur.Tables(0).Rows.Count - 1
                If txtcor.Text = dsCur.Tables(0).Rows(i).Item(0) Then
                    curFound = 1
                    Exit Sub
                End If
            Next

            MessageBox.Show("Cannot find this Course ID in database." + Environment.NewLine + "Please check again", "Error", vbOKOnly, MessageBoxImage.Error)
        End If

    End Sub

    Private Function dsFacGen()
        qryFac = "SELECT * FROM foss01.facdet"
        Me.Cursor = Cursors.Wait
        Try
            Dim da As New MySqlDataAdapter(qryFac, connection.con)
            da.Fill(dsFac)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Errot while genarating faculty dataset", vbOKOnly, MessageBoxImage.Error)
            Me.Cursor = Cursors.Arrow
            Return 0
        End Try

        Me.Cursor = Cursors.Arrow
        Return 1
    End Function

    Private Sub imgCal_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles imgCal.MouseLeftButtonDown
        If cal1.Visibility = Visibility.Visible Then
            cal1.Visibility = Visibility.Collapsed
        Else
            cal1.Visibility = Visibility.Visible
        End If
    End Sub

    Private Sub cal1_SelectedDatesChanged(sender As Object, e As SelectionChangedEventArgs) Handles cal1.SelectedDatesChanged
        txtdob.Text = cal1.SelectedDate.Value.ToString("yyyy - MMMM - dd")
        cal1.Visibility = Visibility.Collapsed
    End Sub

    Private Sub lblStd_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblStd.MouseLeftButtonDown
        addSTD()
    End Sub

    Private Function dsCurGen()
        qryCur = "SELECT * FROM foss01.courdet"
        Me.Cursor = Cursors.Wait
        Try
            Dim da As New MySqlDataAdapter(qryCur, connection.con)
            da.Fill(dsCur)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Errot while genarating course dataset", vbOKOnly, MessageBoxImage.Error)
            Me.Cursor = Cursors.Arrow
            Return 0
        End Try

        Me.Cursor = Cursors.Arrow
        Return 1
    End Function

    Private Sub txtreg_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtreg.TextChanged
        If txtreg.Text = "" Then
            Exit Sub
        End If
        Dim i As Integer

        For i = 0 To dsStd.Tables(0).Rows.Count - 1
            If txtreg.Text = dsStd.Tables(0).Rows(i).Item(0) Then
                txtnm.Text = dsStd.Tables(0).Rows(i).Item(1)
                txtdob.Text = dsStd.Tables(0).Rows(i).Item(2)
                txtid.Text = dsStd.Tables(0).Rows(i).Item(3)
                txtfac.Text = dsStd.Tables(0).Rows(i).Item(4)
                txtcor.Text = dsStd.Tables(0).Rows(i).Item(5)

                'dbimg = dsStd.Tables(0).Rows(i).Item(6)
                'ms = New System.IO.MemoryStream(dbimg)
                'Dim im As Image = Drawing.Image.FromStream(ms)
                'bitmap.BeginInit()
                'bitmap.StreamSource = ms
                'bitmap.EndInit()
                'bitmap.Freeze()

                'imgstd.Source = bitmap2
                Exit Sub
            End If
        Next
    End Sub

    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        facFound = 0
        curFound = 0
        dsFacGen()
        dsCurGen()
        dsStdGen()
        txtdob.IsReadOnly = True
    End Sub

    Private Function addSTD()
        Dim msg() As String
        If facFound = 0 Then
            MessageBox.Show("Please enter valid Faculty ID", "Error", vbOKOnly, MessageBoxImage.Error)
            Return 0
        End If

        If curFound = 0 Then
            MessageBox.Show("Please enter valid Course ID", "Error", vbOKOnly, MessageBoxImage.Error)
            Return 0
        End If

        If txtreg.Text = "" Or txtnm.Text = "" Or txtdob.Text = "" Or txtid.Text = "" Then
            MessageBox.Show("Please complete details", "Error", vbOKOnly, MessageBoxImage.Error)
            Return 0
        End If

        If selectedFileName = "" Then
            MessageBox.Show("Please add image", "Error", vbOKOnly, MessageBoxImage.Error)
            Return 0
        End If

        Dim sqlAddStd As New MySqlCommand("INSERT INTO foss01.stddet VALUES(@regno,@stdnm,@dob,@nic,@facid,@curid,@img)", connection.con)

        sqlAddStd.Parameters.Add("@regno", MySqlDbType.Int16).Value = txtreg.Text
        sqlAddStd.Parameters.Add("@stdnm", MySqlDbType.VarChar).Value = txtnm.Text
        sqlAddStd.Parameters.Add("@dob", MySqlDbType.Date).Value = cal1.SelectedDate.Value
        sqlAddStd.Parameters.Add("@nic", MySqlDbType.VarChar).Value = txtid.Text
        sqlAddStd.Parameters.Add("@facid", MySqlDbType.Int16).Value = txtfac.Text
        sqlAddStd.Parameters.Add("@curid", MySqlDbType.Int16).Value = txtcor.Text
        sqlAddStd.Parameters.Add("@img", MySqlDbType.Blob).Value = bitmap

        Try
            sqlAddStd.ExecuteNonQuery()
        Catch ex As Exception
            msg = ex.Message.ToString.Split(" ")

            If msg(0) = "Duplicate" Then

                Dim cmdu As New MySqlCommand("UPDATE foss01.stddet SET stdName=@stdnm,dob=@dob,nic=@nic,facID=facid,courID=@curid,img=@img WHERE regNo=@regno", connection.con())

                cmdu.Parameters.Add("@regno", MySqlDbType.Int16).Value = txtreg.Text
                cmdu.Parameters.Add("@stdnm", MySqlDbType.VarChar).Value = txtnm.Text
                cmdu.Parameters.Add("@dob", MySqlDbType.Date).Value = cal1.SelectedDate.Value
                cmdu.Parameters.Add("@nic", MySqlDbType.VarChar).Value = txtid.Text
                cmdu.Parameters.Add("@facid", MySqlDbType.Int16).Value = txtfac.Text
                cmdu.Parameters.Add("@curid", MySqlDbType.Int16).Value = txtcor.Text
                cmdu.Parameters.Add("@img", MySqlDbType.Blob).Value = bitmap

                Try
                    cmdu.ExecuteNonQuery()
                Catch ex2 As Exception
                    MessageBox.Show(ex2.Message, "Error while updating duplicate entry", vbOKOnly, MessageBoxImage.Error)
                    Return 0
                End Try
                dsStdGen()
                MessageBox.Show("Student Updated!", "Successful", vbOKOnly, MessageBoxImage.Information)
            Else
                MessageBox.Show(ex.Message, "Error while updating database", vbOKOnly, MessageBoxImage.Error)
                Return 1
            End If
            Return 0
        End Try
        dsStdGen()
        MessageBox.Show("Student Added!", "Successful", vbOKOnly, MessageBoxImage.Information)
        Return 1
    End Function

    Private Function dsStdGen()
        qryFac = "SELECT * FROM foss01.stddet"
        Me.Cursor = Cursors.Wait
        Try
            Dim da As New MySqlDataAdapter(qryFac, connection.con)
            da.Fill(dsStd)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Errot while genarating Student dataset", vbOKOnly, MessageBoxImage.Error)
            Me.Cursor = Cursors.Arrow
            Return 0
        End Try

        Me.Cursor = Cursors.Arrow
        Return 1
    End Function
End Class
