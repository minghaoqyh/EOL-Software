Imports NationalInstruments.DAQmx
Imports System.Windows
Public Class NI6520_DAQ

    Private m_device As String
    Public Property Device() As String
        Get
            Return m_device
        End Get
        Set(ByVal value As String)
            m_device = value
        End Set
    End Property

    Private m_jumper As DioPort
    Public Property Jumper() As DioPort
        Get
            Return m_jumper
        End Get
        Set(ByVal value As DioPort)
            m_jumper = value
        End Set
    End Property

    Private m_safetyBox As DioPort
    Public Property SafetyBox() As DioPort
        Get
            Return m_safetyBox
        End Get
        Set(ByVal value As DioPort)
            m_safetyBox = value
        End Set
    End Property

    Public Sub New()
        m_device = String.Empty
        m_jumper = New DioPort
        m_safetyBox = New DioPort
    End Sub

    Public Function InitNI6520(device As String) As Boolean
        m_device = String.Empty

        If DaqSystem.Local.Devices.Length > 0 Then
            'portStrings = DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.All)
            For Each s As String In DaqSystem.Local.Devices
                If s = device Then
                    m_device = s
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Function setSingleDOLine(ByVal port As Integer, ByVal line As Integer, ByVal value As Boolean)
        Dim returnValue As Boolean

        Dim digitalWriteTask As Task = New Task()
        Try
            digitalWriteTask.DOChannels.CreateChannel(m_device & "/port" & CStr(port) & "/line" & CStr(line), "port0", ChannelLineGrouping.OneChannelForEachLine)
            Dim writer As DigitalSingleChannelWriter = New DigitalSingleChannelWriter(digitalWriteTask.Stream)
            writer.WriteSingleSampleSingleLine(True, value)
            returnValue = True
        Catch ex As System.Exception
            Throw ex
        Finally
            digitalWriteTask.Dispose()
        End Try
        Return returnValue
    End Function

    Public Function getSingleDILine(ByVal port As Integer, ByVal line As Integer, ByRef value As Boolean)

        Dim returnValue As Boolean

        Dim digitalReadTask As Task = New Task()
        Try
            digitalReadTask.DIChannels.CreateChannel(m_device & "/port" & CStr(port) & "/line" & CStr(line), "port0", ChannelLineGrouping.OneChannelForEachLine)
            Dim reader As DigitalSingleChannelReader = New DigitalSingleChannelReader(digitalReadTask.Stream)
            value = reader.ReadSingleSampleSingleLine()
            returnValue = True
        Catch ex As System.Exception
            Throw ex
        Finally
            digitalReadTask.Dispose()
        End Try
        Return returnValue
    End Function
End Class

Public Class DioPort
    Private m_port As Integer
    Public Property Port() As Integer
        Get
            Return m_port
        End Get
        Set(ByVal value As Integer)
            m_port = value
        End Set
    End Property
    Private m_portPin As Integer
    Public Property PortPin() As Integer
        Get
            Return m_portPin
        End Get
        Set(ByVal value As Integer)
            m_portPin = value
        End Set
    End Property
    Public Sub New()
        Me.Port = -1
        Me.PortPin = -1
    End Sub
End Class
