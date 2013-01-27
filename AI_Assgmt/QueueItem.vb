''' <summary>
''' Generic class of queue item, consist of key (int) and a generic object (T).
''' Used in BasePriorityQueue class.
''' </summary>
Public Class QueueItem(Of T)
    Private obj As T
    Private key As Integer

    Public Sub New(ByVal _key As Integer, ByVal _obj As T)
        obj = _obj
        key = _key
    End Sub

    Public Function GetObj() As T
        Return obj
    End Function

    Public Function GetKey() As Integer
        Return key
    End Function
End Class
