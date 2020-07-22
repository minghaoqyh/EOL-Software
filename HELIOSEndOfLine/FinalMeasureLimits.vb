Public Class FinalMeasureLimits
#Region "5500K, 100%"
    Private m_currentR As New LimitMinMax
    Public Property CurrentR() As LimitMinMax
        Get
            Return m_currentR
        End Get
        Set(ByVal value As LimitMinMax)
            m_currentR = value
        End Set
    End Property
    Private m_currentG As New LimitMinMax
    Public Property CurrentG() As LimitMinMax
        Get
            Return m_currentG
        End Get
        Set(ByVal value As LimitMinMax)
            m_currentG = value
        End Set
    End Property
    Private m_currentB As New LimitMinMax
    Public Property CurrentB() As LimitMinMax
        Get
            Return m_currentB
        End Get
        Set(ByVal value As LimitMinMax)
            m_currentB = value
        End Set
    End Property
    Private m_Phi5500K100 As New LimitMinMax
    Public Property Phi5500K100() As LimitMinMax
        Get
            Return m_Phi5500K100
        End Get
        Set(ByVal value As LimitMinMax)
            m_Phi5500K100 = value
        End Set
    End Property
    Private m_CRI5500K100 As New LimitMinMax
    Public Property CRI5500K100() As LimitMinMax
        Get
            Return m_CRI5500K100
        End Get
        Set(ByVal value As LimitMinMax)
            m_CRI5500K100 = value
        End Set
    End Property
    Private m_R95500K100 As New LimitMinMax
    Public Property R95500K100() As LimitMinMax
        Get
            Return m_R95500K100
        End Get
        Set(ByVal value As LimitMinMax)
            m_R95500K100 = value
        End Set
    End Property
    Private m_MacAdam5500K100 As New LimitMinMax
    Public Property MacAdam5500K100() As LimitMinMax
        Get
            Return m_MacAdam5500K100
        End Get
        Set(ByVal value As LimitMinMax)
            m_MacAdam5500K100 = value
        End Set
    End Property
    Private m_SuperCx5500K100 As New LimitMinMax
    Public Property SuperCx5500K100() As LimitMinMax
        Get
            Return m_SuperCx5500K100
        End Get
        Set(ByVal value As LimitMinMax)
            m_SuperCx5500K100 = value
        End Set
    End Property
    Private m_SuperCy5500K100 As New LimitMinMax
    Public Property SuperCy5500K100() As LimitMinMax
        Get
            Return m_SuperCy5500K100
        End Get
        Set(ByVal value As LimitMinMax)
            m_SuperCy5500K100 = value
        End Set
    End Property
    Private m_SuperR95500K100 As New LimitMinMax
    Public Property SuperR95500K100() As LimitMinMax
        Get
            Return m_SuperR95500K100
        End Get
        Set(ByVal value As LimitMinMax)
            m_SuperR95500K100 = value
        End Set
    End Property

#End Region
#Region "5500K, 5%"
    Private m_Phi5500K005 As New LimitMinMax
    Public Property Phi5500K005() As LimitMinMax
        Get
            Return m_Phi5500K005
        End Get
        Set(ByVal value As LimitMinMax)
            m_Phi5500K005 = value
        End Set
    End Property
    Private m_MacAdam5500K005 As New LimitMinMax
    Public Property MacAdam5500K005() As LimitMinMax
        Get
            Return m_MacAdam5500K005
        End Get
        Set(ByVal value As LimitMinMax)
            m_MacAdam5500K005 = value
        End Set
    End Property
#End Region
#Region "5500K 100% low intensity"
    Private m_Phi5500K100low As New LimitMinMax
    Public Property Phi5500K100low() As LimitMinMax
        Get
            Return m_Phi5500K100low
        End Get
        Set(ByVal value As LimitMinMax)
            m_Phi5500K100low = value
        End Set
    End Property
    Private m_MacAdam5500K100low As New LimitMinMax
    Public Property MacAdam5500K100low() As LimitMinMax
        Get
            Return m_MacAdam5500K100low
        End Get
        Set(ByVal value As LimitMinMax)
            m_MacAdam5500K100low = value
        End Set
    End Property
#End Region
End Class
