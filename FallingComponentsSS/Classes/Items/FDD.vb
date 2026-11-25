
Imports System.Drawing.Drawing2D

Public Class FDD

   ' Draws one floppy disk. No object needed: FloppyDisk.Draw(...)
   Public Shared Sub Draw(g As Graphics, item As MediaItem, bodyColor As Color)

      Dim s As Single = item.Size
      Dim h As Single = s / 2          ' half size, because we draw around (0,0)
      Dim cut As Single = s * 0.12F    ' the cut corner at the top right

      ' Save the graphics state, then move and rotate the "canvas"
      Dim state As GraphicsState = g.Save()
      g.SmoothingMode = SmoothingMode.AntiAlias
      g.TranslateTransform(item.X, item.Y)
      g.RotateTransform(item.Rotation)

      ' 1. Plastic body (a square with one corner cut off)
      Dim body() As PointF = {
          New PointF(-h, -h),
          New PointF(h - cut, -h),
          New PointF(h, -h + cut),
          New PointF(h, h),
          New PointF(-h, h)
      }
      Using b As New SolidBrush(bodyColor)
         g.FillPolygon(b, body)
      End Using
      Using p As New Pen(Color.FromArgb(60, 0, 0, 0), 2)
         g.DrawPolygon(p, body)
      End Using

      ' 2. Metal shutter at the top
      Dim shutter As New RectangleF(-s * 0.25F, -h, s * 0.5F, s * 0.28F)
      Using b As New SolidBrush(Color.Silver)
         g.FillRectangle(b, shutter)
      End Using

      ' 3. Dark slot inside the shutter
      Dim slot As New RectangleF(s * 0.08F, -h + s * 0.04F, s * 0.1F, s * 0.2F)
      Using b As New SolidBrush(Color.DimGray)
         g.FillRectangle(b, slot)
      End Using

      ' 4. White label at the bottom, with two "text" lines
      Dim label As New RectangleF(-s * 0.38F, s * 0.05F, s * 0.76F, s * 0.43F)
      Using b As New SolidBrush(Color.WhiteSmoke)
         g.FillRectangle(b, label)
      End Using
      Using p As New Pen(Color.Gray, 1)
         g.DrawLine(p, label.Left + s * 0.06F, label.Top + s * 0.13F,
                       label.Right - s * 0.06F, label.Top + s * 0.13F)
         g.DrawLine(p, label.Left + s * 0.06F, label.Top + s * 0.27F,
                       label.Right - s * 0.2F, label.Top + s * 0.27F)
      End Using

      ' Restore the canvas so the next item is not affected
      g.Restore(state)
   End Sub

End Class