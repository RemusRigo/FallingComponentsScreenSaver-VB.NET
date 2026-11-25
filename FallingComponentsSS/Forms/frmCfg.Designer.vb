<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCfg
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()>
   Private Sub InitializeComponent()
      btnOk = New Button()
      btnCancel = New Button()
      chkBoxEverytingRandom = New CheckBox()
      tbSpeed = New TrackBar()
      lblSpeed = New Label()
      chkBoxRandomColor = New CheckBox()
      grpBoxColors = New GroupBox()
      grpBoxObjects = New GroupBox()
      chkBoxRandomObjects = New CheckBox()
      rBtnOneRandomObject = New RadioButton()
      rBtnAllRandomObjects = New RadioButton()
      CType(tbSpeed, ComponentModel.ISupportInitialize).BeginInit()
      SuspendLayout()
      ' 
      ' btnOk
      ' 
      btnOk.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
      btnOk.DialogResult = DialogResult.OK
      btnOk.Location = New Point(631, 335)
      btnOk.Name = "btnOk"
      btnOk.Size = New Size(35, 23)
      btnOk.TabIndex = 9
      btnOk.Text = "&Ok"
      btnOk.UseVisualStyleBackColor = True
      ' 
      ' btnCancel
      ' 
      btnCancel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
      btnCancel.DialogResult = DialogResult.Cancel
      btnCancel.Location = New Point(672, 335)
      btnCancel.Name = "btnCancel"
      btnCancel.Size = New Size(60, 23)
      btnCancel.TabIndex = 10
      btnCancel.Text = "&Cancel"
      btnCancel.UseVisualStyleBackColor = True
      ' 
      ' chkBoxEverytingRandom
      ' 
      chkBoxEverytingRandom.AutoSize = True
      chkBoxEverytingRandom.Location = New Point(12, 12)
      chkBoxEverytingRandom.Name = "chkBoxEverytingRandom"
      chkBoxEverytingRandom.Size = New Size(130, 19)
      chkBoxEverytingRandom.TabIndex = 11
      chkBoxEverytingRandom.Text = "Everything Random"
      chkBoxEverytingRandom.UseVisualStyleBackColor = True
      ' 
      ' tbSpeed
      ' 
      tbSpeed.Location = New Point(158, 277)
      tbSpeed.Name = "tbSpeed"
      tbSpeed.Size = New Size(557, 45)
      tbSpeed.TabIndex = 13
      ' 
      ' lblSpeed
      ' 
      lblSpeed.AutoSize = True
      lblSpeed.Location = New Point(6, 277)
      lblSpeed.Name = "lblSpeed"
      lblSpeed.Size = New Size(39, 15)
      lblSpeed.TabIndex = 14
      lblSpeed.Text = "Speed"
      ' 
      ' chkBoxRandomColor
      ' 
      chkBoxRandomColor.AutoSize = True
      chkBoxRandomColor.Location = New Point(12, 37)
      chkBoxRandomColor.Name = "chkBoxRandomColor"
      chkBoxRandomColor.Size = New Size(103, 19)
      chkBoxRandomColor.TabIndex = 16
      chkBoxRandomColor.Text = "Random Color"
      chkBoxRandomColor.UseVisualStyleBackColor = True
      ' 
      ' grpBoxColors
      ' 
      grpBoxColors.Location = New Point(12, 62)
      grpBoxColors.Name = "grpBoxColors"
      grpBoxColors.Size = New Size(218, 201)
      grpBoxColors.TabIndex = 20
      grpBoxColors.TabStop = False
      grpBoxColors.Text = "Colors"
      ' 
      ' grpBoxObjects
      ' 
      grpBoxObjects.Location = New Point(261, 62)
      grpBoxObjects.Name = "grpBoxObjects"
      grpBoxObjects.Size = New Size(471, 201)
      grpBoxObjects.TabIndex = 21
      grpBoxObjects.TabStop = False
      grpBoxObjects.Text = "Objects"
      ' 
      ' chkBoxRandomObjects
      ' 
      chkBoxRandomObjects.AutoSize = True
      chkBoxRandomObjects.Location = New Point(261, 37)
      chkBoxRandomObjects.Name = "chkBoxRandomObjects"
      chkBoxRandomObjects.Size = New Size(114, 19)
      chkBoxRandomObjects.TabIndex = 19
      chkBoxRandomObjects.Text = "Random Objects"
      chkBoxRandomObjects.UseVisualStyleBackColor = True
      ' 
      ' rBtnOneRandomObject
      ' 
      rBtnOneRandomObject.AutoSize = True
      rBtnOneRandomObject.Location = New Point(381, 36)
      rBtnOneRandomObject.Name = "rBtnOneRandomObject"
      rBtnOneRandomObject.Size = New Size(47, 19)
      rBtnOneRandomObject.TabIndex = 22
      rBtnOneRandomObject.TabStop = True
      rBtnOneRandomObject.Text = "One"
      rBtnOneRandomObject.UseVisualStyleBackColor = True
      ' 
      ' rBtnAllRandomObjects
      ' 
      rBtnAllRandomObjects.AutoSize = True
      rBtnAllRandomObjects.Location = New Point(434, 37)
      rBtnAllRandomObjects.Name = "rBtnAllRandomObjects"
      rBtnAllRandomObjects.Size = New Size(39, 19)
      rBtnAllRandomObjects.TabIndex = 23
      rBtnAllRandomObjects.TabStop = True
      rBtnAllRandomObjects.Text = "All"
      rBtnAllRandomObjects.UseVisualStyleBackColor = True
      ' 
      ' frmCfg
      ' 
      AcceptButton = btnOk
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      CancelButton = btnCancel
      ClientSize = New Size(734, 361)
      Controls.Add(rBtnAllRandomObjects)
      Controls.Add(rBtnOneRandomObject)
      Controls.Add(grpBoxObjects)
      Controls.Add(grpBoxColors)
      Controls.Add(chkBoxRandomObjects)
      Controls.Add(chkBoxRandomColor)
      Controls.Add(lblSpeed)
      Controls.Add(tbSpeed)
      Controls.Add(chkBoxEverytingRandom)
      Controls.Add(btnCancel)
      Controls.Add(btnOk)
      FormBorderStyle = FormBorderStyle.FixedDialog
      MaximizeBox = False
      MinimizeBox = False
      Name = "frmCfg"
      Text = "Settings"
      CType(tbSpeed, ComponentModel.ISupportInitialize).EndInit()
      ResumeLayout(False)
      PerformLayout()
   End Sub
   Friend WithEvents btnOk As Button
   Friend WithEvents btnCancel As Button
   Friend WithEvents chkBoxEverytingRandom As CheckBox
   Friend WithEvents tbSpeed As TrackBar
   Friend WithEvents lblSpeed As Label
   Friend WithEvents chkBoxRandomColor As CheckBox
   Friend WithEvents grpBoxColors As GroupBox
   Friend WithEvents grpBoxObjects As GroupBox
   Friend WithEvents chkBoxRandomObjects As CheckBox
   Friend WithEvents rBtnOneRandomObject As RadioButton
   Friend WithEvents rBtnAllRandomObjects As RadioButton
End Class
