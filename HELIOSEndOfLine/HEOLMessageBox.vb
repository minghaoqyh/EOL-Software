Imports System.Reflection
Imports System.IO

Public Class HEOLMessageBox
    Dim index As Integer = 0
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal caption As String, ByVal text As String)
        InitializeComponent()
        Me.Text = caption
        Me.lbltext.Text = text
    End Sub
    Public Sub New(ByVal index As Integer)
        InitializeComponent()
        Me.index = index
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        tmrPing.Stop()
        Me.DialogResult = Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        tmrPing.Stop()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub HEOLMessageBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
        Dim myStream As Stream
        Select Case Me.index
            Case 0

            Case 1
                Me.Text = "DIP Switch umstellen"
                Me.lbltext.Text = "DIP Switch umstellen und Deckel wieder schliessen"
                myStream = myAssembly.GetManifestResourceStream("HELIOSEndOfLine.picture1.PNG")
                Dim image As New Bitmap(myStream)
                Me.pBoxPicture.Image = image
                Me.pBoxPicture.SizeMode = PictureBoxSizeMode.StretchImage
            Case 2
                Me.Text = "DIP Switch umstellen"
                Me.lbltext.Text = "DIP Switch umstellen"
                myStream = myAssembly.GetManifestResourceStream("HELIOSEndOfLine.picture2.PNG")
                Dim image As New Bitmap(myStream)
                Me.pBoxPicture.Image = image
                Me.pBoxPicture.SizeMode = PictureBoxSizeMode.StretchImage
        End Select
        tmrPing.Start()
        Me.btnOK.Focus()
    End Sub


    Private Sub tmrPing_Tick(sender As Object, e As EventArgs) Handles tmrPing.Tick
        Try
            System.Media.SystemSounds.Exclamation.Play()
        Catch ex As Exception
            'ignore
        End Try
    End Sub
End Class

