Public Class DataCalib
    Private m_CalibDacDone As Byte
    Private m_CalibDacSaved As Byte
    Private m_CalibDone As Byte
    Private m_CalibSaved As Byte
    Private m_CalibSuperR9Done As Byte
    Private m_CalibSuperR9Saved As Byte
    Private m_CSVpath As String
    Private m_CSVfilename As String

#Region "Properties"
    Public Property CalibDacDone() As Byte
        Get
            Return m_CalibDacDone
        End Get
        Set(ByVal value As Byte)
            m_CalibDacDone = value
        End Set
    End Property

    Public Property CalibDacSaved() As Byte
        Get
            Return m_CalibDacSaved
        End Get
        Set(ByVal value As Byte)
            m_CalibDacSaved = value
        End Set
    End Property

    Public Property CalibDone() As Byte
        Get
            Return m_CalibDone
        End Get
        Set(ByVal value As Byte)
            m_CalibDone = value
        End Set
    End Property

    Public Property CalibSaved() As Byte
        Get
            Return m_CalibSaved
        End Get
        Set(ByVal value As Byte)
            m_CalibSaved = value
        End Set
    End Property

    Public Property CalibSuperR9Done() As Byte
        Get
            Return m_CalibSuperR9Done
        End Get
        Set(ByVal value As Byte)
            m_CalibSuperR9Done = value
        End Set
    End Property

    Public Property CalibSuperR9Saved() As Byte
        Get
            Return m_CalibSuperR9Saved
        End Get
        Set(ByVal value As Byte)
            m_CalibSuperR9Saved = value
        End Set
    End Property

    Public Property CSVpath() As String
        Get
            Return m_CSVpath
        End Get
        Set(ByVal value As String)
            m_CSVpath = value
        End Set
    End Property

    Public Property CSVfilename() As String
        Get
            Return m_CSVfilename
        End Get
        Set(ByVal value As String)
            m_CSVfilename = value
        End Set
    End Property
#End Region

    Public Sub New()
        m_CalibDacDone = 0
        m_CalibDacSaved = 0
        m_CalibDone = 0
        m_CalibSaved = 0
        m_CalibSuperR9Done = 0
        m_CalibSuperR9Saved = 0
        m_CSVpath = String.Empty
        m_CSVfilename = String.Empty
    End Sub
End Class
