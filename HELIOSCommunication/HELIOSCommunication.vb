Imports HELIOSCommunication.Peak.Can.Basic
Imports System.Collections.Generic


Public Class HELIOSCommunication
    Inherits CANCommunication

    'Friend Const CLightCtrl As String = "160C2282"
    'Friend Const CDebug As String = "160C220D"
    'Friend Const CTemp As String = "160c2314"

    Public Sub New(oBaudrate As TPCANBaudrate, oHwType As TPCANType)
        MyBase.New(oBaudrate, oHwType)
    End Sub

    Private Function ReadNxtMsg(Optional ByRef CanMsg As TPCANMsg = Nothing) As TPCANStatus
        'Dim CANMsg As TPCANMsg = Nothing
        Dim CANTimeStamp As TPCANTimestamp

        ReadNxtMsg = PCANBasic.Read(m_PcanHandle, CanMsg, CANTimeStamp)

        Return ReadNxtMsg
    End Function

    ''' <summary>
    ''' Empties the receive queue of the PCAN hardware
    ''' </summary>
    ''' <remarks></remarks>
    Private Function EmptyRcvQueue() As Boolean
        Dim i As Integer
        i = 0
        While (Not (ReadNxtMsg() = TPCANStatus.PCAN_ERROR_QRCVEMPTY))
            System.Threading.Thread.Sleep(20)
            i += 1
            If (i > 10) Then
                Return False
            End If
        End While
        Return True
    End Function
#Region "CheckAnswerAndReturnValues"
    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean) As Boolean
        Dim CANMsg As TPCANMsg = Nothing
        acknowledge = False
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANMsg)
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean, ByRef sByte0 As String) As Boolean
        Dim CANMsg As TPCANMsg = Nothing
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANMsg)
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    If CANMsg.LEN > 2 Then
                        sByte0 = CANMsg.DATA(2).ToString("X2")
                    End If
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean, ByRef sByte0 As String, ByRef sByte1 As String) As Boolean
        Dim CANMsg As TPCANMsg = Nothing
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANMsg)
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    If CANMsg.LEN > 2 Then
                        sByte0 = CANMsg.DATA(2).ToString("X2")
                    End If
                    If CANMsg.LEN > 3 Then
                        sByte1 = CANMsg.DATA(3).ToString("X2")
                    End If
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String) As Boolean
        Dim CANMsg As TPCANMsg = Nothing
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANMsg)
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    If CANMsg.LEN > 2 Then
                        sByte0 = CANMsg.DATA(2).ToString("X2")
                    End If
                    If CANMsg.LEN > 3 Then
                        sByte1 = CANMsg.DATA(3).ToString("X2")
                    End If
                    If CANMsg.LEN > 4 Then
                        sByte2 = CANMsg.DATA(4).ToString("X2")
                    End If
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String) As Boolean
        Dim CANMsg As TPCANMsg = Nothing
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANMsg)
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    If CANMsg.LEN > 2 Then
                        sByte0 = CANMsg.DATA(2).ToString("X2")
                    End If
                    If CANMsg.LEN > 3 Then
                        sByte1 = CANMsg.DATA(3).ToString("X2")
                    End If
                    If CANMsg.LEN > 4 Then
                        sByte2 = CANMsg.DATA(4).ToString("X2")
                    End If
                    If CANMsg.LEN > 5 Then
                        sByte3 = CANMsg.DATA(5).ToString("X2")
                    End If
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String) As Boolean
        Dim CANMsg As TPCANMsg = Nothing
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANMsg)
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    If CANMsg.LEN > 2 Then
                        sByte0 = CANMsg.DATA(2).ToString("X2")
                    End If
                    If CANMsg.LEN > 3 Then
                        sByte1 = CANMsg.DATA(3).ToString("X2")
                    End If
                    If CANMsg.LEN > 4 Then
                        sByte2 = CANMsg.DATA(4).ToString("X2")
                    End If
                    If CANMsg.LEN > 5 Then
                        sByte3 = CANMsg.DATA(5).ToString("X2")
                    End If
                    If CANMsg.LEN > 6 Then
                        sByte4 = CANMsg.DATA(6).ToString("X2")
                    End If
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim CANMsg As TPCANMsg = Nothing
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANMsg)
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    If CANMsg.LEN > 2 Then
                        sByte0 = CANMsg.DATA(2).ToString("X2")
                    End If
                    If CANMsg.LEN > 3 Then
                        sByte1 = CANMsg.DATA(3).ToString("X2")
                    End If
                    If CANMsg.LEN > 4 Then
                        sByte2 = CANMsg.DATA(4).ToString("X2")
                    End If
                    If CANMsg.LEN > 5 Then
                        sByte3 = CANMsg.DATA(5).ToString("X2")
                    End If
                    If CANMsg.LEN > 6 Then
                        sByte4 = CANMsg.DATA(6).ToString("X2")
                    End If
                    If CANMsg.LEN > 7 Then
                        sByte5 = CANMsg.DATA(7).ToString("X2")
                    End If
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function CheckAnswerAndReturnValues(stsResult As TPCANStatus, ND0 As String, ND1 As String, ByRef acknowledge As Boolean, ByRef data() As Byte)
        Dim CANmsg As TPCANMsg = Nothing
        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
            System.Threading.Thread.Sleep(10)
            stsResult = ReadNxtMsg(CANmsg)
            'check 1st Byte of L2C or not
            Dim byteCount As Integer = 0
            If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                If (CANmsg.DATA(0) = Convert.ToByte("80", 16)) Then
                    byteCount = CInt(CANmsg.DATA(1)) + CInt(CANmsg.DATA(2)) * 256
                    ReDim data(byteCount)
                    For i = 0 To 4 Step 1
                        data(i) = CANmsg.DATA(i + 3)
                    Next i
                ElseIf (CANmsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANmsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
                    acknowledge = True
                    If CANmsg.LEN <= 2 Then
                        data = New Byte() {}
                    Else
                        ReDim data(CANmsg.LEN - 3)
                        For i = 0 To data.GetLength(0) - 1 Step 1
                            data(i) = CANmsg.DATA(i + 2).ToString("X2")
                        Next i
                    End If
                    Return True
                End If
            End If

            Dim dataCounter As Integer = 5
            If byteCount <> 0 Then
                Do
                    stsResult = ReadNxtMsg(CANmsg)
                    If stsResult = TPCANStatus.PCAN_ERROR_OK Then
                        If (CANmsg.DATA(0) = Convert.ToByte("81", 16)) Or (CANmsg.DATA(0) = Convert.ToByte("82", 16)) Then
                            For i = 0 To 5 Step 1
                                If dataCounter + i < byteCount Then
                                    data(dataCounter + i) = CANmsg.DATA(2 + i)
                                End If
                            Next i
                            dataCounter += 6
                        End If
                    ElseIf stsResult = TPCANStatus.PCAN_ERROR_QRCVEMPTY Then
                        Return False
                    End If
                Loop Until (CANmsg.DATA(0) = Convert.ToByte("83", 16))
                acknowledge = True
                Return True
            End If
        End If
        Return False
    End Function
#End Region
#Region "CanProtocol"
    Private Function CanProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer) As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        Return CanProtocol(canID, canSource, canDestination, canInstance, canPropMeth, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
    End Function

    Private Function CanProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer, ByRef sByte0 As String) As Boolean
        Dim sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        Return CanProtocol(canID, canSource, canDestination, canInstance, canPropMeth, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
    End Function

    Private Function CanProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer, ByRef sByte0 As String, ByRef sByte1 As String) As Boolean
        Dim sByte2, sByte3, sByte4, sByte5 As String
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        Return CanProtocol(canID, canSource, canDestination, canInstance, canPropMeth, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
    End Function

    Private Function CanProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String) As Boolean
        Dim sByte3, sByte4, sByte5 As String
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        Return CanProtocol(canID, canSource, canDestination, canInstance, canPropMeth, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
    End Function

    Private Function CanProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String) As Boolean
        Dim sByte4, sByte5 As String
        sByte4 = String.Empty
        sByte5 = String.Empty
        Return CanProtocol(canID, canSource, canDestination, canInstance, canPropMeth, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
    End Function

    Private Function CanProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String) As Boolean
        Dim sByte5 As String
        sByte5 = String.Empty
        Return CanProtocol(canID, canSource, canDestination, canInstance, canPropMeth, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
    End Function

    Private Function CanProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim returnValue As Boolean
        returnValue = False
        If EmptyRcvQueue() = False Then
            Me.Uninitialize()
            Me.Initialize(TPCANBaudrate.PCAN_BAUD_250K, TPCANType.PCAN_TYPE_ISA)
            If EmptyRcvQueue() = False Then
                Return False
            End If
        End If

        Select Case canID
            Case CAPPLICATION
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CApplicationHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CLIGHTCTRL
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CLightControlHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CDEBUG
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CDebugHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CFILTERWHEEL
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CFilterWheelHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CNAMEPLATE
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CNamePlateHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CTEMP
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CTempHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CBULBMANAGMENT
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CBulbManagmentHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CERROR
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CErrorHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case CBOOTLOADER
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CBootloaderHandling(canID, canPropMeth, canInstance, canFeat, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
            Case Else
        End Select
        Return returnValue
    End Function
#End Region
#Region "L2C Protokol"
    Private Function L2CProtocol(canID As String, canSource As Integer, canDestination As Integer, canInstance As Integer, canPropMeth As Integer, canFeat As Integer, ByRef data() As Byte)
        Dim returnValue As Boolean
        returnValue = False
        If EmptyRcvQueue() = False Then
            Return False
        End If
        Select Case canID
            Case CERROR
                canID = Hex((Convert.ToUInt64(canID, 16) And &HFF801FFF) Or (canSource << 13) Or (canDestination << 18))
                returnValue = CErrorHandlingL2C(canID, canPropMeth, canInstance, canFeat, data)
        End Select
        Return returnValue
    End Function
#End Region

#Region "C....HandlingL2C"
    Private Function CErrorHandlingL2C(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef data() As Byte)
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CErrorHandlingL2C = False
        GetCommand(eProtocol.GENERICFCTMODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCErrorP.EXTENDEDDATA
                Select Case feat
                    Case eFeatureP.GETSTATIC
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, data(0).ToString("X2"), data(1).ToString("X2"))
                        System.Threading.Thread.Sleep(100)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, data) = True Then
                            CErrorHandlingL2C = canCorrect
                        End If
                End Select
        End Select
    End Function
#End Region

#Region "C....Handling"
    Private Function CLightControlHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CLightControlHandling = False
        GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCLightCtrlP.ONOFFSTATE
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CLightControlHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CLightControlHandling = canCorrect
                        End If
                    Case Else
                End Select
            Case eCLightCtrlP.LIGHTVALUE
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CLightControlHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1) = True Then
                            CLightControlHandling = canCorrect
                        End If
                    Case Else
                End Select
            Case eCLightCtrlP.COLORTEMP
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        If String.IsNullOrEmpty(sByte2) Then
                            stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1)
                        ElseIf String.IsNullOrEmpty(sByte3) Then
                            stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2)
                        Else
                            stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3)
                        End If
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CLightControlHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CLightControlHandling = canCorrect
                        End If
                End Select
            Case eCLightCtrlP.VELOCITY
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CLightControlHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CLightControlHandling = canCorrect
                        End If
                End Select
            Case eCLightCtrlP.BULBCURRENT
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CLightControlHandling = canCorrect
                        End If
                End Select
            Case eCLightCtrlP.BULBVOLTAGE
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CLightControlHandling = canCorrect
                        End If
                End Select
            Case eCLightCtrlP.TYPO
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CLightControlHandling = canCorrect
                        End If
                End Select
            Case eCLightCtrlP.FLASHTIMER
                Select Case feat
                    Case eFeatureP.SETSTATIC
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) Then
                            CLightControlHandling = canCorrect
                        End If
                End Select
            Case eCLightCtrlM.FLASH
                Select Case feat
                    Case eFeatureM.EXECUTE0
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) Then
                            CLightControlHandling = canCorrect
                        End If
                End Select
            Case Else
        End Select
    End Function

    Private Function CTempHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CTempHandling = False
        GetCommand(eProtocol.GENERICFCTMODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCTempP.TEMPERATURE
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CTempHandling = canCorrect
                        End If
                End Select
        End Select
    End Function

    Private Function CNamePlateHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CNamePlateHandling = False
        GetCommand(eProtocol.GENERICFCTMODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCNamePlateP.APPLICATIONVERSION
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CNamePlateHandling = canCorrect
                        End If
                End Select
            Case eCNamePlateP.DEVICESNANDREV
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CNamePlateHandling = canCorrect
                        End If
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CNamePlateHandling = canCorrect
                        End If
                End Select
            Case eCNamePlateM.OPEN
                Select Case feat
                    Case eFeatureM.EXECUTE0
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CNamePlateHandling = canCorrect
                        End If
                End Select
            Case eCNamePlateM.CLOSE
                Select Case feat
                    Case eFeatureM.EXECUTE0
                        System.Threading.Thread.Sleep(700)
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        System.Threading.Thread.Sleep(2000)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CNamePlateHandling = canCorrect
                        End If
                End Select
        End Select
    End Function

    Private Function CFilterWheelHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CFilterWheelHandling = False
        GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCFilterWheelP.FILTERPOS
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        System.Threading.Thread.Sleep(1000)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CFilterWheelHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CFilterWheelHandling = canCorrect
                        End If
                End Select
        End Select
    End Function

    Private Function CErrorHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CErrorHandling = False
        GetCommand(eProtocol.GENERICFCTMODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCErrorP.LASTERROR
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        System.Threading.Thread.Sleep(100)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CErrorHandling = canCorrect
                        End If
                End Select
            Case eCErrorP.ERRORQUEUE
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CErrorHandling = canCorrect
                        End If
                End Select
            Case eCErrorP.EXTENDEDDATA
                Select Case feat
                    Case eFeatureP.GETSTATIC
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CErrorHandling = canCorrect
                        End If
                End Select
        End Select
    End Function

    Private Function CBulbManagmentHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CBulbManagmentHandling = False
        GetCommand(eProtocol.GENERICFCTMODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCBulbManagmentP.BURNTIME
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3, sByte4)
                        Threading.Thread.Sleep(100)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CBulbManagmentHandling = canCorrect
                        End If
                    Case eFeatureP.SETINIT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3, sByte4)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CBulbManagmentHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4) = True Then
                            CBulbManagmentHandling = canCorrect
                        End If
                    Case eFeatureP.GETINIT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4) = True Then
                            CBulbManagmentHandling = canCorrect
                        End If
                End Select
        End Select
    End Function

    Private Function CApplicationHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CApplicationHandling = False
        GetCommand(eProtocol.GENERICFCTMODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCApplicationM.ASYNRESET
                Select Case feat
                    Case eFeatureM.EXECUTE0
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CApplicationHandling = canCorrect
                        End If
                End Select
            Case eCApplicationM.SUPERR9MODE
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CApplicationHandling = canCorrect
                        End If
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CApplicationHandling = canCorrect
                        End If
                End Select
        End Select
    End Function

    Private Function CBootloaderHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0, ND00 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND00 = String.Empty
        ND1 = String.Empty
        CBootloaderHandling = False
        GetCommand(eProtocol.GENERICBASEMODUL, instance, prop, feat, ND1, ND0)
        ND00 = Convert.ToString(Convert.ToUInt16(ND0, 16) + 1, 16)
        Select Case prop
            Case eCBootloaderP.BOOTLOADER
                Select Case feat
                    Case eFeatureP.GETMIN
                        stsResult = MyBase.SendCanExtMsg(canID, ND00, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CBootloaderHandling = canCorrect
                        End If
                    Case eFeatureP.SETMIN
                        stsResult = MyBase.SendCanExtMsg(canID, ND00, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CBootloaderHandling = canCorrect
                        End If
                End Select
        End Select
    End Function

    Private Function CDebugHandling(canID As String, prop As Integer, instance As Integer, feat As Integer, ByRef sByte0 As String, ByRef sByte1 As String, ByRef sByte2 As String, ByRef sByte3 As String, ByRef sByte4 As String, ByRef sByte5 As String) As Boolean
        Dim stsResult As TPCANStatus
        Dim ND1, ND0 As String
        Dim canCorrect As Boolean
        ND0 = String.Empty
        ND1 = String.Empty
        CDebugHandling = False
        GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, instance, prop, feat, ND1, ND0)
        Select Case prop
            Case eCDebugP.FAILSAFETEST
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.CALIBRATIONDATARGB
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CDebugHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CDebugHandling = canCorrect
                        End If
                    Case eFeatureP.SETINIT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.BULBCURRENTRGB
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.PHOTODIODERGB
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.BULBVOLTAGERGB
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.SERIALNUMBERSRGB
                Select Case feat
                    Case eFeatureP.SETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
                        Threading.Thread.Sleep(10)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect) = True Then
                            CDebugHandling = canCorrect
                        End If
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1)
                        System.Threading.Thread.Sleep(20)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.VERSIONNUMBERCMS
                Select Case feat
                    Case eFeatureP.GETSTATIC
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.RAWPOSITION
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.INTENSITYANDCOLORTEMP
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugP.SYSTEMSETTINGS
                Select Case feat
                    Case eFeatureP.GETCURRENT
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0, sByte1, sByte2, sByte3) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugM.OPENRGB
                Select Case feat
                    Case eFeatureM.EXECUTE0
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugM.WIPE
                Select Case feat
                    Case eFeatureM.EXECUTE0
                        System.Threading.Thread.Sleep(500)
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1, sByte0, sByte1, sByte2, sByte3)
                        System.Threading.Thread.Sleep(2500)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
            Case eCDebugM.CLOSERGB
                Select Case feat
                    Case eFeatureM.EXECUTE0
                        System.Threading.Thread.Sleep(700)
                        stsResult = MyBase.SendCanExtMsg(canID, ND0, ND1)
                        System.Threading.Thread.Sleep(2000)
                        If CheckAnswerAndReturnValues(stsResult, ND0, ND1, canCorrect, sByte0) = True Then
                            CDebugHandling = canCorrect
                        End If
                End Select
        End Select
    End Function
#End Region
#Region "Generic Functions"
    ' generic function to generate byte 0 and 1
    Private Function GetCommand(ByVal Protocol As Integer, ByVal Instance As Integer, ByVal Properties As Integer, ByVal Feature As Integer, ByRef ND1 As String, ByRef ND0 As String) As Boolean
        Dim ND10 As UInt16 = &H0
        GetCommand = False

        ND10 = ND10 Or (Convert.ToUInt16(Instance) << 13)
        ND10 = ND10 Or (Convert.ToUInt16(Protocol) << 11)
        ND10 = ND10 Or (Convert.ToUInt16(Properties) << 5)
        ND10 = ND10 Or (Convert.ToUInt16(Feature) << 1)
        ND1 = Convert.ToString(ND10 >> 8, 16)
        ND0 = Convert.ToString(ND10 And &HFF, 16)

        GetCommand = True
    End Function
#End Region


#Region "CApplication"
    Public Function ExecuteRestart(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = "02"
        returnValue = CanProtocol(CAPPLICATION, CAN_SOURCE, canDestination, 0, eCApplicationM.ASYNRESET, eFeatureM.EXECUTE0, sByte0)
        If (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function SetSuperR9(ByVal canDestination As Integer) As Boolean
        Dim sData0 As String
        sData0 = "01"
        Return CanProtocol(CAPPLICATION, CAN_SOURCE, canDestination, 0, eCApplicationM.SUPERR9MODE, eFeatureP.SETCURRENT, sData0)
    End Function

    Public Function ResetSuperR9(ByVal canDestination As Integer) As Boolean
        Dim sData0 As String
        sData0 = "00"
        Return CanProtocol(CAPPLICATION, CAN_SOURCE, canDestination, 0, eCApplicationM.SUPERR9MODE, eFeatureP.SETCURRENT, sData0)
    End Function

#End Region

#Region "CBootloader"
    Public Function GetBootloaderVersion(ByVal canDestination As Integer, ByRef build As UInt32, ByRef main_rel As UInt16, ByRef sub_rel As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CBOOTLOADER, CAN_SOURCE, canDestination, 0, eCBootloaderP.BOOTLOADER, eFeatureP.GETMIN, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            main_rel = Convert.ToUInt16(sByte0)
            sub_rel = Convert.ToUInt16(sByte1)
            build = Convert.ToUInt16(sByte5, 16) * 256 * 256 * 256 + Convert.ToUInt16(sByte4, 16) * 256 * 256 + Convert.ToUInt16(sByte3, 16) * 256 + Convert.ToUInt16(sByte2, 16)
        End If
        Return returnValue
    End Function
#End Region

#Region "CBulbManagement"
    Public Function SetBurnTime(ByVal burnTime As Integer, ByVal canDestination As Integer) As Boolean
        Dim sData0, sData1, sData2, sData3, sData4 As String
        sData0 = "00"
        sData4 = Convert.ToString(burnTime \ 16777216, 16)
        sData3 = Convert.ToString((burnTime \ 65536) Mod 256, 16)
        sData2 = Convert.ToString((burnTime \ 256) Mod 256, 16)
        sData1 = Convert.ToString((burnTime Mod 256), 16)
        Return CanProtocol(CBULBMANAGMENT, CAN_SOURCE, canDestination, 0, eCBulbManagmentP.BURNTIME, eFeatureP.SETCURRENT, sData0, sData1, sData2, sData3, sData4)
    End Function

    Public Function GetBurnTime(ByVal canDestination As Integer, ByRef burnTime As Integer) As Boolean
        Dim sData0, sData1, sData2, sData3, sData4 As String
        Dim returnValue As Boolean
        sData0 = "00"
        sData1 = "00"
        sData2 = "00"
        sData3 = "00"
        sData4 = "00"
        returnValue = CanProtocol(CBULBMANAGMENT, CAN_SOURCE, canDestination, 0, eCBulbManagmentP.BURNTIME, eFeatureP.GETCURRENT, sData0, sData1, sData2, sData3, sData4)
        If returnValue = True Then
            burnTime = Convert.ToUInt32(sData1, 16) + Convert.ToUInt32(sData2, 16) * 256 + Convert.ToUInt32(sData3, 16) * 256 * 256 + Convert.ToUInt32(sData4, 16) * 256 * 256 * 256
        End If
        Return returnValue
    End Function

    Public Function GetBurnTimeUV(ByVal canDestination As Integer, ByRef burnTime As Integer) As Boolean
        Dim sData0, sData1, sData2, sData3, sData4 As String
        Dim returnValue As Boolean
        sData0 = "01"
        sData1 = "00"
        sData2 = "00"
        sData3 = "00"
        sData4 = "00"
        returnValue = CanProtocol(CBULBMANAGMENT, CAN_SOURCE, canDestination, 1, eCBulbManagmentP.BURNTIME, eFeatureP.GETCURRENT, sData0, sData1, sData2, sData3, sData4)
        If returnValue = True Then
            burnTime = Convert.ToUInt32(sData1, 16) + Convert.ToUInt32(sData2, 16) * 256 + Convert.ToUInt32(sData3, 16) * 256 * 256 + Convert.ToUInt32(sData4, 16) * 256 * 256 * 256
        End If
        Return returnValue
    End Function

    Public Function SetTotalBurnTime(ByVal burnTime As Integer, ByVal canDestination As Integer) As Boolean
        Dim sData0, sData1, sData2, sData3, sData4 As String
        sData0 = "00"
        sData4 = Convert.ToString(burnTime \ 16777216, 16)
        sData3 = Convert.ToString((burnTime \ 65536) Mod 256, 16)
        sData2 = Convert.ToString((burnTime \ 256) Mod 256, 16)
        sData1 = Convert.ToString((burnTime Mod 256), 16)
        Return CanProtocol(CBULBMANAGMENT, CAN_SOURCE, canDestination, 0, eCBulbManagmentP.BURNTIME, eFeatureP.SETINIT, sData0, sData1, sData2, sData3, sData4)
    End Function

    Public Function GetTotalBurnTime(ByVal canDestination As Integer, ByRef burnTime As Integer) As Boolean
        Dim sData0, sData1, sData2, sData3, sData4 As String
        Dim returnValue As Boolean
        sData0 = "00"
        sData1 = "00"
        sData2 = "00"
        sData3 = "00"
        sData4 = "00"
        returnValue = CanProtocol(CBULBMANAGMENT, CAN_SOURCE, canDestination, 0, eCBulbManagmentP.BURNTIME, eFeatureP.GETINIT, sData0, sData1, sData2, sData3, sData4)
        If returnValue = True Then
            burnTime = Convert.ToUInt32(sData1, 16) + Convert.ToUInt32(sData2, 16) * 256 + Convert.ToUInt32(sData3, 16) * 256 * 256 + Convert.ToUInt32(sData4, 16) * 256 * 256 * 256
        End If
        Return returnValue
    End Function

    Public Function GetInitBurnTime(ByVal canDestination As Integer, ByRef burnTime As Integer) As Boolean
        Dim sData0, sData1, sData2, sData3, sData4 As String
        Dim returnValue As Boolean
        sData0 = "00"
        sData1 = "00"
        sData2 = "00"
        sData3 = "00"
        sData4 = "00"
        returnValue = CanProtocol(CBULBMANAGMENT, CAN_SOURCE, canDestination, 0, eCBulbManagmentP.BURNTIME, eFeatureP.GETINIT, sData0, sData1, sData2, sData3, sData4)
        If returnValue = True Then
            burnTime = Convert.ToUInt32(sData1, 16) + Convert.ToUInt32(sData2, 16) * 256 + Convert.ToUInt32(sData3, 16) * 256 * 256 + Convert.ToUInt32(sData4, 16) * 256 * 256 * 256
        End If
        Return returnValue
    End Function
#End Region

#Region "CDebug"
    ''' <summary>
    ''' CDebug.CalibrationDataRGB.setCurrent [0x1832]
    ''' </summary>
    ''' <param name="LedSelection">0 = R ; 1 = G ; 2 = B</param>
    ''' <param name="CalibNr">0 = m ; 1 = n</param>
    ''' <param name="CalibrationData"></param>
    ''' <returns>TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function SetCalibrationDataRgbUv(ByVal canDestination As Integer, LedSelection As Integer, CalibNr As Integer, CalibrationData As Single) As Boolean
        Dim sLedSelection, sCalibNr, sData0, sData1, sData2, sData3 As String
        Dim bytes As Byte() = BitConverter.GetBytes(CalibrationData)
        Dim iInstance As Integer
        If (LedSelection = 3) Then
            LedSelection = 0
            iInstance = 1
        Else
            iInstance = 0
        End If
        sLedSelection = Convert.ToString(LedSelection, 16)
        sCalibNr = Convert.ToString(CalibNr, 16)
        sData0 = Convert.ToString(bytes(0), 16)
        sData1 = Convert.ToString(bytes(1), 16)
        sData2 = Convert.ToString(bytes(2), 16)
        sData3 = Convert.ToString(bytes(3), 16)
        Dim value As Boolean
        value = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, iInstance, eCDebugP.CALIBRATIONDATARGB, eFeatureP.SETCURRENT, sLedSelection, sCalibNr, sData0, sData1, sData2, sData3)
        If value = False Then
            Return value
        Else
            Return value
        End If
    End Function

    ''' <summary>
    ''' CDebug.CalibrationDataRGB.setCurrent [0x1832]
    ''' </summary>
    ''' <param name="LedSelection">0 = R ; 1 = G ; 2 = B</param>
    ''' <param name="CalibNr">0 = m ; 1 = n</param>
    ''' <param name="CalibrationData"></param>
    ''' <returns>TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>

    Public Function SetCalibrationDataRgbUv(ByVal canDestination As Integer, LedSelection As Integer, CalibNr As Integer, CalibrationData As UShort) As Boolean
        Dim sLedSelection, sCalibNr, sData0, sData1, sData2, sData3 As String
        Dim bytes As Byte() = BitConverter.GetBytes(CalibrationData)
        Dim iInstance As Integer
        If (LedSelection = 3) Then
            LedSelection = 0
            iInstance = 1
        Else
            iInstance = 0
        End If
        sLedSelection = Convert.ToString(LedSelection, 16)
        sCalibNr = Convert.ToString(CalibNr, 16)
        sData0 = Convert.ToString(bytes(0), 16)
        sData1 = Convert.ToString(bytes(1), 16)
        sData2 = "00"
        sData3 = "00"
        Return CanProtocol(CDEBUG, CAN_SOURCE, canDestination, iInstance, eCDebugP.CALIBRATIONDATARGB, eFeatureP.SETCURRENT, sLedSelection, sCalibNr, sData0, sData1, sData2, sData3)
    End Function

    ''' <summary>
    ''' CDebug.SerialNumbers.setCurrent [0xUXYZ]
    ''' </summary>
    ''' <returns>TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function SetSerialNumber(ByVal canDestination As Integer, BoardNumber As Integer, iInstance As Integer, DataSelect As Integer, SerialData As UInt32) As Boolean
        Dim sBoardNumber, sDataSelect, sData0, sData1, sData2, sData3 As String
        sBoardNumber = Convert.ToString(BoardNumber, 16)
        sDataSelect = Convert.ToString(DataSelect, 16)
        sData3 = Convert.ToString(SerialData \ 16777216, 16)
        sData2 = Convert.ToString((SerialData \ 65536) Mod 256, 16)
        sData1 = Convert.ToString((SerialData \ 256) Mod 256, 16)
        sData0 = Convert.ToString(SerialData Mod 256, 16)
        Return CanProtocol(CDEBUG, CAN_SOURCE, canDestination, iInstance, eCDebugP.SERIALNUMBERSRGB, eFeatureP.SETCURRENT, sBoardNumber, sDataSelect, sData0, sData1, sData2, sData3)
    End Function

    ''' <summary>
    ''' CDebug.CalibrationDataRGB.getCurrent [0x1822]
    ''' </summary>
    ''' <returns>Calibration data and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetSerialNumber(ByVal canDestination As Integer, ByVal BoardNumber As Integer, ByVal iInstance As Integer, ByVal DataSelect As Integer, ByRef SerialData As UInt32) As Boolean
        Dim returnValue As Boolean
        Dim sData0, sData1, sData2, sData3 As String
        sData0 = String.Empty
        sData1 = String.Empty
        sData2 = String.Empty
        sData3 = String.Empty
        Dim sBoardNumber, sDataSelect As String
        sBoardNumber = Convert.ToString(BoardNumber, 16)
        sDataSelect = Convert.ToString(DataSelect, 16)

        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, iInstance, eCDebugP.SERIALNUMBERSRGB, eFeatureP.GETCURRENT, sBoardNumber, sDataSelect, sData0, sData1, sData2, sData3)
        If returnValue = True Then
            SerialData = Convert.ToUInt32(sData0, 16)
            SerialData += (Convert.ToUInt32(sData1, 16) * 256)
            SerialData += (Convert.ToUInt32(sData2, 16) * 65536)
            SerialData += (Convert.ToUInt32(sData3, 16) * 16777216)
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CDebug.CalibrationDataRGB.getCurrent [0x1822]
    ''' </summary>
    ''' <param name="LedSelection">0 = R ; 1 = G ; 2 = B; 3 = UV</param>
    ''' <param name="CalibNr">0 = m ; 1 = n</param>
    ''' <param name="CalibrationData"></param>
    ''' <returns>Calibration data and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetCalibrationDataRgbUv(ByVal canDestination As Integer, LedSelection As Integer, CalibNr As Integer, ByRef CalibrationData As Single) As Boolean
        Dim returnValue As Boolean
        Dim sLedSelection, sCalibNr, sData0, sData1, sData2, sData3 As String
        sData0 = String.Empty
        sData1 = String.Empty
        sData2 = String.Empty
        sData3 = String.Empty
        Dim bytes As Byte() = BitConverter.GetBytes(CalibrationData)
        Dim iInstance As Integer
        If (LedSelection = 3) Then
            LedSelection = 0
            iInstance = 1
        Else
            iInstance = 0
        End If
        sLedSelection = Convert.ToString(LedSelection, 16)
        sCalibNr = Convert.ToString(CalibNr, 16)

        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, iInstance, eCDebugP.CALIBRATIONDATARGB, eFeatureP.GETCURRENT, sLedSelection, sCalibNr, sData0, sData1, sData2, sData3)
        If returnValue = True Then
            bytes(0) = Convert.ToByte(sData0, 16)
            bytes(1) = Convert.ToByte(sData1, 16)
            bytes(2) = Convert.ToByte(sData2, 16)
            bytes(3) = Convert.ToByte(sData3, 16)
            CalibrationData = BitConverter.ToSingle(bytes, 0)
        End If
        Return returnValue
    End Function

    Public Function GetCalibrationDataRgbUv(ByVal canDestination As Integer, LedSelection As Integer, CalibNr As Integer, ByRef CalibrationData As UShort) As Boolean
        Dim returnValue As Boolean
        Dim sLedSelection, sCalibNr, sData0, sData1, sData2, sData3 As String
        sData0 = String.Empty
        sData1 = String.Empty
        sData2 = String.Empty
        sData3 = String.Empty
        Dim bytes As Byte() = BitConverter.GetBytes(CalibrationData)
        Dim iInstance As Integer
        If (LedSelection = 3) Then
            LedSelection = 0
            iInstance = 1
        Else
            iInstance = 0
        End If
        sLedSelection = Convert.ToString(LedSelection, 16)
        sCalibNr = Convert.ToString(CalibNr, 16)

        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, iInstance, eCDebugP.CALIBRATIONDATARGB, eFeatureP.GETCURRENT, sLedSelection, sCalibNr, sData0, sData1, sData2, sData3)
        If returnValue = True Then
            bytes(0) = Convert.ToByte(sData0, 16)
            bytes(1) = Convert.ToByte(sData1, 16)
            CalibrationData = BitConverter.ToUInt16(bytes, 0)
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CDebug.BulbCurrentRGB.getCurrent [0x1882]
    ''' </summary>
    ''' <returns>RGB [DAC-values] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetDACBulbCurrentRGB(ByVal canDestination As Integer, ByRef ui1 As UInt16, ByRef ui2 As UInt16, ByRef ui3 As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.BULBCURRENTRGB, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            ui1 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
            ui2 = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16))
            ui3 = (Convert.ToInt32(sByte5, 16) * 256 + Convert.ToInt32(sByte4, 16))
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CDebug.BulbCurrentUV.getCurrent [0x3882]
    ''' </summary>
    ''' <returns>RGB [DAC-values] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetDACBulbCurrentUV(ByVal canDestination As Integer, ByRef ui4 As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 1, eCDebugP.BULBCURRENTRGB, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            ui4 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CDebug.GetPDRGB.getCurrent [0x182]
    ''' </summary>
    ''' <returns>RGB [DAC-values] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetPDRGB(ByVal canDestination As Integer, ByRef ui1 As UInt16, ByRef ui2 As UInt16, ByRef ui3 As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.PHOTODIODERGB, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            ui1 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
            ui2 = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16))
            ui3 = (Convert.ToInt32(sByte5, 16) * 256 + Convert.ToInt32(sByte4, 16))
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CDebug.GetPDUVSum.getCurrent [0x182]
    ''' </summary>
    ''' <returns>UV Sum [ADC-values] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetPDUVSum(ByVal canDestination As Integer, ByRef ui1 As UInt16, ByRef ui2 As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 1, eCDebugP.PHOTODIODERGB, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            ui1 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
            ui2 = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16))
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CDebug.BulbVoltageRGB.getCurrent [0x1862]
    ''' </summary>
    ''' <returns>RGB [DAC-values] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetDACBulbVoltageRGB(ByVal canDestination As Integer, ByRef ui1 As UInt16, ByRef ui2 As UInt16, ByRef ui3 As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.BULBVOLTAGERGB, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            ui1 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
            ui2 = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16))
            ui3 = (Convert.ToInt32(sByte5, 16) * 256 + Convert.ToInt32(sByte4, 16))
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CDebug.BulbVoltageRGB.getCurrent [0x1862]
    ''' </summary>
    ''' <returns>RGB [DAC-values] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetDACBulbVoltageUV(ByVal canDestination As Integer, ByRef ui4 As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 1, eCDebugP.BULBVOLTAGERGB, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            ui4 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
        End If
        Return returnValue
    End Function


    'OpenRGB.Execute0
    '0x1C00
    Public Function ExecuteOpenRGB(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = "D8"
        sByte1 = "F5"
        sByte2 = "23"
        sByte3 = "A7"
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugM.OPENRGB, eFeatureM.EXECUTE0, sByte0, sByte1, sByte2, sByte3)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    'OpenRGB.Execute0
    '0x1C00
    Public Function ExecuteOpenUV(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = "D8"
        sByte1 = "F5"
        sByte2 = "23"
        sByte3 = "A7"
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 1, eCDebugM.OPENRGB, eFeatureM.EXECUTE0, sByte0, sByte1, sByte2, sByte3)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function ExecuteOpenDailyCalib(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = "D8"
        sByte1 = "F5"
        sByte2 = "23"
        sByte3 = "A7"
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 3, eCDebugM.OPENRGB, eFeatureM.EXECUTE0, sByte0, sByte1, sByte2, sByte3)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    'Wipe.Execute0
    '0x1C40
    Public Function ExecuteWipeMain(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = "D8"
        sByte1 = "F5"
        sByte2 = "23"
        sByte3 = "A7"
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugM.WIPE, eFeatureM.EXECUTE0, sByte0, sByte1, sByte2, sByte3)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    'Wipe.Execute0
    '0x1C40
    Public Function ExecuteWipeUV(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = "D8"
        sByte1 = "F5"
        sByte2 = "23"
        sByte3 = "A7"
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 1, eCDebugM.WIPE, eFeatureM.EXECUTE0, sByte0, sByte1, sByte2, sByte3)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    'CloseRGB.Execute0
    '0x1C60
    Public Function ExecuteCloseRGB(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugM.CLOSERGB, eFeatureM.EXECUTE0, sByte0)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    'CloseRGB.Execute0
    '0x1C60
    Public Function ExecuteCloseUV(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 1, eCDebugM.CLOSERGB, eFeatureM.EXECUTE0, sByte0)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function ExecuteCloseDailyCalib(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 3, eCDebugM.CLOSERGB, eFeatureM.EXECUTE0, sByte0)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function GetSoftwareVersionCMS(ByVal canDestination As Integer, ByRef sub_rel As UInt16, ByRef main_rel As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.VERSIONNUMBERCMS, eFeatureP.GETSTATIC, sByte0, sByte1, sByte2, sByte3)
        If returnValue = True Then
            main_rel = Convert.ToUInt16(sByte1, 16) * 256 + Convert.ToUInt16(sByte0, 16)
            sub_rel = Convert.ToUInt16(sByte3, 16) * 256 + Convert.ToUInt16(sByte2, 16)
        End If
        Return returnValue
    End Function

    Public Function GetFWPositionRaw(ByVal canDestination As Integer, ByRef position As UInt32) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 2, eCDebugP.RAWPOSITION, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2)
        position = Convert.ToUInt16(sByte0, 16) + Convert.ToUInt16(sByte1, 16) * 256 + Convert.ToUInt16(sByte2, 16) * 256 * 256
        Return returnValue
    End Function

    Public Function GetSystemSettings(ByVal canDestination As Integer, ByRef uvInstalled As Boolean, ByRef decelerate As UInt16, ByRef fwImplemented As Boolean, ByRef testMode As Boolean, ByRef analogConDetect As Boolean, ByRef canConDetect As Boolean)
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.SYSTEMSETTINGS, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3)
        uvInstalled = (Convert.ToUInt16(sByte0, 16) >> 0) And 1
        decelerate = (Convert.ToUInt16(sByte0, 16) >> 1) And 3
        fwImplemented = (Convert.ToUInt16(sByte0, 16) >> 3) And 1
        testMode = (Convert.ToUInt16(sByte0, 16) >> 4) And 1
        analogConDetect = (Convert.ToUInt16(sByte0, 16) >> 5) And 1
        canConDetect = (Convert.ToUInt16(sByte0, 16) >> 6) And 1
        Return returnValue
    End Function

    'needs to be reworked
    Public Function GetRawPositionFW(ByVal canDestination As Integer, ByRef rawPosition As UInt32) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.RAWPOSITION, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3)
        rawPosition = Convert.ToUInt32(sByte0, 16) + Convert.ToUInt32(sByte1, 16) * 256 + Convert.ToUInt32(sByte2, 16) * 256 * 256 + Convert.ToUInt32(sByte3, 16) * 256 * 256 * 256
        Return returnValue
    End Function

    Public Function SetDailyCalibrationData(ByVal canDestination As Integer, LedSelection As Integer, CalibNr As Integer, CalibrationData As Single) As Boolean
        Dim sLedSelection, sCalibNr, sData0, sData1, sData2, sData3 As String
        Dim bytes As Byte() = BitConverter.GetBytes(CalibrationData)
        sLedSelection = Convert.ToString(LedSelection, 16)
        sCalibNr = Convert.ToString(CalibNr, 16)
        sData0 = Convert.ToString(bytes(0), 16)
        sData1 = Convert.ToString(bytes(1), 16)
        sData2 = Convert.ToString(bytes(2), 16)
        sData3 = Convert.ToString(bytes(3), 16)
        Return CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 3, eCDebugP.CALIBRATIONDATARGB, eFeatureP.SETCURRENT, sLedSelection, sCalibNr, sData0, sData1, sData2, sData3)
    End Function

    Public Function GetDailyCalibrationData(ByVal canDestination As Integer, LedSelection As Integer, CalibNr As Integer, ByRef CalibrationData As Single) As Boolean
        Dim returnValue As Boolean
        Dim sLedSelection, sCalibNr, sData0, sData1, sData2, sData3 As String
        sData0 = String.Empty
        sData1 = String.Empty
        sData2 = String.Empty
        sData3 = String.Empty
        Dim bytes As Byte() = BitConverter.GetBytes(CalibrationData)
        sLedSelection = Convert.ToString(LedSelection, 16)
        sCalibNr = Convert.ToString(CalibNr, 16)

        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 3, eCDebugP.CALIBRATIONDATARGB, eFeatureP.GETCURRENT, sLedSelection, sCalibNr, sData0, sData1, sData2, sData3)
        If returnValue = True Then
            bytes(0) = Convert.ToByte(sData0, 16)
            bytes(1) = Convert.ToByte(sData1, 16)
            bytes(2) = Convert.ToByte(sData2, 16)
            bytes(3) = Convert.ToByte(sData3, 16)
            CalibrationData = BitConverter.ToSingle(bytes, 0)
        End If
        Return returnValue
    End Function

    Public Function SetInitDailyCalibration(ByVal canDestination As Integer) As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        Return CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 3, eCDebugP.CALIBRATIONDATARGB, eFeatureP.SETINIT, sByte0)
        If (sByte0 = "00") Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetIntensityAndColorTempInDebugForLabview(ByVal canDestination As Integer, ByRef intensity As Integer, ByRef color1 As Double, ByRef color2 As Double, ByRef color3 As Double) As Boolean
        Dim oDblColor As List(Of Double) = New List(Of Double)
        Dim returnValue As Boolean = False
        returnValue = GetIntensityAndColorTempInDebug(canDestination, intensity, oDblColor)
        color1 = -1
        color2 = -1
        color3 = -1
        Select Case oDblColor.Count
            Case 1
                color1 = oDblColor(0)
            Case 2
                color1 = oDblColor(0)
                color2 = oDblColor(1)
            Case 3
                color1 = oDblColor(0)
                color2 = oDblColor(1)
                color3 = oDblColor(2)
        End Select
        Return returnValue
    End Function

    Public Function GetIntensityAndColorTempInDebug(ByVal canDestination As Integer, ByRef intensity As Integer, ByRef oDblColor As List(Of Double)) As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.INTENSITYANDCOLORTEMP, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If (Not String.IsNullOrEmpty(sByte0)) Then
            intensity = Convert.ToInt32(sByte0, 16) + Convert.ToInt32(sByte1, 16) * 256
            oDblColor.Clear()
            If String.IsNullOrEmpty(sByte4) Then ' CCT
                oDblColor.Add(Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16))
                GetIntensityAndColorTempInDebug = True
            ElseIf String.IsNullOrEmpty(sByte5) Then ' RGB
                oDblColor.Add(Convert.ToInt32(sByte2, 16))
                oDblColor.Add(Convert.ToInt32(sByte3, 16))
                oDblColor.Add(Convert.ToInt32(sByte4, 16))
                GetIntensityAndColorTempInDebug = True
            Else ' cx/cy
                oDblColor.Add(CDbl((Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16)) / 1000.0))
                oDblColor.Add(CDbl((Convert.ToInt32(sByte5, 16) * 256 + Convert.ToInt32(sByte4, 16)) / 1000.0))
                GetIntensityAndColorTempInDebug = True
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Function SetFailSafeStatus(ByVal canDestination As Integer, ByVal Status As Boolean) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String

        If Status = True Then
            sByte0 = "01"
        Else
            sByte0 = "00"
        End If
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.FAILSAFETEST, eFeatureP.SETCURRENT, sByte0)
        Return returnValue
    End Function

    Public Function GetCCTPot(ByVal canDestination As Integer, ByRef position As UInt32) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        returnValue = CanProtocol(CDEBUG, CAN_SOURCE, canDestination, 0, eCDebugP.INTENSITYANDCOLORTEMP, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3)
        If returnValue Then
            position = Convert.ToUInt16(sByte2, 16) + Convert.ToUInt16(sByte3, 16) * 256
        End If
        Return returnValue
    End Function

#End Region

#Region "CError"
    Public Function GetLastError(ByVal canDestination As Integer, ByRef errNo As Short, ByRef fid As UInt16, ByRef crit As UInt16, ByRef smod As UInt16, ByRef instance As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty

        returnValue = CanProtocol(CERROR, CAN_SOURCE, canDestination, 0, eCErrorP.LASTERROR, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If (Not String.IsNullOrEmpty(sByte0)) Then
            If (sByte0 <> "00") Then
                errNo = Convert.ToInt16(sByte5 + sByte4, 16)
                fid = Convert.ToUInt16(sByte1 + sByte0, 16) Mod 512
                crit = Convert.ToUInt16(sByte3)
                smod = (Convert.ToUInt16(sByte1) \ (1 << 3)) Mod (2 << 2)
                instance = (Convert.ToUInt16(sByte1) \ (1 << 5)) Mod (2 << 3)
            End If
        End If
        'lastError = Convert.ToInt16(sByte0, 16) + Convert.ToInt16(sByte1, 16) * 256
        Return returnValue
    End Function

    Public Function GetLastError(ByVal canDestination As Integer, ByRef errorString As String) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CERROR, CAN_SOURCE, canDestination, 0, eCErrorP.LASTERROR, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        errorString = sByte1 + sByte0 + sByte5 + sByte4
        Return returnValue
    End Function

    Public Function GetErrorQueue(ByVal canDestination As Integer, ByRef errNo As Short, ByRef fid As UInt16, ByRef crit As UInt16, ByRef smod As UInt16, ByRef instance As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty

        returnValue = CanProtocol(CERROR, CAN_SOURCE, canDestination, 0, eCErrorP.ERRORQUEUE, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If (Not String.IsNullOrEmpty(sByte0)) Then
            If (sByte0 <> "00") Then
                errNo = Convert.ToInt16(sByte5 + sByte4, 16)
                fid = Convert.ToUInt16(sByte1 + sByte0, 16) Mod 512
                crit = Convert.ToUInt16(sByte3)
                smod = (Convert.ToUInt16(sByte1) \ (1 << 3)) Mod (2 << 2)
                instance = (Convert.ToUInt16(sByte1) \ (1 << 5)) Mod (2 << 3)
            End If
        End If
        Return returnValue
    End Function

    Public Function GetErrorQueue(ByVal canDestination As Integer, ByRef errorString As String) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CERROR, CAN_SOURCE, canDestination, 0, eCErrorP.ERRORQUEUE, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        errorString = sByte1 + sByte0 + sByte5 + sByte4
        Return returnValue
    End Function

    Public Function GetExtendedError(ByRef canDestination As Integer, ByRef errorString As String) As Boolean
        Dim returnValue As Boolean
        Dim data() As Byte
        ReDim data(1)
        errorString = String.Empty
        data(0) = &HC0
        data(1) = &H0
        returnValue = L2CProtocol(CERROR, CAN_SOURCE, canDestination, 0, eCErrorP.EXTENDEDDATA, eFeatureP.GETSTATIC, data)
        If data.Length = 1 Then
            errorString = String.Empty
        ElseIf data.Length = 38 Then
            errorString = "Line: " & (CUInt(data(4)) + CUInt(data(5)) * 256 + CUInt(data(6)) * 256 * 256 + CUInt(data(7)) * 256 * 256 * 256).ToString
            errorString &= "//File: " & System.Text.Encoding.ASCII.GetString(data, 12, 24)
        ElseIf data.Length = 17 Then
            errorString = "extended Error short, ask for support"
        ElseIf data.Length = 59 Then
            errorString = "extended Error long, ask for support"
        Else
            errorString = "extended Error " & data.Length.ToString() & ", ask for support"
        End If
        Return returnValue
    End Function

#End Region

#Region "CFilterWheel"
    Public Function GetFWPosition(ByVal canDestination As Integer, ByRef position As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CFILTERWHEEL, CAN_SOURCE, canDestination, 0, eCFilterWheelP.FILTERPOS, eFeatureP.GETCURRENT, sByte0)
        If returnValue = True Then
            position = Convert.ToUInt16(sByte0, 16)
        End If
        Return returnValue
    End Function

    Public Function SetFWPosition(ByVal canDestination As Integer, ByVal position As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = Convert.ToByte(position)
        returnValue = CanProtocol(CFILTERWHEEL, CAN_SOURCE, canDestination, 0, eCFilterWheelP.FILTERPOS, eFeatureP.SETCURRENT, sByte0)
        Return returnValue
    End Function

    Public Function GetFWSlot(ByVal canDestination As Integer, ByRef position As UInt32) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CFILTERWHEEL, CAN_SOURCE, canDestination, 0, eCFilterWheelP.FILTERPOS, eFeatureP.GETCURRENT, sByte0)
        If returnValue = True Then
            position = Convert.ToUInt32(sByte0, 16)
        End If
        Return returnValue
    End Function

    Public Function SetFWSlot(ByVal canDestination As Integer, ByVal position As Byte) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = Convert.ToString(position, 16)
        returnValue = CanProtocol(CFILTERWHEEL, CAN_SOURCE, canDestination, 0, eCFilterWheelP.FILTERPOS, eFeatureP.SETCURRENT, sByte0)
        Return returnValue
    End Function

    Public Function TurnMotorLeftEnd(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = Convert.ToString(0, 16)
        returnValue = CanProtocol(CFILTERWHEEL, CAN_SOURCE, canDestination, 0, eCFilterWheelP.FILTERPOS, eFeatureP.SETCURRENT, sByte0)
        Return returnValue
    End Function

    Public Function TurnMotorRightEnd(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = Convert.ToString(4, 16)
        returnValue = CanProtocol(CFILTERWHEEL, CAN_SOURCE, canDestination, 0, eCFilterWheelP.FILTERPOS, eFeatureP.SETCURRENT, sByte0)
        Return returnValue
    End Function

    Public Function GetFLTKnobPos(ByVal canDestination As Integer, ByRef position As UInt32) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty

        returnValue = CanProtocol(CFILTERWHEEL, CAN_SOURCE, canDestination, 0, eCFilterWheelP.FILTERPOS, eFeatureP.GETCURRENT, sByte0)
        If returnValue Then
            position = Convert.ToUInt16(sByte0, 16)
        End If
        Return returnValue
    End Function
#End Region

#Region "CLightCtrl"
    Public Function SetRgbOnState(canDestination As Integer, bstate As Boolean) As Boolean
        Dim sByte0 As String
        If bstate Then
            sByte0 = "01"
        Else
            sByte0 = "00"
        End If
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.ONOFFSTATE, eFeatureP.SETCURRENT, sByte0)
    End Function

    Public Function GetRgbOnState(ByVal canDestination As Integer, ByRef bState As Boolean) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.ONOFFSTATE, eFeatureP.GETCURRENT, sByte0)
        If sByte0 = "00" Then
            bState = True
        Else
            bState = False
        End If
        Return returnValue
    End Function

    Public Function SetUvOnState(canDestination As Integer, bState As Boolean) As Boolean
        Dim sByte0 As String
        If bState Then
            sByte0 = "01"
        Else
            sByte0 = "00"
        End If
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 1, eCLightCtrlP.ONOFFSTATE, eFeatureP.SETCURRENT, sByte0)
    End Function

    Public Function GetUvOnState(ByVal canDestination As Integer, ByRef bState As Boolean) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 1, eCLightCtrlP.ONOFFSTATE, eFeatureP.GETCURRENT, sByte0)
        If sByte0 = "00" Then
            bState = True
        Else
            bState = False
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CLightCtrl.LightValue.setCurrent [0x1812]
    ''' </summary>
    ''' <param name="rPercent">5-120 for 5%-120%</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function SetRgbIntensity(canDestination As Integer, rPercent As Double) As Boolean
        Dim iPercent, iPercent0, iPercent1 As Integer
        Dim sByte0, sByte1 As String

        If (rPercent < 5) Or (rPercent > 120) Then Throw New ArgumentOutOfRangeException

        iPercent = rPercent * 100
        iPercent0 = iPercent \ 256
        iPercent1 = iPercent Mod 256
        sByte1 = Convert.ToString(iPercent0, 16)
        sByte0 = Convert.ToString(iPercent1, 16)
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.LIGHTVALUE, eFeatureP.SETCURRENT, sByte0, sByte1)
    End Function


    ''' <summary>
    ''' CLightCtrl.LightValue.getCurrent [0xXXXX]
    ''' </summary>
    ''' <param name="dIntensity">5-120 for 5%-120%</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetRgbIntensity(ByVal canDestination As Integer, ByRef dIntensity As Double) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.LIGHTVALUE, eFeatureP.GETCURRENT, sByte0, sByte1)
        If returnValue = True Then
            dIntensity = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 100
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CLightCtrl.LightValue.setCurrent [0x1812]
    ''' </summary>
    ''' <param name="rPercent">5-100 for 5%-100%</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function SetUvIntensity(canDestination As Integer, rPercent As Double) As Boolean
        Dim iPercent, iPercent0, iPercent1 As Integer
        Dim sByte0, sByte1 As String

        If (rPercent < 5) Or (rPercent > 100) Then Throw New ArgumentOutOfRangeException

        iPercent = rPercent * 100
        iPercent0 = iPercent \ 256
        iPercent1 = iPercent Mod 256
        sByte1 = Convert.ToString(iPercent0, 16)
        sByte0 = Convert.ToString(iPercent1, 16)
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 1, eCLightCtrlP.LIGHTVALUE, eFeatureP.SETCURRENT, sByte0, sByte1)
    End Function

    ''' <summary>
    ''' CLightCtrl.LightValue.getCurrent [0xXXXX]
    ''' </summary>
    ''' <param name="dIntensity">5-100 for 5%-100%</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetUvIntensity(ByVal canDestination As Integer, ByRef dIntensity As Double) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 1, eCLightCtrlP.LIGHTVALUE, eFeatureP.GETCURRENT, sByte0, sByte1)
        If returnValue = True Then
            dIntensity = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 100
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CLightCtrl.TYPO.getCurrent [0xXXXX]
    ''' </summary>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetTYPO(ByVal canDestination As Integer, ByRef bt0 As Byte, ByRef bt1 As Byte, ByRef bt2 As Byte, ByRef bt3 As Byte) As Boolean
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.TYPO, eFeatureP.GETCURRENT, bt0, bt1, bt2, bt3)
    End Function

    ''' <summary>
    ''' CLightCtrl.ColorTemp.getCurrent [0xXXXX]
    ''' </summary>
    ''' <param name="oBtColor"></param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetColor(ByVal canDestination As Integer, ByRef oDblColor As List(Of Double)) As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.COLORTEMP, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3)
        If (Not String.IsNullOrEmpty(sByte0)) Then

            oDblColor.Clear()
            If String.IsNullOrEmpty(sByte2) Then ' CCT
                oDblColor.Add(Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
                GetColor = True
            ElseIf String.IsNullOrEmpty(sByte3) Then ' RGB
                oDblColor.Add(Convert.ToInt32(sByte0, 16))
                oDblColor.Add(Convert.ToInt32(sByte1, 16))
                oDblColor.Add(Convert.ToInt32(sByte2, 16))
                GetColor = True
            Else ' cx/cy
                oDblColor.Add(CDbl((Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 1000.0))
                oDblColor.Add(CDbl((Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16)) / 1000.0))
                GetColor = True
            End If
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' CLightCtrl.ColorTemp.setCurrent [0x1952]
    ''' </summary>
    ''' <param name="btRed">0-255</param>
    ''' <param name="btGreen">0-255</param>
    ''' <param name="btBlue">0-255</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function SetRGbColor(ByVal canDestination As Integer, ByVal btRed As Byte, ByVal btGreen As Byte, ByVal btBlue As Byte) As Boolean
        Dim sByte0, sByte1, sByte2 As String
        sByte0 = Convert.ToString(btRed, 16)
        sByte1 = Convert.ToString(btGreen, 16)
        sByte2 = Convert.ToString(btBlue, 16)
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.COLORTEMP, eFeatureP.SETCURRENT, sByte0, sByte1, sByte2)
    End Function

    ''' <summary>
    ''' CLightCtrl.ColorTemp.setCurrent [0x1952]
    ''' </summary>
    ''' <param name="btCx">0-1000</param>
    ''' <param name="btCy">0-1000</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function SetCxCyColor(ByVal canDestination As Integer, ByVal dCx As Double, ByVal dCy As Double) As Boolean
        Dim sCx0, sCx1, sCy0, sCy1 As String
        dCx = dCx * 1000.0
        dCy = dCy * 1000.0
        sCx0 = Convert.ToString(CInt(dCx) Mod 256, 16)
        sCx1 = Convert.ToString(CInt(dCx) \ 256, 16)
        sCy0 = Convert.ToString(CInt(dCy) Mod 256, 16)
        sCy1 = Convert.ToString(CInt(dCy) \ 256, 16)
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.COLORTEMP, eFeatureP.SETCURRENT, sCx0, sCx1, sCy0, sCy1)
    End Function

    ''' <summary>
    ''' CLightCtrl.ColorTemp.setCurrent [0x1952]
    ''' </summary>
    ''' <param name="btCCT">0-1000</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function SetCctColor(ByVal canDestination As Integer, ByVal btCCT As Integer) As Boolean
        Dim sCCT0, sCCT1 As String
        sCCT0 = Convert.ToString(btCCT Mod 256, 16)
        sCCT1 = Convert.ToString(btCCT \ 256, 16)
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.COLORTEMP, eFeatureP.SETCURRENT, sCCT0, sCCT1)
    End Function

    ''' <summary>
    ''' CLightCtrl.Velocity.setCurrent [0x]
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetVelocity(ByVal canDestination As Integer, ByVal vel As Integer) As Boolean
        Dim vel0, vel1, vel2, vel3 As String
        vel0 = "01"
        vel1 = "00"
        vel2 = Convert.ToString(vel Mod 256, 16)
        vel3 = Convert.ToString(vel \ 256, 16)
        Return CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.VELOCITY, eFeatureP.SETCURRENT, vel0, vel1, vel2, vel3)
    End Function

    Public Function GetVelocity(ByVal canDestination As Integer, ByRef vel As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = "01"
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.VELOCITY, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3)
        If returnValue = True Then
            vel = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16))
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CLightCtrl.GetBulbCurrent [0x1882]
    ''' </summary>
    ''' <returns>RGB [A] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetBulbCurrentRGB(ByVal canDestination As Integer, ByRef d1 As Double, ByRef d2 As Double, ByRef d3 As Double) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.BULBCURRENT, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            d1 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 1000
            d2 = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16)) / 1000
            d3 = (Convert.ToInt32(sByte5, 16) * 256 + Convert.ToInt32(sByte4, 16)) / 1000
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CLightCtrl.GetBulbCurrent [0x1882]
    ''' </summary>
    ''' <returns>RGB [A] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetBulbCurrentUV(ByVal canDestination As Integer, ByRef d4 As Double) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 1, eCLightCtrlP.BULBCURRENT, eFeatureP.GETCURRENT, sByte0, sByte1)
        If returnValue = True Then
            d4 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 1000
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' CLightCtrl.BulbVoltage.getCurrent [0x1862]
    ''' </summary>
    ''' <returns>RGB [V] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetBulbVoltageRGB(ByVal canDestination As Integer, ByRef d1 As Double, ByRef d2 As Double, ByRef d3 As Double) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.BULBVOLTAGE, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            d1 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 100
            d2 = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16)) / 100
            d3 = (Convert.ToInt32(sByte5, 16) * 256 + Convert.ToInt32(sByte4, 16)) / 100
        End If
        Return returnValue
    End Function

    Public Function GetBulbVoltageUV(ByVal canDestination As Integer, ByRef d4 As Double) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 1, eCLightCtrlP.BULBVOLTAGE, eFeatureP.GETCURRENT, sByte0, sByte1)
        If returnValue = True Then
            d4 = (Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 100
        End If
        Return returnValue
    End Function

    Public Function ExecuteFlash(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlM.FLASH, eFeatureM.EXECUTE0)
        If (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function SetFlashProperties(ByVal canDestination As Integer, ByVal flashTime As UShort, ByVal delayedFlash As UShort, ByVal intensity As UShort)
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = Convert.ToString(flashTime Mod 256, 16)
        sByte1 = Convert.ToString(flashTime \ 256, 16)
        sByte2 = Convert.ToString(delayedFlash Mod 256, 16)
        sByte3 = Convert.ToString(delayedFlash \ 256, 16)
        sByte4 = Convert.ToString(intensity Mod 256, 16)
        sByte5 = Convert.ToString(intensity \ 256, 16)
        returnValue = CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.FLASHTIMER, eFeatureP.SETSTATIC, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function GetIntensityAndColorTemp(ByVal canDestination As Integer, ByRef intensity As Integer, ByRef oDblColor As List(Of Double)) As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        CanProtocol(CLIGHTCTRL, CAN_SOURCE, canDestination, 0, eCLightCtrlP.COLORTEMP, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If (Not String.IsNullOrEmpty(sByte0)) Then
            intensity = Convert.ToInt32(sByte0, 16) + Convert.ToInt32(sByte1, 16) * 256
            oDblColor.Clear()
            If String.IsNullOrEmpty(sByte2) Then ' CCT
                oDblColor.Add(Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16))
                GetIntensityAndColorTemp = True
            ElseIf String.IsNullOrEmpty(sByte3) Then ' RGB
                oDblColor.Add(Convert.ToInt32(sByte0, 16))
                oDblColor.Add(Convert.ToInt32(sByte1, 16))
                oDblColor.Add(Convert.ToInt32(sByte2, 16))
                GetIntensityAndColorTemp = True
            Else ' cx/cy
                oDblColor.Add(CDbl((Convert.ToInt32(sByte1, 16) * 256 + Convert.ToInt32(sByte0, 16)) / 1000.0))
                oDblColor.Add(CDbl((Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16)) / 1000.0))
                GetIntensityAndColorTemp = True
            End If
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#Region "CNamePlate"

    Public Function ExecuteOpenNamePlate(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = "D8"
        sByte1 = "F5"
        sByte2 = "23"
        sByte3 = "A7"
        returnValue = CanProtocol(CNAMEPLATE, CAN_SOURCE, canDestination, 0, eCNamePlateM.OPEN, eFeatureM.EXECUTE0, sByte0, sByte1, sByte2, sByte3)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function ExecuteCloseNamePlate(ByVal canDestination As Integer) As Boolean
        Dim returnValue As Boolean
        Dim sByte0 As String
        sByte0 = String.Empty
        returnValue = CanProtocol(CNAMEPLATE, CAN_SOURCE, canDestination, 0, eCNamePlateM.CLOSE, eFeatureM.EXECUTE0, sByte0)
        If (sByte0 = "00") And (returnValue = True) Then
            Return True
        End If
        Return False
    End Function

    Public Function GetSoftwareVersion(ByVal canDestination As Integer, ByRef build As UInt16, ByRef sub_rel As UInt16, ByRef main_rel As UInt16, ByRef state As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        returnValue = CanProtocol(CNAMEPLATE, CAN_SOURCE, canDestination, 0, eCNamePlateP.APPLICATIONVERSION, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3)
        If returnValue = True Then
            build = Convert.ToUInt16(sByte0, 16)
            sub_rel = Convert.ToUInt16(sByte1, 16)
            main_rel = Convert.ToUInt16(sByte2, 16)
            state = Convert.ToUInt16(sByte3, 16)
        End If
        Return returnValue
    End Function

    Public Function GetZeissVersionAndSN(ByVal canDestination As Integer, ByRef sn As UInt32, ByRef main_rel As UInt16, ByRef sub_rel As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = String.Empty
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        sByte4 = String.Empty
        sByte5 = String.Empty
        returnValue = CanProtocol(CNAMEPLATE, CAN_SOURCE, canDestination, 0, eCNamePlateP.DEVICESNANDREV, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        If returnValue = True Then
            main_rel = Convert.ToUInt16(sByte1)
            sub_rel = Convert.ToUInt16(sByte0)
            sn = Convert.ToUInt16(sByte5, 16) * 256 * 256 * 256 + Convert.ToUInt16(sByte4, 16) * 256 * 256 + Convert.ToUInt16(sByte3, 16) * 256 + Convert.ToUInt16(sByte2, 16)
        End If
        Return returnValue
    End Function

    Public Function SetZeissVersionAndSN(ByVal canDestination As Integer, ByRef sn As UInt32, ByRef main_rel As UInt16, ByRef sub_rel As UInt16) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3, sByte4, sByte5 As String
        sByte0 = Convert.ToString(sub_rel, 16)
        sByte1 = Convert.ToString(main_rel, 16)
        sByte5 = Convert.ToString(sn \ 16777216, 16)
        sByte4 = Convert.ToString((sn \ 65536) Mod 256, 16)
        sByte3 = Convert.ToString((sn \ 256) Mod 256, 16)
        sByte2 = Convert.ToString(sn Mod 256, 16)
        returnValue = CanProtocol(CNAMEPLATE, CAN_SOURCE, canDestination, 0, eCNamePlateP.DEVICESNANDREV, eFeatureP.SETCURRENT, sByte0, sByte1, sByte2, sByte3, sByte4, sByte5)
        Return returnValue
    End Function

#End Region

#Region "CTemp"

    ''' <summary>
    ''' CTemp.Temperature.getCurrent [0x0802]
    ''' </summary>
    ''' <returns>RGB [degree Celsius] and TRUE if hardware returns no error</returns>
    ''' <remarks></remarks>
    Public Function GetTemperature(ByVal canDestination As Integer, ByVal tempSensor As Integer, ByRef result As Double) As Boolean
        Dim returnValue As Boolean
        Dim sByte0, sByte1, sByte2, sByte3 As String
        sByte0 = Convert.ToString(tempSensor, 16)
        sByte1 = String.Empty
        sByte2 = String.Empty
        sByte3 = String.Empty
        returnValue = CanProtocol(CTEMP, CAN_SOURCE, canDestination, 0, eCTempP.TEMPERATURE, eFeatureP.GETCURRENT, sByte0, sByte1, sByte2, sByte3)
        If returnValue = True Then
            result = (Convert.ToInt32(sByte3, 16) * 256 + Convert.ToInt32(sByte2, 16)) / 10
        End If
        Return returnValue
    End Function

#End Region

    'Public Function GetFWKnobPositionInAnalog(ByRef position As UInt32)
    '    Dim stsResult As TPCANStatus
    '    Dim CANMsg As TPCANMsg = Nothing

    '    GetFWKnobPositionInAnalog = False

    '    If EmptyRcvQueue() = False Then
    '        Return False
    '    End If

    '    Dim ND1, ND0 As String
    '    ND0 = String.empty
    '    ND1 = String.empty
    '    GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, 2, eCDebugP.RAWPOSITION, eFeatureP.GETCURRENT, ND1, ND0)
    '    'ID: 160C220D // DataLength: 6 // Data: 
    '    stsResult = MyBase.SendCanExtMsg(CDEBUG, ND0, ND1)

    '    If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '        System.Threading.Thread.Sleep(1000)
    '        stsResult = ReadNxtMsg(CANMsg)
    '        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '            If (CANMsg.DATA(0) = (Convert.ToByte(ND0, 16) + 1) And CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then
    '                position = CANMsg.DATA(2) + CANMsg.DATA(3) * 256 + CANMsg.DATA(4) * 256 * 256
    '                GetFWKnobPositionInAnalog = True
    '            End If
    '        End If
    '    End If
    'End Function

    ''' <summary>
    ''' CLightCtrl.ColorTemp.getCurrent [0xXXXX]
    ''' </summary>
    ''' <param name="btRed">0-255</param>
    ''' <param name="btGreen">0-255</param>
    ''' <param name="btBlue">0-255</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    'Public Function GetRGbColor(ByRef btRed As Byte, ByRef btGreen As Byte, ByRef btBlue As Byte) As Boolean
    '    Dim stsResult As TPCANStatus
    '    Dim CANMsg As TPCANMsg = Nothing

    '    GetRGbColor = False

    '    If EmptyRcvQueue() = False Then
    '        Return False
    '    End If

    '    Dim ND1, ND0 As String
    '    ND0 = String.empty
    '    ND1 = String.empty
    '    GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, 0, eCLightCtrlP.COLORTEMP, eFeatureP.GETCURRENT, ND1, ND0)

    '    stsResult = MyBase.SendCanExtMsg(CLIGHTCTRL, ND0, ND1)

    '    If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '        System.Threading.Thread.Sleep(100)
    '        stsResult = ReadNxtMsg(CANMsg)
    '        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '            If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then '0x53 And 0x19
    '                btRed = CANMsg.DATA(2)
    '                btGreen = CANMsg.DATA(3)
    '                btBlue = CANMsg.DATA(4)
    '                GetRGbColor = True
    '            End If
    '        End If
    '    End If
    'End Function

    ''' <summary>
    ''' CLightCtrl.ColorTemp.getCurrent [0xXXXX]
    ''' </summary>
    ''' <param name="btCx">0-1000</param>
    ''' <param name="btCy">0-1000</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    'Public Function GetCxCyColor(ByRef btCx As Double, ByRef btCy As Double) As Boolean
    '    Dim stsResult As TPCANStatus
    '    Dim CANMsg As TPCANMsg = Nothing

    '    GetCxCyColor = False

    '    If EmptyRcvQueue() = False Then
    '        Return False
    '    End If

    '    Dim ND1, ND0 As String
    '    ND0 = String.empty
    '    ND1 = String.empty
    '    GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, 0, eCLightCtrlP.COLORTEMP, eFeatureP.GETCURRENT, ND1, ND0)

    '    stsResult = MyBase.SendCanExtMsg(CLIGHTCTRL, ND0, ND1)

    '    If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '        System.Threading.Thread.Sleep(100)
    '        stsResult = ReadNxtMsg(CANMsg)
    '        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '            If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then '0x53 And 0x19
    '                btCx = (CANMsg.DATA(3) * 256 + CANMsg.DATA(2)) / 1000.0
    '                btCy = (CANMsg.DATA(5) * 256 + CANMsg.DATA(4)) / 1000.0
    '                GetCxCyColor = True
    '            End If
    '        End If
    '    End If
    'End Function

    ''' <summary>
    ''' CLightCtrl.ColorTemp.getCurrent [0xXXXX]
    ''' </summary>
    ''' <param name="btCCT">0-1000</param>
    ''' <returns>True if hardware returns no error</returns>
    ''' <remarks></remarks>
    'Public Function GetCctColor(ByRef btCCT As Integer) As Boolean
    '    Dim stsResult As TPCANStatus
    '    Dim CANMsg As TPCANMsg = Nothing

    '    GetCctColor = False

    '    If EmptyRcvQueue() = False Then
    '        Return False
    '    End If

    '    Dim ND1, ND0 As String
    '    ND0 = String.empty
    '    ND1 = String.empty
    '    GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, 0, eCLightCtrlP.COLORTEMP, eFeatureP.GETCURRENT, ND1, ND0)

    '    stsResult = MyBase.SendCanExtMsg(CLIGHTCTRL, ND0, ND1)

    '    If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '        System.Threading.Thread.Sleep(100)
    '        stsResult = ReadNxtMsg(CANMsg)
    '        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '            If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then '0x53 And 0x19
    '                btCCT = CANMsg.DATA(3) * 256 + CANMsg.DATA(2)
    '                GetCctColor = True
    '            End If
    '        End If
    '    End If
    'End Function

    'Private Function SetFilterwheelPosition() As Boolean
    '    Dim stsResult As TPCANStatus
    '    Dim CANMsg As TPCANMsg = Nothing

    '    SetFilterwheelPosition = False

    '    If EmptyRcvQueue() = False Then
    '        Return False
    '    End If


    '    Dim ND1, ND0 As String
    '    ND0 = String.empty
    '    ND1 = String.empty
    '    GetCommand(eProtocol.SPECIFICAL_FCT_MODULE, 0, eCDebugP.F, eFeatureP.GETCURRENT, ND1, ND0)

    '    'ID: 160C220D // DataLength: 2 // Data: 0x62 0x18
    '    stsResult = MyBase.SendCanExtMsg(CDEBUG, ND0, ND1)

    '    If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '        System.Threading.Thread.Sleep(100)
    '        stsResult = ReadNxtMsg(CANMsg)
    '        If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
    '            If (CANMsg.DATA(0) = Convert.ToByte(ND0, 16) + 1) And (CANMsg.DATA(1) = Convert.ToByte(ND1, 16)) Then '0x63 And 0x18

    '                SetFilterwheelPosition = True
    '            End If
    '        End If
    '    End If

    'End Function

End Class
