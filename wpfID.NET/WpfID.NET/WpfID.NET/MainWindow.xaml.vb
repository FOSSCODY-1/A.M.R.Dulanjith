﻿Class MainWindow
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

        lblQR.FontSize = 30
        lblSD.FontSize = 24
        lblUD.FontSize = 24

        lblQRclk = 1
        lblSDclk = 0
        lblUDclk = 0

        tb1.SelectedIndex = 1
        tb1.SelectedItem = ti1
        ti1.IsSelected = True
    End Sub

    Private Sub lblSD_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblSD.MouseLeftButtonDown
        lblSD.Background = vc.ConvertFrom("#FF2D2D30")
        lblQR.Background = vc.ConvertFrom("#FF141414")
        lblUD.Background = vc.ConvertFrom("#FF141414")

        lblQR.FontSize = 24
        lblSD.FontSize = 30
        lblUD.FontSize = 24

        lblQRclk = 0
        lblSDclk = 1
        lblUDclk = 0

        tb1.SelectedIndex = 2
        tb1.SelectedItem = ti2
        ti2.IsSelected = True

    End Sub

    Private Sub lblUD_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles lblUD.MouseLeftButtonDown
        lblUD.Background = vc.ConvertFrom("#FF2D2D30")
        lblSD.Background = vc.ConvertFrom("#FF141414")
        lblQR.Background = vc.ConvertFrom("#FF141414")

        lblQR.FontSize = 24
        lblSD.FontSize = 24
        lblUD.FontSize = 30

        lblQRclk = 0
        lblSDclk = 0
        lblUDclk = 1

        tb1.SelectedIndex = 3
        tb1.SelectedItem = ti3
        ti3.IsSelected = True
    End Sub

    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized

    End Sub
End Class
