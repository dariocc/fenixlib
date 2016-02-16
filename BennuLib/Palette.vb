<Serializable>
Public Class Palette
    Implements IEnumerable

    Public Structure Color
        Public ReadOnly r As Integer
        Public ReadOnly g As Integer
        Public ReadOnly b As Integer

        Public Sub New(r As Integer, g As Integer, b As Integer)
            Me.r = r
            Me.g = g
            Me.b = b
        End Sub
    End Structure

    Private _colors() As Color

    Public Shared Function Create(colors() As Color) As Palette
        Return New Palette(colors)
    End Function

    Private Sub New(colors() As Color)
        _colors = colors
    End Sub

    Default Public Property Item(index As Integer) As Color
        Get
            Return _colors(index)
        End Get
        Set(value As Color)
            _colors(index) = value
        End Set
    End Property

    Public ReadOnly Property Colors As Color()
        Get
            Return _colors
        End Get
    End Property

    Public Function GetCopy() As Palette
        Dim colors(_colors.Length - 1) As Color
        _colors.CopyTo(colors, 0)
        Return Create(colors)
    End Function

    Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return _colors.GetEnumerator
    End Function
End Class