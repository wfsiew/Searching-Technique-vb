Imports System.IO

''' <summary>
''' Breadth First Search class.
''' Uses Queue as open list, and Dictionary as close list.
''' </summary>
Public Class BreadthFirst
    Inherits BaseSolver

    Private openlist As Queue(Of State)
    Private closelist As Dictionary(Of Integer, State)

    Public Sub New(ByVal state As Integer)
        MyBase.New(state)
        openlist = New Queue(Of State)
        closelist = New Dictionary(Of Integer, State)
    End Sub

    Public Overrides Sub Solve()
        Dim activestate As New State(initstate)
        Dim solved As Boolean = False

        watch.Reset()
        watch.Start()

        openlist.Enqueue(activestate)

        While openlist.Count > 0
            activestate = openlist.Dequeue()

            If Utils.IsGoalState(activestate.StateValue) Then
                solved = True
                closelist.Add(activestate.StateValue, activestate)
                Exit While

            Else
                If Not closelist.ContainsKey(activestate.StateValue) Then
                    closelist.Add(activestate.StateValue, activestate)
                End If

                Dim nextstates() As Integer = Utils.ResolveNextStates(activestate.StateValue)

                For i As Integer = 0 To nextstates.GetUpperBound(0)
                    Dim state As New State(nextstates(i))
                    state.Parent = activestate

                    If Not closelist.ContainsKey(nextstates(i)) AndAlso Not openlist.Contains(state) Then
                        If Utils.IsGoalState(nextstates(i)) Then
                            solved = True
                            closelist.Add(nextstates(i), state)
                            Exit For

                        Else
                            openlist.Enqueue(state)
                        End If
                    End If
                Next

                If solved Then
                    Exit While
                End If
            End If
        End While

        watch.Stop()
        WriteSolution(solved)
    End Sub

    Protected Overrides Sub WriteSolution(solved As Boolean)
        Console.WriteLine(vbCr & "Time elapsed for Breadth First Search in milliseconds = {0}", watch.ElapsedMilliseconds)
        Console.WriteLine("States generated in Breadth First Search = {0}", closelist.Count + openlist.Count)
        Console.WriteLine("States generated in open list = {0}", openlist.Count)
        Console.WriteLine("States generated in close list = {0}", closelist.Count)

        Dim dir As String = Directory.GetCurrentDirectory()
        Dim filename As String = Path.Combine(dir, Utils.BreadthFirstFile)

        If Not solved Then
            Utils.ShowNoSolutionMessage()

            Try
                File.Delete(filename)

            Catch ex As Exception

            End Try

        Else
            Dim sw As StreamWriter = Nothing

            Try
                Dim o As State = closelist(Utils.goalstate)
                Dim ls As New List(Of String)
                Dim s As String = Nothing

                Console.WriteLine("Goal state found at level {0}", o.Level)

                While Not IsNothing(o)
                    s = Utils.GetStateString(o.StateValue)
                    ls.Add(s)
                    o = o.Parent
                End While

                sw = New StreamWriter(filename, False)

                For i As Integer = ls.Count - 1 To 0 Step -1
                    sw.WriteLine(ls(i))
                Next

                Console.WriteLine("Solution count (excluding initial state) = {0}", ls.Count - 1)
                Console.WriteLine("Please refer to {0} file for the solutions." & vbCrLf, filename)

            Catch e As Exception
                Console.WriteLine("Error occurred in generating the solution file." & vbCrLf & "{0}", e.ToString())

            Finally
                If Not IsNothing(sw) Then
                    sw.Close()
                End If
            End Try
        End If
    End Sub
End Class
