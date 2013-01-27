Imports System.IO

''' <summary>
''' Best First Search class.
''' Uses PriorityQueue as open list, and Dictionary as close list.
''' </summary>
Public Class BestFirst
    Inherits BaseSolver

    Private openlist As PriorityQueue
    Private closelist As Dictionary(Of Integer, State)

    Public Sub New(ByVal state As Integer)
        MyBase.New(state)
        openlist = New PriorityQueue(False)
        closelist = New Dictionary(Of Integer, State)
    End Sub

    Public Overrides Sub Solve()
        Dim activestate As New State(initstate, Utils.GetHeuristicValue(initstate))
        Dim solved As Boolean = False

        watch.Reset()
        watch.Start()

        openlist.Enqueue(activestate.EValue, activestate)

        While openlist.Count > 0
            activestate = openlist.Dequeue()

            If Utils.IsGoalState(activestate.StateValue) Then
                solved = True
                closelist.Add(activestate.StateValue, activestate)
                Exit While

            Else
                Dim nextstates() As Integer = Utils.ResolveNextStates(activestate.StateValue)

                For i As Integer = 0 To nextstates.GetUpperBound(0)
                    Dim state As New State(nextstates(i), Utils.GetHeuristicValue(nextstates(i)))
                    state.Parent = activestate

                    Dim is_exist_on_openlist As Boolean = openlist.IsRepetitionExist(state.EValue, state)
                    Dim is_exist_on_closelist As Boolean = closelist.ContainsKey(state.StateValue)

                    If Not is_exist_on_closelist AndAlso Not is_exist_on_openlist Then
                        openlist.Enqueue(state.EValue, state)
                    End If

                    If is_exist_on_closelist Then
                        Dim o As State = closelist(state.StateValue)

                        If o.Level > state.Level Then
                            closelist.Remove(state.StateValue)
                            openlist.Enqueue(state.EValue, state)
                        End If
                    End If
                Next

                If Not closelist.ContainsKey(activestate.StateValue) Then
                    closelist.Add(activestate.StateValue, activestate)
                End If
            End If
        End While

        watch.Stop()
        WriteSolution(solved)
    End Sub

    Protected Overrides Sub WriteSolution(solved As Boolean)
        Console.WriteLine(vbCr & "Time elapsed for Best First Search in milliseconds = {0}", watch.ElapsedMilliseconds)
        Console.WriteLine("States generated in Best First Search = {0}", closelist.Count + openlist.Count)
        Console.WriteLine("States generated in open list = {0}", openlist.Count)
        Console.WriteLine("States generated in close list = {0}", closelist.Count)

        Dim dir As String = Directory.GetCurrentDirectory()
        Dim filename As String = Path.Combine(dir, Utils.BestFirstFile)

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
