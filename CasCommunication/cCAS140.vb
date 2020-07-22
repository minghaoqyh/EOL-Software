Option Strict On
Option Explicit On
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Text
'Imports System.Windows.Forms
'Imports InstrumentSystems.CAS4
Imports System.IO
Imports CasCommunication.InstrumentSystems.CAS4.CAS4DLL

Public Class cCAS140

    Private Const SpectraFileExtension As String = ".isd"
    'Private Const LambdaMinEqui As Integer = 278
    'Private Const LambdaMaxEqui As Integer = 801
    Private Spectrometermodeltypes As String() = {"unknown", "MAS30UV", "MAS30", "CAS140B", "CAS140B NMOS", "CAS140CT InGaAS", _
        "CAS140CT", "MAS40", "MAS40A", "CAS140 CTS", "CAS120"}

    Private CasID As Integer
    'UPGRADE_NOTE: Interface wurde aktualisiert auf Interface_Renamed. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private intDeviceType As Integer
    Private intDeviceTypeOption As Integer
    Private CasResult As Integer
    Private sb As StringBuilder = New StringBuilder(256)
    Private DeviceTypes() As Integer
    Private DeviceTypeNames() As String
    Private DeviceTypeOptions() As Integer
    Private DeviceTypeOptionNames() As String
    Private Spectrum(,) As Double
    Private equiSpectrum(,) As Double
    Private strConfigFile As String
    Private strCalibFile As String
    Private strCasDLLFileName As String
    Private strCasDLLVersion As String
    Private VisiblePixels As Integer
    Private DeadPixels As Integer
    Private DeviceOptions As Integer

    ' Spectral results
    Private MaximumCounts As Integer
    Private nmRangeMin As Integer
    Private nmRangeMax As Integer
    ' Private IntegrationTime As Integer
    Private NumberOfAverages As Integer
    Private SpectrometerModel As String
    Private DensityFilter As Integer
    Private AmpOffset As Double
    Private SerialNumber As String
    Private MinimumX As Double
    Private MaximumX As Double

    Private APhot As Double
    Private x As Double
    Private y As Double
    Private z As Double
    Private u As Double
    Private v1976 As Double
    Private v1960 As Double
    Private Dom As Double
    Private Purity As Double
    Private Integral As Double
    Private photUnit As StringBuilder = New StringBuilder(20)
    Private radUnit As StringBuilder = New StringBuilder(20)
    Private CCT As Double
    Private Scotopic As Double
    Private UVA, UVB, UVC, Vis As Double
    Private CRI(16) As Double
    Private VisEffect As Double
    Private Width50 As Double
    Private PeakWaveLength, PeakIntensity As Double
    Private CentroidWaveLength As Double
    Private CRICCT As Double
    Private DistToPlanck As Double
    Private CDI As Double
    Private _X, _Y, _Z As Double
    Private ResultString As String
    Private Observer As Integer
    Private CCDTemperature As Double
    Private DCCCDTemperature As Double
    Private LastDarkCurrent As Integer
    Private IntegrationTimeMax As Integer
    Private IntegrationTimeMin As Integer
    Private InitialContFileNumber As Integer
    Private ContFileandPath As String
    Private SaveContFile As Boolean




#Region "Constructor"
    Public Sub New()
        casGetDLLFileName(sb, sb.Capacity)
        strCasDLLFileName = sb.ToString
        casGetDLLVersionNumber(sb, sb.Capacity)
        strCasDLLVersion = sb.ToString
        GetDeviceTypes()
        CasID = -1
    End Sub
#End Region

#Region "Initialization, Devicetype and Option"

#Region "Properties"

    ''' <summary>
    ''' Get Array of DeviceTypeNames. Used to display DeviceTypeName as clear text instead of number
    ''' </summary>
    ''' <value></value>
    ''' <returns>Array of DevicetypeNames</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ReadDeviceTypeNames() As String()
        Get
            'GetDeviceTypes()
            Return DeviceTypeNames
        End Get
    End Property

    ''' <summary>
    ''' Get Array of DeviceTypeOptionNames. Used to display DeviceTypeOptionName as clear text instead of number
    ''' </summary>
    ''' <value></value>
    ''' <returns>Array of DevicetypeOptionNames</returns>
    ''' <remarks>Array of DeviceTypeOptionNames generated when user calls SetDeviceTypebyIndex</remarks>
    Public ReadOnly Property ReadDeviceTypeOptionNames() As String()
        Get
            'GetDeviceTypeOptions()
            Return DeviceTypeOptionNames
        End Get
    End Property

    Public ReadOnly Property DeviceType As Integer
        Get
            Return intDeviceType
        End Get
        'Set(value As Integer)
        '    intDeviceType = value
        'End Set
    End Property

    Public ReadOnly Property GetDeviceTypeIndex(ByVal DeviceType As Integer) As Integer
        Get
            Dim index As Integer
            index = 0
            While index <= UBound(DeviceTypes) And DeviceTypes(index) <> DeviceType
                index += 1
            End While
            If DeviceTypes(index) = DeviceType Then
                Return index
            Else
                Return -1
            End If
        End Get
    End Property

    ''' <summary>
    ''' Property to get the Configuration File Name incl. path and drive or to set this property
    ''' </summary>
    ''' <value>Configuration File Name incl. path and drive </value>
    ''' <returns>Configuration File Name incl. path and drive</returns>
    ''' <remarks></remarks>
    Public Property ConfigFileName As String
        Set(ByVal value As String)
            strConfigFile = value
        End Set
        Get
            Return strConfigFile
        End Get
    End Property

    ''' <summary>
    ''' Property to get the Calibration File Name incl. path and drive or to set this property
    ''' </summary>
    ''' <value>Calibration File Name incl. path and drive </value>
    ''' <returns>Calibration File Name incl. path and drive</returns>
    ''' <remarks></remarks>
    Public Property CalibFileName As String
        Set(ByVal value As String)
            strCalibFile = value
        End Set
        Get
            Return strCalibFile
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Function to get all the available DeviceTypes. Returns the number of available interfaces. Array of DeviceTypenumber (integer) generated and array of DeviceTypeName (string) generated. 
    ''' These arrays are private member of this class can be read with properties.
    ''' </summary>
    ''' <returns>Number of available devices</returns>
    ''' <remarks>Function called in the constructor</remarks>
    Private Function GetDeviceTypes() As Integer
        Dim NumberOfInterfaces As Integer
        Dim RealInterfaces As Integer

        NumberOfInterfaces = casGetDeviceTypes() 'get number of interfaces
        RealInterfaces = -1
        For i = 0 To NumberOfInterfaces - 1
            casGetDeviceTypeName(i, sb, sb.Capacity) 'get name of interface
            If sb.Length > 0 Then
                RealInterfaces += 1
                ReDim Preserve DeviceTypes(RealInterfaces)
                ReDim Preserve DeviceTypeNames(RealInterfaces)
                DeviceTypes(RealInterfaces) = i
                DeviceTypeNames(RealInterfaces) = sb.ToString
            End If
            'cbInterface.Items.Add(sb.ToString)
            'tbxCalibFileName.Text = sb.ToString
        Next i
        Return RealInterfaces
    End Function


    ''' <summary>
    ''' Function to get all the available DeviceTypeOptions. Returns the number of available DeviceTypeOptions. Array of DeviceTypeOptionnumber (integer) generated and array of DeviceTypeOptionNames (string) generated. 
    ''' These arrays are private member of this class can be read with properties.
    ''' </summary>
    ''' <returns>Number of available DeviceTypeOptions</returns>
    ''' <remarks>Function called when DeviceType is set</remarks>
    Private Function GetDeviceTypeOptions() As Integer
        Dim NumberOfOptions As Integer
        Dim RealOptions As Integer
        Dim i As Integer

        NumberOfOptions = casGetDeviceTypeOptions(intDeviceType)
        RealOptions = -1
        For i = 0 To NumberOfOptions - 1
            casGetDeviceTypeOptionName(intDeviceType, i, sb, sb.Capacity)
            If sb.Length > 0 Then
                RealOptions += 1
                ReDim Preserve DeviceTypeOptions(RealOptions)
                ReDim Preserve DeviceTypeOptionNames(RealOptions)
                DeviceTypeOptions(RealOptions) = casGetDeviceTypeOption(intDeviceType, i)
                DeviceTypeOptionNames(RealOptions) = sb.ToString
            End If
        Next i
        Return RealOptions
    End Function

    ''' <summary>
    ''' Function sets the devicetype. Index has to be given and then you get number of the DeviceType. As not all devicetypes must be available maximum index is number of available devices -1
    ''' </summary>
    ''' <param name="index">index of the DeviceType. maximum index is number of available devices -1</param>
    ''' <returns>DeviceType(number) if valid else -1 is returned. </returns>
    ''' <remarks></remarks>
    Public Function SetDeviceTypebyIndex(ByVal index As Integer) As Integer
        If index <= UBound(DeviceTypes) Then
            intDeviceType = DeviceTypes(index)
            GetDeviceTypeOptions()
        Else
            intDeviceType = -1
        End If
        Return intDeviceType
    End Function


    ''' <summary>
    ''' Function sets the devicetypeoptions. Index has to be given and then you get number of the DeviceTypeoption. As not all devicetypeoptions must be available maximum index is number of available devicetypeoptions -1
    ''' </summary>
    ''' <param name="index">index of the DeviceTypeOption. Maximum index is number of available devicetypeotpions -1</param>
    ''' <returns>DeviceTypeOption(number) if valid else -1 is returned. </returns>
    ''' <remarks></remarks>
    Public Function SetDeviceTypeOptionbyIndex(ByVal index As Integer) As Integer
        intDeviceTypeOption = -1
        If Not IsNothing(DeviceTypeOptions) Then
            If index <= UBound(DeviceTypeOptions) Then
                intDeviceTypeOption = DeviceTypeOptions(index)
            End If
        End If     
        Return intDeviceTypeOption
    End Function





    Public Sub SetConfFileNameAndPath(ByVal SaveFile As Boolean, Optional ByVal ContPath As String = "C:\", Optional ByVal ContFile As String = "kai", Optional ByVal Initialnumber As Integer = 1)
        If SaveFile = True Then
            ContFileandPath = ContPath & "\" & ContFile
            InitialContFileNumber = Initialnumber
        End If
        SaveContFile = SaveFile
    End Sub

    Public Sub CasDoneWhenNeeded()

        If (CasID >= 0) Then
            casDoneDevice(CasID)
            CasID = -1
        End If

    End Sub



    Public Function Done() As Integer
        Done = 0
        If CasID > -1 Then
            Done = casDoneDevice(CasID)
        End If
        CasID = -1
    End Function

    ''' <summary>
    ''' Get Handle to the Device, load config and calibration file and perform initialization of the hardware.
    ''' </summary>
    ''' <param name="ACal">Set after initialization to the Calibration unit</param>
    ''' <param name="AConfig">Path and Filename of Configuration File</param>
    ''' <param name="ACalib">Path and Filename of Calibration File</param>
    ''' <returns>True if initialization successful, False if initialization not successful, in that case an error message will be shown that is handled in the catch statement</returns>
    ''' <remarks></remarks>
    Public Function Init(ByRef ACal As String, ByVal AConfig As String, ByVal ACalib As String) As Boolean

        CasDoneWhenNeeded()

        Try
            ' MessageBox.Show("Folgender Devicetyp ausgewählt: " & DeviceType & " Option gewählt: " & DeviceTypeOption, "Device", MessageBoxButtons.OK, MessageBoxIcon.Information)

            CasID = casCreateDeviceEx(intDeviceType, intDeviceTypeOption) 'create handle
            CheckCASError(CasID)
            strCalibFile = ACalib
            strConfigFile = AConfig
            casSetDeviceParameterString(CasID, dpidConfigFileName, AConfig)
            casSetDeviceParameterString(CasID, dpidCalibFileName, ACalib)

            CheckCASError(casInitialize(CasID, InitForced)) 'init device

            casGetDeviceParameterString(CasID, dpidCalibrationUnit, sb, sb.Capacity)
            ACal = sb.ToString
            VisiblePixels = CInt(casGetDeviceParameter(CasID, dpidVisiblePixels))
            DeadPixels = CInt(casGetDeviceParameter(CasID, dpidDeadPixels))
            IntegrationTimeMax = CInt(casGetDeviceParameter(CasID, dpidIntTimeMax))
            IntegrationTimeMin = CInt(casGetDeviceParameter(CasID, dpidIntTimeMin))
        Catch ex As Exception
            CasDoneWhenNeeded()
            'MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        Return True
    End Function

#End Region

#Region "Properties"

#Region "Properties of spectral data"
    Public ReadOnly Property FluxUnit As String
        Get
            Return CStr(photUnit.ToString)
        End Get
    End Property

    Public ReadOnly Property PowerUnit As String
        Get
            Return CStr(radUnit.ToString)
        End Get
    End Property


    Public ReadOnly Property LambdaDom As Double
        Get
            Return Dom
        End Get
    End Property

    Public ReadOnly Property xyPurity As Double
        Get
            Return Purity
        End Get
    End Property

    Public ReadOnly Property Flux As Double
        Get
            Return APhot
        End Get
    End Property

    Public ReadOnly Property Power As Double
        Get
            Return Integral
        End Get
    End Property



    Public ReadOnly Property Cx As Double
        Get
            Return x
        End Get
    End Property

    Public ReadOnly Property Cy As Double
        Get
            Return y
        End Get
    End Property

    Public ReadOnly Property Cz As Double
        Get
            Return z
        End Get
    End Property

    Public ReadOnly Property uStrich As Double
        Get
            Return u
        End Get
    End Property

    Public ReadOnly Property vStrich1960 As Double
        Get
            Return v1960
        End Get
    End Property

    Public ReadOnly Property vStrich1976 As Double
        Get
            Return v1976
        End Get
    End Property

    Public ReadOnly Property TriStimX As Double
        Get
            Return _X
        End Get
    End Property

    Public ReadOnly Property TriStimY As Double
        Get
            Return _Y
        End Get
    End Property

    Public ReadOnly Property TriStimZ As Double
        Get
            Return _Z
        End Get
    End Property

    Public ReadOnly Property LambdaPeak As Double
        Get
            Return PeakWaveLength
        End Get
    End Property

    Public ReadOnly Property GetCRI As Double
        Get
            Return CRI(0)
        End Get
    End Property

    Public ReadOnly Property GetR9 As Double
        Get
            Return CRI(9)
        End Get
    End Property

    Public ReadOnly Property GetR13 As Double
        Get
            Return CRI(13)
        End Get
    End Property

    Public ReadOnly Property GetR15 As Double
        Get
            Return CRI(15)
        End Get
    End Property

    Public ReadOnly Property GetCCT As Double
        Get
            Return CCT
        End Get
    End Property
#End Region
    

    Public ReadOnly Property CCDTemperatur As Double
        Get
            Return CCDTemperature
        End Get
    End Property
    

    Public ReadOnly Property MaxCounts As Integer
        Get
            Return MaximumCounts
        End Get
    End Property

    

    Public Property FilterPosition As Integer
        Get
            Return CInt(casGetMeasurementParameter(CasID, mpidCurrentDensityFilter))
        End Get
        Set(value As Integer)
            If value >= 0 And value <= 7 Then
                casSetMeasurementParameter(CasID, mpidDensityFilter, value)
            Else
                If value < 0 Then
                    casSetMeasurementParameter(CasID, mpidDensityFilter, 0)
                Else
                    casSetMeasurementParameter(CasID, mpidDensityFilter, 7)
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property MaximumIntTime As Integer
        Get
            Return IntegrationTimeMax
        End Get
    End Property

    Public ReadOnly Property MinimumIntTime As Integer
        Get
            Return IntegrationTimeMin
        End Get
    End Property
    Public Property IntegrationTime As Integer
        Get
            Return CInt(casGetMeasurementParameter(CasID, mpidIntegrationTime))
        End Get
        Set(value As Integer)
            If value >= IntegrationTimeMin And value <= IntegrationTimeMax Then
                casSetMeasurementParameter(CasID, mpidIntegrationTime, value)
            ElseIf value < IntegrationTimeMin Then
                casSetMeasurementParameter(CasID, mpidIntegrationTime, IntegrationTimeMin)
            Else
                casSetMeasurementParameter(CasID, mpidIntegrationTime, IntegrationTimeMax)
            End If

        End Set
    End Property
    Public Property Averages As Integer
        Get
            Return CInt(casGetMeasurementParameter(CasID, mpidAverages))
        End Get
        Set(value As Integer)
            casSetMeasurementParameter(CasID, mpidAverages, value)
        End Set
    End Property

    Public ReadOnly Property Filter As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coFilter) <> 0
        End Get
    End Property

    Public ReadOnly Property Shutter As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coShutter) <> 0
        End Get
    End Property

    Public ReadOnly Property GetShutter As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coGetShutter) <> 0
        End Get
    End Property

    Public ReadOnly Property GetFilter As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coGetFilter) <> 0
        End Get
    End Property

    Public ReadOnly Property GetAccessories As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coGetAccessories) <> 0
        End Get
    End Property

    Public ReadOnly Property GetTemperature As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coGetTemperature) <> 0
        End Get
    End Property

    Public ReadOnly Property GetMultiTrackPossibility As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coCanMultiTrack) <> 0
        End Get
    End Property

    Public Property UseDarkArray As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coUseDarkcurrentArray) <> 0
        End Get
        Set(value As Boolean)
            SetDeviceOptionsOnOff(coUseDarkcurrentArray, CInt(value))
        End Set
    End Property

    Public Property UseTransmission As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coUseTransmission) <> 0
        End Get
        Set(value As Boolean)
            SetDeviceOptionsOnOff(coUseTransmission, CInt(value))
        End Set
    End Property

    Public Property AutoRangeMeasurement As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coAutorangeMeasurement) <> 0
        End Get
        Set(value As Boolean)
            SetDeviceOptionsOnOff(coAutorangeMeasurement, CInt(value))
        End Set
    End Property

    Public Property AutoRangeFilter As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coAutorangeFilter) <> 0
        End Get
        Set(value As Boolean)
            SetDeviceOptionsOnOff(coAutorangeFilter, CInt(value))
        End Set
    End Property

    Public Property CheckCalibConfigSerials As Boolean
        Get
            GetDeviceOptions()
            Return (DeviceOptions And coCheckCalibConfigSerials) <> 0
        End Get
        Set(value As Boolean)
            SetDeviceOptionsOnOff(coCheckCalibConfigSerials, CInt(value))
        End Set
    End Property


    Public ReadOnly Property AmountVisiblePixels As Integer
        Get
            Return VisiblePixels
        End Get
    End Property

    Public ReadOnly Property AmountDeadPiexels As Integer
        Get
            Return DeadPixels
        End Get
    End Property

    

    Public ReadOnly Property DLLFileName As String
        Get
            Return strCasDLLFileName
        End Get
    End Property

    Public ReadOnly Property DLLVersion As String
        Get
            Return strCasDLLVersion
        End Get
    End Property



#End Region

   
    Public Function Measurement() As Boolean
        Dim i As Integer
        Dim SpecModelnumber As Integer

        CasResult = Measure()
        x = 0
        y = 0
        z = 0
        u = 0
        v1976 = 0
        v1960 = 0
        If CasResult >= 0 Then
            'casColorMetric(CasID)
            'retrieve color temperature
            CCT = casGetCCT(CasID)
            'retrive scotopic integral
            Scotopic = casGetExtendedColorValues(CasID, ecvScotopicInt)
            'retrieve Radiometric integral in UVA, UVB, UVC and Visible
            UVA = casGetExtendedColorValues(CasID, ecvUVA)
            UVB = casGetExtendedColorValues(CasID, ecvUVB)
            UVC = casGetExtendedColorValues(CasID, ecvUVC)
            Vis = casGetExtendedColorValues(CasID, ecvVIS)
            'retrieve color rendering index for all 16 colors
            casCalculateCRI(CasID)
            For i = 0 To UBound(CRI)
                CRI(i) = casGetCRI(CasID, i)
            Next
            'retrieve Centroidwavelength
            CentroidWaveLength = casGetCentroid(CasID)
            'retrieve Peakwavelength
            casGetPeak(CasID, PeakWaveLength, PeakIntensity)
            'retrieve Width (FWHM)
            Width50 = casGetWidth(CasID)
            'retrieve Visual Effect (Tristimulus Y divided by radiometric Integral)
            VisEffect = casGetExtendedColorValues(CasID, ecvVisualEffect)
            'retrieve Distance to Planckian Locus
            DistToPlanck = casGetExtendedColorValues(CasID, ecvDistance)
            'retrieve CDI
            CDI = casGetExtendedColorValues(CasID, ecvCDI)
            'retrieve Correlated color temperatures for calculating CRI
            CRICCT = casGetExtendedColorValues(CasID, ecvCRICCT)
            'retrieve Tristimulus Values (X, Y, Z)
            casGetTriStimulus(CasID, _X, _Y, _Z)
            'retrieve color coordinates; neccessary for cmXYToDominantWavelength below!
            casGetColorCoordinates(CasID, x, y, z, u, v1976, v1960)
            'retrieve Observer
            Observer = CInt(casGetMeasurementParameter(CasID, mpidObserver))
            If Observer = 0 Then
                Observer = 2
            Else
                Observer = 10
            End If
            'retrieve CCD Temperature of Dark Current measurement
            DCCCDTEmperature = casGetMeasurementParameter(CasID, mpidDCCCDTemperature)
            'retrieve time since last dark current measurement
            LastDarkCurrent = CInt(casGetMeasurementParameter(CasID, mpidLastDCAge))
            LastDarkCurrent = CInt(LastDarkCurrent / 1000)
            Dom = 0
            Purity = 0
            CasResult = cmXYToDominantWavelength(x, y, 1.0# / 3, 1.0# / 3, Dom, Purity) 'calculate dom WL and purity

            'get radiometric integral
            casGetRadInt(CasID, Integral, radUnit, radUnit.Capacity)

            'get photometric integral
            casGetPhotInt(CasID, APhot, photUnit, photUnit.Capacity)
            MaximumCounts = CInt(casGetMeasurementParameter(CasID, mpidMaxADCValue))
            nmRangeMin = CInt(casGetMeasurementParameter(CasID, mpidColormetricStart))
            nmRangeMax = CInt(casGetMeasurementParameter(CasID, mpidColormetricStop))
            'IntegrationTime = CInt(casGetMeasurementParameter(CasID, mpidIntegrationTime))
            NumberOfAverages = CInt(casGetMeasurementParameter(CasID, mpidAverages))
            SpecModelnumber = CInt(casGetDeviceParameter(CasID, dpidSpectrometerModel))
            If SpecModelnumber < LBound(Spectrometermodeltypes) Or SpecModelnumber > UBound(Spectrometermodeltypes) Then
                SpecModelnumber = 0
            End If
            SpectrometerModel = Spectrometermodeltypes(SpecModelnumber)
            DensityFilter = CInt(casGetMeasurementParameter(CasID, mpidDensityFilter))
            AmpOffset = casGetMeasurementParameter(CasID, mpidAmpOffset)
            CCDTemperature = casGetMeasurementParameter(CasID, mpidCurrentCCDTemperature)
            casGetSerialNumberEx(CasID, casSerialDevice, sb, sb.Capacity)
            SerialNumber = sb.ToString

            Return True
        Else
            Return False
        End If
    End Function

    Private Function SpecDistrValueNearWavelength(ByRef index As Integer, ByVal Wavelength As Integer) As Double
        Do
            index += 1
        Loop Until index >= UBound(Spectrum, 2) Or Spectrum(0, index) > Wavelength
        If Spectrum(0, index) > Wavelength Then
            'MessageBox.Show("Vorgegebene Wellenlänage: " & Wavelength & vbCrLf & _
            '                "Kleinere Wellenlänge: " & Format(Spectrum(0, index - 1), "0.000") & vbCrLf & _
            '                "Grössere Wellenlänge: " & Format(Spectrum(0, index), "0.000"), _
            '                "Wellenlängensuche", MessageBoxButtons.OK, MessageBoxIcon.Information)


            If Math.Abs(Spectrum(0, index) - Wavelength) < Math.Abs(Spectrum(0, index - 1) - Wavelength) Then
                'MessageBox.Show("Grössere Wellenlänge genommen", "Wahl der Wellenlänge für Auswertung", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Spectrum(1, index)
            Else
                'MessageBox.Show("Kleinere Wellenlänge genommen", "Wahl der Wellenlänge für Auswertung", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return Spectrum(1, index - 1)
            End If
        Else
            Return -1
        End If
    End Function

    Public Function WaveLengthQuotient(ByVal Selection As Integer) As Double
        Const Numerator_Magg As Integer = 482
        Const Denominator_Magg As Integer = 578
        'Const Numerator_Magg As Integer = 350
        'Const Denominator_Magg As Integer = 700
        Const Numerator_none As Integer = 480
        Const Denominator_none As Integer = 580
        Const Magg As Integer = 1
        Const None As Integer = 0
        Dim Denominator As Integer
        Dim Numerator As Integer
        Dim SpecDistrAtDenominator As Double
        Dim SpecDistrAtNumerator As Double
        Dim i As Integer
        If IsNothing(Spectrum) = False Then
            i = 0
            If Selection = Magg Then
                Denominator = Denominator_Magg
                Numerator = Numerator_Magg
            ElseIf Selection = None Then
                Denominator = Denominator_none
                Numerator = Numerator_none
            Else
                Return -1
            End If
            SpecDistrAtNumerator = SpecDistrValueNearWavelength(i, Numerator)
            If SpecDistrAtNumerator <> -1 Then
                SpecDistrAtDenominator = SpecDistrValueNearWavelength(i, Denominator)
                If SpecDistrAtDenominator <> -1 Then
                    Return SpecDistrAtNumerator / SpecDistrAtDenominator
                Else
                    Return -1
                End If
            Else
                Return -1
            End If
        Else
            Return -1
        End If

    End Function

    Public Sub ReadSpectrum(ByRef SpectralValues(,) As Double)
        'GetSpectrum()
        'MakeEquiSpectrum()
        SpectralValues = Spectrum
    End Sub

    Private Function MakeEquiSpectrum() As Boolean
        Dim i As Integer
        Dim j As Integer
        Dim LambdaMax As Integer
        Dim LambdaMin As Integer

        j = 0
        LambdaMax = CInt(Fix(MaximumX))
        LambdaMin = CInt(Fix(MinimumX)) + 1


        'ReDim equiSpectrum(1, LambdaMaxEqui - LambdaMinEqui)
        ReDim equiSpectrum(1, LambdaMax - LambdaMin)
        For i = 0 To UBound(equiSpectrum, 2)
            equiSpectrum(0, i) = LambdaMin + i
        Next i
        'MessageBox.Show("Der letzte Wert ist: " & equiSpectrum(0, UBound(equiSpectrum, 2)), "Last Spec Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
        For i = 0 To UBound(equiSpectrum, 2)
            'For i = 0 To 20
            'MessageBox.Show("Wert für i: " & i, "Zählerlauf", MessageBoxButtons.OK, MessageBoxIcon.Information)
            While j < UBound(Spectrum, 2) And Spectrum(0, j) < equiSpectrum(0, i)
                j += 1
            End While
           
            equiSpectrum(1, i) = Spectrum(1, j - 1) * ((Spectrum(0, j) - equiSpectrum(0, i)) / (Spectrum(0, j) - Spectrum(0, j - 1))) + Spectrum(1, j) * ((equiSpectrum(0, i) - Spectrum(0, j - 1)) / (Spectrum(0, j) - Spectrum(0, j - 1)))
            'If i = 0 Or i = UBound(equiSpectrum, 2) Then
            '    MessageBox.Show("Zu ermittelnder Wellenlängenwert: " & equiSpectrum(0, i) & " großer Wellenlängenwert " _
            '                    & Spectrum(0, j) & " kleinerer Wert: " & Spectrum(0, j - 1), "Wellenlängenerte", MessageBoxButtons.OK, MessageBoxIcon.Information)

            'End If
            'Return True
        Next i
        Return True
    End Function

    Public Sub ReadEquiSpectrum(ByRef EquiSpectralValues(,) As Double)
        GetSpectrum()
        MakeEquiSpectrum()
        EquiSpectralValues = equiSpectrum

    End Sub

    Private Sub GetDeviceOptions()
        DeviceOptions = casGetOptions(CasID)
    End Sub

    Private Sub SetDeviceOptionsOnOff(ByVal OptionsToSwitch As Integer, ByVal SwitchOn As Integer)
        casSetOptionsOnOff(CasID, OptionsToSwitch, SwitchOn)
    End Sub


    Private Function GetSpectrum() As Boolean
        Dim i As Integer

        ReDim Spectrum(1, VisiblePixels - 1)

        For i = 0 To VisiblePixels - 1
            Spectrum(0, i) = casGetXArray(CasID, DeadPixels + i)
            Spectrum(1, i) = casGetData(CasID, DeadPixels + i)
        Next i
        MinimumX = Spectrum(0, 0)
        MaximumX = Spectrum(0, VisiblePixels - 1)
        Return True
    End Function

    Private Function CheckCASError(ByVal AError As Integer) As Integer
        If (AError < ErrorNoError) Then
            casGetErrorMessage(AError, sb, sb.Capacity)
            Throw New Exception(String.Format("CAS DLL error ({0}): {1}", AError, sb.ToString()))
        End If

        Return AError
    End Function

    Private Sub CheckCASError()
        CheckCASError(casGetError(CasID))
    End Sub

    

    Public Function MeasureDarkCurrent() As Integer
        casSetShutter(CasID, 1)
        MeasureDarkCurrent = casMeasureDarkCurrent(CasID)
        casSetShutter(CasID, 0)
    End Function
    Public Function SaveSpectrumContinous() As Integer
        If SaveContFile = True Then
            SaveSpectrumtoFile(ContFileandPath & "_" & CStr(Format(InitialContFileNumber, "000")) & SpectraFileExtension)
            InitialContFileNumber += 1
        End If

        Return 1

    End Function

    Public Function SaveSpectrumtoFile(ByRef Filename As String) As Integer
        Dim OutputFile As System.IO.StreamWriter
        OutputFile = My.Computer.FileSystem.OpenTextFileWriter(Filename, False)
        GetSpectrum()
        MakeEquiSpectrum()

        With OutputFile
            .WriteLine("[Curve Information]")
            .WriteLine("Name=CASDATA")
            .WriteLine("Data=CASDATA")
            .WriteLine("Class=TLWNumMode")
            .WriteLine()
            .WriteLine("[Comment]")
            .WriteLine()
            .WriteLine()
            .WriteLine("[Measurement Conditions]")
            .WriteLine("Maxcounts=" & MaximumCounts)
            .WriteLine("Range [nm]=" & nmRangeMin & ".." & nmRangeMax)
            .WriteLine("Integration Time [ms]=" & IntegrationTime)
            .WriteLine("Averaging=" & NumberOfAverages)
            .WriteLine("Spectrometer Type=" & SpectrometerModel)
            .WriteLine("TOP100 Aperture=0")
            .WriteLine("TOP100 Distance [nm]=0")
            .WriteLine("Density Filter=" & DensityFilter)
            .WriteLine("Configuration=" & strConfigFile)
            .WriteLine("Calibration=" & strCalibFile)
            .WriteLine("TOP100 File=")
            .WriteLine("Offset=" & AmpOffset)
            .WriteLine("CCDTemperature=" & CCDTemperature)
            .WriteLine("SerialNumber=" & SerialNumber)
            .WriteLine("DCCCDTemperature=" & DCCCDTemperature)
            .WriteLine("Last Dark Current [s]=" & LastDarkCurrent)
            .WriteLine("Observer [°]=" & Observer & "°")
            .WriteLine()

            .WriteLine("[Results]")
            .WriteLine("Radiometric [W]=" & Integral)
            .WriteLine("Photometric [lm]=" & APhot)
            .WriteLine("Scotopic [lm]=" & Scotopic)
            .WriteLine("RadiometricUnit=" & radUnit.ToString)
            .WriteLine("PhotometricUnit=" & photUnit.ToString)
            .WriteLine("ScotopicUnit=" & photUnit.ToString)
            .WriteLine("RadiometricName=Strahlungsleistung")
            .WriteLine("PhotometricName=Lichtstrom")
            .WriteLine("ScotopicName=Lichtstrom")
            .WriteLine("UVA [W]=" & UVA)
            .WriteLine("UVB [W]=" & UVB)
            .WriteLine("UVC [W]=" & UVC)
            .WriteLine("VIS [W]=" & Vis)
            .WriteLine("Tristimulus_X [lm]=" & _X)
            .WriteLine("Tristimulus_Y [lm]=" & _Y)
            .WriteLine("Tristimulus_Z [lm]=" & _Z)
            .WriteLine("FootLambert [MSCP]=")
            .WriteLine("ColorCoordinates/x=" & x)
            .WriteLine("ColorCoordinates/y=" & y)
            .WriteLine("ColorCoordinates/z=" & z)
            .WriteLine("ColorCoordinates/u=" & u)
            .WriteLine("ColorCoordinates/v1960=" & v1960)
            .WriteLine("ColorCoordinates/v1976=" & v1976)
            .WriteLine("PeakWavelength [nm]=" & PeakWaveLength)
            .WriteLine("CentroidWavelength [nm]=" & CentroidWaveLength)
            .WriteLine("DominantWavelength [nm]=" & Dom)
            .WriteLine("Purity=" & Purity)
            .WriteLine("Width50 [nm]=" & Width50)
            .WriteLine("CCT [K]=" & CCT)
            .WriteLine("PlanckDistance=" & DistToPlanck)
            .WriteLine("RedEffect [%]=")
            .WriteLine("VisEffect [%]=" & VisEffect)
            .WriteLine("CRI=" & CRI(0))
            For i = 1 To UBound(CRI)
                .WriteLine("CRI/CRI" & Format(i, "00") & "=" & CRI(i))
            Next i
            .WriteLine("CDI=" & CDI)
            .WriteLine("CRITemp [K]=" & CRICCT)
            .WriteLine()
            .WriteLine("[Data]")
            .WriteLine("Name=CasData")
            .WriteLine("Type=Double")
            .WriteLine("NumberOfDataX=" & VisiblePixels)
            .WriteLine("NumberOfDataY=2")
            .WriteLine("X-MIN=" & MinimumX)
            .WriteLine("X-MAX=" & MaximumX)
            .WriteLine("X-UNIT=nm")
            .WriteLine("Y-UNIT=W/nm")
            .WriteLine("Data")
            For i = 0 To VisiblePixels - 1
                .WriteLine(Spectrum(0, i) & vbTab & Spectrum(1, i))
            Next i
            .WriteLine()
            For i = 0 To UBound(equiSpectrum, 2)
                .WriteLine(equiSpectrum(0, i) & vbTab & equiSpectrum(1, i))
            Next i
        End With
        OutputFile.Close()
        Return 1
    End Function

    'Public Function SaveSpectrumtoFile(ByRef FileName As String) As Integer
    '    GenerateResultString()
    '    casSaveSpectrum(CasID, FileName)
    '    ZeileEinfügen(5, FileName, "[Comment]" & vbCrLf & vbCrLf)
    '    ZeileEinfügen(23, FileName)
    '    Return 1
    'End Function

    'Private Sub ZeileEinfügen(ByVal NachZeile As Integer, ByVal Datei As String, Optional ByVal InsertString As String = "ABC")
    '    Dim DateiInhalt As String
    '    Dim Pos As Integer = -2
    '    Dim i As Integer
    '    If InsertString = "ABC" Then
    '        InsertString = ResultString
    '    End If
    '    DateiInhalt = My.Computer.FileSystem.ReadAllText(Datei)

    '    For i = 1 To NachZeile
    '        Pos = DateiInhalt.IndexOf(vbCrLf, Pos + 2)
    '        If Pos = -1 Then Exit For
    '    Next

    '    If NachZeile = 0 Then
    '        ' Vor allen Zeilen einfügen
    '        DateiInhalt = InsertString & vbCrLf & DateiInhalt
    '        ' Zuwenig Zeilen, daher nach allen Zeilen einfügen
    '    ElseIf Pos = -1 Then
    '        DateiInhalt = DateiInhalt & InsertString & vbCrLf
    '    Else
    '        ' Ziwschen Anfang und Ende einfügen
    '        DateiInhalt = DateiInhalt.Insert(Pos + 2, InsertString & vbCrLf)
    '    End If

    '    My.Computer.FileSystem.WriteAllText(Datei, DateiInhalt, False)

    'End Sub



    Private Function Measure() As Integer
        'perform background measurement, if necessary!
        If System.Math.Round(casGetDeviceParameter(CasID, dpidNeedDarkCurrent)) <> 0 Then
            CasResult = MeasureDarkCurrent()
        End If
        CasResult = casMeasure(CasID) 'perform one measurement
        If CasResult >= 0 Then
            CasResult = casColorMetric(CasID) 'perform calculation
        Else
            CasResult = casGetError(CasID) 'check for errors
        End If
        GetSpectrum()
        Measure = CasResult

    End Function
End Class
