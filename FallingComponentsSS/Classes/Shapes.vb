Imports System.Drawing.Drawing2D

' Small drawing helpers shared by the item classes
Public Module Shapes

   ' Builds a rectangle path with rounded corners (radius r)
   Public Function RoundedRect(x As Single, y As Single, w As Single, h As Single, r As Single) As GraphicsPath
      Dim d As Single = r * 2
      Dim path As New GraphicsPath()
      path.AddArc(x, y, d, d, 180, 90)                    ' top left
      path.AddArc(x + w - d, y, d, d, 270, 90)            ' top right
      path.AddArc(x + w - d, y + h - d, d, d, 0, 90)      ' bottom right
      path.AddArc(x, y + h - d, d, d, 90, 90)             ' bottom left
      path.CloseFigure()
      Return path
   End Function

End Module
