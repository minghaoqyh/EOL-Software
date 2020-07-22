Public Class MeasureLimits

#Region "set values"
    Private m_setIntensity As New LimitMinMax
    Public Property SetIntensity() As LimitMinMax
        Get
            Return m_setIntensity
        End Get
        Set(ByVal value As LimitMinMax)
            m_setIntensity = value
        End Set
    End Property
#End Region

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

    Private m_PD As New LimitSpecial
    Public Property PD() As LimitSpecial
        Get
            Return m_PD
        End Get
        Set(ByVal value As LimitSpecial)
            m_PD = value
        End Set
    End Property

#End Region

#Region "Cas Values"
    Private m_cx As New LimitMinMax
    Public Property Cx() As LimitMinMax
        Get
            Return m_cx
        End Get
        Set(ByVal value As LimitMinMax)
            m_cx = value
        End Set
    End Property

    Private m_cy As New LimitMinMax
    Public Property Cy() As LimitMinMax
        Get
            Return m_cy
        End Get
        Set(ByVal value As LimitMinMax)
            m_cy = value
        End Set
    End Property

    Private m_phi As New LimitMinMax
    Public Property Phi() As LimitMinMax
        Get
            Return m_phi
        End Get
        Set(ByVal value As LimitMinMax)
            m_phi = value
        End Set
    End Property

    Private m_cas_adc As New LimitMinMax
    Public Property CAS_ADC() As LimitMinMax
        Get
            Return m_cas_adc
        End Get
        Set(ByVal value As LimitMinMax)
            m_cas_adc = value
        End Set
    End Property

    Private m_filter As New LimitFixed
    Public Property Filter() As LimitFixed
        Get
            Return m_filter
        End Get
        Set(ByVal value As LimitFixed)
            m_filter = value
        End Set
    End Property

    Private m_averageCount As New LimitFixed
    Public Property AverageCount() As LimitFixed
        Get
            Return m_averageCount
        End Get
        Set(ByVal value As LimitFixed)
            m_averageCount = value
        End Set
    End Property

    Private m_integrationTime As New LimitMinMax
    Public Property IntegrationTime() As LimitMinMax
        Get
            Return m_integrationTime
        End Get
        Set(ByVal value As LimitMinMax)
            m_integrationTime = value
        End Set
    End Property

#End Region

#Region "Compensated Values"
    Private m_tj As New LimitMinMax
    Public Property Tj() As LimitMinMax
        Get
            Return m_tj
        End Get
        Set(ByVal value As LimitMinMax)
            m_tj = value
        End Set
    End Property

    Private m_cxTempComp As New LimitMinMax
    Public Property CxTempComp() As LimitMinMax
        Get
            Return m_cxTempComp
        End Get
        Set(ByVal value As LimitMinMax)
            m_cxTempComp = value
        End Set
    End Property

    Private m_cyTempComp As New LimitMinMax
    Public Property CyTempComp() As LimitMinMax
        Get
            Return m_cyTempComp
        End Get
        Set(ByVal value As LimitMinMax)
            m_cyTempComp = value
        End Set
    End Property

    Private m_phiTempComp As New LimitSpecial
    Public Property PhiTempComp() As LimitSpecial
        Get
            Return m_phiTempComp
        End Get
        Set(ByVal value As LimitSpecial)
            m_phiTempComp = value
        End Set
    End Property

#End Region

End Class
