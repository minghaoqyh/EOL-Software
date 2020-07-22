Public Class ProtocolEOLBox

    Private WithEvents myRs232 As New System.IO.Ports.SerialPort

    Private Const Baudrate As Integer = 57600
    Private Const Databits As Integer = 8
    Private Const Parity As System.IO.Ports.Parity = System.IO.Ports.Parity.None
    Private Const Stopbits As System.IO.Ports.StopBits = System.IO.Ports.StopBits.One
    Private Const Buffer As Integer = 512
    Private Const Timeout As Integer = 100

    Public Event PinChanged(sender As Object, e As System.IO.Ports.SerialPinChangedEventArgs)

#Region "Hardware Layer"
    Public Sub Open(ByVal MyComPort As String)
        If myRs232.IsOpen Then
            myRs232.Close()
        End If
        myRs232.PortName = MyComPort
        myRs232.BaudRate = Baudrate
        myRs232.DataBits = Databits
        myRs232.Parity = Parity
        myRs232.StopBits = Stopbits
        myRs232.ReadBufferSize = Buffer
        myRs232.ReadTimeout = Timeout

        myRs232.Open()
    End Sub

    Public Sub Close()
        myRs232.Close()
    End Sub

    Public ReadOnly Property IsOpen() As Boolean
        Get
            Return myRs232.IsOpen()
        End Get
    End Property

    Private Sub MyRs232_PinChanged(sender As Object, e As System.IO.Ports.SerialPinChangedEventArgs) Handles myRs232.PinChanged
        RaiseEvent PinChanged(sender, e)
    End Sub

    'Private Sub rs232read(ByRef ReceivedBytes As Byte())
    '    Dim BytesRead As Integer = 0
    '    Do
    '        BytesRead += myRs232.Read(ReceivedBytes, BytesRead, ReceivedBytes.Length - BytesRead)
    '    Loop Until BytesRead = ReceivedBytes.Length
    'End Sub
#End Region

#Region "Internal Functions"
    'Private Function ArrayCompare(ByVal array As System.Array, ByVal array2 As System.Array, Optional ByVal length As Integer = 0) As Boolean
    '    Dim i As Integer

    '    If length = 0 And Not array.Length = array2.Length Then Return False

    '    If length = 0 Then
    '        length = array.Length
    '    End If

    '    For i = 0 To length - 1
    '        If Not array(i) = array2(i) Then Return False
    '    Next

    '    Return True
    'End Function

    'Private Function Dec2Float(ByVal dec As Double) As UInt32
    '    Dim s As Integer
    '    Dim exp As Integer = 0
    '    Dim mant As Integer

    '    If dec = 0 Then
    '        s = 0
    '        exp = 0
    '        mant = 0
    '    ElseIf Double.IsPositiveInfinity(dec) Then
    '        s = 0
    '        exp = 255
    '        mant = 0
    '    ElseIf Double.IsNegativeInfinity(dec) Then
    '        s = 1
    '        exp = 255
    '        mant = 0
    '    ElseIf Double.IsNaN(dec) Then
    '        s = 0
    '        exp = 255
    '        mant = Math.Pow(2, 23) - 1
    '    Else
    '        If dec < 0 Then
    '            s = 1
    '            dec = -dec
    '        End If

    '        Do While dec >= 2
    '            dec /= 2
    '            exp += 1
    '        Loop

    '        Do While dec < 1
    '            dec *= 2
    '            exp -= 1
    '        Loop

    '        exp += 127
    '        dec -= 1

    '        mant = dec * Math.Pow(2, 23)
    '    End If

    '    Dim ret As UInt32
    '    ret = s * Math.Pow(2, 31)
    '    ret += exp * Math.Pow(2, 23)
    '    ret += mant

    '    Return ret
    'End Function
#End Region

#Region "Low Level Communication"
    'Public Function SendRaw(ByVal command As Byte, ByVal data() As Byte, ByVal ReplyBytes As Integer) As Byte()
    '    Dim buf(), buf2() As Byte

    '    If data Is Nothing Then
    '        ReDim buf(0)
    '    Else
    '        ReDim buf(data.Length)
    '    End If

    '    If Not myRs232.IsOpen Then Throw New System.Exception("communication port closed")

    '    buf(0) = command

    '    If Not data Is Nothing Then
    '        Array.Copy(data, 0, buf, 1, data.Length)
    '    End If

    '    myRs232.DiscardInBuffer()

    '    myRs232.Write(buf, 0, buf.Length)
    '    If ReplyBytes < 0 Then
    '        Return Nothing  ' No Echo expected
    '    End If

    '    If data Is Nothing Then
    '        ReDim buf2(0)
    '    Else
    '        ReDim buf2(data.Length)
    '    End If
    '    rs232read(buf2)


    '    If Not ArrayCompare(buf, buf2) Then
    '        myRs232.DiscardInBuffer()
    '        Throw New Exception("Wrong Echo.")
    '    End If

    '    If ReplyBytes > 0 Then
    '        ReDim buf2(ReplyBytes - 1)
    '        rs232read(buf2)
    '        myRs232.DiscardInBuffer()
    '        Return buf2
    '    Else
    '        Return Nothing
    '    End If
    'End Function
#End Region

#Region "High Level Communication"
    Public Sub SwitchToHeliosSet(ByVal unit As Integer)
        Select Case unit
            Case 0
                myRs232.DtrEnable = False
                myRs232.RtsEnable = False
            Case 1
                myRs232.DtrEnable = False
                myRs232.RtsEnable = True
            Case 2
                myRs232.DtrEnable = True
                myRs232.RtsEnable = False
            Case 3
                myRs232.DtrEnable = True
                myRs232.RtsEnable = True
            Case Else
        End Select
    End Sub

    Public Function GetHeliosPowered() As Boolean
        Return Not myRs232.CtsHolding
    End Function

    Public Function GetVersion(ByRef version As String) As Boolean
        Try
            Dim buffer(0) As Char
            Dim count As Integer
            myRs232.DiscardInBuffer()
            myRs232.Write("VER" & vbCr)
            Threading.Thread.Sleep(100)
            count = myRs232.BytesToRead()
            ReDim buffer(count)
            myRs232.Read(buffer, 0, count)
            version = New String(buffer)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetLWLPos(ByRef result As String) As Boolean
        Dim buffer(10) As Char
        Dim count As Integer
        Dim returnValue As Boolean = False
        myRs232.DiscardInBuffer()
        myRs232.Write("LWL?" & vbCr)
        Threading.Thread.Sleep(50)
        count = myRs232.BytesToRead()
        myRs232.Read(buffer, 0, count)
        If buffer(0) = "0" Then
            result = "out"
            returnValue = True
        ElseIf buffer(0) = "1" Then
            result = "in"
            returnValue = True
        ElseIf New String(buffer) = ("ERR: Collision!" & vbCr) Then
            result = "Collision error"
            returnValue = False
        Else
            result = "unknown error"
            returnValue = False
        End If
        Return returnValue
    End Function

    Public Function GetCapPos(ByRef result As String) As Boolean
        Dim buffer(10) As Char
        Dim count As Integer
        myRs232.DiscardInBuffer()
        myRs232.Write("Open?" & vbCr)
        Threading.Thread.Sleep(50)
        count = myRs232.BytesToRead()
        myRs232.Read(buffer, 0, count)
        If buffer(0) = "0" Then
            result = "closed"
        ElseIf buffer(0) = "1" Then
            result = "open"
        Else
            result = "error"
            Return False
        End If
        Return True
    End Function

    Public Function SetLWLPos(ByVal pos As String, ByRef buffer() As Char) As Boolean
        Dim count As Integer
        Dim result As Boolean
        myRs232.DiscardInBuffer()
        If pos = "in" Then
            myRs232.Write("LWL 1" & vbCr)
        ElseIf pos = "out" Then
            myRs232.Write("LWL 0" & vbCr)
        End If
        Threading.Thread.Sleep(1000)
        count = myRs232.BytesToRead()
        myRs232.Read(buffer, 0, count)
        If buffer(0) = "A" And buffer(1) = "c" And buffer(2) = "k" Then
            result = True
        Else
            result = False
        End If
        Return result
    End Function

    'Public Sub LampOn()
    '    SendRaw(&H25, Nothing, 0) '[25] Lamp On
    'End Sub

    'Public Sub LampOff()
    '    SendRaw(&H26, Nothing, 0) '[26] Lamp Off
    'End Sub

    'Public Sub GotoStandByBlue()
    '    SendRaw(&H27, Nothing, 0) '[27] Goto StandBy Blue
    'End Sub

    'Public Sub GotoStandByYellow()
    '    SendRaw(&H28, Nothing, 0) '[28] Goto StandBy Yellow
    'End Sub

    'Public Sub GotoCalibration()
    '    Dim buf(1) As Byte
    '    buf(0) = &H42
    '    buf(1) = &H43
    '    SendRaw(&H30, buf, -1) '[30] Goto Calibration
    'End Sub

    'Public Sub Reset()
    '    SendRaw(&H3A, Nothing, -1) '[3A] Reset
    'End Sub

    'Public Sub SetDim(ByVal newDim As UShort)
    '    Dim buf(1) As Byte
    '    buf(0) = newDim \ 256
    '    buf(1) = newDim And &HFF
    '    SendRaw(&H6D, buf, 0) '[6D] Set Dim
    'End Sub

    'Public Sub WriteItem(ByVal item As Byte, ByVal data As UShort)
    '    Dim buf(2) As Byte
    '    buf(0) = item
    '    buf(1) = data \ 256
    '    buf(2) = data And &HFF
    '    SendRaw(&H6E, buf, 0) '[6E] Write Item
    'End Sub

    'Public Sub WriteWord(ByVal data As UShort)
    '    Dim buf(1) As Byte
    '    buf(0) = data \ 256
    '    buf(1) = data And &HFF
    '    SendRaw(&H6F, buf, 0) '[6F] Write Word
    'End Sub

    'Public Sub EnableCommunication()
    '    SendRaw(&H70, Nothing, -1) '[70] Enable Communication
    'End Sub

    'Public Sub WriteByte(ByVal value As Byte)
    '    Dim buf(0) As Byte
    '    buf(0) = value
    '    SendRaw(&H73, buf, 0) '[73] Write Byte
    'End Sub

    'Public Sub SetMailbox(ByVal address As UShort)
    '    Dim buf(2) As Byte
    '    buf(0) = address \ 256
    '    buf(1) = address Mod 256
    '    buf(2) = 1
    '    SendRaw(&H74, buf, 0) '[74] Set Mailbox
    'End Sub

    'Public Sub DisableCommunication()
    '    SendRaw(&H75, Nothing, -1) '[75] Disable Communication
    'End Sub

    'Public Function QueryStatus() As UShort
    '    Dim buf() As Byte

    '    buf = SendRaw(&HED, Nothing, 2) '[ED] Query Status
    '    Return Convert.ToUInt16(buf(0) * 256 + buf(1))
    'End Function

    'Public Function QueryItem(ByVal item As Byte) As UInt16
    '    Dim buf(0), buf2() As Byte

    '    buf(0) = item
    '    buf2 = SendRaw(&HEE, buf, 2) '[EE] Query Item
    '    Return Convert.ToUInt16(buf2(0) * 256 + buf2(1))
    'End Function

    'Public Function QueryWord() As UInt16
    '    Dim buf() As Byte

    '    buf = SendRaw(&HEF, Nothing, 2) '[EF] Query Word
    '    Return Convert.ToUInt16(buf(0) * 256 + buf(1))
    'End Function

    'Public Function QueryDriverID() As UShort
    '    Dim buf() As Byte

    '    buf = SendRaw(&HF1, Nothing, 2) '[F1] Query Driver ID
    '    Return Convert.ToUInt16(buf(0) * 256 + buf(1))
    'End Function

    'Public Function QueryByte() As Byte
    '    Dim buf() As Byte
    '    buf = SendRaw(&HF9, Nothing, 1) '[F9] Query Byte
    '    Return buf(0)
    'End Function

    'Public Function QueryAddress(ByVal item As Byte) As UInt16
    '    Dim buf(0), buf2() As Byte

    '    buf(0) = item
    '    buf2 = SendRaw(&HFF, buf, 2) '[FF] Query Address
    '    Return Convert.ToUInt16(buf2(0) * 256 + buf2(1))
    'End Function

    'Public Sub WriteFloat(ByVal data As Double)
    '    Dim tmp As UInt32
    '    tmp = Dec2Float(data)
    '    WriteWord(tmp And &HFFFF)
    '    WriteWord(tmp \ 65536)
    'End Sub

    'Public Sub WriteColor(ByVal cx As UShort, ByVal cy As UShort, ByVal phi As UShort, ByVal red As Byte, ByVal gre As Byte, ByVal blu As Byte, ByVal dimValue As Byte, ByVal trigger As Boolean)
    '    Dim buf(10), buf2() As Byte
    '    buf(0) = cx \ 256
    '    buf(1) = cx Mod 256
    '    buf(2) = cy \ 256
    '    buf(3) = cy Mod 256
    '    buf(4) = phi \ 256
    '    buf(5) = phi Mod 256
    '    buf(6) = red
    '    buf(7) = gre
    '    buf(8) = blu
    '    buf(9) = dimValue
    '    If (trigger = True) Then
    '        buf(10) = 2
    '    Else
    '        buf(10) = 0
    '    End If
    '    buf2 = SendRaw(&H40, buf, -1)
    'End Sub

    'Public Sub WriteBlink(ByVal color As Byte, ByVal onTime As Byte, ByVal offTime As Byte, ByVal count As Byte)
    '    Dim buf(3), buf2() As Byte
    '    buf(0) = color
    '    buf(1) = onTime
    '    buf(2) = offTime
    '    buf(3) = count
    '    buf2 = SendRaw(&H41, buf, -1)
    'End Sub
#End Region

End Class
