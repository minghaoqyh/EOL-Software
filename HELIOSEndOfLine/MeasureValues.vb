Public Class MeasureValues
#Region "set values"
    Private m_setIntensity As Double
    Public Property SetIntensity() As Double
        Get
            Return m_setIntensity
        End Get
        Set(ByVal value As Double)
            m_setIntensity = value
        End Set
    End Property
#End Region

#Region "Helios values"
    Private m_current As Double
    Public Property Current() As Double
        Get
            Return m_current
        End Get
        Set(ByVal value As Double)
            m_current = value
        End Set
    End Property

    Private m_voltage As Double
    Public Property Voltage() As Double
        Get
            Return m_voltage
        End Get
        Set(ByVal value As Double)
            m_voltage = value
        End Set
    End Property

    Private m_temperature As Double
    Public Property Temperature() As Double
        Get
            Return m_temperature
        End Get
        Set(ByVal value As Double)
            m_temperature = value
        End Set
    End Property

    Private m_PD As Integer
    Public Property PD() As Integer
        Get
            Return m_PD
        End Get
        Set(ByVal value As Integer)
            m_PD = value
        End Set
    End Property

#End Region

#Region "Cas Values"
    Private m_cx As Double
    Public Property Cx() As Double
        Get
            Return m_cx
        End Get
        Set(ByVal value As Double)
            m_cx = value
        End Set
    End Property

    Private m_cy As Double
    Public Property Cy() As Double
        Get
            Return m_cy
        End Get
        Set(ByVal value As Double)
            m_cy = value
        End Set
    End Property

    Private m_phi As Double
    Public Property Phi() As Double
        Get
            Return m_phi
        End Get
        Set(ByVal value As Double)
            m_phi = value
        End Set
    End Property

    Private m_cas_adc As Integer
    Public Property CAS_ADC() As Integer
        Get
            Return m_cas_adc
        End Get
        Set(ByVal value As Integer)
            m_cas_adc = value
        End Set
    End Property

    Private m_filter As Integer
    Public Property Filter() As Integer
        Get
            Return m_filter
        End Get
        Set(ByVal value As Integer)
            m_filter = value
        End Set
    End Property

    Private m_averageCount As Integer
    Public Property AverageCount() As Integer
        Get
            Return m_averageCount
        End Get
        Set(ByVal value As Integer)
            m_averageCount = value
        End Set
    End Property

    Private m_integrationTime As Integer
    Public Property IntegrationTime() As Integer
        Get
            Return m_integrationTime
        End Get
        Set(ByVal value As Integer)
            m_integrationTime = value
        End Set
    End Property

#End Region

#Region "Compensated Values"
    Private m_tj As Double
    Public Property Tj() As Double
        Get
            Return m_tj
        End Get
        Set(ByVal value As Double)
            m_tj = value
        End Set
    End Property

    Private m_cxTempComp As Double
    Public Property CxTempComp() As Double
        Get
            Return m_cxTempComp
        End Get
        Set(ByVal value As Double)
            m_cxTempComp = value
        End Set
    End Property

    Private m_cyTempComp As Double
    Public Property CyTempComp() As Double
        Get
            Return m_cyTempComp
        End Get
        Set(ByVal value As Double)
            m_cyTempComp = value
        End Set
    End Property

    Private m_phiTempComp As Double
    Public Property PhiTempComp() As Double
        Get
            Return m_phiTempComp
        End Get
        Set(ByVal value As Double)
            m_phiTempComp = value
        End Set
    End Property

    Private m_intensityDAC As Double
    Public Property IntensityDAC() As Double
        Get
            Return m_intensityDAC
        End Get
        Set(ByVal value As Double)
            m_intensityDAC = value
        End Set
    End Property

    Private m_tempExtern As Double
    Public Property TempExtern() As Double
        Get
            Return m_tempExtern
        End Get
        Set(ByVal value As Double)
            m_tempExtern = value
        End Set
    End Property


#End Region

End Class
