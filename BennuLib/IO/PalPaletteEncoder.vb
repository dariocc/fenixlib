Namespace Bennu.IO
    Public Class PalPaletteEncoder
        Inherits NativeEncoder(Of Palette)

        Protected Overrides ReadOnly Property Version As Byte = &H0

        Protected Overrides Sub WriteNativeFormat(palette As Palette, writer As NativeFormatWriter)
            writer.Write(palette)
            writer.WriteReservedPaletteGammaSection()
        End Sub

        Protected Overrides Function GetFileId(obj As Palette) As String
            Return "pal"
        End Function
    End Class
End Namespace
