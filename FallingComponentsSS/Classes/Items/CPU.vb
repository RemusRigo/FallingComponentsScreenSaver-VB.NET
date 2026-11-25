Imports System.Drawing.Drawing2D

Public Class CPU

   ' Draws a processor seen from the top (substrate, metal heat spreader, pin 1 marker)
   Public Shared Sub Draw(g As Graphics, item As MediaItem, tint As Color)

      Dim s As Single = item.Size
      Dim h As Single = s / 2

      Dim state As GraphicsState = g.Save()
      g.SmoothingMode = SmoothingMode.AntiAlias
      g.TranslateTransform(item.X, item.Y)
      g.RotateTransform(item.Rotation)

      ' 1. Substrate (the coloured board under the metal lid)
      Using subPath As GraphicsPath = RoundedRect(-h, -h, s, s, s * 0.03F)
         Using b As New SolidBrush(tint)
            g.FillPath(b, subPath)
         End Using
         Using p As New Pen(Color.FromArgb(90, Color.Black), 2)
            g.DrawPath(p, subPath)
         End Using
      End Using

      ' 2. Small gold components around the lid
      Dim cw As Single = s * 0.05F
      Dim ch As Single = s * 0.025F
      Using b As New SolidBrush(Color.Goldenrod)
         For Each cx As Single In {-s * 0.2F, 0, s * 0.2F}
            g.FillRectangle(b, cx - cw / 2, -h + s * 0.04F, cw, ch)    ' top edge
            g.FillRectangle(b, cx - cw / 2, h - s * 0.04F - ch, cw, ch) ' bottom edge
         Next
      End Using

      ' 3. Metal heat spreader
      Dim lid As Single = s * 0.68F
      Using lidPath As GraphicsPath = RoundedRect(-lid / 2, -lid / 2, lid, lid, s * 0.05F)
         Using b As New LinearGradientBrush(New PointF(-lid / 2, -lid / 2), New PointF(lid / 2, lid / 2),
                                            Color.FromArgb(225, 228, 232), Color.FromArgb(160, 164, 170))
            g.FillPath(b, lidPath)
         End Using
         Using p As New Pen(Color.FromArgb(110, Color.Gray), 1)
            g.DrawPath(p, lidPath)
         End Using
      End Using

      ' 4. Engraved "text" on the lid
      Using p As New Pen(Color.FromArgb(140, Color.DimGray), 1)
         g.DrawLine(p, -lid * 0.35F, -lid * 0.15F, lid * 0.35F, -lid * 0.15F)
         g.DrawLine(p, -lid * 0.35F, lid * 0.02F, lid * 0.2F, lid * 0.02F)
         g.DrawLine(p, -lid * 0.35F, lid * 0.19F, lid * 0.05F, lid * 0.19F)
      End Using

      ' 5. Gold triangle in the bottom left corner (pin 1 marker)
      Dim t As Single = s * 0.1F
      Dim corner As Single = h - s * 0.04F
      Dim marker() As PointF = {
         New PointF(-corner, corner),
         New PointF(-corner + t, corner),
         New PointF(-corner, corner - t)
      }
      Using b As New SolidBrush(Color.Gold)
         g.FillPolygon(b, marker)
      End Using

      g.Restore(state)
   End Sub

End Class
