Public Class HEOLCheckDio
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal caption As String, ByVal text As String)
        InitializeComponent()
        Me.Text = caption
        Me.lbltext.Text = text
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub

    Private Sub timerTest_Tick(sender As Object, e As EventArgs) Handles timerTest.Tick
        Dim result As String = String.Empty
        Dim iniReader As IniReader = New IniReader
        Dim port As String
        port = iniReader.ReadValueFromFile("COM", "port", "", ".\Settings.ini")

        Dim myProtocol As HardwareCommunication.ProtocolEOLBox = New HardwareCommunication.ProtocolEOLBox

        myProtocol.Open(port)
        If Not myProtocol.GetCapPos(result) Then

        End If
        myProtocol.Close()

        If result = "closed" Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub HEOLCheckDio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.timerTest.Enabled = True
    End Sub

End Class