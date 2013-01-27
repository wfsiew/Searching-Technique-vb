Imports System.Diagnostics

''' <summary>
''' Base class.
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BaseSolver
    ''' <summary>
    ''' The initial state of the 8 puzzle.
    ''' </summary>
    Protected initstate As Integer
    ''' <summary>
    ''' Used to capture the time taken for the Solve method to find the goal state.
    ''' </summary>
    Protected watch As Stopwatch

    Public Sub New(ByVal state As Integer)
        initstate = state
        watch = New Stopwatch
    End Sub

    Public MustOverride Sub Solve()
    Protected MustOverride Sub WriteSolution(ByVal solved As Boolean)
End Class
