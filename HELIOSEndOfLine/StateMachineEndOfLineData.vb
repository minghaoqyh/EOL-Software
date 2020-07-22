Public Class StateMachineEndOfLineData
    Private m_serialNumber() As String
    Public Property SerialNumber() As String()
        Get
            Return m_serialNumber
        End Get
        Set(ByVal value As String())
            m_serialNumber = value
        End Set
    End Property

    Private m_binning() As String
    Public Property Binning() As String()
        Get
            Return m_binning
        End Get
        Set(ByVal value As String())
            m_binning = value
        End Set
    End Property

    Private m_matNumber() As String
    Public Property MatNumber() As String()
        Get
            Return m_matNumber
        End Get
        Set(ByVal value As String())
            m_matNumber = value
        End Set
    End Property

    Private m_czmSeriennummer As String
    Public Property CZMSeriennummer() As String
        Get
            Return m_czmSeriennummer
        End Get
        Set(ByVal value As String)
            m_czmSeriennummer = value
        End Set
    End Property

    Private m_barcode As String
    Public Property Barcode() As String
        Get
            Return m_barcode
        End Get
        Set(ByVal value As String)
            m_barcode = value
        End Set
    End Property

    Private m_barcodeV As String
    Public Property BarcodeV() As String
        Get
            Return m_barcodeV
        End Get
        Set(ByVal value As String)
            m_barcodeV = value
        End Set
    End Property

    Private m_Config2D1 As String
    Public Property Config2D1() As String
        Get
            Return m_Config2D1
        End Get
        Set(ByVal value As String)
            m_Config2D1 = value
        End Set
    End Property

    Private m_ConfigLastModifiedUtc As Date
    Public Property ConfigLastModifiedUtc() As Date
        Get
            Return m_ConfigLastModifiedUtc
        End Get
        Set(ByVal value As Date)
            m_ConfigLastModifiedUtc = value
        End Set
    End Property

    Private m_ModuleType As Integer
    Public Property ModuleType As Integer
        Get
            Return m_ModuleType
        End Get
        Set(value As Integer)
            m_ModuleType = value
        End Set
    End Property

    Public Sub New()
        ReDim m_serialNumber(6)
        ReDim m_matNumber(6)
        ReDim m_binning(6)
        For i = 0 To 6
            m_serialNumber(i) = String.Empty
            m_matNumber(i) = String.Empty
            m_binning(i) = String.Empty
        Next i
        m_czmSeriennummer = String.Empty
        m_barcode = String.Empty
        m_barcodeV = String.Empty
        m_Config2D1 = String.Empty
        m_ConfigLastModifiedUtc = Date.MinValue
    End Sub
End Class
