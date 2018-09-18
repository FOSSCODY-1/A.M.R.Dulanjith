Imports Microsoft.Win32

Class MainWindow
    Dim lblQRclk As Integer = 0
    Dim lblSDclk As Integer = 0
    Dim lblUDclk As Integer = 0
    Dim vc = New BrushConverter()

    Private Sub Rectangle_MouseDown(sender As Object, e As MouseButtonEventArgs)
        mnWN.DragMove()
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

    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized

    End Sub

    Private Sub brnimg_Click(sender As Object, e As RoutedEventArgs) Handles brnimg.Click
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
            img1.Source = bitmap
        End If
    End Sub
End Class
