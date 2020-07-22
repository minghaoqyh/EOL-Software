Imports HELIOSCommunication
Imports HELIOSCommunication.Peak.Can.Basic



Public Class HELIOSEndOfLine
    Public WithEvents smh As StateMachineEndOfLine = New StateMachineEndOfLine()

    Private Sub startCalibrationHelios(ByVal filename As String)
        Dim smData As StateMachineEndOfLineData = New StateMachineEndOfLineData
        Try
            GetMatData(smData)
        Catch ex As Exception
            MsgBox("Fehler: " & ex.Message, vbExclamation)
            Exit Sub
        End Try

        btnStartMeasurement.Enabled = False
        pBarCalib.Value = 0
        btnStopMeasurement.Enabled = True
        rtbOverview.Text = ""

        changeStatusLight(StateMachineEndOfLine.statusLight.Yellow)
        smh.LoadProgramFLowHelios(filename)
        smh.SetData(smData)
        smh.ExecuteProgramFlowHelios()

        While (smh.Status = StateMachineEndOfLine.smhStatus.WorkingGood) Or (smh.Status = StateMachineEndOfLine.smhStatus.WorkingBad)
            System.Threading.Thread.Sleep(20)
            Application.DoEvents()
        End While

        btnBackToSerNr.Enabled = True
        If smh.Status = StateMachineEndOfLine.smhStatus.FinishGood Then
            changeStatusLight(StateMachineEndOfLine.statusLight.Green)
            btnStartMeasurement.Enabled = False
            btnStopMeasurement.Enabled = False
            btnBackToSerNr.Enabled = True
        Else
            changeStatusLight(StateMachineEndOfLine.statusLight.Red)
            btnStartMeasurement.Enabled = True
            btnStopMeasurement.Enabled = False
            btnBackToSerNr.Enabled = True
        End If
    End Sub

    Private Sub GetMatData(ByRef smData As StateMachineEndOfLineData)
        smData.ModuleType = cBxModuleType.SelectedIndex
        Select Case cBxModuleType.SelectedIndex
            Case 0 ' RGB-Modul
                For i = 0 To 6
                    Try
                        smData.MatNumber(i) = dgvSerialNumberRGB.Item(1, i).Value
                        If (smData.MatNumber(i)(0) <> Chr(68)) Or (smData.MatNumber(i).Length <> 9) Then Throw New Exception
                    Catch ex As Exception
                        Throw New Exception("Ungültige Materialnummer")
                    End Try

                    Try
                        smData.SerialNumber(i) = dgvSerialNumberRGB.Item(2, i).Value
                        Dim tmp As UInteger = CUInt(smData.SerialNumber(i))
                        If smData.SerialNumber(i) = String.Empty Then Throw New Exception
                    Catch ex As Exception
                        Throw New Exception("Ungültige Seriennummer")
                    End Try

                    If (i > 3) Then
                        Try
                            smData.Binning(i) = dgvSerialNumberRGB.Item(3, i).Value
                            If (smData.Binning(i).Length <> 2) Then Throw New Exception
                        Catch ex As Exception
                            Throw New Exception("Ungültiges Binning")
                        End Try
                    End If
                Next i
                smData.CZMSeriennummer = tbxSerialNumber.Text.Substring(14)
                smData.Barcode = tbxSerialNumber.Text
            Case 1   ' V-Modul
                If Not IsNothing(dgvSerialNumberRGB.Item(1, 0).Value) Then
                    smData.MatNumber(0) = dgvSerialNumberRGB.Item(1, 0).Value
                    If (smData.MatNumber(0)(0) <> Chr(68)) Or (smData.MatNumber(0).Length <> 9) Then
                        Throw New Exception("Ungültige Materialnummer")
                    End If
                End If
                If Not IsNothing(dgvSerialNumberRGB.Item(2, 0).Value) Then
                    smData.SerialNumber(0) = dgvSerialNumberRGB.Item(2, 0).Value
                    Try
                        Dim tmp As UInteger = CUInt(smData.SerialNumber(0))
                    Catch ex As Exception
                        Throw New Exception("Ungültige Seriennummer")
                    End Try
                End If
                If Not IsNothing(dgvSerialNumberRGB.Item(3, 0).Value) Then
                    smData.Binning(0) = dgvSerialNumberRGB.Item(3, 0).Value
                    If (smData.Binning(0).Length <> 2) Then
                        Throw New Exception("Ungültiges Binning")
                    End If
                End If
                smData.CZMSeriennummer = tbxVSerialNumber.Text.Substring(14)
                smData.Barcode = tbxSerialNumber.Text
                smData.BarcodeV = tbxVSerialNumber.Text
            Case 2, 4   'RGB nur Messung
                If (tbxSerialNumber.Text.Length = 23) Then smData.CZMSeriennummer = tbxSerialNumber.Text.Substring(14)
                smData.Barcode = tbxSerialNumber.Text
            Case 3 'V nur Messung
                If (tbxVSerialNumber.Text.Length = 23) Then smData.CZMSeriennummer = tbxVSerialNumber.Text.Substring(14)
                smData.Barcode = tbxSerialNumber.Text
                smData.BarcodeV = tbxVSerialNumber.Text
        End Select
    End Sub

    Private Sub HELIOSEndOfLine_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'Load if reference measurement is necessary at the beginning
            Dim refOK As Boolean
            Dim hardwareOK As Boolean
            refOK = InitSoftware()
            'init and check Hardware
            hardwareOK = InitHardware()
            'if hardware OK, enable buttons
            DecideVisibiltySection12(refOK, hardwareOK)
            Me.WindowState = FormWindowState.Maximized
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub
#Region "Init Software functions"
    Private Function InitSoftware() As Boolean
        Dim iniReader As IniReader = New IniReader
        Dim strDate As String
        Dim refOK As Boolean = False
        strDate = iniReader.ReadValueFromFile("RefMeasurement", "date", "", ".\Settings.ini")
        Dim datum As Date = Date.Parse(strDate)
        If ((datum.Year = Now.Year) And (datum.Month = Now.Month) And (datum.Day = Now.Day)) Then
            refOK = True
        Else
            refOK = False
        End If

        cBxModuleType.SelectedIndex = 0

        Dim tempText As String
        tempText = iniReader.ReadValueFromFile("ConfigRGB", "config", "", ".\Settings.ini")
        If tempText <> String.Empty Then
            tbxModuleConfig.Text = tempText
        Else
            tbxModuleConfig.Text = String.Empty
        End If
        Return refOK
    End Function

    Private Sub DecideVisibiltySection12(ByVal refOK As Boolean, ByVal hardwareOK As Boolean)
        If (refOK And hardwareOK) Then
            visibleOfSection2(True)
            visibleOfSection1a(True)
            visibleOfSection1b(False)
            tbxSerialNumber.Focus()
        ElseIf (refOK And Not hardwareOK) Then
            visibleOfSection1a(False)
            visibleOfSection1b(False)
            visibleOfSection2(False)
        ElseIf (Not refOK And hardwareOK) Then
            visibleOfSection1a(False)
            visibleOfSection2(False)
            visibleOfSection1b(True)
        Else
            visibleOfSection1a(False)
            visibleOfSection1b(False)
            visibleOfSection2(False)
        End If
    End Sub
#End Region

#Region "Init Hardware functions"
    Private Function InitHardware() As Boolean
        Dim initOK As Boolean = True
        Dim iniReader As IniReader = New IniReader
        If ((Not Me.InitCasCommunication()) And (iniReader.ReadValueFromFile("EnableSection", "CAS", "", ".\Settings.ini") = "1")) Then
            initOK = False
        End If
        If ((Not Me.InitCanCommunication()) And (iniReader.ReadValueFromFile("EnableSection", "CAN", "", ".\Settings.ini") = "1")) Then
            initOK = False
        End If
        If ((Not Me.InitPSCommunication()) And (iniReader.ReadValueFromFile("EnableSection", "PS", "", ".\Settings.ini") = "1")) Then
            initOK = False
        End If
        If ((Not Me.InitDIOCommunication()) And (iniReader.ReadValueFromFile("EnableSection", "DIO", "", ".\Settings.ini") = "1")) Then
            initOK = False
        End If
        If ((Not Me.InitBoxCommunication()) And (iniReader.ReadValueFromFile("EnableSection", "", "", ".\Settings.ini") = "1")) Then
            initOK = False
        End If
        Return initOK
    End Function

    Private Function InitDIOCommunication() As Boolean
        Dim iniReader As IniReader = New IniReader
        Dim deviceName As String = String.Empty
        Dim returnValue As Boolean = False
        smh.dioPorts = New HardwareCommunication.NI6520_DAQ()
        deviceName = iniReader.ReadValueFromFile("DIO", "deviceName", "", ".\Settings.ini")

        If Not smh.dioPorts.InitNI6520(deviceName) Then
            lblDIOStatus.BackColor = Color.Red
            Return False
        Else
            lblDIOStatus.BackColor = Color.LightGreen
            returnValue = True
        End If

        'assignment of individual pins
        Dim text As String
        text = iniReader.ReadValueFromFile("DIO", "jumper", "", ".\Settings.ini")
        smh.dioPorts.Jumper.Port = text.Substring(0, 1)
        smh.dioPorts.Jumper.PortPin = text.Substring(2, 1)
        text = iniReader.ReadValueFromFile("DIO", "safetyBox", "", ".\Settings.ini")
        smh.dioPorts.SafetyBox.Port = text.Substring(0, 1)
        smh.dioPorts.SafetyBox.PortPin = text.Substring(2, 1)

        Return returnValue
    End Function

    Private Function InitPSCommunication() As Boolean
        Dim iniReader As IniReader = New IniReader
        Dim hostname As String
        Dim port As String
        Dim result As String = String.Empty
        Dim returnValue As Boolean = False
        hostname = iniReader.ReadValueFromFile("Power Supply", "address", "", ".\Settings.ini")
        port = iniReader.ReadValueFromFile("Power Supply", "port", "", ".\Settings.ini")
        smh.powerSupply = New HardwareCommunication.TcpKeysightN5767Communication(hostname.Replace(" ", ""), Convert.ToInt32(port))
        smh.powerSupply.getIdentification(result)
        If String.IsNullOrEmpty(result) Then
            lblPSStatus.BackColor = Color.Red
        Else
            lblPSStatus.BackColor = Color.LightGreen
            returnValue = True
        End If
        Return returnValue
    End Function

    Private Function InitCanCommunication() As Boolean
        Dim returnValue As Boolean = False
        Try
            If IsNothing(smh.HeliosCommunicationBoard) Then
                smh.HeliosCommunicationBoard = New HELIOSCommunication.HELIOSCommunication(TPCANBaudrate.PCAN_BAUD_250K, TPCANType.PCAN_TYPE_ISA)

                If Not (smh.HeliosCommunicationBoard.Initialized) Then
                    If (MsgBox("No CAN communication available. Please check the connection!", MsgBoxStyle.RetryCancel + MsgBoxStyle.Exclamation + vbDefaultButton1) = MsgBoxResult.Retry) Then
                        InitCanCommunication()
                    Else
                        Me.Close()
                    End If
                End If
            End If
            returnValue = True
        Catch ex As Exception
            returnValue = True
            'WriteErrorToLogFile("Error Init Can Communication")
        End Try
        Return returnValue
    End Function

    Private Function InitBoxCommunication() As Boolean
        Dim returnValue As Boolean = False
        Try
            Dim comPort As String
            Dim iniReader As IniReader = New IniReader
            comPort = iniReader.ReadValueFromFile("COM", "port", "", ".\Settings.ini")
            Dim myProtocol As HardwareCommunication.ProtocolEOLBox = New HardwareCommunication.ProtocolEOLBox
            Try

                myProtocol.Open(comPort)
                Dim version As String = String.Empty
                myProtocol.GetVersion(version)
                If version.Length > 5 Then
                    lblBoxStatus.BackColor = Color.LightGreen
                Else
                    lblBoxStatus.BackColor = Color.Red
                End If
                If myProtocol.IsOpen() Then
                    myProtocol.Close()
                End If
            Catch ex As Exception
                lblBoxStatus.BackColor = Color.Red
            End Try
            returnValue = True
        Catch ex As Exception
            returnValue = True
            'WriteErrorToLogFile("Error Init Can Communication")
        End Try
        Return returnValue
    End Function

    Private Function InitCasCommunication() As Boolean
        Dim cal As String
        Dim casresult As Boolean
        Dim returnValue As Boolean = False
        Dim deviceTypeName, deviceTypeNameOption, confFile, calibFile As String
        Dim iniReader As IniReader = New IniReader
        cal = Space(32)
        deviceTypeName = iniReader.ReadValueFromFile("Cas140", "deviceTypeName", "", ".\Settings.ini")
        deviceTypeNameOption = iniReader.ReadValueFromFile("Cas140", "deviceTypeNameOption", "", ".\Settings.ini")
        confFile = iniReader.ReadValueFromFile("Cas140", "confFile", "", ".\Settings.ini")
        calibFile = iniReader.ReadValueFromFile("Cas140", "calFile", "", ".\Settings.ini")

        Dim interfaceName() As String
        Dim interfaceFound As Boolean = False
        Dim optionName() As String
        Dim optionFound As Boolean = False

        lblCasStatus.BackColor = Color.Red

        interfaceName = smh.m_cas140.ReadDeviceTypeNames
        For i = 0 To UBound(interfaceName)
            If interfaceName(i) = deviceTypeName Then
                smh.m_cas140.SetDeviceTypebyIndex(i)
                interfaceFound = True
            End If
        Next

        If interfaceFound Then
            optionName = smh.m_cas140.ReadDeviceTypeOptionNames
            If Not IsNothing(optionName) Then
                For i = 0 To UBound(optionName)
                    If optionName(i) = deviceTypeNameOption Then
                        smh.m_cas140.SetDeviceTypeOptionbyIndex(i)
                        optionFound = True
                    End If
                Next
            End If
        End If

        If optionFound Then
            casresult = smh.m_cas140.Init(cal, (confFile), (calibFile))
            If casresult = True Then

                smh.CasConfig.ConfigurationFile = confFile
                smh.CasConfig.CalibrationFile = calibFile
                smh.CasConfig.DeviceType = smh.m_cas140.DeviceType
                lblCasStatus.BackColor = Color.LightGreen
                returnValue = True
            End If
        End If
        Return returnValue
    End Function
#End Region

#Region "StatusText Update Handle"
    Public Delegate Sub updateStatusTextDelegate(ByVal status As StateMachineStatus)

    Private Sub smh_NewStatusText(sender As Object, ByVal status As StateMachineStatus) Handles smh.newStateMachineStatus
        Try
            'Alternative um einen Handler zu erzeugen
            'AddHandler smh.testEvent, AddressOf test
            Dim dele As updateStatusTextDelegate = AddressOf updateStatusText
            Me.Invoke(dele, status)
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub updateStatusText(ByVal status As StateMachineStatus)
        Static oldText As String

        If Strings.StrComp(Strings.Left(oldText, 10), Strings.Left(status.NewText, 10)) <> 0 Then

            oldText = status.NewText
            rtbOverview.SelectionStart = 0
            rtbOverview.SelectionLength = 0
            rtbOverview.SelectedText = status.NewText & vbTab & vbTab & vbTab & vbCrLf
            rtbOverview.SelectionStart = 0
            rtbOverview.SelectionLength = status.NewText.Length + 3
            rtbOverview.SelectionBackColor = status.NewTextColor
            tbStatus.Text = status.NewText
            pBarCalib.Value = status.Progress * 100

            Me.pBarCalib.Update()
            Me.rtbOverview.Update()
            Me.tbStatus.Update()
        End If

        If Strings.StrComp(oldText, status.NewText) <> 0 Then
            oldText = status.NewText
            tbStatus.Text = status.NewText
            Me.tbStatus.Update()
        End If
    End Sub
#End Region

#Region "Add LogFile Handle"
    Public Delegate Sub addLogFileDelegate(ByVal text As String)

    Private Sub smh_AddLogFile(sender As Object, ByVal addText As String) Handles smh.addLogFile
        Try
            'Alternative um einen Handler zu erzeugen
            'AddHandler smh.testEvent, AddressOf test
            Dim dele As addLogFileDelegate = AddressOf addLogFile
            Me.Invoke(dele, addText)
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub addLogFile(ByVal addText As String)
        Dim strWriter As System.IO.StreamWriter
        strWriter = My.Computer.FileSystem.OpenTextFileWriter(My.Computer.FileSystem.CurrentDirectory & "\logFile_" & _
                                                              Now.Date.Year.ToString("D4") & _
                                                              Now.Date.Month.ToString("D2") & _
                                                              Now.Date.Day.ToString("D2") & ".txt", True)
        strWriter.WriteLine(addText)
        strWriter.Close()
    End Sub
#End Region

#Region "Change StatusLights Handle"
    Public Delegate Sub changeStatusLightDelegate(ByVal statusLight As StateMachineEndOfLine.statusLight)

    Private Sub smh_ChangeStatusLight(sender As Object, ByVal statusLight As StateMachineEndOfLine.statusLight) Handles smh.changeStatusLight
        Try
            'Alternative um einen Handler zu erzeugen
            'AddHandler smh.testEvent, AddressOf test
            Dim dele As changeStatusLightDelegate = AddressOf changeStatusLight
            Me.Invoke(dele, statusLight)
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub changeStatusLight(ByVal statusLight As StateMachineEndOfLine.statusLight)
        Select Case statusLight
            Case StateMachineEndOfLine.statusLight.Green
                SetLightRed(False, String.Empty)
                SetLightYellow(False, String.Empty)
                SetLightGreen(True, "Messung erfolgreich beendet")
            Case StateMachineEndOfLine.statusLight.Yellow
                SetLightRed(False, String.Empty)
                SetLightYellow(True, "in Bearbeitung")
                SetLightGreen(False, String.Empty)
            Case StateMachineEndOfLine.statusLight.Red
                SetLightRed(True, "mit Fehler beendet")
                SetLightYellow(False, String.Empty)
                SetLightGreen(False, String.Empty)
        End Select
    End Sub
#End Region

    Private Function showErrorMessageBox(message As String, type As String) As System.Windows.Forms.DialogResult
        Return MessageBox.Show(message, type, _
                 MessageBoxButtons.RetryCancel, _
                 MessageBoxIcon.Exclamation, _
                 MessageBoxDefaultButton.Button1)
    End Function

    Private Sub btRestartHelios_Click(sender As Object, e As EventArgs) Handles btRestartHelios.Click
        Try
            smh.HeliosCommunicationBoard.ExecuteRestart(StateMachineEndOfLine.CAN_DEST)
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnStartMeasurement_Click(sender As Object, e As EventArgs) Handles btnStartMeasurement.Click
        Try
            Dim filename As String
            filename = "programFlow_" & tbxModuleConfig.Text & ".txt"
            'todo: naechste Zeile sollte wieder entfernt werden!!!
            dgvSerialNumberRGB.Enabled = True
            btnBackToSerNr.Enabled = False
            Me.startCalibrationHelios(filename)
            btnBackToSerNr.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnStopMeasurement_Click(sender As Object, e As EventArgs) Handles btnStopMeasurement.Click
        Try
            While smh.ProgramFlow.Count > 1
                smh.ProgramFlow.Clear()
            End While
            addLogFile("%%% SN" & smh.smData.CZMSeriennummer.ToString() & " " &
                                              Now.Date.Year.ToString("D4") &
                                              Now.Date.Month.ToString("D2") &
                                              Now.Date.Day.ToString("D2") & " " &
                                              Now.Hour.ToString("D2") &
                                              Now.Minute.ToString("D2") &
                                              Now.Second.ToString("D2") & " " &
                                              smh.CurrentStep.ToString & "/" & smh.MaxSteps.ToString & " User Aborted %%%" & vbNewLine)
            smh.Status = StateMachineEndOfLine.smhStatus.FinishBad
            btnStartMeasurement.Enabled = True
            btnStopMeasurement.Enabled = False
            btnBackToSerNr.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub btnProperties_Click(sender As Object, e As EventArgs) Handles btnProperties.Click
        Try
            Dim frmSettings As HEOLCommonSettings = New HEOLCommonSettings()
            Dim returnString As String
            returnString = InputBox("Bitte Einrichter-Passwort eingeben", "Passwortabfrage", "", 100, 100)
            If returnString = "123" Then
                frmSettings.ShowDialog()
            Else
                'wrong password
            End If
            DecideVisibiltySection12(InitSoftware(), InitHardware())
            tbxSerialNumber.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub cBxModuleType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cBxModuleType.SelectedIndexChanged
        Try
            'change config List according possible Configs
            Dim iniReader As IniReader = New IniReader
            Dim sectionConfigFile As String
            Select Case cBxModuleType.SelectedIndex
                Case 0     ' RGB-Modul Kalibrierung
                    sectionConfigFile = "ConfigRGB"
                    lblVSerialNumber.Visible = False
                    tbxVSerialNumber.Visible = False
                Case 1   ' V-Modul Kalibrierung
                    sectionConfigFile = "ConfigV"
                    lblVSerialNumber.Visible = True
                    tbxVSerialNumber.Visible = True
                Case 2     ' RGB-Modul Abschlussmessung
                    sectionConfigFile = "ConfigRGB2"
                    lblVSerialNumber.Visible = False
                    tbxVSerialNumber.Visible = False
                Case 3   ' V-Modul Abschlussmessung
                    sectionConfigFile = "ConfigV2"
                    lblVSerialNumber.Visible = True
                    tbxVSerialNumber.Visible = True
                Case 4     ' RGB-Modul Finalmessung
                    sectionConfigFile = "ConfigRGB3"
                    lblVSerialNumber.Visible = False
                    tbxVSerialNumber.Visible = False
                Case Else
                    sectionConfigFile = ""
            End Select

            Dim tempText As String
            tempText = iniReader.ReadValueFromFile(sectionConfigFile, "config", "", ".\Settings.ini")
            If tempText <> String.Empty Then
                tbxModuleConfig.Text = tempText
            End If
            tbxSerialNumber.Focus()
            tbxSerialNumber.Text = String.Empty
            tbxVSerialNumber.Text = String.Empty
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnMeasure_Click(sender As Object, e As EventArgs) Handles btnMeasureAgain.Click
        Try
            enabledOfSection1(False)
            enabledOfSection2(False)
            visibleOfSection3(True)
            btnStartMeasurement.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnBackToSerNr_Click(sender As Object, e As EventArgs) Handles btnBackToSerNr.Click
        Try
            enabledOfSection1(True)
            enabledOfSection2(True)
            visibleOfSection2(False)
            visibleOfSection3(False)
            btnStartMeasurement.Enabled = True
            pBarCalib.Value = 0
            btnStopMeasurement.Enabled = False
            'Load if reference measurement is necessary at the beginning
            Dim refOK As Boolean = True
            Dim hardwareOK As Boolean
            'init and check Hardware
            hardwareOK = InitHardware()
            DecideVisibiltySection12(refOK, hardwareOK)
            tbxSerialNumber.Text = String.Empty
            tbxVSerialNumber.Text = String.Empty
            dgvSerialNumberRGB.Rows.Clear()
            tbxSerialNumber.Focus()
            rtbOverview.Text = String.Empty
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub tbxSerialNumber_KeyDown(sender As Object, e As KeyEventArgs) Handles tbxSerialNumber.KeyDown
        Try
            serialNumberKeyDown(e)
            If e.KeyCode = Keys.Enter Then
                Select Case cBxModuleType.SelectedIndex
                    Case 1, 3    'V-Modul
                        If tbxVSerialNumber.Text = String.Empty Then
                            tbxVSerialNumber.Focus()
                        End If
                End Select
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub tbxVSerialNumber_KeyDown(sender As Object, e As KeyEventArgs) Handles tbxVSerialNumber.KeyDown
        Try
            serialNumberKeyDown(e)
            If e.KeyCode = Keys.Enter Then
                If tbxSerialNumber.Text = String.Empty Then
                    tbxSerialNumber.Focus()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub serialNumberKeyDown(ByRef e As KeyEventArgs)
        Dim foundInDB As Boolean = False
        Dim alreadyCalibrated As Boolean = False
        Dim SNvalid As Boolean = True
        If e.KeyCode = Keys.Enter Then

            'Suche in der Datenbank nach diesem Wert
            Select Case cBxModuleType.SelectedIndex
                Case 0   'Kalibrierung RGB-Modul (Bei Messungen-only wird SN nicht überprüft)
                    'Seriennummer auf Plausibilität prüfen:
                    If (tbxSerialNumber.Text.Length <> 23) Or (Not tbxSerialNumber.Text.StartsWith("@02AB453300004")) Then
                        SNvalid = False
                    Else
                        findSNInDataBase(tbxSerialNumber.Text, False, foundInDB, alreadyCalibrated)
                    End If
                Case 1   'Kalibrierung V-Modul (Bei Messungen-only wird SN nicht überprüft)
                    'TODO Seriennummer V-Modul /Datenbank
                    If (tbxSerialNumber.Text <> String.Empty) And (tbxVSerialNumber.Text <> String.Empty) Then
                        If (tbxVSerialNumber.Text.Length <> 23) Or (Not tbxVSerialNumber.Text.StartsWith("@02AB453280004")) _
                           Or (Not tbxSerialNumber.Text.StartsWith("@02AB453300004V-TESTER")) Then
                            SNvalid = False
                        Else
                            findSNInDataBase(tbxVSerialNumber.Text, True, foundInDB, alreadyCalibrated)
                            dgvSerialNumberRGB.Item(2, 0).Value = CUInt(tbxVSerialNumber.Text.Substring(14)).ToString
                        End If

                        ' CAMSTAR 
                        'Dim iniReader As IniReader = New IniReader
                        'If iniReader.ReadValueFromFile("EnableSection", "CamstarSN", "", ".\Settings.ini") = "1" Then
                        '    foundInDB = True
                        'End If
                    End If
                Case Else
                    foundInDB = True   ' Database disabled
                    SNvalid = True ' No plausibility check
            End Select



            tbxSerialNumber.Update()
            tbxVSerialNumber.Update()
            'Test if everything is OK
            If SNvalid = False Then
                lblStatusInDB.Visible = True
                lblStatusInDB.Text = "Modul-Seriennummer ungültig"
                btnMeasureAgain.Visible = False
            ElseIf foundInDB = True Then
                If (alreadyCalibrated = True) Then
                    lblStatusInDB.Visible = True
                    lblStatusInDB.Text = "Modul wurde schon einmal vermessen!"
                    btnMeasureAgain.Visible = True
                Else
                    enabledOfSection1(False)
                    enabledOfSection2(False)
                    'todo: naechste Zeile sollte wieder entfernt werden
                    dgvSerialNumberRGB.Enabled = True
                    visibleOfSection3(True)
                    tbStatus.Text = String.Empty
                    rtbOverview.Text = String.Empty
                    lblStatusInDB.Visible = False
                    btnStartMeasurement.Focus()
                End If
            Else
                lblStatusInDB.Visible = True
                lblStatusInDB.Text = "Modul existiert nicht in der Datenbank"
            End If
        End If
    End Sub

#Region "Visible and Enable functions of section 1, 2, 3"
    Private Sub visibleOfSection1a(ByVal visible As Boolean)
        lblModuleConfig.Visible = visible
        lblModuleType.Visible = visible
        tbxModuleConfig.Visible = visible
        cBxModuleType.Visible = visible
    End Sub
    Private Sub visibleOfSection1b(ByVal visible As Boolean)
        btnDoRefMeas.Visible = visible
    End Sub
    Private Sub visibleOfSection2(ByVal visible As Boolean)
        Me.tbxSerialNumber.Visible = visible
        Me.lblSerialNumber.Visible = visible
        Me.dgvSerialNumberRGB.Visible = visible
        Me.btnMeasureAgain.Visible = False
        Me.lblStatusInDB.Visible = False
    End Sub
    Private Sub visibleOfSection3(ByVal visible As Boolean)
        Me.btnStartMeasurement.Visible = visible
        Me.btnStopMeasurement.Visible = visible
        Me.btnBackToSerNr.Visible = visible
        visibleOfSection3b(visible)
    End Sub
    Private Sub visibleOfSection3b(ByVal visible As Boolean)
        Me.tbStatus.Visible = visible
        Me.rtbOverview.Visible = visible
        Me.pBarCalib.Visible = visible
        Me.panRed.Visible = visible
        Me.panYellow.Visible = visible
        Me.panGreen.Visible = visible
        Me.panRed.BackColor = Color.Empty
        Me.panYellow.BackColor = Color.Empty
        Me.panGreen.BackColor = Color.Empty
        Me.lblPanRed.Visible = visible
        Me.lblPanYellow.Visible = visible
        Me.lblPanGreen.Visible = visible
        Me.lblPanRed.Text = String.Empty
        Me.lblPanYellow.Text = String.Empty
        Me.lblPanGreen.Text = String.Empty
    End Sub
    Private Sub enabledOfSection1(ByVal enabled As Boolean)
        tbxModuleConfig.Enabled = enabled
        cBxModuleType.Enabled = enabled
        btnProperties.Enabled = enabled
    End Sub
    Private Sub enabledOfSection2(ByVal enabled As Boolean)
        tbxSerialNumber.Enabled = enabled
        tbxVSerialNumber.Enabled = enabled
        dgvSerialNumberRGB.Enabled = enabled
        btnMeasureAgain.Enabled = enabled
    End Sub
#End Region

#Region "Database functions"
    Private Function findSNInDataBase(ByVal sn As String, ByVal vModule As Boolean, ByRef foundInDB As Boolean, ByRef alreadyCalibrated As Boolean)
        Dim returnValue As Boolean = True

        dgvSerialNumberRGB.Rows.Clear()

        If Not vModule Then
            For i = 0 To 6
                dgvSerialNumberRGB.Rows.Add()
            Next i
            dgvSerialNumberRGB.CurrentCell = dgvSerialNumberRGB.Rows(0).Cells(0)
            dgvSerialNumberRGB.BeginEdit(True)
            dgvSerialNumberRGB.Item(0, 0).Value = "Main Board"
            dgvSerialNumberRGB.Item(0, 1).Value = "Capacitor Board"
            dgvSerialNumberRGB.Item(0, 2).Value = "Sensor Board"
            dgvSerialNumberRGB.Item(0, 3).Value = "Connector Board"
            dgvSerialNumberRGB.Item(0, 4).Value = "LED R Board"
            dgvSerialNumberRGB.Item(0, 5).Value = "LED G Board"
            dgvSerialNumberRGB.Item(0, 6).Value = "LED B Board"

            dgvSerialNumberRGB.Item(1, 0).Value = "D03077248"
            dgvSerialNumberRGB.Item(1, 1).Value = "D03077250"
            dgvSerialNumberRGB.Item(1, 2).Value = "D03124099"
            dgvSerialNumberRGB.Item(1, 3).Value = "D03077252"
            dgvSerialNumberRGB.Item(1, 4).Value = "D03124090"
            dgvSerialNumberRGB.Item(1, 5).Value = "D03124093"
            dgvSerialNumberRGB.Item(1, 6).Value = "D03124096"

            dgvSerialNumberRGB.Item(3, 0).Value = String.Empty
            dgvSerialNumberRGB.Item(3, 0).ReadOnly = True
            dgvSerialNumberRGB.Item(3, 1).Value = String.Empty
            dgvSerialNumberRGB.Item(3, 1).ReadOnly = True
            dgvSerialNumberRGB.Item(3, 2).Value = String.Empty
            dgvSerialNumberRGB.Item(3, 2).ReadOnly = True
            dgvSerialNumberRGB.Item(3, 3).Value = String.Empty
            dgvSerialNumberRGB.Item(3, 3).ReadOnly = True
            dgvSerialNumberRGB.Item(3, 4).Value = "A3"
            dgvSerialNumberRGB.Item(3, 5).Value = "A2"
            dgvSerialNumberRGB.Item(3, 6).Value = "A2"

            'Try
            '    System.IO.File.Delete("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn)  'Delete old result if exists
            'Catch ex As Exception
            '    'ignore
            'End Try

            'Try
            '    System.IO.File.Create("C:\CamstarDataExchange\EOLTest\GetComponents\Input\" & sn).Close()
            '    Threading.Thread.Sleep(500)
            'Catch ex As Exception
            '    'ignore
            'End Try

            'If System.IO.File.Exists("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn) Then
            '    Dim stream As System.IO.StreamReader = Nothing

            '    Try
            '        stream = System.IO.File.OpenText("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn)
            '        While stream.Peek() >= 0
            '            Dim text As String = stream.ReadLine
            '            Dim splitText As String() = text.Split("=")
            '            Dim s As String
            '            If splitText.Count <> 2 Then Continue While
            '            Select Case splitText(0)
            '                Case "105303510100"     'Main Board
            '                    s = splitText(1)
            '                    If s.Length = 24 Then
            '                        dgvSerialNumberRGB.Item(2, 0).Value = s.Substring(14, 6)
            '                    End If
            '                Case "105303520100"     'Capacitor Board
            '                    s = splitText(1)
            '                    If s.Length = 24 Then
            '                        dgvSerialNumberRGB.Item(2, 1).Value = s.Substring(14, 6)
            '                    End If
            '                Case "105505730000"     'LED R Board
            '                    s = splitText(1)
            '                    splitText = s.Split("_")
            '                    If splitText.Count >= 4 Then
            '                        If splitText(0) = "105505730000" Then
            '                            dgvSerialNumberRGB.Item(2, 4).Value = splitText(1)
            '                            dgvSerialNumberRGB.Item(3, 4).Value = splitText(3)
            '                        End If
            '                    End If
            '                Case "105505740000"     'LED G Board
            '                    s = splitText(1)
            '                    splitText = s.Split("_")
            '                    If splitText.Count >= 4 Then
            '                        If splitText(0) = "105505740000" Then
            '                            dgvSerialNumberRGB.Item(2, 5).Value = splitText(1)
            '                            dgvSerialNumberRGB.Item(3, 5).Value = splitText(3)
            '                        End If
            '                    End If
            '                Case "105505750000"     'LED B Board
            '                    s = splitText(1)
            '                    splitText = s.Split("_")
            '                    If splitText.Count >= 4 Then
            '                        If splitText(0) = "105505750000" Then
            '                            dgvSerialNumberRGB.Item(2, 6).Value = splitText(1)
            '                            dgvSerialNumberRGB.Item(3, 6).Value = splitText(3)
            '                        End If
            '                    End If
            '                Case "105505760100"     'Sensor Board
            '                    s = splitText(1)
            '                    If s.Length = 24 Then
            '                        dgvSerialNumberRGB.Item(2, 2).Value = s.Substring(14, 6)
            '                    End If
            '                Case Else
            '            End Select
            '        End While
            '        stream.Close()
            '        foundInDB = True
            '        System.IO.File.Delete("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn)
            '    Catch ex As Exception
            '        'ignore
            '    Finally
            '        If Not stream Is Nothing Then stream.Close()
            '    End Try
            'End If
            foundInDB = True ' CAMSTAR Database Disabled
            dgvSerialNumberRGB.Item(2, 3).Value = "0"
        Else
            dgvSerialNumberRGB.Rows.Add()
            dgvSerialNumberRGB.CurrentCell = dgvSerialNumberRGB.Rows(0).Cells(0)
            dgvSerialNumberRGB.BeginEdit(True)
            dgvSerialNumberRGB.Item(0, 0).Value = "VBoard"
            dgvSerialNumberRGB.Item(1, 0).Value = "D30011787"
            dgvSerialNumberRGB.Item(3, 0).Value = "00"

            'Try
            '    System.IO.File.Delete("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn)  'Delete old result if exists
            'Catch ex As Exception
            '    'ignore
            'End Try

            'Try
            '    System.IO.File.Create("C:\CamstarDataExchange\EOLTest\GetComponents\Input\" & sn).Close()
            '    Threading.Thread.Sleep(500)
            'Catch ex As Exception
            '    'ignore
            'End Try

            'If System.IO.File.Exists("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn) Then
            '    Dim stream As System.IO.StreamReader = Nothing

            '    Try
            '        stream = System.IO.File.OpenText("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn)
            '        While stream.Peek() >= 0
            '            Dim text As String = stream.ReadLine
            '            Dim splitText As String() = text.Split("=")
            '            Dim s As String
            '            If splitText.Count <> 2 Then Continue While
            '            Select Case splitText(0)
            '                Case "105867930000"     'LED V Board
            '                    s = splitText(1)
            '                    splitText = s.Split("_")
            '                    If splitText.Count >= 4 Then
            '                        If splitText(0) = "105867930000" Then
            '                            'dgvSerialNumberRGB.Item(2, 4).Value = splitText(1)
            '                            dgvSerialNumberRGB.Item(3, 0).Value = splitText(3)
            '                        End If
            '                    End If
            '            End Select
            '        End While
            '        stream.Close()
            '        foundInDB = True
            '        System.IO.File.Delete("C:\CamstarDataExchange\EOLTest\GetComponents\Output\" & sn)
            '    Catch ex As Exception
            '        'ignore
            '    Finally
            '        If Not stream Is Nothing Then stream.Close()
            '    End Try
            'End If
            foundInDB = True ' CAMSTAR Database Disabled
            dgvSerialNumberRGB.Item(2, 3).Value = "0"
        End If
        dgvSerialNumberRGB.Update()

        'alreadyCalibrated = True
        Return returnValue
    End Function
#End Region


    Private Sub cBxModuleConfig_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            tbxSerialNumber.Focus()
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SetLightRed(ByVal value As Boolean, ByVal text As String)
        If value = True Then
            panRed.BackColor = Color.Red
        Else
            panRed.BackColor = Color.Empty
        End If
        lblPanRed.Text = text
    End Sub

    Private Sub SetLightYellow(ByVal value As Boolean, ByVal text As String)
        If value = True Then
            panYellow.BackColor = Color.Yellow
        Else
            panYellow.BackColor = Color.Empty
        End If
        lblPanYellow.Text = text
    End Sub

    Private Sub SetLightGreen(ByVal value As Boolean, ByVal text As String)
        If value = True Then
            panGreen.BackColor = Color.LimeGreen
        Else
            panGreen.BackColor = Color.Empty
        End If
        lblPanGreen.Text = text
    End Sub

    Protected Friend Function DoRefMeas(ModuleType As Integer) As Boolean
        Dim result As Boolean = False
        Dim smData As StateMachineEndOfLineData = New StateMachineEndOfLineData
        smData.ModuleType = ModuleType
        btnDoRefMeas.Enabled = False

        visibleOfSection3b(True)
        btnStopMeasurement.Text = "Referenzmessung abbrechen"
        btnStopMeasurement.Visible = True
        btnStopMeasurement.Enabled = True
        tbStatus.Text = String.Empty
        rtbOverview.Text = String.Empty


        changeStatusLight(StateMachineEndOfLine.statusLight.Yellow)

        smh.LoadProgramFlowHelios("programFlowRefMeas.txt")
        smh.SetData(smData)
        smh.ExecuteProgramFlowHelios()

        While (smh.Status = StateMachineEndOfLine.smhStatus.WorkingGood) Or (smh.Status = StateMachineEndOfLine.smhStatus.WorkingBad)
            System.Threading.Thread.Sleep(20)
            Application.DoEvents()
        End While

        If smh.Status = StateMachineEndOfLine.smhStatus.FinishGood Then
            result = True
            changeStatusLight(StateMachineEndOfLine.statusLight.Green)
        Else
            changeStatusLight(StateMachineEndOfLine.statusLight.Red)
        End If

        btnDoRefMeas.Enabled = True
        btnStopMeasurement.Text = "Kalibrierung abbrechen"
        btnStopMeasurement.Visible = False
        btnStopMeasurement.Enabled = False
        DecideVisibiltySection12(InitSoftware(), InitHardware())
        tbxSerialNumber.Focus()

        Return result
    End Function

    Private Sub btnDoRefMeas_Click(sender As Object, e As EventArgs) Handles btnDoRefMeas.Click
        Try
            Dim strDate As String = String.Empty
            strDate = Now.ToString()
            Dim iniReader As IniReader = New IniReader

            If DoRefMeas(-1) Then
                iniReader.WriteValueToFile("RefMeasurement", "date", strDate, ".\Settings.ini")
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

End Class
