Public Class Calibration
    Private m_calibValue As CalibValues
    Public Property CValue() As CalibValues
        Get
            Return m_calibValue
        End Get
        Set(ByVal value As CalibValues)
            m_calibValue = value
        End Set
    End Property
    Private m_calibLimit As CalibLimits
    Public Property CLimit() As CalibLimits
        Get
            Return m_calibLimit
        End Get
        Set(ByVal value As CalibLimits)
            m_calibLimit = value
        End Set
    End Property
    Private m_color As String
    Public Property Color() As String
        Get
            Return m_color
        End Get
        Set(ByVal value As String)
            m_color = value
        End Set
    End Property

    Sub New(ByVal color As String)
        Me.Color = color
        Me.CLimit = New CalibLimits
        Me.CValue = New CalibValues
    End Sub

End Class
