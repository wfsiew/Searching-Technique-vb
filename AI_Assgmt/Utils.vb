Imports System.Text
Imports System.IO
Imports System.Xml

''' <summary>
''' Helper class which provides all the necessary static methods used in the entire application.
''' </summary>
Public Class Utils
    ''' <summary>
    ''' The initial state of the puzzle.
    ''' It will be overwritten by the init_state specified in the config.xml file.
    ''' </summary>
    Public Shared initstate As Integer = 250816743

    ''' <summary>
    ''' The goal state of the puzzle that the program will try to find.
    ''' It will be overwritten by the goal_state specified in the config.xml file.
    ''' </summary>
    Public Shared goalstate As Integer = 765408321

    ''' <summary>
    ''' The depth limit that will be used in Depth First Search to search up to
    ''' a certain level.
    ''' It will be overwritten by the depth_limit specified in the config.xml file
    ''' </summary>
    Public Shared depthlimit As Integer = 41

    ''' <summary>
    ''' The goal state map array. It maps each digit in the goal state to row
    ''' and column.
    ''' Used for heuristic value calculation.
    ''' </summary>
    Private Shared goalstatemap(8, 1) As Byte

    ''' <summary>
    ''' The file name for Breadth First Search output.
    ''' The output is the path of the initial state to goal state (in reverse,
    ''' goal state first).
    ''' </summary>
    Public Const BreadthFirstFile As String = "breadth_first.txt"

    ''' <summary>
    ''' The file name for Depth First Search output.
    ''' The output is the path of the initial state to goal state (in reverse,
    ''' goal state first).
    ''' </summary>
    Public Const DepthFirstFile As String = "depth_first.txt"

    ''' <summary>
    ''' The file name for Best First Search output.
    ''' The output is the path of the initial state to goal state (in reverse,
    ''' goal state first).
    ''' </summary>
    Public Const BestFirstFile As String = "best_first.txt"

    ''' <summary>
    ''' The message to be displayed if the goal state not found.
    ''' </summary>
    Public Const GoalStateNotFound As String = "Goal state does not exist"

    Public Enum Direction
        Up
        Down
        Left
        Right
    End Enum

    ''' <summary>
    ''' Checks whether the given state is the goal state.
    ''' </summary>
    ''' <returns>True if the state is goal state, False otherwise.</returns>
    Public Shared Function IsGoalState(ByVal state As Integer) As Boolean
        Return state = goalstate
    End Function

    ''' <summary>
    ''' Gets the index of digit 0 from a given state.
    ''' </summary>
    ''' <returns>The index of digit 0.</returns>
    Public Shared Function GetBlankIndex(ByVal state As Integer) As Byte
        Return GetIndexOfDigit(state, 0)
    End Function

    ''' <summary>
    ''' Gets the index of a specified digit from a given state.
    ''' </summary>
    ''' <returns>The index of specified digit.</returns>
    Public Shared Function GetIndexOfDigit(ByVal state As Integer, ByVal digit As Integer) As Byte
        Dim index As Byte = 0
        While (state Mod 10) <> digit
            state \= 10
            index = CByte(index + 1)
            If index = 8 Then
                Exit While
            End If
        End While

        Return index
    End Function

    ''' <summary>
    ''' Gets a digit from a specified index from a given state.
    ''' </summary>
    ''' <returns>The digit from a specified index.</returns>
    Public Shared Function GetDigitAtIndex(ByVal state As Integer, ByVal index As Integer) As Integer
        Dim x As Double = Math.Pow(10, index)
        Dim q As Integer = CInt(state \ CLng(x))
        If q = 0 Then
            Return 0

        Else
            Return q Mod 10
        End If
    End Function

    ''' <summary>
    ''' Sets a digit at the specified index.
    ''' </summary>
    ''' <returns>A new state with the digit sets at the specified index.</returns>
    Public Shared Function SetDigitAtIndex(ByVal state As Integer, ByVal index As Integer, _
                                           ByVal digit As Integer) As Integer
        Dim x As Double = Math.Pow(10, index)
        Dim q As Integer = CInt(state \ CLng(x))
        Dim r As Integer = state Mod CType(x, Integer)
        Dim a As Integer = q \ 10
        Dim b As Integer = a * 10
        Dim val As Integer = ((b + digit) * CType(x, Integer)) + r

        Return val
    End Function

    ''' <summary>
    ''' Swaps 2 digits and returns a new state.
    ''' </summary>
    ''' <returns>A new state with 2 digits being exchanged.</returns>
    Public Shared Function SwapDigit(ByVal state As Integer, ByVal index1 As Integer, _
                                     ByVal index2 As Integer) As Integer
        Dim digit1 As Integer = GetDigitAtIndex(state, index1)
        Dim digit2 As Integer = GetDigitAtIndex(state, index2)
        Dim tmpstate As Integer = SetDigitAtIndex(state, index1, digit2)

        Return SetDigitAtIndex(tmpstate, index2, digit1)
    End Function

    ''' <summary>
    ''' Moves the blank with the specified direction.
    ''' </summary>
    ''' <returns>A new state with a new blank position.</returns>
    Public Shared Function MoveBlank(ByVal state As Integer, ByVal blankindex As Integer, _
                                     ByVal dir As Direction) As Integer
        Select Case dir
            Case Direction.Up
                Return SwapDigit(state, blankindex, blankindex - 3)

            Case Direction.Down
                Return SwapDigit(state, blankindex, blankindex + 3)

            Case Direction.Left
                Return SwapDigit(state, blankindex, blankindex - 1)

            Case Else
                Return SwapDigit(state, blankindex, blankindex + 1)
        End Select
    End Function

    ''' <summary>
    ''' Resolves the possible next states from a given state
    ''' which determines by the current blank index.
    ''' </summary>
    ''' <returns>An array of possible next states.</returns>
    Public Shared Function ResolveNextStates(ByVal state As Integer) As Integer()
        Dim blankindex As Byte = GetBlankIndex(state)
        Dim states() As Integer

        Select Case blankindex
            Case 1
                ReDim states(2)
                states(0) = MoveBlank(state, blankindex, Direction.Left)
                states(1) = MoveBlank(state, blankindex, Direction.Right)
                states(2) = MoveBlank(state, blankindex, Direction.Down)
                Return states

            Case 2
                ReDim states(1)
                states(0) = MoveBlank(state, blankindex, Direction.Left)
                states(1) = MoveBlank(state, blankindex, Direction.Down)
                Return states

            Case 3
                ReDim states(2)
                states(0) = MoveBlank(state, blankindex, Direction.Up)
                states(1) = MoveBlank(state, blankindex, Direction.Down)
                states(2) = MoveBlank(state, blankindex, Direction.Right)
                Return states

            Case 4
                ReDim states(3)
                states(0) = MoveBlank(state, blankindex, Direction.Up)
                states(1) = MoveBlank(state, blankindex, Direction.Left)
                states(2) = MoveBlank(state, blankindex, Direction.Right)
                states(3) = MoveBlank(state, blankindex, Direction.Down)
                Return states

            Case 5
                ReDim states(2)
                states(0) = MoveBlank(state, blankindex, Direction.Up)
                states(1) = MoveBlank(state, blankindex, Direction.Down)
                states(2) = MoveBlank(state, blankindex, Direction.Left)
                Return states

            Case 6
                ReDim states(1)
                states(0) = MoveBlank(state, blankindex, Direction.Up)
                states(1) = MoveBlank(state, blankindex, Direction.Right)
                Return states

            Case 7
                ReDim states(2)
                states(0) = MoveBlank(state, blankindex, Direction.Up)
                states(1) = MoveBlank(state, blankindex, Direction.Left)
                states(2) = MoveBlank(state, blankindex, Direction.Right)
                Return states

            Case 8
                ReDim states(1)
                states(0) = MoveBlank(state, blankindex, Direction.Up)
                states(1) = MoveBlank(state, blankindex, Direction.Left)
                Return states

            Case Else
                ReDim states(1)
                states(0) = MoveBlank(state, blankindex, Direction.Right)
                states(1) = MoveBlank(state, blankindex, Direction.Down)
                Return states
        End Select
    End Function

    ''' <summary>
    ''' Gets the heuristic value from a given state.
    ''' It calculates the row offset and column offset from the goal state.
    ''' The distance is row offset + column offset.
    ''' </summary>
    ''' <returns>The heuristic value from a given state.</returns>
    Public Shared Function GetHeuristicValue(ByVal state As Integer) As Integer
        Dim hval As Integer = 0

        For i As Integer = 0 To 8
            Dim digit As Integer = GetDigitAtIndex(state, i)
            If digit = 0 Then
                Continue For
            End If

            Dim row1 As Integer = i \ 3
            Dim column1 As Integer = i Mod 3
            Dim row2 As Integer = goalstatemap(digit, 0)
            Dim column2 As Integer = goalstatemap(digit, 1)

            Dim rowoffset As Integer = Math.Abs(row1 - row2)
            Dim columnoffset As Integer = Math.Abs(column1 - column2)
            hval += rowoffset + columnoffset
        Next

        Return hval
    End Function

    ''' <summary>
    ''' Gets a string representation from a given state.
    ''' e.g A state with 250816743 will be represented in
    '''
    ''' 347
    ''' 618
    '''  52
    ''' </summary>
    ''' <returns>The string representation from a given state.</returns>
    Public Shared Function GetStateString(ByVal state As Integer) As String
        Dim sb As New StringBuilder()
        Dim c(8) As Char
        Dim len As Byte = CType(c.GetUpperBound(0), Byte)

        For i As Byte = 0 To len
            Dim x As Integer = GetDigitAtIndex(state, i)
            Dim tmp As Char

            If x = 0 Then
                tmp = Chr(32) ' space

            Else
                tmp = Char.Parse(x.ToString())
            End If

            c(i) = tmp
        Next

        sb.AppendFormat("{0}{1}{2}" & Environment.NewLine, c(0), c(1), c(2))
        sb.AppendFormat("{0}{1}{2}" & Environment.NewLine, c(3), c(4), c(5))
        sb.AppendFormat("{0}{1}{2}" & Environment.NewLine, c(6), c(7), c(8))

        Return sb.ToString
    End Function

    ''' <summary>
    ''' Displays the 8 puzzle in the console.
    ''' </summary>
    Public Shared Sub ShowPuzzle()
        Console.WriteLine("Puzzle initial state =")
        Console.WriteLine(GetStateString(initstate))
        Console.WriteLine("Puzzle goal state =")
        Console.WriteLine(GetStateString(goalstate))
    End Sub

    ''' <summary>
    ''' Displays the solution not found message.
    ''' </summary>
    Public Shared Sub ShowNoSolutionMessage()
        Console.WriteLine("There is no solution exist." & vbCrLf)
    End Sub

    ''' <summary>
    ''' Initializes the 8 puzzle with initial state, goal state, and
    ''' depth limit from config.xml file.
    ''' </summary>
    Public Shared Sub SetPuzzle()
        Try
            Dim dir As String = Directory.GetCurrentDirectory
            Dim filename As String = Path.Combine(dir, "config.xml")
            If Not File.Exists(filename) Then
                Exit Sub
            End If

            Dim doc As XmlDocument = New XmlDocument
            doc.Load(filename)
            Dim initnode As XmlNode = doc.SelectSingleNode("/config/init_state")
            Dim goalnode As XmlNode = doc.SelectSingleNode("/config/goal_state")
            Dim depthnode As XmlNode = doc.SelectSingleNode("/config/depth_limit")

            Dim init As String = ReverseString(initnode.InnerText)
            Dim goal As String = ReverseString(goalnode.InnerText)
            Dim depth As String = depthnode.InnerText

            Dim _initstate As Integer = Integer.Parse(init)
            Dim _goalstate As Integer = Integer.Parse(goal)
            Dim _depthlimit As Integer = Integer.Parse(depth)

            initstate = _initstate
            goalstate = _goalstate
            depthlimit = _depthlimit

        Catch ex As Exception

        End Try

        SetGoalStateMap()
    End Sub

    ''' <summary>
    ''' Initializes the goalstatemap array.
    ''' It keeps the row and column for each digit in the goal state.
    ''' </summary>
    Private Shared Sub SetGoalStateMap()
        For i As Byte = 0 To 8
            Dim digit As Integer = GetDigitAtIndex(goalstate, i)
            goalstatemap(digit, 0) = CType(i \ 3, Byte)
            goalstatemap(digit, 1) = CType(i Mod 3, Byte)
        Next
    End Sub

    ''' <summary>
    ''' Reverses a string.
    ''' </summary>
    ''' <returns>A new string with the original string being reversed.</returns>
    Private Shared Function ReverseString(ByVal s As String) As String
        Dim c() As Char = s.ToCharArray
        Array.Reverse(c)
        Return New String(c)
    End Function
End Class
