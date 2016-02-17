Option Infer On
Imports BennuLib

<Serializable>
Public Class Int16Pixel565
    Implements IPixel

    ' TODO: It is certainly possible to speed up this colo conversions by 
    ' keeping the list of all 65535 colors possible in memmory (65Kb)...

    Private ReadOnly _value As UShort

    Public Sub New(r As Byte, g As Byte, b As Byte)
        Me.New(Convert.ToUInt16((r >> 3) << 11 Or (g >> 2) << 5 Or b >> 2))
    End Sub

    Friend Sub New(value As UShort)
        _value = value
    End Sub

    Public ReadOnly Property Alpha As Integer Implements IPixel.Alpha
        Get
            Return If(_value = 0, 255, 0)
        End Get
    End Property

    Public ReadOnly Property Argb As Integer Implements IPixel.Argb
        Get
            Return (Alpha << 24 Or Red << 16 Or Green << 8 Or Blue)
        End Get
    End Property

    Public ReadOnly Property Blue As Integer Implements IPixel.Blue
        Get
            Return _value And &H1F
        End Get
    End Property

    Public ReadOnly Property Green As Integer Implements IPixel.Green
        Get
            Return _value >> 5 And &H3F
        End Get
    End Property

    Public ReadOnly Property Red As Integer Implements IPixel.Red
        Get
            Return _value >> 11 And &H1F
        End Get
    End Property

    Public ReadOnly Property Value As Integer Implements IPixel.Value
        Get
            Return _value
        End Get
    End Property

    Public ReadOnly Property IsTransparent As Boolean Implements IPixel.IsTransparent
        Get
            Return Value = 0
        End Get
    End Property

    Public Function GetTransparentCopy() As IPixel Implements IPixel.GetTransparentCopy
        Return New Int16Pixel565(0)
    End Function

    Public Shared Function CreateBufferFromBytes(graphicData() As Byte) As Int16Pixel565()
        Dim buffer(graphicData.Length \ 3 - 1) As Int16Pixel565
        For n = 0 To buffer.Length - 1 Step 3
            buffer(n) = New Int16Pixel565(graphicData(n), graphicData(n + 1), graphicData(n + 2))
        Next
        Return buffer
    End Function

    Public Function GetOpaqueCopy() As IPixel Implements IPixel.GetOpaqueCopy
        If _value = 0 Then
            Return New Int16Pixel565(1)
        Else
            Return New Int16Pixel565(_value)
        End If
    End Function
End Class