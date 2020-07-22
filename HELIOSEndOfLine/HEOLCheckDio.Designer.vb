<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HEOLCheckDio
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
        Me.timerTest = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(81, 45)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(116, 28)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Abbrechen"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lbltext
        '
        Me.lbltext.AutoSize = True
        Me.lbltext.Location = New System.Drawing.Point(12, 9)
        Me.lbltext.Name = "lbltext"
        Me.lbltext.Size = New System.Drawing.Size(24, 13)
        Me.lbltext.TabIndex = 1
        Me.lbltext.Text = "text"
        '
        'timerTest
        '
        Me.timerTest.Interval = 20
        '
        'HEOLCheckDio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 85)
        Me.ControlBox = False
        Me.Controls.Add(Me.lbltext)
        Me.Controls.Add(Me.btnCancel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "HEOLCheckDio"
        Me.ShowIcon = False
        Me.Text = "caption"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lbltext As System.Windows.Forms.Label
    Friend WithEvents timerTest As System.Windows.Forms.Timer
End Class
