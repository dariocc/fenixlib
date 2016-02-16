Option Infer On
Imports BennuLib

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
            If _index = 0 Then
                Return 0
            Else
                Return 255
            End If
        End Get
    End Property

    Public ReadOnly Property Blue As Integer Implements IPixel.Blue
        Get
            ' TODO: Might need to keep a reference to a color table
            Throw New InvalidOperationException()
        End Get
    End Property

    Public ReadOnly Property Green As Integer Implements IPixel.Green
        Get
            ' TODO: Might need to keep a reference to a color table
            Throw New InvalidOperationException()
        End Get
    End Property

    Public ReadOnly Property IsTransparent As Boolean Implements IPixel.IsTransparent
        Get
            Return _index = 0
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

    Public Function GetOpaqueCopy() As IPixel Implements IPixel.GetOpaqueCopy
        ' TODO: Should find the closest color in the color space that is not in index 0
        Throw New InvalidOperationException()
    End Function

    Public Function GetTransparentCopy() As IPixel Implements IPixel.GetTransparentCopy
        Return New IndexedPixel(0)
    End Function

    ' TODO: Might not belong here
    Public Shared Function CreateBufferFromBytes(graphicData() As Byte) As IndexedPixel()
        Dim buffer(graphicData.Length - 1) As IndexedPixel
        For n = 0 To buffer.Length - 1
            buffer(n) = New IndexedPixel(graphicData(n))
        Next
        Return buffer
    End Function
End Class