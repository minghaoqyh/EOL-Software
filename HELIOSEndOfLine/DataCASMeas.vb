Public Class DataCASMeas
    Private m_cx As Double
    Private m_cy As Double
    Private m_lambdaPeak As Double
    Private m_lambdaDom As Double
    Private m_cri As Double
    Private m_r9 As Double
    Private m_r13 As Double
    Private m_r15 As Double
    Private m_CCT As Double
    Private m_spectrum As Double(,)
    Private m_radIntegral As Double
    Private m_photoIntegal As Double

    Private m_MacAdam As Double

    Private m_pathSpecta As String
    Private m_filenameSpecta As String

    Public Property Cx As Double
        Get
            Return m_cx
        End Get
        Set(value As Double)
            m_cx = value
        End Set
    End Property

    Public Property Cy As Double
        Get
            Return m_cy
        End Get
        Set(value As Double)
            m_cy = value
        End Set
    End Property

    Public Property LambdaPeak As Double
        Get
            Return m_lambdaPeak
        End Get
        Set(value As Double)
            m_lambdaPeak = value
        End Set
    End Property

    Public Property LambdaDom As Double
        Get
            Return m_lambdaDom
        End Get
        Set(value As Double)
            m_lambdaDom = value
        End Set
    End Property

    Public Property CRI As Double
        Get
            Return m_cri
        End Get
        Set(value As Double)
            m_cri = value
        End Set
    End Property

    Public Property R9 As Double
        Get
            Return m_r9
        End Get
        Set(value As Double)
            m_r9 = value
        End Set
    End Property

    Public Property R13 As Double
        Get
            Return m_r13
        End Get
        Set(value As Double)
            m_r13 = value
        End Set
    End Property

    Public Property R15 As Double
        Get
            Return m_r15
        End Get
        Set(value As Double)
            m_r15 = value
        End Set
    End Property

    Public Property CCT As Double
        Get
            Return m_CCT
        End Get
        Set(ByVal value As Double)
            m_CCT = value
        End Set
    End Property


    Public Property RadIntegral As Double
        Get
            Return m_radIntegral
        End Get
        Set(value As Double)
            m_radIntegral = value
        End Set
    End Property

    Public Property PhotoIntegral As Double
        Get
            Return m_photoIntegal
        End Get
        Set(value As Double)
            m_photoIntegal = value
        End Set
    End Property

    Public Property Spectrum As Double(,)
        Get
            Return m_spectrum
        End Get
        Set(value As Double(,))
            m_spectrum = value
        End Set
    End Property


    Public Property MacAdam As Double
        Get
            Return m_MacAdam
        End Get
        Set(value As Double)
            m_MacAdam = value
        End Set
    End Property

    Public Property PathSpectra() As String
        Get
            Return m_pathSpecta
        End Get
        Set(ByVal value As String)
            m_pathSpecta = value
        End Set
    End Property

    Public Property FilenameSpectra() As String
        Get
            Return m_filenameSpecta
        End Get
        Set(ByVal value As String)
            m_filenameSpecta = value
        End Set
    End Property
End Class
