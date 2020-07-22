Imports HELIOSCommunication.Peak.Can.Basic

Public Class CANMsgObj

    Private CanMsg As TPCANMsg
    Private CanTimestamp As TPCANTimestamp

    Public Sub New(ByVal Msg As TPCANMsg, ByVal Timestamp As TPCANTimestamp)
        CanMsg = Msg
        CanTimestamp = Timestamp
    End Sub
End Class
