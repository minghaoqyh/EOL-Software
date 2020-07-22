Public Class StateMachine
    Private m_ProgramFlow As List(Of SequenceStep) = New List(Of SequenceStep)
    Public Property ProgramFlow() As List(Of SequenceStep)
        Get
            Return m_ProgramFlow
        End Get
        Set(ByVal value As List(Of SequenceStep))
            m_ProgramFlow = value
        End Set
    End Property
    Private m_maxSteps As Integer
    Public Property MaxSteps() As Integer
        Get
            Return m_maxSteps
        End Get
        Set(ByVal value As Integer)
            m_maxSteps = value
        End Set
    End Property
    Private m_currentStep As Integer
    Public Property CurrentStep() As Integer
        Get
            Return m_currentStep
        End Get
        Set(ByVal value As Integer)
            m_currentStep = value
        End Set
    End Property
    Private m_abort As Boolean
    Public Property Abort() As Boolean
        Get
            Return m_abort
        End Get
        Set(ByVal value As Boolean)
            m_abort = value
        End Set
    End Property
End Class

Public Class SequenceStep

    Public Delegate Sub StepDelegate(ByVal stepName As String, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)

    Private m_stepName As String
    Public Property StepName() As String
        Get
            Return m_stepName
        End Get
        Set(ByVal value As String)
            m_stepName = value
        End Set
    End Property

    Private m_stepDelegate As StepDelegate
    Public Property StepDele() As StepDelegate
        Get
            Return m_stepDelegate
        End Get
        Set(ByVal value As StepDelegate)
            m_stepDelegate = value
        End Set
    End Property

    Private m_data0 As Object
    Public Property Data0() As Object
        Get
            Return m_data0
        End Get
        Set(ByVal value As Object)
            m_data0 = value
        End Set
    End Property

    Private m_data1 As Object
    Public Property Data1() As Object
        Get
            Return m_data1
        End Get
        Set(ByVal value As Object)
            m_data1 = value
        End Set
    End Property

    Private m_data2 As Object
    Public Property Data2() As Object
        Get
            Return m_data2
        End Get
        Set(ByVal value As Object)
            m_data2 = value
        End Set
    End Property

    Sub New(ByVal stepName As String, ByRef stepDel As StepDelegate, ByVal data0 As Object, ByVal data1 As Object, ByVal data2 As Object)
        m_stepName = stepName
        m_stepDelegate = stepDel
        m_data0 = data0
        m_data1 = data1
        m_data2 = data2
    End Sub

End Class

Public Class StateMachineStatus
    Private m_newText As String
    Public Property NewText() As String
        Get
            Return m_newText
        End Get
        Set(ByVal value As String)
            m_newText = value
        End Set
    End Property
    Private m_newTextColor As Color
    Public Property NewTextColor() As Color
        Get
            Return m_newTextColor
        End Get
        Set(ByVal value As Color)
            m_newTextColor = value
        End Set
    End Property
    Private m_progress As Double
    Public Property Progress() As Double
        Get
            Return m_progress
        End Get
        Set(ByVal value As Double)
            m_progress = value
        End Set
    End Property
    Sub New(ByVal newText As String, ByVal newTextColor As Color, ByVal progress As Double)
        m_newText = newText
        m_newTextColor = newTextColor
        m_progress = progress
    End Sub


End Class
