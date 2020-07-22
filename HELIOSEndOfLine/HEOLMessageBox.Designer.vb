<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HEOLMessageBox
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lbltext = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.pBoxPicture = New System.Windows.Forms.PictureBox()
        Me.tmrPing = New System.Windows.Forms.Timer(Me.components)
        CType(Me.pBoxPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(304, 365)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(68, 28)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Abbrechen"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lbltext
        '
        Me.lbltext.AutoSize = True
        Me.lbltext.Location = New System.Drawing.Point(23, 21)
        Me.lbltext.Name = "lbltext"
        Me.lbltext.Size = New System.Drawing.Size(24, 13)
        Me.lbltext.TabIndex = 1
        Me.lbltext.Text = "text"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(157, 347)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(141, 46)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'pBoxPicture
        '
        Me.pBoxPicture.Location = New System.Drawing.Point(26, 63)
        Me.pBoxPicture.Name = "pBoxPicture"
        Me.pBoxPicture.Size = New System.Drawing.Size(527, 263)
        Me.pBoxPicture.TabIndex = 2
        Me.pBoxPicture.TabStop = False
        '
        'tmrPing
        '
        Me.tmrPing.Interval = 1000
        '
        'HEOLMessageBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(579, 405)
        Me.ControlBox = False
        Me.Controls.Add(Me.pBoxPicture)
        Me.Controls.Add(Me.lbltext)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "HEOLMessageBox"
        Me.ShowIcon = False
        Me.Text = "caption"
        Me.TopMost = True
        CType(Me.pBoxPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lbltext As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents pBoxPicture As System.Windows.Forms.PictureBox
    Friend WithEvents tmrPing As System.Windows.Forms.Timer
End Class
