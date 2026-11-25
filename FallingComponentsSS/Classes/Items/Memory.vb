Imports System.Drawing.Drawing2D

Public Class Memory

   ' Draws a RAM stick (DIMM): board with chips, gold contacts and the key notch
   Public Shared Sub Draw(g As Graphics, item As MediaItem, tint As Color)

      Dim w As Single = item.Size
      Dim h As Single = item.Size * 0.27F      ' a real DIMM is about 133 x 31 mm (a bit taller here, to stay visible)

      Dim state As GraphicsState = g.Save()
      g.SmoothingMode = SmoothingMode.AntiAlias
      g.TranslateTransform(item.X, item.Y)
      g.RotateTransform(item.Rotation)

      ' 1. Circuit board with the key notch in the bottom edge (a bit off-center, like a real DIMM)
      Dim notchX As Single = w * 0.08F
      Dim notchW As Single = w * 0.03F
      Dim notchD As Single = h * 0.22F
      Dim board() As PointF = {
         New PointF(-w / 2, -h / 2),
         New PointF(w / 2, -h / 2),
         New PointF(w / 2, h / 2),
         New PointF(notchX + notchW / 2, h / 2),
         New PointF(notchX + notchW / 2, h / 2 - notchD),
         New PointF(notchX - notchW / 2, h / 2 - notchD),
         New PointF(notchX - notchW / 2, h / 2),
         New PointF(-w / 2, h / 2)
      }
      Using b As New SolidBrush(tint)
         g.FillPolygon(b, board)
      End Using
      Using p As New Pen(Color.FromArgb(90, Color.Black), 2)
         g.DrawPolygon(p, board)
      End Using

      ' 2. Gold contacts along the bottom edge (none in the notch)
      Dim pinH As Single = h * 0.18F
      Dim pins As Integer = 28
      Dim pitch As Single = w * 0.92F / pins
      Using b As New SolidBrush(Color.Goldenrod)
         For i As Integer = 0 To pins - 1
            Dim px As Single = -w * 0.46F + i * pitch
            If px + pitch * 0.6F > notchX - notchW / 2 AndAlso px < notchX + notchW / 2 Then Continue For
            g.FillRectangle(b, px, h / 2 - pinH, pitch * 0.6F, pinH)
         Next
      End Using

      ' 3. Four black memory chips
      Dim chipW As Single = w * 0.17F
      Dim chipH As Single = h * 0.46F
      Dim chipY As Single = -h * 0.36F
      For i As Integer = 0 To 3
         Dim cx As Single = -w * 0.42F + i * w * 0.22F
         Using b As New SolidBrush(Color.FromArgb(30, 30, 30))
            g.FillRectangle(b, cx, chipY, chipW, chipH)
         End Using
         Using p As New Pen(Color.FromArgb(80, Color.Gray), 1)
            g.DrawRectangle(p, cx, chipY, chipW, chipH)
         End Using
      Next

      g.Restore(state)
   End Sub

End Class
