Option Infer On

Namespace Bennu.IO
    Public Class FpgSpriteAssetEncoder
        Inherits NativeEncoder(Of SpriteAsset)

        Protected Overrides ReadOnly Property Version As Byte = &H0

        Protected Overrides Sub WriteNativeFormat(asset As SpriteAsset, writer As NativeFormatWriter)
            If HasPalette(asset) Then
                ' TODO: Write the palette
                ' TODO: Write the unused bytes
            End If

            For Each sprite In asset
                ' TODO: Will fail for no control points defined
                Dim maxPivotPointId = Convert.ToUInt16(sprite.PivotPoints.Max(Function(p) p.Id))
                Dim maplen = Convert.ToUInt32(64 + Convert.ToUInt32(sprite.Width) * Convert.ToUInt32(sprite.Height) * asset.Depth \ 8 + maxPivotPointId * 4)

                writer.Write(maplen)
                writer.WriteAsciiZ(sprite.Description, 32)
                writer.WriteAsciiZ("SpritePocket", 12)
                writer.Write(Convert.ToUInt32(sprite.Width))
                writer.Write(Convert.ToUInt32(sprite.Height))
                writer.Write(Convert.ToUInt32(maxPivotPointId))
                writer.Write(sprite.PivotPoints)
                ' TODO: Fix
                writer.Write(DirectCast(sprite.Pixels, Int32PixelARGB()))
            Next
        End Sub

        Private Function HasPalette(asset As SpriteAsset) As Boolean
            Return False ' TODO
        End Function

        Protected Overrides Function GetFileId(asset As SpriteAsset) As String
            ' TODO
            Select Case asset.Depth
                Case 8
                    Return "fpg"
                Case 16
                    Return "f16"
                Case 32
                    Return "f32"
                Case Else
                    Throw New ArgumentException() ' TODO more specific
            End Select
        End Function
    End Class
End Namespace
