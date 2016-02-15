Option Infer On

Imports PkSprite.Core

Namespace Bennu.IO
    Public Class DivFormatPaletteDecoder
        Inherits NativeDecoder(Of Palette)

        Public Overrides ReadOnly Property MaxSupportedVersion As Integer = &H0

        Protected Overrides ReadOnly Property KnownFileExtensions As String() = {"pal", "fnt", "map", "fpg"}

        Protected Overrides ReadOnly Property KnownFileIds As String() = {"pal", "fnt", "map", "fpg"}

        Protected Overrides Function ReadNativeFormat(magic As Magic, reader As NativeFormatReader) As Palette
            ' Map files have the Palette data in a different position than the rest of the files
            If magic.FileType = "map" Then reader.ReadBytes(40)

            Return Palette.Create(VGAtoColors(reader.ReadPalette))
        End Function
    End Class
End Namespace