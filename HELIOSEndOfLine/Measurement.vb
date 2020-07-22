Public Class Measurement
    Implements IEquatable(Of Measurement)
    Private m_measureValue As New MeasureValues
    Public Property MValue() As MeasureValues
        Get
            Return m_measureValue
        End Get
        Set(ByVal value As MeasureValues)
            m_measureValue = value
        End Set
    End Property
    Private m_limitValue As New MeasureLimits
    Public Property MLimit() As MeasureLimits
        Get
            Return m_limitValue
        End Get
        Set(ByVal value As MeasureLimits)
            m_limitValue = value
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
    Private m_stepNumber As Integer
    Public Property StepNumber() As Integer
        Get
            Return m_stepNumber
        End Get
        Set(ByVal value As Integer)
            m_stepNumber = value
        End Set
    End Property

    Sub New(ByVal color As String, ByVal stepNumber As Integer)
        m_color = color
        m_stepNumber = stepNumber
        m_limitValue = New MeasureLimits()
        m_measureValue = New MeasureValues()
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        End If
        Dim objAsMeasurement As Measurement = TryCast(obj, Measurement)
        If objAsMeasurement Is Nothing Then
            Return False
        Else
            Return Equals(objAsMeasurement)
        End If
    End Function

    Public Overloads Function Equals(other As Measurement) As Boolean _
        Implements IEquatable(Of Measurement).Equals
        If other Is Nothing Then
            Return False
        End If
        If Me.Color.Equals(other.Color) And Me.StepNumber.Equals(other.StepNumber) Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
