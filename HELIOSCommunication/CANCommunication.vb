Imports HELIOSCommunication.Peak.Can.Basic
Imports TPCANHandle = System.Byte
Imports System.Collections.Generic




Public Class CANCommunication
    Friend m_PcanHandle As TPCANHandle
    Private m_ReadThread As System.Threading.Thread
    Private m_ReceiveEvent As System.Threading.AutoResetEvent
    Private m_CANMsgBuffer As Queue(Of CANMsgObj)

    Private bInitialized As Boolean

    Private Const PCAN_USB = &H51

    Public Sub New(oBaudrate As TPCANBaudrate, oHwType As TPCANType)
        Initialize(oBaudrate, oHwType)
    End Sub

    Public Sub Initialize(oBaudrate As TPCANBaudrate, oHwType As TPCANType)
        Dim stsResult As TPCANStatus

        m_PcanHandle = PCAN_USB 'Convert.ToByte(51, 16)

        stsResult = PCANBasic.Initialize(m_PcanHandle, oBaudrate, oHwType, 0, 0)

        If stsResult = TPCANStatus.PCAN_ERROR_OK Then
            bInitialized = True
        Else
            bInitialized = False
        End If

        m_CANMsgBuffer = New Queue(Of CANMsgObj)

        If stsResult <> TPCANStatus.PCAN_ERROR_OK Then
            'MessageBox.Show(GetFormatedError(stsResult))
        Else
            ' Prepares the PCAN-Basic's PCAN-Trace file
            '
            'ConfigureTraceFile()

            'm_ReadDelegate = New ReadDelegateHandler(AddressOf ReadMessages)

            Dim threadDelegate As New System.Threading.ThreadStart(AddressOf Me.CANReadThreadFunc)
            m_ReadThread = New System.Threading.Thread(threadDelegate)
            m_ReadThread.IsBackground = True
            m_ReadThread.Start()

            m_ReceiveEvent = New System.Threading.AutoResetEvent(False)

        End If

        ' Sets the connection status of the main-form
        '
        'SetConnectionStatus(stsResult = TPCANStatus.PCAN_ERROR_OK)
    End Sub

    Public Sub Uninitialize()
        ' Releases a current connected PCAN-Basic channel

        bInitialized = False
        PCANBasic.Uninitialize(m_PcanHandle)
        If m_ReadThread IsNot Nothing Then
            m_ReadThread.Abort()
            m_ReadThread.Join()
            m_ReadThread = Nothing
        End If

        m_CANMsgBuffer.Clear()

    End Sub


    Private Sub Write(sCanId As String, btaData() As Byte, oTPCANMessageType As TPCANMessageType)
        Dim CANMsg As TPCANMsg
        Dim stsResult As TPCANStatus

        ' We create a TCLightMsg message structure 
        '
        CANMsg = New TPCANMsg()
        CANMsg.DATA = New Byte(7) {}

        ' We configurate the Message.  The ID (max 0x1FF),
        ' Length of the Data, Message Type (Standard in 
        ' this example) and die data
        '
        CANMsg.ID = Convert.ToUInt32(sCanId, 16)
        CANMsg.LEN = Convert.ToByte(UBound(btaData))
        CANMsg.MSGTYPE = oTPCANMessageType 'TPCANMessageType.PCAN_MESSAGE_EXTENDED;   TPCANMessageType.PCAN_MESSAGE_STANDARD        CANMsg.MSGTYPE = IIf((chbExtended.Checked), TPCANMessageType.PCAN_MESSAGE_EXTENDED, TPCANMessageType.PCAN_MESSAGE_STANDARD)
        ' If a remote frame will be sent, the data bytes are not important.
        '

        ' We get so much data as the Len of the message
        '

        For i As Integer = 0 To UBound(btaData) - 1
            CANMsg.DATA(i) = btaData(i) 'Convert.ToByte(txtbCurrentTextBox.Text, 16)
        Next

        ' The message is sent to the configured hardware
        '
        stsResult = PCANBasic.Write(m_PcanHandle, CANMsg)

        ' The Hardware was successfully sent
        '
        If stsResult = TPCANStatus.PCAN_ERROR_OK Then
            'IncludeTextMessage("Message was successfully SENT")
        Else
            ' An error occurred.  We show the error.
            '			
            'MessageBox.Show(GetFormatedError(stsResult))
        End If
    End Sub


    Private Sub CANReadThreadFunc()
        Dim iBuffer As UInt32
        Dim stsResult As TPCANStatus
        Dim CANMsg As TPCANMsg = Nothing
        'Dim CANTimeStamp As TPCANTimestamp
        Try
            iBuffer = Convert.ToUInt32(m_ReceiveEvent.SafeWaitHandle.DangerousGetHandle().ToInt32())
        ' Sets the handle of the Receive-Event.
        '
        stsResult = PCANBasic.SetValue(m_PcanHandle, TPCANParameter.PCAN_RECEIVE_EVENT, iBuffer, CType(System.Runtime.InteropServices.Marshal.SizeOf(iBuffer), UInteger))

        If stsResult <> TPCANStatus.PCAN_ERROR_OK Then
            'MessageBox.Show(GetFormatedError(stsResult), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' While this mode is selected
        While (1)
            ' Waiting for Receive-Event
            ' 
            If m_ReceiveEvent.WaitOne(50) Then
                ' Process Receive-Event using .NET Invoke function
                ' in order to interact with Winforms UI (calling the 
                ' function ReadMessages)
                ' 
                'Form1.Invoke(m_ReadDelegate)
                'stsResult = PCANBasic.Read(m_PcanHandle, CANMsg, CANTimeStamp)

                'Dim msg = New CANMsgObj(CANMsg, CANTimeStamp)

                'If (stsResult = TPCANStatus.PCAN_ERROR_OK) Then
                '   m_CANMsgBuffer.Enqueue(msg)
                'End If


            End If
            End While
        Catch ex As NullReferenceException
            iBuffer = 0
        End Try
    End Sub

    Public Function GetFilterStatus(ByRef status As UInteger) As Boolean
        Dim stsResult As TPCANStatus

        ' Tries to get the sttaus of the filter for the current connected hardware
        '
        stsResult = PCANBasic.GetValue(m_PcanHandle, TPCANParameter.PCAN_MESSAGE_FILTER, status, CType(System.Runtime.InteropServices.Marshal.SizeOf(status), UInteger))

        ' If it fails, a error message is shown
        '
        If stsResult <> TPCANStatus.PCAN_ERROR_OK Then
            ' MessageBox.Show(GetFormatedError(stsResult))
            Return False
        End If
        Return True
    End Function

    Public Function SendCanExtMsg(sID As String, btDataLenght As Byte, abtData As Byte()) As TPCANStatus
        Dim CANMsg As TPCANMsg
        Dim stsResult As TPCANStatus

        CANMsg = New TPCANMsg()
        'CANMsg.DATA = New Byte(7) {}

        CANMsg.ID = Convert.ToUInt32(sID, 16)
        CANMsg.LEN = btDataLenght
        CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED

        CANMsg.DATA = abtData


        stsResult = PCANBasic.Write(m_PcanHandle, CANMsg)

        If stsResult = TPCANStatus.PCAN_ERROR_OK Then
            'IncludeTextMessage("Message was successfully SENT")
        Else
            ' An error occurred.  We show the error.
            '			
            'MessageBox.Show(GetFormatedError(stsResult))
        End If
        Return stsResult
    End Function

    Public Function SendCanExtMsg(sID As String, sByte0 As String, sByte1 As String, Optional sByte2 As String = "", Optional sByte3 As String = "", Optional sByte4 As String = "", Optional sByte5 As String = "", Optional sByte6 As String = "", Optional sByte7 As String = "") As TPCANStatus
        Dim CANMsg As TPCANMsg
        Dim stsResult As TPCANStatus

        CANMsg = New TPCANMsg()

        CANMsg.ID = Convert.ToUInt32(sID, 16)
        CANMsg.DATA = New Byte(7) {}
        CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED

        CANMsg.DATA(0) = Convert.ToByte(sByte0, 16)
        CANMsg.DATA(1) = Convert.ToByte(sByte1, 16)
        CANMsg.LEN = 2

        If (sByte2 <> "") Then
            CANMsg.DATA(2) = Convert.ToByte(sByte2, 16)
            CANMsg.LEN += 1
        End If
        If (sByte3 <> "") Then
            CANMsg.DATA(3) = Convert.ToByte(sByte3, 16)
            CANMsg.LEN += 1
        End If
        If (sByte4 <> "") Then
            CANMsg.DATA(4) = Convert.ToByte(sByte4, 16)
            CANMsg.LEN += 1
        End If
        If (sByte5 <> "") Then
            CANMsg.DATA(5) = Convert.ToByte(sByte5, 16)
            CANMsg.LEN += 1
        End If
        If (sByte6 <> "") Then
            CANMsg.DATA(6) = Convert.ToByte(sByte6, 16)
            CANMsg.LEN += 1
        End If
        If (sByte7 <> "") Then
            CANMsg.DATA(7) = Convert.ToByte(sByte7, 16)
            CANMsg.LEN += 1
        End If

        stsResult = PCANBasic.Write(m_PcanHandle, CANMsg)

        If stsResult = TPCANStatus.PCAN_ERROR_OK Then
            'IncludeTextMessage("Message was successfully SENT")
        Else
            ' An error occurred.  We show the error.
            '			
            'MessageBox.Show(GetFormatedError(stsResult))
        End If

        Return stsResult
    End Function


    Public Sub NewDataArray(ByRef btArray As Byte(), ByRef btLenght As Byte, sByte0 As String, sByte1 As String, Optional sByte2 As String = "", Optional sByte3 As String = "", Optional sByte4 As String = "", Optional sByte5 As String = "", Optional sByte6 As String = "", Optional sByte7 As String = "")
        btArray = New Byte(7) {}

        btArray(0) = Convert.ToByte(sByte0, 16)
        btArray(1) = Convert.ToByte(sByte1, 16)
        btLenght = 2

        If (sByte2 <> "") Then
            btArray(2) = Convert.ToByte(sByte2, 16)
            btLenght += 1
        End If
        If (sByte3 <> "") Then
            btArray(3) = Convert.ToByte(sByte3, 16)
            btLenght += 1
        End If
        If (sByte4 <> "") Then
            btArray(4) = Convert.ToByte(sByte4, 16)
            btLenght += 1
        End If
        If (sByte5 <> "") Then
            btArray(5) = Convert.ToByte(sByte5, 16)
            btLenght += 1
        End If
        If (sByte6 <> "") Then
            btArray(6) = Convert.ToByte(sByte6, 16)
            btLenght += 1
        End If
        If (sByte7 <> "") Then
            btArray(7) = Convert.ToByte(sByte7, 16)
            btLenght += 1
        End If
    End Sub

    Public ReadOnly Property Initialized() As Boolean
        Get
            Return bInitialized
        End Get
    End Property
End Class
