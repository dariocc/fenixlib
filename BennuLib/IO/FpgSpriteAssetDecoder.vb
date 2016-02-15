Option Infer On

Imports PkSprite.Core

Namespace Bennu.IO
    Public Class FpgSpriteAssetDecoder
        Inherits NativeDecoder(Of SpriteAsset)

        Public Overrides ReadOnly Property MaxSupportedVersion As Integer = &H0 ' Current readable Fpg version

        Protected Overrides ReadOnly Property KnownFileExtensions As String() = {"fpg"}

        Protected Overrides ReadOnly Property KnownFileIds As String() = {"fpg", "f16", "f32"}

        Protected Overrides Function ReadNativeFormat(magic As Magic, reader As NativeFormatReader) As SpriteAsset
            Dim depth = magic.Depth
            If depth = 8 Then
                Dim pal = Palette.Create(VGAtoColors(reader.ReadPalette))
                reader.ReadUnusedPaletteGamma()
            End If

            Dim fpg As New SpriteAsset

            Try
                Do
                    Dim code = reader.ReadInt32
                    Dim maplen = reader.ReadInt32
                    Dim description = reader.ReadDescription
                    Dim name = reader.ReadChars(12).ToString()
                    Dim width = reader.ReadInt32
                    Dim height = reader.ReadInt32

                    Dim mapDataLength = width * height * (depth \ 8)

                    Dim numberOfPivotPoints = reader.ReadPivotPointsNumber()
                    Dim pivotPoints() = reader.ReadPivotPoints(numberOfPivotPoints)

                    ' Serves as checksum for FPGs created with non-standard tools such
                    ' as FPG Edit
                    If mapDataLength + 64 + numberOfPivotPoints * 4 <> maplen Then
                        Exit Do
                    End If


                    Dim graphicData = reader.ReadBytes(mapDataLength)
                    Dim pixels = CreatePixelBuffer(magic.Depth, graphicData)

                    Dim map = Sprite.Create(width, height, pixels)
                    map.Description = description

                    fpg.Update(code, map)
                Loop
            Catch exception As System.IO.EndOfStreamException
                ' Do nothing (the reading of the map
            End Try

            Return fpg
        End Function
    End Class
End Namespace
