Public Class DataHelios
    Private m_u1, m_u2, m_u3, m_u4 As Double
    Private m_i1, m_i2, m_i3, m_i4 As Double
    Private m_t1, m_t2, m_t3, m_t4, m_t5, m_t6 As Double
    Private m_sn As UInteger
    Private m_snUV As UInteger
    Private m_burnTime As UInteger
    Private m_uvStatus As Boolean
    Private m_version_main As UShort
    Private m_version_sub As UShort
    Private m_version_build As UShort
    Private m_version_state As UShort
    Private m_SN_saved As Byte
    Private m_SNV_saved As Byte
    Private m_PD5dark, m_PD5 As UShort

#Region "Properties"
    Public Property U1() As Double
        Get
            Return m_u1
        End Get
        Set(ByVal value As Double)
            m_u1 = value
        End Set
    End Property

    Public Property U2() As Double
        Get
            Return m_u2
        End Get
        Set(ByVal value As Double)
            m_u2 = value
        End Set
    End Property

    Public Property U3() As Double
        Get
            Return m_u3
        End Get
        Set(ByVal value As Double)
            m_u3 = value
        End Set
    End Property

    Public Property U4() As Double
        Get
            Return m_u4
        End Get
        Set(ByVal value As Double)
            m_u4 = value
        End Set
    End Property

    Public Property I1() As Double
        Get
            Return m_i1
        End Get
        Set(ByVal value As Double)
            m_i1 = value
        End Set
    End Property

    Public Property I2() As Double
        Get
            Return m_i2
        End Get
        Set(ByVal value As Double)
            m_i2 = value
        End Set
    End Property

    Public Property I3() As Double
        Get
            Return m_i3
        End Get
        Set(ByVal value As Double)
            m_i3 = value
        End Set
    End Property

    Public Property I4() As Double
        Get
            Return m_i4
        End Get
        Set(ByVal value As Double)
            m_i4 = value
        End Set
    End Property

    Public Property T1() As Double
        Get
            Return m_t1
        End Get
        Set(ByVal value As Double)
            m_t1 = value
        End Set
    End Property

    Public Property T2() As Double
        Get
            Return m_t2
        End Get
        Set(ByVal value As Double)
            m_t2 = value
        End Set
    End Property

    Public Property T3() As Double
        Get
            Return m_t3
        End Get
        Set(ByVal value As Double)
            m_t3 = value
        End Set
    End Property


    Public Property T4() As Double
        Get
            Return m_t4
        End Get
        Set(ByVal value As Double)
            m_t4 = value
        End Set
    End Property

    Public Property T5() As Double
        Get
            Return m_t5
        End Get
        Set(ByVal value As Double)
            m_t5 = value
        End Set
    End Property

    Public Property T6() As Double
        Get
            Return m_t6
        End Get
        Set(ByVal value As Double)
            m_t6 = value
        End Set
    End Property

    Public Property BurnTime() As UInteger
        Get
            Return m_burnTime
        End Get
        Set(ByVal value As UInteger)
            m_burnTime = value
        End Set
    End Property

    Public Property SN() As UInteger
        Get
            Return m_sn
        End Get
        Set(ByVal value As UInteger)
            m_sn = value
        End Set
    End Property

    Public Property SnUV() As UInteger
        Get
            Return m_snUV
        End Get
        Set(ByVal value As UInteger)
            m_snUV = value
        End Set
    End Property

    Public Property UVStatus() As Boolean
        Get
            Return m_uvStatus
        End Get
        Set(ByVal value As Boolean)
            m_uvStatus = value
        End Set
    End Property

    Public Property Version_main() As UShort
        Get
            Return m_version_main
        End Get
        Set(ByVal value As UShort)
            m_version_main = value
        End Set
    End Property

    Public Property Version_sub() As UShort
        Get
            Return m_version_sub
        End Get
        Set(ByVal value As UShort)
            m_version_sub = value
        End Set
    End Property

    Public Property Version_build() As UShort
        Get
            Return m_version_build
        End Get
        Set(ByVal value As UShort)
            m_version_build = value
        End Set
    End Property

    Public Property Version_state() As UShort
        Get
            Return m_version_state
        End Get
        Set(ByVal value As UShort)
            m_version_state = value
        End Set
    End Property

    Public Property SN_saved As Byte
        Get
            Return m_SN_saved
        End Get
        Set(value As Byte)
            m_SN_saved = value
        End Set
    End Property

    Public Property SNV_saved As Byte
        Get
            Return m_SNV_saved
        End Get
        Set(value As Byte)
            m_SNV_saved = value
        End Set
    End Property

    Public Property PD5 As UShort
        Get
            Return m_PD5
        End Get
        Set(value As UShort)
            m_PD5 = value
        End Set
    End Property

    Public Property PD5dark As UShort
        Get
            Return m_PD5dark
        End Get
        Set(value As UShort)
            m_PD5dark = value
        End Set
    End Property
#End Region

    Public Sub New()
        m_u1 = 0
        m_u2 = 0
        m_u3 = 0
        m_u4 = 0
        m_i1 = 0
        m_i2 = 0
        m_i3 = 0
        m_i4 = 0
        m_t1 = 0
        m_t2 = 0
        m_t3 = 0
        m_t4 = 0
        m_t5 = 0
        m_t6 = 0
        m_sn = 0
        m_snUV = 0
        m_burnTime = 0
        m_uvStatus = 0
        m_version_main = 0
        m_version_sub = 0
        m_version_build = 0
        m_version_state = 0
        m_SN_saved = 0
        m_SNV_saved = 0
        m_PD5 = 0
        m_PD5dark = 0
    End Sub
End Class
