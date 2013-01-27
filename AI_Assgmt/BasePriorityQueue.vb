''' <summary>
''' Base class of priority queue, where an element in the queue is QueueItem object.
''' </summary>
Public Class BasePriorityQueue(Of T As Class)
    ''' <summary>
    ''' A list of QueueItem.
    ''' </summary>
    Protected items As LinkedList(Of QueueItem(Of T))
    ''' <summary>
    ''' A flag to determine whether the queue allows repetition of the same item.
    ''' </summary>
    Private m_allowrepeat As Boolean

    Public Sub New()
        Me.New(True)
    End Sub

    Public Sub New(ByVal _allowrepeat As Boolean)
        items = New LinkedList(Of QueueItem(Of T))
        m_allowrepeat = _allowrepeat
    End Sub

    ''' <summary>
    ''' Adds an object to the queue, and place the object in the queue
    ''' based on the key.
    ''' Object with key will be at the left end in the queue.
    ''' </summary>
    Public Sub Enqueue(ByVal key As Integer, ByVal obj As T)
        If Not m_allowrepeat Then
            If IsRepetitionExist(key, obj) Then
                Exit Sub
            End If
        End If

        Dim node As New LinkedListNode(Of QueueItem(Of T))(New QueueItem(Of T)(key, obj))

        If items.Count = 0 Then
            items.AddFirst(node)

        Else
            Dim current As LinkedListNode(Of QueueItem(Of T)) = items.First

            While Not IsNothing(current)
                If node.Value.GetKey <= current.Value.GetKey Then
                    items.AddBefore(current, node)
                    Exit While
                End If

                current = current.Next
            End While

            If IsNothing(current) Then
                items.AddLast(node)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Removes the left most object from the queue.
    ''' </summary>
    ''' <returns>The left most object in the queue.</returns>
    Public Function Dequeue() As T
        If items.Count = 0 Then
            Return Nothing

        Else
            Dim obj As T = items.First.Value.GetObj
            items.RemoveFirst()
            Return obj
        End If
    End Function

    ''' <summary>
    ''' Gets the number of objects actually contained in the queue.
    ''' </summary>
    Public ReadOnly Property Count As Integer
        Get
            Return items.Count
        End Get
    End Property

    ''' <summary>
    ''' Checks whether there is repetition of the key in the queue.
    ''' </summary>
    ''' <returns>True if there is repetition of the key, False otherwise.</returns>
    Public Overridable Function IsRepetitionExist(ByVal key As Integer, ByVal obj As T) As Boolean
        If items.Count = 0 Then
            Return False
        End If

        Dim current As LinkedListNode(Of QueueItem(Of T)) = items.First
        Dim repetition As Integer = 0

        While Not IsNothing(current) OrElse (Not IsNothing(current) AndAlso current.Value.GetKey > key)
            If current.Value.GetKey = key Then
                repetition += 1
            End If

            If repetition > 1 Then
                Exit While
            End If

            current = current.Next
        End While

        If repetition < 2 Then
            Return False

        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Gets/Sets the allow repeat mode of the queue.
    ''' </summary>
    Public Property AllowRepeat As Boolean
        Get
            Return m_allowrepeat
        End Get
        Set(value As Boolean)
            m_allowrepeat = value
        End Set
    End Property

    ''' <summary>
    ''' Replaces an existing object in the queue with a new object.
    ''' </summary>
    Public Sub Replace(ByVal oldobj As LinkedListNode(Of QueueItem(Of T)), ByVal newobj As QueueItem(Of T))
        items.AddAfter(oldobj, newobj)
        items.Remove(oldobj)
    End Sub

    ''' <summary>
    ''' Gets the actual queue.
    ''' </summary>
    Public ReadOnly Property List As LinkedList(Of QueueItem(Of T))
        Get
            Return items
        End Get
    End Property
End Class
