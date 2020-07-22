Imports System.Text
Imports System.Net
Imports System.Net.Sockets

Public Class TcpKeysightN5767Communication : Implements IDisposable
    Private client As TcpClient
    Private stream As NetworkStream
    Private hostname As String = "192.168.23.2"
    Private port As Integer = 5025
    Private connectionIsActive As Boolean

    Sub New(ByVal hostname As String, ByVal port As Integer)
        client = New TcpClient()
        Dim result As System.IAsyncResult
        Dim success As Boolean
        result = client.BeginConnect(hostname, port, Nothing, Nothing)
        success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(200))

        If success Then
            stream = client.GetStream
            Me.hostname = hostname
            Me.port = port
            Me.connectionIsActive = True
        Else
            client = Nothing
            stream = Nothing
            Me.hostname = ""
            Me.port = 0
            Me.connectionIsActive = False
        End If
    End Sub

    Protected Overrides Sub Finalize()
        If Not IsNothing(stream) Then
            stream.Close()
        End If
        If Not IsNothing(client) Then
            client.Close()
        End If
        MyBase.Finalize()
    End Sub

    Private Function Send(ByVal Text As String) As Boolean
        If Me.connectionIsActive = True Then
            Dim sendbuffer(128) As Byte
            sendbuffer = Encoding.ASCII.GetBytes(Text)
            stream.Write(sendbuffer, 0, sendbuffer.Length)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Read(ByRef data As String) As Boolean
        If Me.connectionIsActive = True Then
            Dim readbuffer(128) As Byte
            Dim bytecount As Integer
            stream.WriteTimeout = 100
            bytecount = stream.Read(readbuffer, 0, readbuffer.Length)
            data = Encoding.ASCII.GetString(readbuffer)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function getIdentification(ByRef result As String) As Boolean
        If Send("*IDN?" & vbCrLf) Then
            If Read(result) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function setOutputOnOff(ByVal value As Boolean) As Boolean
        Dim command As String = String.Empty
        If value = True Then
            command = "OUTP ON"
        Else
            command = "OUTP OFF"
        End If

        If Send(command & vbCrLf) Then
            Return True
        End If
        Return False
    End Function

    Public Function setVoltage(ByVal value As Double) As Boolean
        Dim command As String = String.Empty
        command = "VOLT " & value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
        If Send(command & vbCrLf) Then
            Return True
        End If
        Return False
    End Function

    Public Function setCurrent(ByVal value As Double) As Boolean
        Dim command As String = String.Empty
        command = "CURR " & value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
        If Send(command & vbCrLf) Then
            Return True
        End If
        Return False
    End Function

    Public Function getVoltage(ByRef result As String) As Boolean
        Dim command As String = String.Empty
        command = "VOLT?"
        If Send(command & vbCrLf) Then
            If Read(result) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function getCurrent(ByRef result As String) As Boolean
        Dim command As String = String.Empty
        command = "CURR?"
        If Send(command & vbCrLf) Then
            If Read(result) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function measVoltage(ByRef result As String) As Boolean
        Dim command As String = String.Empty
        command = "MEAS:VOLT?"
        If Send(command & vbCrLf) Then
            If Read(result) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function measCurrent(ByRef result As String) As Boolean
        Dim command As String = String.Empty
        command = "MEAS:CURR?"
        If Send(command & vbCrLf) Then
            If Read(result) Then
                Return True
            End If
        End If
        Return False
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Verwalteten Zustand löschen (verwaltete Objekte).
                If Not IsNothing(stream) Then
                    stream.Close()
                End If
                If Not IsNothing(client) Then
                    client.Close()
                End If
                MyBase.Finalize()
            End If

            ' TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalize() unten überschreiben.
            ' TODO: Große Felder auf NULL festlegen.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: Finalize() nur überschreiben, wenn Dispose(ByVal disposing As Boolean) oben über Code zum Freigeben von nicht verwalteten Ressourcen verfügt.
    'Protected Overrides Sub Finalize()
    '    ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(disposing As Boolean) Bereinigungscode ein.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
