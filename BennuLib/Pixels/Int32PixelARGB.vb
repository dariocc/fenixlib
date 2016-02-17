Option Infer On
Imports BennuLib

<Serializable>
Public Class Int32PixelARGB
    Implements IPixel

    Private ReadOnly _value As Integer

    Public Sub New(alpha As Byte, r As Byte, g As Byte, b As Byte)
        Me.New(alpha << 24 Or r << 16 Or g << 8 Or b)
    End Sub

    Friend Sub New(value As Integer)
        _value = value
    End Sub

    Public ReadOnly Property Alpha As Integer Implements IPixel.Alpha
        Get
            Return _value >> 24
        End Get
    End Property

    Public ReadOnly Property Argb As Integer Implements IPixel.Argb
        Get
            Return _value
        End Get
    End Property

    Public ReadOnly Property Blue As Integer Implements IPixel.Blue
        Get
            Return _value And &HFF
        End Get
    End Property

    Public ReadOnly Property Green As Integer Implements IPixel.Green
        Get
            Return _value >> 8 And &HFF
        End Get
    End Property

    Public ReadOnly Property Red As Integer Implements IPixel.Red
        Get
            Return _value >> 24 And &HFF
        End Get
    End Property

    Public ReadOnly Property Value As Integer Implements IPixel.Value
        Get
            Return Value
        End Get
    End Property

    Public ReadOnly Property IsTransparent As Boolean Implements IPixel.IsTransparent
        Get
            Return Alpha = 255
        End Get
    End Property

    Public Function GetTransparentCopy() As IPixel Implements IPixel.GetTransparentCopy
        Return New Int32PixelARGB(_value)
    End Function

    Public Shared Function CreateBufferFromBytes(graphicData() As Byte) As Int32PixelARGB()
        Dim buffer(graphicData.Length \ 4 - 1) As Int32PixelARGB

        For n = 0 To buffer.Length - 1
            buffer(n) = New Int32PixelARGB(graphicData(n), graphicData(n + 1), graphicData(n + 2), graphicData(n + 4))
        Next
        Return buffer
    End Function

    Public Function GetOpaqueCopy() As IPixel Implements IPixel.GetOpaqueCopy
        Return New Int32PixelARGB(Value And &HFFFFFF)
    End Function
End Class