Public Class CAS140IniFile
    Inherits IniFile

#Region "Constant Data"
    Private Const SectionDateien As String = "Files"

    Private Const SectionDevice As String = "Device"
    'Private Const SectionParameter As String = "Show Parameter"
    Private Const SectionSettings As String = "Settings"


    Private Const ConfigFile As String = "Configuration File"
    Private Const CalibFile As String = "Calibration File"
    Private Const HeliosCalDateien As String = "HELIOS Calibration File"

    Private Const DevType As String = "CAS DeviceType"
    Private Const DevTypeOption As String = "CAS DeviceTypeOption"
    Private Const HeliosComPort As String = "HELIOS COM Port"
    'Private Const WaveLengthQuotient As String = "WaveLengthQuotient"

    'Private Const ParameterIdentifier As String = "Parameter Number"

    Private path As String = System.AppDomain.CurrentDomain.BaseDirectory & "\data\HELIOSCalib.ini"

#End Region

#Region "Private Data"
    Private strComPort As String
    Private strCalFile As String
    Private strConfFile As String
    Private strHELIOSCalFile As String
    Private intDevType As Integer
    Private intDevTypeOption As Integer
    Private strSelectedString() As String
    Private intWhichWaveQuotient As Integer
#End Region

#Region "Properties"
    'Public Property ComPort As String
    '    Get
    '        Return strComPort
    '    End Get
    '    Set(value As String)
    '        strComPort = value
    '        write_Data()
    '    End Set
    'End Property

    Public Property HELIOSCalibrationFile As String
        Get
            Return strHELIOSCalFile
        End Get
        Set(value As String)
            strHELIOSCalFile = value
            'write_Data() ?????????????????????????? FB150507
        End Set
    End Property
    'Public Sub SetShowParameter(ByVal ShowParameter() As String)
    '    strSelectedString = ShowParameter
    '    write_Data()
    'End Sub

    'Public ReadOnly Property ShowParameter() As String
    '    Get
    '        Return SelectedString
    '    End Get
    'End Property

    'Public Sub GetShowparameter(ByRef ShowParameter() As String)
    '    ShowParameter = strSelectedString
    'End Sub

    'Public Property LambdaQuot As Integer
    '    Get
    '        Return intWhichWaveQuotient
    '    End Get
    '    Set(value As Integer)
    '        intWhichWaveQuotient = value
    '        write_Data()
    '    End Set
    'End Property
    Public Property ConfigurationFile As String
        Get
            Return strConfFile
        End Get
        Set(value As String)
            strConfFile = value
            'write_Data() ????????????????????????????????? später
        End Set
    End Property

    Public Property CalibrationFile As String
        Get
            Return strCalFile
        End Get
        Set(value As String)
            strCalFile = value
            'write_Data() ???????????????????????????????????später
        End Set
    End Property

    Public Property DeviceType As Integer
        Get
            Return intDevType
        End Get
        Set(value As Integer)
            intDevType = value
            'write_Data()
        End Set
    End Property

    Public Property DeviceTypeOption As Integer
        Get
            Return intDevTypeOption
        End Get
        Set(value As Integer)
            intDevTypeOption = value
            'write_Data()
        End Set
    End Property

#End Region

#Region "public"
    Public Sub New()
        Read_Data()
    End Sub
#End Region

#Region "private"
    Private Sub Read_Data()
        'Dim i As Integer
        'Dim ReadValue As String
        strConfFile = INI_ReadValue(SectionDateien, ConfigFile, "", path)
        strCalFile = INI_ReadValue(SectionDateien, CalibFile, "", path)
        strHELIOSCalFile = INI_ReadValue(SectionDateien, HeliosCalDateien, "", path)
        intDevType = CInt(INI_ReadValue(SectionDevice, DevType, "-1", path))
        intDevTypeOption = CInt(INI_ReadValue(SectionDevice, DevTypeOption, "-1", path))
        strComPort = INI_ReadValue(SectionDevice, HeliosComPort, "", path)
        'intWhichWaveQuotient = CInt(INI_ReadValue(SectionSettings, WaveLengthQuotient, "0", path))
        'i = 0
        'Do
        '    ReadValue = INI_ReadValue(SectionParameter, ParameterIdentifier & i, "", path)
        '    If ReadValue <> "" Then
        '        ReDim Preserve strSelectedString(i)
        '        strSelectedString(i) = ReadValue
        '    End If
        '    i += 1
        'Loop Until ReadValue = ""
    End Sub



    Private Sub write_Data()
        'Dim i As Integer
        System.IO.File.Delete(path)
        INI_WriteValue(SectionDateien, ConfigFile, strConfFile, path)
        INI_WriteValue(SectionDateien, CalibFile, strCalFile, path)
        INI_WriteValue(SectionDateien, HeliosCalDateien, strHELIOSCalFile, path)
        INI_WriteValue(SectionDevice, DevType, intDevType, path)
        INI_WriteValue(SectionDevice, DevTypeOption, intDevTypeOption, path)
        INI_WriteValue(SectionDevice, HeliosComPort, strComPort, path)
        'INI_WriteValue(SectionSettings, WaveLengthQuotient, intWhichWaveQuotient, path)
        'If IsNothing(strSelectedString) = False Then
        '    For i = 0 To UBound(strSelectedString)
        '        INI_WriteValue(SectionParameter, ParameterIdentifier & i, strSelectedString(i), path)
        '    Next i
        'End If

        'INI_WriteValue(SectionRefmodules, Amount, AmountRefModules, path)
        'INI_WriteValue(SectionRefmodules, MaxAbweichungRef, RefMeasMaxDeviation, path)
    End Sub

#End Region



End Class
