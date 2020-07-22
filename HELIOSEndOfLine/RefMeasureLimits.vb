Public Class RefMeasureLimits
#Region "Common Properties"
    Private m_color As String
    Public Property Color() As String
        Get
            Return m_color
        End Get
        Set(ByVal value As String)
            m_color = value
        End Set
    End Property
#End Region
#Region "Helios values"
    Private m_current14 As New LimitMinMax
    Public Property Current14() As LimitMinMax
        Get
            Return m_current14
        End Get
        Set(ByVal value As LimitMinMax)
            m_current14 = value
        End Set
    End Property

    Private m_voltage14 As New LimitMinMax
    Public Property Voltage14() As LimitMinMax
        Get
            Return m_voltage14
        End Get
        Set(ByVal value As LimitMinMax)
            m_voltage14 = value
        End Set
    End Property

    Private m_temperature14 As New LimitMinMax
    Public Property Temperature14() As LimitMinMax
        Get
            Return m_temperature14
        End Get
        Set(ByVal value As LimitMinMax)
            m_temperature14 = value
        End Set
    End Property
    Private m_current50 As New LimitMinMax
    Public Property Current50() As LimitMinMax
        Get
            Return m_current50
        End Get
        Set(ByVal value As LimitMinMax)
            m_current50 = value
        End Set
    End Property

    Private m_voltage50 As New LimitMinMax
    Public Property Voltage50() As LimitMinMax
        Get
            Return m_voltage50
        End Get
        Set(ByVal value As LimitMinMax)
            m_voltage50 = value
        End Set
    End Property

    Private m_temperature50 As New LimitMinMax
    Public Property Temperature50() As LimitMinMax
        Get
            Return m_temperature50
        End Get
        Set(ByVal value As LimitMinMax)
            m_temperature50 = value
        End Set
    End Property
#End Region
#Region "Cas Values"
    Private m_cx14 As New LimitMinMax
    Public Property Cx14() As LimitMinMax
        Get
            Return m_cx14
        End Get
        Set(ByVal value As LimitMinMax)
            m_cx14 = value
        End Set
    End Property

    Private m_cy14 As New LimitMinMax
    Public Property Cy14() As LimitMinMax
        Get
            Return m_cy14
        End Get
        Set(ByVal value As LimitMinMax)
            m_cy14 = value
        End Set
    End Property

    Private m_phi14 As New LimitMinMax
    Public Property Phi14() As LimitMinMax
        Get
            Return m_phi14
        End Get
        Set(ByVal value As LimitMinMax)
            m_phi14 = value
        End Set
    End Property

    Private m_lambdaDom14 As New LimitMinMax
    Public Property LambdaDom14() As LimitMinMax
        Get
            Return m_lambdaDom14
        End Get
        Set(ByVal value As LimitMinMax)
            m_lambdaDom14 = value
        End Set
    End Property

    Private m_cx50 As New LimitMinMax
    Public Property Cx50() As LimitMinMax
        Get
            Return m_cx50
        End Get
        Set(ByVal value As LimitMinMax)
            m_cx50 = value
        End Set
    End Property

    Private m_cy50 As New LimitMinMax
    Public Property Cy50() As LimitMinMax
        Get
            Return m_cy50
        End Get
        Set(ByVal value As LimitMinMax)
            m_cy50 = value
        End Set
    End Property

    Private m_phi50 As New LimitMinMax
    Public Property Phi50() As LimitMinMax
        Get
            Return m_phi50
        End Get
        Set(ByVal value As LimitMinMax)
            m_phi50 = value
        End Set
    End Property

    Private m_lambdaDom50 As New LimitMinMax
    Public Property LambdaDom50() As LimitMinMax
        Get
            Return m_lambdaDom50
        End Get
        Set(ByVal value As LimitMinMax)
            m_lambdaDom50 = value
        End Set
    End Property
#End Region
End Class
