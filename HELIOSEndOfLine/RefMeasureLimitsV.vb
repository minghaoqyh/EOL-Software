Public Class RefMeasureLimitsV

#Region "Helios values"
    Private m_current As New LimitMinMax
    Public Property Current() As LimitMinMax
        Get
            Return m_current
        End Get
        Set(ByVal value As LimitMinMax)
            m_current = value
        End Set
    End Property

    Private m_voltage As New LimitMinMax
    Public Property Voltage() As LimitMinMax
        Get
            Return m_voltage
        End Get
        Set(ByVal value As LimitMinMax)
            m_voltage = value
        End Set
    End Property

    Private m_temperature As New LimitMinMax
    Public Property Temperature() As LimitMinMax
        Get
            Return m_temperature
        End Get
        Set(ByVal value As LimitMinMax)
            m_temperature = value
        End Set
    End Property
#End Region
#Region "Cas Values"
    Private m_Popt As New LimitMinMax
    Public Property Popt() As LimitMinMax
        Get
            Return m_Popt
        End Get
        Set(ByVal value As LimitMinMax)
            m_Popt = value
        End Set
    End Property

    Private m_lambdaPeak As New LimitMinMax
    Public Property lambdaPeak() As LimitMinMax
        Get
            Return m_lambdaPeak
        End Get
        Set(ByVal value As LimitMinMax)
            m_lambdaPeak = value
        End Set
    End Property
#End Region
End Class
