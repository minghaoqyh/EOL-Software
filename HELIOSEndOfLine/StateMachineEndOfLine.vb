Imports System
Imports System.IO
Imports HELIOSEndOfLine.SequenceStep
Imports Spline
Imports MathNet.Numerics.Interpolation
Imports Microsoft.Office.Interop
Imports System.ComponentModel
Imports Microsoft.VisualBasic.FileIO


Public Class StateMachineEndOfLine
    Inherits StateMachine
#Region "defines"
    Private Const RED_LAMBDADOM_A3 As Double = -3268.38754123805
    Private Const RED_LAMBDADOM_A2 As Double = 389.939577125107
    Private Const RED_LAMBDADOM_A1 As Double = -98.5022032373655
    Private Const RED_LAMBDADOM_A0 As Double = 610.121239760893

    Private Const GRE_LAMBDADOM_A3 As Double = -11.8375233279469
    Private Const GRE_LAMBDADOM_A2 As Double = -58.4468132312786
    Private Const GRE_LAMBDADOM_A1 As Double = 44.7556929666063
    Private Const GRE_LAMBDADOM_A0 As Double = 554.425448365825

    Private Const BLU_LAMBDADOM_A3 As Double = -28.52946177
    Private Const BLU_LAMBDADOM_A2 As Double = 91.02364511
    Private Const BLU_LAMBDADOM_A1 As Double = -117.62023
    Private Const BLU_LAMBDADOM_A0 As Double = 532.2237204

    Private Const RED_VLAMBDA_A3 As Double = 0.00000139627
    Private Const RED_VLAMBDA_A2 As Double = -0.002540562
    Private Const RED_VLAMBDA_A1 As Double = 1.528145652
    Private Const RED_VLAMBDA_A0 As Double = -303.2487616

    Private Const GRE_VLAMBDA_A3 As Double = 0.000000793445
    Private Const GRE_VLAMBDA_A2 As Double = -0.001536995
    Private Const GRE_VLAMBDA_A1 As Double = 0.97258864
    Private Const GRE_VLAMBDA_A0 As Double = -200.99534

    Private Const BLU_VLAMBDA_A3 As Double = 0.000000751868
    Private Const BLU_VLAMBDA_A2 As Double = -0.000982811
    Private Const BLU_VLAMBDA_A1 As Double = 0.429487954
    Private Const BLU_VLAMBDA_A0 As Double = -62.72623533

    Private Const RED_PD_LAMBDADOM_A3 As Double = 0.0
    Private Const RED_PD_LAMBDADOM_A2 As Double = -0.000044642857143
    Private Const RED_PD_LAMBDADOM_A1 As Double = 0.050432142857
    Private Const RED_PD_LAMBDADOM_A0 As Double = -13.230357143

    Private Const GRE_PD_LAMBDADOM_A3 As Double = 0.0
    Private Const GRE_PD_LAMBDADOM_A2 As Double = -0.000035952380952
    Private Const GRE_PD_LAMBDADOM_A1 As Double = 0.041754761905
    Private Const GRE_PD_LAMBDADOM_A0 As Double = -11.126571429

    Private Const BLU_PD_LAMBDADOM_A3 As Double = 0.0
    Private Const BLU_PD_LAMBDADOM_A2 As Double = 0.0000095238095238
    Private Const BLU_PD_LAMBDADOM_A1 As Double = 0.00016666666667
    Private Const BLU_PD_LAMBDADOM_A0 As Double = -1.6657142857

    Private Const PD_TEMPSENSOR_A3 As Double = 0.0
    Private Const PD_TEMPSENSOR_A2 As Double = 0.000000520833437500024
    Private Const PD_TEMPSENSOR_A1 As Double = 0.0016458333275
    Private Const PD_TEMPSENSOR_A0 As Double = 0.9599999997

    Public Const CAN_DEST As Integer = 3
    Public Const STEP_COUNT As Integer = 16
#End Region
#Region "custom variables"
    Enum smhStatus
        ReadyForStart
        WorkingGood
        WorkingBad
        FinishGood
        FinishBad
        Finished
    End Enum

    Enum statusLight
        Green
        Yellow
        Red
    End Enum

    Private m_measurementList As List(Of Measurement)
    Private m_calibList As List(Of Calibration)
    Private m_integrationTime As Integer
    Private m_oHeliosCommunicationBoard As HELIOSCommunication.HELIOSCommunication
    Public Property HeliosCommunicationBoard() As HELIOSCommunication.HELIOSCommunication
        Get
            Return m_oHeliosCommunicationBoard
        End Get
        Set(ByVal value As HELIOSCommunication.HELIOSCommunication)
            m_oHeliosCommunicationBoard = value
        End Set
    End Property
    Private m_status As smhStatus
    Public Property Status() As smhStatus
        Get
            Return m_status
        End Get
        Set(ByVal value As smhStatus)
            m_status = value
        End Set
    End Property
    Private m_smData As StateMachineEndOfLineData
    Public Property smData() As StateMachineEndOfLineData
        Get
            Return m_smData
        End Get
        Set(ByVal value As StateMachineEndOfLineData)
            m_smData = value
        End Set
    End Property

    Private m_dataCASMeas As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeasRed14 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeasRed50 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeasGre14 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeasGre50 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeasBlu14 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeasBlu50 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas100CCT30 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas100CCT55 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas100CCT65 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas20CCT30 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas20CCT55 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas20CCT65 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas5CCT55 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas100CCT55low As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeas100SuperR9 As DataCASMeas = New DataCASMeas()
    Private m_dataCasMeasUV As DataCASMeas = New DataCASMeas()

    Private m_dataHelios As DataHelios = New DataHelios()
    Private m_dataHeliosRed14 As DataHelios = New DataHelios()
    Private m_dataHeliosRed50 As DataHelios = New DataHelios()
    Private m_dataHeliosGre14 As DataHelios = New DataHelios()
    Private m_dataHeliosGre50 As DataHelios = New DataHelios()
    Private m_dataHeliosBlu14 As DataHelios = New DataHelios()
    Private m_dataHeliosBlu50 As DataHelios = New DataHelios()
    Private m_dataHelios100CCT30 As DataHelios = New DataHelios()
    Private m_dataHelios100CCT55 As DataHelios = New DataHelios()
    Private m_dataHelios100CCT65 As DataHelios = New DataHelios()
    Private m_dataHelios20CCT30 As DataHelios = New DataHelios()
    Private m_dataHelios20CCT55 As DataHelios = New DataHelios()
    Private m_dataHelios20CCT65 As DataHelios = New DataHelios()
    Private m_dataHelios5CCT55 As DataHelios = New DataHelios()
    Private m_dataHelios100CCT55low As DataHelios = New DataHelios()
    Private m_dataHelios100SuperR9 As DataHelios = New DataHelios()
    Private m_dataHeliosUV As DataHelios = New DataHelios()

    Private m_dataPSSby As New DataPS
    Private m_dataPSSbyV As New DataPS
    Private m_dataPS100 As New DataPS
    Private m_dataPSV As New DataPS

    Private m_dataCalib As New DataCalib

    Public m_cas140 As New CasCommunication.cCAS140
    Public CasConfig As New CasCommunication.CAS140IniFile
    Public powerSupply As HardwareCommunication.TcpKeysightN5767Communication
    Public dioPorts As HardwareCommunication.NI6520_DAQ

    Private snReferenceSample As Integer



    Private WithEvents m_oBackgroundWorker As BackgroundWorker

#End Region

#Region "Helper functions that have to be customised"

    Public Event newStateMachineStatus(ByVal sender As Object, ByVal status As StateMachineStatus)
    Public Event addLogFile(ByVal sender As Object, ByVal text As String)
    Public Event changeStatusLight(ByVal sender As Object, ByVal statusLight As statusLight)

    Public Sub LoadProgramFlowHelios(ByVal filename As String)
        Dim objReader As New StreamReader(filename)
        Dim sLine As String
        Dim arrText() As String
        Dim loopText() As String
        ProgramFlow.Clear()
        MaxSteps = 0

        Dim tempSaveSteps As Boolean
        Dim loop1 As List(Of String) = New List(Of String)
        Dim loop2 As List(Of String) = New List(Of String)
        Dim replaceText1 As String = ""
        Dim replaceText2 As String = ""
        Dim tempProgramFlow As List(Of SequenceStep) = New List(Of SequenceStep)
        Do
            sLine = objReader.ReadLine()
            If Not (sLine Is Nothing Or sLine = "") Then

                'copy data to string array
                arrText = Split(sLine, " ")
                Dim LastNonEmpty As Integer = -1
                For i As Integer = 0 To arrText.Length - 1
                    If arrText(i) <> "" Then
                        LastNonEmpty += 1
                        arrText(LastNonEmpty) = arrText(i)
                    End If
                Next
                ReDim Preserve arrText(LastNonEmpty)

                Dim stepName As String
                stepName = arrText(0)

                Select Case stepName.ToLower

                    Case "{".ToLower
                        tempSaveSteps = True
                        tempProgramFlow.Clear()
                        'setze trigger, dass die naechsten Befehle aller wiederholt werden
                    Case "}1".ToLower
                        Dim stepCount As Integer = tempProgramFlow.Count
                        For Each str As String In loop1
                            For ii = 0 To stepCount - 1
                                Dim tempStepName As String
                                Dim tempDelegate As StepDelegate
                                Dim tempData0 As Object
                                Dim tempData1 As Object
                                Dim tempData2 As Object

                                tempStepName = tempProgramFlow.Item(ii).StepName
                                tempDelegate = tempProgramFlow.Item(ii).StepDele
                                tempData0 = tempProgramFlow.Item(ii).Data0
                                tempData1 = tempProgramFlow.Item(ii).Data1
                                tempData2 = tempProgramFlow.Item(ii).Data2
                                If TypeOf tempProgramFlow.Item(ii).Data0 Is String Then
                                    If tempProgramFlow.Item(ii).Data0 = "loop1" Then
                                        tempData0 = str
                                    End If
                                End If
                                If TypeOf tempProgramFlow.Item(ii).Data1 Is String Then
                                    If tempProgramFlow.Item(ii).Data1 = "loop1" Then
                                        tempData1 = str
                                    End If
                                End If
                                If TypeOf tempProgramFlow.Item(ii).Data2 Is String Then
                                    If tempProgramFlow.Item(ii).Data2 = "loop1" Then
                                        tempData2 = str
                                    End If
                                End If
                                ProgramFlow.Add(New SequenceStep(tempStepName, tempDelegate, tempData0, tempData1, tempData2))

                            Next
                        Next
                        tempSaveSteps = False

                        'ende der schleife. Wiederhole alle Befehle jetzt anhand der indizes und ersetze die Variablen

                    Case "}2".ToLower
                        Dim stepCount As Integer = tempProgramFlow.Count
                        For Each str As String In loop1
                            For Each str2 As String In loop2
                                For ii = 0 To stepCount - 1
                                    Dim tempStepName As String
                                    Dim tempDelegate As StepDelegate
                                    Dim tempData0 As Object
                                    Dim tempData1 As Object
                                    Dim tempData2 As Object

                                    tempStepName = tempProgramFlow.Item(ii).StepName
                                    tempDelegate = tempProgramFlow.Item(ii).StepDele
                                    tempData0 = tempProgramFlow.Item(ii).Data0
                                    tempData1 = tempProgramFlow.Item(ii).Data1
                                    tempData2 = tempProgramFlow.Item(ii).Data2
                                    If TypeOf tempProgramFlow.Item(ii).Data0 Is String Then
                                        If tempProgramFlow.Item(ii).Data0 = "loop1" Then
                                            tempData0 = str
                                        End If
                                    End If
                                    If TypeOf tempProgramFlow.Item(ii).Data1 Is String Then
                                        If tempProgramFlow.Item(ii).Data1 = "loop1" Then
                                            tempData1 = str
                                        End If
                                    End If
                                    If TypeOf tempProgramFlow.Item(ii).Data2 Is String Then
                                        If tempProgramFlow.Item(ii).Data2 = "loop1" Then
                                            tempData2 = str
                                        End If
                                    End If
                                    If TypeOf tempProgramFlow.Item(ii).Data0 Is String Then
                                        If tempProgramFlow.Item(ii).Data0 = "loop2" Then
                                            tempData0 = str2
                                        End If
                                    End If
                                    If TypeOf tempProgramFlow.Item(ii).Data1 Is String Then
                                        If tempProgramFlow.Item(ii).Data1 = "loop2" Then
                                            tempData1 = str2
                                        End If
                                    End If
                                    If TypeOf tempProgramFlow.Item(ii).Data2 Is String Then
                                        If tempProgramFlow.Item(ii).Data2 = "loop2" Then
                                            tempData2 = str2
                                        End If
                                    End If
                                    ProgramFlow.Add(New SequenceStep(tempStepName, tempDelegate, tempData0, tempData1, tempData2))

                                Next
                            Next
                        Next
                        tempSaveSteps = False
                    Case "calibCx".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibCx, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibCx, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibCy".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibCy, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibCy, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibDacMax".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibDacMax, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibDacMax, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibIByDac".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibIByDAC, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibIByDAC, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibIDac".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibIDac, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibIDac, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibPhiADC".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibPhiADC, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibPhiADC, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibPhi".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibPhi, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibPhi, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibPhiMax".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibPhiMax, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibPhiMax, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibSuperR9".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibSuperR9, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "calibTempChip".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibTempChip, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibTempChip, arrText(1), Nothing, Nothing))
                        End If
                    Case "calibUByI".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibUbyI, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalibUbyI, arrText(1), Nothing, Nothing))
                        End If
                    Case "calcAdditionalValuesRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalcAdditionalValuesRef, arrText(1), arrText(2), Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalcAdditionalValuesRef, arrText(1), arrText(2), Nothing))
                        End If
                    Case "calcIntegrationTimes".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalcIntegrationTimes, arrText(1), arrText(2), Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalcIntegrationTimes, arrText(1), arrText(2), Nothing))
                        End If
                    Case "calcLimitsRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalcLimitsRef, arrText(1), arrText(2), Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCalcLimitsRef, arrText(1), arrText(2), Nothing))
                        End If
                    Case "checkBoxClosed".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckBoxClosed, Nothing, Nothing, Nothing))
                    Case "checkCalibLimitsRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckCalibLimitsRef, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckCalibLimitsRef, arrText(1), Nothing, Nothing))
                        End If
                    Case "checkCalibSuperR9LimitsRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckCalibSuperR9LimitsRef, Nothing, Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckCalibSuperR9LimitsRef, Nothing, Nothing, Nothing))
                        End If
                    Case "checkCommunicationHelios".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckCommunicationHelios, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "checkCommunicationCas".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckCommunicationCas, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "checkCommunicationPS".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckCommunicationPS, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "checkGeneralFunction".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckGeneralFunction, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "checkGeneralFunctionV".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckGeneralFunctionV, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "checkIfAllIsGood".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckIfAllIsGood, Nothing, Nothing, Nothing))
                    Case "checkIfInDebug".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckIfInDebug, GetVariableInstance(arrText(1)), arrText(2), Nothing))
                    Case "checkLimitsFinal".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckLimitsFinal, Nothing, Nothing, Nothing))
                    Case "checkLimitsPS".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckLimitsPS, arrText(1), Nothing, Nothing))
                    Case "checkLimitsRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckLimitsRef, arrText(1), arrText(2), Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckLimitsRef, arrText(1), arrText(2), Nothing))
                        End If
                    Case "checkRefMeasLimits".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckRefMeasLimits, Nothing, Nothing, Nothing))
                    Case "checkRefMeasLimitsV".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckRefMeasLimitsV, Nothing, Nothing, Nothing))
                    Case "checkSafetyLabel".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCheckSafetyLabel, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "compareSerialNumber".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepCompareSerialNumber, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "doCalibCalc".ToLower
                        loop1.Clear()
                        'load loop1 with requested colors
                        arrText(1) = arrText(1).Replace("[", "")
                        arrText(1) = arrText(1).Replace("]", "")
                        loopText = Split(arrText(1), "/")
                        For Each str As String In loopText
                            loop1.Add(str)
                        Next
                    Case "doDACCalib".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepDoDACCalib, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "doIntegrationTimeEstimate".ToLower
                        loop1.Clear()
                        'load loop1 with requested colors
                        arrText(1) = arrText(1).Replace("[", "")
                        arrText(1) = arrText(1).Replace("]", "")
                        loopText = Split(arrText(1), "/")
                        For Each str As String In loopText
                            loop1.Add(str)
                        Next
                    Case "doOneMeasurement".ToLower
                        loop1.Clear()
                        arrText(1) = arrText(1).Replace("[", "")
                        arrText(1) = arrText(1).Replace("]", "")
                        loopText = Split(arrText(1), "/")
                        For Each str As String In loopText
                            loop1.Add(str)
                        Next
                        loop2.Clear()
                        arrText(2) = arrText(2).Replace("[", "")
                        arrText(2) = arrText(2).Replace("]", "")
                        loopText = Split(arrText(2), "/")
                        For Each str As String In loopText
                            loop2.Add(str)
                        Next
                    Case "generateVariables".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepGenerateVariables, arrText(1), arrText(2), Nothing))
                    Case "getCalibCurVolDacFromHelios".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepGetCalibCurVolDacFromHelios, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "loadVariablesData".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepLoadVariablesData, Nothing, Nothing, Nothing))
                    Case "loadLimitValues".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepLoadLimitValues, Nothing, Nothing, Nothing))
                    Case "moveLightGuide".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepMoveLightGuide, arrText(1), Nothing, Nothing))
                    Case "readCASForEstimate".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadCasForEstimate, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadCasForEstimate, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        End If
                    Case "readCASRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadCasRef, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadCasRef, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        End If
                    Case "readCasValues".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadCas, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "readPSValues".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadPS, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "readHeliosRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadHeliosRef, arrText(1), arrText(2), arrText(3)))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadHeliosRef, arrText(1), arrText(2), arrText(3)))
                        End If
                    Case "readHeliosValues".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadHeliosValues, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "readSNandLifeTime".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadSNandLifeTime, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "readSNandLifeTimeUV".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepReadSNandLifeTimeUV, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), Nothing))
                    Case "resetDailyCalib".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepResetDailyCalib, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "restart".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepRestartHelios, GetVariableInstance(arrText(1)), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepRestartHelios, GetVariableInstance(arrText(1)), Nothing, Nothing))
                        End If
                    Case "saveCalibValuesToFile".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSaveCalibValuesToFile, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "saveFinalTestDataRGBToFile".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSaveFinalTestDataRGBToFile, arrText(1), Nothing, Nothing))
                    Case "saveGoldenTestDataToFile".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSaveGoldenTestDataToFile, arrText(1), Nothing, Nothing))
                    Case "saveMeasurementToFile".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSaveMeasurementToFile, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "saveUVValuesToFile".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSaveUVValuesToFile, arrText(1), Nothing, Nothing))
                    Case "saveSpectra2File".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSaveSpectra2File, GetVariableInstance(arrText(1)), GetVariableInstance(arrText(2)), arrText(3)))
                    Case "setDebugStatus".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetDebugStatus, GetVariableInstance(arrText(1)), arrText(2), Nothing))
                    Case "setHeliosCCT".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosCCT, GetVariableInstance(arrText(1)), arrText(2), Nothing))
                    Case "setHeliosCurrent".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosCurrent, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                    Case "setHeliosColorIntensity".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosColorIntensity, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosColorIntensity, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        End If
                    Case "setHeliosColorIntensityRef".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosColorIntensityRef, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosColorIntensityRef, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                        End If
                    Case "setHeliosIntensityRGB".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosIntensityRGB, GetVariableInstance(arrText(1)), arrText(2), Nothing))
                    Case "setHeliosIntensityUVDebug".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosIntensityUVDebug, GetVariableInstance(arrText(1)), arrText(2), Nothing))
                    Case "setHeliosSuperR9".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetHeliosSuperR9, GetVariableInstance(arrText(1)), arrText(2), Nothing))
                    Case "setPSStatus".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepSetPSStatus, GetVariableInstance(arrText(1)), arrText(2), arrText(3)))
                    Case "waitForOK".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWaitForOK, arrText(1), Nothing, Nothing))
                    Case "waitSomeTime".ToLower
                        If tempSaveSteps = True Then
                            tempProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWaitSomeTime, arrText(1), Nothing, Nothing))
                        Else
                            ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWaitSomeTime, arrText(1), Nothing, Nothing))
                        End If
                    Case "test".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepTest, Nothing, Nothing, Nothing))
                    Case "waitUntilLightGuideIsMoved".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWaitUntilLightGuideIsMoved, GetVariableInstance(arrText(1)), arrText(2), Nothing))
                    Case "writeCalibValuesToEEPROM".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWriteCalibValuesToEEPROM, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "writeSNToEEPROM".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWriteSNToEEPROM, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "writeSuperR9Values".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWriteSuperR9Values, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case "writeUVDataToEEPROM".ToLower
                        ProgramFlow.Add(New SequenceStep(stepName, AddressOf stepWriteUVDataToEEPROM, GetVariableInstance(arrText(1)), Nothing, Nothing))
                    Case Else
                End Select
            End If
        Loop Until sLine Is Nothing
        MaxSteps = ProgramFlow.Count
        objReader.Close()
    End Sub

    Private Function GetVariableInstance(ByVal vName As String) As Object
        Select Case vName
            Case "can"
                Return Me.m_oHeliosCommunicationBoard
            Case "cas"
                Return Me.m_cas140

            Case "dataCasMeas"
                Return m_dataCASMeas
            Case "dataCasMeas100CCT30"
                Return m_dataCasMeas100CCT30
            Case "dataCasMeas100CCT55"
                Return m_dataCasMeas100CCT55
            Case "dataCasMeas100CCT65"
                Return m_dataCasMeas100CCT65
            Case "dataCasMeas20CCT30"
                Return m_dataCasMeas20CCT30
            Case "dataCasMeas20CCT55"
                Return m_dataCasMeas20CCT55
            Case "dataCasMeas20CCT65"
                Return m_dataCasMeas20CCT65
            Case "dataCasMeasRed14"
                Return m_dataCasMeasRed14
            Case "dataCasMeasRed50"
                Return m_dataCasMeasRed50
            Case "dataCasMeasGre14"
                Return m_dataCasMeasGre14
            Case "dataCasMeasGre50"
                Return m_dataCasMeasGre50
            Case "dataCasMeasBlu14"
                Return m_dataCasMeasBlu14
            Case "dataCasMeasBlu50"
                Return m_dataCasMeasBlu50
            Case "dataCasMeas5CCT55"
                Return m_dataCasMeas5CCT55
            Case "dataCasMeas100CCT55low"
                Return m_dataCasMeas100CCT55low
            Case "dataCasMeas100SuperR9"
                Return m_dataCasMeas100SuperR9
            Case "dataCasMeasUV"
                Return m_dataCasMeasUV

            Case "dataHelios"
                Return m_dataHelios
            Case "dataHelios100CCT30"
                Return m_dataHelios100CCT30
            Case "dataHelios100CCT55"
                Return m_dataHelios100CCT55
            Case "dataHelios100CCT65"
                Return m_dataHelios100CCT65
            Case "dataHelios20CCT30"
                Return m_dataHelios20CCT30
            Case "dataHelios20CCT55"
                Return m_dataHelios20CCT55
            Case "dataHelios20CCT65"
                Return m_dataHelios20CCT65
            Case "dataHeliosRed14"
                Return m_dataHeliosRed14
            Case "dataHeliosRed50"
                Return m_dataHeliosRed50
            Case "dataHeliosGre14"
                Return m_dataHeliosGre14
            Case "dataHeliosGre50"
                Return m_dataHeliosGre50
            Case "dataHeliosBlu14"
                Return m_dataHeliosBlu14
            Case "dataHeliosBlu50"
                Return m_dataHeliosBlu50
            Case "dataHelios5CCT55"
                Return m_dataHelios5CCT55
            Case "dataHelios100CCT55low"
                Return m_dataHelios100CCT55low
            Case "dataHelios100SuperR9"
                Return m_dataHelios100SuperR9
            Case "dataHeliosUV"
                Return m_dataHeliosUV
            Case "dataPSSby"
                Return m_dataPSSby
            Case "dataPSSbyV"
                Return m_dataPSSbyV
            Case "dataPS100"
                Return m_dataPS100
            Case "dataPSV"
                Return m_dataPSV

            Case "dio"
                Return Me.dioPorts
            Case "ps"
                Return Me.powerSupply
            Case "time"
                Return Me.m_integrationTime
            Case Else
                Return Nothing
        End Select
    End Function

    Public Sub ExecuteProgramFlowHelios()
        Me.Status = smhStatus.WorkingGood
        m_oBackgroundWorker = New BackgroundWorker()
        m_oBackgroundWorker.WorkerSupportsCancellation = True
        m_oBackgroundWorker.WorkerReportsProgress = True
        m_oBackgroundWorker.RunWorkerAsync()
    End Sub

    Public Sub SetData(ByVal smData As StateMachineEndOfLineData)
        m_smData = smData
    End Sub

    Private Sub BackgroundWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles m_oBackgroundWorker.DoWork
        Try
            Dim myStep As StepDelegate
            CurrentStep = 0
            RaiseEvent addLogFile(Me, "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%")
            RaiseEvent addLogFile(Me, "%%% SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Execution started. %%%")

            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet = xlsWorkBook.Sheets("Config")
            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing

            currentFind = usedRange.Find("Config2D1", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            smData.Config2D1 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            xlsWorkBook.Close()
            xlsApp.Quit()
            smData.ConfigLastModifiedUtc = IO.File.GetLastWriteTimeUtc(Application.StartupPath & "\" & path)

            RaiseEvent addLogFile(Me, "%% Config 2D1-" & smData.Config2D1 & " - Last modified UTC: " & smData.ConfigLastModifiedUtc.Date.ToShortDateString() & " " & smData.ConfigLastModifiedUtc.ToLongTimeString())

            While ProgramFlow.Count > 0
                CurrentStep = CurrentStep + 1
                myStep = ProgramFlow.Item(0).StepDele
                myStep.Invoke(ProgramFlow.Item(0).StepName,
                              ProgramFlow.Item(0).Data0,
                              ProgramFlow.Item(0).Data1,
                              ProgramFlow.Item(0).Data2)
                ProgramFlow.RemoveAt(0)
                If Me.Status = smhStatus.FinishBad Then
                    ProgramFlow.Clear()

                    RaiseEvent addLogFile(Me, "%%% SN" & smData.CZMSeriennummer.ToString() & " " &
                                              Now.Date.Year.ToString("D4") &
                                              Now.Date.Month.ToString("D2") &
                                              Now.Date.Day.ToString("D2") & " " &
                                              Now.Hour.ToString("D2") &
                                              Now.Minute.ToString("D2") &
                                              Now.Second.ToString("D2") & " " &
                                              CurrentStep.ToString & "/" & MaxSteps.ToString & " Exit: FAIL %%%")

                    Exit While
                End If
            End While

            Dim myProtocol As HardwareCommunication.ProtocolEOLBox = New HardwareCommunication.ProtocolEOLBox
            Dim buffer(20) As Char
            powerSupply.setVoltage(0)
            powerSupply.setCurrent(0)
            powerSupply.setOutputOnOff(False)

            Dim iniReader As IniReader = New IniReader
            Dim comPort As String
            comPort = iniReader.ReadValueFromFile("COM", "port", "", ".\Settings.ini")
            myProtocol.Open(comPort)
            myProtocol.SetLWLPos("out", buffer)
            myProtocol.Close()

            ProgramFlow.Clear()

            If Me.Status = smhStatus.WorkingGood Then
                Me.Status = smhStatus.FinishGood
                RaiseEvent addLogFile(Me, "%%% SN" & smData.CZMSeriennummer.ToString() & " " &
                                          Now.Date.Year.ToString("D4") &
                                          Now.Date.Month.ToString("D2") &
                                          Now.Date.Day.ToString("D2") & " " &
                                          Now.Hour.ToString("D2") &
                                          Now.Minute.ToString("D2") &
                                          Now.Second.ToString("D2") & " " &
                                          CurrentStep.ToString & "/" & MaxSteps.ToString & " Exit: OK %%%" & vbNewLine)
            ElseIf Me.Status = smhStatus.WorkingBad Then
                Me.Status = smhStatus.FinishBad
            Else

            End If

            Select Case smData.ModuleType
                Case 0     ' RGB-Modul Kalibrierung
                    If Me.Status = smhStatus.FinishGood Then
                        'SaveDataToCamstarRGB(True) 'CAMSTAR Database Export
                    Else
                        'SaveDataToCamstarRGB(False) 'CAMSTAR Database Export
                    End If
                Case 1 ' V-Modul Kalibrierung
                    If Me.Status = smhStatus.FinishGood Then
                        'SaveDataToCamstarV(True) 'CAMSTAR Database Export
                    Else
                        'SaveDataToCamstarV(False) 'CAMSTAR Database Export
                    End If
            End Select

            RaiseEvent newStateMachineStatus(Me, New StateMachineStatus("--- Ablauf wurde beendet ---", Drawing.Color.LightGreen, 1))
        Catch ex As Exception
            Try
                powerSupply.setVoltage(0)
                powerSupply.setCurrent(0)
                powerSupply.setOutputOnOff(False)
                RaiseEvent addLogFile(Me, "PS off after error")

                Dim myProtocol As HardwareCommunication.ProtocolEOLBox = New HardwareCommunication.ProtocolEOLBox
                Dim buffer(20) As Char
                Dim iniReader As IniReader = New IniReader
                Dim comPort As String
                comPort = iniReader.ReadValueFromFile("COM", "port", "", ".\Settings.ini")
                myProtocol.Open(comPort)
                myProtocol.SetLWLPos("out", buffer)
                myProtocol.Close()
            Catch ex1 As Exception
                'ignore errors
            End Try

            'write Status to statusText
            RaiseEvent newStateMachineStatus(Me,
                                             New StateMachineStatus(CurrentStep.ToString & "/" & MaxSteps.ToString & " Error. Execution aborted",
                                                                    Color.Red,
                                                                    0))
            Me.Status = smhStatus.Finished
            ProgramFlow.Clear()
            RaiseEvent addLogFile(Me, "%%% SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      CurrentStep.ToString & "/" & MaxSteps.ToString & " aborted. %%%")
        End Try

        System.Media.SystemSounds.Exclamation.Play()
        Threading.Thread.Sleep(1000)
        System.Media.SystemSounds.Exclamation.Play()
        Threading.Thread.Sleep(1000)
        System.Media.SystemSounds.Exclamation.Play()
        Threading.Thread.Sleep(1000)
        System.Media.SystemSounds.Exclamation.Play()
        Threading.Thread.Sleep(1000)
    End Sub

#End Region

    Private Function SaveDataToCamstarRGB(result As Boolean) As Boolean
        'Bezeichner und Werte als Strings je maximal 30 Zeichen!
        Dim tempString As String = ""

        tempString += "PASS=" & result.ToString() & vbNewLine
        tempString += "DateTime=" & Now.Date.ToShortDateString() & " " & Now.ToLongTimeString() & vbNewLine
        tempString += "User=" & Environment.UserDomainName & "\" & Environment.UserName & vbNewLine
        tempString += "Config2D1=" & smData.Config2D1 & vbNewLine
        tempString += "ConfigLastModifiedUTC=" & smData.ConfigLastModifiedUtc.Date.ToShortDateString() & " " & smData.ConfigLastModifiedUtc.ToLongTimeString() & vbNewLine
        'tempString += "FailedEC=0" & vbNewLine  'TODO

        tempString += "UinSby-EC1=" & m_dataPSSby.Voltage.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "IinSby-EC3=" & m_dataPSSby.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine

        tempString += "PD5dark-EC4=" & m_dataHelios.PD5dark.ToString("0") & vbNewLine
        tempString += "PD5-EC12=" & m_dataHelios.PD5.ToString("0") & vbNewLine
        tempString += "TempPCB1-EC23=" & m_dataHelios.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempSensor1-EC25=" & m_dataHelios.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine

        tempString += "SNtoEEPROM-EC27=" & m_dataHelios.SN_saved.ToString("0") & vbNewLine
        tempString += "Lifetime-EC31=" & m_dataHelios.BurnTime.ToString("0") & vbNewLine
        tempString += "SWmain-EC35=" & m_dataHelios.Version_main.ToString("0") & vbNewLine
        tempString += "SWsub-EC37=" & m_dataHelios.Version_sub.ToString("0") & vbNewLine
        tempString += "SWbuild-EC38=" & m_dataHelios.Version_build.ToString("0") & vbNewLine

        tempString += "CalibDAC-EC39=" & m_dataCalib.CalibDacDone.ToString("0") & vbNewLine
        tempString += "CalibDACtoEEPROM-EC41=" & m_dataCalib.CalibDacSaved.ToString("0") & vbNewLine
        tempString += "Calib-EC43=" & m_dataCalib.CalibDone.ToString("0") & vbNewLine
        tempString += "CalibToEEPROM-EC45=" & m_dataCalib.CalibSaved.ToString("0") & vbNewLine
        tempString += "CalibR9-EC47=" & m_dataCalib.CalibSuperR9Done.ToString("0") & vbNewLine
        tempString += "CalibR9toEEPROM-EC49=" & m_dataCalib.CalibSuperR9Saved.ToString("0") & vbNewLine
        tempString += "CalibDataCSV-EC51=" & m_dataCalib.CSVfilename & vbNewLine

        tempString += "PhiR14-EC53=" & m_dataCasMeasRed14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CxR14-EC55=" & m_dataCasMeasRed14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CyR14-EC57=" & m_dataCasMeasRed14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "LambdaDomR14-EC59=" & m_dataCasMeasRed14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutVoR14-EC61=" & m_dataHeliosRed14.U1.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrR14-EC63=" & m_dataHeliosRed14.I1.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempR14-EC65=" & m_dataHeliosRed14.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "RefDataR14-EC67=" & m_dataCasMeasRed14.FilenameSpectra & vbNewLine

        tempString += "PhiR50-EC69=" & m_dataCasMeasRed50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CxR50-EC71=" & m_dataCasMeasRed50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CyR50-EC73=" & m_dataCasMeasRed50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "LambdaDomR50-EC75=" & m_dataCasMeasRed50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutVoR50-EC77=" & m_dataHeliosRed50.U1.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrR50-EC79=" & m_dataHeliosRed50.I1.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempR50-EC81=" & m_dataHeliosRed50.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "RefDataR50-EC83=" & m_dataCasMeasRed50.FilenameSpectra & vbNewLine

        tempString += "PhiG14-EC85=" & m_dataCasMeasGre14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CxG14-EC87=" & m_dataCasMeasGre14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CyG14-EC89=" & m_dataCasMeasGre14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "LambdaDomG14-EC91=" & m_dataCasMeasGre14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutVoG14-EC93=" & m_dataHeliosGre14.U2.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrG14-EC95=" & m_dataHeliosGre14.I2.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempG14-EC97=" & m_dataHeliosGre14.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "RefDataG14-EC99=" & m_dataCasMeasGre14.FilenameSpectra & vbNewLine

        tempString += "PhiG50-EC101=" & m_dataCasMeasGre50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CxG50-EC103=" & m_dataCasMeasGre50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CyG50-EC105=" & m_dataCasMeasGre50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "LambdaDomG50-EC107=" & m_dataCasMeasGre50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutVoG50-EC109=" & m_dataHeliosGre50.U2.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrG50-EC111=" & m_dataHeliosGre50.I2.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempG50-EC113=" & m_dataHeliosGre50.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "RefDataG50-EC115=" & m_dataCasMeasGre50.FilenameSpectra & vbNewLine

        tempString += "PhiB14-EC117=" & m_dataCasMeasBlu14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CxB14-EC119=" & m_dataCasMeasBlu14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CyB14-EC121=" & m_dataCasMeasBlu14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "LambdaDomB14-EC123=" & m_dataCasMeasBlu14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutVoB14-EC125=" & m_dataHeliosBlu14.U3.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrB14-EC127=" & m_dataHeliosBlu14.I3.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempB14-EC129=" & m_dataHeliosBlu14.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "RefDataB14-EC131=" & m_dataCasMeasBlu14.FilenameSpectra & vbNewLine

        tempString += "PhiB50-EC133=" & m_dataCasMeasBlu50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CxB50-EC135=" & m_dataCasMeasBlu50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "CyB50-EC137=" & m_dataCasMeasBlu50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "LambdaDomB50-EC139=" & m_dataCasMeasBlu50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutVoB50-EC141=" & m_dataHeliosBlu50.U3.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrB50-EC143=" & m_dataHeliosBlu50.I3.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempB50-EC145=" & m_dataHeliosBlu50.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "RefDataB50-EC147=" & m_dataCasMeasBlu50.FilenameSpectra & vbNewLine

        tempString += "OutCurrR-EC149=" & m_dataHelios100CCT55.I1.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrG-EC151=" & m_dataHelios100CCT55.I2.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrB-EC153=" & m_dataHelios100CCT55.I3.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalPhi100-EC155=" & m_dataCasMeas100CCT55.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCRI100-EC157=" & m_dataCasMeas100CCT55.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalR9100-EC159=" & m_dataCasMeas100CCT55.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCx100-EC161=" & m_dataCasMeas100CCT55.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCy100-EC163=" & m_dataCasMeas100CCT55.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalMacAdam100-EC165=" & m_dataCasMeas100CCT55.MacAdam.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalData100-EC167=" & m_dataCasMeas100CCT55.FilenameSpectra & vbNewLine
        tempString += "Iin100-EC169=" & m_dataPS100.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine

        tempString += "FinalCx100s-EC171=" & m_dataCasMeas100SuperR9.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCy100s-EC173=" & m_dataCasMeas100SuperR9.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalR9100s-EC175=" & m_dataCasMeas100SuperR9.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalData100s-EC177=" & m_dataCasMeas100SuperR9.FilenameSpectra & vbNewLine

        tempString += "FinalPhi5-EC189=" & m_dataCasMeas5CCT55.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCx5-EC191=" & m_dataCasMeas5CCT55.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCy5-EC193=" & m_dataCasMeas5CCT55.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalMacAdam5-EC195=" & m_dataCasMeas5CCT55.MacAdam.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalData5-EC197=" & m_dataCasMeas5CCT55.FilenameSpectra & vbNewLine

        tempString += "FinalPhi100-low-EC199=" & m_dataCasMeas100CCT55low.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCx100-low-EC201=" & m_dataCasMeas100CCT55low.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalCy100-low-EC203=" & m_dataCasMeas100CCT55low.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalMacAdam100-low-EC205=" & m_dataCasMeas100CCT55low.MacAdam.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalData100-low-EC207=" & m_dataCasMeas100CCT55low.FilenameSpectra & vbNewLine

        Dim filename As String = "C:\CamstarDataExchange\EOLTest\DataCollection\Input\" & Me.smData.Barcode
        'Dim filename As String = "C:\CamstarDataExchange\Test\" & Me.smData.Barcode

        FileSystem.WriteAllText(filename, tempString, False)

        'TODO: Nach X ms Prüfen, ob Datei von Camstar "abgeholt" wurde?
        'System.Threading.Thread.Sleep(500)
        'If File.Exists(filename) Then
        '    Return False
        'Else
        '    Return True
        'End If

        Return True
    End Function

    Private Function SaveDataToCamstarV(result As Boolean) As Boolean
        'Bezeichner und Werte als Strings je maximal 30 Zeichen!
        Dim tempString As String = ""

        tempString += "PASS=" & result.ToString() & vbNewLine
        tempString += "DateTime=" & Now.Date.ToShortDateString() & " " & Now.ToLongTimeString() & vbNewLine
        tempString += "User=" & Environment.UserDomainName & "\" & Environment.UserName & vbNewLine
        tempString += "Config2D1=" & smData.Config2D1 & vbNewLine
        tempString += "ConfigLastModifiedUTC=" & smData.ConfigLastModifiedUtc.Date.ToShortDateString() & " " & smData.ConfigLastModifiedUtc.ToLongTimeString() & vbNewLine
        'tempString += "FailedEC=0" & vbNewLine  'TODO
        tempString += "BarcodeRGBModul=" & smData.Barcode & vbNewLine

        tempString += "UinSby-EC1=" & m_dataPSSbyV.Voltage.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "IinSby-EC3=" & m_dataPSSbyV.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine

        tempString += "TempPCB1-EC23=" & m_dataHelios.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempSensor1-EC25=" & m_dataHelios.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine

        tempString += "SNVtoEEPROM-EC29=" & m_dataHelios.SNV_saved.ToString("0") & vbNewLine
        tempString += "LifetimeV-EC33=" & m_dataHelios.BurnTime.ToString("0") & vbNewLine
        tempString += "SWmain-EC35=" & m_dataHelios.Version_main.ToString("0") & vbNewLine
        tempString += "SWsub-EC37=" & m_dataHelios.Version_sub.ToString("0") & vbNewLine
        tempString += "SWbuild-EC38=" & m_dataHelios.Version_build.ToString("0") & vbNewLine

        tempString += "PoptV100-EC209=" & m_dataCasMeasUV.RadIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "LambdaPeakV100-EC211=" & m_dataCasMeasUV.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutVoV100-EC213=" & m_dataHeliosUV.U4.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "OutCurrV100-EC215=" & m_dataHeliosUV.I4.ToString("F3", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "TempV100-EC217=" & m_dataHeliosUV.T4.ToString("F1", Globalization.CultureInfo.InvariantCulture) & vbNewLine
        tempString += "FinalDataV100-EC219=" & m_dataCasMeasUV.FilenameSpectra & vbNewLine
        tempString += "IinV100-EC221=" & m_dataPSV.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture) & vbNewLine

        Dim filename As String = "C:\CamstarDataExchange\EOLTest\DataCollection\Input\" & Me.smData.BarcodeV
        'Dim filename As String = "C:\CamstarDataExchange\Test\" & Me.smData.BarcodeV

        FileSystem.WriteAllText(filename, tempString, False)

        'TODO: Nach X ms Prüfen, ob Datei von Camstar "abgeholt" wurde?
        'System.Threading.Thread.Sleep(500)
        'If File.Exists(filename) Then
        '    Return False
        'Else
        '    Return True
        'End If

        Return True
    End Function


#Region "state machine functions in alphabetic order"
    Public Sub stepCalcAdditionalValuesRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim color As String = String.Empty
        Dim stepNumber As Integer = 0
        Dim meas As MeasureValues = New MeasureValues
        Try
            color = DirectCast(data0, String)
            stepNumber = Convert.ToInt32(DirectCast(data1, String))
            Dim index As Integer
            index = m_measurementList.FindIndex(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber)))
            meas = m_measurementList.Item(index).MValue

            Select Case color
                Case "red"
                    meas.Tj = CalTempChipRthRed * meas.Voltage * meas.Current * (1 - CalTempChipEtaRed) + meas.Temperature
                    meas.CxTempComp = meas.Cx - (CalCxTLowRedX0 + CalCxTLowRedX1 * meas.Tj)
                    meas.CyTempComp = meas.Cy - (CalCyTLowRedX0 + CalCyTLowRedX1 * meas.Tj)
                    meas.PhiTempComp = meas.Phi / (CalPhiTRedX0 + CalPhiTRedX1 * meas.Tj + CalPhiTRedX2 * meas.Tj ^ 2 + CalPhiTRedX3 * meas.Tj ^ 3)
                Case "green"
                    meas.Tj = CalTempChipRthGre * meas.Voltage * meas.Current * (1 - CalTempChipEtaGre) + meas.Temperature
                    meas.CxTempComp = meas.Cx - (CalCxTLowGreX0 + CalCxTLowGreX1 * meas.Tj)
                    If meas.Tj < CalCyTBorderGre Then
                        meas.CyTempComp = meas.Cy - (CalCyTLowGreX0 + CalCyTLowGreX1 * meas.Tj)
                    Else
                        meas.CyTempComp = meas.Cy - (CalCyTHighGreX0 + CalCyTHighGreX1 * meas.Tj + CalCyTHighGreX2 * meas.Tj ^ 2 + CalCyTHighGreX3 * meas.Tj ^ 3)
                    End If
                    meas.PhiTempComp = meas.Phi / (CalPhiTGreX0 + CalPhiTGreX1 * meas.Tj + CalPhiTGreX2 * meas.Tj ^ 2 + CalPhiTGreX3 * meas.Tj ^ 3)
                Case "blue"
                    meas.Tj = CalTempChipRthBlu * meas.Voltage * meas.Current * (1 - CalTempChipEtaBlu) + meas.Temperature
                    meas.CxTempComp = meas.Cx - (CalCxTLowBluX0 + CalCxTLowBluX1 * meas.Tj)
                    meas.CyTempComp = meas.Cy - (CalCyTLowBluX0 + CalCyTLowBluX1 * meas.Tj)
                    meas.PhiTempComp = meas.Phi / (CalPhiTBluX0 + CalPhiTBluX1 * meas.Tj + CalPhiTBluX2 * meas.Tj ^ 2 + CalPhiTBluX3 * meas.Tj ^ 3)
                Case Else
            End Select
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalcAdditionalValuesRef: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Berechnung zusätzliche Messwerte " & color & " " & stepNumber.ToString(),
                           "",
                           " fehlgeschlagen")
            If fctGood = True Then
                RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" &
                                      MaxSteps.ToString &
                                      " Calc Additional Values " &
                                      color & " " &
                                      stepNumber.ToString())
                RaiseEvent addLogFile(Me, "          Tj:          " & meas.Tj.ToString("F1"))
                RaiseEvent addLogFile(Me, "          Cx/Cy:       " & meas.CxTempComp.ToString("F3") & "/" & meas.CyTempComp.ToString("F3"))
                RaiseEvent addLogFile(Me, "          Phi:         " & meas.PhiTempComp.ToString("F2"))
            End If
        End Try
    End Sub

    Public Sub stepCalcIntegrationTimes(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim refTime As Integer
            refTime = DirectCast(GetVariableInstance(DirectCast(data1, String)), Integer)
            Select Case color
                Case "red"
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 1)).MValue.IntegrationTime = refTime * IntTimeRed1
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 2)).MValue.IntegrationTime = refTime * IntTimeRed2
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 3)).MValue.IntegrationTime = refTime * IntTimeRed3
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 4)).MValue.IntegrationTime = refTime * IntTimeRed4
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 5)).MValue.IntegrationTime = refTime * IntTimeRed5
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 6)).MValue.IntegrationTime = refTime * IntTimeRed6
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 7)).MValue.IntegrationTime = refTime * IntTimeRed7
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 8)).MValue.IntegrationTime = refTime * IntTimeRed8
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 9)).MValue.IntegrationTime = refTime * IntTimeRed9
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 10)).MValue.IntegrationTime = refTime * IntTimeRed10
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 11)).MValue.IntegrationTime = refTime * IntTimeRed11
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 12)).MValue.IntegrationTime = refTime * IntTimeRed12
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 13)).MValue.IntegrationTime = refTime * IntTimeRed13
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 14)).MValue.IntegrationTime = refTime * IntTimeRed14
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 15)).MValue.IntegrationTime = refTime * IntTimeRed15
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 16)).MValue.IntegrationTime = refTime * IntTimeRed16
                    fctGood = True
                Case "green"
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 1)).MValue.IntegrationTime = refTime * IntTimeGre1
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 2)).MValue.IntegrationTime = refTime * IntTimeGre2
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 3)).MValue.IntegrationTime = refTime * IntTimeGre3
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 4)).MValue.IntegrationTime = refTime * IntTimeGre4
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 5)).MValue.IntegrationTime = refTime * IntTimeGre5
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 6)).MValue.IntegrationTime = refTime * IntTimeGre6
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 7)).MValue.IntegrationTime = refTime * IntTimeGre7
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 8)).MValue.IntegrationTime = refTime * IntTimeGre8
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 9)).MValue.IntegrationTime = refTime * IntTimeGre9
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 10)).MValue.IntegrationTime = refTime * IntTimeGre10
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 11)).MValue.IntegrationTime = refTime * IntTimeGre11
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 12)).MValue.IntegrationTime = refTime * IntTimeGre12
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 13)).MValue.IntegrationTime = refTime * IntTimeGre13
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 14)).MValue.IntegrationTime = refTime * IntTimeGre14
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 15)).MValue.IntegrationTime = refTime * IntTimeGre15
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 16)).MValue.IntegrationTime = refTime * IntTimeGre16
                    fctGood = True
                Case "blue"
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 1)).MValue.IntegrationTime = refTime * IntTimeBlu1
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 2)).MValue.IntegrationTime = refTime * IntTimeBlu2
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 3)).MValue.IntegrationTime = refTime * IntTimeBlu3
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 4)).MValue.IntegrationTime = refTime * IntTimeBlu4
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 5)).MValue.IntegrationTime = refTime * IntTimeBlu5
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 6)).MValue.IntegrationTime = refTime * IntTimeBlu6
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 7)).MValue.IntegrationTime = refTime * IntTimeBlu7
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 8)).MValue.IntegrationTime = refTime * IntTimeBlu8
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 9)).MValue.IntegrationTime = refTime * IntTimeBlu9
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 10)).MValue.IntegrationTime = refTime * IntTimeBlu10
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 11)).MValue.IntegrationTime = refTime * IntTimeBlu11
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 12)).MValue.IntegrationTime = refTime * IntTimeBlu12
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 13)).MValue.IntegrationTime = refTime * IntTimeBlu13
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 14)).MValue.IntegrationTime = refTime * IntTimeBlu14
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 15)).MValue.IntegrationTime = refTime * IntTimeBlu15
                    m_measurementList.Find(Function(x) (x.Color = color) And (x.StepNumber = 16)).MValue.IntegrationTime = refTime * IntTimeBlu16
                    fctGood = True
                Case Else
            End Select
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalcIntegrationTimes: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Berechnung Integrationszeiten",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    'todo
    Public Sub stepCalcLimitsRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        RaiseEvent newStateMachineStatus(Me,
                                             New StateMachineStatus(CurrentStep.ToString & "/" & MaxSteps.ToString & " stepCalcLimitsRef Schritt ist ein leerer Schritt ohne Funktion",
                                                                    Drawing.Color.Yellow,
                                                                    CurrentStep / MaxSteps))
    End Sub

    Public Sub stepCalibCx(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            'StepNumber 0 is the unpowered step
            'this step is not needed for the calculations
            Dim xx(STEP_COUNT - 2) As Double
            Dim yy(STEP_COUNT - 2) As Double
            Dim cubicData(4) As Double
            For i = 0 To xx.Length - 1
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.CxTempComp
            Next i
            'calculation of the fit of CxTempComp = f(I)
            cubicData = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
            Me.m_calibList.Item(index).CValue.Cx.I_low.F_x0 = cubicData(0)
            Me.m_calibList.Item(index).CValue.Cx.I_high.F_x0 = cubicData(0)
            Me.m_calibList.Item(index).CValue.Cx.I_low.F_x1 = cubicData(1)
            Me.m_calibList.Item(index).CValue.Cx.I_high.F_x1 = cubicData(1)
            Me.m_calibList.Item(index).CValue.Cx.I_low.F_x2 = cubicData(2)
            Me.m_calibList.Item(index).CValue.Cx.I_high.F_x2 = cubicData(2)
            Me.m_calibList.Item(index).CValue.Cx.I_low.F_x3 = cubicData(3)
            Me.m_calibList.Item(index).CValue.Cx.I_high.F_x3 = cubicData(3)
            Me.m_calibList.Item(index).CValue.Cx.I_border = CalCxCyIBorder

            Select Case color
                Case "red"
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x0 = CalCxTLowRedX0
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x1 = CalCxTLowRedX1
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x2 = CalCxTLowRedX2
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x3 = CalCxTLowRedX3
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x0 = CalCxTHighRedX0
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x1 = CalCxTHighRedX1
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x2 = CalCxTHighRedX2
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x3 = CalCxTHighRedX3
                    Me.m_calibList.Item(index).CValue.Cx.T_border = CalCxTBorderRed
                    fctGood = True
                Case "green"
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x0 = CalCxTLowGreX0
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x1 = CalCxTLowGreX1
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x2 = CalCxTLowGreX2
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x3 = CalCxTLowGreX3
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x0 = CalCxTHighGreX0
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x1 = CalCxTHighGreX1
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x2 = CalCxTHighGreX2
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x3 = CalCxTHighGreX3
                    Me.m_calibList.Item(index).CValue.Cx.T_border = CalCxTBorderGre
                    fctGood = True
                Case "blue"
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x0 = CalCxTLowBluX0
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x1 = CalCxTLowBluX1
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x2 = CalCxTLowBluX2
                    Me.m_calibList.Item(index).CValue.Cx.T_low.F_x3 = CalCxTLowBluX3
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x0 = CalCxTHighBluX0
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x1 = CalCxTHighBluX1
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x2 = CalCxTHighBluX2
                    Me.m_calibList.Item(index).CValue.Cx.T_high.F_x3 = CalCxTHighBluX3
                    Me.m_calibList.Item(index).CValue.Cx.T_border = CalCxTBorderBlu
                    fctGood = True
                Case Else
            End Select
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function StepCalibCx: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung Cx",
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepCalibCy(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            'StepNumber 0 is the unpowered step
            'this step is not needed for the calculations
            Dim xx(STEP_COUNT - 2) As Double
            Dim yy(STEP_COUNT - 2) As Double
            Dim cubicData(4) As Double
            For i = 0 To xx.Length - 1
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.CyTempComp
            Next i
            'calculation of the fit of CyTempComp = f(I)
            cubicData = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
            Me.m_calibList.Item(index).CValue.Cy.I_low.F_x0 = cubicData(0)
            Me.m_calibList.Item(index).CValue.Cy.I_high.F_x0 = cubicData(0)
            Me.m_calibList.Item(index).CValue.Cy.I_low.F_x1 = cubicData(1)
            Me.m_calibList.Item(index).CValue.Cy.I_high.F_x1 = cubicData(1)
            Me.m_calibList.Item(index).CValue.Cy.I_low.F_x2 = cubicData(2)
            Me.m_calibList.Item(index).CValue.Cy.I_high.F_x2 = cubicData(2)
            Me.m_calibList.Item(index).CValue.Cy.I_low.F_x3 = cubicData(3)
            Me.m_calibList.Item(index).CValue.Cy.I_high.F_x3 = cubicData(3)
            Me.m_calibList.Item(index).CValue.Cy.I_border = CalCxCyIBorder

            Select Case color
                Case "red"
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x0 = CalCyTLowRedX0
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x1 = CalCyTLowRedX1
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x2 = CalCyTLowRedX2
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x3 = CalCyTLowRedX3
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x0 = CalCyTHighRedX0
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x1 = CalCyTHighRedX1
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x2 = CalCyTHighRedX2
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x3 = CalCyTHighRedX3
                    Me.m_calibList.Item(index).CValue.Cy.T_border = CalCyTBorderRed
                    fctGood = True
                Case "green"
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x0 = CalCyTLowGreX0
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x1 = CalCyTLowGreX1
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x2 = CalCyTLowGreX2
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x3 = CalCyTLowGreX3
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x0 = CalCyTHighGreX0
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x1 = CalCyTHighGreX1
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x2 = CalCyTHighGreX2
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x3 = CalCyTHighGreX3
                    Me.m_calibList.Item(index).CValue.Cy.T_border = CalCyTBorderGre
                    fctGood = True
                Case "blue"
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x0 = CalCyTLowBluX0
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x1 = CalCyTLowBluX1
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x2 = CalCyTLowBluX2
                    Me.m_calibList.Item(index).CValue.Cy.T_low.F_x3 = CalCyTLowBluX3
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x0 = CalCyTHighBluX0
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x1 = CalCyTHighBluX1
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x2 = CalCyTHighBluX2
                    Me.m_calibList.Item(index).CValue.Cy.T_high.F_x3 = CalCyTHighBluX3
                    Me.m_calibList.Item(index).CValue.Cy.T_border = CalCyTBorderBlu
                    fctGood = True
                Case Else
            End Select
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function StepCalibCy: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung Cy",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCalibDacMax(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Dim xx(STEP_COUNT - 1) As Double
            Dim yy(STEP_COUNT - 1) As Double
            Dim cubicData(4) As Double
            For i = 0 To xx.Length - 1
                Dim stepNumber As Integer
                stepNumber = i + 1
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntensityDAC
            Next i
            cubicData = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
            Me.m_calibList.Item(index).CValue.DacMax = Convert.ToInt32(cubicData(3) * (MaxRgbCur ^ 3) + cubicData(2) * (MaxRgbCur ^ 2) + cubicData(1) * (MaxRgbCur) + cubicData(0))
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function StepCalibDacMax: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung DacMax",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCalibIByDAC(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Dim xx(STEP_COUNT - 2) As Double
            Dim yy(STEP_COUNT - 2) As Double
            Dim cubicData(4) As Double
            For i = 0 To xx.Length - 1
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntensityDAC
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
            Next i
            cubicData = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
            Me.m_calibList.Item(index).CValue.IByDac.Fct.F_x0 = cubicData(0)
            Me.m_calibList.Item(index).CValue.IByDac.Fct.F_x1 = cubicData(1)
            Me.m_calibList.Item(index).CValue.IByDac.Fct.F_x2 = cubicData(2)
            Me.m_calibList.Item(index).CValue.IByDac.Fct.F_x3 = cubicData(3)
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibIByDAC: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung IByDac",
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepCalibIDac(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Dim xx(STEP_COUNT - 2) As Double
            Dim yy(STEP_COUNT - 2) As Double
            Dim cubicData(4) As Double
            For i = 0 To xx.Length - 1
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntensityDAC
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
            Next i
            cubicData = MathNet.Numerics.Fit.Polynomial(xx, yy, 1)
            Me.m_calibList.Item(index).CValue.Dac.Slope = cubicData(1)
            Me.m_calibList.Item(index).CValue.Dac.Offset = cubicData(0)
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibIDAC: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung CurrentDac",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    'todo
    Public Sub stepCalibPhiADC(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))

            Dim xx() As Double
            Dim yy() As Double

            Dim fitCxCas(STEP_COUNT - 2) As Double
            Dim fitCyCas(STEP_COUNT - 2) As Double
            Dim fitTsensor(STEP_COUNT - 2) As Double
            Dim fitPD(STEP_COUNT - 2) As Double
            Dim fitCasPhi(STEP_COUNT - 2) As Double
            Dim fitTj(STEP_COUNT - 2) As Double
            Dim fitTLed(STEP_COUNT - 2) As Double
            Dim fitCurrent(STEP_COUNT - 2) As Double

            Dim lambdaDom0 As Double
            Dim etaPD_c0 As Double

            Dim lambdaDom(STEP_COUNT - 2) As Double
            Dim etaVLambda_b(STEP_COUNT - 2) As Double
            Dim etaPD_c(STEP_COUNT - 2) As Double
            Dim etaPD_d(STEP_COUNT - 2) As Double
            Dim etaPD_f(STEP_COUNT - 2) As Double
            Dim data_e(STEP_COUNT - 2) As Double
            Dim tanAlpha As Double
            'Calc LambdaDom0 based on Cx0, Cy0 (1)

            For i = 0 To STEP_COUNT - 2
                Dim stepNumber As Integer
                stepNumber = i + 2
                fitCxCas(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Cx
                fitCyCas(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Cy
                fitTsensor(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.TempExtern
                fitPD(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.PD
                fitCasPhi(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Phi
                fitTj(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Tj
                fitTLed(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Temperature
                fitCurrent(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
            Next i

            Select Case color
                Case "red"
                    'Calc LambdaDom0 based on Cx0, Cy0 (1)
                    tanAlpha = (fitCyCas(0) - 1.0 / 3.0) / (fitCxCas(0) - 1.0 / 3.0)
                    lambdaDom0 = RED_LAMBDADOM_A3 * tanAlpha ^ 3 + RED_LAMBDADOM_A2 * tanAlpha ^ 2 + RED_LAMBDADOM_A1 * tanAlpha + RED_LAMBDADOM_A0

                    'Calc etaPD(lambdaDom0) factor(c0) // (3)
                    etaPD_c0 = RED_PD_LAMBDADOM_A3 * lambdaDom0 ^ 3 + RED_PD_LAMBDADOM_A2 * lambdaDom0 ^ 2 + RED_PD_LAMBDADOM_A1 * lambdaDom0 + RED_PD_LAMBDADOM_A0

                    '###### for each point
                    For i = 0 To STEP_COUNT - 2
                        'Calc LambdaDom based on Cx,Cy for each point (1)
                        tanAlpha = (fitCyCas(i) - 1.0 / 3.0) / (fitCxCas(i) - 1.0 / 3.0)
                        lambdaDom(i) = RED_LAMBDADOM_A3 * tanAlpha ^ 3 + RED_LAMBDADOM_A2 * tanAlpha ^ 2 + RED_LAMBDADOM_A1 * tanAlpha + RED_LAMBDADOM_A0
                        'Calc etaVlambda(lambdaDom)  factor(b) //  (2)
                        etaVLambda_b(i) = RED_VLAMBDA_A3 * lambdaDom(i) ^ 3 + RED_VLAMBDA_A2 * lambdaDom(i) ^ 2 + RED_VLAMBDA_A1 * lambdaDom(i) + RED_VLAMBDA_A0
                        'Calc etaPD(lambdaDom) factor(c) //  (3)
                        etaPD_c(i) = RED_PD_LAMBDADOM_A3 * lambdaDom(i) ^ 3 + RED_PD_LAMBDADOM_A2 * lambdaDom(i) ^ 2 + RED_PD_LAMBDADOM_A1 * lambdaDom(i) + RED_PD_LAMBDADOM_A0
                        'Calc etaPD(TsensorPD) factor(d) // (4)
                        etaPD_d(i) = PD_TEMPSENSOR_A3 * fitTsensor(i) ^ 3 + PD_TEMPSENSOR_A2 * fitTsensor(i) ^ 2 + PD_TEMPSENSOR_A1 * fitTsensor(i) + PD_TEMPSENSOR_A0
                        'Calc fit 
                        'Phi = delta(PD) * b * c / d / c0 / e
                        '=> e = delta(PD) * b * c / d / c0 / phi
                        data_e(i) = (fitPD(i) - m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.PD) * etaVLambda_b(i) * etaPD_c(i) / etaPD_d(i) / etaPD_c0 / fitCasPhi(i)
                    Next i
                    'fit e = f(Tj)
                    Dim cubicData_factor1(3) As Double
                    ReDim xx(STEP_COUNT - 2)
                    ReDim yy(STEP_COUNT - 2)
                    For i = 0 To STEP_COUNT - 2
                        xx(i) = fitTj(i)
                        yy(i) = data_e(i)
                    Next
                    cubicData_factor1 = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x0 = cubicData_factor1(0)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x1 = cubicData_factor1(1)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x2 = cubicData_factor1(2)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x3 = cubicData_factor1(3)

                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x0 = 0
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x1 = 0
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x2 = 0
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x3 = 0

                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x0 = RED_PD_LAMBDADOM_A0
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x1 = RED_PD_LAMBDADOM_A1
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x2 = RED_PD_LAMBDADOM_A2
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x3 = RED_PD_LAMBDADOM_A3

                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x0 = PD_TEMPSENSOR_A0
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x1 = PD_TEMPSENSOR_A1
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x2 = PD_TEMPSENSOR_A2
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x3 = PD_TEMPSENSOR_A3

                    m_calibList.Item(index).CValue.PhiADC.Offset_PD = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.PD
                    m_calibList.Item(index).CValue.PhiADC.Dummy0 = 0
                    m_calibList.Item(index).CValue.Additional.Cx5500K = 0.332
                    m_calibList.Item(index).CValue.Additional.Cy5500K = 0.352
                Case "green"
                    'Calc LambdaDom0 based on Cx0, Cy0 (1)
                    tanAlpha = (fitCxCas(0) - 1.0 / 3.0) / (fitCyCas(0) - 1.0 / 3.0)
                    lambdaDom0 = GRE_LAMBDADOM_A3 * tanAlpha ^ 3 + GRE_LAMBDADOM_A2 * tanAlpha ^ 2 + GRE_LAMBDADOM_A1 * tanAlpha + GRE_LAMBDADOM_A0

                    'Calc etaPD(lambdaDom0) factor(c0) // (3)
                    etaPD_c0 = GRE_PD_LAMBDADOM_A3 * lambdaDom0 ^ 3 + GRE_PD_LAMBDADOM_A2 * lambdaDom0 ^ 2 + GRE_PD_LAMBDADOM_A1 * lambdaDom0 + GRE_PD_LAMBDADOM_A0

                    '###### for each point
                    For i = 0 To STEP_COUNT - 2
                        'Calc LambdaDom based on Cx,Cy for each point (1)
                        tanAlpha = (fitCxCas(i) - 1.0 / 3.0) / (fitCyCas(i) - 1.0 / 3.0)
                        lambdaDom(i) = GRE_LAMBDADOM_A3 * tanAlpha ^ 3 + GRE_LAMBDADOM_A2 * tanAlpha ^ 2 + GRE_LAMBDADOM_A1 * tanAlpha + GRE_LAMBDADOM_A0
                        'Calc etaVlambda(lambdaDom)  factor(b) //  (2)
                        etaVLambda_b(i) = GRE_VLAMBDA_A3 * lambdaDom(i) ^ 3 + GRE_VLAMBDA_A2 * lambdaDom(i) ^ 2 + GRE_VLAMBDA_A1 * lambdaDom(i) + GRE_VLAMBDA_A0
                        'Calc etaPD(lambdaDom) factor(c) //  (3)
                        etaPD_c(i) = GRE_PD_LAMBDADOM_A3 * lambdaDom(i) ^ 3 + GRE_PD_LAMBDADOM_A2 * lambdaDom(i) ^ 2 + GRE_PD_LAMBDADOM_A1 * lambdaDom(i) + GRE_PD_LAMBDADOM_A0
                        'Calc etaPD(TsensorPD) factor(d) // (4)
                        etaPD_d(i) = PD_TEMPSENSOR_A3 * fitTsensor(i) ^ 3 + PD_TEMPSENSOR_A2 * fitTsensor(i) ^ 2 + PD_TEMPSENSOR_A1 * fitTsensor(i) + PD_TEMPSENSOR_A0

                        'Calc etaPD(delta T) factor (f) // 

                        etaPD_f(i) = -0.00244444 * (fitTj(i) - fitTLed(i)) + 1.05688
                        'Calc fit 
                        'Phi = delta(PD) * b * c * d / e / f
                        '=> e = delta(PD) * b * c * d / f / phi
                        data_e(i) = (fitPD(i) - m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.PD) * etaVLambda_b(i) * etaPD_c(i) * etaPD_d(i) / etaPD_f(i) / fitCasPhi(i)
                    Next i
                    'fit e = f(Tj)
                    Dim cubicData_factor1(3) As Double
                    Dim cubicData_factor2(3) As Double
                    ReDim xx(STEP_COUNT - 2)
                    ReDim yy(STEP_COUNT - 2)
                    For i = 0 To STEP_COUNT - 2
                        xx(i) = fitCurrent(i)
                        yy(i) = data_e(i)
                    Next
                    cubicData_factor1 = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
                    cubicData_factor2(3) = 0
                    cubicData_factor2(2) = 0
                    cubicData_factor2(1) = -0.00244444
                    cubicData_factor2(0) = 1.05688

                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x0 = cubicData_factor1(0)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x1 = cubicData_factor1(1)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x2 = cubicData_factor1(2)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x3 = cubicData_factor1(3)

                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x0 = cubicData_factor2(0)
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x1 = cubicData_factor2(1)
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x2 = cubicData_factor2(2)
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x3 = cubicData_factor2(3)

                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x0 = GRE_PD_LAMBDADOM_A0
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x1 = GRE_PD_LAMBDADOM_A1
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x2 = GRE_PD_LAMBDADOM_A2
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x3 = GRE_PD_LAMBDADOM_A3

                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x0 = PD_TEMPSENSOR_A0
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x1 = PD_TEMPSENSOR_A1
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x2 = PD_TEMPSENSOR_A2
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x3 = PD_TEMPSENSOR_A3

                    m_calibList.Item(index).CValue.PhiADC.Offset_PD = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.PD
                    m_calibList.Item(index).CValue.PhiADC.Dummy0 = 0
                    m_calibList.Item(index).CValue.Additional.Cx5500K = 0
                    m_calibList.Item(index).CValue.Additional.Cy5500K = 0
                Case "blue"
                    'Calc LambdaDom0 based on Cx0, Cy0 (1)
                    tanAlpha = (fitCyCas(0) - 1.0 / 3.0) / (fitCxCas(0) - 1.0 / 3.0)
                    lambdaDom0 = BLU_LAMBDADOM_A3 * tanAlpha ^ 3 + BLU_LAMBDADOM_A2 * tanAlpha ^ 2 + BLU_LAMBDADOM_A1 * tanAlpha + BLU_LAMBDADOM_A0

                    'Calc etaPD(lambdaDom0) factor(c0) // (3)
                    etaPD_c0 = BLU_PD_LAMBDADOM_A3 * lambdaDom0 ^ 3 + BLU_PD_LAMBDADOM_A2 * lambdaDom0 ^ 2 + BLU_PD_LAMBDADOM_A1 * lambdaDom0 + BLU_PD_LAMBDADOM_A0

                    '###### for each point
                    For i = 0 To STEP_COUNT - 2
                        'Calc LambdaDom based on Cx,Cy for each point (1)
                        tanAlpha = (fitCyCas(i) - 1.0 / 3.0) / (fitCxCas(i) - 1.0 / 3.0)
                        lambdaDom(i) = BLU_LAMBDADOM_A3 * tanAlpha ^ 3 + BLU_LAMBDADOM_A2 * tanAlpha ^ 2 + BLU_LAMBDADOM_A1 * tanAlpha + BLU_LAMBDADOM_A0
                        'Calc etaVlambda(lambdaDom)  factor(b) //  (2)
                        etaVLambda_b(i) = BLU_VLAMBDA_A3 * lambdaDom(i) ^ 3 + BLU_VLAMBDA_A2 * lambdaDom(i) ^ 2 + BLU_VLAMBDA_A1 * lambdaDom(i) + BLU_VLAMBDA_A0
                        'Calc etaPD(lambdaDom) factor(c) //  (3)
                        etaPD_c(i) = BLU_PD_LAMBDADOM_A3 * lambdaDom(i) ^ 3 + BLU_PD_LAMBDADOM_A2 * lambdaDom(i) ^ 2 + BLU_PD_LAMBDADOM_A1 * lambdaDom(i) + BLU_PD_LAMBDADOM_A0
                        'Calc etaPD(TsensorPD) factor(d) // (4)
                        etaPD_d(i) = PD_TEMPSENSOR_A3 * fitTsensor(i) ^ 3 + PD_TEMPSENSOR_A2 * fitTsensor(i) ^ 2 + PD_TEMPSENSOR_A1 * fitTsensor(i) + PD_TEMPSENSOR_A0

                        'Calc etaPD(delta T) factor (f) // 

                        etaPD_f(i) = 1.0
                        'Calc fit 
                        'Phi = delta(PD) * b * c * d / e / f
                        '=> e = delta(PD) * b * c * d / f / phi
                        data_e(i) = (fitPD(i) - m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.PD) * etaVLambda_b(i) * etaPD_c(i) * etaPD_d(i) / etaPD_f(i) / fitCasPhi(i)
                    Next i
                    'fit e = f(Tj)
                    Dim cubicData_factor1(3) As Double
                    Dim cubicData_factor2(3) As Double
                    ReDim xx(STEP_COUNT - 2)
                    ReDim yy(STEP_COUNT - 2)
                    For i = 0 To STEP_COUNT - 2
                        xx(i) = fitCurrent(i)
                        yy(i) = data_e(i)
                    Next
                    cubicData_factor1 = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
                    cubicData_factor2(3) = 0
                    cubicData_factor2(2) = 0
                    cubicData_factor2(1) = 0
                    cubicData_factor2(0) = 1

                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x0 = cubicData_factor1(0)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x1 = cubicData_factor1(1)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x2 = cubicData_factor1(2)
                    m_calibList.Item(index).CValue.PhiADC.Factor1.F_x3 = cubicData_factor1(3)

                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x0 = cubicData_factor2(0)
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x1 = cubicData_factor2(1)
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x2 = cubicData_factor2(2)
                    m_calibList.Item(index).CValue.PhiADC.Factor2.F_x3 = cubicData_factor2(3)

                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x0 = BLU_PD_LAMBDADOM_A0
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x1 = BLU_PD_LAMBDADOM_A1
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x2 = BLU_PD_LAMBDADOM_A2
                    m_calibList.Item(index).CValue.PhiADC.PDLambdaDom.F_x3 = BLU_PD_LAMBDADOM_A3

                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x0 = PD_TEMPSENSOR_A0
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x1 = PD_TEMPSENSOR_A1
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x2 = PD_TEMPSENSOR_A2
                    m_calibList.Item(index).CValue.PhiADC.PDTemp.F_x3 = PD_TEMPSENSOR_A3

                    m_calibList.Item(index).CValue.PhiADC.Offset_PD = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.PD
                    m_calibList.Item(index).CValue.PhiADC.Dummy0 = 0
                    m_calibList.Item(index).CValue.Additional.Cx5500K = 0
                    m_calibList.Item(index).CValue.Additional.Cy5500K = 0
            End Select

            m_calibList.Item(index).CValue.Additional.Dummy02 = 0
            m_calibList.Item(index).CValue.Additional.Dummy03 = 0
            m_calibList.Item(index).CValue.Additional.Dummy10 = 0
            m_calibList.Item(index).CValue.Additional.Dummy11 = 0
            m_calibList.Item(index).CValue.Additional.Dummy12 = 0
            m_calibList.Item(index).CValue.Additional.Dummy13 = 0
            m_calibList.Item(index).CValue.Additional.Dummy20 = 0
            m_calibList.Item(index).CValue.Additional.Dummy21 = 0
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibPhiADC: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung PhiADC",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCalibPhi(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Dim xx(4) As Double
            Dim yy(4) As Double
            xx(0) = 0
            yy(0) = 0
            For i = 0 To 3
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i + 1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i + 1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.PhiTempComp
            Next i
            Dim splineFit As New Spline.SplineInterpolation(xx, yy)
            Dim cof(3) As Single
            cof = calcSplineCoef(splineFit.CoefficientList(0))
            Me.m_calibList.Item(index).CValue.Phi.I_spl1.F_x0 = cof(0)
            Me.m_calibList.Item(index).CValue.Phi.I_spl1.F_x1 = cof(1)
            Me.m_calibList.Item(index).CValue.Phi.I_spl1.F_x2 = cof(2)
            Me.m_calibList.Item(index).CValue.Phi.I_spl1.F_x3 = cof(3)
            cof = calcSplineCoef(splineFit.CoefficientList(1))
            Me.m_calibList.Item(index).CValue.Phi.I_spl2.F_x0 = cof(0)
            Me.m_calibList.Item(index).CValue.Phi.I_spl2.F_x1 = cof(1)
            Me.m_calibList.Item(index).CValue.Phi.I_spl2.F_x2 = cof(2)
            Me.m_calibList.Item(index).CValue.Phi.I_spl2.F_x3 = cof(3)
            cof = calcSplineCoef(splineFit.CoefficientList(2))
            Me.m_calibList.Item(index).CValue.Phi.I_spl3.F_x0 = cof(0)
            Me.m_calibList.Item(index).CValue.Phi.I_spl3.F_x1 = cof(1)
            Me.m_calibList.Item(index).CValue.Phi.I_spl3.F_x2 = cof(2)
            Me.m_calibList.Item(index).CValue.Phi.I_spl3.F_x3 = cof(3)
            cof = calcSplineCoef(splineFit.CoefficientList(3))
            Me.m_calibList.Item(index).CValue.Phi.I_spl4.F_x0 = cof(0)
            Me.m_calibList.Item(index).CValue.Phi.I_spl4.F_x1 = cof(1)
            Me.m_calibList.Item(index).CValue.Phi.I_spl4.F_x2 = cof(2)
            Me.m_calibList.Item(index).CValue.Phi.I_spl4.F_x3 = cof(3)

            Me.m_calibList.Item(index).CValue.Phi.Border_I_1 = xx(1)
            Me.m_calibList.Item(index).CValue.Phi.Border_I_2 = xx(2)
            Me.m_calibList.Item(index).CValue.Phi.Border_I_3 = xx(3)
            Me.m_calibList.Item(index).CValue.Phi.Border_I_4 = xx(4)

            ReDim xx(STEP_COUNT - 5)
            ReDim yy(STEP_COUNT - 5)
            For i = 5 To STEP_COUNT
                Dim stepNumber As Integer
                stepNumber = i
                xx(i - 5) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i - 5) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.PhiTempComp
            Next i
            Dim cubicData(4) As Double
            cubicData = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
            Me.m_calibList.Item(index).CValue.Phi.I_last.F_x0 = cubicData(0)
            Me.m_calibList.Item(index).CValue.Phi.I_last.F_x1 = cubicData(1)
            Me.m_calibList.Item(index).CValue.Phi.I_last.F_x2 = cubicData(2)
            Me.m_calibList.Item(index).CValue.Phi.I_last.F_x3 = cubicData(3)
            Select Case color
                Case "red"
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x0 = CalPhiTRedX0
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x1 = CalPhiTRedX1
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x2 = CalPhiTRedX2
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x3 = CalPhiTRedX3
                    fctGood = True
                Case "green"
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x0 = CalPhiTGreX0
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x1 = CalPhiTGreX1
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x2 = CalPhiTGreX2
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x3 = CalPhiTGreX3
                    fctGood = True
                Case "blue"
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x0 = CalPhiTBluX0
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x1 = CalPhiTBluX1
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x2 = CalPhiTBluX2
                    Me.m_calibList.Item(index).CValue.Phi.T.F_x3 = CalPhiTBluX3
                    fctGood = True
                Case Else
            End Select
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibPhi: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung Phi",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCalibPhiMax(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Select Case color
                Case "red"
                    Me.m_calibList.Item(index).CValue.PhiMax = CalPhiMaxRed
                Case "green"
                    Me.m_calibList.Item(index).CValue.PhiMax = CalPhiMaxGre
                Case "blue"
                    Me.m_calibList.Item(index).CValue.PhiMax = CalPhiMaxBlu
                Case Else
            End Select
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibPhiMax: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung PhiMax",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCalibTempChip(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Select Case color
                Case "red"
                    Me.m_calibList.Item(index).CValue.TempChip.Rth = CalTempChipRthRed
                    Me.m_calibList.Item(index).CValue.TempChip.Eta = CalTempChipEtaRed
                    fctGood = True
                Case "green"
                    Me.m_calibList.Item(index).CValue.TempChip.Rth = CalTempChipRthGre
                    Me.m_calibList.Item(index).CValue.TempChip.Eta = CalTempChipEtaGre
                    fctGood = True
                Case "blue"
                    Me.m_calibList.Item(index).CValue.TempChip.Rth = CalTempChipRthBlu
                    Me.m_calibList.Item(index).CValue.TempChip.Eta = CalTempChipEtaBlu
                    fctGood = True
                Case Else
            End Select
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibTempChip: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung Temp Junction",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCalibSuperR9(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim result As Boolean = True

            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data0, CasCommunication.cCAS140)
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data1, HELIOSCommunication.HELIOSCommunication)

            Dim oldR9 As Double = 0
            Dim bestCy As Double = 0.346
            Dim cy As Double = 0.346
            cas.AutoRangeMeasurement = False
            cas.FilterPosition = 2
            cas.Averages = 2
            cas.IntegrationTime = m_measurementList.Find(Function(x) (x.Color = "red") And (x.StepNumber = 16)).MValue.IntegrationTime * 0.7
            If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")

            If Not can.SetCxCyColor(CAN_DEST, 0.332, cy) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 100) Then result = False

            If result Then
                Threading.Thread.Sleep(200)
                Do
                    cy = cy + 0.001
                    If Not can.SetCxCyColor(CAN_DEST, 0.332, cy) Then result = False
                    Threading.Thread.Sleep(100)
                    If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
                    If cas.GetR9 > oldR9 Then
                        oldR9 = cas.GetR9
                        bestCy = cy
                    End If
                Loop While (cy < 0.42) And (result = True)
            End If

            If result Then
                For index = 0 To 2
                    Me.m_calibList.Item(index).CValue.SuperR9.Cx = 0.332 * 1000
                    Me.m_calibList.Item(index).CValue.SuperR9.Cy = bestCy * 1000
                Next
            End If

            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibSuperR9: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung SuperR9",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCalibUbyI(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Dim xx(STEP_COUNT - 2) As Double
            Dim yy(STEP_COUNT - 2) As Double
            Dim cubicData(4) As Double
            For i = 0 To xx.Length - 1
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Voltage
            Next i
            cubicData = MathNet.Numerics.Fit.Polynomial(xx, yy, 3)
            Me.m_calibList.Item(index).CValue.UByI.Fct.F_x0 = cubicData(0)
            Me.m_calibList.Item(index).CValue.UByI.Fct.F_x1 = cubicData(1)
            Me.m_calibList.Item(index).CValue.UByI.Fct.F_x2 = cubicData(2)
            Me.m_calibList.Item(index).CValue.UByI.Fct.F_x3 = cubicData(3)
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCalibUbyI: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Kalibrierwertberechnung UbyI",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCheckBoxClosed(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim myProtocol As HardwareCommunication.ProtocolEOLBox = New HardwareCommunication.ProtocolEOLBox
            Dim result As String = String.Empty
            Dim buffer(20) As Char
            Dim iniReader As IniReader = New IniReader
            Dim comPort As String
            comPort = iniReader.ReadValueFromFile("COM", "port", "", ".\Settings.ini")
            myProtocol.Open(comPort)
            If myProtocol.GetCapPos(result) Then
                If result = "closed" Then
                    fctGood = True
                End If
            End If
            myProtocol.Close()
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckBoxClosed: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Box",
                           " geschlossen",
                           " noch offen")
        End Try
    End Sub



    Public Sub stepCheckCalibLimitsRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim color As String
            color = DirectCast(data0, String)
            Dim index As Integer
            index = m_calibList.FindIndex(Function(x) (x.Color = color))
            Dim cValue As CalibValues
            cValue = m_calibList.Item(index).CValue
            Dim limit As CalibLimits
            limit = m_calibList.Item(index).CLimit
            Dim resultGood As Boolean
            resultGood = True
            Dim minRatio As Double
            Dim maxRatio As Double
            Dim midValue As Double
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'CURRENT
            If Not limit.Current.Slope.CheckLimit(cValue.Current.Slope) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:curr:slo V: " & cValue.Current.Slope.ToString & ", Min: " & limit.Current.Slope.Min.ToString & ", Max: " & limit.Current.Slope.Max.ToString)
            End If
            If Not limit.Current.Offset.CheckLimit(cValue.Current.Offset) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:curr:off V: " & cValue.Current.Offset.ToString & ", Min: " & limit.Current.Offset.Min.ToString & ", Max: " & limit.Current.Offset.Max.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'VOLTAGE
            If Not limit.Voltage.Slope.CheckLimit(cValue.Voltage.Slope) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:volt:slo V: " & cValue.Voltage.Slope.ToString & ", Min: " & limit.Voltage.Slope.Min.ToString & ", Max: " & limit.Voltage.Slope.Max.ToString)
            End If
            If Not limit.Voltage.Offset.CheckLimit(cValue.Voltage.Offset) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:curr:off V: " & cValue.Voltage.Offset.ToString & ", Min: " & limit.Voltage.Offset.Min.ToString & ", Max: " & limit.Voltage.Offset.Max.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'DAC
            If Not limit.Dac.Slope.CheckLimit(cValue.Dac.Slope) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:dac:slo V: " & cValue.Dac.Slope.ToString & ", Min: " & limit.Dac.Slope.Min.ToString & ", Max: " & limit.Dac.Slope.Max.ToString)
            End If
            If Not limit.Dac.Offset.CheckLimit(cValue.Dac.Offset) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:dac:off V: " & cValue.Dac.Offset.ToString & ", Min: " & limit.Dac.Offset.Min.ToString & ", Max: " & limit.Dac.Offset.Max.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'CX
            Dim xx(), yy() As Double
            ReDim xx(14)
            ReDim yy(14)
            For i = 0 To 14
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.CxTempComp
            Next
            If Not limit.Cx.I_low.CheckLimit(xx, yy, cValue.Cx.I_low, minRatio, maxRatio) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cx:Ilow err\\ Vmin: " & minRatio.ToString & ", Vmax: " & maxRatio.ToString & ", Min: " & limit.Cx.I_low.DevBottomPercent.ToString & ", Max: " & limit.Cx.I_low.DevTopPercent.ToString)
            End If
            If Not limit.Cx.I_high.CheckLimit(xx, yy, cValue.Cx.I_high, minRatio, maxRatio) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cx:Ihigh err\\ Vmin: " & minRatio.ToString & ", Vmax: " & maxRatio.ToString & ", Min: " & limit.Cx.I_high.DevBottomPercent.ToString & ", Max: " & limit.Cx.I_high.DevTopPercent.ToString)
            End If

            If Not limit.Cx.I_Border.CheckLimit(cValue.Cx.I_border) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cx:Iborder err\\ V: " & cValue.Cx.I_border.ToString & ", Fix: " & limit.Cx.I_Border.Fixed.ToString)
            End If
            If Not limit.Cx.T_low.CheckLimit(cValue.Cx.T_low) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cx:Tlow err\\ V: " & cValue.Cx.T_low.F_x0.ToString & ", Fix: " & limit.Cx.T_low.Fixed.F_x0.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cx.T_low.F_x1.ToString & ", Fix: " & limit.Cx.T_low.Fixed.F_x1.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cx.T_low.F_x2.ToString & ", Fix: " & limit.Cx.T_low.Fixed.F_x2.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cx.T_low.F_x3.ToString & ", Fix: " & limit.Cx.T_low.Fixed.F_x3.ToString)
            End If
            If Not limit.Cx.T_high.CheckLimit(cValue.Cx.T_high) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cx:Thigh err\\ V: " & cValue.Cx.T_high.F_x0.ToString & ", Fix: " & limit.Cx.T_high.Fixed.F_x0.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cx.T_high.F_x1.ToString & ", Fix: " & limit.Cx.T_high.Fixed.F_x1.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cx.T_high.F_x2.ToString & ", Fix: " & limit.Cx.T_high.Fixed.F_x2.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cx.T_high.F_x3.ToString & ", Fix: " & limit.Cx.T_high.Fixed.F_x3.ToString)
            End If
            If Not limit.Cx.T_Border.CheckLimit(cValue.Cx.T_border) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cx:Tborder V: " & cValue.Cx.T_border.ToString & ", Fix: " & limit.Cx.T_Border.Fixed.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'CY
            For i = 0 To 14
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.CyTempComp
            Next
            If Not limit.Cy.I_low.CheckLimit(xx, yy, cValue.Cy.I_low, minRatio, maxRatio) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cy:Ilow err\\ Vmin: " & minRatio.ToString & ", Vmax: " & maxRatio.ToString & ", Min: " & limit.Cy.I_low.DevBottomPercent.ToString & ", Max: " & limit.Cy.I_low.DevTopPercent.ToString)
            End If
            If Not limit.Cy.I_high.CheckLimit(xx, yy, cValue.Cy.I_high, minRatio, maxRatio) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cy:Ihigh err\\ Vmin: " & minRatio.ToString & ", Vmax: " & maxRatio.ToString & ", Min: " & limit.Cy.I_high.DevBottomPercent.ToString & ", Max: " & limit.Cy.I_high.DevTopPercent.ToString)
            End If

            If Not limit.Cy.I_Border.CheckLimit(cValue.Cy.I_border) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cy:Iborder V: " & cValue.Cy.I_border.ToString & ", Fix: " & limit.Cy.I_Border.Fixed.ToString)
            End If
            If Not limit.Cy.T_low.CheckLimit(cValue.Cy.T_low) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cy:Tlow err\\ V: " & cValue.Cy.T_low.F_x0.ToString & ", Fix: " & limit.Cy.T_low.Fixed.F_x0.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cy.T_low.F_x1.ToString & ", Fix: " & limit.Cy.T_low.Fixed.F_x1.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cy.T_low.F_x2.ToString & ", Fix: " & limit.Cy.T_low.Fixed.F_x2.ToString & _
                                         vbCrLf & "                  V: " & cValue.Cy.T_low.F_x3.ToString & ", Fix: " & limit.Cy.T_low.Fixed.F_x3.ToString)
            End If
            If Not limit.Cy.T_high.CheckLimit(cValue.Cy.T_high) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cy:Thigh err\\ V: " & cValue.Cy.T_high.F_x0.ToString & ", Fix: " & limit.Cy.T_high.Fixed.F_x0.ToString & _
                                         vbCrLf & "                   V: " & cValue.Cy.T_high.F_x1.ToString & ", Fix: " & limit.Cy.T_high.Fixed.F_x1.ToString & _
                                         vbCrLf & "                   V: " & cValue.Cy.T_high.F_x2.ToString & ", Fix: " & limit.Cy.T_high.Fixed.F_x2.ToString & _
                                         vbCrLf & "                   V: " & cValue.Cy.T_high.F_x3.ToString & ", Fix: " & limit.Cy.T_high.Fixed.F_x3.ToString)
            End If
            If Not limit.Cy.T_Border.CheckLimit(cValue.Cy.T_border) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:cy:Tborder V: " & cValue.Cy.T_border.ToString & ", Fix: " & limit.Cy.T_Border.Fixed.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'tempChip
            If Not limit.TempChip.Eta.CheckLimit(cValue.TempChip.Eta) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:tempChip:eta V: " & cValue.TempChip.Eta.ToString & ", Fix: " & limit.TempChip.Eta.Fixed.ToString)
            End If
            If Not limit.TempChip.Rth.CheckLimit(cValue.TempChip.Rth) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:tempChip:rth V: " & cValue.TempChip.Rth.ToString & ", Fix: " & limit.TempChip.Rth.Fixed.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'Phi
            If Not limit.Phi.T.CheckLimit(cValue.Phi.T) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:T err\\ V: " & cValue.Phi.T.F_x0.ToString & ", Fix: " & limit.Phi.T.Fixed.F_x0.ToString & _
                                         vbCrLf & "                V: " & cValue.Phi.T.F_x1.ToString & ", Fix: " & limit.Phi.T.Fixed.F_x1.ToString & _
                                         vbCrLf & "                V: " & cValue.Phi.T.F_x2.ToString & ", Fix: " & limit.Phi.T.Fixed.F_x2.ToString & _
                                         vbCrLf & "                V: " & cValue.Phi.T.F_x3.ToString & ", Fix: " & limit.Phi.T.Fixed.F_x3.ToString)
            End If
            'spline1
            ReDim xx(1)
            ReDim yy(1)
            xx(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.Current
            xx(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 2))).MValue.Current
            yy(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 1))).MValue.PhiTempComp
            yy(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 2))).MValue.PhiTempComp
            If Not limit.Phi.I_spl1.CheckLimit(xx, yy, cValue.Phi.I_spl1, midValue) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:spl1 err\\ V: " & midValue.ToString & ", Min: " & limit.Phi.I_spl1.MinDelta.ToString & ", Max: " & limit.Phi.I_spl1.MaxDelta.ToString)
            End If
            'spline2
            xx(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 2))).MValue.Current
            xx(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 3))).MValue.Current
            yy(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 2))).MValue.PhiTempComp
            yy(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 3))).MValue.PhiTempComp
            If Not limit.Phi.I_spl2.CheckLimit(xx, yy, cValue.Phi.I_spl2, midValue) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:spl2 err\\ V: " & midValue.ToString & ", Min: " & limit.Phi.I_spl2.MinDelta.ToString & ", Max: " & limit.Phi.I_spl2.MaxDelta.ToString)
            End If
            'spline3
            xx(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 3))).MValue.Current
            xx(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 4))).MValue.Current
            yy(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 3))).MValue.PhiTempComp
            yy(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 4))).MValue.PhiTempComp
            If Not limit.Phi.I_spl3.CheckLimit(xx, yy, cValue.Phi.I_spl3, midValue) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:spl3 err\\ V: " & midValue.ToString & ", Min: " & limit.Phi.I_spl3.MinDelta.ToString & ", Max: " & limit.Phi.I_spl3.MaxDelta.ToString)
            End If
            'spline4
            xx(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 4))).MValue.Current
            xx(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 5))).MValue.Current
            yy(0) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 4))).MValue.PhiTempComp
            yy(1) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = 5))).MValue.PhiTempComp
            If Not limit.Phi.I_spl4.CheckLimit(xx, yy, cValue.Phi.I_spl4, midValue) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:spl4 err\\ V: " & midValue.ToString & ", Min: " & limit.Phi.I_spl4.MinDelta.ToString & ", Max: " & limit.Phi.I_spl4.MaxDelta.ToString)
            End If
            'last
            ReDim xx(11)
            ReDim yy(11)
            For i = 0 To 11
                Dim stepNumber As Integer
                stepNumber = i + 5
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.PhiTempComp
            Next
            If Not limit.Phi.I_last.CheckLimit(xx, yy, cValue.Phi.I_last, minRatio, maxRatio) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:spllast err\\ Vmin: " & minRatio.ToString & ", Vmax: " & maxRatio.ToString & ", Min: " & limit.Phi.I_last.DevBottomPercent.ToString & ", Max: " & limit.Phi.I_last.DevTopPercent.ToString)
            End If
            If Not limit.Phi.Border_I_1.CheckLimit(cValue.Phi.Border_I_1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:border1 V: " & cValue.Phi.Border_I_1.ToString & ", Min: " & limit.Phi.Border_I_1.Min.ToString & ", Max: " & limit.Phi.Border_I_1.Max.ToString)
            End If
            If Not limit.Phi.Border_I_2.CheckLimit(cValue.Phi.Border_I_2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:border2 V: " & cValue.Phi.Border_I_2.ToString & ", Min: " & limit.Phi.Border_I_2.Min.ToString & ", Max: " & limit.Phi.Border_I_2.Max.ToString)
            End If
            If Not limit.Phi.Border_I_3.CheckLimit(cValue.Phi.Border_I_3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:border3 V: " & cValue.Phi.Border_I_3.ToString & ", Min: " & limit.Phi.Border_I_3.Min.ToString & ", Max: " & limit.Phi.Border_I_3.Max.ToString)
            End If
            If Not limit.Phi.Border_I_4.CheckLimit(cValue.Phi.Border_I_4) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phi:border4 V: " & cValue.Phi.Border_I_4.ToString & ", Min: " & limit.Phi.Border_I_4.Min.ToString & ", Max: " & limit.Phi.Border_I_4.Max.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'PhiADC
            'T

            'I

            'spline_ADC_1

            'spline_ADC_2
            'spline_ADC_3

            'If Not limit.PhiADC.Border_Adc_1.CheckLimit(cValue.PhiADC.Border_Adc_1) Then
            '    resultGood = False
            '    RaiseEvent addLogFile(Me, "## " & color & "lim:phiADC:borderADC1 V: " & cValue.PhiADC.Border_Adc_1.ToString & ", Min: " & limit.PhiADC.Border_Adc_1.Min.ToString & ", Max: " & limit.PhiADC.Border_Adc_1.Max.ToString)
            'End If
            'If Not limit.PhiADC.Border_Adc_2.CheckLimit(cValue.PhiADC.Border_Adc_2) Then
            '    resultGood = False
            '    RaiseEvent addLogFile(Me, "## " & color & "lim:phiADC:borderADC2 V: " & cValue.PhiADC.Border_Adc_2.ToString & ", Min: " & limit.PhiADC.Border_Adc_2.Min.ToString & ", Max: " & limit.PhiADC.Border_Adc_2.Max.ToString)
            'End If
            'If Not limit.PhiADC.Border_Adc_3.CheckLimit(cValue.PhiADC.Border_Adc_3) Then
            '    resultGood = False
            '    RaiseEvent addLogFile(Me, "## " & color & "lim:phiADC:borderADC3 V: " & cValue.PhiADC.Border_Adc_3.ToString & ", Min: " & limit.PhiADC.Border_Adc_3.Min.ToString & ", Max: " & limit.PhiADC.Border_Adc_3.Max.ToString)
            'End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'phiMax
            If Not limit.PhiMax.CheckLimit(cValue.PhiMax) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:phiMax V: " & cValue.PhiMax.ToString & ", Min: " & limit.PhiMax.Min.ToString & ", Max: " & limit.PhiMax.Max.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'dacMax
            If Not limit.DacMax.CheckLimit(cValue.DacMax) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:dacMax V: " & cValue.DacMax.ToString & ", Min: " & limit.DacMax.Min.ToString & ", Max: " & limit.DacMax.Max.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'IByDac
            ReDim xx(14)
            ReDim yy(14)
            For i = 0 To 14
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntensityDAC
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
            Next
            If Not limit.IByDac.Fct.CheckLimit(xx, yy, cValue.IByDac.Fct, minRatio, maxRatio) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:iByDac err\\ Vmin: " & minRatio.ToString & ", Vmax: " & maxRatio.ToString & ", Min: " & limit.IByDac.Fct.DevBottomPercent.ToString & ", Max: " & limit.IByDac.Fct.DevTopPercent.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'UByI
            ReDim xx(14)
            ReDim yy(14)
            For i = 0 To 14
                Dim stepNumber As Integer
                stepNumber = i + 2
                xx(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current
                yy(i) = m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Voltage
            Next
            If Not limit.UByI.Fct.CheckLimit(xx, yy, cValue.UByI.Fct, minRatio, maxRatio) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## " & color & "lim:uByI err\\ Vmin: " & minRatio.ToString & ", Vmax: " & maxRatio.ToString & ", Min: " & limit.UByI.Fct.DevBottomPercent.ToString & ", Max: " & limit.UByI.Fct.DevTopPercent.ToString)
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''
            'Super R9 can not be checked, because not generated yet
            'If Not limit.SuperR9.Cx.CheckLimit(cValue.SuperR9.Cx) Then
            'resultGood = False
            'End If
            'If Not limit.SuperR9.Cy.CheckLimit(cValue.SuperR9.Cy) Then
            'resultGood = False
            'End If
            fctGood = resultGood
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckCalibLimitsRef: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfung Limits Kalibrierung",
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepCheckCalibSuperR9LimitsRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim index As Integer
            Dim cValue As CalibValues
            Dim limit As CalibLimits
            Dim resultGood As Boolean
            resultGood = True

            index = m_calibList.FindIndex(Function(x) (x.Color = "red"))
            cValue = m_calibList.Item(index).CValue
            limit = m_calibList.Item(index).CLimit

            '''''''''''''''''''''''''''''''''''''''''''''''''
            'Super R9 
            If Not limit.SuperR9.Cx.CheckLimit(cValue.SuperR9.Cx) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## lim:SuperR9 cx: " & cValue.SuperR9.Cx.ToString & ", Min: " & limit.SuperR9.Cx.Min.ToString & ", Max: " & limit.SuperR9.Cx.Max.ToString)
            End If
            If Not limit.SuperR9.Cy.CheckLimit(cValue.SuperR9.Cy) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "## lim:SuperR9 cx: " & cValue.SuperR9.Cy.ToString & ", Min: " & limit.SuperR9.Cy.Min.ToString & ", Max: " & limit.SuperR9.Cy.Max.ToString)
            End If

            fctGood = resultGood
            If fctGood Then m_dataCalib.CalibSuperR9Done = 1
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckCalibSuperR9LimitsRef: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfung Limits Kalibrierung SuperR9",
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepCheckCommunicationCas(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data0, CasCommunication.cCAS140)
            Dim text() As String
            text = cas.ReadDeviceTypeNames()
            If (IsNothing(text)) Then 'todo
                Throw New Exception("ReadDeviceTypeNames failed.")
            Else
                fctGood = True
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckCommunicationCas: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfung CAS Kommunikation",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCheckCommunicationHelios(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim build As Integer
            Dim sub_rel As Integer
            Dim main_rel As Integer
            Dim state As Integer
            Dim returnValue As Boolean
            returnValue = can.GetSoftwareVersion(CAN_DEST, build, sub_rel, main_rel, state)
            If returnValue = False Then
                Throw New Exception("GetSoftwareVersion returned FALSE")
            ElseIf main_rel = 2 Then
                fctGood = True
            Else
                Throw New Exception("GetSoftwareVersion: main_rel != 2")
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckCommunicationHelios: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfung Helios Kommunikation",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCheckCommunicationPS(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim result As String = String.Empty
            Dim ps As HardwareCommunication.TcpKeysightN5767Communication
            ps = DirectCast(data0, HardwareCommunication.TcpKeysightN5767Communication)
            ps.getIdentification(result)
            If String.IsNullOrEmpty(result) Then
                'communication PS works not
                fctGood = False
                Throw New Exception("ps.getIdentification failed")
            Else
                'communication PS works
                '   RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Communication to PS works")
                fctGood = True
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckCommunicationPS: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfung Spannungsversorgung Kommunikation",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCheckGeneralFunction(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim textResult As String = String.Empty
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data1, CasCommunication.cCAS140)
            Dim PD5dark, PD5, Dummy As UShort

            Dim decelerate As Short
            Dim boolean1, boolean2, boolean3, boolean4, boolean5 As Boolean
            If Not can.GetSystemSettings(CAN_DEST, boolean1, decelerate, boolean2, boolean3, boolean4, boolean5) Then
                result = False
            Else
                If decelerate <> 0 Then
                    textResult += "DIP-Switch falsch, "
                    RaiseEvent addLogFile(Me, "##decelerate: " & decelerate.ToString & ", Min: 0, Max: 0")
                End If
            End If


            If Not can.GetPDUVSum(CAN_DEST, Dummy, PD5dark) Then result = False 'Measure Photodiode Dark
            m_dataHelios.PD5dark = PD5dark
            If PD5dark > 500 Then
                textResult = textResult + "PD zu hoch, "
                RaiseEvent addLogFile(Me, "##PD5dark: " & PD5dark.ToString & ", Min: 0, Max: 500")
            End If

            'set Red and check if red is red
            If Not can.SetRGbColor(CAN_DEST, 255, 0, 0) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 50) Then result = False

            cas.AutoRangeMeasurement = False
            cas.FilterPosition = 2
            cas.Averages = 2
            cas.IntegrationTime = 20
            If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
            If cas.Cx > 0.8 Or cas.Cx < 0.5 Or cas.Cy > 0.4 Or cas.Cy < 0.2 Then
                textResult = textResult + "LED Rot falsch, "
                RaiseEvent addLogFile(Me, "##red:Cx: " & cas.Cx.ToString & ", Min: 0.5, Max: 0.8")
                RaiseEvent addLogFile(Me, "##red:Cy: " & cas.Cy.ToString & ", Min: 0.2, Max: 0.4")
                RaiseEvent addLogFile(Me, "##red:Phi: " & cas.Flux.ToString)
            End If

            'set Green and check if green is green
            If Not can.SetRGbColor(CAN_DEST, 0, 255, 0) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 50) Then result = False

            cas.AutoRangeMeasurement = False
            cas.FilterPosition = 2
            cas.Averages = 2
            cas.IntegrationTime = 20
            If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
            If cas.Cx > 0.4 Or cas.Cx < 0 Or cas.Cy > 0.9 Or cas.Cy < 0.5 Then
                textResult = textResult + "LED Gruen falsch, "
                RaiseEvent addLogFile(Me, "##green:Cx: " & cas.Cx.ToString & ", Min: 0, Max: 0.4")
                RaiseEvent addLogFile(Me, "##green:Cy: " & cas.Cy.ToString & ", Min: 0.5, Max: 0.9")
                RaiseEvent addLogFile(Me, "##green:Phi: " & cas.Flux.ToString)
            End If

            If Not can.GetPDUVSum(CAN_DEST, Dummy, PD5) Then result = False 'Measure Photodiode
            m_dataHelios.PD5 = PD5
            If (PD5 < 800) Or (PD5 > 4095) Then
                textResult = textResult + "PD zu niedrig, "
                RaiseEvent addLogFile(Me, "##PD5: " & PD5.ToString & ", Min: 800, Max: 4095")
            End If

            'set Blue and check if blue is blue
            If Not can.SetRGbColor(CAN_DEST, 0, 0, 255) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 50) Then result = False

            cas.AutoRangeMeasurement = False
            cas.FilterPosition = 2
            cas.Averages = 2
            cas.IntegrationTime = 20
            If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
            If cas.Cx > 0.3 Or cas.Cx < 0 Or cas.Cy > 0.4 Or cas.Cy < 0 Then
                textResult = textResult + "LED Blau falsch, "
                RaiseEvent addLogFile(Me, "##blue:Cx: " & cas.Cx.ToString & ", Min: 0, Max: 0.3")
                RaiseEvent addLogFile(Me, "##blue:Cy: " & cas.Cy.ToString & ", Min: 0, Max: 0.4")
                RaiseEvent addLogFile(Me, "##blue:Phi: " & cas.Flux.ToString)
            End If

            If Not can.SetRgbIntensity(CAN_DEST, 5) Then result = False
            If Not can.SetRgbOnState(CAN_DEST, False) Then result = False

            Dim t1, t2, t3, t5, t6 As Double

            If Not can.GetTemperature(CAN_DEST, 0, t1) Then
                result = False
            Else
                m_dataHelios.T1 = t1
                If t1 > 40 Or t1 < 15 Then
                    textResult = textResult + "TempR defekt, "
                    RaiseEvent addLogFile(Me, "##TempR: " & t1.ToString & ", Min: 15, Max: 40")
                End If
            End If
            If Not can.GetTemperature(CAN_DEST, 1, t2) Then
                result = False
            Else
                m_dataHelios.T2 = t2
                If t2 > 40 Or t2 < 15 Then
                    textResult = textResult + "TempG defekt, "
                    RaiseEvent addLogFile(Me, "##TempG: " & t2.ToString & ", Min: 15, Max: 40")
                End If
            End If
            If Not can.GetTemperature(CAN_DEST, 2, t3) Then
                result = False
            Else
                m_dataHelios.T3 = t3
                If t3 > 40 Or t3 < 15 Then
                    textResult = textResult + "TempB defekt, "
                    RaiseEvent addLogFile(Me, "##TempB: " & t3.ToString & ", Min: 15, Max: 40")
                End If
            End If
            If Not can.GetTemperature(CAN_DEST, 4, t5) Then
                result = False
            Else
                m_dataHelios.T5 = t5
                If t5 > 40 Or t5 < 15 Then
                    textResult = textResult + "TempMainPCB defekt, "
                    RaiseEvent addLogFile(Me, "##TempMainPCB: " & t5.ToString & ", Min: 15, Max: 40")
                End If
            End If
            If Not can.GetTemperature(CAN_DEST, 5, t6) Then
                result = False
            Else
                m_dataHelios.T6 = t6
                If t6 > 40 Or t6 < 15 Then
                    textResult = textResult + "SensorTemp defekt, "
                    RaiseEvent addLogFile(Me, "##SensorTemp: " & t6.ToString & ", Min: 15, Max: 40")
                End If
            End If

            If textResult = String.Empty Then
                fctGood = result
            Else
                RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkGeneralFunction: " & textResult)
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckGeneralFunction: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "Genereller Test ist gut",
                           "Abbruch " & textResult)
        End Try
    End Sub

    Public Sub stepCheckGeneralFunctionV(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim textResult As String = String.Empty
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data1, CasCommunication.cCAS140)

            Dim t4, t5, t6 As Double

            If Not can.GetTemperature(CAN_DEST, 3, t4) Then
                result = False
            Else
                m_dataHelios.T4 = t4
                If t4 > 40 Or t4 < 15 Then
                    textResult = textResult + "TempV defekt, "
                    RaiseEvent addLogFile(Me, "##TempV: " & t4.ToString & ", Min: 15, Max: 40")
                End If
            End If
            If Not can.GetTemperature(CAN_DEST, 4, t5) Then
                result = False
            Else
                m_dataHelios.T5 = t5
                If t5 > 40 Or t5 < 15 Then
                    textResult = textResult + "TempMainPCB defekt, "
                    RaiseEvent addLogFile(Me, "##TempMainPCB: " & t5.ToString & ", Min: 15, Max: 40")
                End If
            End If
            If Not can.GetTemperature(CAN_DEST, 5, t6) Then
                result = False
            Else
                m_dataHelios.T6 = t6
                If t6 > 40 Or t6 < 15 Then
                    textResult = textResult + "SensorTemp defekt, "
                    RaiseEvent addLogFile(Me, "##SensorTemp: " & t6.ToString & ", Min: 15, Max: 40")
                End If
            End If

            If textResult = String.Empty Then
                fctGood = result
            Else
                RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkGeneralFunctionV: " & textResult)
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckGeneralFunctionV: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "Genereller Test ist gut",
                           "Abbruch " & textResult)
        End Try
    End Sub

    Public Sub stepCheckIfAllIsGood(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = True
        Try
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckIfAllIsGood: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "Alle Schritte bisher erfolgreich",
                           "Abbruch ")
        End Try
    End Sub

    Public Sub stepCheckIfInDebug(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = True
        Try
            Dim IsInDebug As Boolean
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim valueText As String
            valueText = DirectCast(data1, String)

            Dim d1, d2, d3 As UShort
            Try
                can.GetPDRGB(CAN_DEST, d1, d2, d3)
                IsInDebug = (d1 <> 0) Or (d2 <> 0) Or (d3 <> 0)
            Catch ex As Exception
                IsInDebug = False
            End Try

            Select Case valueText
                Case "true"
                    If IsInDebug = True Then
                        fctGood = True
                        RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkIfInDebug: YES, OK")
                    Else
                        fctGood = False
                        RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkIfInDebug: NO, FAIL")
                    End If
                Case "false"
                    If IsInDebug = False Then
                        fctGood = True
                        RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkIfInDebug: NO, OK")
                    Else
                        fctGood = False
                        RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkIfInDebug: YES, FAIL")
                    End If
                Case Else
                    fctGood = False
                    RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkIfInDebug: invalid parameter: " & valueText)
            End Select
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckIfInDebug: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "Jumperstecker ist richtig gesteckt",
                           "Jumperstecker ist NICHT!!! richtig gesteckt (oder DIP-Switch falsch)")
        End Try
    End Sub

    Public Sub stepCheckLimitsFinal(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim limit As FinalMeasureLimits
            limit = New FinalMeasureLimits()
            Dim resultGood As Boolean = True

            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet = xlsWorkBook.Sheets("LimitFinal")
            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing
            currentFind = usedRange.Find("5500K 100 current R", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.CurrentR.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.CurrentR.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 current G", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.CurrentG.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.CurrentG.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 current B", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.CurrentB.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.CurrentB.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 phi", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.Phi5500K100.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.Phi5500K100.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 CRI", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.CRI5500K100.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.CRI5500K100.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 R9", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.R95500K100.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.R95500K100.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 MacAdam", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.MacAdam5500K100.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.MacAdam5500K100.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 SuperCx", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.SuperCx5500K100.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.SuperCx5500K100.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 SuperCy", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.SuperCy5500K100.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.SuperCy5500K100.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100 SuperR9", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.SuperR95500K100.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.SuperR95500K100.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

            currentFind = usedRange.Find("5500K 5 phi", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.Phi5500K005.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.Phi5500K005.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 5 MacAdam", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.MacAdam5500K005.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.MacAdam5500K005.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

            currentFind = usedRange.Find("5500K 100low phi", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.Phi5500K100low.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.Phi5500K100low.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("5500K 100low MacAdam", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.MacAdam5500K100low.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.MacAdam5500K100low.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value


            If Not limit.CurrentR.CheckLimit(m_dataHelios100CCT55.I1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:current R V: " & m_dataHelios100CCT55.I1.ToString & ", Min: " & limit.CurrentR.Min.ToString & ", Max: " & limit.CurrentR.Max.ToString)
            End If
            If Not limit.CurrentG.CheckLimit(m_dataHelios100CCT55.I2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:current G V: " & m_dataHelios100CCT55.I2.ToString & ", Min: " & limit.CurrentG.Min.ToString & ", Max: " & limit.CurrentG.Max.ToString)
            End If
            If Not limit.CurrentB.CheckLimit(m_dataHelios100CCT55.I3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:current B V: " & m_dataHelios100CCT55.I3.ToString & ", Min: " & limit.CurrentB.Min.ToString & ", Max: " & limit.CurrentB.Max.ToString)
            End If
            If Not limit.Phi5500K100.CheckLimit(m_dataCasMeas100CCT55.PhotoIntegral) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:Phi 5500K 100% V: " & m_dataCasMeas100CCT55.PhotoIntegral.ToString & ", Min: " & limit.Phi5500K100.Min.ToString & ", Max: " & limit.Phi5500K100.Max.ToString)
            End If
            If Not limit.CRI5500K100.CheckLimit(m_dataCasMeas100CCT55.CRI) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:CRI 5500K 100% V: " & m_dataCasMeas100CCT55.CRI.ToString & ", Min: " & limit.CRI5500K100.Min.ToString & ", Max: " & limit.CRI5500K100.Max.ToString)
            End If
            If Not limit.R95500K100.CheckLimit(m_dataCasMeas100CCT55.R9) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:R9 5500K 100% V: " & m_dataCasMeas100CCT55.R9.ToString & ", Min: " & limit.R95500K100.Min.ToString & ", Max: " & limit.R95500K100.Max.ToString)
            End If

            m_dataCasMeas100CCT55.MacAdam = calcMacAdam(0.332, 0.347, m_dataCasMeas100CCT55.Cx, m_dataCasMeas100CCT55.Cy)
            If Not limit.MacAdam5500K100.CheckLimit(m_dataCasMeas100CCT55.MacAdam) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:MacAdam 5500K 100% V: " & m_dataCasMeas100CCT55.MacAdam.ToString & ", Min: " & limit.MacAdam5500K100.Min.ToString & ", Max: " & limit.MacAdam5500K100.Max.ToString)
            End If
            If Not limit.SuperCx5500K100.CheckLimit(m_dataCasMeas100SuperR9.Cx) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:SuperCx 5500K 100% V: " & m_dataCasMeas100SuperR9.Cx.ToString & ", Min: " & limit.SuperCx5500K100.Min.ToString & ", Max: " & limit.SuperCx5500K100.Max.ToString)
            End If
            If Not limit.SuperCy5500K100.CheckLimit(m_dataCasMeas100SuperR9.Cy) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:SuperCy 5500K 100% V: " & m_dataCasMeas100SuperR9.Cy.ToString & ", Min: " & limit.SuperCy5500K100.Min.ToString & ", Max: " & limit.SuperCy5500K100.Max.ToString)
            End If
            If Not limit.SuperR95500K100.CheckLimit(m_dataCasMeas100SuperR9.R9) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:SuperR9 5500K 100% V: " & m_dataCasMeas100SuperR9.R9.ToString & ", Min: " & limit.SuperR95500K100.Min.ToString & ", Max: " & limit.SuperR95500K100.Max.ToString)
            End If

            If Not limit.Phi5500K005.CheckLimit(m_dataCasMeas5CCT55.PhotoIntegral) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:Phi 5500K 5% V: " & m_dataCasMeas5CCT55.PhotoIntegral.ToString & ", Min: " & limit.Phi5500K005.Min.ToString & ", Max: " & limit.Phi5500K005.Max.ToString)
            End If
            m_dataCasMeas5CCT55.MacAdam = calcMacAdam(0.332, 0.347, m_dataCasMeas5CCT55.Cx, m_dataCasMeas5CCT55.Cy)
            If Not limit.MacAdam5500K005.CheckLimit(m_dataCasMeas5CCT55.MacAdam) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:MacAdam 5500K 5% V: " & m_dataCasMeas5CCT55.MacAdam.ToString & ", Min: " & limit.MacAdam5500K005.Min.ToString & ", Max: " & limit.MacAdam5500K005.Max.ToString)
            End If

            If Not limit.Phi5500K100low.CheckLimit(m_dataCasMeas100CCT55low.PhotoIntegral) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:Phi 5500K 100% low V: " & m_dataCasMeas100CCT55low.PhotoIntegral.ToString & ", Min: " & limit.Phi5500K100low.Min.ToString & ", Max: " & limit.Phi5500K100low.Max.ToString)
            End If
            m_dataCasMeas100CCT55low.MacAdam = calcMacAdam(0.332, 0.347, m_dataCasMeas100CCT55low.Cx, m_dataCasMeas100CCT55low.Cy)
            If Not limit.MacAdam5500K100low.CheckLimit(m_dataCasMeas100CCT55low.MacAdam) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##final:MacAdam 5500K 100% low V: " & m_dataCasMeas100CCT55low.MacAdam.ToString & ", Min: " & limit.MacAdam5500K100low.Min.ToString & ", Max: " & limit.MacAdam5500K100low.Max.ToString)
            End If
            xlsWorkBook.Close()
            xlsApp.Quit()
            fctGood = resultGood
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckLimitsFinal: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfung Limits Abschlussmessung",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCheckLimitsPS(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = True
        Dim voltage As Double = 0
        Dim current As Double = 0
        Dim voltageString As String = Nothing
        Dim currentString As String = Nothing
        Try
            Dim dataPS As DataPS
            Dim dataString As String
            dataString = DirectCast(data0, String)
            dataPS = GetVariableInstance(dataString)

            'load values from Excel File
            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet = xlsWorkBook.Sheets("LimitPS")
            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing
            Dim limitU, limitI As New LimitMinMax

            Select Case dataString
                Case "dataPSSby"
                    currentFind = usedRange.Find("IinSby", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                    limitI.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                    limitI.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                    currentFind = usedRange.Find("UinSby", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                    limitU.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                    limitU.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                    If Not limitU.CheckLimit(dataPS.Voltage) Then
                        fctGood = False
                        RaiseEvent addLogFile(Me, "##ps:volt:sby V: " & dataPS.Voltage.ToString & ", Min: " & limitU.Min.ToString & ", Max: " & limitU.Max.ToString)
                    End If
                    If Not limitI.CheckLimit(dataPS.Current) Then
                        fctGood = False
                        RaiseEvent addLogFile(Me, "##ps:curr:sby V: " & dataPS.Current.ToString & ", Min: " & limitI.Min.ToString & ", Max: " & limitI.Max.ToString)
                    End If

                Case "dataPSSbyV"
                    currentFind = usedRange.Find("IinSbyV", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                    limitI.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                    limitI.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                    currentFind = usedRange.Find("UinSbyV", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                    limitU.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                    limitU.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                    If Not limitU.CheckLimit(dataPS.Voltage) Then
                        fctGood = False
                        RaiseEvent addLogFile(Me, "##ps:volt:sbyV V: " & dataPS.Voltage.ToString & ", Min: " & limitU.Min.ToString & ", Max: " & limitU.Max.ToString)
                    End If
                    If Not limitI.CheckLimit(dataPS.Current) Then
                        fctGood = False
                        RaiseEvent addLogFile(Me, "##ps:curr:sbyV V: " & dataPS.Current.ToString & ", Min: " & limitI.Min.ToString & ", Max: " & limitI.Max.ToString)
                    End If

                Case "dataPS100"
                    currentFind = usedRange.Find("Iin100", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                    limitI.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                    limitI.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                    If Not limitI.CheckLimit(dataPS.Current) Then
                        fctGood = False
                        RaiseEvent addLogFile(Me, "##ps:curr:100 V: " & dataPS.Current.ToString & ", Min: " & limitI.Min.ToString & ", Max: " & limitI.Max.ToString)
                    End If

                Case "dataPSV"
                    currentFind = usedRange.Find("IinV100", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                    limitI.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                    limitI.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                    If Not limitI.CheckLimit(dataPS.Current) Then
                        fctGood = False
                        RaiseEvent addLogFile(Me, "##ps:curr:100V V: " & dataPS.Current.ToString & ", Min: " & limitI.Min.ToString & ", Max: " & limitI.Max.ToString)
                    End If
            End Select

            xlsWorkBook.Close()
            xlsApp.Quit()
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckLimitsPS: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfen der Netzteil-Messung",
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepCheckLimitsRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim color As String = String.Empty
        Dim stepNumber As Integer = 0
        Dim fctGood As Boolean = False
        Try
            color = DirectCast(data0, String)
            stepNumber = Convert.ToInt32(DirectCast(data1, String))
            Dim index As Integer
            index = m_measurementList.FindIndex(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber)))
            Dim meas As MeasureValues
            meas = m_measurementList.Item(index).MValue
            Dim limit As MeasureLimits
            limit = m_measurementList.Item(index).MLimit

            Dim resultGood As Boolean
            resultGood = True

            If Not limit.AverageCount.CheckLimit(meas.AverageCount) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Average Count   V: " & meas.AverageCount.ToString & ", Fixed: " & limit.AverageCount.Fixed.ToString)
            End If
            If Not limit.CAS_ADC.CheckLimit(meas.CAS_ADC) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##CAS_ADC         V: " & meas.CAS_ADC.ToString & ", Min: " & limit.CAS_ADC.Min.ToString & ", Max: " & limit.CAS_ADC.Max.ToString)
            End If
            If Not limit.Current.CheckLimit(meas.Current) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Current         V: " & meas.Current.ToString & ", Min: " & limit.Current.Min.ToString & ", Max: " & limit.Current.Max.ToString)
            End If
            If Not limit.Cx.CheckLimit(meas.Cx) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Cx              V: " & meas.Cx.ToString & ", Min: " & limit.Cx.Min.ToString & ", Max: " & limit.Cx.Max.ToString)
            End If
            If Not limit.Cy.CheckLimit(meas.Cy) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Cy              V: " & meas.Cy.ToString & ", Min: " & limit.Cy.Min.ToString & ", Max: " & limit.Cy.Max.ToString)
            End If
            If Not limit.CxTempComp.CheckLimit(meas.CxTempComp) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Cx TempComp     V: " & meas.CxTempComp.ToString & ", Min: " & limit.CxTempComp.Min.ToString & ", Max: " & limit.CxTempComp.Max.ToString)
            End If
            If Not limit.CyTempComp.CheckLimit(meas.CyTempComp) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Cy Temp Comp    V: " & meas.CyTempComp.ToString & ", Min: " & limit.CyTempComp.Min.ToString & ", Max: " & limit.CyTempComp.Max.ToString)
            End If
            If Not limit.Filter.CheckLimit(meas.Filter) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Filter          V: " & meas.Filter.ToString & ", Fixed: " & limit.Filter.Fixed.ToString)
            End If
            If Not limit.IntegrationTime.CheckLimit(meas.IntegrationTime) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Int Time        V: " & meas.IntegrationTime.ToString & ", Min: " & limit.IntegrationTime.Min.ToString & ", Max: " & limit.IntegrationTime.Max.ToString)
            End If
            If Not limit.PD.CheckLimit(meas.PD) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##PD              V: " & meas.PD.ToString & ", Min: " & limit.PD.Min.ToString & ", Max: " & limit.PD.Max.ToString)
            End If
            If Not limit.Phi.CheckLimit(meas.Phi) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Phi             V: " & meas.Phi.ToString & ", Min: " & limit.Phi.Min.ToString & ", Max: " & limit.Phi.Max.ToString)
            End If
            If Not limit.PhiTempComp.CheckLimit(meas.PhiTempComp) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##PhiTempComp     V: " & meas.PhiTempComp.ToString & ", Min: " & limit.PhiTempComp.Min.ToString & ", Max: " & limit.PhiTempComp.Max.ToString)
            End If
            If Not limit.SetIntensity.CheckLimit(meas.SetIntensity) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##SetIntensity    V: " & meas.SetIntensity.ToString & ", Min: " & limit.SetIntensity.Min.ToString & ", Max: " & limit.SetIntensity.Max.ToString)
            End If
            If Not limit.Temperature.CheckLimit(meas.Temperature) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##Temperature     V: " & meas.Temperature.ToString & ", Min: " & limit.Temperature.Min.ToString & ", Max: " & limit.Temperature.Max.ToString)
            End If
            fctGood = resultGood
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & "_" &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & "_" &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & "_" &
                                      "Error in function stepCheckLimitsRef: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfen der Limits " & color & " " & stepNumber.ToString(),
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepCheckRefMeasLimits(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        If smData.ModuleType = -2 Then
            'Skip Check for Monthly Reference Measurement

            feedbackStatus(True,
               Me.Status,
               False,
               Me.CurrentStep,
               Me.MaxSteps,
               "Überprüfen der Referenzmessung",
               " übersprungen",
               " fehlgeschlagen")
            Exit Sub
        End If

        Dim fctGood As Boolean = False
        Try
            Dim limit(2) As RefMeasureLimits
            Dim color As String = String.Empty
            Dim resultGood As Boolean = True
            For i = 0 To 2
                limit(i) = New RefMeasureLimits
                Select Case i
                    Case 0
                        limit(i).Color = "red"
                    Case 1
                        limit(i).Color = "green"
                    Case 2
                        limit(i).Color = "blue"
                    Case Else
                        'Error
                End Select
            Next

            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet
            Select Case smData.ModuleType
                Case -1 'Referenz Messung / Golden Sample
                    xlsWorkSheet = xlsWorkBook.Sheets("LimitGolden")
                Case Else   'Normale Messung
                    xlsWorkSheet = xlsWorkBook.Sheets("LimitRef")
            End Select

            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing

            'load values from Excel File
            For Each lim As RefMeasureLimits In limit
                Dim text As String
                text = lim.Color
                currentFind = usedRange.Find(text & " current 14", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Current14.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Current14.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " voltage 14", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Voltage14.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Voltage14.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " temperature 14", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Temperature14.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Temperature14.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi 14", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Phi14.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Phi14.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cx 14", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Cx14.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Cx14.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cy 14", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Cy14.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Cy14.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " lambdaDom 14", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.LambdaDom14.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.LambdaDom14.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " current 50", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Current50.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Current50.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " voltage 50", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Voltage50.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Voltage50.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " temperature 50", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Temperature50.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Temperature50.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi 50", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Phi50.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Phi50.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cx 50", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Cx50.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Cx50.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cy 50", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.Cy50.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.Cy50.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " lambdaDom 50", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                lim.LambdaDom50.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                lim.LambdaDom50.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            Next
            xlsWorkBook.Close()
            xlsApp.Quit()


            ''''''''''''''''''''''''''''''''''''''''''''''
            'red
            If Not limit(0).Current14.CheckLimit(m_dataHeliosRed14.I1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:red:curr14 V: " & m_dataHeliosRed14.I1.ToString & ", Min: " & limit(0).Current14.Min.ToString & ", Max: " & limit(0).Current14.Max.ToString)
            End If
            If Not limit(0).Voltage14.CheckLimit(m_dataHeliosRed14.U1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:red:volt14 V: " & m_dataHeliosRed14.U1.ToString & ", Min: " & limit(0).Voltage14.Min.ToString & ", Max: " & limit(0).Voltage14.Max.ToString)
            End If
            If Not limit(0).Temperature14.CheckLimit(m_dataHeliosRed14.T1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:red:temp14 V: " & m_dataHeliosRed14.T1.ToString & ", Min: " & limit(0).Temperature14.Min.ToString & ", Max: " & limit(0).Temperature14.Max.ToString)
            End If

            If Not limit(0).Current50.CheckLimit(m_dataHeliosRed50.I1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:red:curr50 V: " & m_dataHeliosRed50.I1.ToString & ", Min: " & limit(0).Current50.Min.ToString & ", Max: " & limit(0).Current50.Max.ToString)
            End If
            If Not limit(0).Voltage50.CheckLimit(m_dataHeliosRed50.U1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:red:volt50 V: " & m_dataHeliosRed50.U1.ToString & ", Min: " & limit(0).Voltage50.Min.ToString & ", Max: " & limit(0).Voltage50.Max.ToString)
            End If
            If Not limit(0).Temperature50.CheckLimit(m_dataHeliosRed50.T1) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:red:temp50 V: " & m_dataHeliosRed50.T1.ToString & ", Min: " & limit(0).Temperature50.Min.ToString & ", Max: " & limit(0).Temperature50.Max.ToString)
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''
            'green
            If Not limit(1).Current14.CheckLimit(m_dataHeliosGre14.I2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:green:curr14 V: " & m_dataHeliosGre14.I2.ToString & ", Min: " & limit(1).Current14.Min.ToString & ", Max: " & limit(1).Current14.Max.ToString)
            End If
            If Not limit(1).Voltage14.CheckLimit(m_dataHeliosGre14.U2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:green:volt14 V: " & m_dataHeliosGre14.U2.ToString & ", Min: " & limit(1).Voltage14.Min.ToString & ", Max: " & limit(1).Voltage14.Max.ToString)
            End If
            If Not limit(1).Temperature14.CheckLimit(m_dataHeliosGre14.T2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:green:temp14 V: " & m_dataHeliosGre14.T2.ToString & ", Min: " & limit(1).Temperature14.Min.ToString & ", Max: " & limit(1).Temperature14.Max.ToString)
            End If

            If Not limit(1).Current50.CheckLimit(m_dataHeliosGre50.I2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:green:curr50 V: " & m_dataHeliosGre50.I2.ToString & ", Min: " & limit(1).Current50.Min.ToString & ", Max: " & limit(1).Current50.Max.ToString)
            End If
            If Not limit(1).Voltage50.CheckLimit(m_dataHeliosGre50.U2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:green:volt50 V: " & m_dataHeliosGre50.U2.ToString & ", Min: " & limit(1).Voltage50.Min.ToString & ", Max: " & limit(1).Voltage50.Max.ToString)
            End If
            If Not limit(1).Temperature50.CheckLimit(m_dataHeliosGre50.T2) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:green:temp50 V: " & m_dataHeliosGre50.T2.ToString & ", Min: " & limit(1).Temperature50.Min.ToString & ", Max: " & limit(1).Temperature50.Max.ToString)
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''
            'blue
            If Not limit(2).Current14.CheckLimit(m_dataHeliosBlu14.I3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:blue:curr14 V: " & m_dataHeliosBlu14.I3.ToString & ", Min: " & limit(2).Current14.Min.ToString & ", Max: " & limit(2).Current14.Max.ToString)
            End If
            If Not limit(2).Voltage14.CheckLimit(m_dataHeliosBlu14.U3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:blue:volt14 V: " & m_dataHeliosBlu14.U3.ToString & ", Min: " & limit(2).Voltage14.Min.ToString & ", Max: " & limit(2).Voltage14.Max.ToString)
            End If
            If Not limit(2).Temperature14.CheckLimit(m_dataHeliosBlu14.T3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:blue:temp14 V: " & m_dataHeliosBlu14.T3.ToString & ", Min: " & limit(2).Temperature14.Min.ToString & ", Max: " & limit(2).Temperature14.Max.ToString)
            End If

            If Not limit(2).Current50.CheckLimit(m_dataHeliosBlu50.I3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:blue:curr50 V: " & m_dataHeliosBlu50.I3.ToString & ", Min: " & limit(2).Current50.Min.ToString & ", Max: " & limit(2).Current50.Max.ToString)
            End If
            If Not limit(2).Voltage50.CheckLimit(m_dataHeliosBlu50.U3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:blue:volt50 V: " & m_dataHeliosBlu50.U3.ToString & ", Min: " & limit(2).Voltage50.Min.ToString & ", Max: " & limit(2).Voltage50.Max.ToString)
            End If
            If Not limit(2).Temperature50.CheckLimit(m_dataHeliosBlu50.T3) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:blue:temp50 V: " & m_dataHeliosBlu50.T3.ToString & ", Min: " & limit(2).Temperature50.Min.ToString & ", Max: " & limit(2).Temperature50.Max.ToString)
            End If

            For Each lim As RefMeasureLimits In limit
                Dim dataCas14 As DataCASMeas = New DataCASMeas
                Dim dataCas50 As DataCASMeas = New DataCASMeas

                Select Case lim.Color
                    Case "red"
                        dataCas14 = m_dataCasMeasRed14
                        dataCas50 = m_dataCasMeasRed50
                    Case "green"
                        dataCas14 = m_dataCasMeasGre14
                        dataCas50 = m_dataCasMeasGre50
                    Case "blue"
                        dataCas14 = m_dataCasMeasBlu14
                        dataCas50 = m_dataCasMeasBlu50
                End Select

                If Not lim.Phi14.CheckLimit(dataCas14.PhotoIntegral) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":phi14 V: " & dataCas14.PhotoIntegral.ToString & ", Min: " & lim.Phi14.Min.ToString & ", Max: " & lim.Phi14.Max.ToString)
                End If
                If Not lim.Phi50.CheckLimit(dataCas50.PhotoIntegral) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":phi50 V: " & dataCas50.PhotoIntegral.ToString & ", Min: " & lim.Phi50.Min.ToString & ", Max: " & lim.Phi50.Max.ToString)
                End If
                If Not lim.Cx14.CheckLimit(dataCas14.Cx) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":cx14 V: " & dataCas14.Cx.ToString & ", Min: " & lim.Cx14.Min.ToString & ", Max: " & lim.Cx14.Max.ToString)
                End If
                If Not lim.Cx50.CheckLimit(dataCas50.Cx) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":cx50 V: " & dataCas50.Cx.ToString & ", Min: " & lim.Cx50.Min.ToString & ", Max: " & lim.Cx50.Max.ToString)
                End If
                If Not lim.Cy14.CheckLimit(dataCas14.Cy) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":cy14 V: " & dataCas14.Cy.ToString & ", Min: " & lim.Cy14.Min.ToString & ", Max: " & lim.Cy14.Max.ToString)
                End If
                If Not lim.Cy50.CheckLimit(dataCas50.Cy) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":cy50 V: " & dataCas50.Cy.ToString & ", Min: " & lim.Cy50.Min.ToString & ", Max: " & lim.Cy50.Max.ToString)
                End If
                If Not lim.LambdaDom14.CheckLimit(dataCas14.LambdaDom) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":lambdaDom14 V: " & dataCas14.LambdaDom.ToString & ", Min: " & lim.LambdaDom14.Min.ToString & ", Max: " & lim.LambdaDom14.Max.ToString)
                End If
                If Not lim.LambdaDom50.CheckLimit(dataCas50.LambdaDom) Then
                    resultGood = False
                    RaiseEvent addLogFile(Me, "##ref:" & lim.Color & ":lambdaDom50 V: " & dataCas50.LambdaDom.ToString & ", Min: " & lim.LambdaDom50.Min.ToString & ", Max: " & lim.LambdaDom50.Max.ToString)
                End If
            Next
            fctGood = resultGood
            If Me.Status = smhStatus.WorkingBad Then
                fctGood = False
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckRefMeasLimits: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfen der Referenzmessung",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCheckRefMeasLimitsV(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        If smData.ModuleType = -2 Then
            'Skip Check for Monthly Reference Measurement

            feedbackStatus(True,
               Me.Status,
               False,
               Me.CurrentStep,
               Me.MaxSteps,
               "Überprüfen der Referenzmessung",
               " übersprungen",
               " fehlgeschlagen")
            Exit Sub
        End If

        Dim fctGood As Boolean = False
        Try
            Dim limit As New RefMeasureLimitsV
            Dim resultGood As Boolean = True

            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet
            Select Case smData.ModuleType
                Case -1 'Referenz Messung / Golden Sample
                    xlsWorkSheet = xlsWorkBook.Sheets("LimitGolden")
                Case Else   'Normale Messung
                    xlsWorkSheet = xlsWorkBook.Sheets("LimitV")
            End Select

            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing

            'load values from Excel File
            currentFind = usedRange.Find("V current", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.Current.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.Current.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("V voltage", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.Voltage.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.Voltage.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("V temperature", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.Temperature.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.Temperature.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("V optical power", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.Popt.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.Popt.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            currentFind = usedRange.Find("V lambdaPeak", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
            limit.lambdaPeak.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
            limit.lambdaPeak.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

            xlsWorkBook.Close()
            xlsApp.Quit()


            ''''''''''''''''''''''''''''''''''''''''''''''
            If Not limit.Current.CheckLimit(m_dataHeliosUV.I4) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:V:curr V: " & m_dataHeliosUV.I4.ToString & ", Min: " & limit.Current.Min.ToString & ", Max: " & limit.Current.Max.ToString)
            End If
            If Not limit.Voltage.CheckLimit(m_dataHeliosUV.U4) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:V:volt V: " & m_dataHeliosUV.U4.ToString & ", Min: " & limit.Voltage.Min.ToString & ", Max: " & limit.Voltage.Max.ToString)
            End If
            If Not limit.Temperature.CheckLimit(m_dataHeliosUV.T4) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:V:temp V: " & m_dataHeliosUV.T4.ToString & ", Min: " & limit.Temperature.Min.ToString & ", Max: " & limit.Temperature.Max.ToString)
            End If

            If Not limit.Popt.CheckLimit(m_dataCasMeasUV.RadIntegral) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:V:Popt V: " & m_dataCasMeasUV.RadIntegral.ToString & ", Min: " & limit.Popt.Min.ToString & ", Max: " & limit.Popt.Max.ToString)
            End If
            If Not limit.lambdaPeak.CheckLimit(m_dataCasMeasUV.LambdaPeak) Then
                resultGood = False
                RaiseEvent addLogFile(Me, "##ref:V:lambdaDom14 V: " & m_dataCasMeasUV.LambdaPeak.ToString & ", Min: " & limit.lambdaPeak.Min.ToString & ", Max: " & limit.lambdaPeak.Max.ToString)
            End If

            fctGood = resultGood
            If Me.Status = smhStatus.WorkingBad Then
                fctGood = False
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckRefMeasLimitsV: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Überprüfen der Referenzmessung",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCheckSafetyLabel(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim textString As String = String.Empty
        Dim retVal As Boolean
        Try
            Dim dioPorts As HardwareCommunication.NI6520_DAQ
            dioPorts = DirectCast(data0, HardwareCommunication.NI6520_DAQ)
            Dim valueText As String
            valueText = DirectCast(data1, String)
            If Not String.IsNullOrEmpty(dioPorts.Device) Then
                If Not dioPorts.getSingleDILine(dioPorts.SafetyBox.Port, dioPorts.SafetyBox.PortPin, retVal) Then
                    textString = " Fehler!! Schritt"
                Else
                    If retVal Then
                        textString = " Label vorhanden"
                        fctGood = True
                    Else
                        textString = " Label NICHT vorhanden!! Schritt"
                        RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " checkSafetyLabel: Not detected, FAIL")
                    End If
                End If

            Else
                textString = " Keine DIO Karte!! Schritt"
            End If

        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCheckSafetyLabel: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           textString,
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepCompareSerialNumber(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim dataHelios As DataHelios
        dataHelios = DirectCast(data0, DataHelios)
        Try
            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet = xlsWorkBook.Sheets("LimitGolden")
            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing

            Dim sn As String
            For i = 1 To 6
                currentFind = usedRange.Find("serialNumber" & i.ToString(), , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                sn = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                If sn = dataHelios.SN.ToString Then
                    fctGood = True
                End If
            Next i
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepCompareSerialNumber: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "korrektes GS eingelegt",
                           "falsches Modul eingelegt")
        End Try
    End Sub

    Public Sub stepDoDACCalib(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim I1low, I2low, I3low As Double
            Dim I1high, I2high, I3high As Double
            Dim I1, I2, I3 As Double
            Dim result As Boolean = True

            If Not can.SetRGbColor(CAN_DEST, 255, 0, 0) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 25) Then result = False
            System.Threading.Thread.Sleep(100)
            If Not can.GetBulbCurrentRGB(CAN_DEST, I1low, I2, I3) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 75) Then result = False
            System.Threading.Thread.Sleep(100)
            If Not can.GetBulbCurrentRGB(CAN_DEST, I1high, I2, I3) Then result = False
            If result Then
                Me.m_calibList.Find(Function(x) (x.Color = "red")).CValue.Dac.Slope = (I1high - I1low) / (2048)
                Me.m_calibList.Find(Function(x) (x.Color = "red")).CValue.Dac.Offset = I1low - (I1high - I1low) / 2
            End If

            If Not can.SetRGbColor(CAN_DEST, 0, 255, 0) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 25) Then result = False
            System.Threading.Thread.Sleep(100)
            If Not can.GetBulbCurrentRGB(CAN_DEST, I1, I2low, I3) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 75) Then result = False
            System.Threading.Thread.Sleep(100)
            If Not can.GetBulbCurrentRGB(CAN_DEST, I1, I2high, I3) Then result = False
            If result Then
                Me.m_calibList.Find(Function(x) (x.Color = "green")).CValue.Dac.Slope = (I2high - I2low) / (2048)
                Me.m_calibList.Find(Function(x) (x.Color = "green")).CValue.Dac.Offset = I2low - (I2high - I2low) / 2
            End If

            If Not can.SetRGbColor(CAN_DEST, 0, 0, 255) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 25) Then result = False
            System.Threading.Thread.Sleep(100)
            If Not can.GetBulbCurrentRGB(CAN_DEST, I1, I2, I3low) Then result = False
            If Not can.SetRgbIntensity(CAN_DEST, 75) Then result = False
            System.Threading.Thread.Sleep(100)
            If Not can.GetBulbCurrentRGB(CAN_DEST, I1, I2, I3high) Then result = False
            If result Then
                Me.m_calibList.Find(Function(x) (x.Color = "blue")).CValue.Dac.Slope = (I3high - I3low) / (2048)
                Me.m_calibList.Find(Function(x) (x.Color = "blue")).CValue.Dac.Offset = I3low - (I3high - I3low) / 2
                m_dataCalib.CalibDacDone = 1
            End If

            If result Then
                If Not can.ExecuteOpenRGB(CAN_DEST) Then result = False
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, 0, 4, Convert.ToSingle(Me.m_calibList.Find(Function(x) (x.Color = "red")).CValue.Dac.Slope)) Then result = False
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, 0, 5, Convert.ToSingle(Me.m_calibList.Find(Function(x) (x.Color = "red")).CValue.Dac.Offset)) Then result = False
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, 1, 4, Convert.ToSingle(Me.m_calibList.Find(Function(x) (x.Color = "green")).CValue.Dac.Slope)) Then result = False
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, 1, 5, Convert.ToSingle(Me.m_calibList.Find(Function(x) (x.Color = "green")).CValue.Dac.Offset)) Then result = False
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, 2, 4, Convert.ToSingle(Me.m_calibList.Find(Function(x) (x.Color = "blue")).CValue.Dac.Slope)) Then result = False
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, 2, 5, Convert.ToSingle(Me.m_calibList.Find(Function(x) (x.Color = "blue")).CValue.Dac.Offset)) Then result = False
                If Not can.ExecuteCloseRGB(CAN_DEST) Then result = False
            End If

            fctGood = result
            If fctGood Then m_dataCalib.CalibDacSaved = 1
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                          Now.Date.Year.ToString("D4") &
                                          Now.Date.Month.ToString("D2") &
                                          Now.Date.Day.ToString("D2") & " " &
                                          Now.Hour.ToString("D2") &
                                          Now.Minute.ToString("D2") &
                                          Now.Second.ToString("D2") & " " &
                                          "Error in function stepDoDACCalib: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "DAC Kalibrierung",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepGenerateVariables(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Me.m_measurementList = New List(Of Measurement)
            Me.m_calibList = New List(Of Calibration)

            Dim loopText0() As String
            Dim loopText1() As String
            data0 = data0.Replace("[", "")
            data0 = data0.Replace("]", "")
            loopText0 = Split(data0, "/")

            data1 = data1.Replace("[", "")
            data1 = data1.Replace("]", "")
            loopText1 = Split(data1, "/")

            For Each str0 As String In loopText0
                m_calibList.Add(New Calibration(str0))
                For Each str1 As String In loopText1
                    m_measurementList.Add(New Measurement(str0, Convert.ToInt32(str1)))
                Next
            Next
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepGenerateVariables: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Variablen erzeugen",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepGetCalibCurVolDacFromHelios(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim test As Single
            Dim color As String = String.Empty
            For i = 0 To 2
                Select Case i
                    Case 0
                        color = "red"
                    Case 1
                        color = "green"
                    Case 2
                        color = "blue"
                End Select
                If can.GetCalibrationDataRgbUv(CAN_DEST, i, 0, test) Then
                    m_calibList.Find(Function(x) (x.Color = color)).CValue.Current.Slope = test
                Else
                    result = False
                End If

                If can.GetCalibrationDataRgbUv(CAN_DEST, i, 1, test) Then
                    m_calibList.Find(Function(x) (x.Color = color)).CValue.Current.Offset = test
                Else
                    result = False
                End If

                If can.GetCalibrationDataRgbUv(CAN_DEST, i, 2, test) Then
                    m_calibList.Find(Function(x) (x.Color = color)).CValue.Voltage.Slope = test
                Else
                    result = False
                End If

                If can.GetCalibrationDataRgbUv(CAN_DEST, i, 3, test) Then
                    m_calibList.Find(Function(x) (x.Color = color)).CValue.Voltage.Offset = test
                Else
                    result = False
                End If

                If can.GetCalibrationDataRgbUv(CAN_DEST, i, 4, test) Then
                    m_calibList.Find(Function(x) (x.Color = color)).CValue.Dac.Slope = test
                Else
                    result = False
                End If

                If can.GetCalibrationDataRgbUv(CAN_DEST, i, 5, test) Then
                    m_calibList.Find(Function(x) (x.Color = color)).CValue.Dac.Offset = test
                Else
                    result = False
                End If

                If can.GetCalibrationDataRgbUv(CAN_DEST, i, 101, test) Then
                    m_calibList.Find(Function(x) (x.Color = color)).CValue.DacMax = Convert.ToInt32(test)
                Else
                    result = False
                End If
            Next
            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepGetCalibCurVolDacFromHelios: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Laden der Strom-, Spannungskalibierung",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepLoadVariablesData(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet = xlsWorkBook.Sheets("Calib")
            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing
            Dim colors() As String = {"red", "green", "blue"}
            For Each color As String In colors
                For i = 0 To STEP_COUNT - 1
                    Dim stepnumber As Integer
                    stepnumber = i + 1
                    Dim text As String
                    text = (color & " " & stepnumber.ToString & " Intensity")
                    currentFind = usedRange.Find(text, , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                    m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepnumber))).MValue.SetIntensity = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                Next
            Next
            xlsWorkBook.Close()
            xlsApp.Quit()
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepLoadVariablesData: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Laden der Variablendaten",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepLoadLimitValues(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim path As String = "Config.xlsx"
            Dim xlsApp As Excel.Application = New Excel.Application
            Dim xlsWorkBook As Excel.Workbook = xlsApp.Workbooks.Open(Application.StartupPath & "\" & path)
            Dim xlsWorkSheet As Excel.Worksheet = xlsWorkBook.Sheets("LimitMred")
            Dim usedRange As Excel.Range = xlsWorkSheet.UsedRange
            Dim currentFind As Excel.Range = Nothing
            Dim colors() As String = {"red", "green", "blue"}

            For Each meas As Measurement In m_measurementList
                Dim text As String
                xlsWorkSheet = xlsWorkBook.Sheets("LimitM" & meas.Color)
                usedRange = xlsWorkSheet.UsedRange
                text = (meas.Color & " " & meas.StepNumber.ToString)
                currentFind = usedRange.Find(text & " curr", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.Current.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.Current.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " volt", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.Voltage.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.Voltage.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " temp", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.Temperature.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.Temperature.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " PD", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.PD.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.PD.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cx", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.Cx.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.Cx.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cy", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.Cy.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.Cy.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.Phi.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.Phi.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " casADC", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.CAS_ADC.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.CAS_ADC.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " filt", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.Filter.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                currentFind = usedRange.Find(text & " avgCoun", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.AverageCount.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                currentFind = usedRange.Find(text & " intTime", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.IntegrationTime.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.IntegrationTime.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cxTempComp", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.CxTempComp.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.CxTempComp.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cyTempComp", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.CyTempComp.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.CyTempComp.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiTempComp", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.PhiTempComp.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.PhiTempComp.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                meas.MLimit.PhiTempComp.Factor = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 3).value
                currentFind = usedRange.Find(text & " setIntensity", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                meas.MLimit.SetIntensity.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                meas.MLimit.SetIntensity.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            Next

            For Each calib As Calibration In m_calibList
                Dim text As String
                xlsWorkSheet = xlsWorkBook.Sheets("LimitC" & calib.Color)
                usedRange = xlsWorkSheet.UsedRange
                text = calib.Color
                currentFind = usedRange.Find(text & " current slope", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Current.Slope.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Current.Slope.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " current offset", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Current.Offset.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Current.Offset.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " voltage slope", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Voltage.Slope.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Voltage.Slope.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " voltage offset", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Voltage.Offset.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Voltage.Offset.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " dac slope", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Dac.Slope.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Dac.Slope.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " dac offset", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Dac.Offset.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Dac.Offset.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " cx I low", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cx.I_low.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cx.I_low.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cx I high", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cx.I_high.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cx.I_high.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cx T low", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cx.T_low.Fixed.F_x0 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cx.T_low.Fixed.F_x1 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                calib.CLimit.Cx.T_low.Fixed.F_x2 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 3).value
                calib.CLimit.Cx.T_low.Fixed.F_x3 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 4).value
                currentFind = usedRange.Find(text & " cx T high", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cx.T_high.Fixed.F_x0 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cx.T_high.Fixed.F_x1 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                calib.CLimit.Cx.T_high.Fixed.F_x2 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 3).value
                calib.CLimit.Cx.T_high.Fixed.F_x3 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 4).value
                currentFind = usedRange.Find(text & " cx I border", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cx.I_Border.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                currentFind = usedRange.Find(text & " cx T border", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cx.T_Border.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value

                currentFind = usedRange.Find(text & " cy I low", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cy.I_low.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cy.I_low.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cy I high", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cy.I_high.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cy.I_high.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " cy T low", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cy.T_low.Fixed.F_x0 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cy.T_low.Fixed.F_x1 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                calib.CLimit.Cy.T_low.Fixed.F_x2 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 3).value
                calib.CLimit.Cy.T_low.Fixed.F_x3 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 4).value
                currentFind = usedRange.Find(text & " cy T high", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cy.T_high.Fixed.F_x0 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Cy.T_high.Fixed.F_x1 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                calib.CLimit.Cy.T_high.Fixed.F_x2 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 3).value
                calib.CLimit.Cy.T_high.Fixed.F_x3 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 4).value
                currentFind = usedRange.Find(text & " cy I border", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cy.I_Border.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                currentFind = usedRange.Find(text & " cy T border", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Cy.T_Border.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value


                currentFind = usedRange.Find(text & " tempChip rth", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.TempChip.Rth.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                currentFind = usedRange.Find(text & " tempChip eta", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.TempChip.Eta.Fixed = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value

                currentFind = usedRange.Find(text & " phi T", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.T.Fixed.F_x0 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.T.Fixed.F_x1 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                calib.CLimit.Phi.T.Fixed.F_x2 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 3).value
                calib.CLimit.Phi.T.Fixed.F_x3 = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 4).value
                currentFind = usedRange.Find(text & " phi I spl1", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.I_spl1.MinDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.I_spl1.MaxDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I spl2", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.I_spl2.MinDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.I_spl2.MaxDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I spl3", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.I_spl3.MinDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.I_spl3.MaxDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I spl4", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.I_spl4.MinDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.I_spl4.MaxDelta = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I last", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.I_last.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.I_last.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I border 1", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.Border_I_1.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.Border_I_1.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I border 2", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.Border_I_2.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.Border_I_2.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I border 3", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.Border_I_3.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.Border_I_3.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phi I border 4", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.Phi.Border_I_4.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.Phi.Border_I_4.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " phiAdc T", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.T.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.T.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc I", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.I.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.I.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc adc1", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.Adc_spl1.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.Adc_spl1.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc adc2", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.Adc_spl2.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.Adc_spl2.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc adc3", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.Adc_spl3.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.Adc_spl3.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc adc last", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.Adc_last.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.Adc_last.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc adc border 1", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.Border_Adc_1.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.Border_Adc_1.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc adc border 2", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.Border_Adc_2.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.Border_Adc_2.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " phiAdc adc border 3", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiADC.Border_Adc_3.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiADC.Border_Adc_3.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " phiMax", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.PhiMax.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.PhiMax.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " dacMax", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.DacMax.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.DacMax.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " iByDac fct", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.IByDac.Fct.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.IByDac.Fct.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " uByI fct", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.UByI.Fct.DevBottomPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.UByI.Fct.DevTopPercent = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value

                currentFind = usedRange.Find(text & " superR9 cx", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.SuperR9.Cx.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.SuperR9.Cx.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
                currentFind = usedRange.Find(text & " superR9 cy", , Excel.XlFindLookIn.xlValues, Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlNext, False)
                calib.CLimit.SuperR9.Cy.Min = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 1).value
                calib.CLimit.SuperR9.Cy.Max = xlsWorkSheet.Cells(currentFind.Row, currentFind.Column + 2).value
            Next


            xlsWorkBook.Close()
            xlsApp.Quit()
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepLoadLimitValues: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Laden der Grenzwerte",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepMoveLightGuide(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim direction As String = String.Empty
        Try
            direction = DirectCast(data0, String)
            Dim myProtocol As HardwareCommunication.ProtocolEOLBox = New HardwareCommunication.ProtocolEOLBox
            Dim result As String = String.Empty
            Dim buffer(20) As Char
            Dim iniReader As IniReader = New IniReader
            Dim comPort As String
            comPort = iniReader.ReadValueFromFile("COM", "port", "", ".\Settings.ini")
            myProtocol.Open(comPort)
            Select Case direction
                Case "in"
                    If myProtocol.SetLWLPos("in", buffer) Then
                        fctGood = True
                    End If
                Case "out"
                    If myProtocol.SetLWLPos("out", buffer) Then
                        fctGood = True
                    End If
                Case Else
            End Select
            Dim test As String = New String(buffer)
            Select Case test.Substring(0, 10)
                Case ("ERR: Colli")
                    direction = "Kollision"
                Case Else
                    If test.Substring(0, 3) = "Ack" Then
                        fctGood = True
                    Else
                        direction = "unbekannter Fehler"
                    End If
            End Select
            myProtocol.Close()
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepMoveLightGuide: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "fahre Lichtleiter " + direction,
                           "",
                           " !!")
        End Try
    End Sub

    Public Sub stepReadCasForEstimate(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)

        Dim fctGood As Boolean = False
        Try
            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data0, CasCommunication.cCAS140)
            Dim color As String
            color = DirectCast(data1, String)
            Dim time As Integer
            time = DirectCast(GetVariableInstance(DirectCast(data2, String)), Integer)
            Dim intTime As Integer
            intTime = 200
            Dim counts As Integer
            counts = 26000
            cas.FilterPosition = 2
            Do
                intTime = intTime * 26000 / counts
                Select Case color
                    Case "red"
                        cas.IntegrationTime = intTime
                    Case "green"
                        cas.IntegrationTime = intTime
                    Case "blue"
                        cas.IntegrationTime = intTime
                    Case Else

                End Select
                If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
                counts = cas.MaxCounts
            Loop While ((counts < 25500) Or (counts > 26500))
            m_integrationTime = intTime
            RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Read CAS for Estimate: " & color & " intTime=" & intTime.ToString)
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepReadCasForEstimate: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "schätze Integrationszeiten ab",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepReadCas(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            'only first object is used as an dataCASMeas variable
            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data0, CasCommunication.cCAS140)
            Dim dataCas As DataCASMeas
            dataCas = DirectCast(data1, DataCASMeas)

            'write Status to statusText
            'statusText.Add(CurrentStep.ToString & "/" & MaxSteps.ToString & " Read Cas Values")

            'Auto Range Measurement
            cas.AutoRangeMeasurement = True
            'use laways filter position 1
            cas.FilterPosition = 2
            'start measurement
            If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
            'copy data to variable
            dataCas.PhotoIntegral = cas.Flux
            dataCas.RadIntegral = cas.Power
            dataCas.Cx = cas.Cx
            dataCas.Cy = cas.Cy
            dataCas.LambdaPeak = cas.LambdaPeak
            dataCas.LambdaDom = cas.LambdaDom
            dataCas.CRI = cas.GetCRI
            dataCas.R9 = cas.GetR9
            dataCas.R13 = cas.GetR13
            dataCas.R15 = cas.GetR15
            dataCas.CCT = cas.GetCCT
            cas.ReadSpectrum(dataCas.Spectrum)
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepReadCas: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Messwerte von Spektrometer lesen",
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepReadCasRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim color As String = String.Empty
        Dim stepNumber As Integer = 0
        Try
            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data0, CasCommunication.cCAS140)
            color = DirectCast(data1, String)
            stepNumber = Convert.ToInt32(DirectCast(data2, String))
            Dim index As Integer
            index = m_measurementList.FindIndex(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber)))

            cas.AutoRangeMeasurement = False
            cas.FilterPosition = 2
            cas.Averages = 2
            cas.IntegrationTime = m_measurementList.Item(index).MValue.IntegrationTime
            If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
            If cas.Cx > 1 Or cas.Cx < 0 Then
                m_measurementList.Item(index).MValue.Cx = 0
            Else
                m_measurementList.Item(index).MValue.Cx = cas.Cx
            End If
            If cas.Cy > 1 Or cas.Cy < 0 Then
                m_measurementList.Item(index).MValue.Cy = 0
            Else
                m_measurementList.Item(index).MValue.Cy = cas.Cy
            End If
            m_measurementList.Item(index).MValue.Phi = cas.Flux
            m_measurementList.Item(index).MValue.CAS_ADC = cas.MaxCounts
            m_measurementList.Item(index).MValue.Filter = cas.FilterPosition
            m_measurementList.Item(index).MValue.AverageCount = cas.Averages

            RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Read Cas " & color & " " & stepNumber.ToString())
            RaiseEvent addLogFile(Me, "          ColorPoint: " & m_measurementList.Item(index).MValue.Cx.ToString("F4") & "/" & m_measurementList.Item(index).MValue.Cy.ToString("F4"))
            RaiseEvent addLogFile(Me, "          Flux: " & cas.Flux.ToString("F2") & " lumen")
            RaiseEvent addLogFile(Me, "          Counts: " & cas.MaxCounts.ToString("F0") & " counts")
            RaiseEvent addLogFile(Me, "          Filter/Average: " & cas.FilterPosition.ToString("F0") & "/" & cas.Averages.ToString("F0"))
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & "_" &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & "_" &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & "_" &
                                      "Error in function stepReadCasRef: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Read Cas " & color & " " & stepNumber.ToString(),
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepReadHeliosRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim color As String = String.Empty
        Dim stepNumber As Integer = 0
        Try
            Dim result As Boolean = True
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = GetVariableInstance(DirectCast(data0, String))
            color = DirectCast(data1, String)
            stepNumber = Convert.ToInt32(DirectCast(data2, String))
            Dim index As Integer
            index = m_measurementList.FindIndex(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber)))

            Dim I1, I2, I3 As Double
            Dim U1, U2, U3 As Double
            Dim T1, T2, T3, T6 As Double
            Dim PD1, PD2, PD3, PD4, PD5 As UShort


            If Not can.GetBulbCurrentRGB(CAN_DEST, I1, I2, I3) Then result = False
            If Not can.GetBulbVoltageRGB(CAN_DEST, U1, U2, U3) Then result = False
            If Not can.GetPDRGB(CAN_DEST, PD1, PD2, PD3) Then result = False
            If result Then
                Select Case color
                    Case "red"
                        m_measurementList.Item(index).MValue.Current = I1
                        m_measurementList.Item(index).MValue.Voltage = U1
                        If Not can.GetTemperature(CAN_DEST, 0, T1) Then
                            result = False
                        Else
                            m_measurementList.Item(index).MValue.Temperature = T1
                        End If
                    Case "green"
                        m_measurementList.Item(index).MValue.Current = I2
                        m_measurementList.Item(index).MValue.Voltage = U2
                        If Not can.GetTemperature(CAN_DEST, 1, T2) Then
                            result = False
                        Else
                            m_measurementList.Item(index).MValue.Temperature = T2
                        End If
                    Case "blue"
                        m_measurementList.Item(index).MValue.Current = I3
                        m_measurementList.Item(index).MValue.Voltage = U3
                        If Not can.GetTemperature(CAN_DEST, 2, T3) Then
                            result = False
                        Else
                            m_measurementList.Item(index).MValue.Temperature = T3
                        End If
                    Case Else
                End Select
                If Not can.GetTemperature(CAN_DEST, 5, T6) Then
                    result = False
                Else
                    m_measurementList.Item(index).MValue.TempExtern = T6
                End If
                If Not can.GetPDUVSum(CAN_DEST, PD4, PD5) Then
                    result = False
                Else
                    m_measurementList.Item(index).MValue.PD = PD5
                End If
            End If
            RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Read Helios " & color & " " & stepNumber.ToString() & ":")
            RaiseEvent addLogFile(Me, "         I:  " & I1.ToString("F3") & " " & I2.ToString("F3") & " " & I3.ToString("F3"))
            RaiseEvent addLogFile(Me, "         U:  " & U1.ToString("F2") & " " & U2.ToString("F2") & " " & U3.ToString("F2"))
            RaiseEvent addLogFile(Me, "         T:  " & T1.ToString("F1") & " " & T2.ToString("F1") & " " & T3.ToString("F1"))
            RaiseEvent addLogFile(Me, "         PD: " & PD1.ToString("F0") & " " & PD2.ToString("F0") & " " & PD3.ToString("F0") & " " & PD5.ToString("F0"))
            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepReadHeliosRef: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Werte von Helios lesen " & color & " " & stepNumber.ToString(),
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepReadHeliosValues(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim data As DataHelios
            data = DirectCast(data1, DataHelios)
            'write Status to statusText
            'statusText.Add(CurrentStep.ToString & "/" & MaxSteps.ToString & " Read Helios Values")
            'Application.DoEvents()
            If Not can.GetBulbVoltageRGB(CAN_DEST, data.U1, data.U2, data.U3) Then result = False
            If Not can.GetBulbCurrentRGB(CAN_DEST, data.I1, data.I2, data.I3) Then result = False
            If Not can.GetTemperature(CAN_DEST, 0, data.T1) Then result = False
            If Not can.GetTemperature(CAN_DEST, 1, data.T2) Then result = False
            If Not can.GetTemperature(CAN_DEST, 2, data.T3) Then result = False
            If Not can.GetTemperature(CAN_DEST, 3, data.T4) Then result = False
            If Not can.GetTemperature(CAN_DEST, 4, data.T5) Then result = False
            If Not can.GetTemperature(CAN_DEST, 5, data.T6) Then result = False
            If Not can.GetBulbVoltageUV(CAN_DEST, data.U4) Then result = False
            If Not can.GetBulbCurrentUV(CAN_DEST, data.I4) Then result = False
            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function  stepReadHeliosValues: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Werte von Helios lesen",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepReadPS(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim voltage As Double = 0
        Dim current As Double = 0
        Dim voltageString As String = Nothing
        Dim currentString As String = Nothing
        Try
            Dim ps As HardwareCommunication.TcpKeysightN5767Communication
            ps = DirectCast(data0, HardwareCommunication.TcpKeysightN5767Communication)
            Dim dataPS As DataPS
            dataPS = DirectCast(data1, DataPS)

            If ps.measVoltage(voltageString) = True Then
                voltage = Convert.ToDouble(voltageString, System.Globalization.CultureInfo.InvariantCulture)
                dataPS.Voltage = voltage
            Else
                Throw New Exception("ps.measVoltage failed")
            End If

            If ps.measCurrent(currentString) = True Then
                current = Convert.ToDouble(currentString, System.Globalization.CultureInfo.InvariantCulture)
                dataPS.Current = current
            Else
                Throw New Exception("ps.measCurrent failed")
            End If

            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepReadPS: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Werte von PS lesen",
                           String.Format(": {0:0.0} V, {1:0.000} A", voltage, current),
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepReadSNandLifeTime(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim result As Boolean = True
            Dim m_oHeliosCommunicationBoard As HELIOSCommunication.HELIOSCommunication
            m_oHeliosCommunicationBoard = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim data As DataHelios
            data = DirectCast(data1, DataHelios)
            Dim main_rel, sub_rel, build, state As UShort

            If Not m_oHeliosCommunicationBoard.GetZeissVersionAndSN(CAN_DEST, data.SN, main_rel, sub_rel) Then result = False
            If Not m_oHeliosCommunicationBoard.GetBurnTime(CAN_DEST, data.BurnTime) Then result = False
            If Not m_oHeliosCommunicationBoard.GetSoftwareVersion(CAN_DEST, build, sub_rel, main_rel, state) Then result = False
            data.Version_main = main_rel
            data.Version_sub = sub_rel
            data.Version_build = build
            data.Version_state = state

            fctGood = result
        Catch ex As Exception
            '            RaiseEvent addLogFile(Me, "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%")
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function  stepReadSNandLifeTime: " & ex.Message & vbNewLine & ex.StackTrace)
            '            RaiseEvent addLogFile(Me, "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%")
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "SN und Lebensdauer von Helios lesen",
                           "",
                           " fehlgeschlagen")
        End Try




    End Sub

    Public Sub stepReadSNandLifeTimeUV(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim m_oHeliosCommunicationBoard As HELIOSCommunication.HELIOSCommunication
            m_oHeliosCommunicationBoard = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim data As DataHelios
            data = DirectCast(data1, DataHelios)
            Dim main_rel, sub_rel, build, state As UShort
            Dim result As Boolean
            result = True
            Threading.Thread.Sleep(500)
            If Not m_oHeliosCommunicationBoard.GetZeissVersionAndSN(CAN_DEST, data.SN, main_rel, sub_rel) Then result = False
            If Not m_oHeliosCommunicationBoard.GetSerialNumber(CAN_DEST, 0, 1, 2, data.SnUV) Then result = False
            If Not m_oHeliosCommunicationBoard.GetBurnTimeUV(CAN_DEST, data.BurnTime) Then result = False
            If Not m_oHeliosCommunicationBoard.GetSoftwareVersion(CAN_DEST, build, sub_rel, main_rel, state) Then result = False
            data.Version_main = main_rel
            data.Version_sub = sub_rel
            data.Version_build = build
            data.Version_state = state

            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function  stepReadSNandLifeTime: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "SN und Lebensdauer von Helios lesen",
                           "",
                           " fehlgeschlagen")
        End Try




    End Sub

    Public Sub stepResetDailyCalib(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)

            If Not can.ExecuteOpenDailyCalib(CAN_DEST) Then
                result = False
            Else
                If Not can.SetInitDailyCalibration(CAN_DEST) Then result = False
                If Not can.ExecuteCloseDailyCalib(CAN_DEST) Then result = False
            End If

            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function  stepResetDailyCalib: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Rücksetzen der Daily Calibration",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepRestartHelios(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            'write Status to statusText
            'RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Restart Helios Values")
            can.ExecuteRestart(CAN_DEST)    ' Liefert keine Antwort, daher auch FALSE wenn erfolgreich
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function  stepRestartHelios: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Helios Neustarten",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Private Sub stepSaveCalibValuesToFile(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim data As DataHelios
        data = DirectCast(data0, DataHelios)
        Dim outFile As IO.StreamWriter
        Dim path, filename As String
        Try
            Dim color As String = String.Empty
            Dim iniReader As IniReader = New IniReader
            path = iniReader.ReadValueFromFile("DataStorage", "path", "", ".\Settings.ini") & "\calib\"
            filename = "cal_SN" + data.SN.ToString("000000000") + "_" + _
                Now.Year.ToString("0000") + Now.Month.ToString("00") + Now.Day.ToString("00") + "_" + _
                Now.Hour.ToString("00") + Now.Minute.ToString("00") + Now.Second.ToString("00") + _
                ".csv"
            m_dataCalib.CSVpath = path
            m_dataCalib.CSVfilename = filename

            outFile = My.Computer.FileSystem.OpenTextFileWriter(path + filename, False)
            Dim index As Integer
            Dim calibString(112) As String
            For i = 0 To 111
                calibString(i) = String.Empty
            Next
            For i = 0 To 2
                Select Case i
                    Case 0
                        index = m_calibList.FindIndex(Function(x) (x.Color = "red"))
                    Case 1
                        index = m_calibList.FindIndex(Function(x) (x.Color = "green"))
                    Case 2
                        index = m_calibList.FindIndex(Function(x) (x.Color = "blue"))
                End Select
                calibString(0) += m_calibList(index).CValue.Current.Slope.ToString("") + ";"
                calibString(1) += m_calibList(index).CValue.Current.Offset.ToString("") + ";"
                calibString(2) += m_calibList(index).CValue.Voltage.Slope.ToString("") + ";"
                calibString(3) += m_calibList(index).CValue.Voltage.Offset.ToString("") + ";"
                calibString(4) += m_calibList(index).CValue.Dac.Slope.ToString("") + ";"
                calibString(5) += m_calibList(index).CValue.Dac.Offset.ToString("") + ";"

                calibString(6) += m_calibList(index).CValue.Cx.I_low.F_x3.ToString("") + ";"
                calibString(7) += m_calibList(index).CValue.Cx.I_low.F_x2.ToString("") + ";"
                calibString(8) += m_calibList(index).CValue.Cx.I_low.F_x1.ToString("") + ";"
                calibString(9) += m_calibList(index).CValue.Cx.I_low.F_x0.ToString("") + ";"
                calibString(10) += m_calibList(index).CValue.Cx.T_low.F_x3.ToString("") + ";"
                calibString(11) += m_calibList(index).CValue.Cx.T_low.F_x2.ToString("") + ";"
                calibString(12) += m_calibList(index).CValue.Cx.T_low.F_x1.ToString("") + ";"
                calibString(13) += m_calibList(index).CValue.Cx.T_low.F_x0.ToString("") + ";"
                calibString(14) += m_calibList(index).CValue.Cx.I_high.F_x3.ToString("") + ";"
                calibString(15) += m_calibList(index).CValue.Cx.I_high.F_x2.ToString("") + ";"
                calibString(16) += m_calibList(index).CValue.Cx.I_high.F_x1.ToString("") + ";"
                calibString(17) += m_calibList(index).CValue.Cx.I_high.F_x0.ToString("") + ";"
                calibString(18) += m_calibList(index).CValue.Cx.T_high.F_x3.ToString("") + ";"
                calibString(19) += m_calibList(index).CValue.Cx.T_high.F_x2.ToString("") + ";"
                calibString(20) += m_calibList(index).CValue.Cx.T_high.F_x1.ToString("") + ";"
                calibString(21) += m_calibList(index).CValue.Cx.T_high.F_x0.ToString("") + ";"
                calibString(22) += m_calibList(index).CValue.Cx.I_border.ToString("") + ";"
                calibString(23) += m_calibList(index).CValue.Cx.T_border.ToString("") + ";"

                calibString(24) += m_calibList(index).CValue.Cy.I_low.F_x3.ToString("") + ";"
                calibString(25) += m_calibList(index).CValue.Cy.I_low.F_x2.ToString("") + ";"
                calibString(26) += m_calibList(index).CValue.Cy.I_low.F_x1.ToString("") + ";"
                calibString(27) += m_calibList(index).CValue.Cy.I_low.F_x0.ToString("") + ";"
                calibString(28) += m_calibList(index).CValue.Cy.T_low.F_x3.ToString("") + ";"
                calibString(29) += m_calibList(index).CValue.Cy.T_low.F_x2.ToString("") + ";"
                calibString(30) += m_calibList(index).CValue.Cy.T_low.F_x1.ToString("") + ";"
                calibString(31) += m_calibList(index).CValue.Cy.T_low.F_x0.ToString("") + ";"
                calibString(32) += m_calibList(index).CValue.Cy.I_high.F_x3.ToString("") + ";"
                calibString(33) += m_calibList(index).CValue.Cy.I_high.F_x2.ToString("") + ";"
                calibString(34) += m_calibList(index).CValue.Cy.I_high.F_x1.ToString("") + ";"
                calibString(35) += m_calibList(index).CValue.Cy.I_high.F_x0.ToString("") + ";"
                calibString(36) += m_calibList(index).CValue.Cy.T_high.F_x3.ToString("") + ";"
                calibString(37) += m_calibList(index).CValue.Cy.T_high.F_x2.ToString("") + ";"
                calibString(38) += m_calibList(index).CValue.Cy.T_high.F_x1.ToString("") + ";"
                calibString(39) += m_calibList(index).CValue.Cy.T_high.F_x0.ToString("") + ";"
                calibString(40) += m_calibList(index).CValue.Cy.I_border.ToString("") + ";"
                calibString(41) += m_calibList(index).CValue.Cy.T_border.ToString("") + ";"

                calibString(42) += m_calibList(index).CValue.TempChip.Rth.ToString("") + ";"
                calibString(43) += m_calibList(index).CValue.TempChip.Eta.ToString("") + ";"

                calibString(44) += m_calibList(index).CValue.Phi.T.F_x3.ToString("") + ";"
                calibString(45) += m_calibList(index).CValue.Phi.T.F_x2.ToString("") + ";"
                calibString(46) += m_calibList(index).CValue.Phi.T.F_x1.ToString("") + ";"
                calibString(47) += m_calibList(index).CValue.Phi.T.F_x0.ToString("") + ";"
                calibString(48) += m_calibList(index).CValue.Phi.I_spl1.F_x3.ToString("") + ";"
                calibString(49) += m_calibList(index).CValue.Phi.I_spl1.F_x2.ToString("") + ";"
                calibString(50) += m_calibList(index).CValue.Phi.I_spl1.F_x1.ToString("") + ";"
                calibString(51) += m_calibList(index).CValue.Phi.I_spl1.F_x0.ToString("") + ";"
                calibString(52) += m_calibList(index).CValue.Phi.I_spl2.F_x3.ToString("") + ";"
                calibString(53) += m_calibList(index).CValue.Phi.I_spl2.F_x2.ToString("") + ";"
                calibString(54) += m_calibList(index).CValue.Phi.I_spl2.F_x1.ToString("") + ";"
                calibString(55) += m_calibList(index).CValue.Phi.I_spl2.F_x0.ToString("") + ";"
                calibString(56) += m_calibList(index).CValue.Phi.I_spl3.F_x3.ToString("") + ";"
                calibString(57) += m_calibList(index).CValue.Phi.I_spl3.F_x2.ToString("") + ";"
                calibString(58) += m_calibList(index).CValue.Phi.I_spl3.F_x1.ToString("") + ";"
                calibString(59) += m_calibList(index).CValue.Phi.I_spl3.F_x0.ToString("") + ";"
                calibString(60) += m_calibList(index).CValue.Phi.I_spl4.F_x3.ToString("") + ";"
                calibString(61) += m_calibList(index).CValue.Phi.I_spl4.F_x2.ToString("") + ";"
                calibString(62) += m_calibList(index).CValue.Phi.I_spl4.F_x1.ToString("") + ";"
                calibString(63) += m_calibList(index).CValue.Phi.I_spl4.F_x0.ToString("") + ";"
                calibString(64) += m_calibList(index).CValue.Phi.I_last.F_x3.ToString("") + ";"
                calibString(65) += m_calibList(index).CValue.Phi.I_last.F_x2.ToString("") + ";"
                calibString(66) += m_calibList(index).CValue.Phi.I_last.F_x1.ToString("") + ";"
                calibString(67) += m_calibList(index).CValue.Phi.I_last.F_x0.ToString("") + ";"
                calibString(68) += m_calibList(index).CValue.Phi.Border_I_1.ToString("") + ";"
                calibString(69) += m_calibList(index).CValue.Phi.Border_I_2.ToString("") + ";"
                calibString(70) += m_calibList(index).CValue.Phi.Border_I_3.ToString("") + ";"
                calibString(71) += m_calibList(index).CValue.Phi.Border_I_4.ToString("") + ";"

                calibString(72) += m_calibList(index).CValue.PhiADC.Factor1.F_x3.ToString("") + ";"
                calibString(73) += m_calibList(index).CValue.PhiADC.Factor1.F_x2.ToString("") + ";"
                calibString(74) += m_calibList(index).CValue.PhiADC.Factor1.F_x1.ToString("") + ";"
                calibString(75) += m_calibList(index).CValue.PhiADC.Factor1.F_x0.ToString("") + ";"
                calibString(76) += m_calibList(index).CValue.PhiADC.Factor2.F_x3.ToString("") + ";"
                calibString(77) += m_calibList(index).CValue.PhiADC.Factor2.F_x2.ToString("") + ";"
                calibString(78) += m_calibList(index).CValue.PhiADC.Factor2.F_x1.ToString("") + ";"
                calibString(79) += m_calibList(index).CValue.PhiADC.Factor2.F_x0.ToString("") + ";"
                calibString(80) += m_calibList(index).CValue.PhiADC.PDLambdaDom.F_x3.ToString("") + ";"
                calibString(81) += m_calibList(index).CValue.PhiADC.PDLambdaDom.F_x2.ToString("") + ";"
                calibString(82) += m_calibList(index).CValue.PhiADC.PDLambdaDom.F_x1.ToString("") + ";"
                calibString(83) += m_calibList(index).CValue.PhiADC.PDLambdaDom.F_x0.ToString("") + ";"
                calibString(84) += m_calibList(index).CValue.PhiADC.PDTemp.F_x3.ToString("") + ";"
                calibString(85) += m_calibList(index).CValue.PhiADC.PDTemp.F_x2.ToString("") + ";"
                calibString(86) += m_calibList(index).CValue.PhiADC.PDTemp.F_x1.ToString("") + ";"
                calibString(87) += m_calibList(index).CValue.PhiADC.PDTemp.F_x0.ToString("") + ";"
                calibString(88) += m_calibList(index).CValue.PhiADC.Offset_PD.ToString("") + ";"
                calibString(89) += m_calibList(index).CValue.PhiADC.Dummy0.ToString("") + ";"
                calibString(90) += m_calibList(index).CValue.Additional.Cx5500K.ToString("") + ";"
                calibString(91) += m_calibList(index).CValue.Additional.Cy5500K.ToString("") + ";"
                calibString(92) += m_calibList(index).CValue.Additional.Dummy02.ToString("") + ";"
                calibString(93) += m_calibList(index).CValue.Additional.Dummy03.ToString("") + ";"
                calibString(94) += m_calibList(index).CValue.Additional.Dummy10.ToString("") + ";"
                calibString(95) += m_calibList(index).CValue.Additional.Dummy11.ToString("") + ";"
                calibString(96) += m_calibList(index).CValue.Additional.Dummy12.ToString("") + ";"
                calibString(97) += m_calibList(index).CValue.Additional.Dummy13.ToString("") + ";"
                calibString(98) += m_calibList(index).CValue.Additional.Dummy20.ToString("") + ";"
                calibString(99) += m_calibList(index).CValue.Additional.Dummy21.ToString("") + ";"

                calibString(100) += m_calibList(index).CValue.PhiMax.ToString("") + ";"
                calibString(101) += m_calibList(index).CValue.DacMax.ToString("") + ";"

                calibString(102) += m_calibList(index).CValue.IByDac.Fct.F_x3.ToString("") + ";"
                calibString(103) += m_calibList(index).CValue.IByDac.Fct.F_x2.ToString("") + ";"
                calibString(104) += m_calibList(index).CValue.IByDac.Fct.F_x1.ToString("") + ";"
                calibString(105) += m_calibList(index).CValue.IByDac.Fct.F_x0.ToString("") + ";"

                calibString(106) += m_calibList(index).CValue.UByI.Fct.F_x3.ToString("") + ";"
                calibString(107) += m_calibList(index).CValue.UByI.Fct.F_x2.ToString("") + ";"
                calibString(108) += m_calibList(index).CValue.UByI.Fct.F_x1.ToString("") + ";"
                calibString(109) += m_calibList(index).CValue.UByI.Fct.F_x0.ToString("") + ";"

                calibString(110) += m_calibList(index).CValue.SuperR9.Cx.ToString("") + ";"
                calibString(111) += m_calibList(index).CValue.SuperR9.Cy.ToString("") + ";"
            Next i
            For i = 0 To 111
                outFile.WriteLine(calibString(i))
            Next i
            outFile.Close()
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSaveCalibValuesToFile: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Speichern der Kalibrierwerte",
                           "",
                           "fehlgeschlagen")
        End Try
    End Sub

    Private Sub stepSaveMeasurementToFile(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim data As DataHelios
        data = DirectCast(data0, DataHelios)
        Dim outFile As IO.StreamWriter
        Try
            Dim color As String = String.Empty
            Dim iniReader As IniReader = New IniReader
            Dim csvFile As String = iniReader.ReadValueFromFile("DataStorage", "path", "", ".\Settings.ini") & "\measurement" + _
                "\mea_SN" + data.SN.ToString("000000000") + "_" + _
                Now.Year.ToString("0000") + Now.Month.ToString("00") + Now.Day.ToString("00") + "_" + _
                Now.Hour.ToString("00") + Now.Minute.ToString("00") + Now.Second.ToString("00") + _
                ".csv"
            outFile = My.Computer.FileSystem.OpenTextFileWriter(csvFile, False)
            For i = 0 To 2
                Select Case i
                    Case 0
                        color = "red"
                    Case 1
                        color = "green"
                    Case 2
                        color = "blue"
                End Select
                For j = 1 To STEP_COUNT
                    Dim textString As String = String.Empty
                    Dim stepNumber As Integer
                    stepNumber = j
                    textString = _
                        color + ";" + _
                        j.ToString("F0") + ";" +
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.SetIntensity.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.SetIntensity.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntensityDAC.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntensityDAC.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.PhiTempComp.ToString("F2") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.CxTempComp.ToString("F4") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.CyTempComp.ToString("F4") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Current.ToString("F3") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Voltage.ToString("F2") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntegrationTime.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.AverageCount.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.CAS_ADC.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Filter.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Temperature.ToString("F2") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.PD.ToString("F0") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Phi.ToString("F2") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Cx.ToString("F4") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Cy.ToString("F4") + ";" + _
                        m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.Tj.ToString("F2") + ";"
                    outFile.WriteLine(textString)
                Next j
            Next i
            outFile.Close()
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & "" &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSaveMeasurementToFile: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Speichern der Messwerte",
                           "",
                           "fehlgeschlagen")
        End Try
    End Sub

    Private Sub stepSaveFinalTestDataRGBToFile(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = True
        Try

            Dim filename As String
            filename = DirectCast(data0, String)

            Dim xlsApp As Excel.Application
            Dim xlsWorkBook As Excel.Workbook
            Dim xlsWorksheet As Excel.Worksheet

            Dim row As Integer
            'open excel
            xlsApp = New Excel.Application
            Dim iniReader As IniReader = New IniReader
            xlsWorkBook = xlsApp.Workbooks.Open(iniReader.ReadValueFromFile("DataStorage", "path", "", ".\Settings.ini") & filename)
            xlsWorksheet = xlsWorkBook.Worksheets("data")

            'search for new line to inserted

            xlsWorksheet.Cells(100000, 2).End(Excel.XlDirection.xlUp).Offset(1, 0).Select()
            Dim bereich As Excel.Range
            bereich = xlsApp.ActiveCell()
            row = bereich.Row
            'add date, clock
            Dim i As Integer
            i = 1
            xlsWorksheet.Cells(row, i).value = ""
            i += 1
            xlsWorksheet.Cells(row, i).value = Now.Date.ToShortDateString()
            i += 1
            xlsWorksheet.Cells(row, i).value = Now.ToLongTimeString()
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios.SN.ToString("000000")
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios.BurnTime.ToString("000000")
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios.Version_main & "." & m_dataHelios.Version_sub & "." & m_dataHelios.Version_build
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataPSSby.Voltage.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataPSSby.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1

            'add data
            'Red 1.4A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed14.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed14.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed14.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasRed14.FilenameSpectra)
            i += 1
            'Red 5.0A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed50.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed50.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed50.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasRed50.FilenameSpectra)
            i += 1
            'Green 1.4A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre14.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre14.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre14.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasGre14.FilenameSpectra)
            i += 1
            'Green 5.0A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre50.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre50.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre50.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasGre50.FilenameSpectra)
            i += 1
            'Blue 1.4A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu14.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu14.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu14.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasBlu14.FilenameSpectra)
            i += 1
            'Blue 5.0A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu50.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu50.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu50.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasBlu50.FilenameSpectra)
            i += 1
            'CCT 3000, 100%
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT30.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT30.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas100CCT30.FilenameSpectra)
            i += 1

            'CCT 5500, 100%
            xlsWorksheet.Cells(row, i).value = m_dataPS100.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.MacAdam.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas100CCT55.FilenameSpectra)
            i += 1

            'CCT 6500, 100%
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT65.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT65.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas100CCT65.FilenameSpectra)
            i += 1

            'CCT 3000, 20%
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT30.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT30.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas20CCT30.FilenameSpectra)
            i += 1

            'CCT 5500, 20%
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT55.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT55.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas20CCT55.FilenameSpectra)
            i += 1
            'CCT 6500, 20%
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios20CCT65.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas20CCT65.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas20CCT65.FilenameSpectra)
            i += 1

            'add data
            '5% 5500K
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios5CCT55.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.MacAdam.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.RadIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas5CCT55.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas5CCT55.FilenameSpectra)
            i += 1
            'add data
            '100% 5500K lowIntensity
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100CCT55low.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.MacAdam.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.RadIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100CCT55low.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas100CCT55low.FilenameSpectra)
            i += 1
            '100% 5500K SuperR9
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.T5.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios100SuperR9.T6.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.RadIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.CRI.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.R9.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.R13.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.R15.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeas100SuperR9.CCT.ToString("F0", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeas100SuperR9.FilenameSpectra)
            i += 1
            'save
            xlsWorkBook.Save()
            xlsWorksheet = Nothing
            xlsWorkBook = Nothing
            xlsApp.Quit()
            xlsApp = Nothing
            GC.Collect()
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSaveFinalTestDataRGBToFile: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Speichern der Messwerte",
                           "",
                           "fehlgeschlagen")
        End Try
    End Sub

    Private Sub stepSaveGoldenTestDataToFile(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = True
        Try

            Dim filename As String
            filename = DirectCast(data0, String)

            Dim xlsApp As Excel.Application
            Dim xlsWorkBook As Excel.Workbook
            Dim xlsWorksheet As Excel.Worksheet

            Dim row As Integer
            'open excel
            xlsApp = New Excel.Application
            Dim iniReader As IniReader = New IniReader
            xlsWorkBook = xlsApp.Workbooks.Open(iniReader.ReadValueFromFile("DataStorage", "path", "", ".\Settings.ini") & filename)
            xlsWorksheet = xlsWorkBook.Worksheets("data")

            'search for new line to inserted

            xlsWorksheet.Cells(100000, 2).End(Excel.XlDirection.xlUp).Offset(1, 0).Select()
            Dim bereich As Excel.Range
            bereich = xlsApp.ActiveCell()
            row = bereich.Row
            'add date, clock
            Dim i As Integer
            i = 1
            xlsWorksheet.Cells(row, i).value = ""
            i += 1
            xlsWorksheet.Cells(row, i).value = Now.Date.ToShortDateString()
            i += 1
            xlsWorksheet.Cells(row, i).value = Now.ToLongTimeString()
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios.SN.ToString("000000")
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios.BurnTime.ToString("000000")
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHelios.Version_main & "." & m_dataHelios.Version_sub & "." & m_dataHelios.Version_build
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataPSSby.Voltage.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataPSSby.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1

            'add data
            'Red 1.4A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed14.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed14.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed14.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed14.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasRed14.FilenameSpectra)
            i += 1
            'Red 5.0A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed50.U1.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed50.I1.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosRed50.T1.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasRed50.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasRed50.FilenameSpectra)
            i += 1
            'Green 1.4A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre14.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre14.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre14.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre14.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasGre14.FilenameSpectra)
            i += 1
            'Green 5.0A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre50.U2.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre50.I2.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosGre50.T2.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasGre50.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasGre50.FilenameSpectra)
            i += 1
            'Blue 1.4A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu14.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu14.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu14.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu14.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasBlu14.FilenameSpectra)
            i += 1
            'Blue 5.0A
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu50.U3.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu50.I3.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosBlu50.T3.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.Cx.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.Cy.ToString("F4", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.PhotoIntegral.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.LambdaDom.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasBlu50.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasBlu50.FilenameSpectra)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosUV.U4.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosUV.I4.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataHeliosUV.T4.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasUV.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Cells(row, i).value = m_dataCasMeasUV.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            i += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, i), "spectra\" & m_dataCasMeasUV.FilenameSpectra)
            'save
            xlsWorkBook.Save()
            xlsWorksheet = Nothing
            xlsWorkBook = Nothing
            xlsApp.Quit()
            xlsApp = Nothing
            GC.Collect()
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSaveGoldenTestDataToFile: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Speichern der Messwerte",
                           "",
                           "fehlgeschlagen")
        End Try
    End Sub

    Private Sub stepSaveUVValuesToFile(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim filename As String
            filename = DirectCast(data0, String)

            'write Status to statusText

            Dim xlsApp As Excel.Application
            Dim xlsWorkBook As Excel.Workbook
            Dim xlsWorksheet As Excel.Worksheet

            Dim row As Integer
            'open excel
            xlsApp = New Excel.Application
            Dim iniReader As IniReader = New IniReader
            xlsWorkBook = xlsApp.Workbooks.Open(iniReader.ReadValueFromFile("DataStorage", "path", "", ".\Settings.ini") & filename)
            xlsWorksheet = xlsWorkBook.Worksheets("data")

            'search for new line to inserted
            xlsWorksheet.Cells(100000, 2).End(Excel.XlDirection.xlUp).Offset(1, 0).Select()
            Dim bereich As Excel.Range
            bereich = xlsApp.ActiveCell()
            row = bereich.Row
            Dim col As Integer
            col = bereich.Column
            'add date, clock
            col = 1
            xlsWorksheet.Cells(row, col).value = ""
            col += 1
            xlsWorksheet.Cells(row, col).value = Now.Date.ToShortDateString()
            col += 1
            xlsWorksheet.Cells(row, col).value = Now.ToLongTimeString()
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataHelios.SnUV.ToString("000000")
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataHelios.SN.ToString("000000")
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataHelios.BurnTime.ToString("000000")
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataHelios.Version_main.ToString("0") & "." & m_dataHelios.Version_sub.ToString("0") & "." & m_dataHelios.Version_build.ToString("0")
            col += 1

            xlsWorksheet.Cells(row, col).value = m_dataPSV.Current.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataHeliosUV.U4.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataHeliosUV.I4.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataHeliosUV.T4.ToString("F1", Globalization.CultureInfo.InvariantCulture)
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataCasMeasUV.RadIntegral.ToString("F3", Globalization.CultureInfo.InvariantCulture)
            col += 1
            xlsWorksheet.Cells(row, col).value = m_dataCasMeasUV.LambdaPeak.ToString("F2", Globalization.CultureInfo.InvariantCulture)
            col += 1
            xlsWorksheet.Hyperlinks.Add(xlsWorksheet.Cells(row, col), "spectra\" & m_dataCasMeasUV.FilenameSpectra)
            'save
            xlsWorkBook.Save()
            xlsWorksheet = Nothing
            xlsWorkBook = Nothing
            xlsApp.Quit()
            xlsApp = Nothing
            GC.Collect()
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSaveUVValuesToFile: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Speichern der Messwerte",
                           "",
                           "fehlgeschlagen")
        End Try
    End Sub

    Private Sub stepSaveSpectra2File(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim dataCas As DataCASMeas
            dataCas = DirectCast(data0, DataCASMeas)
            Dim data As DataHelios
            data = DirectCast(data1, DataHelios)
            Dim name As String
            name = DirectCast(data2, String)
            Dim filename As String = String.Empty
            If name.ToLower.EndsWith("uv") Then
                filename = data.SnUV.ToString("000000")
            Else
                filename = data.SN.ToString("000000")
            End If

            filename &= "_" &
                       data.BurnTime.ToString("000000") & "_" &
                       name & "_" &
                       Now.Date.Year.ToString("0000") & Now.Date.Month.ToString("00") & Now.Date.Day.ToString("00") & "_" &
                       Now.Hour.ToString("00") & Now.Minute.ToString("00") &
                       ".isd"
            'dataCas.PathSpectra = Application.StartupPath & "\spectra\"
            Dim iniReader As IniReader = New IniReader
            dataCas.PathSpectra = iniReader.ReadValueFromFile("DataStorage", "path", "", ".\Settings.ini") & "spectra\"
            dataCas.FilenameSpectra = filename
            m_cas140.SaveSpectrumtoFile(dataCas.PathSpectra & dataCas.FilenameSpectra)
            Dim saveString As String
            saveString = FileSystem.ReadAllText(dataCas.PathSpectra & dataCas.FilenameSpectra, System.Text.Encoding.UTF8)
            FileSystem.WriteAllText(dataCas.PathSpectra & dataCas.FilenameSpectra, saveString, False, System.Text.Encoding.GetEncoding("Windows-1252"))
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSaveSpectra2File: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Speichern des Spektrums",
                           "",
                           "fehlgeschlagen")

        End Try
    End Sub

    Public Sub stepSetDebugStatus(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim textString As String = String.Empty
        Try
            Dim dioPorts As HardwareCommunication.NI6520_DAQ
            dioPorts = DirectCast(data0, HardwareCommunication.NI6520_DAQ)
            Dim valueText As String
            valueText = DirectCast(data1, String)
            If Not String.IsNullOrEmpty(dioPorts.Device) Then
                Select Case valueText
                    Case "true"
                        dioPorts.setSingleDOLine(dioPorts.Jumper.Port, dioPorts.Jumper.PortPin, True)
                        textString = " Jumper eingesteckt"
                        fctGood = True
                    Case "false"
                        dioPorts.setSingleDOLine(dioPorts.Jumper.Port, dioPorts.Jumper.PortPin, False)
                        textString = " Jumper ausgesteckt"
                        fctGood = True
                    Case Else
                        textString = " Fehler!! Schritt"
                        fctGood = False
                End Select
            Else
                textString = " Keine DIO Karte!! Schritt"
            End If

        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetDebugStatus: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           textString,
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepSetHeliosCCT(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            'write Status to statusText
            'statusText.Add(CurrentStep.ToString & "/" & MaxSteps.ToString & " Set Helios CCT")
            'Application.DoEvents()
            Dim cctString As String
            cctString = DirectCast(data1, String)
            Dim cct As Integer
            cct = CInt(cctString)
            If can.SetCctColor(CAN_DEST, cct) Then fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetHeliosCCT: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Helios CCT setzen",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepSetHeliosColorIntensity(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim intensity As Double
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim color As String
            color = DirectCast(data1, String)
            intensity = Convert.ToDouble(DirectCast(data2, String))
            Select Case color
                Case "red"
                    If Not can.SetRGbColor(CAN_DEST, 255, 0, 0) Then result = False
                Case "green"
                    If Not can.SetRGbColor(CAN_DEST, 0, 255, 0) Then result = False
                Case "blue"
                    If Not can.SetRGbColor(CAN_DEST, 0, 0, 255) Then result = False
                Case Else
                    Throw New InvalidEnumArgumentException
            End Select
            If intensity < 5 Then
                If Not can.SetRgbIntensity(CAN_DEST, 5) Then result = False
                If Not can.SetRgbOnState(CAN_DEST, False) Then result = False
            Else
                If Not can.SetRgbIntensity(CAN_DEST, intensity) Then result = False
            End If
            'RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Set Helios Color Intensity: " & color & " " & intensity.ToString("F0"))
            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetHeliosColorIntensity: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Setze Intensität für Farbe: " & intensity.ToString("F0"),
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepSetHeliosIntensityRGB(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim intensity As Double
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim intensityString As String
            intensityString = DirectCast(data1, String)

            intensity = Convert.ToDouble(intensityString, Globalization.CultureInfo.InvariantCulture)
            If intensity < 5 Then
                If Not can.SetRgbIntensity(CAN_DEST, 5) Then result = False
                If Not can.SetRgbOnState(CAN_DEST, False) Then result = False
            Else
                If Not can.SetRgbIntensity(CAN_DEST, intensity) Then result = False
            End If
            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetHeliosIntensityRGB: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Setze Intensität: " & intensity.ToString("F0"),
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepSetHeliosIntensityUVDebug(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim intensity As Double
        Try
            Dim returnValue As Boolean = True
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim intensityString As String
            intensityString = DirectCast(data1, String)

            intensity = Convert.ToDouble(intensityString, Globalization.CultureInfo.InvariantCulture)
            Dim dacMax As Single
            If Not can.GetCalibrationDataRgbUv(CAN_DEST, 3, 101, dacMax) Then
                returnValue = False
            End If

            If intensity < 5 Then
                If Not can.SetUvIntensity(CAN_DEST, 5) Then
                    returnValue = False
                End If
                If Not can.SetUvOnState(CAN_DEST, False) Then
                    returnValue = False
                End If
            Else
                If Not m_oHeliosCommunicationBoard.SetRGbColor(CAN_DEST, 255, 255, 255) Then
                    returnValue = False
                End If
                If Not m_oHeliosCommunicationBoard.SetRgbOnState(CAN_DEST, False) Then
                    returnValue = False
                End If
                If Not can.SetUvIntensity(CAN_DEST, (intensity * CDbl(dacMax) / 4095.0F)) Then
                    returnValue = False
                End If
                If Not can.SetUvOnState(CAN_DEST, True) Then
                    returnValue = False
                End If
            End If
            fctGood = returnValue
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetHeliosIntensityUVDebug: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "setzen des UV Kanals",
                           "erfolgreich",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepSetHeliosColorIntensityRef(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim color As String = String.Empty
        Dim stepNumber As Integer = 0
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            color = DirectCast(data1, String)
            stepNumber = Convert.ToInt32(DirectCast(data2, String))
            Dim m, n As Single
            Select Case color
                Case "red"
                    If Not can.SetRGbColor(CAN_DEST, 255, 0, 0) Then result = False
                Case "green"
                    If Not can.SetRGbColor(CAN_DEST, 0, 255, 0) Then result = False
                Case "blue"
                    If Not can.SetRGbColor(CAN_DEST, 0, 0, 255) Then result = False
                Case Else
            End Select

            m = Me.m_calibList.Find(Function(x) (x.Color = color)).CValue.Dac.Slope
            n = Me.m_calibList.Find(Function(x) (x.Color = color)).CValue.Dac.Offset

            Dim intensity As Double
            Dim intensity2Dac As Double
            intensity = Me.m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.SetIntensity
            If (intensity >= 2.0) Then
                intensity2Dac = (intensity * 5.8 / 100.0 - n) / (m * 40.95)
                If intensity2Dac > 100 And intensity2Dac <= 120 Then
                    intensity2Dac = 100
                End If
            Else
                intensity2Dac = 5
            End If
            Me.m_measurementList.Find(Function(x) ((x.Color = color) And (x.StepNumber = stepNumber))).MValue.IntensityDAC = intensity2Dac * 40.95

            If (intensity2Dac < 0) Or (intensity2Dac > 100) Then
                fctGood = False
                RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:intensity2Dac: " & intensity2Dac & ", Min: 0 , Max: 120")
                RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:intensity: " & intensity)
                RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:m: " & m)
                RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:n: " & n)
            End If
            If Not can.SetRgbIntensity(CAN_DEST, intensity2Dac) Then result = False

            'write Status to statusText
            'RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Set Helios Intensity: " & color & " " & intensity & ". Step:" & stepNumber.ToString)
            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetHeliosColorIntensityRef: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Setze Intensität " & color & " " & stepNumber.ToString,
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepSetHeliosCurrent(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim color As String = String.Empty
        Dim current As Double = 0
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            color = DirectCast(data1, String)
            current = Convert.ToDouble(DirectCast(data2, String), Globalization.CultureInfo.InvariantCulture)
            Dim m, n As Single
            Dim intensity As Double
            Select Case color
                Case "red"
                    If Not can.SetRGbColor(CAN_DEST, 255, 0, 0) Then result = False
                    If Not can.GetCalibrationDataRgbUv(CAN_DEST, 0, 4, m) Then result = False
                    If Not can.GetCalibrationDataRgbUv(CAN_DEST, 0, 5, n) Then result = False
                Case "green"
                    If Not can.SetRGbColor(CAN_DEST, 0, 255, 0) Then result = False
                    If Not can.GetCalibrationDataRgbUv(CAN_DEST, 1, 4, m) Then result = False
                    If Not can.GetCalibrationDataRgbUv(CAN_DEST, 1, 5, n) Then result = False
                Case "blue"
                    If Not can.SetRGbColor(CAN_DEST, 0, 0, 255) Then result = False
                    If Not can.GetCalibrationDataRgbUv(CAN_DEST, 2, 4, m) Then result = False
                    If Not can.GetCalibrationDataRgbUv(CAN_DEST, 2, 5, n) Then result = False
                Case Else
                    Throw New InvalidEnumArgumentException
            End Select
            If result Then

            End If
            If (current > 0.01) Then
                intensity = ((current - n) / m) / 40.95
                If (intensity < 0) Or (intensity > 100) Then
                    fctGood = False
                    RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:intensity: " & intensity & ", Min: 0 , Max: 100")
                    RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:current: " & current)
                    RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:m: " & m)
                    RaiseEvent addLogFile(Me, "## " & color & "stepSetHeliosCurrent:n: " & n)
                End If
                If Not can.SetRgbIntensity(CAN_DEST, intensity) Then result = False
            Else
                If Not can.SetRgbIntensity(CAN_DEST, 5) Then result = False
                If Not can.SetRgbOnState(CAN_DEST, False) Then result = False
            End If
            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetHeliosCurrent: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Setze Strom in LED " & color & " " & current.ToString("F2"),
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepSetHeliosSuperR9(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim direction As String = String.Empty
        Dim result As Boolean = True
        Try
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            direction = DirectCast(data1, String)

            Select Case direction
                Case "on"
                    If Not can.SetSuperR9(CAN_DEST) Then result = False
                Case "off"
                    If Not can.ResetSuperR9(CAN_DEST) Then result = False
                Case Else
            End Select

            fctGood = result
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetHeliosSuperR9: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Setze SuperR9 " & direction,
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepSetPSStatus(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim voltage As Double = 0
        Try
            Dim ps As HardwareCommunication.TcpKeysightN5767Communication
            ps = DirectCast(data0, HardwareCommunication.TcpKeysightN5767Communication)
            Dim voltageString As String
            voltageString = DirectCast(data1, String)
            Dim currentString As String
            currentString = DirectCast(data2, String)
            voltage = Convert.ToDouble(voltageString, System.Globalization.CultureInfo.InvariantCulture)
            Dim current As Double = Convert.ToDouble(currentString, System.Globalization.CultureInfo.InvariantCulture)
            ps.setVoltage(voltage)
            ps.setCurrent(current)
            If voltage > 0.1 Then
                ps.setOutputOnOff(True)
            Else
                ps.setOutputOnOff(False)
            End If
            Threading.Thread.Sleep(100)
            ps.getVoltage(voltageString)
            If Math.Abs(voltage - Convert.ToDouble(voltageString, System.Globalization.CultureInfo.InvariantCulture)) < 0.1 Then
                fctGood = True
            End If

        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepSetPSStatus: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           " PS auf " & voltage.ToString("F2") & "V gesetzt",
                           "",
                           " fehlgeschlagen")
        End Try

    End Sub

    Public Sub stepTest(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = True
        Try
            'Dim myProtocol As ProtocolEOLBox = New ProtocolEOLBox
            'Dim result As String = String.Empty
            'myProtocol.Open("COM5")
            'If Not myProtocol.SetLWLPos("in") Then
            '    fctGood = False
            'End If
            'Threading.Thread.Sleep(1000)
            'If Not myProtocol.GetLWLPos(result) Then
            '    fctGood = False
            'End If
            'If Not myProtocol.SetLWLPos("out") Then
            '    fctGood = False
            'End If
            'Threading.Thread.Sleep(1000)
            'If Not myProtocol.GetLWLPos(result) Then
            '    fctGood = True
            'End If
            'myProtocol.Close()
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepTest: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Test",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepWaitForOK(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim text As String
            text = DirectCast(data0, String)
            Dim messBox As HEOLMessageBox
            If IsNumeric(text) Then
                messBox = New HEOLMessageBox(CInt(text))
            Else
                messBox = New HEOLMessageBox("Information", text)
            End If

            If DialogResult.OK = messBox.ShowDialog() Then
                fctGood = True
            End If
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepWaitForOK: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Anweisung folgen",
                           "erfolgreich",
                           "fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepWaitSomeTime(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim sleepTime As Double = 0
        Dim timeOut As Date = Date.Now
        Try
            sleepTime = Convert.ToDouble(DirectCast(data0, String))
            timeOut = Date.Now.AddMilliseconds(CInt(sleepTime * 1000))
            While timeOut > Date.Now
                feedbackStatus(True,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "Warten: " & (timeOut.Subtract(Date.Now).TotalMilliseconds / 1000).ToString("F1") & "/" & sleepTime.ToString("F1"),
                           "")
                'RaiseEvent newStateMachineStatus(Me, New StateMachineStatus(CurrentStep.ToString & "/" & MaxSteps.ToString & " Wait: " & (timeOut.Subtract(Date.Now).TotalMilliseconds / 1000).ToString("F1") & "/" & sleepTime.ToString("F1"), Drawing.Color.White, CurrentStep / MaxSteps))
                If ProgramFlow.Count = 1 Then
                    feedbackStatus(False,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "",
                           "Ablauf abgebrochen")
                    'RaiseEvent newStateMachineStatus(Me, New StateMachineStatus(CurrentStep.ToString & "/" & MaxSteps.ToString & " Sequence aborted", Drawing.Color.Red, CurrentStep / MaxSteps))
                    timeOut = Date.Now
                End If
                Threading.Thread.Sleep(10)
            End While
            '            RaiseEvent addLogFile(Me, CurrentStep.ToString & "/" & MaxSteps.ToString & " Wait:" & sleepTime.ToString("F1"))
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepWaitSomeTime: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           True,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "",
                           "Warten: " & (timeOut.Subtract(Date.Now).TotalMilliseconds / 1000).ToString("F1") & "/" & sleepTime.ToString("F1"),
                           "Warten fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepWaitUntilLightGuideIsMoved(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Dim position As String = String.Empty
        Try
            Dim cas As CasCommunication.cCAS140
            cas = DirectCast(data0, CasCommunication.cCAS140)
            Dim direction As String
            direction = DirectCast(data1, String)
            Do
                cas.AutoRangeMeasurement = False
                cas.FilterPosition = 2
                cas.Averages = 2
                cas.IntegrationTime = 50
                If Not cas.Measurement() Then Throw New Exception("CAS.measurement() failed")
                If (cas.Flux > 10) Then
                    position = "in"
                Else
                    position = "out"
                End If
            Loop While position <> direction
            fctGood = True
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepWaitUntilLightGuideIsMoved: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           " Lichtleiter " & position,
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepWriteCalibValuesToEEPROM(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            m_dataCalib.CalibDone = 1

            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim color As String = ""
            Dim errorIndex As Boolean = False
            Dim result As Boolean = True
            If Not can.ExecuteOpenRGB(CAN_DEST) Then result = False
            For i = 0 To 2
                Select Case i
                    Case 0
                        color = "red"
                    Case 1
                        color = "green"
                    Case 2
                        color = "blue"
                End Select
                Dim colorIndex As Integer
                colorIndex = m_calibList.FindIndex(Function(x) (x.Color = color))
                Dim vIndex As Integer
                vIndex = CalIndexCur
                'current
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Current.Slope)) then result=false
                vIndex = vIndex + 1
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Current.Offset)) then result=false
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexVol) Then
                    errorIndex = True
                End If
                'voltage
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Voltage.Slope)) then result=false
                vIndex = vIndex + 1
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Voltage.Offset)) then result=false
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexDac) Then
                    errorIndex = True
                End If
                'dac
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Dac.Slope)) then result=false
                vIndex = vIndex + 1
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Dac.Offset)) then result=false
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexCx) Then
                    errorIndex = True
                End If
                'cx
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_low.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_low.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_low.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_low.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_low.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_low.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_low.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_low.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_high.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_high.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_high.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_high.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_high.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_high.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_high.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_high.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.I_border)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cx.T_border)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexCy) Then
                    errorIndex = True
                End If
                'cy
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_low.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_low.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_low.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_low.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_low.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_low.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_low.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_low.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_high.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_high.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_high.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_high.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_high.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_high.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_high.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_high.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.I_border)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Cy.T_border)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexTempChip) Then
                    errorIndex = True
                End If
                'tempChip
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.TempChip.Rth)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.TempChip.Eta)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexPhi) Then
                    errorIndex = True
                End If
                'phi
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.T.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.T.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.T.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.T.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl1.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl1.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl1.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl1.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl2.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl2.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl2.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl2.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl3.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl3.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl3.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl3.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl4.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl4.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl4.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_spl4.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_last.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_last.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_last.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.I_last.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.Border_I_1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.Border_I_2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.Border_I_3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Phi.Border_I_4)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexPhiAdc) Then
                    errorIndex = True
                End If
                'phiADC
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor1.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor1.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor1.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor1.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor2.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor2.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor2.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Factor2.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDLambdaDom.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDLambdaDom.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDLambdaDom.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDLambdaDom.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDTemp.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDTemp.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDTemp.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.PDTemp.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Offset_PD)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiADC.Dummy0)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexPhiMax) Then
                    errorIndex = True
                End If
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Cx5500K)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Cy5500K)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy02)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy03)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy10)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy11)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy12)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy13)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy20)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.Additional.Dummy21)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexAdditional) Then
                    errorIndex = True
                End If
                'phiMax
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.PhiMax)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexDacMax) Then
                    errorIndex = True
                End If
                'dacMax
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToUInt16(Me.m_calibList.Item(colorIndex).CValue.DacMax)) then result=false
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexIByDac) Then
                    errorIndex = True
                End If
                'iByDac
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.IByDac.Fct.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.IByDac.Fct.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.IByDac.Fct.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.IByDac.Fct.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexUByI) Then
                    errorIndex = True
                End If
                'uByI
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.UByI.Fct.F_x3)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.UByI.Fct.F_x2)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.UByI.Fct.F_x1)) Then result = False
                vIndex = vIndex + 1
                If Not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToSingle(Me.m_calibList.Item(colorIndex).CValue.UByI.Fct.F_x0)) Then result = False
                vIndex = vIndex + 1
                If Not (vIndex = CalIndexSuperR9) Then
                    errorIndex = True
                End If
                ''superR9
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToUInt16(Me.m_calibList.Item(colorIndex).CValue.SuperR9.Cx)) then result=false
                'vIndex = vIndex + 1
                'if not can.SetCalibrationDataRgbUv(CAN_DEST, i, vIndex, Convert.ToUInt16(Me.m_calibList.Item(colorIndex).CValue.SuperR9.Cy)) then result=false
                'vIndex = vIndex + 1
                'If Not (vIndex = CalIndexAfterLast) Then
                '    errorIndex = True
                'End If
            Next
            If Not can.ExecuteCloseRGB(CAN_DEST) Then result = False
            'can.ExecuteOpenUV(CAN_DEST)
            'Dim dacMaxUv As Integer
            ''dacMax should be 5.2A. So 5.2A will be probably be the average of Red/Green/Blue times 5.2/5
            '' (red + green + blue) / 3 / 5 * 5.2 =  (red + green + blue) / 2.884615
            'dacMaxUv = Convert.ToInt16((Me.m_calibList.Item(0).CValue.DacMax +
            '            Me.m_calibList.Item(0).CValue.DacMax +
            '            Me.m_calibList.Item(0).CValue.DacMax) /
            '            2.884615)
            'If Not can.SetCalibrationDataRgbUv(CAN_DEST, 3, 101, Convert.ToUInt16(dacMaxUv)) Then result = False
            'If Not can.ExecuteCloseUV(CAN_DEST) Then result = False

            fctGood = result
            If fctGood Then m_dataCalib.CalibSaved = 1
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepWriteCalibValuesToEEPROM: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Werte in EEPROM schreiben",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Private Sub stepWriteSNToEEPROM(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim returnValue As Boolean = True
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)


            Dim data As Byte()
            Dim intData0, intData1 As UInt32
            If Not (can.ExecuteOpenRGB(CAN_DEST)) Then
                returnValue = False
            Else
                For i = 0 To 6
                    'Material Number
                    data = System.Text.Encoding.ASCII.GetBytes(Me.smData.MatNumber(i))
                    If data(0) = CByte(68) Then
                        intData1 = CUInt(data(1) * 16777216)
                        intData1 += CUInt(data(2) * 65536)
                        intData1 += CUInt(data(3) * 256)
                        intData1 += CUInt(data(4))
                        If Not can.SetSerialNumber(CAN_DEST, i, 0, 1, intData1) Then
                            returnValue = False
                        End If

                        intData0 = CUInt(data(5) * 16777216)
                        intData0 += CUInt(data(6) * 65536)
                        intData0 += CUInt(data(7) * 256)
                        intData0 += CUInt(data(8))
                        If Not can.SetSerialNumber(CAN_DEST, i, 0, 0, intData0) Then
                            returnValue = False
                        End If
                    End If

                    'Serial Number
                    intData0 = CUInt(Me.smData.SerialNumber(i))
                    If Not can.SetSerialNumber(CAN_DEST, i, 0, 2, intData0) Then
                        returnValue = False
                    End If

                    'Binning
                    If i < 4 Then

                    Else
                        data = System.Text.Encoding.ASCII.GetBytes(Me.smData.Binning(i))
                        intData0 = CUInt(data(0))
                        intData1 = CUInt(data(1))
                        If Not can.SetSerialNumber(CAN_DEST, i, 0, 3, intData0) Then
                            returnValue = False
                        End If
                        If Not can.SetSerialNumber(CAN_DEST, i, 0, 4, intData1) Then
                            returnValue = False
                        End If
                    End If
                Next
            End If
            If Not (can.ExecuteCloseRGB(CAN_DEST)) Then
                returnValue = False
            End If



            Dim sn As UInt32
            Dim main_rel As UInt16
            Dim sub_rel As UInt16
            If Not can.GetZeissVersionAndSN(CAN_DEST, sn, main_rel, sub_rel) Then
                returnValue = False
            End If


            If Not can.ExecuteOpenNamePlate(CAN_DEST) Then
                returnValue = False
            End If

            Threading.Thread.Sleep(10)

            sn = CUInt(Me.smData.CZMSeriennummer)
            If Not can.SetZeissVersionAndSN(CAN_DEST, sn, main_rel, sub_rel) Then
                returnValue = False
            End If
            Threading.Thread.Sleep(10)
            If Not can.ExecuteCloseNamePlate(CAN_DEST) Then
                returnValue = False
            End If

            fctGood = returnValue

            If fctGood Then m_dataHelios.SN_saved = 1

        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepWriteSNToEEPROM: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Platinennummern schreiben",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepWriteSuperR9Values(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim result As Boolean = True
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            If Not can.ExecuteOpenRGB(CAN_DEST) Then result = False
            If Not can.SetCalibrationDataRgbUv(CAN_DEST, 0, CalIndexSuperR9, Convert.ToUInt16(Me.m_calibList.Item(0).CValue.SuperR9.Cx)) Then result = False
            If Not can.SetCalibrationDataRgbUv(CAN_DEST, 0, CalIndexSuperR9 + 1, Convert.ToUInt16(Me.m_calibList.Item(0).CValue.SuperR9.Cy)) Then result = False
            If Not can.ExecuteCloseRGB(CAN_DEST) Then result = False
            fctGood = result
            If fctGood Then m_dataCalib.CalibSuperR9Saved = 1
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepWriteSuperR9Values: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Super R9 Werte in EEPROM schreiben",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

    Public Sub stepWriteUVDataToEEPROM(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        Dim fctGood As Boolean = False
        Try
            Dim returnValue As Boolean = True
            Dim can As HELIOSCommunication.HELIOSCommunication
            can = DirectCast(data0, HELIOSCommunication.HELIOSCommunication)
            Dim bytes As Byte()
            Dim intData0, intData1 As UInt32
            If Not (can.ExecuteOpenUV(CAN_DEST)) Then
                returnValue = False
            Else
                'Material Number
                bytes = System.Text.Encoding.ASCII.GetBytes(smData.MatNumber(0))
                If bytes(0) = CByte(68) Then
                    intData1 = CUInt(bytes(1) * 16777216)
                    intData1 += CUInt(bytes(2) * 65536)
                    intData1 += CUInt(bytes(3) * 256)
                    intData1 += CUInt(bytes(4))
                    If Not can.SetSerialNumber(CAN_DEST, 0, 1, 1, intData1) Then
                        'returnValue = False
                    End If

                    intData0 = CUInt(bytes(5) * 16777216)
                    intData0 += CUInt(bytes(6) * 65536)
                    intData0 += CUInt(bytes(7) * 256)
                    intData0 += CUInt(bytes(8))
                    If Not can.SetSerialNumber(CAN_DEST, 0, 1, 0, intData0) Then
                        'returnValue = False
                    End If
                End If

                'Serial Number
                intData0 = CUInt(Me.smData.SerialNumber(0))
                If Not m_oHeliosCommunicationBoard.SetSerialNumber(CAN_DEST, 0, 1, 2, intData0) Then
                    returnValue = False
                End If

                'Binning
                bytes = System.Text.Encoding.ASCII.GetBytes(Me.smData.Binning(0))
                intData0 = CUInt(bytes(0))
                intData1 = CUInt(bytes(1))
                If Not can.SetSerialNumber(CAN_DEST, 0, 1, 3, intData0) Then
                    returnValue = False
                End If
                If Not can.SetSerialNumber(CAN_DEST, 0, 1, 4, intData1) Then
                    returnValue = False
                End If

            End If
            If Not (can.ExecuteCloseUV(CAN_DEST)) Then
                'returnValue = False
            End If
            fctGood = returnValue

            If fctGood Then m_dataHelios.SNV_saved = 1
        Catch ex As Exception
            RaiseEvent addLogFile(Me, "!! SN" & smData.CZMSeriennummer.ToString() & " " &
                                      Now.Date.Year.ToString("D4") &
                                      Now.Date.Month.ToString("D2") &
                                      Now.Date.Day.ToString("D2") & " " &
                                      Now.Hour.ToString("D2") &
                                      Now.Minute.ToString("D2") &
                                      Now.Second.ToString("D2") & " " &
                                      "Error in function stepWriteUVDataToEEPROM: " & ex.Message & vbNewLine & ex.StackTrace)
            Throw New Exception("stateMachineEndOfLineError")
        Finally
            feedbackStatus(fctGood,
                           Me.Status,
                           False,
                           Me.CurrentStep,
                           Me.MaxSteps,
                           "Seriennummern in EEPROM schreiben",
                           "",
                           " fehlgeschlagen")
        End Try
    End Sub

#End Region

    Public Sub feedbackStatus(ByVal fctGood As Boolean,
                              ByVal prevStatus As smhStatus,
                              ByVal continueOnError As Boolean,
                              ByVal currentStep As Integer,
                              ByVal maxStep As Integer,
                              ByVal text As String,
                              ByVal textGood As String,
                              ByVal textBad As String)
        'statusLight
        If Not (fctGood = True) Or Not (prevStatus = smhStatus.WorkingGood) Then
            RaiseEvent changeStatusLight(Me, statusLight.Red)
        End If

        'statusText
        If fctGood = True And prevStatus = smhStatus.WorkingBad And continueOnError = False Then
            'yellow text with abort message
            RaiseEvent newStateMachineStatus(Me,
                                             New StateMachineStatus(currentStep.ToString & "/" & maxStep.ToString & " " & text & " " & "Abarbeitung wegen vorheriger Fehler beendet",
                                                                    Color.Yellow,
                                                                    currentStep / maxStep))
        ElseIf fctGood = False Then
            'red text
            RaiseEvent newStateMachineStatus(Me,
                                             New StateMachineStatus(currentStep.ToString & "/" & maxStep.ToString & " " & text & " " & textBad,
                                                                    Color.Red,
                                                                    currentStep / maxStep))
        Else
            RaiseEvent newStateMachineStatus(Me,
                                             New StateMachineStatus(currentStep.ToString & "/" & maxStep.ToString & " " & text & " " & textGood,
                                                                    Color.LightGreen,
                                                                    currentStep / maxStep))
        End If

        'further action (setting of smhStatus)
        If (continueOnError = False And fctGood = False) Or (continueOnError = False And prevStatus = smhStatus.WorkingBad) Then
            'end smhExecution
            Me.Status = smhStatus.FinishBad
        ElseIf (fctGood = True And prevStatus = smhStatus.WorkingGood) Then
            'continue with execution with no restrictions
            Me.Status = smhStatus.WorkingGood
        Else
            'continue, but because of errors quit at the end
            Me.Status = smhStatus.WorkingBad
        End If
    End Sub

    Private Function calcSplineCoef(ByVal splineFit As Spline.SplineCoefficient) As Single()
        Dim coef(3) As Single
        Dim a, b, c, d As Double
        a = splineFit.a - splineFit.b * splineFit.x + splineFit.c * splineFit.x ^ 2 - splineFit.d * splineFit.x ^ 3
        b = splineFit.b - 2 * splineFit.c * splineFit.x + 3 * splineFit.d * splineFit.x ^ 2
        c = splineFit.c - 3 * splineFit.d * splineFit.x
        d = splineFit.d
        coef(0) = CSng(a)
        coef(1) = CSng(b)
        coef(2) = CSng(c)
        coef(3) = CSng(d)
        calcSplineCoef = coef
    End Function

    Private Function calcMacAdam(ByVal cxSet As Double,
                                 ByVal cySet As Double,
                                 ByVal cxReal As Double,
                                 ByVal cyReal As Double) As Double
        Dim uSet, vSet, uReal, vReal As Double
        uSet = (4 * cxSet) / (12 * cySet - 2 * cxSet + 3)
        vSet = (6 * cySet) / (12 * cySet - 2 * cxSet + 3)
        uReal = (4 * cxReal) / (12 * cyReal - 2 * cxReal + 3)
        vReal = (6 * cyReal) / (12 * cyReal - 2 * cxReal + 3)
        Return Math.Sqrt(Math.Pow(uSet - uReal, 2) + Math.Pow(vSet - vReal, 2)) * 1000.0
    End Function
End Class
