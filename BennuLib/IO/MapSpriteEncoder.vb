Option Infer On

Namespace Bennu.IO
    Public Class MapSpriteEncoder
        Inherits NativeEncoder(Of Sprite)

        Protected Overrides ReadOnly Property Version As Byte = &H0

        Protected Overrides Sub WriteNativeFormat(sprite As Sprite, writer As NativeFormatWriter)
            writer.Write(Convert.ToUInt16(sprite.Width))
            writer.Write(Convert.ToUInt16(sprite.Height))
            writer.Write(Convert.ToUInt32(sprite.Id.GetValueOrDefault))

            If Not sprite.Palette Is Nothing Then
                writer.Write(sprite.Palette)
                writer.WriteReservedPaletteGammaSection()
            End If

            Dim ids = sprite.PivotPoints.Select(Function(p) p.Id)

            writer.Write(Convert.ToUInt16(If(ids.Count > 0, ids.Max, 0)))
            writer.Write(sprite.PivotPoints)
            'TODO
            writer.Write(DirectCast(sprite.Pixels, Int32PixelARGB()))
        End Sub

        Protected Overrides Function GetFileId(obj As Sprite) As String

            If TypeOf (obj.Pixels(0)) Is IndexedPixel Then
                Return "map"
            ElseIf TypeOf (obj.Pixels(0)) Is Int16Pixel565 Then
                Return "m16"
            ElseIf TypeOf (obj.Pixels(0)) Is Int32PixelARGB Then
                Return "m32"
            Else
                Throw New ArgumentException
            End If

        End Function

    End Class
End Namespace
