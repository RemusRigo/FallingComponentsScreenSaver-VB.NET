Imports System.Drawing.Drawing2D

Public Class Fragment

   Private Const GRAVITY As Single = 0.35F

   Private ReadOnly _source As MediaItem   ' an unrotated copy at (0,0), used only for drawing
   Private ReadOnly _qx As Integer         ' -1 = left half, +1 = right half
   Private ReadOnly _qy As Integer         ' -1 = top half,  +1 = bottom half

   Public Property X As Single
   Public Property Y As Single
   Public Property VX As Single            ' horizontal speed
   Public Property VY As Single            ' vertical speed
   Public Property Rotation As Single
   Public Property RotationSpeed As Single

   ' quadrant: 0 = top left, 1 = top right, 2 = bottom left, 3 = bottom right
   Public Sub New(item As MediaItem, quadrant As Integer, rnd As Random)
      _qx = If(quadrant Mod 2 = 0, -1, 1)
      _qy = If(quadrant < 2, -1, 1)
      _source = New MediaItem(item.Kind, 0, 0, 0, item.Size, 0) With {.Tint = item.Tint}

      ' center of this quarter, relative to the floppy center
      Dim q As Single = item.Size / 4.0F
      Dim ox As Single = _qx * q
      Dim oy As Single = _qy * q

      ' rotate that offset by the floppy's angle to get its position on screen
      Dim a As Double = item.Rotation * Math.PI / 180
      X = item.X + CSng(ox * Math.Cos(a) - oy * Math.Sin(a))
      Y = item.Y + CSng(ox * Math.Sin(a) + oy * Math.Cos(a))

      Rotation = item.Rotation
      VX = _qx * CSng(rnd.NextDouble() * 3 + 1)         ' flies outward
      VY = -CSng(rnd.NextDouble() * 4 + 3)              ' bounces upward
      RotationSpeed = _qx * CSng(rnd.NextDouble() * 6 + 2)
   End Sub

   Public Sub Update()
      VY += GRAVITY
      X += VX
      Y += VY
      Rotation += RotationSpeed
   End Sub

   Public Function IsOffScreen(w As Integer, h As Integer) As Boolean
      Dim s As Integer = _source.Size
      Return Y > h + s OrElse X < -s OrElse X > w + s
   End Function

   Public Sub Draw(g As Graphics)
      Dim s As Single = _source.Size
      Dim q As Single = s / 4.0F

      Dim state As GraphicsState = g.Save()
      g.TranslateTransform(X, Y)
      g.RotateTransform(Rotation)
      ' move the origin to the floppy center, as seen from this quarter
      g.TranslateTransform(-_qx * q, -_qy * q)

      ' only this quarter of the floppy is visible
      g.SetClip(New RectangleF(If(_qx < 0, -s / 2, 0), If(_qy < 0, -s / 2, 0), s / 2, s / 2))

      MediaRenderer.Draw(g, _source, _source.Tint)
      g.Restore(state)
   End Sub

End Class