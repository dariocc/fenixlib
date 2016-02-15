Option Infer On

Imports PkSprite.Core

Namespace Bennu.IO
    Public Class DivFntFpgDecoder
        Inherits NativeDecoder(Of SpriteAsset)

        Public Overrides ReadOnly Property MaxSupportedVersion As Integer = &H0

        Protected Overrides ReadOnly Property KnownFileExtensions As String() = {"fnt"}

        Protected Overrides ReadOnly Property KnownFileIds As String() = {"fnt"}

        Protected Overrides Function ReadNativeFormat(magic As Magic, reader As NativeFormatReader) As SpriteAsset

            Dim pal = Palette.Create(VGAtoColors(reader.ReadPalette))
            reader.ReadUnusedPaletteGamma()

            Dim fontInfo As Integer = reader.ReadInt32

            Dim characters(255) As GlyphInfo
            For n = 0 To 255
                characters(n) = reader.ReadGlyphInfo()
            Next

            Dim fpg As New SpriteAsset
            For Each character In characters
                Dim dataLength = character.Height * character.Width

                If character.FileOffset = 0 Or dataLength = 0 Then Continue For

                Dim graphicData = reader.ReadBytes(dataLength)

                Dim pixels = IndexedPixel.CreateBufferFromBytes(graphicData)
                Dim map = Sprite.Create(character.Width, character.Height, pixels)
                fpg.Add(fpg.FindFreeCode, map)
            Next

            Return fpg
        End Function
    End Class
End Namespace