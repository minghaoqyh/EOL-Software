Public Class HEOLCommonSettings
    Private cas140 As CasCommunication.cCAS140
    Private casConfig As CasCommunication.CAS140IniFile
    Private deviceTypeName As String = String.Empty
    Private deviceTypeNameOption As String = String.Empty
    Private interfaceName() As String
    Private optionName() As String

    Private Sub btnConfFile_Click(sender As Object, e As EventArgs) Handles btnConfFile.Click
        Try
            OpenFile(tbxConfFile, "Configuration Datei")
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCalibFile_Click(sender As Object, e As EventArgs) Handles btnCalibFile.Click
        Try
            OpenFile(tbxCalFile, "Calibration Datei")
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub OpenFile(ByRef elemTextbox As TextBox, ByVal whichData As String)
        Dim ofd As New OpenFileDialog
        ofd.DefaultExt = "txt"
        If whichData.Contains("Configuration") = True Then
            ofd.Filter = "Configuration Files(*.ini)|*.ini|Text files (*.txt)|*.txt"
        Else
            ofd.Filter = "Calibration Files(*.isc)|*.isc|Text files (*.txt)|*.txt"
        End If

        ofd.CheckFileExists = True
        ofd.Title = whichData & " einlesen"
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            elemTextbox.Text = ofd.FileName.ToString
        End If

    End Sub

    Private Sub cmbInterface_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbInterface.SelectedIndexChanged
        Try
            Dim triggerDeviceTypeNameOK As Boolean
            triggerDeviceTypeNameOK = False

            For i = 0 To UBound(interfaceName)
                If cmbInterface.SelectedItem = interfaceName(i) Then
                    triggerDeviceTypeNameOK = True
                End If
            Next i

            If triggerDeviceTypeNameOK Then
                cmbInterface.BackColor = Color.LightGreen
            Else
                cmbInterface.BackColor = Color.Red
            End If

            'Fill DeviceTypeNameOptionBox with possible solutions and the value from the ini file
            cas140.SetDeviceTypebyIndex(cmbInterface.SelectedIndex)
            optionName = cas140.ReadDeviceTypeOptionNames
            cmbInterfaceOption.Items.Clear()

            Dim triggerDeviceTypeNameOptionOK As Boolean
            triggerDeviceTypeNameOptionOK = False
            If Not IsNothing(optionName) Then
                For i = 0 To UBound(optionName)
                    cmbInterfaceOption.Items.Add(optionName(i))
                    If deviceTypeNameOption = optionName(i) Then
                        triggerDeviceTypeNameOptionOK = True
                        cmbInterfaceOption.SelectedIndex = i
                    End If
                Next i
            End If
            If Not triggerDeviceTypeNameOptionOK Then
                cmbInterfaceOption.Items.Add(deviceTypeNameOption)
                cmbInterfaceOption.SelectedIndex = cmbInterfaceOption.Items.Count - 1
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim iniReader As IniReader = New IniReader()
            iniReader.WriteValueToFile("Power Supply", "address", mtbxPSIPAddress.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("Power Supply", "port", tbxPSPort.Text, ".\Settings.ini")

            iniReader.WriteValueToFile("Cas140", "deviceTypeName", cmbInterface.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("Cas140", "deviceTypeNameOption", cmbInterfaceOption.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("Cas140", "confFile", tbxConfFile.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("Cas140", "calFile", tbxCalFile.Text, ".\Settings.ini")

            iniReader.WriteValueToFile("DIO", "deviceName", tBxDeviceName.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("DIO", "jumper", mtbJumper.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("DIO", "safetyBox", mtbSafetyBox.Text, ".\Settings.ini")

            iniReader.WriteValueToFile("ConfigRGB", "config", tbxConfigRGB.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("ConfigV", "config", tbxConfigV.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("ConfigRGB2", "config", tbxConfigRGB2.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("ConfigV2", "config", tbxConfigV2.Text, ".\Settings.ini")
            iniReader.WriteValueToFile("ConfigRGB3", "config", tbxConfigRGB3.Text, ".\Settings.ini")

            iniReader.WriteValueToFile("COM", "port", cmbComport.Text, ".\Settings.ini")

            iniReader.WriteValueToFile("DataStorage", "path", tbxStoragePath.Text, ".\Settings.ini")

            If checkBxCAN.Checked = True Then
                iniReader.WriteValueToFile("EnableSection", "CAN", "1", ".\Settings.ini")
            Else
                iniReader.WriteValueToFile("EnableSection", "CAN", "0", ".\Settings.ini")
            End If
            If checkBxCAS.Checked = True Then
                iniReader.WriteValueToFile("EnableSection", "CAS", "1", ".\Settings.ini")
            Else
                iniReader.WriteValueToFile("EnableSection", "CAS", "0", ".\Settings.ini")
            End If
            If checkBxPS.Checked = True Then
                iniReader.WriteValueToFile("EnableSection", "PS", "1", ".\Settings.ini")
            Else
                iniReader.WriteValueToFile("EnableSection", "PS", "0", ".\Settings.ini")
            End If
            If checkBxDIO.Checked = True Then
                iniReader.WriteValueToFile("EnableSection", "DIO", "1", ".\Settings.ini")
            Else
                iniReader.WriteValueToFile("EnableSection", "DIO", "0", ".\Settings.ini")
            End If
            If checkBxBox.Checked = True Then
                iniReader.WriteValueToFile("EnableSection", "Box", "1", ".\Settings.ini")
            Else
                iniReader.WriteValueToFile("EnableSection", "Box", "0", ".\Settings.ini")
            End If
            If chkCamstarSN.Checked = True Then
                iniReader.WriteValueToFile("EnableSection", "CamstarSN", "1", ".\Settings.ini")
            Else
                iniReader.WriteValueToFile("EnableSection", "CamstarSN", "0", ".\Settings.ini")
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Try
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub HEOLCommonSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cas140 = New CasCommunication.cCAS140
            casConfig = New CasCommunication.CAS140IniFile

            'Read data from ini file and put it into the corresponding textboxes
            Dim iniReader As IniReader = New IniReader()
            If IO.File.Exists(".\Settings.ini") Then
                mtbxPSIPAddress.Text = iniReader.ReadValueFromFile("Power Supply", "address", "", ".\Settings.ini")
                tbxPSPort.Text = iniReader.ReadValueFromFile("Power Supply", "port", "", ".\Settings.ini")

                deviceTypeName = iniReader.ReadValueFromFile("Cas140", "deviceTypeName", "", ".\Settings.ini")
                deviceTypeNameOption = iniReader.ReadValueFromFile("Cas140", "deviceTypeNameOption", "", ".\Settings.ini")
                tbxConfFile.Text = iniReader.ReadValueFromFile("Cas140", "confFile", "", ".\Settings.ini")
                tbxCalFile.Text = iniReader.ReadValueFromFile("Cas140", "calFile", "", ".\Settings.ini")

                tBxDeviceName.Text = iniReader.ReadValueFromFile("DIO", "deviceName", "", ".\Settings.ini")
                mtbJumper.Text = iniReader.ReadValueFromFile("DIO", "jumper", "", ".\Settings.ini")
                mtbSafetyBox.Text = iniReader.ReadValueFromFile("DIO", "safetyBox", "", ".\Settings.ini")

                tbxConfigRGB.Text = iniReader.ReadValueFromFile("ConfigRGB", "config", "", ".\Settings.ini")
                tbxConfigV.Text = iniReader.ReadValueFromFile("ConfigV", "config", "", ".\Settings.ini")
                tbxConfigRGB2.Text = iniReader.ReadValueFromFile("ConfigRGB2", "config", "", ".\Settings.ini")
                tbxConfigV2.Text = iniReader.ReadValueFromFile("ConfigV2", "config", "", ".\Settings.ini")
                tbxConfigRGB3.Text = iniReader.ReadValueFromFile("ConfigRGB3", "config", "", ".\Settings.ini")

                tbxStoragePath.Text = iniReader.ReadValueFromFile("DataStorage", "path", "", ".\Settings.ini")

                If iniReader.ReadValueFromFile("EnableSection", "CAN", "", ".\Settings.ini") = "1" Then
                    checkBxCAN.Checked = True
                Else
                    checkBxCAN.Checked = False
                End If
                If iniReader.ReadValueFromFile("EnableSection", "CAS", "", ".\Settings.ini") = "1" Then
                    checkBxCAS.Checked = True
                Else
                    checkBxCAS.Checked = False
                End If
                If iniReader.ReadValueFromFile("EnableSection", "PS", "", ".\Settings.ini") = "1" Then
                    checkBxPS.Checked = True
                Else
                    checkBxPS.Checked = False
                End If
                If iniReader.ReadValueFromFile("EnableSection", "DIO", "", ".\Settings.ini") = "1" Then
                    checkBxDIO.Checked = True
                Else
                    checkBxDIO.Checked = False
                End If
                If iniReader.ReadValueFromFile("EnableSection", "Box", "", ".\Settings.ini") = "1" Then
                    checkBxBox.Checked = True
                Else
                    checkBxBox.Checked = False
                End If
                If iniReader.ReadValueFromFile("EnableSection", "CamstarSN", "", ".\Settings.ini") = "1" Then
                    chkCamstarSN.Checked = True
                Else
                    chkCamstarSN.Checked = False
                End If
            End If

            'COM Port Settings
            cmbComport.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames())
            cmbComport.SelectedIndex = 0
            Dim text As String
            text = iniReader.ReadValueFromFile("COM", "port", "", ".\Settings.ini")
            If Not cmbComport.Items.Contains(text) Then
                cmbComport.Items.Add(text)
            Else
                cmbComport.SelectedIndex = cmbComport.Items.IndexOf(text)
            End If

            'Fill DeviceTypeNameBox with possible solutions and the saved solution
            interfaceName = cas140.ReadDeviceTypeNames
            Dim triggerDeviceTypeNameOK As Boolean
            triggerDeviceTypeNameOK = False
            For i = 0 To UBound(interfaceName)
                cmbInterface.Items.Add(interfaceName(i))
                If deviceTypeName = interfaceName(i) Then
                    triggerDeviceTypeNameOK = True
                    cmbInterface.SelectedIndex = i
                End If
            Next i
            If Not triggerDeviceTypeNameOK Then
                cmbInterface.Items.Add(deviceTypeName)
                cmbInterface.SelectedIndex = cmbInterface.Items.Count - 1
            End If

            'DeviceTypeNameOption is filled in selection_changed of cmbInterface
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub cmbInterfaceOption_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbInterfaceOption.SelectedIndexChanged
        Try
            Dim triggerDeviceTypeNameOptionOK As Boolean
            triggerDeviceTypeNameOptionOK = False

            If Not IsNothing(optionName) Then
                For i = 0 To UBound(optionName)
                    If cmbInterfaceOption.SelectedItem = optionName(i) Then
                        triggerDeviceTypeNameOptionOK = True
                    End If
                Next i
            End If

            If triggerDeviceTypeNameOptionOK Then
                cmbInterfaceOption.BackColor = Color.LightGreen
            Else
                cmbInterfaceOption.BackColor = Color.Red
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCheckCas_Click(sender As Object, e As EventArgs) Handles btnCheckCas.Click
        Try
            Dim cal As String
            Dim casresult As Boolean
            cal = Space(32)
            cas140.SetDeviceTypebyIndex(cmbInterface.SelectedIndex)

            cas140.SetDeviceTypeOptionbyIndex(cmbInterfaceOption.SelectedIndex)
            casresult = cas140.Init(cal, (tbxConfFile.Text.ToString), (tbxCalFile.Text.ToString))
            If casresult Then
                btnCheckCas.BackColor = Color.LightGreen
            Else
                btnCheckCas.BackColor = Color.Red
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCheckPS_Click(sender As Object, e As EventArgs) Handles btnCheckPS.Click
        Try
            Dim powerSupply As HardwareCommunication.TcpKeysightN5767Communication = New HardwareCommunication.TcpKeysightN5767Communication(mtbxPSIPAddress.Text.Replace(" ", ""), tbxPSPort.Text)
            Dim result As String = String.Empty
            powerSupply.getIdentification(result)
            If String.IsNullOrEmpty(result) Then
                btnCheckPS.BackColor = Color.Red
            Else
                btnCheckPS.BackColor = Color.LightGreen
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCheckDIO_Click(sender As Object, e As EventArgs) Handles btnCheckDIO.Click
        Try
            Dim dio As HardwareCommunication.NI6520_DAQ = New HardwareCommunication.NI6520_DAQ()
            If Not dio.InitNI6520(tBxDeviceName.Text) Then
                btnCheckDIO.BackColor = Color.Red
            Else
                btnCheckDIO.BackColor = Color.LightGreen
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCheckConfig_Click(sender As Object, e As EventArgs) Handles btnCheckConfig.Click
        Try
            Dim filepathRGB As String = String.Empty
            Dim filepathV As String = String.Empty
            Dim filepathRGB2 As String = String.Empty
            Dim filepathV2 As String = String.Empty
            Dim filepathRGB3 As String = String.Empty
            filepathRGB = Application.StartupPath & "\programFlow_" & tbxConfigRGB.Text & ".txt"
            filepathV = Application.StartupPath & "\programFlow_" & tbxConfigV.Text & ".txt"
            filepathRGB2 = Application.StartupPath & "\programFlow_" & tbxConfigRGB2.Text & ".txt"
            filepathV2 = Application.StartupPath & "\programFlow_" & tbxConfigV2.Text & ".txt"
            filepathRGB3 = Application.StartupPath & "\programFlow_" & tbxConfigRGB3.Text & ".txt"
            If (System.IO.File.Exists(filepathRGB)) And (System.IO.File.Exists(filepathV)) And (System.IO.File.Exists(filepathRGB2)) And (System.IO.File.Exists(filepathV2)) And (System.IO.File.Exists(filepathRGB3)) Then
                btnCheckConfig.BackColor = Color.LightGreen
            Else
                btnCheckConfig.BackColor = Color.Red
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCheckCOM_Click(sender As Object, e As EventArgs) Handles btnCheckCOM.Click
        Try
            Dim myProtocol As HardwareCommunication.ProtocolEOLBox = New HardwareCommunication.ProtocolEOLBox
            Try

                myProtocol.Open(cmbComport.Text)
                Dim version As String = String.Empty
                myProtocol.GetVersion(version)
                If version.Length > 5 Then
                    btnCheckCOM.BackColor = Color.LightGreen
                Else
                    btnCheckCOM.BackColor = Color.Red
                End If
                If myProtocol.IsOpen() Then
                    myProtocol.Close()
                End If
            Catch ex As Exception
                btnCheckCOM.BackColor = Color.Red
            End Try
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnPSOff_Click(sender As Object, e As EventArgs) Handles btnPSOff.Click
        Try
            Dim powerSupply As HardwareCommunication.TcpKeysightN5767Communication = New HardwareCommunication.TcpKeysightN5767Communication(mtbxPSIPAddress.Text.Replace(" ", ""), tbxPSPort.Text)
            Dim result As String = String.Empty
            powerSupply.setOutputOnOff(False)
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnCheckStorage_Click(sender As Object, e As EventArgs) Handles btnCheckStorage.Click
        Try
            If tbxStoragePath.Text.Last <> "\" Then tbxStoragePath.Text += "\"

            If Not IO.Directory.Exists(tbxStoragePath.Text) Then
                btnCheckStorage.BackColor = Color.Red
                Exit Sub
            End If

            IO.File.WriteAllText(tbxStoragePath.Text & "EOL-WriteAccessTest", "test")
            IO.File.Delete(tbxStoragePath.Text & "EOL-WriteAccessTest")
            btnCheckStorage.BackColor = Color.LightGreen
        Catch ex As Exception
            btnCheckStorage.BackColor = Color.Red
        End Try
    End Sub

    Private Sub btnDoMonthlyRef_Click(sender As Object, e As EventArgs) Handles btnDoMonthlyRef.Click
        Try
            Me.Close()
            HELIOSEndOfLine.DoRefMeas(-2)
        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class