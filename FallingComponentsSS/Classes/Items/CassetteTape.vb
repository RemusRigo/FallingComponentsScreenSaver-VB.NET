Imports System.Drawing.Drawing2D

Public Class CassetteTape

   Public Shared Sub Draw(g As Graphics, item As MediaItem, tint As Color)

      Dim w As Single = item.Size
      Dim h As Single = item.Size * 0.63F      ' a real cassette is about 100 x 63 mm

      Dim state As GraphicsState = g.Save()
      g.SmoothingMode = SmoothingMode.AntiAlias
      g.TranslateTransform(item.X, item.Y)
      g.RotateTransform(item.Rotation)

      ' 1. Plastic body with rounded corners
      Using bodyPath As GraphicsPath = RoundedRect(-w / 2, -h / 2, w, h, w * 0.04F)
         Using b As New SolidBrush(tint)
            g.FillPath(b, bodyPath)
         End Using
         Using p As New Pen(Color.FromArgb(90, Color.Black), 2)
            g.DrawPath(p, bodyPath)
         End Using
      End Using

      ' 2. White label in the upper part
      Dim label As New RectangleF(-w * 0.44F, -h * 0.44F, w * 0.88F, h * 0.5F)
      Using b As New SolidBrush(Color.WhiteSmoke)
         g.FillRectangle(b, label)
      End Using
      Using p As New Pen(Color.Gray, 1)
         g.DrawLine(p, label.Left + w * 0.04F, label.Top + h * 0.09F,
                       label.Right - w * 0.04F, label.Top + h * 0.09F)
      End Using

      ' 3. Dark window with the two reels
      Dim win As New RectangleF(-w * 0.32F, -h * 0.22F, w * 0.64F, h * 0.3F)
      Using b As New SolidBrush(Color.FromArgb(40, 40, 40))
         g.FillRectangle(b, win)
      End Using

      Dim reelR As Single = w * 0.085F
      Dim holeR As Single = w * 0.035F
      Dim reelY As Single = win.Top + win.Height / 2
      For Each reelX As Single In {-w * 0.17F, w * 0.17F}
         Using b As New SolidBrush(Color.WhiteSmoke)
            g.FillEllipse(b, reelX - reelR, reelY - reelR, reelR * 2, reelR * 2)
         End Using
         Using b As New SolidBrush(Color.FromArgb(40, 40, 40))
            g.FillEllipse(b, reelX - holeR, reelY - holeR, holeR * 2, holeR * 2)
         End Using
      Next

      ' 4. Dark bottom part (a trapezoid) with two small holes
      Dim bottom() As PointF = {
         New PointF(-w * 0.3F, h / 2),
         New PointF(-w * 0.22F, h * 0.15F),
         New PointF(w * 0.22F, h * 0.15F),
         New PointF(w * 0.3F, h / 2)
      }
      Using b As New SolidBrush(Color.FromArgb(70, 70, 70))
         g.FillPolygon(b, bottom)
      End Using
      Dim smallR As Single = w * 0.025F
      For Each holeX As Single In {-w * 0.17F, w * 0.17F}
         Using b As New SolidBrush(Color.FromArgb(25, 25, 25))
            g.FillEllipse(b, holeX - smallR, h * 0.33F - smallR, smallR * 2, smallR * 2)
         End Using
      Next

      g.Restore(state)
   End Sub

End Class