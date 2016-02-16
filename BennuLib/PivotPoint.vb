<Serializable>
Public Structure PivotPoint
    Public ReadOnly Id As Integer
    Public ReadOnly X As Integer
    Public ReadOnly Y As Integer

    Public Sub New(id As Integer, x As Integer, y As Integer)
        Me.Id = id
        Me.X = x
        Me.Y = y
    End Sub
End Structure
