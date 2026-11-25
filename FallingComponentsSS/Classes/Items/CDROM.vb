Imports System.Drawing.Drawing2D

Public Class CDROM

   Public Shared Sub Draw(g As Graphics, item As MediaItem, tint As Color)

      Dim s As Single = item.Size
      Dim r As Single = s / 2
      Dim holeR As Single = s * 0.07F

      Dim state As GraphicsState = g.Save()
      g.SmoothingMode = SmoothingMode.AntiAlias
      g.TranslateTransform(item.X, item.Y)
      g.RotateTransform(item.Rotation)

      ' 1. The disc shape: a big circle with a small circle cut out (the center hole)
      Using path As New GraphicsPath(FillMode.Alternate)
         path.AddEllipse(-r, -r, s, s)
         path.AddEllipse(-holeR, -holeR, holeR * 2, holeR * 2)

         ' 2. Silver body
         Using b As New SolidBrush(Color.Silver)
            g.FillPath(b, path)
         End Using

         ' 3. Everything below is drawn only inside the disc (and never in the hole)
         g.SetClip(path, CombineMode.Intersect)

         ' 4. Light reflections: four pie slices, so the spin is visible
         Using brWhite As New SolidBrush(Color.FromArgb(130, Color.White)), brTint As New SolidBrush(Color.FromArgb(90, tint))
            g.FillPie(brWhite, -r, -r, s, s, 100, 50)
            g.FillPie(brWhite, -r, -r, s, s, 280, 50)
            g.FillPie(brTint, -r, -r, s, s, 10, 50)
            g.FillPie(brTint, -r, -r, s, s, 190, 50)
         End Using

         ' 5. Hub: a lighter circle around the hole
         Dim hubR As Single = s * 0.24F
         Using b As New SolidBrush(Color.FromArgb(150, Color.WhiteSmoke))
            g.FillEllipse(b, -hubR, -hubR, hubR * 2, hubR * 2)
         End Using
         Using p As New Pen(Color.FromArgb(120, Color.Gray), 1)
            g.DrawEllipse(p, -hubR, -hubR, hubR * 2, hubR * 2)
         End Using

         ' 6. Thin dark edge
         Using p As New Pen(Color.FromArgb(90, Color.Black), 2)
            g.DrawEllipse(p, -r, -r, s, s)
         End Using
      End Using

      g.Restore(state)
   End Sub

End Class