<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HELIOSEndOfLine
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.tbStatus = New System.Windows.Forms.TextBox()
        Me.btRestartHelios = New System.Windows.Forms.Button()
        Me.btnStopMeasurement = New System.Windows.Forms.Button()
        Me.btnStartMeasurement = New System.Windows.Forms.Button()
        Me.btnProperties = New System.Windows.Forms.Button()
        Me.lblCasStatus = New System.Windows.Forms.Label()
        Me.lblPSStatus = New System.Windows.Forms.Label()
        Me.cBxModuleType = New System.Windows.Forms.ComboBox()
        Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        Me.LineShape3 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.LineShape2 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.LineShape1 = New Microsoft.VisualBasic.PowerPacks.LineShape()
        Me.lblModuleType = New System.Windows.Forms.Label()
        Me.lblModuleConfig = New System.Windows.Forms.Label()
        Me.tbxSerialNumber = New System.Windows.Forms.TextBox()
        Me.dgvSerialNumberRGB = New System.Windows.Forms.DataGridView()
        Me.Board = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MaterialNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.serialNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.binning = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblSerialNumber = New System.Windows.Forms.Label()
        Me.lblStatusInDB = New System.Windows.Forms.Label()
        Me.btnMeasureAgain = New System.Windows.Forms.Button()
        Me.btnBackToSerNr = New System.Windows.Forms.Button()
        Me.lblDIOStatus = New System.Windows.Forms.Label()
        Me.rtbOverview = New System.Windows.Forms.RichTextBox()
        Me.pBarCalib = New System.Windows.Forms.ProgressBar()
        Me.tbxModuleConfig = New System.Windows.Forms.TextBox()
        Me.btnDoRefMeas = New System.Windows.Forms.Button()
        Me.panRed = New System.Windows.Forms.Panel()
        Me.panYellow = New System.Windows.Forms.Panel()
        Me.panGreen = New System.Windows.Forms.Panel()
        Me.lblBoxStatus = New System.Windows.Forms.Label()
        Me.tbxVSerialNumber = New System.Windows.Forms.TextBox()
        Me.lblVSerialNumber = New System.Windows.Forms.Label()
        Me.lblPanRed = New System.Windows.Forms.Label()
        Me.lblPanYellow = New System.Windows.Forms.Label()
        Me.lblPanGreen = New System.Windows.Forms.Label()
        CType(Me.dgvSerialNumberRGB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbStatus
        '
        Me.tbStatus.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.tbStatus.Location = New System.Drawing.Point(32, 376)
        Me.tbStatus.Name = "tbStatus"
        Me.tbStatus.ReadOnly = True
        Me.tbStatus.Size = New System.Drawing.Size(377, 20)
        Me.tbStatus.TabIndex = 2
        Me.tbStatus.Visible = False
        '
        'btRestartHelios
        '
        Me.btRestartHelios.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btRestartHelios.Location = New System.Drawing.Point(810, 488)
        Me.btRestartHelios.Name = "btRestartHelios"
        Me.btRestartHelios.Size = New System.Drawing.Size(214, 46)
        Me.btRestartHelios.TabIndex = 77
        Me.btRestartHelios.Text = "Restart Helios"
        Me.btRestartHelios.UseVisualStyleBackColor = True
        Me.btRestartHelios.Visible = False
        '
        'btnStopMeasurement
        '
        Me.btnStopMeasurement.Enabled = False
        Me.btnStopMeasurement.Location = New System.Drawing.Point(226, 331)
        Me.btnStopMeasurement.Name = "btnStopMeasurement"
        Me.btnStopMeasurement.Size = New System.Drawing.Size(183, 31)
        Me.btnStopMeasurement.TabIndex = 78
        Me.btnStopMeasurement.Text = "Kalibrierung Abbrechen"
        Me.btnStopMeasurement.UseVisualStyleBackColor = True
        Me.btnStopMeasurement.Visible = False
        '
        'btnStartMeasurement
        '
        Me.btnStartMeasurement.Location = New System.Drawing.Point(32, 331)
        Me.btnStartMeasurement.Name = "btnStartMeasurement"
        Me.btnStartMeasurement.Size = New System.Drawing.Size(188, 31)
        Me.btnStartMeasurement.TabIndex = 79
        Me.btnStartMeasurement.Text = "Starte Kalibrierung und Messung"
        Me.btnStartMeasurement.UseVisualStyleBackColor = True
        Me.btnStartMeasurement.Visible = False
        '
        'btnProperties
        '
        Me.btnProperties.Location = New System.Drawing.Point(884, 20)
        Me.btnProperties.Name = "btnProperties"
        Me.btnProperties.Size = New System.Drawing.Size(126, 30)
        Me.btnProperties.TabIndex = 81
        Me.btnProperties.Text = "Einstellungen"
        Me.btnProperties.UseVisualStyleBackColor = True
        '
        'lblCasStatus
        '
        Me.lblCasStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCasStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCasStatus.Location = New System.Drawing.Point(778, 14)
        Me.lblCasStatus.Name = "lblCasStatus"
        Me.lblCasStatus.Size = New System.Drawing.Size(100, 20)
        Me.lblCasStatus.TabIndex = 82
        Me.lblCasStatus.Text = "CAS Status"
        '
        'lblPSStatus
        '
        Me.lblPSStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPSStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPSStatus.Location = New System.Drawing.Point(672, 14)
        Me.lblPSStatus.Name = "lblPSStatus"
        Me.lblPSStatus.Size = New System.Drawing.Size(100, 20)
        Me.lblPSStatus.TabIndex = 82
        Me.lblPSStatus.Text = "PS Status"
        '
        'cBxModuleType
        '
        Me.cBxModuleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cBxModuleType.FormattingEnabled = True
        Me.cBxModuleType.Items.AddRange(New Object() {"Kalibrierung RGB-Modul", "Kalibrierung V-Modul", "Messung RGB (ohne Kalibrierung)", "Messung V (ohne Kalibrierung)", "Finalmessung RGB"})
        Me.cBxModuleType.Location = New System.Drawing.Point(12, 36)
        Me.cBxModuleType.Name = "cBxModuleType"
        Me.cBxModuleType.Size = New System.Drawing.Size(145, 21)
        Me.cBxModuleType.TabIndex = 83
        '
        'ShapeContainer1
        '
        Me.ShapeContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.ShapeContainer1.Name = "ShapeContainer1"
        Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.LineShape3, Me.LineShape2, Me.LineShape1})
        Me.ShapeContainer1.Size = New System.Drawing.Size(1103, 658)
        Me.ShapeContainer1.TabIndex = 84
        Me.ShapeContainer1.TabStop = False
        '
        'LineShape3
        '
        Me.LineShape3.Name = "LineShape3"
        Me.LineShape3.X1 = 635
        Me.LineShape3.X2 = 635
        Me.LineShape3.Y1 = 80
        Me.LineShape3.Y2 = 3
        '
        'LineShape2
        '
        Me.LineShape2.Cursor = System.Windows.Forms.Cursors.Default
        Me.LineShape2.Name = "LineShape1"
        Me.LineShape2.X1 = 22
        Me.LineShape2.X2 = 1036
        Me.LineShape2.Y1 = 308
        Me.LineShape2.Y2 = 308
        '
        'LineShape1
        '
        Me.LineShape1.Name = "LineShape1"
        Me.LineShape1.X1 = 19
        Me.LineShape1.X2 = 1033
        Me.LineShape1.Y1 = 81
        Me.LineShape1.Y2 = 81
        '
        'lblModuleType
        '
        Me.lblModuleType.AutoSize = True
        Me.lblModuleType.Location = New System.Drawing.Point(50, 20)
        Me.lblModuleType.Name = "lblModuleType"
        Me.lblModuleType.Size = New System.Drawing.Size(57, 13)
        Me.lblModuleType.TabIndex = 85
        Me.lblModuleType.Text = "Modul Typ"
        '
        'lblModuleConfig
        '
        Me.lblModuleConfig.AutoSize = True
        Me.lblModuleConfig.Location = New System.Drawing.Point(163, 20)
        Me.lblModuleConfig.Name = "lblModuleConfig"
        Me.lblModuleConfig.Size = New System.Drawing.Size(69, 13)
        Me.lblModuleConfig.TabIndex = 85
        Me.lblModuleConfig.Text = "Konfiguration"
        '
        'tbxSerialNumber
        '
        Me.tbxSerialNumber.Location = New System.Drawing.Point(12, 109)
        Me.tbxSerialNumber.Name = "tbxSerialNumber"
        Me.tbxSerialNumber.Size = New System.Drawing.Size(166, 20)
        Me.tbxSerialNumber.TabIndex = 1
        Me.tbxSerialNumber.Visible = False
        '
        'dgvSerialNumberRGB
        '
        Me.dgvSerialNumberRGB.AllowUserToAddRows = False
        Me.dgvSerialNumberRGB.AllowUserToDeleteRows = False
        Me.dgvSerialNumberRGB.AllowUserToResizeColumns = False
        Me.dgvSerialNumberRGB.AllowUserToResizeRows = False
        Me.dgvSerialNumberRGB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSerialNumberRGB.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Board, Me.MaterialNumber, Me.serialNumber, Me.binning})
        Me.dgvSerialNumberRGB.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.dgvSerialNumberRGB.Location = New System.Drawing.Point(184, 94)
        Me.dgvSerialNumberRGB.Name = "dgvSerialNumberRGB"
        Me.dgvSerialNumberRGB.RowHeadersVisible = False
        Me.dgvSerialNumberRGB.Size = New System.Drawing.Size(424, 193)
        Me.dgvSerialNumberRGB.TabIndex = 87
        Me.dgvSerialNumberRGB.Visible = False
        '
        'Board
        '
        Me.Board.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Board.DefaultCellStyle = DataGridViewCellStyle1
        Me.Board.HeaderText = "Leiterplatte"
        Me.Board.Name = "Board"
        Me.Board.ReadOnly = True
        Me.Board.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Board.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Board.Width = 120
        '
        'MaterialNumber
        '
        Me.MaterialNumber.HeaderText = "Materialnummer"
        Me.MaterialNumber.Name = "MaterialNumber"
        Me.MaterialNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'serialNumber
        '
        Me.serialNumber.HeaderText = "Seriennummer"
        Me.serialNumber.Name = "serialNumber"
        Me.serialNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'binning
        '
        Me.binning.HeaderText = "Binning"
        Me.binning.Name = "binning"
        Me.binning.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'lblSerialNumber
        '
        Me.lblSerialNumber.AutoSize = True
        Me.lblSerialNumber.Location = New System.Drawing.Point(12, 93)
        Me.lblSerialNumber.Name = "lblSerialNumber"
        Me.lblSerialNumber.Size = New System.Drawing.Size(132, 13)
        Me.lblSerialNumber.TabIndex = 88
        Me.lblSerialNumber.Text = "RGB-Modul Seriennummer"
        Me.lblSerialNumber.Visible = False
        '
        'lblStatusInDB
        '
        Me.lblStatusInDB.AutoSize = True
        Me.lblStatusInDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusInDB.Location = New System.Drawing.Point(636, 186)
        Me.lblStatusInDB.Name = "lblStatusInDB"
        Me.lblStatusInDB.Size = New System.Drawing.Size(374, 25)
        Me.lblStatusInDB.TabIndex = 89
        Me.lblStatusInDB.Text = "Modul existiert nicht in der Datenbank"
        Me.lblStatusInDB.Visible = False
        '
        'btnMeasureAgain
        '
        Me.btnMeasureAgain.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMeasureAgain.Location = New System.Drawing.Point(832, 236)
        Me.btnMeasureAgain.Name = "btnMeasureAgain"
        Me.btnMeasureAgain.Size = New System.Drawing.Size(192, 51)
        Me.btnMeasureAgain.TabIndex = 90
        Me.btnMeasureAgain.Text = "Erneut vermessen"
        Me.btnMeasureAgain.UseVisualStyleBackColor = True
        Me.btnMeasureAgain.Visible = False
        '
        'btnBackToSerNr
        '
        Me.btnBackToSerNr.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBackToSerNr.Location = New System.Drawing.Point(453, 331)
        Me.btnBackToSerNr.Name = "btnBackToSerNr"
        Me.btnBackToSerNr.Size = New System.Drawing.Size(119, 31)
        Me.btnBackToSerNr.TabIndex = 91
        Me.btnBackToSerNr.Text = "↑ zurück"
        Me.btnBackToSerNr.UseVisualStyleBackColor = True
        Me.btnBackToSerNr.Visible = False
        '
        'lblDIOStatus
        '
        Me.lblDIOStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDIOStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDIOStatus.Location = New System.Drawing.Point(672, 34)
        Me.lblDIOStatus.Name = "lblDIOStatus"
        Me.lblDIOStatus.Size = New System.Drawing.Size(100, 20)
        Me.lblDIOStatus.TabIndex = 82
        Me.lblDIOStatus.Text = "DIO Status"
        '
        'rtbOverview
        '
        Me.rtbOverview.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.rtbOverview.Location = New System.Drawing.Point(32, 425)
        Me.rtbOverview.Name = "rtbOverview"
        Me.rtbOverview.ReadOnly = True
        Me.rtbOverview.Size = New System.Drawing.Size(377, 208)
        Me.rtbOverview.TabIndex = 92
        Me.rtbOverview.Text = ""
        Me.rtbOverview.Visible = False
        '
        'pBarCalib
        '
        Me.pBarCalib.Location = New System.Drawing.Point(32, 402)
        Me.pBarCalib.Name = "pBarCalib"
        Me.pBarCalib.Size = New System.Drawing.Size(375, 17)
        Me.pBarCalib.TabIndex = 93
        Me.pBarCalib.Visible = False
        '
        'tbxModuleConfig
        '
        Me.tbxModuleConfig.Location = New System.Drawing.Point(166, 36)
        Me.tbxModuleConfig.Name = "tbxModuleConfig"
        Me.tbxModuleConfig.ReadOnly = True
        Me.tbxModuleConfig.Size = New System.Drawing.Size(100, 20)
        Me.tbxModuleConfig.TabIndex = 94
        '
        'btnDoRefMeas
        '
        Me.btnDoRefMeas.Location = New System.Drawing.Point(290, 13)
        Me.btnDoRefMeas.Name = "btnDoRefMeas"
        Me.btnDoRefMeas.Size = New System.Drawing.Size(119, 47)
        Me.btnDoRefMeas.TabIndex = 95
        Me.btnDoRefMeas.Text = "Tägliche Referenzmessung durchführen"
        Me.btnDoRefMeas.UseVisualStyleBackColor = True
        '
        'panRed
        '
        Me.panRed.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.panRed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panRed.Location = New System.Drawing.Point(423, 380)
        Me.panRed.Name = "panRed"
        Me.panRed.Size = New System.Drawing.Size(80, 80)
        Me.panRed.TabIndex = 96
        Me.panRed.Visible = False
        '
        'panYellow
        '
        Me.panYellow.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.panYellow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panYellow.Location = New System.Drawing.Point(423, 459)
        Me.panYellow.Name = "panYellow"
        Me.panYellow.Size = New System.Drawing.Size(80, 80)
        Me.panYellow.TabIndex = 96
        Me.panYellow.Visible = False
        '
        'panGreen
        '
        Me.panGreen.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.panGreen.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.panGreen.Location = New System.Drawing.Point(423, 538)
        Me.panGreen.Name = "panGreen"
        Me.panGreen.Size = New System.Drawing.Size(80, 80)
        Me.panGreen.TabIndex = 96
        Me.panGreen.Visible = False
        '
        'lblBoxStatus
        '
        Me.lblBoxStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBoxStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBoxStatus.Location = New System.Drawing.Point(672, 54)
        Me.lblBoxStatus.Name = "lblBoxStatus"
        Me.lblBoxStatus.Size = New System.Drawing.Size(100, 20)
        Me.lblBoxStatus.TabIndex = 82
        Me.lblBoxStatus.Text = "Box Status"
        '
        'tbxVSerialNumber
        '
        Me.tbxVSerialNumber.Location = New System.Drawing.Point(15, 182)
        Me.tbxVSerialNumber.Name = "tbxVSerialNumber"
        Me.tbxVSerialNumber.Size = New System.Drawing.Size(163, 20)
        Me.tbxVSerialNumber.TabIndex = 1
        Me.tbxVSerialNumber.Visible = False
        '
        'lblVSerialNumber
        '
        Me.lblVSerialNumber.AutoSize = True
        Me.lblVSerialNumber.Location = New System.Drawing.Point(12, 166)
        Me.lblVSerialNumber.Name = "lblVSerialNumber"
        Me.lblVSerialNumber.Size = New System.Drawing.Size(116, 13)
        Me.lblVSerialNumber.TabIndex = 88
        Me.lblVSerialNumber.Text = "V-Modul Seriennummer"
        Me.lblVSerialNumber.Visible = False
        '
        'lblPanRed
        '
        Me.lblPanRed.AutoSize = True
        Me.lblPanRed.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPanRed.Location = New System.Drawing.Point(509, 402)
        Me.lblPanRed.Name = "lblPanRed"
        Me.lblPanRed.Size = New System.Drawing.Size(84, 37)
        Me.lblPanRed.TabIndex = 97
        Me.lblPanRed.Text = "label"
        Me.lblPanRed.Visible = False
        '
        'lblPanYellow
        '
        Me.lblPanYellow.AutoSize = True
        Me.lblPanYellow.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPanYellow.Location = New System.Drawing.Point(509, 488)
        Me.lblPanYellow.Name = "lblPanYellow"
        Me.lblPanYellow.Size = New System.Drawing.Size(84, 37)
        Me.lblPanYellow.TabIndex = 97
        Me.lblPanYellow.Text = "label"
        Me.lblPanYellow.Visible = False
        '
        'lblPanGreen
        '
        Me.lblPanGreen.AutoSize = True
        Me.lblPanGreen.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPanGreen.Location = New System.Drawing.Point(509, 565)
        Me.lblPanGreen.Name = "lblPanGreen"
        Me.lblPanGreen.Size = New System.Drawing.Size(84, 37)
        Me.lblPanGreen.TabIndex = 97
        Me.lblPanGreen.Text = "label"
        Me.lblPanGreen.Visible = False
        '
        'HELIOSEndOfLine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1103, 658)
        Me.Controls.Add(Me.lblPanGreen)
        Me.Controls.Add(Me.lblPanYellow)
        Me.Controls.Add(Me.lblPanRed)
        Me.Controls.Add(Me.lblVSerialNumber)
        Me.Controls.Add(Me.tbxVSerialNumber)
        Me.Controls.Add(Me.lblBoxStatus)
        Me.Controls.Add(Me.panGreen)
        Me.Controls.Add(Me.panYellow)
        Me.Controls.Add(Me.panRed)
        Me.Controls.Add(Me.btnDoRefMeas)
        Me.Controls.Add(Me.tbxModuleConfig)
        Me.Controls.Add(Me.pBarCalib)
        Me.Controls.Add(Me.rtbOverview)
        Me.Controls.Add(Me.lblDIOStatus)
        Me.Controls.Add(Me.btnBackToSerNr)
        Me.Controls.Add(Me.btnMeasureAgain)
        Me.Controls.Add(Me.lblStatusInDB)
        Me.Controls.Add(Me.lblSerialNumber)
        Me.Controls.Add(Me.dgvSerialNumberRGB)
        Me.Controls.Add(Me.tbxSerialNumber)
        Me.Controls.Add(Me.lblModuleConfig)
        Me.Controls.Add(Me.lblModuleType)
        Me.Controls.Add(Me.cBxModuleType)
        Me.Controls.Add(Me.lblPSStatus)
        Me.Controls.Add(Me.lblCasStatus)
        Me.Controls.Add(Me.btnProperties)
        Me.Controls.Add(Me.btnStartMeasurement)
        Me.Controls.Add(Me.btnStopMeasurement)
        Me.Controls.Add(Me.btRestartHelios)
        Me.Controls.Add(Me.tbStatus)
        Me.Controls.Add(Me.ShapeContainer1)
        Me.Name = "HELIOSEndOfLine"
        Me.Text = "HELIOSEndOfLine"
        CType(Me.dgvSerialNumberRGB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbStatus As System.Windows.Forms.TextBox
    Friend WithEvents btRestartHelios As System.Windows.Forms.Button
    Friend WithEvents btnStopMeasurement As System.Windows.Forms.Button
    Friend WithEvents btnStartMeasurement As System.Windows.Forms.Button
    Friend WithEvents btnProperties As System.Windows.Forms.Button
    Friend WithEvents lblCasStatus As System.Windows.Forms.Label
    Friend WithEvents lblPSStatus As System.Windows.Forms.Label
    Friend WithEvents cBxModuleType As System.Windows.Forms.ComboBox
    Friend WithEvents lblModuleType As System.Windows.Forms.Label
    Friend WithEvents lblModuleConfig As System.Windows.Forms.Label
    Friend WithEvents tbxSerialNumber As System.Windows.Forms.TextBox
    Friend WithEvents dgvSerialNumberRGB As System.Windows.Forms.DataGridView
    Friend WithEvents lblSerialNumber As System.Windows.Forms.Label
    Friend WithEvents lblStatusInDB As System.Windows.Forms.Label
    Friend WithEvents btnMeasureAgain As System.Windows.Forms.Button
    Friend WithEvents btnBackToSerNr As System.Windows.Forms.Button
    Friend WithEvents lblDIOStatus As System.Windows.Forms.Label
    Friend WithEvents rtbOverview As System.Windows.Forms.RichTextBox
    Friend WithEvents pBarCalib As System.Windows.Forms.ProgressBar
    Friend WithEvents tbxModuleConfig As System.Windows.Forms.TextBox
    Friend WithEvents btnDoRefMeas As System.Windows.Forms.Button
    Friend WithEvents panRed As System.Windows.Forms.Panel
    Friend WithEvents panYellow As System.Windows.Forms.Panel
    Friend WithEvents panGreen As System.Windows.Forms.Panel
    Friend WithEvents lblBoxStatus As System.Windows.Forms.Label
    Friend WithEvents tbxVSerialNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblVSerialNumber As System.Windows.Forms.Label
    Friend WithEvents lblPanRed As System.Windows.Forms.Label
    Friend WithEvents lblPanYellow As System.Windows.Forms.Label
    Friend WithEvents lblPanGreen As System.Windows.Forms.Label
    Private WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    Private WithEvents LineShape1 As Microsoft.VisualBasic.PowerPacks.LineShape
    Private WithEvents LineShape2 As Microsoft.VisualBasic.PowerPacks.LineShape
    Private WithEvents LineShape3 As Microsoft.VisualBasic.PowerPacks.LineShape
    Friend WithEvents Board As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MaterialNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents serialNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents binning As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
