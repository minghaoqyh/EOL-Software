Option Strict Off
Option Explicit On

Imports System.Text
Imports System.Runtime.InteropServices
Namespace InstrumentSystems.CAS4
    Public Class CAS4DLL

        Public Const ModuleName As String = "CAS4.dll"
        Public Const ErrorNoError As Integer = 0
        Public Const ErrorUnknown As Integer = -1
        Public Const ErrorTimeoutRWSNoData As Integer = -2
        Public Const ErrorInvalidDeviceType As Integer = -3
        Public Const ErrorAcquisition As Integer = -4
        Public Const ErrorAccuDataStream As Integer = -5
        Public Const ErrorPrivilege As Integer = -6
        Public Const ErrorFIFOOverflow As Integer = -7
        Public Const ErrorTimeoutEOSScan As Integer = -8 'ISA only  
        Public Const ErrorCCDTemperatureFail As Integer = -13
        Public Const ErrorAdrControl As Integer = -14
        Public Const ErrorFloat As Integer = -15  'floating point error  
        Public Const ErrorTriggerTimeout As Integer = -16
        Public Const ErrorAbortWaitTrigger As Integer = -17
        Public Const ErrorDarkArray As Integer = -18
        Public Const ErrorNoCalibration As Integer = -19
        Public Const ErrorInterfaceVersion As Integer = -20
        Public Const ErrorCRI As Integer = -21
        Public Const ErrorNoMultiTrack As Integer = -25
        Public Const ErrorInvalidTrack As Integer = -26
        Public Const ErrorDetectPixel As Integer = -31
        Public Const ErrorSelectParamSet As Integer = -32
        Public Const ErrorI2CInit As Integer = -35
        Public Const ErrorI2CBusy As Integer = -36
        Public Const ErrorI2CNotAck As Integer = -37
        Public Const ErrorI2CRelease As Integer = -38
        Public Const ErrorI2CTimeOut As Integer = -39
        Public Const ErrorI2CTransmission As Integer = -40
        Public Const ErrorI2CController As Integer = -41
        Public Const ErrorDataNotAck As Integer = -42
        Public Const ErrorNoExternalADC As Integer = -52
        Public Const ErrorShutterPos As Integer = -53
        Public Const ErrorFilterPos As Integer = -54
        Public Const ErrorConfigSerialMismatch As Integer = -55
        Public Const ErrorCalibSerialMismatch As Integer = -56
        Public Const ErrorInvalidParameter As Integer = -57
        Public Const ErrorGetFilterPos As Integer = -58
        Public Const ErrorParamOutOfRange As Integer = -59
        Public Const ErrorDeviceFileChecksum As Integer = -60
        Public Const ErrorInvalidEEPromType As Integer = -61
        Public Const ErrorDeviceFileTooLarge As Integer = -62
        Public Const errCASOK As Integer = ErrorNoError
        Public Const errCASError As Integer = -1000
        Public Const errCasNoConfig As Integer = errCASError - 3
        Public Const errCASDriverMissing As Integer = errCASError - 6
        'driver stuff missing, returned in INITDevice  
        Public Const errCasDeviceNotFound As Integer = errCASError - 10
        'invalid ADevice param  
        'ErrorHandling Commands  
        <DllImport(ModuleName)> _
        Public Shared Function casGetError(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetErrorMessage(AError As Integer, ADest As StringBuilder, AMaxLen As Integer) As Integer
        End Function

        'Device Handles and Interfaces  
        Public Const InterfaceISA As Integer = 0
        Public Const InterfacePCI As Integer = 1
        Public Const InterfaceTest As Integer = 3
        Public Const InterfaceUSB As Integer = 5
        <DllImport(ModuleName)> _
        Public Shared Function casCreateDevice() As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casCreateDeviceEx(AInterfaceType As Integer, AInterfaceOption As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casChangeDevice(ADevice As Integer, AInterfaceType As Integer, AInterfaceOption As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casDoneDevice(ADevice As Integer) As Integer
        End Function
        Public Const aoAssignDevice As Integer = 0
        Public Const aoAssignParameters As Integer = 1
        Public Const aoAssignComplete As Integer = 2
        <DllImport(ModuleName)> _
        Public Shared Function casAssignDeviceEx(ASourceDevice As Integer, ADestDevice As Integer, AOption As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetDeviceTypes() As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetDeviceTypeName(AInterfaceType As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetDeviceTypeOptions(AInterfaceType As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetDeviceTypeOption(AInterfaceType As Integer, AIndex As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetDeviceTypeOptionName(AInterfaceType As Integer, AInterfaceOption As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function

        'Initialization  
        Public Const InitOnce As Integer = 0
        Public Const InitForced As Integer = 1
        Public Const InitNoHardware As Integer = 2
        <DllImport(ModuleName)> _
        Public Shared Function casInitialize(ADevice As Integer, Perform As Integer) As Integer
        End Function
        'Instrument properties  
        'AWhat parameter constants for DeviceParameter methods below  
        Public Const dpidIntTimeMin As Integer = 101
        Public Const dpidIntTimeMax As Integer = 102
        Public Const dpidDeadPixels As Integer = 103
        Public Const dpidVisiblePixels As Integer = 104
        Public Const dpidPixels As Integer = 105
        Public Const dpidParamSets As Integer = 106
        Public Const dpidCurrentParamSet As Integer = 107
        Public Const dpidADCRange As Integer = 108
        Public Const dpidADCBits As Integer = 109
        Public Const dpidSerialNo As Integer = 110
        Public Const dpidTOPSerial As Integer = 111
        Public Const dpidTransmissionFileName As Integer = 112
        Public Const dpidConfigFileName As Integer = 113
        Public Const dpidCalibFileName As Integer = 114
        Public Const dpidCalibrationUnit As Integer = 115
        Public Const dpidAccessorySerial As Integer = 116
        Public Const dpidTriggerCapabilities As Integer = 118
        Public Const dpidAveragesMax As Integer = 119
        Public Const dpidFilterType As Integer = 120
        Public Const dpidRelSaturationMin As Integer = 123
        Public Const dpidRelSaturationMax As Integer = 124
        Public Const dpidInterfaceVersion As Integer = 125
        Public Const dpidTriggerDelayTimeMax As Integer = 126
        Public Const dpidSpectrometerName As Integer = 127
        Public Const dpidNeedDarkCurrent As Integer = 130
        Public Const dpidNeedDensityFilterChange As Integer = 131
        Public Const dpidSpectrometerModel As Integer = 132
        Public Const dpidLine1FlipFlop As Integer = 133
        Public Const dpidTimer As Integer = 134
        Public Const dpidInterfaceType As Integer = 135
        Public Const dpidInterfaceOption As Integer = 136
        Public Const dpidInitialized As Integer = 137
        Public Const dpidDCRemeasureReasons As Integer = 138
        Public Const dpidAbortWaitForTrigger As Integer = 140
        Public Const dpidGetFilesFromDevice As Integer = 142
        Public Const dpidTOPType As Integer = 143
        Public Const dpidTOPSerialEx As Integer = 144
        'dpidTriggerCapabilities TriggerCapabilities constants  
        Public Const tcoCanTrigger As Integer = &H1
        Public Const tcoTriggerDelay As Integer = &H2
        Public Const tcoTriggerOnlyWhenReady As Integer = &H4
        Public Const tcoAutoRangeTriggering As Integer = &H8
        Public Const tcoShowBusyState As Integer = &H10
        Public Const tcoShowACQState As Integer = &H20
        Public Const tcoFlashOutput As Integer = &H40
        Public Const tcoFlashHardware As Integer = &H80
        Public Const tcoFlashForEachAverage As Integer = &H100
        Public Const tcoFlashDelay As Integer = &H200
        Public Const tcoFlashDelayNegative As Integer = &H400
        Public Const tcoFlashSoftware As Integer = &H800
        Public Const tcoGetFlipFlopState As Integer = &H1000
        'DCRemeasureReasons constants; seedpidDCRemeasureReasons   
        Public Const todcrrNeedDarkCurrent As Integer = &H1
        Public Const todcrrCCDTemperature As Integer = &H2
        'TOPType constants; see dpidTOPType  
        Public Const ttNone As Integer = 0
        Public Const ttTOP100 As Integer = 1
        Public Const ttTOP200 As Integer = 2
        Public Const ttTOP150 As Integer = 3
        Public Const ttTOPLumiTOP As Integer = 4
        <DllImport(ModuleName)> _
        Public Shared Function casGetDeviceParameter(ADevice As Integer, AWhat As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casSetDeviceParameter(ADevice As Integer, AWhat As Integer, AValue As Double) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetDeviceParameterString(ADevice As Integer, AWhat As Integer, ADest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casSetDeviceParameterString(ADevice As Integer, AWhat As Integer, AValue As String) As Integer
        End Function
        Public Const casSerialComplete As Integer = 0
        Public Const casSerialAccessory As Integer = 1
        Public Const casSerialExtInfo As Integer = 2
        Public Const casSerialDevice As Integer = 3
        Public Const casSerialTOP As Integer = 4
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetSerialNumberEx(ADevice As Integer, AWhat As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        Public Const coShutter As Integer = &H1
        Public Const coFilter As Integer = &H2
        Public Const coGetShutter As Integer = &H4
        Public Const coGetFilter As Integer = &H8
        Public Const coGetAccessories As Integer = &H10
        Public Const coGetTemperature As Integer = &H20
        Public Const coUseDarkcurrentArray As Integer = &H40
        Public Const coUseTransmission As Integer = &H80
        Public Const coAutorangeMeasurement As Integer = &H100
        Public Const coAutorangeFilter As Integer = &H200
        Public Const coCheckCalibConfigSerials As Integer = &H400
        Public Const coTOPHasFieldOfViewConfig As Integer = &H800
        Public Const coAutoRemeasureDC As Integer = &H1000
        Public Const coCanMultiTrack As Integer = &H8000
        Public Const coCanSwitchLEDOff As Integer = &H10000
        Public Const coLEDOffWhileMeasuring As Integer = &H20000
        <DllImport(ModuleName)> _
        Public Shared Function casGetOptions(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casSetOptionsOnOff(ADevice As Integer, AOptions As Integer, AOnOff As Integer)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Sub casSetOptions(ADevice As Integer, AOptions As Integer)
        End Sub

        ' Measurement commands  
        <DllImport(ModuleName)> _
        Public Shared Function casMeasure(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casStart(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casFIFOHasData(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetFIFOData(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casMeasureDarkCurrent(ADevice As Integer) As Integer
        End Function
        Public Const paPrepareMeasurement As Integer = 1
        Public Const paLoadCalibration As Integer = 3
        Public Const paCheckAccessories As Integer = 4
        <DllImport(ModuleName)> _
        Public Shared Function casPerformAction(ADevice As Integer, AID As Integer) As Integer
        End Function

        'Measurement Parameter  

        'AWhat parameter constants for MeasurementParameter methods below  
        Public Const mpidIntegrationTime As Integer = 1
        Public Const mpidAverages As Integer = 2
        Public Const mpidTriggerDelayTime As Integer = 3
        Public Const mpidTriggerTimeout As Integer = 4
        Public Const mpidCheckStart As Integer = 5
        Public Const mpidCheckStop As Integer = 6
        Public Const mpidColormetricStart As Integer = 7
        Public Const mpidColormetricStop As Integer = 8
        Public Const mpidACQTime As Integer = 10
        Public Const mpidMaxADCValue As Integer = 11
        Public Const mpidMaxADCPixel As Integer = 12
        Public Const mpidTriggerSource As Integer = 14
        Public Const mpidAmpOffset As Integer = 15
        Public Const mpidSkipLevel As Integer = 16
        Public Const mpidSkipLevelEnabled As Integer = 17
        Public Const mpidScanStartTime As Integer = 18
        Public Const mpidAutoRangeMaxIntTime As Integer = 19
        Public Const mpidAutoRangeLevel As Integer = 20
        'deprecated; use mpidAutoRangeMinLevel below  
        Public Const mpidAutoRangeMinLevel As Integer = 20
        Public Const mpidDensityFilter As Integer = 21
        Public Const mpidCurrentDensityFilter As Integer = 22
        Public Const mpidNewDensityFilter As Integer = 23
        Public Const mpidLastDCAge As Integer = 24
        Public Const mpidRelSaturation As Integer = 25
        Public Const mpidPulseWidth As Integer = 27
        Public Const mpidRemeasureDCInterval As Integer = 28
        Public Const mpidFlashDelayTime As Integer = 29
        Public Const mpidTOPAperture As Integer = 30
        Public Const mpidTOPDistance As Integer = 31
        Public Const mpidTOPSpotSize As Integer = 32
        Public Const mpidTriggerOptions As Integer = 33
        Public Const mpidForceFilter As Integer = 34
        Public Const mpidFlashType As Integer = 35
        Public Const mpidFlashOptions As Integer = 36
        Public Const mpidACQStateLine As Integer = 37
        Public Const mpidACQStateLinePolarity As Integer = 38
        Public Const mpidBusyStateLine As Integer = 39
        Public Const mpidBusyStateLinePolarity As Integer = 40
        Public Const mpidAutoFlowTime As Integer = 41
        Public Const mpidCRIMode As Integer = 42
        Public Const mpidObserver As Integer = 43
        Public Const mpidTOPFieldOfView As Integer = 44
        Public Const mpidCurrentCCDTemperature As Integer = 46
        Public Const mpidLastCCDTemperature As Integer = 47
        Public Const mpidDCCCDTemperature As Integer = 48
        Public Const mpidAutoRangeMaxLevel As Integer = 49

        'mpidTriggerOptions constants  
        Public Const toAcceptOnlyWhenReady As Integer = 1
        Public Const toForEachAutoRangeTrial As Integer = 2
        Public Const toShowBusyState As Integer = 4
        Public Const toShowACQState As Integer = 8

        'mpidFlashType constants  
        Public Const ftNone As Integer = 0
        Public Const ftHardware As Integer = 1
        Public Const ftSoftware As Integer = 2

        'mpidFlashOptions constants  
        Public Const foEveryAverage As Integer = 1

        'mpidTriggerSource  
        Public Const trgSoftware As Integer = 0
        Public Const trgFlipFlop As Integer = 3

        'mpidCRIMode  
        Public Const criDIN6169 As Integer = 0
        Public Const criCIE13_3_95 As Integer = 1

        'mpidObserver  
        Public Const cieObserver1931 As Integer = 0
        Public Const cieObserver1964 As Integer = 1
        <DllImport(ModuleName)> _
        Public Shared Function casGetMeasurementParameter(ADevice As Integer, AWhat As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casSetMeasurementParameter(ADevice As Integer, AWhat As Integer, AValue As Double) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casClearDarkCurrent(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casDeleteParamSet(ADevice As Integer, AParamSet As Integer) As Integer
        End Function

        'Filter and Shutter commands  
        Public Const casShutterInvalid As Integer = -1
        Public Const casShutterOpen As Integer = 0
        Public Const casShutterClose As Integer = 1

        <DllImport(ModuleName)> _
        Public Shared Function casGetShutter(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casSetShutter(ADevice As Integer, OnOff As Integer)
        End Sub
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetFilterName(ADevice As Integer, AFilter As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetDigitalOut(ADevice As Integer, APort As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casSetDigitalOut(ADevice As Integer, APort As Integer, OnOff As Integer)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Function casGetDigitalIn(ADevice As Integer, APort As Integer) As Integer
        End Function

        'Calibration and Configuration Commands  
        <DllImport(ModuleName)> _
        Public Shared Sub casCalculateCorrectedData(ADevice As Integer)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Sub casConvoluteTransmission(ADevice As Integer)
        End Sub

        Public Const gcfDensityFunction As Integer = 0
        Public Const gcfSensitivityFunction As Integer = 1
        Public Const gcfTransmissionFunction As Integer = 2
        Public Const gcfDensityFactor As Integer = 3
        Public Const gcfTOPApertureFactor As Integer = 4
        Public Const gcfTOPDistanceFactor As Integer = 5
        Public Const gcfTDCount As Integer = -1
        Public Const gcfTDExtraDistance As Integer = 1
        Public Const gcfTDExtraFactor As Integer = 2
        Public Const gcfWLCalibrationChannel As Integer = 6
        Public Const gcfWLCalibPointCount As Integer = -1
        Public Const gcfWLExtraCalibrationDelete As Integer = 1
        Public Const gcfWLExtraCalibrationDeleteAll As Integer = 2
        Public Const gcfWLCalibrationAlias As Integer = 7
        Public Const gcfWLCalibrationSave As Integer = 8
        Public Const gcfDarkArrayValues As Integer = 9
        Public Const gcfDarkArrayDepth As Integer = -1
        'Extra  
        Public Const gcfDarkArrayIntTime As Integer = -2
        'Extra  
        Public Const gcfTOPParameter As Integer = 11
        Public Const gcfTOPApertureSize As Integer = 0
        'Extra  
        Public Const gcfTOPSpotSizeDenominator As Integer = 1
        Public Const gcfTOPSpotSizeOffset As Integer = 2
        Public Const gcfLinearityFunction As Integer = 12
        Public Const gcfLinearityCounts As Integer = 0
        Public Const gcfLinearityFactor As Integer = 1

        <DllImport(ModuleName)> _
        Public Shared Function casGetCalibrationFactors(ADevice As Integer, What As Integer, Index As Integer, Extra As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casSetCalibrationFactors(ADevice As Integer, What As Integer, Index As Integer, Extra As Integer, Value As Double)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Sub casUpdateCalibrations(ADevice As Integer)
        End Sub
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Sub casSaveCalibration(ADevice As Integer, AFileName As String)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Sub casClearCalibration(ADevice As Integer, What As Integer)
        End Sub

        'Measurement Results  
        <DllImport(ModuleName)> _
        Public Shared Function casGetData(ADevice As Integer, AIndex As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetXArray(ADevice As Integer, AIndex As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetDarkCurrent(ADevice As Integer, AIndex As Integer) As Double
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Sub casGetPhotInt(ADevice As Integer, ByRef APhotInt As Double, AUnit As StringBuilder, AUnitMaxLen As Integer)
        End Sub
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Sub casGetRadInt(ADevice As Integer, ByRef ARadInt As Double, AUnit As StringBuilder, AUnitMaxLen As Integer)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Function casGetCentroid(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casGetPeak(ADevice As Integer, ByRef x As Double, ByRef y As Double)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Function casGetWidth(ADevice As Integer) As Double
        End Function

        Public Const cLambdaWidth As Integer = 0
        Public Const cLambdaLow As Integer = 1
        Public Const cLambdaMiddle As Integer = 2
        Public Const cLambdaHigh As Integer = 3
        Public Const cLambdaOuterWidth As Integer = 4
        Public Const cLambdaOuterLow As Integer = 5
        Public Const cLambdaOuterMiddle As Integer = 6
        Public Const cLambdaOuterHigh As Integer = 7

        <DllImport(ModuleName)> _
        Public Shared Function casGetWidthEx(ADevice As Integer, What As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casGetColorCoordinates(ADevice As Integer, ByRef x As Double, ByRef y As Double, ByRef z As Double, ByRef u As Double, ByRef v1976 As Double, ByRef v1960 As Double)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Function casGetCCT(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetCRI(ADevice As Integer, Index As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casGetTriStimulus(ADevice As Integer, ByRef X As Double, ByRef Y As Double, ByRef Z As Double)
        End Sub

        Public Const ecvVisualEffect As Integer = 2
        Public Const ecvUVA As Integer = 3
        Public Const ecvUVB As Integer = 4
        Public Const ecvUVC As Integer = 5
        Public Const ecvVIS As Integer = 6
        Public Const ecvCRICCT As Integer = 7
        Public Const ecvCDI As Integer = 8
        Public Const ecvDistance As Integer = 9
        Public Const ecvCalibMin As Integer = 10
        Public Const ecvCalibMax As Integer = 11
        Public Const ecvScotopicInt As Integer = 12
        Public Const ecvCRIFirst As Integer = 100
        Public Const ecvCRILast As Integer = 116
        Public Const ecvCRITriKXFirst As Integer = 120
        Public Const ecvCRITriKXLast As Integer = 136
        Public Const ecvCRITriKYFirst As Integer = 140
        Public Const ecvCRITriKYLast As Integer = 156
        Public Const ecvCRITriKZFirst As Integer = 160
        Public Const ecvCRITriKZLast As Integer = 176
        Public Const ecvCRITriRXordUFirst As Integer = 180
        Public Const ecvCRITriRXordULast As Integer = 196
        Public Const ecvCRITriRYordVFirst As Integer = 200
        Public Const ecvCRITriRYordVLast As Integer = 216
        Public Const ecvCRITriRZordWFirst As Integer = 220
        Public Const ecvCRITriRZordWLast As Integer = 236
        <DllImport(ModuleName)> _
        Public Shared Function casGetExtendedColorValues(ADevice As Integer, What As Integer) As Double
        End Function

        'Colormetric Calculation  
        <DllImport(ModuleName)> _
        Public Shared Function casColorMetric(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casCalculateCRI(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function cmXYToDominantWavelength(x As Double, y As Double, IllX As Double, IllY As Double, ByRef LambdaDom As Double, ByRef Purity As Double) As Integer
        End Function

        'Utilities  
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetDLLFileName(Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casGetDLLVersionNumber(Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casSaveSpectrum(ADevice As Integer, AFileName As String) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetExternalADCValue(ADevice As Integer, AIndex As Integer) As Double
        End Function

        Public Const extNoError As Integer = 0
        Public Const extExternalError As Integer = 1
        Public Const extFilterBlink As Integer = 2
        Public Const extShutterBlink As Integer = 4

        <DllImport(ModuleName)> _
        Public Shared Sub casSetStatusLED(ADevice As Integer, AWhat As Integer)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Function casStopTime(ADevice As Integer, ARefTime As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casNmToPixel(ADevice As Integer, nm As Double) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casPixelToNm(ADevice As Integer, APixel As Integer) As Double
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casCalculateTOPParameter(ADevice As Integer, AAperture As Integer, ADistance As Double, ByRef ASpotSize As Double, ByRef AFieldOfView As Double) As Integer
        End Function

        'MultiTrack  
        <DllImport(ModuleName)> _
        Public Shared Function casMultiTrackInit(ADevice As Integer, ATracks As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casMultiTrackDone(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casMultiTrackCount(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Sub casMultiTrackCopySet(ADevice As Integer)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Function casMultiTrackReadData(ADevice As Integer, ATrack As Integer) As Integer
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casMultiTrackCopyData(ADevice As Integer, ATrack As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casMultiTrackSaveData(ADevice As Integer, AFileName As String) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Function casMultiTrackLoadData(ADevice As Integer, AFileName As String) As Integer
        End Function

        'Spectrum Manipulation  
        <DllImport(ModuleName)> _
        Public Shared Sub casSetData(ADevice As Integer, AIndex As Integer, Value As Double)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Sub casSetXArray(ADevice As Integer, AIndex As Integer, Value As Double)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Sub casSetDarkCurrent(ADevice As Integer, AIndex As Integer, Value As Double)
        End Sub
        <DllImport(ModuleName)> _
        Public Shared Function casGetDataPtr(ADevice As Integer) As IntPtr
        End Function
        <DllImport(ModuleName)> _
        Public Shared Function casGetXPtr(ADevice As Integer) As IntPtr
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True)> _
        Public Shared Sub casLoadTestData(ADevice As Integer, AFileName As String)
        End Sub


        'deprecated methods!!  
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetInitialized(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetDeviceType(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetDeviceOption(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetAdcBits(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetAdcRange(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetSerialNumber(ADevice As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetDeadPixels(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetVisiblePixels(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetPixels(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetModel(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetAmpOffset(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetIntTimeMin(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetIntTimeMax(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casBackgroundMeasure(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetIntegrationTime(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetIntegrationTime(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetAccumulations(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetAccumulations(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetAutoIntegrationLevel(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetAutoIntegrationLevel(ADevice As Integer, ALevel As Double)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetAutoIntegrationTimeMax(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetAutoIntegrationTimeMax(ADevice As Integer, AMaxTime As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casClearBackground(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetNeedBackground(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetNeedBackground(ADevice As Integer, AValue As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetTop100(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetTop100(ADevice As Integer, AIndex As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetTop100Distance(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetTop100Distance(ADevice As Integer, ADistance As Double)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetFilter(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetFilter(ADevice As Integer, AFilter As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetActualFilter(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetNewDensityFilter(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetNewDensityFilter(ADevice As Integer, AFilter As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetForceFilter(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetForceFilter(ADevice As Integer, AForce As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetParamSets(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetParamSets(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetParamSet(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetParamSet(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetCalibrationFileName(ADevice As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetCalibrationFileName(ADevice As Integer, Value As String)
        End Sub
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetConfigFileName(ADevice As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetConfigFileName(ADevice As Integer, Value As String)
        End Sub
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetTransmissionFileName(ADevice As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetTransmissionFileName(ADevice As Integer, Value As String)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casValidateConfigAndCalibFile(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetCalibrationUnit(ADevice As Integer, Dest As StringBuilder, AMaxLen As Integer) As Integer
        End Function
        <DllImport(ModuleName, CharSet:=CharSet.Ansi, ExactSpelling:=True), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetCalibrationUnit(ADevice As Integer, Value As String)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetBackground(ADevice As Integer, AIndex As Integer) As Double
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetBackground(ADevice As Integer, AIndex As Integer, Value As Double)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetMaxAdcValue(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetCheckStart(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetCheckStart(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetCheckStop(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetCheckStop(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetColormetricStart(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetColormetricStart(ADevice As Integer, Value As Double)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetColormetricStop(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetColormetricStop(ADevice As Integer, Value As Double)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetObserver() As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetObserver(AObserver As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetSkipLevel(ADevice As Integer) As Double
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetSkipLevel(ADevice As Integer, ASkipLevel As Double)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetSkipLevelEnabled(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetSkipLevelEnabled(ADevice As Integer, ASkipLevel As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetTriggerSource(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetTriggerSource(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetLine1FlipFlop(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetLine1FlipFlop(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetTimeout(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetTimeout(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetFlash(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetFlash(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetFlashDelayTime(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetFlashDelayTime(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetFlashOptions(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetFlashOptions(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetDelayTime(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Sub casSetDelayTime(ADevice As Integer, Value As Integer)
        End Sub
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetStartTime(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casGetACQTime(ADevice As Integer) As Integer
        End Function
        <DllImport(ModuleName), ObsoleteAttribute("method obsolete!")> _
        Public Shared Function casReadWatch(ADevice As Integer) As Integer
        End Function
    End Class
End Namespace

