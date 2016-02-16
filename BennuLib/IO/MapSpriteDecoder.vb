Option Infer On

Namespace Bennu.IO
    Public Class MapSpriteDecoder
        Inherits NativeDecoder(Of Sprite)

        Public Overrides ReadOnly Property MaxSupportedVersion As Integer = &H0

        Protected Overrides ReadOnly Property KnownFileExtensions As String() = {"fnt"}

        Protected Overrides ReadOnly Property KnownFileIds As String() = {"map", "m16", "m32"}

        Protected Overrides Function ReadNativeFormat(magic As Magic, reader As NativeFormatReader) As Sprite
            Dim width As Integer = reader.ReadUInt16()
            Dim height As Integer = reader.ReadUInt16()
            Dim code As Integer = reader.ReadInt32()

            Dim description = reader.ReadDescription()

            Dim depth = magic.Depth
            If depth = 8 Then
                Dim pal = Palette.Create(VGAtoColors(reader.ReadPalette))
                reader.ReadUnusedPaletteGamma()
            End If

            Dim numberOfPivotPoints = reader.ReadPivotPointsNumber()
            Dim pivotPoints() = reader.ReadPivotPoints(numberOfPivotPoints)

            Dim mapDataLength = width * height * (depth \ 8)
            Dim graphicData = reader.ReadBytes(mapDataLength)
            Dim pixels = CreatePixelBuffer(magic.Depth, graphicData)

            Dim map = Sprite.Create(width, height, pixels)

            For Each pivotPoint In pivotPoints
                map.DefinePivotPoint(pivotPoint.Id, pivotPoint.X, pivotPoint.Y)
            Next

            Return map
        End Function
    End Class
End Namespace