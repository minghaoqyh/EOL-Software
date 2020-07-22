Public Class DataPS
    Private m_voltage As Double
    Private m_current As Double

#Region "Properties"
    Public Property Voltage() As Double
        Get
            Return m_voltage
        End Get
        Set(ByVal value As Double)
            m_voltage = value
        End Set
    End Property

    Public Property Current() As Double
        Get
            Return m_current
        End Get
        Set(ByVal value As Double)
            m_current = value
        End Set
    End Property
#End Region

    Public Sub New()
        m_voltage = 0
        m_current = 0
    End Sub
End Class
