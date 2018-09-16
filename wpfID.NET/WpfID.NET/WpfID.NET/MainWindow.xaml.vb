Class MainWindow
    Private Sub Window_MouseDown(sender As Object, e As MouseButtonEventArgs)
        ' Me.DragMove()

    End Sub

    Private Sub Rectangle_MouseDown(sender As Object, e As MouseButtonEventArgs)
        mainWn.DragMove()
    End Sub

    Private Sub Label_MouseMove(sender As Object, e As MouseEventArgs)

    End Sub

    Private Sub lblQR_MouseEnter(sender As Object, e As MouseEventArgs) Handles lblQR.MouseEnter
        Dim vc = New BrushConverter()
        lblQR.Background = vc.ConvertFrom("#FF2D2D30")

    End Sub

    Private Sub lblQR_MouseLeave(sender As Object, e As MouseEventArgs) Handles lblQR.MouseLeave
        Dim vc = New BrushConverter()
        lblQR.Background = vc.ConvertFrom("#FF141414")
    End Sub

    Private Sub lblSD_MouseEnter(sender As Object, e As MouseEventArgs) Handles lblSD.MouseEnter
        Dim vc = New BrushConverter()
        lblSD.Background = vc.ConvertFrom("#FF2D2D30")
    End Sub

    Private Sub lblSD_MouseLeave(sender As Object, e As MouseEventArgs) Handles lblSD.MouseLeave
        Dim vc = New BrushConverter()
        lblSD.Background = vc.ConvertFrom("#FF141414")
    End Sub

    Private Sub lblUD_MouseEnter(sender As Object, e As MouseEventArgs) Handles lblUD.MouseEnter
        Dim vc = New BrushConverter()
        lblUD.Background = vc.ConvertFrom("#FF2D2D30")
    End Sub

    Private Sub lblUD_MouseLeave(sender As Object, e As MouseEventArgs) Handles lblUD.MouseLeave
        Dim vc = New BrushConverter()
        lblUD.Background = vc.ConvertFrom("#FF141414")
    End Sub
End Class
