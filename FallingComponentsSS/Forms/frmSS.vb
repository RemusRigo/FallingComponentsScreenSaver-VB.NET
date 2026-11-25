'--------------------------------------------------------------------------------------------------
' FallingComponentsSS: frmSS.vb
'    © 2026 Remus Rigo
'       v1.0.20261122
'--------------------------------------------------------------------------------------------------

Imports FallingComponentsSS.API

Public Class frmSS

#Region "Data"

   Private items As New List(Of MediaItem)
   Private fragments As New List(Of Fragment)

   Private Const MAX_ITEMS As Integer = 30
   Private Const SPAWN_CHANCE As Double = 0.02    ' chance per tick (~1.8 new items per second at 60 fps)

   Private ReadOnly rnd As New Random()

   ' "Random Objects: One": the single object type used for this run, the same on every monitor
   Private Shared ReadOnly oneKind As MediaType = PickOneKind()

   Private Shared Function PickOneKind() As MediaType
      Dim kinds As MediaType() = AppSettings.AllMediaTypes()
      Return kinds(New Random().Next(kinds.Length))
   End Function

   Private mouseStart As Point? = Nothing         ' Nothing until the first MouseMove

   Private ReadOnly _screenBounds As Rectangle    ' the monitor this form must cover (preview: the parent's client area)
   Private ReadOnly _previewHandle As IntPtr = IntPtr.Zero

   Private ReadOnly Property IsPreview As Boolean
      Get
         Return _previewHandle <> IntPtr.Zero
      End Get
   End Property

   '-----------------------------------------------------------------------------------------------
   ' CreateParams: in preview mode the window is created directly as a child of the preview monitor
   Protected Overrides ReadOnly Property CreateParams As CreateParams
      Get
         Dim cp As CreateParams = MyBase.CreateParams
         If IsPreview Then
            cp.Parent = _previewHandle
            cp.Style = (cp.Style And Not WS_POPUP) Or WS_CHILD
            cp.X = 0
            cp.Y = 0
            cp.Width = _screenBounds.Width
            cp.Height = _screenBounds.Height
         End If
         Return cp
      End Get
   End Property

#End Region

#Region "Constructor"

   Public Sub New()
      Me.New(Screen.PrimaryScreen.Bounds)
   End Sub

   Public Sub New(bounds As Rectangle)
      InitializeComponent()
      _screenBounds = bounds
      ' border style and scaling must be set BEFORE the bounds: changing FormBorderStyle later keeps the
      ' ClientSize and shrinks the window, leaving an uncovered strip at the bottom/right of the screen
      Me.AutoScaleMode = AutoScaleMode.None
      Me.FormBorderStyle = FormBorderStyle.None
      Me.StartPosition = FormStartPosition.Manual
      Me.WindowState = FormWindowState.Normal
      Me.ShowInTaskbar = False                     ' set before the handle exists: changing it later recreates the window
      Me.Bounds = bounds
   End Sub

   Public Sub InitData()
      items.Clear()
      fragments.Clear()
   End Sub

   Private Sub ResetItem(item As MediaItem)
      item.Size = rnd.Next(40, 90)
      item.X = rnd.Next(0, Me.ClientSize.Width)
      item.Speed = CSng(rnd.NextDouble() * 4 + 1) * SpeedFactor()  ' 1 to 5 pixels per tick at the default speed
      item.RotationSpeed = CSng(rnd.NextDouble() * 4 - 2)       ' -2 to +2 degrees per tick
      item.Rotation = rnd.Next(0, 360)
      item.Y = -item.Size                                    ' just above the top
      item.Kind = RandomKind()
      item.Tint = RandomTint()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' SpeedFactor: settings speed 1..10 -> 0.2x..2x (5 = 1x); Everything Random picks a new one per item
   Private Function SpeedFactor() As Single
      Dim s As Double = If(cfg.EverythingRandom,
                           AppSettings.SpeedMin + rnd.NextDouble() * (AppSettings.SpeedMax - AppSettings.SpeedMin),
                           cfg.Speed)
      Return CSng(s / 5)
   End Function

   '-----------------------------------------------------------------------------------------------
   ' RandomKind: the one random type for this run, any MediaType, or one of the checked objects
   Private Function RandomKind() As MediaType
      If cfg.UseOneObject Then Return oneKind
      Dim kinds As IList(Of MediaType) = If(cfg.UseAllObjects, AppSettings.AllMediaTypes(), cfg.Objects)
      Return kinds(rnd.Next(kinds.Count))
   End Function

   '-----------------------------------------------------------------------------------------------
   ' RandomTint: a random bright color, or one of the checked palette colors
   Private Function RandomTint() As Color
      If cfg.UseRandomColor Then
         Return ColorFromHsv(rnd.NextDouble() * 360, 0.55 + rnd.NextDouble() * 0.35, 0.7 + rnd.NextDouble() * 0.25)
      End If
      Return AppSettings.Palette(cfg.Colors(rnd.Next(cfg.Colors.Count)))
   End Function

#End Region

#Region "Preview"
   '-----------------------------------------------------------------------------------------------
   ' Preview constructor: draw inside the small monitor of the Screen Saver Settings dialog
   Public Sub New(previewHandle As IntPtr)
      Me.New(PreviewRect(previewHandle))
      _previewHandle = previewHandle
   End Sub

   Private Shared Function PreviewRect(hWnd As IntPtr) As Rectangle
      Dim r As RECT
      GetClientRect(hWnd, r)
      Return New Rectangle(0, 0, r.Right - r.Left, r.Bottom - r.Top)
   End Function

#End Region

#Region "Form lifecycle"

   '-----------------------------------------------------------------------------------------------
   ' frmSS: Load
   Private Sub frmSS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      With Me
         .BackColor = Color.Black
         .DoubleBuffered = True
         .KeyPreview = True
      End With

      If IsPreview Then
         ' WinForms may park the child window under its own hidden window: attach it to the preview monitor
         ' (no TopMost and no hidden cursor in preview mode)
         SetParent(Me.Handle, _previewHandle)
         Me.Bounds = PreviewRect(_previewHandle)
         InitData()
         Return
      End If

#If Not DEBUG Then
      Me.TopMost = True
#End If
      Me.Bounds = _screenBounds                  ' re-apply in case anything resized the window
      Cursor.Hide()
      InitData()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSS: Shown
   Private Sub frmSS_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
      tmr.Interval = 16
      tmr.Enabled = True
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSS: FormClosed
   Private Sub frmSS_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
      tmr.Enabled = False
      Cursor.Show()
      Application.Exit()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSS: DpiChanged
   ' One form per monitor. With mixed scaling (e.g. 100% + 150%) WinForms would rescale/move the window
   ' and leave gaps. Cancel that and keep covering exactly this form's monitor.
   Private Sub frmSS_DpiChanged(sender As Object, e As DpiChangedEventArgs) Handles MyBase.DpiChanged
      If IsPreview Then Return
      e.Cancel = True
      Me.Bounds = _screenBounds
   End Sub

#End Region

#Region "Animation"

   '-----------------------------------------------------------------------------------------------
   ' tmr: Tick
   ' update the points and request a repaint
   Private Sub tmr_Tick(sender As Object, e As EventArgs) Handles tmr.Tick
      ' preview: exit when the Screen Saver Settings dialog closes or switches to another screensaver
      If IsPreview AndAlso Not IsWindow(_previewHandle) Then
         Me.Close()
         Return
      End If

      ' 1. Maybe spawn a new item (only if below the maximum)
      If items.Count < MAX_ITEMS AndAlso rnd.NextDouble() < SPAWN_CHANCE Then
         Dim newItem As New MediaItem(MediaType.FDD, 0, 0, 0, 0, 0)
         ResetItem(newItem)
         items.Add(newItem)
      End If

      ' 2. Update items; when one hits the bottom it breaks into 4 fragments
      For i As Integer = items.Count - 1 To 0 Step -1
         items(i).Update()
         If items(i).HitsBottom(Me.ClientSize.Height) Then
            For quarter As Integer = 0 To 3
               fragments.Add(New Fragment(items(i), quarter, rnd))   ' create 4 pieces from this item
            Next
            items.RemoveAt(i)   ' delete the original item
         End If
      Next

      ' 3. Update fragments; remove them when they leave the screen
      For i As Integer = fragments.Count - 1 To 0 Step -1
         fragments(i).Update()
         If fragments(i).IsOffScreen(Me.ClientSize.Width, Me.ClientSize.Height) Then
            fragments.RemoveAt(i)
         End If
      Next

      Me.Invalidate()                                      'request a repaint of the form
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSS: Paint
   Private Sub frmSS_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
      If items.Count = 0 Then Return      ' Safeguard
      e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

      For Each item As MediaItem In items
         MediaRenderer.Draw(e.Graphics, item, item.Tint)
      Next

      For Each f As Fragment In fragments
         f.Draw(e.Graphics)
      Next
   End Sub

#End Region

#Region "Keyboard/Mouse Movement"

   '-----------------------------------------------------------------------------------------------
   ' frmSS: KeyDown
   Private Sub frmSS_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
      If IsPreview Then Return
      Me.Close()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSS: MouseDown
   Private Sub frmSS_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
      If IsPreview Then Return
      Me.Close()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmSS: MouseMove
   Private Sub frmSS_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
      If IsPreview Then Return

      ' remember the first position
      If Not mouseStart.HasValue Then
         mouseStart = e.Location
         Return
      End If

      ' exit if the mouse moved more than 4 pixels
      Dim start As Point = mouseStart.Value
      If Math.Abs(e.X - start.X) > 4 OrElse Math.Abs(e.Y - start.Y) > 4 Then
         Me.Close()
      End If
   End Sub

#End Region

End Class
