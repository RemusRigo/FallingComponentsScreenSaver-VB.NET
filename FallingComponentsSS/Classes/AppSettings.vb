'--------------------------------------------------------------------------------------------------
' FallingComponentsSS: AppSettings.vb: Class to Save/Load settings to the Registry
'    © 2026 Remus Rigo
'       v1.0.20261125
'--------------------------------------------------------------------------------------------------

Imports Microsoft.Win32

Public Class AppSettings

   Private Const regAppPath As String = "Software\Remus RIGO\FallingComponentsSS-VB.NET"

   '-----------------------------------------------------------------------------------------------
   ' Colors the user can pick from (name shown in frmCfg / saved in the registry, color used to draw)
   ' add a line here and it shows up in frmCfg automatically
   Public Shared ReadOnly Palette As New Dictionary(Of String, Color) From {
      {"Blue", Color.RoyalBlue},
      {"Green", Color.SeaGreen},
      {"Red", Color.Firebrick}
   }

   '-----------------------------------------------------------------------------------------------
   ' Settings
   Public Property EverythingRandom As Boolean = False   ' random colors, all objects, random speed per item
   Public Property RandomColor As Boolean = False        ' every item gets a random color
   Public Property RandomObjects As Boolean = True       ' objects are picked at random (see RandomOneObject)
   Public Property RandomOneObject As Boolean = False    ' with RandomObjects: True = one random type per run, False = all types mixed

   Public Property Colors As New List(Of String) From {"Blue"}                         ' used when RandomColor is off
   Public Property Objects As New List(Of MediaType)(AllMediaTypes())                   ' used when RandomObjects is off

   Public Property Speed As Integer = 5
   Public Const SpeedMin As Integer = 1
   Public Const SpeedMax As Integer = 10

   '-----------------------------------------------------------------------------------------------
   ' Effective values: what the screensaver should actually use
   Public ReadOnly Property UseRandomColor As Boolean
      Get
         Return EverythingRandom OrElse RandomColor OrElse Colors.Count = 0
      End Get
   End Property

   ' one random object type for the whole run (Everything Random always mixes all types)
   Public ReadOnly Property UseOneObject As Boolean
      Get
         Return Not EverythingRandom AndAlso RandomObjects AndAlso RandomOneObject
      End Get
   End Property

   ' all object types mixed
   Public ReadOnly Property UseAllObjects As Boolean
      Get
         Return EverythingRandom OrElse (RandomObjects AndAlso Not RandomOneObject) OrElse Objects.Count = 0
      End Get
   End Property

   Public Shared Function AllMediaTypes() As MediaType()
      Return DirectCast([Enum].GetValues(GetType(MediaType)), MediaType())
   End Function

   '-----------------------------------------------------------------------------------------------
   ' Save
   Public Sub Save()
      Try
         Using key As RegistryKey = Registry.CurrentUser.CreateSubKey(regAppPath)
            If key Is Nothing Then Return

            key.SetValue(NameOf(EverythingRandom), If(EverythingRandom, 1, 0), RegistryValueKind.DWord)
            key.SetValue(NameOf(RandomColor), If(RandomColor, 1, 0), RegistryValueKind.DWord)
            key.SetValue(NameOf(RandomObjects), If(RandomObjects, 1, 0), RegistryValueKind.DWord)
            key.SetValue(NameOf(RandomOneObject), If(RandomOneObject, 1, 0), RegistryValueKind.DWord)
            key.SetValue(NameOf(Colors), Colors.ToArray(), RegistryValueKind.MultiString)
            key.SetValue(NameOf(Objects), Objects.Select(Function(o) o.ToString()).ToArray(), RegistryValueKind.MultiString)
            key.SetValue(NameOf(Speed), Speed, RegistryValueKind.DWord)
         End Using
      Catch ex As Exception
         ' Ignore registry errors (e.g. access denied)
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Load: missing, corrupt or out-of-range values fall back to / are clamped to valid values
   Public Sub Load()
      Try
         Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(regAppPath, False)
            If key Is Nothing Then Return ' Nothing saved yet, keep defaults

            EverythingRandom = ReadBoolean(key, NameOf(EverythingRandom), EverythingRandom)
            RandomColor = ReadBoolean(key, NameOf(RandomColor), RandomColor)
            RandomObjects = ReadBoolean(key, NameOf(RandomObjects), RandomObjects)
            RandomOneObject = ReadBoolean(key, NameOf(RandomOneObject), RandomOneObject)

            ' unknown names (e.g. a color removed from the palette) are skipped
            Dim c As String() = ReadStrings(key, NameOf(Colors))
            If c IsNot Nothing Then
               Colors = c.Where(Function(n) Palette.ContainsKey(n)).Distinct().ToList()
            End If

            Dim o As String() = ReadStrings(key, NameOf(Objects))
            If o IsNot Nothing Then
               Dim list As New List(Of MediaType)
               For Each n As String In o
                  Dim kind As MediaType
                  If [Enum].TryParse(n, kind) AndAlso [Enum].IsDefined(GetType(MediaType), kind) AndAlso Not list.Contains(kind) Then list.Add(kind)
               Next
               Objects = list
            End If

            Speed = IntegerMinMax(key, NameOf(Speed), Speed, SpeedMin, SpeedMax)
         End Using
      Catch ex As Exception
         ' Ignore registry errors, keep defaults
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Delete
   Public Sub Delete()
      Try
         Registry.CurrentUser.DeleteSubKeyTree(regAppPath, False)
      Catch ex As Exception
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' IntegerMinMax: reads one DWord value, keeping the default if it is missing or not a number
   Private Shared Function IntegerMinMax(key As RegistryKey, name As String, def As Integer, min As Integer, max As Integer) As Integer
      Try
         Dim v As Integer = Convert.ToInt32(key.GetValue(name, def))
         Return Math.Min(max, Math.Max(min, v))
      Catch
         Return def
      End Try
   End Function

   '-----------------------------------------------------------------------------------------------
   ' ReadBoolean: reads one DWord value (0 = False, anything else = True)
   Private Shared Function ReadBoolean(key As RegistryKey, name As String, def As Boolean) As Boolean
      Try
         Return Convert.ToInt32(key.GetValue(name, If(def, 1, 0))) <> 0
      Catch
         Return def
      End Try
   End Function

   '-----------------------------------------------------------------------------------------------
   ' ReadStrings: reads one MultiString value, Nothing if it is missing or of another type
   Private Shared Function ReadStrings(key As RegistryKey, name As String) As String()
      Return TryCast(key.GetValue(name), String())
   End Function

End Class
