'--------------------------------------------------------------------------------------------------
' FallingComponentsSS: Miscellaneous.vb
'    © 2026 Remus Rigo
'       v1.0.20261125
'--------------------------------------------------------------------------------------------------

Module Miscellaneous

   '-----------------------------------------------------------------------------------------------
   ' ColorFromHSV: converts hue (0-360), saturation (0-1) and value (0-1) to a Color
   Public Function ColorFromHsv(h As Double, s As Double, v As Double) As Color
      h = h Mod 360
      Dim c As Double = v * s
      Dim x As Double = c * (1 - Math.Abs((h / 60) Mod 2 - 1))
      Dim m As Double = v - c
      Dim r, g, b As Double

      Select Case CInt(Math.Floor(h / 60))
         Case 0 : r = c : g = x : b = 0
         Case 1 : r = x : g = c : b = 0
         Case 2 : r = 0 : g = c : b = x
         Case 3 : r = 0 : g = x : b = c
         Case 4 : r = x : g = 0 : b = c
         Case Else : r = c : g = 0 : b = x
      End Select

      Return Color.FromArgb(CInt((r + m) * 255), CInt((g + m) * 255), CInt((b + m) * 255))
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Clamp
   Public Function Clamp(v As Integer, min As Integer, max As Integer) As Integer
      Return Math.Min(max, Math.Max(min, v))
   End Function

End Module
