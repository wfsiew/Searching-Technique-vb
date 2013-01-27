Module Module1

    Sub Main()
        Console.WriteLine("Please note that you may set the initial state, goal state,")
        Console.WriteLine("and depth limit in the config.xml file." & vbCrLf)

        Utils.SetPuzzle()
        Utils.ShowPuzzle()

        StartDepthFirst(Utils.initstate)
        StartBreadthFirst(Utils.initstate)
        StartBestFirst(Utils.initstate)

        Console.WriteLine(vbCrLf & "Press any key to continue . . .")
        Console.ReadKey()
    End Sub

    Sub StartDepthFirst(ByVal initstate As Integer)
        Try
            Dim o As New DepthFirst(initstate)
            o.Depth = Utils.depthlimit
            Console.Write(vbCr & "Depth First Search starts . . . Please wait")
            o.Solve()

        Catch e As Exception
            HandleException(e)
        End Try
    End Sub

    Sub StartBreadthFirst(ByVal initstate As Integer)
        Try
            Dim o As New BreadthFirst(initstate)
            Console.Write(vbCr & "Breadth First Search starts . . . Please wait")
            o.Solve()

        Catch e As Exception
            HandleException(e)
        End Try
    End Sub

    Sub StartBestFirst(ByVal initstate As Integer)
        Try
            Dim o As New BestFirst(initstate)
            Console.Write(vbCr & "Best First Search starts . . . Please wait")
            o.Solve()

        Catch e As Exception
            HandleException(e)
        End Try
    End Sub

    Sub HandleException(ByVal e As Exception)
        Console.WriteLine()
        Console.WriteLine(e.ToString())
    End Sub

End Module
