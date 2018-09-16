Class MainWindow
    Dim lblQRclk As Integer = 0
    Dim lblSDclk As Integer = 0
    Dim lblUDclk As Integer=0
    Dim vc = New BrushConverter()

    Private Sub Rectangle_MouseDown(sender As Object, e As MouseButtonEventArgs)
      mnWN.DragMove()
    End Sub

    Private Sub lblQR_MouseEnter(sender As Object, e As MouseEventArgs) Handles lblQR.MouseEnter
        lblQR.Background = vc.ConvertFrom("#FF2D2D30")

    End Sub

    Private Sub lblQR_MouseLeave(sender As Object, e As MouseEventArgs) Handles lblQR.MouseLeave
        If lblQRclk = 0 Then
            lblQR.Background = vc.ConvertFrom("#FF141414")
        End If
    End Sub

    Private Sub lblSD_MouseEnter(sender As Object, e As MouseEventArgs) Handles lblSD.MouseEnter
        lblSD.Background = vc.ConvertFrom("#FF2D2D30")
    End Sub

    Private Sub lblSD_MouseLeave(sender As Object, e As MouseEventArgs) Handles lblSD.MouseLeave
        If lblSDclk = 0 Then
            lblSD.Background = vc.ConvertFrom("#FF141414")
        End If
    End Sub

    Private Sub lblUD_MouseEnter(sender As Object, e As MouseEventArgs) Handles lblUD.MouseEnter
        lblUD.Background = vc.ConvertFrom("#FF2D2D30")
    End Sub

    Private Sub lblUD_MouseLeave(sender As Object, e As MouseEventArgs) Handles lblUD.MouseLeave
        If lblUDclk = 0 Then
            lblUD.Background = vc.ConvertFrom("#FF141414")
        End If
    End Sub

    Private Sub lblQR_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblQR.MouseLeftButtonDown
        lblQR.Background = vc.ConvertFrom("#FF2D2D30")
        lblSD.Background = vc.ConvertFrom("#FF141414")
        lblUD.Background = vc.ConvertFrom("#FF141414")

        lblQR.FontSize = 25
        lblSD.FontSize = 20
        lblUD.FontSize = 20

        lblQRclk = 1
        lblSDclk = 0
        lblUDclk = 0
    End Sub

    Private Sub lblSD_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblSD.MouseLeftButtonDown
        lblSD.Background = vc.ConvertFrom("#FF2D2D30")
        lblQR.Background = vc.ConvertFrom("#FF141414")
        lblUD.Background = vc.ConvertFrom("#FF141414")

        lblQR.FontSize = 20
        lblSD.FontSize = 25
        lblUD.FontSize = 20

        lblQRclk = 0
        lblSDclk = 1
        lblUDclk = 0
    End Sub

    Private Sub lblUD_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblUD.MouseLeftButtonDown
        lblUD.Background = vc.ConvertFrom("#FF2D2D30")
        lblSD.Background = vc.ConvertFrom("#FF141414")
        lblQR.Background = vc.ConvertFrom("#FF141414")

        lblQR.FontSize = 20
        lblSD.FontSize = 20
        lblUD.FontSize = 25

        lblQRclk = 0
        lblSDclk = 0
        lblUDclk = 1
    End Sub
End Class
