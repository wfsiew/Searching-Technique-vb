''' <summary>
''' A class which encapsulates a state in the 8 puzzle.
''' </summary>
Public Class State
    Implements IEquatable(Of State)

    ''' <summary>
    ''' The state value, represented in reversed order from the 9 slots in the 8 puzzle.
    ''' </summary>
    Private m_state As Integer
    ''' <summary>
    ''' The heuristic value of this state.
    ''' </summary>
    Private m_heuristicval As Integer
    ''' <summary>
    ''' The depth level of this state. The initial state will have level 0.
    ''' </summary>
    Private m_level As Integer
    ''' <summary>
    ''' The evaluation value of this state.
    ''' EValue = depth level + heuristic value
    ''' </summary>
    Private m_evalue As Integer
    ''' <summary>
    ''' A reference of the parent state.
    ''' </summary>
    Private m_parent As State

    Public Sub New(ByVal s As Integer)
        Me.New(s, 0)
    End Sub

    Public Sub New(ByVal s As Integer, ByVal hval As Integer)
        m_state = s
        m_heuristicval = hval
        Parent = Nothing
    End Sub

    ''' <summary>
    ''' Gets the state value.
    ''' </summary>
    Public ReadOnly Property StateValue As Integer
        Get
            Return m_state
        End Get
    End Property

    ''' <summary>
    ''' Gets/Sets the heuristic value.
    ''' </summary>
    Public Property HeuristicValue As Integer
        Get
            Return m_heuristicval
        End Get
        Set(value As Integer)
            m_heuristicval = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the depth level.
    ''' </summary>
    Public ReadOnly Property Level As Integer
        Get
            Return m_level
        End Get
    End Property

    ''' <summary>
    ''' Gets the evaluation value.
    ''' </summary>
    Public ReadOnly Property EValue As Integer
        Get
            Return m_evalue
        End Get
    End Property

    ''' <summary>
    ''' Gets/Sets the reference of the parent state.
    ''' </summary>
    Public Property Parent As State
        Get
            Return m_parent
        End Get
        Set(value As State)
            m_parent = value
            If Not value Is Nothing Then
                m_level = value.Level + 1

            Else
                m_level = 0
            End If

            m_evalue = Level + HeuristicValue
        End Set
    End Property

    ''' <summary>
    ''' Indicates whether the current object is equal to another object of the same type.
    ''' </summary>
    ''' <returns>True if the state of this object is equal to another object's state, False otherwise.</returns>
    Public Shadows Function Equals(other As State) As Boolean Implements IEquatable(Of State).Equals
        Return m_state = other.m_state
    End Function
End Class
