
Public Enum MediaType
   FDD
   CDROM
   CassetteTape
   HardDisk
   CPU
   Memory
End Enum

Public Class MediaItem

   Public Property Kind As MediaType
   Public Property X As Single              ' horizontal position
   Public Property Y As Single              ' vertical position
   Public Property Speed As Single          ' pixels per tick (downward)
   Public Property Size As Integer          ' width of the item in pixels
   Public Property Rotation As Single       ' current angle in degrees
   Public Property RotationSpeed As Single  ' degrees per tick
   Public Property Tint As Color = Color.RoyalBlue   ' body color

   Public Sub New(kind As MediaType, x As Single, y As Single, speed As Single, size As Integer, rotationSpeed As Single)
      Me.Kind = kind
      Me.X = x
      Me.Y = y
      Me.Speed = speed
      Me.Size = size
      Me.RotationSpeed = rotationSpeed
      Me.Rotation = 0
   End Sub

   ' Called on every timer tick: move down and spin a little
   Public Sub Update()
      Y += Speed
      Rotation += RotationSpeed
      If Rotation >= 360 Then Rotation -= 360
   End Sub

   ' True when the item has fully left the bottom of the screen
   Public Function IsOffScreen(screenHeight As Integer) As Boolean
      Return Y > screenHeight + Size
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Detects the moment of impact ~ True when the item touches the bottom edge of the screen
   Public Function HitsBottom(screenHeight As Integer) As Boolean
      Return Y + Size / 2 >= screenHeight
   End Function

End Class