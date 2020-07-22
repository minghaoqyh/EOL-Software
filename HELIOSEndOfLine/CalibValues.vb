Public Class CalibValues

#Region "Properties Calibration Values"
    Private m_current As New CalibCurrent
    Public Property Current() As CalibCurrent
        Get
            Return m_current
        End Get
        Set(ByVal value As CalibCurrent)
            m_current = value
        End Set
    End Property

    Private m_voltage As New CalibVoltage
    Public Property Voltage() As CalibVoltage
        Get
            Return m_voltage
        End Get
        Set(ByVal value As CalibVoltage)
            m_voltage = value
        End Set
    End Property

    Private m_dac As New CalibDac
    Public Property Dac() As CalibDac
        Get
            Return m_dac
        End Get
        Set(ByVal value As CalibDac)
            m_dac = value
        End Set
    End Property

    Private m_cx As New CalibColorCoord
    Public Property Cx() As CalibColorCoord
        Get
            Return m_cx
        End Get
        Set(ByVal value As CalibColorCoord)
            m_cx = value
        End Set
    End Property

    Private m_cy As New CalibColorCoord
    Public Property Cy() As CalibColorCoord
        Get
            Return m_cy
        End Get
        Set(ByVal value As CalibColorCoord)
            m_cy = value
        End Set
    End Property

    Private m_tempChip As New CalibTempChip
    Public Property TempChip() As CalibTempChip
        Get
            Return m_tempChip
        End Get
        Set(ByVal value As CalibTempChip)
            m_tempChip = value
        End Set
    End Property

    Private m_phi As New CalibPhi
    Public Property Phi() As CalibPhi
        Get
            Return m_phi
        End Get
        Set(ByVal value As CalibPhi)
            m_phi = value
        End Set
    End Property

    Private m_phiADC As New CalibPhiADC
    Public Property PhiADC() As CalibPhiADC
        Get
            Return m_phiADC
        End Get
        Set(ByVal value As CalibPhiADC)
            m_phiADC = value
        End Set
    End Property

    Private m_additional As New CalibAdditional
    Public Property Additional() As CalibAdditional
        Get
            Return m_additional
        End Get
        Set(ByVal value As CalibAdditional)
            m_additional = value
        End Set
    End Property

    Private m_phiMax As Double
    Public Property PhiMax() As Double
        Get
            Return m_phiMax
        End Get
        Set(ByVal value As Double)
            m_phiMax = value
        End Set
    End Property

    Private m_dacMax As Integer
    Public Property DacMax() As Integer
        Get
            Return m_dacMax
        End Get
        Set(ByVal value As Integer)
            m_dacMax = value
        End Set
    End Property

    Private m_iByDac As New CalibIByDac
    Public Property IByDac() As CalibIByDac
        Get
            Return m_iByDac
        End Get
        Set(ByVal value As CalibIByDac)
            m_iByDac = value
        End Set
    End Property

    Private m_uByI As New CalibUByI
    Public Property UByI() As CalibUByI
        Get
            Return m_uByI
        End Get
        Set(ByVal value As CalibUByI)
            m_uByI = value
        End Set
    End Property

    Private m_superR9 As New CalibSuperR9
    Public Property SuperR9() As CalibSuperR9
        Get
            Return m_superR9
        End Get
        Set(ByVal value As CalibSuperR9)
            m_superR9 = value
        End Set
    End Property

#End Region

End Class

Public Class CalibCurrent
    Private m_slope As Double
    Public Property Slope() As Double
        Get
            Return m_slope
        End Get
        Set(ByVal value As Double)
            m_slope = value
        End Set
    End Property
    Private m_offset As Double
    Public Property Offset() As Double
        Get
            Return m_offset
        End Get
        Set(ByVal value As Double)
            m_offset = value
        End Set
    End Property
End Class

Public Class CalibVoltage
    Private m_slope As Double
    Public Property Slope() As Double
        Get
            Return m_slope
        End Get
        Set(ByVal value As Double)
            m_slope = value
        End Set
    End Property
    Private m_offset As Double
    Public Property Offset() As Double
        Get
            Return m_offset
        End Get
        Set(ByVal value As Double)
            m_offset = value
        End Set
    End Property
End Class

Public Class CalibDac
    Private m_slope As Double
    Public Property Slope() As Double
        Get
            Return m_slope
        End Get
        Set(ByVal value As Double)
            m_slope = value
        End Set
    End Property
    Private m_offset As Double
    Public Property Offset() As Double
        Get
            Return m_offset
        End Get
        Set(ByVal value As Double)
            m_offset = value
        End Set
    End Property
End Class

Public Class CalibColorCoord
#Region "I_low"
    Private m_i_low As New CubicFct
    Public Property I_low() As CubicFct
        Get
            Return m_i_low
        End Get
        Set(ByVal value As CubicFct)
            m_i_low = value
        End Set
    End Property
#End Region

#Region "T_low"
    Private m_t_low As New CubicFct
    Public Property T_low() As CubicFct
        Get
            Return m_t_low
        End Get
        Set(ByVal value As CubicFct)
            m_t_low = value
        End Set
    End Property
#End Region

#Region "I_high"
    Private m_i_high As New CubicFct
    Public Property I_high() As CubicFct
        Get
            Return m_i_high
        End Get
        Set(ByVal value As CubicFct)
            m_i_high = value
        End Set
    End Property
#End Region

#Region "T_high"
    Private m_t_high As New CubicFct
    Public Property T_high() As CubicFct
        Get
            Return m_t_high
        End Get
        Set(ByVal value As CubicFct)
            m_t_high = value
        End Set
    End Property
#End Region

#Region "border"
    Private m_i_border As Double
    Public Property I_border() As Double
        Get
            Return m_i_border
        End Get
        Set(ByVal value As Double)
            m_i_border = value
        End Set
    End Property
    Private m_t_border As Double
    Public Property T_border() As Double
        Get
            Return m_t_border
        End Get
        Set(ByVal value As Double)
            m_t_border = value
        End Set
    End Property
#End Region

End Class

Public Class CalibTempChip
    Private m_rth As Double
    Public Property Rth() As Double
        Get
            Return m_rth
        End Get
        Set(ByVal value As Double)
            m_rth = value
        End Set
    End Property

    Private m_eta As Double
    Public Property Eta() As Double
        Get
            Return m_eta
        End Get
        Set(ByVal value As Double)
            m_eta = value
        End Set
    End Property

End Class

Public Class CalibPhi
#Region "dependency of T"
    Private m_t As New CubicFct
    Public Property T() As CubicFct
        Get
            Return m_t
        End Get
        Set(ByVal value As CubicFct)
            m_t = value
        End Set
    End Property
#End Region
#Region "4 splines vor current"
    Private m_i_spl1 As New CubicFct
    Public Property I_spl1() As CubicFct
        Get
            Return m_i_spl1
        End Get
        Set(ByVal value As CubicFct)
            m_i_spl1 = value
        End Set
    End Property
    Private m_i_spl2 As New CubicFct
    Public Property I_spl2() As CubicFct
        Get
            Return m_i_spl2
        End Get
        Set(ByVal value As CubicFct)
            m_i_spl2 = value
        End Set
    End Property
    Private m_i_spl3 As New CubicFct
    Public Property I_spl3() As CubicFct
        Get
            Return m_i_spl3
        End Get
        Set(ByVal value As CubicFct)
            m_i_spl3 = value
        End Set
    End Property
    Private m_i_spl4 As New CubicFct
    Public Property I_spl4() As CubicFct
        Get
            Return m_i_spl4
        End Get
        Set(ByVal value As CubicFct)
            m_i_spl4 = value
        End Set
    End Property
#End Region
#Region "Finishing cubic fct"
    Private m_I_last As New CubicFct
    Public Property I_last() As CubicFct
        Get
            Return m_I_last
        End Get
        Set(ByVal value As CubicFct)
            m_I_last = value
        End Set
    End Property
#End Region
#Region "borders for current spline"
    Private m_border_I_1 As Double
    Public Property Border_I_1() As Double
        Get
            Return m_border_I_1
        End Get
        Set(ByVal value As Double)
            m_border_I_1 = value
        End Set
    End Property
    Private m_border_I_2 As Double
    Public Property Border_I_2() As Double
        Get
            Return m_border_I_2
        End Get
        Set(ByVal value As Double)
            m_border_I_2 = value
        End Set
    End Property
    Private m_border_I_3 As Double
    Public Property Border_I_3() As Double
        Get
            Return m_border_I_3
        End Get
        Set(ByVal value As Double)
            m_border_I_3 = value
        End Set
    End Property
    Private m_border_I_4 As Double
    Public Property Border_I_4() As Double
        Get
            Return m_border_I_4
        End Get
        Set(ByVal value As Double)
            m_border_I_4 = value
        End Set
    End Property
#End Region
End Class

Public Class CalibPhiADC
#Region "dependency of factor1"
    Private m_factor1 As New CubicFct
    Public Property Factor1() As CubicFct
        Get
            Return m_factor1
        End Get
        Set(ByVal value As CubicFct)
            m_factor1 = value
        End Set
    End Property
#End Region
#Region "dependency of factor 2"
    Private m_factor2 As New CubicFct
    Public Property Factor2() As CubicFct
        Get
            Return m_factor2
        End Get
        Set(ByVal value As CubicFct)
            m_factor2 = value
        End Set
    End Property
#End Region
#Region "dependency of PD by lambdaDom"
    Private m_pd_lambdaDom As New CubicFct
    Public Property PDLambdaDom() As CubicFct
        Get
            Return m_pd_lambdaDom
        End Get
        Set(ByVal value As CubicFct)
            m_pd_lambdaDom = value
        End Set
    End Property
#End Region
#Region "dependency of PD by temperature"
    Private m_pd_temp As New CubicFct
    Public Property PDTemp() As CubicFct
        Get
            Return m_pd_temp
        End Get
        Set(ByVal value As CubicFct)
            m_pd_temp = value
        End Set
    End Property
#End Region
#Region "offset PD"
    Private m_offset_pd As Double
    Public Property Offset_PD() As Double
        Get
            Return m_offset_pd
        End Get
        Set(ByVal value As Double)
            m_offset_pd = value
        End Set
    End Property
#End Region

#Region "dummy0"
    Private m_dummy0 As Double
    Public Property Dummy0() As Double
        Get
            Return m_dummy0
        End Get
        Set(ByVal value As Double)
            m_dummy0 = value
        End Set
    End Property
#End Region
End Class

Public Class CalibAdditional
    Private m_cx_5500K As Double
    Public Property Cx5500K() As Double
        Get
            Return m_cx_5500K
        End Get
        Set(ByVal value As Double)
            m_cx_5500K = value
        End Set
    End Property
    Private m_cy_5500K As Double
    Public Property Cy5500K() As Double
        Get
            Return m_cy_5500K
        End Get
        Set(ByVal value As Double)
            m_cy_5500K = value
        End Set
    End Property
    Private m_dummy_02 As Double
    Public Property Dummy02() As Double
        Get
            Return m_dummy_02
        End Get
        Set(ByVal value As Double)
            m_dummy_02 = value
        End Set
    End Property
    Private m_dummy_03 As Double
    Public Property Dummy03() As Double
        Get
            Return m_dummy_03
        End Get
        Set(ByVal value As Double)
            m_dummy_03 = value
        End Set
    End Property

    Private m_dummy_10 As Double
    Public Property Dummy10() As Double
        Get
            Return m_dummy_10
        End Get
        Set(ByVal value As Double)
            m_dummy_10 = value
        End Set
    End Property
    Private m_dummy_11 As Double
    Public Property Dummy11() As Double
        Get
            Return m_dummy_11
        End Get
        Set(ByVal value As Double)
            m_dummy_11 = value
        End Set
    End Property
    Private m_dummy_12 As Double
    Public Property Dummy12() As Double
        Get
            Return m_dummy_12
        End Get
        Set(ByVal value As Double)
            m_dummy_12 = value
        End Set
    End Property
    Private m_dummy_13 As Double
    Public Property Dummy13() As Double
        Get
            Return m_dummy_13
        End Get
        Set(ByVal value As Double)
            m_dummy_13 = value
        End Set
    End Property

    Private m_dummy_20 As Double
    Public Property Dummy20() As Double
        Get
            Return m_dummy_20
        End Get
        Set(ByVal value As Double)
            m_dummy_20 = value
        End Set
    End Property
    Private m_dummy_21 As Double
    Public Property Dummy21() As Double
        Get
            Return m_dummy_21
        End Get
        Set(ByVal value As Double)
            m_dummy_21 = value
        End Set
    End Property
    
End Class

Public Class CalibIByDac
    Private m_fct As New CubicFct
    Public Property Fct() As CubicFct
        Get
            Return m_fct
        End Get
        Set(ByVal value As CubicFct)
            m_fct = value
        End Set
    End Property
End Class

Public Class CalibUByI
    Private m_fct As New CubicFct
    Public Property Fct() As CubicFct
        Get
            Return m_fct
        End Get
        Set(ByVal value As CubicFct)
            m_fct = value
        End Set
    End Property
End Class

Public Class CalibSuperR9
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
End Class

Public Class CubicFct
    Private m_f_x3 As Double
    Public Property F_x3() As Double
        Get
            Return m_f_x3
        End Get
        Set(ByVal value As Double)
            m_f_x3 = value
        End Set
    End Property
    Private m_f_x2 As Double
    Public Property F_x2() As Double
        Get
            Return m_f_x2
        End Get
        Set(ByVal value As Double)
            m_f_x2 = value
        End Set
    End Property
    Private m_f_x1 As Double
    Public Property F_x1() As Double
        Get
            Return m_f_x1
        End Get
        Set(ByVal value As Double)
            m_f_x1 = value
        End Set
    End Property
    Private m_f_x0 As Double
    Public Property F_x0() As Double
        Get
            Return m_f_x0
        End Get
        Set(ByVal value As Double)
            m_f_x0 = value
        End Set
    End Property
End Class
