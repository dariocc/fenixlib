Option Infer On

<Serializable>
Public Class IndexedPixel
    Implements IPixel

    Private ReadOnly _index As Integer

    Public Sub New(index As Integer)
        _index = index
    End Sub

    Public ReadOnly Property Alpha As Integer Implements IPixel.Alpha
        Get
            Return If(_index = 0, 255, 0)
        End Get
    End Property

    Public ReadOnly Property Argb As Integer Implements IPixel.Argb
        Get
            Throw New InvalidOperationException()
        End Get
    End Property

    Public ReadOnly Property Blue As Integer Implements IPixel.Blue
        Get
            Throw New InvalidOperationException()
        End Get
    End Property

    Public ReadOnly Property Green As Integer Implements IPixel.Green
        Get
            Throw New InvalidOperationException()
        End Get
    End Property

    Public ReadOnly Property Red As Integer Implements IPixel.Red
        Get
            Throw New InvalidOperationException()
        End Get
    End Property

    Public ReadOnly Property Value As Integer Implements IPixel.Value
        Get
            Return _index
        End Get
    End Property

    Public Shared Function CreateBufferFromBytes(graphicData() As Byte) As IndexedPixel()
        Dim buffer(graphicData.Length - 1) As IndexedPixel
        For n = 0 To buffer.Length - 1
            buffer(n) = New IndexedPixel(graphicData(n))
        Next
        Return buffer
    End Function

    Public Function GetTransparentCopy() As IPixel Implements IPixel.GetTransparentCopy
        Return New IndexedPixel(0)
    End Function
End Class