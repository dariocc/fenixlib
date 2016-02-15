Option Infer On
Imports System.IO
Imports PkSprite.Core

Namespace Bennu.IO
    Public Class NativeFormatWriter
        Inherits BinaryWriter

        Private Shared ReadOnly _encoding As Text.Encoding = Text.Encoding.GetEncoding(850)

        Public Sub New(input As Stream)
            MyBase.New(input, _encoding)
        End Sub

        Private Sub WriteHeader(formatHeader As String, version As Byte)
            If formatHeader.Length <> 3 Then
                Throw New ArgumentException ' TODO: Customize
            End If

            MyBase.Write(_encoding.GetBytes(formatHeader))
            MyBase.Write(NativeDescriptor)
            MyBase.Write(version)
        End Sub

        Public Sub WriteAsciiZ(text As String, maxLength As Integer)
            ' Texts are encoded in ASCIZZ format
            Dim clippedText = text.Substring(0, Math.Min(text.Length, maxLength))
            Dim bytes = _encoding.GetBytes(clippedText.ToCharArray)
            Array.Resize(bytes, maxLength)
            MyBase.Write(bytes)
        End Sub

        Public Overloads Sub Write(palette As Palette)
            Dim bytes(palette.Colors.Length * 3 - 1) As Byte
            For n = 0 To palette.Colors.Length
                bytes(n * 3) = Convert.ToByte(palette(n).r)
                bytes(n * 3 + 1) = Convert.ToByte(palette(n).g)
                bytes(n * 3 + 2) = Convert.ToByte(palette(n).b)
            Next

            MyBase.Write(bytes)
        End Sub

        Public Overloads Sub Write(pivotPoints As IEnumerable(Of PivotPoint))
            Dim ids = From p In pivotPoints Select p.Id

            If ids.Count = 0 Then Exit Sub

            Dim pivotPointsIncludingUndefined(ids.Max - 1) As PivotPoint
            For n = 0 To ids.Max
                Dim id = n
                Dim p? = pivotPoints.Where(Function(x) x.Id = id).FirstOrDefault
                pivotPointsIncludingUndefined(n) = If(p Is Nothing,
                    New PivotPoint(id, -1, -1),
                    New PivotPoint(id, p.Value.X, p.Value.Y))
            Next

            For Each pivotPoint In pivotPointsIncludingUndefined
                Write(Convert.ToInt16(pivotPoint.X))
                Write(Convert.ToInt16(pivotPoint.Y))
            Next
        End Sub

        Public Overloads Sub Write(pixels As Int32PixelARGB())
            For Each pixel In pixels
                Write(pixel.Value)
            Next
        End Sub

        Public Overloads Sub Write(pixels As Int16Pixel565())
            For Each pixel In pixels
                Write(Convert.ToUInt16(pixel.Value))
            Next
        End Sub

        Public Overloads Sub Write(pixels As IndexedPixel())
            For Each pixel In pixels
                Write(Convert.ToByte(pixel.Value))
            Next
        End Sub

        Public Sub WriteReservedPaletteGammaSection()
            Dim bytes(ReservedBytesSize - 1) As Byte
            MyBase.Write(bytes)
        End Sub
    End Class
End Namespace
