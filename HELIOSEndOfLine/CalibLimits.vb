Public Class CalibLimits

#Region "Properties Limit of Calibration Values"

    Private m_current As New CalibLimitCurrent
    Public Property Current() As CalibLimitCurrent
        Get
            Return m_current
        End Get
        Set(ByVal value As CalibLimitCurrent)
            m_current = value
        End Set
    End Property

    Private m_voltage As New CalibLimitVoltage
    Public Property Voltage() As CalibLimitVoltage
        Get
            Return m_voltage
        End Get
        Set(ByVal value As CalibLimitVoltage)
            m_voltage = value
        End Set
    End Property
    Private m_dac As New CalibLimitDac
    Public Property Dac() As CalibLimitDac
        Get
            Return m_dac
        End Get
        Set(ByVal value As CalibLimitDac)
            m_dac = value
        End Set
    End Property
    Private m_cx As New CalibLimitColorCoord
    Public Property Cx() As CalibLimitColorCoord
        Get
            Return m_cx
        End Get
        Set(ByVal value As CalibLimitColorCoord)
            m_cx = value
        End Set
    End Property
    Private m_cy As New CalibLimitColorCoord
    Public Property Cy() As CalibLimitColorCoord
        Get
            Return m_cy
        End Get
        Set(ByVal value As CalibLimitColorCoord)
            m_cy = value
        End Set
    End Property
    Private m_tempChip As New CalibLimitTempChip
    Public Property TempChip() As CalibLimitTempChip
        Get
            Return m_tempChip
        End Get
        Set(ByVal value As CalibLimitTempChip)
            m_tempChip = value
        End Set
    End Property
    Private m_phi As New CalibLimitPhi
    Public Property Phi() As CalibLimitPhi
        Get
            Return m_phi
        End Get
        Set(ByVal value As CalibLimitPhi)
            m_phi = value
        End Set
    End Property
    Private m_phiADC As New CalibLimitPhiADC
    Public Property PhiADC() As CalibLimitPhiADC
        Get
            Return m_phiADC
        End Get
        Set(ByVal value As CalibLimitPhiADC)
            m_phiADC = value
        End Set
    End Property
    Private m_phiMax As New LimitMinMax
    Public Property PhiMax() As LimitMinMax
        Get
            Return m_phiMax
        End Get
        Set(ByVal value As LimitMinMax)
            m_phiMax = value
        End Set
    End Property
    Private m_dacMax As New LimitMinMax
    Public Property DacMax() As LimitMinMax
        Get
            Return m_dacMax
        End Get
        Set(ByVal value As LimitMinMax)
            m_dacMax = value
        End Set
    End Property

    Private m_iByDac As New CalibLimitIByDac
    Public Property IByDac() As CalibLimitIByDac
        Get
            Return m_iByDac
        End Get
        Set(ByVal value As CalibLimitIByDac)
            m_iByDac = value
        End Set
    End Property
    Private m_uByI As New CalibLimitUByI
    Public Property UByI() As CalibLimitUByI
        Get
            Return m_uByI
        End Get
        Set(ByVal value As CalibLimitUByI)
            m_uByI = value
        End Set
    End Property
    Private m_superR9 As New CalibLimitSuperR9
    Public Property SuperR9() As CalibLimitSuperR9
        Get
            Return m_superR9
        End Get
        Set(ByVal value As CalibLimitSuperR9)
            m_superR9 = value
        End Set
    End Property
#End Region

End Class

Public Class CalibLimitCurrent
    Private m_slope As New LimitMinMax
    Public Property Slope() As LimitMinMax
        Get
            Return m_slope
        End Get
        Set(ByVal value As LimitMinMax)
            m_slope = value
        End Set
    End Property
    Private m_offset As New LimitMinMax
    Public Property Offset() As LimitMinMax
        Get
            Return m_offset
        End Get
        Set(ByVal value As LimitMinMax)
            m_offset = value
        End Set
    End Property
End Class

Public Class CalibLimitVoltage
    Private m_slope As New LimitMinMax
    Public Property Slope() As LimitMinMax
        Get
            Return m_slope
        End Get
        Set(ByVal value As LimitMinMax)
            m_slope = value
        End Set
    End Property
    Private m_offset As New LimitMinMax
    Public Property Offset() As LimitMinMax
        Get
            Return m_offset
        End Get
        Set(ByVal value As LimitMinMax)
            m_offset = value
        End Set
    End Property
End Class

Public Class CalibLimitDac
    Private m_slope As New LimitMinMax
    Public Property Slope() As LimitMinMax
        Get
            Return m_slope
        End Get
        Set(ByVal value As LimitMinMax)
            m_slope = value
        End Set
    End Property
    Private m_offset As New LimitMinMax
    Public Property Offset() As LimitMinMax
        Get
            Return m_offset
        End Get
        Set(ByVal value As LimitMinMax)
            m_offset = value
        End Set
    End Property
End Class

Public Class CalibLimitColorCoord
#Region "I_low"
    Private m_i_low As New LimitCubicFct
    Public Property I_low() As LimitCubicFct
        Get
            Return m_i_low
        End Get
        Set(ByVal value As LimitCubicFct)
            m_i_low = value
        End Set
    End Property
#End Region
#Region "T_low"
    Private m_t_low As New LimitCubicFctFixed
    Public Property T_low() As LimitCubicFctFixed
        Get
            Return m_t_low
        End Get
        Set(ByVal value As LimitCubicFctFixed)
            m_t_low = value
        End Set
    End Property
#End Region
#Region "I_high"
    Private m_i_high As New LimitCubicFct
    Public Property I_high() As LimitCubicFct
        Get
            Return m_i_high
        End Get
        Set(ByVal value As LimitCubicFct)
            m_i_high = value
        End Set
    End Property
#End Region
#Region "T_high"
    Private m_t_high As New LimitCubicFctFixed
    Public Property T_high() As LimitCubicFctFixed
        Get
            Return m_t_high
        End Get
        Set(ByVal value As LimitCubicFctFixed)
            m_t_high = value
        End Set
    End Property
#End Region
#Region "border"
    Private m_i_border As New LimitFixed
    Public Property I_Border() As LimitFixed
        Get
            Return m_i_border
        End Get
        Set(ByVal value As LimitFixed)
            m_i_border = value
        End Set
    End Property
    Private m_t_border As New LimitFixed
    Public Property T_Border() As LimitFixed
        Get
            Return m_t_border
        End Get
        Set(ByVal value As LimitFixed)
            m_t_border = value
        End Set
    End Property
#End Region
End Class

Public Class CalibLimitTempChip
    Private m_rth As New LimitFixed
    Public Property Rth() As LimitFixed
        Get
            Return m_rth
        End Get
        Set(ByVal value As LimitFixed)
            m_rth = value
        End Set
    End Property
    Private m_eta As New LimitFixed
    Public Property Eta() As LimitFixed
        Get
            Return m_eta
        End Get
        Set(ByVal value As LimitFixed)
            m_eta = value
        End Set
    End Property
End Class

Public Class CalibLimitPhi
#Region "dependency of T"
    Private m_t As New LimitCubicFctFixed
    Public Property T() As LimitCubicFctFixed
        Get
            Return m_t
        End Get
        Set(ByVal value As LimitCubicFctFixed)
            m_t = value
        End Set
    End Property
#End Region
#Region "4 splines vor current"
    Private m_i_spl1 As New LimitCubicFctSpline
    Public Property I_spl1() As LimitCubicFctSpline
        Get
            Return m_i_spl1
        End Get
        Set(ByVal value As LimitCubicFctSpline)
            m_i_spl1 = value
        End Set
    End Property
    Private m_i_spl2 As New LimitCubicFctSpline
    Public Property I_spl2() As LimitCubicFctSpline
        Get
            Return m_i_spl2
        End Get
        Set(ByVal value As LimitCubicFctSpline)
            m_i_spl2 = value
        End Set
    End Property
    Private m_i_spl3 As New LimitCubicFctSpline
    Public Property I_spl3() As LimitCubicFctSpline
        Get
            Return m_i_spl3
        End Get
        Set(ByVal value As LimitCubicFctSpline)
            m_i_spl3 = value
        End Set
    End Property
    Private m_i_spl4 As New LimitCubicFctSpline
    Public Property I_spl4() As LimitCubicFctSpline
        Get
            Return m_i_spl4
        End Get
        Set(ByVal value As LimitCubicFctSpline)
            m_i_spl4 = value
        End Set
    End Property
#End Region
#Region "Finishing cubic fct"
    Private m_I_last As New LimitCubicFct
    Public Property I_last() As LimitCubicFct
        Get
            Return m_I_last
        End Get
        Set(ByVal value As LimitCubicFct)
            m_I_last = value
        End Set
    End Property
#End Region
#Region "borders for current spline"
    Private m_border_I_1 As New LimitMinMax
    Public Property Border_I_1() As LimitMinMax
        Get
            Return m_border_I_1
        End Get
        Set(ByVal value As LimitMinMax)
            m_border_I_1 = value
        End Set
    End Property
    Private m_border_I_2 As New LimitMinMax
    Public Property Border_I_2() As LimitMinMax
        Get
            Return m_border_I_2
        End Get
        Set(ByVal value As LimitMinMax)
            m_border_I_2 = value
        End Set
    End Property
    Private m_border_I_3 As New LimitMinMax
    Public Property Border_I_3() As LimitMinMax
        Get
            Return m_border_I_3
        End Get
        Set(ByVal value As LimitMinMax)
            m_border_I_3 = value
        End Set
    End Property
    Private m_border_I_4 As New LimitMinMax
    Public Property Border_I_4() As LimitMinMax
        Get
            Return m_border_I_4
        End Get
        Set(ByVal value As LimitMinMax)
            m_border_I_4 = value
        End Set
    End Property
#End Region
End Class

Public Class CalibLimitPhiADC
#Region "dependency of T"
    Private m_t As New LimitCubicFct
    Public Property T() As LimitCubicFct
        Get
            Return m_t
        End Get
        Set(ByVal value As LimitCubicFct)
            m_t = value
        End Set
    End Property
#End Region
#Region "dependency of I"
    Private m_i As New LimitCubicFct
    Public Property I() As LimitCubicFct
        Get
            Return m_i
        End Get
        Set(ByVal value As LimitCubicFct)
            m_i = value
        End Set
    End Property
#End Region
#Region "dependency of ADC"
    Private m_adc_spl1 As New LimitCubicFct
    Public Property Adc_spl1() As LimitCubicFct
        Get
            Return m_adc_spl1
        End Get
        Set(ByVal value As LimitCubicFct)
            m_adc_spl1 = value
        End Set
    End Property
    Private m_adc_spl2 As New LimitCubicFct
    Public Property Adc_spl2() As LimitCubicFct
        Get
            Return m_adc_spl2
        End Get
        Set(ByVal value As LimitCubicFct)
            m_adc_spl2 = value
        End Set
    End Property
    Private m_adc_spl3 As New LimitCubicFct
    Public Property Adc_spl3() As LimitCubicFct
        Get
            Return m_adc_spl3
        End Get
        Set(ByVal value As LimitCubicFct)
            m_adc_spl3 = value
        End Set
    End Property
    Private m_adc_last As New LimitCubicFct
    Public Property Adc_last() As LimitCubicFct
        Get
            Return m_adc_last
        End Get
        Set(ByVal value As LimitCubicFct)
            m_adc_last = value
        End Set
    End Property
#Region "border of adc splines"
    Private m_border_adc_1 As New LimitMinMax
    Public Property Border_Adc_1() As LimitMinMax
        Get
            Return m_border_adc_1
        End Get
        Set(ByVal value As LimitMinMax)
            m_border_adc_1 = value
        End Set
    End Property
    Private m_border_adc_2 As New LimitMinMax
    Public Property Border_Adc_2() As LimitMinMax
        Get
            Return m_border_adc_2
        End Get
        Set(ByVal value As LimitMinMax)
            m_border_adc_2 = value
        End Set
    End Property
    Private m_border_adc_3 As New LimitMinMax
    Public Property Border_Adc_3() As LimitMinMax
        Get
            Return m_border_adc_3
        End Get
        Set(ByVal value As LimitMinMax)
            m_border_adc_3 = value
        End Set
    End Property
#End Region
#End Region
End Class

Public Class CalibLimitIByDac
    Private m_fct As New LimitCubicFct
    Public Property Fct() As LimitCubicFct
        Get
            Return m_fct
        End Get
        Set(ByVal value As LimitCubicFct)
            m_fct = value
        End Set
    End Property
End Class

Public Class CalibLimitUByI
    Private m_fct As New LimitCubicFct
    Public Property Fct() As LimitCubicFct
        Get
            Return m_fct
        End Get
        Set(ByVal value As LimitCubicFct)
            m_fct = value
        End Set
    End Property
End Class

Public Class CalibLimitSuperR9
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
End Class