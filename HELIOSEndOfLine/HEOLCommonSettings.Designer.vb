<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HEOLCommonSettings
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
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.gBxPowerSupply = New System.Windows.Forms.GroupBox()
        Me.btnCheckPS = New System.Windows.Forms.Button()
        Me.mtbxPSIPAddress = New System.Windows.Forms.MaskedTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblPSIPAddress = New System.Windows.Forms.Label()
        Me.tbxPSPort = New System.Windows.Forms.TextBox()
        Me.grbCASSettings = New System.Windows.Forms.GroupBox()
        Me.btnCheckCas = New System.Windows.Forms.Button()
        Me.grbConfigCalib = New System.Windows.Forms.GroupBox()
        Me.btnCalibFile = New System.Windows.Forms.Button()
        Me.btnConfFile = New System.Windows.Forms.Button()
        Me.tbxCalFile = New System.Windows.Forms.TextBox()
        Me.tbxConfFile = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbInterfaceOption = New System.Windows.Forms.ComboBox()
        Me.cmbInterface = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnCheckDIO = New System.Windows.Forms.Button()
        Me.mtbSafetyBox = New System.Windows.Forms.MaskedTextBox()
        Me.mtbJumper = New System.Windows.Forms.MaskedTextBox()
        Me.lbl = New System.Windows.Forms.Label()
        Me.tBxDeviceName = New System.Windows.Forms.TextBox()
        Me.lblDeviceName = New System.Windows.Forms.Label()
        Me.lblJumper = New System.Windows.Forms.Label()
        Me.gBxAdminEnable = New System.Windows.Forms.GroupBox()
        Me.checkBxBox = New System.Windows.Forms.CheckBox()
        Me.checkBxDIO = New System.Windows.Forms.CheckBox()
        Me.checkBxPS = New System.Windows.Forms.CheckBox()
        Me.checkBxCAS = New System.Windows.Forms.CheckBox()
        Me.checkBxCAN = New System.Windows.Forms.CheckBox()
        Me.gBxConfig = New System.Windows.Forms.GroupBox()
        Me.btnCheckConfig = New System.Windows.Forms.Button()
        Me.tbxConfigV2 = New System.Windows.Forms.TextBox()
        Me.tbxConfigV = New System.Windows.Forms.TextBox()
        Me.tbxConfigRGB3 = New System.Windows.Forms.TextBox()
        Me.tbxConfigRGB2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbxConfigRGB = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblConfigRGB = New System.Windows.Forms.Label()
        Me.lblConfigUV = New System.Windows.Forms.Label()
        Me.grpComport = New System.Windows.Forms.GroupBox()
        Me.btnCheckCOM = New System.Windows.Forms.Button()
        Me.cmbComport = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnPSOff = New System.Windows.Forms.Button()
        Me.gBxDataStorage = New System.Windows.Forms.GroupBox()
        Me.btnCheckStorage = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbxStoragePath = New System.Windows.Forms.TextBox()
        Me.chkCamstarSN = New System.Windows.Forms.CheckBox()
        Me.btnDoMonthlyRef = New System.Windows.Forms.Button()
        Me.gBxPowerSupply.SuspendLayout()
        Me.grbCASSettings.SuspendLayout()
        Me.grbConfigCalib.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.gBxAdminEnable.SuspendLayout()
        Me.gBxConfig.SuspendLayout()
        Me.grpComport.SuspendLayout()
        Me.gBxDataStorage.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(696, 59)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(111, 33)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Speichern"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(696, 98)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(111, 34)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Schliessen"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'gBxPowerSupply
        '
        Me.gBxPowerSupply.Controls.Add(Me.btnCheckPS)
        Me.gBxPowerSupply.Controls.Add(Me.mtbxPSIPAddress)
        Me.gBxPowerSupply.Controls.Add(Me.Label2)
        Me.gBxPowerSupply.Controls.Add(Me.lblPSIPAddress)
        Me.gBxPowerSupply.Controls.Add(Me.tbxPSPort)
        Me.gBxPowerSupply.Location = New System.Drawing.Point(22, 22)
        Me.gBxPowerSupply.Name = "gBxPowerSupply"
        Me.gBxPowerSupply.Size = New System.Drawing.Size(312, 108)
        Me.gBxPowerSupply.TabIndex = 2
        Me.gBxPowerSupply.TabStop = False
        Me.gBxPowerSupply.Text = "Spannungsversorgung"
        '
        'btnCheckPS
        '
        Me.btnCheckPS.Location = New System.Drawing.Point(6, 76)
        Me.btnCheckPS.Name = "btnCheckPS"
        Me.btnCheckPS.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckPS.TabIndex = 2
        Me.btnCheckPS.Text = "Prüfen"
        Me.btnCheckPS.UseVisualStyleBackColor = True
        '
        'mtbxPSIPAddress
        '
        Me.mtbxPSIPAddress.Culture = New System.Globalization.CultureInfo("en-US")
        Me.mtbxPSIPAddress.Location = New System.Drawing.Point(148, 19)
        Me.mtbxPSIPAddress.Mask = "##0.##0.##0.##0"
        Me.mtbxPSIPAddress.Name = "mtbxPSIPAddress"
        Me.mtbxPSIPAddress.Size = New System.Drawing.Size(100, 20)
        Me.mtbxPSIPAddress.TabIndex = 2
        Me.mtbxPSIPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Port"
        '
        'lblPSIPAddress
        '
        Me.lblPSIPAddress.AutoSize = True
        Me.lblPSIPAddress.Location = New System.Drawing.Point(15, 22)
        Me.lblPSIPAddress.Name = "lblPSIPAddress"
        Me.lblPSIPAddress.Size = New System.Drawing.Size(84, 13)
        Me.lblPSIPAddress.TabIndex = 1
        Me.lblPSIPAddress.Text = "Netwerkadresse"
        '
        'tbxPSPort
        '
        Me.tbxPSPort.Location = New System.Drawing.Point(148, 45)
        Me.tbxPSPort.Name = "tbxPSPort"
        Me.tbxPSPort.Size = New System.Drawing.Size(100, 20)
        Me.tbxPSPort.TabIndex = 0
        '
        'grbCASSettings
        '
        Me.grbCASSettings.Controls.Add(Me.btnCheckCas)
        Me.grbCASSettings.Controls.Add(Me.grbConfigCalib)
        Me.grbCASSettings.Controls.Add(Me.GroupBox1)
        Me.grbCASSettings.Location = New System.Drawing.Point(22, 206)
        Me.grbCASSettings.Name = "grbCASSettings"
        Me.grbCASSettings.Size = New System.Drawing.Size(584, 231)
        Me.grbCASSettings.TabIndex = 11
        Me.grbCASSettings.TabStop = False
        Me.grbCASSettings.Text = "CAS 140 Einstellungen"
        '
        'btnCheckCas
        '
        Me.btnCheckCas.Location = New System.Drawing.Point(6, 198)
        Me.btnCheckCas.Name = "btnCheckCas"
        Me.btnCheckCas.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckCas.TabIndex = 2
        Me.btnCheckCas.Text = "Prüfen"
        Me.btnCheckCas.UseVisualStyleBackColor = True
        '
        'grbConfigCalib
        '
        Me.grbConfigCalib.Controls.Add(Me.btnCalibFile)
        Me.grbConfigCalib.Controls.Add(Me.btnConfFile)
        Me.grbConfigCalib.Controls.Add(Me.tbxCalFile)
        Me.grbConfigCalib.Controls.Add(Me.tbxConfFile)
        Me.grbConfigCalib.Controls.Add(Me.Label9)
        Me.grbConfigCalib.Controls.Add(Me.Label8)
        Me.grbConfigCalib.Location = New System.Drawing.Point(6, 89)
        Me.grbConfigCalib.Name = "grbConfigCalib"
        Me.grbConfigCalib.Size = New System.Drawing.Size(551, 103)
        Me.grbConfigCalib.TabIndex = 1
        Me.grbConfigCalib.TabStop = False
        Me.grbConfigCalib.Text = "Konfigurations- und Kalibrierdatei"
        '
        'btnCalibFile
        '
        Me.btnCalibFile.AutoSize = True
        Me.btnCalibFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCalibFile.Location = New System.Drawing.Point(519, 56)
        Me.btnCalibFile.Name = "btnCalibFile"
        Me.btnCalibFile.Size = New System.Drawing.Size(26, 23)
        Me.btnCalibFile.TabIndex = 5
        Me.btnCalibFile.Text = "..."
        Me.btnCalibFile.UseVisualStyleBackColor = True
        '
        'btnConfFile
        '
        Me.btnConfFile.AutoSize = True
        Me.btnConfFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnConfFile.Location = New System.Drawing.Point(519, 30)
        Me.btnConfFile.Name = "btnConfFile"
        Me.btnConfFile.Size = New System.Drawing.Size(26, 23)
        Me.btnConfFile.TabIndex = 4
        Me.btnConfFile.Text = "..."
        Me.btnConfFile.UseVisualStyleBackColor = True
        '
        'tbxCalFile
        '
        Me.tbxCalFile.Location = New System.Drawing.Point(72, 59)
        Me.tbxCalFile.Name = "tbxCalFile"
        Me.tbxCalFile.ReadOnly = True
        Me.tbxCalFile.Size = New System.Drawing.Size(427, 20)
        Me.tbxCalFile.TabIndex = 3
        '
        'tbxConfFile
        '
        Me.tbxConfFile.Location = New System.Drawing.Point(72, 33)
        Me.tbxConfFile.Name = "tbxConfFile"
        Me.tbxConfFile.ReadOnly = True
        Me.tbxConfFile.Size = New System.Drawing.Size(427, 20)
        Me.tbxConfFile.TabIndex = 2
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 62)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(53, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Kal. Datei"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 36)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Konf. Datei"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cmbInterfaceOption)
        Me.GroupBox1.Controls.Add(Me.cmbInterface)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 19)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(551, 64)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Schnittstelle"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(260, 16)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "Optionen"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Schnittstelle"
        '
        'cmbInterfaceOption
        '
        Me.cmbInterfaceOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbInterfaceOption.FormattingEnabled = True
        Me.cmbInterfaceOption.Location = New System.Drawing.Point(263, 34)
        Me.cmbInterfaceOption.Name = "cmbInterfaceOption"
        Me.cmbInterfaceOption.Size = New System.Drawing.Size(242, 21)
        Me.cmbInterfaceOption.TabIndex = 1
        '
        'cmbInterface
        '
        Me.cmbInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbInterface.Location = New System.Drawing.Point(23, 34)
        Me.cmbInterface.Name = "cmbInterface"
        Me.cmbInterface.Size = New System.Drawing.Size(222, 21)
        Me.cmbInterface.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnCheckDIO)
        Me.GroupBox2.Controls.Add(Me.mtbSafetyBox)
        Me.GroupBox2.Controls.Add(Me.mtbJumper)
        Me.GroupBox2.Controls.Add(Me.lbl)
        Me.GroupBox2.Controls.Add(Me.tBxDeviceName)
        Me.GroupBox2.Controls.Add(Me.lblDeviceName)
        Me.GroupBox2.Controls.Add(Me.lblJumper)
        Me.GroupBox2.Location = New System.Drawing.Point(22, 443)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(312, 172)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Digital Input Output"
        '
        'btnCheckDIO
        '
        Me.btnCheckDIO.Location = New System.Drawing.Point(6, 135)
        Me.btnCheckDIO.Name = "btnCheckDIO"
        Me.btnCheckDIO.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckDIO.TabIndex = 2
        Me.btnCheckDIO.Text = "Prüfen"
        Me.btnCheckDIO.UseVisualStyleBackColor = True
        '
        'mtbSafetyBox
        '
        Me.mtbSafetyBox.Culture = New System.Globalization.CultureInfo("en-US")
        Me.mtbSafetyBox.Location = New System.Drawing.Point(218, 133)
        Me.mtbSafetyBox.Mask = "0.0"
        Me.mtbSafetyBox.Name = "mtbSafetyBox"
        Me.mtbSafetyBox.Size = New System.Drawing.Size(30, 20)
        Me.mtbSafetyBox.TabIndex = 2
        Me.mtbSafetyBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'mtbJumper
        '
        Me.mtbJumper.Culture = New System.Globalization.CultureInfo("en-US")
        Me.mtbJumper.Location = New System.Drawing.Point(148, 52)
        Me.mtbJumper.Mask = "0.0"
        Me.mtbJumper.Name = "mtbJumper"
        Me.mtbJumper.Size = New System.Drawing.Size(30, 20)
        Me.mtbJumper.TabIndex = 2
        Me.mtbJumper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl
        '
        Me.lbl.AutoSize = True
        Me.lbl.Location = New System.Drawing.Point(106, 140)
        Me.lbl.Name = "lbl"
        Me.lbl.Size = New System.Drawing.Size(72, 13)
        Me.lbl.TabIndex = 1
        Me.lbl.Text = "Schutzkasten"
        '
        'tBxDeviceName
        '
        Me.tBxDeviceName.Location = New System.Drawing.Point(148, 25)
        Me.tBxDeviceName.Name = "tBxDeviceName"
        Me.tBxDeviceName.Size = New System.Drawing.Size(100, 20)
        Me.tBxDeviceName.TabIndex = 0
        '
        'lblDeviceName
        '
        Me.lblDeviceName.AutoSize = True
        Me.lblDeviceName.Location = New System.Drawing.Point(15, 28)
        Me.lblDeviceName.Name = "lblDeviceName"
        Me.lblDeviceName.Size = New System.Drawing.Size(67, 13)
        Me.lblDeviceName.TabIndex = 1
        Me.lblDeviceName.Text = "Devicename"
        '
        'lblJumper
        '
        Me.lblJumper.AutoSize = True
        Me.lblJumper.Location = New System.Drawing.Point(15, 55)
        Me.lblJumper.Name = "lblJumper"
        Me.lblJumper.Size = New System.Drawing.Size(41, 13)
        Me.lblJumper.TabIndex = 1
        Me.lblJumper.Text = "Jumper"
        '
        'gBxAdminEnable
        '
        Me.gBxAdminEnable.Controls.Add(Me.checkBxBox)
        Me.gBxAdminEnable.Controls.Add(Me.checkBxDIO)
        Me.gBxAdminEnable.Controls.Add(Me.checkBxPS)
        Me.gBxAdminEnable.Controls.Add(Me.checkBxCAS)
        Me.gBxAdminEnable.Controls.Add(Me.checkBxCAN)
        Me.gBxAdminEnable.Location = New System.Drawing.Point(353, 443)
        Me.gBxAdminEnable.Name = "gBxAdminEnable"
        Me.gBxAdminEnable.Size = New System.Drawing.Size(126, 146)
        Me.gBxAdminEnable.TabIndex = 2
        Me.gBxAdminEnable.TabStop = False
        Me.gBxAdminEnable.Text = "Enable Hardware"
        '
        'checkBxBox
        '
        Me.checkBxBox.AutoSize = True
        Me.checkBxBox.Location = New System.Drawing.Point(18, 116)
        Me.checkBxBox.Name = "checkBxBox"
        Me.checkBxBox.Size = New System.Drawing.Size(44, 17)
        Me.checkBxBox.TabIndex = 2
        Me.checkBxBox.Text = "Box"
        Me.checkBxBox.UseVisualStyleBackColor = True
        '
        'checkBxDIO
        '
        Me.checkBxDIO.AutoSize = True
        Me.checkBxDIO.Location = New System.Drawing.Point(18, 93)
        Me.checkBxDIO.Name = "checkBxDIO"
        Me.checkBxDIO.Size = New System.Drawing.Size(45, 17)
        Me.checkBxDIO.TabIndex = 2
        Me.checkBxDIO.Text = "DIO"
        Me.checkBxDIO.UseVisualStyleBackColor = True
        '
        'checkBxPS
        '
        Me.checkBxPS.AutoSize = True
        Me.checkBxPS.Location = New System.Drawing.Point(18, 70)
        Me.checkBxPS.Name = "checkBxPS"
        Me.checkBxPS.Size = New System.Drawing.Size(40, 17)
        Me.checkBxPS.TabIndex = 2
        Me.checkBxPS.Text = "PS"
        Me.checkBxPS.UseVisualStyleBackColor = True
        '
        'checkBxCAS
        '
        Me.checkBxCAS.AutoSize = True
        Me.checkBxCAS.Location = New System.Drawing.Point(18, 49)
        Me.checkBxCAS.Name = "checkBxCAS"
        Me.checkBxCAS.Size = New System.Drawing.Size(47, 17)
        Me.checkBxCAS.TabIndex = 2
        Me.checkBxCAS.Text = "CAS"
        Me.checkBxCAS.UseVisualStyleBackColor = True
        '
        'checkBxCAN
        '
        Me.checkBxCAN.AutoSize = True
        Me.checkBxCAN.Location = New System.Drawing.Point(18, 28)
        Me.checkBxCAN.Name = "checkBxCAN"
        Me.checkBxCAN.Size = New System.Drawing.Size(48, 17)
        Me.checkBxCAN.TabIndex = 2
        Me.checkBxCAN.Text = "CAN"
        Me.checkBxCAN.UseVisualStyleBackColor = True
        '
        'gBxConfig
        '
        Me.gBxConfig.Controls.Add(Me.btnCheckConfig)
        Me.gBxConfig.Controls.Add(Me.tbxConfigV2)
        Me.gBxConfig.Controls.Add(Me.tbxConfigV)
        Me.gBxConfig.Controls.Add(Me.tbxConfigRGB3)
        Me.gBxConfig.Controls.Add(Me.tbxConfigRGB2)
        Me.gBxConfig.Controls.Add(Me.Label5)
        Me.gBxConfig.Controls.Add(Me.tbxConfigRGB)
        Me.gBxConfig.Controls.Add(Me.Label4)
        Me.gBxConfig.Controls.Add(Me.Label3)
        Me.gBxConfig.Controls.Add(Me.lblConfigRGB)
        Me.gBxConfig.Controls.Add(Me.lblConfigUV)
        Me.gBxConfig.Location = New System.Drawing.Point(346, 22)
        Me.gBxConfig.Name = "gBxConfig"
        Me.gBxConfig.Size = New System.Drawing.Size(326, 107)
        Me.gBxConfig.TabIndex = 12
        Me.gBxConfig.TabStop = False
        Me.gBxConfig.Text = "Konfiguration"
        '
        'btnCheckConfig
        '
        Me.btnCheckConfig.Location = New System.Drawing.Point(9, 78)
        Me.btnCheckConfig.Name = "btnCheckConfig"
        Me.btnCheckConfig.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckConfig.TabIndex = 2
        Me.btnCheckConfig.Text = "Prüfen"
        Me.btnCheckConfig.UseVisualStyleBackColor = True
        '
        'tbxConfigV2
        '
        Me.tbxConfigV2.Location = New System.Drawing.Point(156, 54)
        Me.tbxConfigV2.Name = "tbxConfigV2"
        Me.tbxConfigV2.Size = New System.Drawing.Size(77, 20)
        Me.tbxConfigV2.TabIndex = 0
        '
        'tbxConfigV
        '
        Me.tbxConfigV.Location = New System.Drawing.Point(73, 54)
        Me.tbxConfigV.Name = "tbxConfigV"
        Me.tbxConfigV.Size = New System.Drawing.Size(77, 20)
        Me.tbxConfigV.TabIndex = 0
        '
        'tbxConfigRGB3
        '
        Me.tbxConfigRGB3.Location = New System.Drawing.Point(239, 31)
        Me.tbxConfigRGB3.Name = "tbxConfigRGB3"
        Me.tbxConfigRGB3.Size = New System.Drawing.Size(77, 20)
        Me.tbxConfigRGB3.TabIndex = 0
        '
        'tbxConfigRGB2
        '
        Me.tbxConfigRGB2.Location = New System.Drawing.Point(156, 31)
        Me.tbxConfigRGB2.Name = "tbxConfigRGB2"
        Me.tbxConfigRGB2.Size = New System.Drawing.Size(77, 20)
        Me.tbxConfigRGB2.TabIndex = 0
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(233, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Finalmessung"
        '
        'tbxConfigRGB
        '
        Me.tbxConfigRGB.Location = New System.Drawing.Point(73, 31)
        Me.tbxConfigRGB.Name = "tbxConfigRGB"
        Me.tbxConfigRGB.Size = New System.Drawing.Size(77, 20)
        Me.tbxConfigRGB.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(150, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Abschlussmess."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(71, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Kalibrierung"
        '
        'lblConfigRGB
        '
        Me.lblConfigRGB.AutoSize = True
        Me.lblConfigRGB.Location = New System.Drawing.Point(9, 34)
        Me.lblConfigRGB.Name = "lblConfigRGB"
        Me.lblConfigRGB.Size = New System.Drawing.Size(30, 13)
        Me.lblConfigRGB.TabIndex = 1
        Me.lblConfigRGB.Text = "RGB"
        '
        'lblConfigUV
        '
        Me.lblConfigUV.AutoSize = True
        Me.lblConfigUV.Location = New System.Drawing.Point(9, 57)
        Me.lblConfigUV.Name = "lblConfigUV"
        Me.lblConfigUV.Size = New System.Drawing.Size(14, 13)
        Me.lblConfigUV.TabIndex = 1
        Me.lblConfigUV.Text = "V"
        '
        'grpComport
        '
        Me.grpComport.Controls.Add(Me.btnCheckCOM)
        Me.grpComport.Controls.Add(Me.cmbComport)
        Me.grpComport.Controls.Add(Me.Label1)
        Me.grpComport.Location = New System.Drawing.Point(485, 443)
        Me.grpComport.Name = "grpComport"
        Me.grpComport.Size = New System.Drawing.Size(168, 75)
        Me.grpComport.TabIndex = 13
        Me.grpComport.TabStop = False
        Me.grpComport.Text = "Serial Port"
        '
        'btnCheckCOM
        '
        Me.btnCheckCOM.Location = New System.Drawing.Point(6, 46)
        Me.btnCheckCOM.Name = "btnCheckCOM"
        Me.btnCheckCOM.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckCOM.TabIndex = 3
        Me.btnCheckCOM.Text = "Prüfen"
        Me.btnCheckCOM.UseVisualStyleBackColor = True
        '
        'cmbComport
        '
        Me.cmbComport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbComport.FormattingEnabled = True
        Me.cmbComport.Location = New System.Drawing.Point(68, 19)
        Me.cmbComport.Name = "cmbComport"
        Me.cmbComport.Size = New System.Drawing.Size(94, 21)
        Me.cmbComport.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "COM-Port:"
        '
        'btnPSOff
        '
        Me.btnPSOff.Location = New System.Drawing.Point(696, 209)
        Me.btnPSOff.Name = "btnPSOff"
        Me.btnPSOff.Size = New System.Drawing.Size(111, 34)
        Me.btnPSOff.TabIndex = 1
        Me.btnPSOff.Text = "PS OFF"
        Me.btnPSOff.UseVisualStyleBackColor = True
        '
        'gBxDataStorage
        '
        Me.gBxDataStorage.Controls.Add(Me.btnCheckStorage)
        Me.gBxDataStorage.Controls.Add(Me.Label10)
        Me.gBxDataStorage.Controls.Add(Me.tbxStoragePath)
        Me.gBxDataStorage.Location = New System.Drawing.Point(22, 136)
        Me.gBxDataStorage.Name = "gBxDataStorage"
        Me.gBxDataStorage.Size = New System.Drawing.Size(650, 64)
        Me.gBxDataStorage.TabIndex = 3
        Me.gBxDataStorage.TabStop = False
        Me.gBxDataStorage.Text = "Datenablage"
        '
        'btnCheckStorage
        '
        Me.btnCheckStorage.Location = New System.Drawing.Point(569, 17)
        Me.btnCheckStorage.Name = "btnCheckStorage"
        Me.btnCheckStorage.Size = New System.Drawing.Size(75, 23)
        Me.btnCheckStorage.TabIndex = 2
        Me.btnCheckStorage.Text = "Prüfen"
        Me.btnCheckStorage.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(29, 13)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Pfad"
        '
        'tbxStoragePath
        '
        Me.tbxStoragePath.Location = New System.Drawing.Point(41, 19)
        Me.tbxStoragePath.Name = "tbxStoragePath"
        Me.tbxStoragePath.Size = New System.Drawing.Size(464, 20)
        Me.tbxStoragePath.TabIndex = 0
        '
        'chkCamstarSN
        '
        Me.chkCamstarSN.AutoSize = True
        Me.chkCamstarSN.Location = New System.Drawing.Point(494, 559)
        Me.chkCamstarSN.Name = "chkCamstarSN"
        Me.chkCamstarSN.Size = New System.Drawing.Size(235, 17)
        Me.chkCamstarSN.TabIndex = 14
        Me.chkCamstarSN.Text = "Camstar-Zwang für Seriennummer auslassen"
        Me.chkCamstarSN.UseVisualStyleBackColor = True
        '
        'btnDoMonthlyRef
        '
        Me.btnDoMonthlyRef.Location = New System.Drawing.Point(699, 270)
        Me.btnDoMonthlyRef.Name = "btnDoMonthlyRef"
        Me.btnDoMonthlyRef.Size = New System.Drawing.Size(108, 55)
        Me.btnDoMonthlyRef.TabIndex = 15
        Me.btnDoMonthlyRef.Text = "Monatliche Referenzmessung starten"
        Me.btnDoMonthlyRef.UseVisualStyleBackColor = True
        '
        'HEOLCommonSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 645)
        Me.Controls.Add(Me.btnDoMonthlyRef)
        Me.Controls.Add(Me.chkCamstarSN)
        Me.Controls.Add(Me.gBxDataStorage)
        Me.Controls.Add(Me.grpComport)
        Me.Controls.Add(Me.gBxConfig)
        Me.Controls.Add(Me.grbCASSettings)
        Me.Controls.Add(Me.gBxAdminEnable)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.gBxPowerSupply)
        Me.Controls.Add(Me.btnPSOff)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Name = "HEOLCommonSettings"
        Me.Text = "CommonSettings"
        Me.gBxPowerSupply.ResumeLayout(False)
        Me.gBxPowerSupply.PerformLayout()
        Me.grbCASSettings.ResumeLayout(False)
        Me.grbConfigCalib.ResumeLayout(False)
        Me.grbConfigCalib.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.gBxAdminEnable.ResumeLayout(False)
        Me.gBxAdminEnable.PerformLayout()
        Me.gBxConfig.ResumeLayout(False)
        Me.gBxConfig.PerformLayout()
        Me.grpComport.ResumeLayout(False)
        Me.grpComport.PerformLayout()
        Me.gBxDataStorage.ResumeLayout(False)
        Me.gBxDataStorage.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents gBxPowerSupply As System.Windows.Forms.GroupBox
    Friend WithEvents mtbxPSIPAddress As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblPSIPAddress As System.Windows.Forms.Label
    Friend WithEvents tbxPSPort As System.Windows.Forms.TextBox
    Friend WithEvents grbCASSettings As System.Windows.Forms.GroupBox
    Friend WithEvents btnCheckCas As System.Windows.Forms.Button
    Friend WithEvents grbConfigCalib As System.Windows.Forms.GroupBox
    Friend WithEvents btnCalibFile As System.Windows.Forms.Button
    Friend WithEvents btnConfFile As System.Windows.Forms.Button
    Friend WithEvents tbxCalFile As System.Windows.Forms.TextBox
    Friend WithEvents tbxConfFile As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbInterfaceOption As System.Windows.Forms.ComboBox
    Friend WithEvents cmbInterface As System.Windows.Forms.ComboBox
    Friend WithEvents btnCheckPS As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCheckDIO As System.Windows.Forms.Button
    Friend WithEvents lbl As System.Windows.Forms.Label
    Friend WithEvents lblJumper As System.Windows.Forms.Label
    Friend WithEvents mtbSafetyBox As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbJumper As System.Windows.Forms.MaskedTextBox
    Friend WithEvents tBxDeviceName As System.Windows.Forms.TextBox
    Friend WithEvents lblDeviceName As System.Windows.Forms.Label
    Friend WithEvents gBxAdminEnable As System.Windows.Forms.GroupBox
    Friend WithEvents checkBxCAS As System.Windows.Forms.CheckBox
    Friend WithEvents checkBxCAN As System.Windows.Forms.CheckBox
    Friend WithEvents checkBxDIO As System.Windows.Forms.CheckBox
    Friend WithEvents checkBxPS As System.Windows.Forms.CheckBox
    Friend WithEvents gBxConfig As System.Windows.Forms.GroupBox
    Friend WithEvents btnCheckConfig As System.Windows.Forms.Button
    Friend WithEvents tbxConfigV As System.Windows.Forms.TextBox
    Friend WithEvents tbxConfigRGB As System.Windows.Forms.TextBox
    Friend WithEvents lblConfigRGB As System.Windows.Forms.Label
    Friend WithEvents lblConfigUV As System.Windows.Forms.Label
    Friend WithEvents grpComport As System.Windows.Forms.GroupBox
    Friend WithEvents btnCheckCOM As System.Windows.Forms.Button
    Friend WithEvents cmbComport As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnPSOff As System.Windows.Forms.Button
    Friend WithEvents checkBxBox As System.Windows.Forms.CheckBox
    Friend WithEvents tbxConfigV2 As System.Windows.Forms.TextBox
    Friend WithEvents tbxConfigRGB2 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tbxConfigRGB3 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents gBxDataStorage As System.Windows.Forms.GroupBox
    Friend WithEvents btnCheckStorage As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbxStoragePath As System.Windows.Forms.TextBox
    Friend WithEvents chkCamstarSN As System.Windows.Forms.CheckBox
    Friend WithEvents btnDoMonthlyRef As System.Windows.Forms.Button
End Class
