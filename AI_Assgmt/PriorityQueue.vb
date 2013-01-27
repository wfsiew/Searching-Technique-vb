''' <summary>
''' PriorityQueue class.
''' </summary>
Public Class PriorityQueue
    Inherits BasePriorityQueue(Of State)

    Public Sub New(ByVal _allowrepeat As Boolean)
        MyBase.New(_allowrepeat)
    End Sub

    ''' <summary>
    ''' Overrides the method from BasePriority class.
    ''' It checks whether the same state exist in the queue.
    ''' If yes, it compares the state's level found from the queue with the specified state's level.
    ''' If the state from the queue is deeper, it will be replaced with the specified state.
    ''' </summary>
    ''' <returns>True if there is repetition of the state, False otherwise.</returns>
    Public Overrides Function IsRepetitionExist(key As Integer, obj As State) As Boolean
        Dim exist As Boolean = False
        Dim current As LinkedListNode(Of QueueItem(Of State)) = items.First

        While Not IsNothing(current)
            If current.Value.GetObj().StateValue = obj.StateValue Then
                If current.Value.GetObj.Level > obj.Level Then
                    Replace(current, New QueueItem(Of State)(obj.EValue, obj))
                    exist = True
                    Exit While
                End If
            End If

            current = current.Next
        End While

        Return exist
    End Function
End Class
