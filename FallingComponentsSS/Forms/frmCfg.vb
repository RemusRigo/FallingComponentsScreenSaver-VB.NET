'--------------------------------------------------------------------------------------------------
' FallingComponentsSS: frmCfg.vb
'    © 2026 Remus Rigo
'       v1.0.20261125
'--------------------------------------------------------------------------------------------------

Imports FallingComponentsSS.API

Public Class frmCfg

   Private Const SYSMENU_ABOUT_ID As Integer = &H3F0   ' low 4 bits must be 0

   '-----------------------------------------------------------------------------------------------
   ' create menu
   Protected Overrides Sub OnHandleCreated(e As EventArgs)
      MyBase.OnHandleCreated(e)
      Dim hSysMenu As IntPtr = GetSystemMenu(Me.Handle, False)
      ' Add a separator and then your custom item
      AppendMenu(hSysMenu, MF_SEPARATOR, 0, String.Empty)
      AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUT_ID, "About...")
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' append menu to app system menu
   Protected Overrides Sub WndProc(ByRef m As Message)
      MyBase.WndProc(m)
      ' Windows uses the low 4 bits of wParam internally, so mask them off before comparing
      If m.Msg = WM_SYSCOMMAND AndAlso (m.WParam.ToInt64() And &HFFF0L) = SYSMENU_ABOUT_ID Then
         Using f As New frmAbout()
            f.ShowDialog(Me)
         End Using
      End If
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmCfg: constructor
   Public Sub New()
      InitializeComponent()

      tbSpeed.Minimum = AppSettings.SpeedMin
      tbSpeed.Maximum = AppSettings.SpeedMax
      tbSpeed.TickFrequency = 1
      tbSpeed.LargeChange = 1
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Save settings
   Private Sub SaveSettings()
      cfg.EverythingRandom = chkBoxEverytingRandom.Checked
      cfg.RandomColor = chkBoxRandomColor.Checked
      cfg.RandomObjects = chkBoxRandomObjects.Checked
      cfg.RandomOneObject = rBtnOneRandomObject.Checked
      cfg.Colors = CheckedColors()
      cfg.Objects = CheckedObjects()
      cfg.Speed = tbSpeed.Value
      cfg.Save()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' Load settings
   Private Sub LoadSettings()
      cfg.Load()
      chkBoxEverytingRandom.Checked = cfg.EverythingRandom
      chkBoxRandomColor.Checked = cfg.RandomColor
      chkBoxRandomObjects.Checked = cfg.RandomObjects
      rBtnOneRandomObject.Checked = cfg.RandomOneObject
      rBtnAllRandomObjects.Checked = Not cfg.RandomOneObject

      For Each chk As CheckBox In grpBoxColors.Controls.OfType(Of CheckBox)()
         chk.Checked = cfg.Colors.Contains(CStr(chk.Tag))
      Next
      For Each chk As CheckBox In grpBoxObjects.Controls.OfType(Of CheckBox)()
         chk.Checked = cfg.Objects.Contains(DirectCast(chk.Tag, MediaType))
      Next

      tbSpeed.Value = Clamp(cfg.Speed, tbSpeed.Minimum, tbSpeed.Maximum)
      UpdateSpeedLabel()
      UpdateEnabledState()
   End Sub

   Private Function CheckedColors() As List(Of String)
      Return grpBoxColors.Controls.OfType(Of CheckBox)().Where(Function(c) c.Checked).Select(Function(c) CStr(c.Tag)).ToList()
   End Function

   Private Function CheckedObjects() As List(Of MediaType)
      Return grpBoxObjects.Controls.OfType(Of CheckBox)().Where(Function(c) c.Checked).Select(Function(c) DirectCast(c.Tag, MediaType)).ToList()
   End Function

   '-----------------------------------------------------------------------------------------------
   ' frmCfg: Load
   Private Sub frmCfg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      Me.Text = appTitle & " Settings"

      ' one checkbox for every color in the palette
      Dim top As Integer = 20
      For Each entry As KeyValuePair(Of String, Color) In AppSettings.Palette
         Dim chk As New CheckBox()
         chk.Name = "chk" & entry.Key
         chk.Text = entry.Key
         chk.Tag = entry.Key                  ' the palette name this checkbox stands for
         chk.ForeColor = entry.Value
         chk.AutoSize = True
         chk.Location = New Point(5, top)
         grpBoxColors.Controls.Add(chk)
         top += 20   ' spacing between checkboxes
      Next

      ' one checkbox for every MediaType, so new types show up here automatically
      top = 20
      For Each kind As MediaType In AppSettings.AllMediaTypes()
         Dim chk As New CheckBox()
         chk.Name = "chk" & kind.ToString()
         chk.Text = SplitWords(kind.ToString())
         chk.Tag = kind                       ' the MediaType this checkbox stands for
         chk.AutoSize = True
         chk.Location = New Point(5, top)
         grpBoxObjects.Controls.Add(chk)
         top += 20   ' spacing between checkboxes
      Next

      LoadSettings()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' "Random" checkboxes: disable whatever they override
   Private Sub RandomCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles chkBoxEverytingRandom.CheckedChanged, chkBoxRandomColor.CheckedChanged, chkBoxRandomObjects.CheckedChanged
      UpdateEnabledState
   End Sub

   Private Sub UpdateEnabledState()
      Dim all As Boolean = chkBoxEverytingRandom.Checked
      chkBoxRandomColor.Enabled = Not all
      chkBoxRandomObjects.Enabled = Not all
      grpBoxColors.Enabled = Not all AndAlso Not chkBoxRandomColor.Checked
      grpBoxObjects.Enabled = Not all AndAlso Not chkBoxRandomObjects.Checked
      rBtnOneRandomObject.Enabled = Not all AndAlso chkBoxRandomObjects.Checked
      rBtnAllRandomObjects.Enabled = Not all AndAlso chkBoxRandomObjects.Checked
      tbSpeed.Enabled = Not all
      lblSpeed.Enabled = Not all
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' tbSpeed: Scroll
   Private Sub tbSpeed_Scroll(sender As Object, e As EventArgs) Handles tbSpeed.Scroll
      UpdateSpeedLabel()
   End Sub

   Private Sub UpdateSpeedLabel()
      lblSpeed.Text = "Speed: " & tbSpeed.Value.ToString()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' SplitWords: "CassetteTape" -> "Cassette Tape", "HardDisk" -> "Hard Disk" (all-caps names like "CDROM" stay as they are)
   Private Shared Function SplitWords(name As String) As String
      Dim sb As New System.Text.StringBuilder()
      For i As Integer = 0 To name.Length - 1
         If i > 0 AndAlso Char.IsUpper(name(i)) AndAlso Char.IsLower(name(i - 1)) Then sb.Append(" "c)
         sb.Append(name(i))
      Next
      Return sb.ToString()
   End Function

   '-----------------------------------------------------------------------------------------------
   ' frmCfg: FormClosing
   ' only save if user clicked OK, X or Esc closes the form without saving
   Private Sub frmCfg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
      If Me.DialogResult <> DialogResult.OK Then Return

      ' at least one color / object must be checked when they are not random
      Dim all As Boolean = chkBoxEverytingRandom.Checked
      Dim msg As String = Nothing
      If Not all AndAlso Not chkBoxRandomColor.Checked AndAlso CheckedColors().Count = 0 Then
         msg = "Please check at least one color or check ""Random Color""."
      ElseIf Not all AndAlso Not chkBoxRandomObjects.Checked AndAlso CheckedObjects().Count = 0 Then
         msg = "Please check at least one object or check ""Random Objects""."
      End If
      If msg IsNot Nothing Then
         MessageBox.Show(Me, msg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
         e.Cancel = True
         Me.DialogResult = DialogResult.None
         Return
      End If

      SaveSettings()
   End Sub

End Class
