Imports System.Threading
Imports System.Threading.Tasks

Module Module2

    Sub Main()
        Console.WriteLine("Please note that you may set the initial state, goal state,")
        Console.WriteLine("and depth limit in the config.xml file." & vbCrLf)

        Utils.SetPuzzle()
        Utils.ShowPuzzle()

        Dim solvers() As BaseSolver = {GetDepthFirst(Utils.initstate),
                                       GetBreathFirst(Utils.initstate),
                                       GetBestFirst(Utils.initstate)}

        Parallel.ForEach(solvers,
                         Sub(o As BaseSolver)
                             o.Solve()
                         End Sub)

        Console.WriteLine(vbCrLf & "Press any key to continue . . .")
        Console.ReadKey()
    End Sub

    Function GetDepthFirst(ByVal initstate As Integer) As DepthFirst
        Dim o As New DepthFirst(initstate)
        o.Depth = Utils.depthlimit
        Return o
    End Function

    Function GetBreathFirst(ByVal initstate As Integer) As BreadthFirst
        Dim o As New BreadthFirst(initstate)
        Return o
    End Function

    Function GetBestFirst(ByVal initstate As Integer) As BestFirst
        Dim o As New BestFirst(initstate)
        Return o
    End Function
End Module
