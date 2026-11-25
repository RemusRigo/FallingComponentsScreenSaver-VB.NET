Imports System.Drawing.Drawing2D

Public Class HardDisk

   ' Draws a 3.5" hard disk seen from the top (aluminium cover, platter bump, label)
   Public Shared Sub Draw(g As Graphics, item As MediaItem, tint As Color)

      Dim w As Single = item.Size
      Dim h As Single = item.Size * 0.69F      ' a real 3.5" drive is about 146 x 101 mm

      Dim state As GraphicsState = g.Save()
      g.SmoothingMode = SmoothingMode.AntiAlias
      g.TranslateTransform(item.X, item.Y)
      g.RotateTransform(item.Rotation)

      ' 1. Aluminium body with rounded corners
      Using bodyPath As GraphicsPath = RoundedRect(-w / 2, -h / 2, w, h, w * 0.03F)
         Using b As New SolidBrush(Color.FromArgb(180, 184, 190))
            g.FillPath(b, bodyPath)
         End Using
         Using p As New Pen(Color.FromArgb(90, Color.Black), 2)
            g.DrawPath(p, bodyPath)
         End Using
      End Using

      ' 2. Slightly lighter top cover, inset from the edge
      Using lidPath As GraphicsPath = RoundedRect(-w * 0.46F, -h * 0.44F, w * 0.92F, h * 0.88F, w * 0.04F)
         Using b As New SolidBrush(Color.FromArgb(205, 208, 212))
            g.FillPath(b, lidPath)
         End Using
      End Using

      ' 3. Round bump above the platters, with the spindle in the middle
      Dim platR As Single = h * 0.4F
      Dim platX As Single = -w * 0.17F
      Using p As New Pen(Color.FromArgb(120, Color.Gray), 1)
         g.DrawEllipse(p, platX - platR, -platR, platR * 2, platR * 2)
      End Using
      Dim hubR As Single = h * 0.09F
      Using b As New SolidBrush(Color.FromArgb(150, 150, 155))
         g.FillEllipse(b, platX - hubR, -hubR, hubR * 2, hubR * 2)
      End Using
      Using b As New SolidBrush(Color.FromArgb(90, 90, 95))
         g.FillEllipse(b, platX - hubR * 0.35F, -hubR * 0.35F, hubR * 0.7F, hubR * 0.7F)
      End Using

      ' 4. Label on the right side, with three "text" lines
      Dim label As New RectangleF(w * 0.1F, -h * 0.36F, w * 0.32F, h * 0.72F)
      Using b As New SolidBrush(tint)
         g.FillRectangle(b, label)
      End Using
      Using p As New Pen(Color.FromArgb(200, Color.WhiteSmoke), 1)
         For i As Integer = 1 To 3
            Dim ly As Single = label.Top + label.Height * i / 4.0F
            Dim right As Single = If(i = 3, label.Right - w * 0.12F, label.Right - w * 0.04F)
            g.DrawLine(p, label.Left + w * 0.04F, ly, right, ly)
         Next
      End Using

      ' 5. Four screws in the corners
      Dim screwR As Single = w * 0.022F
      For Each sx As Single In {-w * 0.42F, w * 0.42F}
         For Each sy As Single In {-h * 0.39F, h * 0.39F}
            Using b As New SolidBrush(Color.FromArgb(110, 110, 115))
               g.FillEllipse(b, sx - screwR, sy - screwR, screwR * 2, screwR * 2)
            End Using
         Next
      Next

      g.Restore(state)
   End Sub

End Class
