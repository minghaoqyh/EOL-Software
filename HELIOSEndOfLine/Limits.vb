Public Class LimitFixed
    Private m_fixed As Double
    Public Property Fixed() As Double
        Get
            Return m_fixed
        End Get
        Set(ByVal value As Double)
            m_fixed = value
        End Set
    End Property
    Public Function CheckLimit(ByVal value As Double) As Boolean
        If value = m_fixed Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function CheckLimit(ByVal value As Integer) As Boolean
        If value = m_fixed Then
            Return True
        Else
            Return False
        End If
    End Function
    
End Class

Public Class LimitCubicFctFixed
    Private m_fixed As New CubicFct
    Public Property Fixed() As CubicFct
        Get
            Return m_fixed
        End Get
        Set(ByVal value As CubicFct)
            m_fixed = value
        End Set
    End Property
    Public Function CheckLimit(ByVal value As CubicFct) As Boolean
        If Math.Abs(value.F_x0 - Me.m_fixed.F_x0) < Single.Epsilon And _
           Math.Abs(value.F_x1 - Me.m_fixed.F_x1) < Single.Epsilon And _
           Math.Abs(value.F_x2 - Me.m_fixed.F_x2) < Single.Epsilon And _
           Math.Abs(value.F_x3 - Me.m_fixed.F_x3) < Single.Epsilon Then
            Return True
        Else
            Return False
        End If
    End Function
End Class

Public Class LimitMinMax
    Private m_min As Double
    Public Property Min() As Double
        Get
            Return m_min
        End Get
        Set(ByVal value As Double)
            m_min = value
        End Set
    End Property

    Private m_max As Double
    Public Property Max() As Double
        Get
            Return m_max
        End Get
        Set(ByVal value As Double)
            m_max = value
        End Set
    End Property

    Public Function CheckLimit(ByVal value As Double) As Boolean
        If ((value >= m_min) And (value <= m_max)) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function CheckLimit(ByVal value As Integer) As Boolean
        If ((value >= m_min) And (value <= m_max)) Then
            Return True
        Else
            Return False
        End If
    End Function

End Class

Public Class LimitSpecial
    Private m_min As Double
    Public Property Min() As Double
        Get
            Return m_min
        End Get
        Set(ByVal value As Double)
            m_min = value
        End Set
    End Property

    Private m_max As Double
    Public Property Max() As Double
        Get
            Return m_max
        End Get
        Set(ByVal value As Double)
            m_max = value
        End Set
    End Property

    Private m_factor As Double
    Public Property Factor() As Double
        Get
            Return m_factor
        End Get
        Set(ByVal value As Double)
            m_factor = value
        End Set
    End Property
    Public Function CheckLimit(ByVal value As Double) As Boolean
        If ((value >= m_min) And (value <= m_max)) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function CheckLimit(ByVal value As Integer) As Boolean
        If ((value >= Me.m_min) And (value <= Me.m_max)) Then
            Return True
        Else
            Return False
        End If
    End Function
End Class

Public Class LimitCubicFct
    Private m_devBottomPercent As Double
    Public Property DevBottomPercent() As Double
        Get
            Return m_devBottomPercent
        End Get
        Set(ByVal value As Double)
            m_devBottomPercent = value
        End Set
    End Property
    Private m_devTopPercent As Double
    Public Property DevTopPercent() As Double
        Get
            Return m_devTopPercent
        End Get
        Set(ByVal value As Double)
            m_devTopPercent = value
        End Set
    End Property
    Public Function CheckLimit(ByVal xx() As Double, ByVal yy() As Double, ByVal cubicFct As CubicFct, ByRef minRatio As Double, ByRef maxRatio As Double) As Boolean
        Dim returnValue As Boolean = True
        Dim count As Integer
        Dim value As Double
        count = xx.GetLength(0)
        minRatio = 1
        maxRatio = 1
        For i = 0 To count - 1
            value = cubicFct.F_x0 + cubicFct.F_x1 * xx(i) + cubicFct.F_x2 * xx(i) * xx(i) + cubicFct.F_x3 * xx(i) * xx(i) * xx(i)
            Dim ratio As Double
            ratio = value / yy(i)
            If ((ratio < (1 + Me.DevBottomPercent / 100)) Or (ratio > (1 + Me.DevTopPercent / 100))) Then
                returnValue = False
                If ratio < minRatio Then
                    minRatio = ratio
                End If
                If ratio > maxRatio Then
                    maxRatio = ratio
                End If
            End If
        Next
        Return returnValue
    End Function
End Class

Public Class LimitCubicFctSpline
    Private m_minDelta As Double
    Public Property MinDelta() As Double
        Get
            Return m_minDelta
        End Get
        Set(ByVal value As Double)
            m_minDelta = value
        End Set
    End Property
    Private m_maxDelta As Double
    Public Property MaxDelta() As Double
        Get
            Return m_maxDelta
        End Get
        Set(ByVal value As Double)
            m_maxDelta = value
        End Set
    End Property

    Public Function CheckLimit(ByVal xx() As Double, ByVal yy() As Double, ByVal cubicFct As CubicFct, ByRef midValue As Double) As Boolean
        Dim x(2) As Double
        Dim y2(2) As Double
        Dim yMid As Double
        x(0) = xx(0)
        x(1) = (xx(1) + xx(0)) / 2
        x(2) = xx(1)

        y2(0) = cubicFct.F_x3 * (x(0) ^ 3) + (cubicFct.F_x2 * x(0) ^ 2) + cubicFct.F_x1 * x(0) + cubicFct.F_x0
        y2(1) = cubicFct.F_x3 * (x(1) ^ 3) + (cubicFct.F_x2 * x(1) ^ 2) + cubicFct.F_x1 * x(1) + cubicFct.F_x0
        y2(2) = cubicFct.F_x3 * (x(2) ^ 3) + (cubicFct.F_x2 * x(2) ^ 2) + cubicFct.F_x1 * x(2) + cubicFct.F_x0

        yMid = (y2(0) + y2(2)) / 2
        midValue = yMid / y2(1)
        If ((yMid / y2(1) - 1) > MinDelta) And _
           ((yMid / y2(1) - 1) < MaxDelta) Then
            Return True
        Else
            Return False
        End If
    End Function
End Class