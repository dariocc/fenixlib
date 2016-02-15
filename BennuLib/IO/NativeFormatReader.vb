Option Infer On

Imports System.IO
Imports PkSprite.Core

Namespace Bennu.IO
    Public Class NativeFormatReader
        Inherits BinaryReader

        Private Shared ReadOnly _encoding As Text.Encoding = Text.Encoding.GetEncoding(850)

        Public Sub New(input As Stream)
            MyBase.New(input, _encoding)
        End Sub

        Public Function ReadDescription() As String
            Return _encoding.GetString(ReadBytes(32))
        End Function

        Public Function ReadMagic() As Magic
            ' 3 first bytes describe the depth of the MAP
            Dim fileType = _encoding.GetString(ReadBytes(3))
            ' Next 4 bytes are MS-DOS termination, and last is the MAP version
            Dim descriptor = ReadBytes(4)
            Dim version = ReadByte()

            Return New Magic(fileType, version, descriptor)
        End Function

        Public Function ReadPalette() As Byte()
            Dim paletteColors = ReadBytes(PaletteSize - 1)
            Return paletteColors
        End Function

        Public Function ReadPivotPoints(number As Integer) As PivotPoint()
            Dim points As New List(Of PivotPoint)
            For n As Integer = 0 To number - 1
                Dim point As New PivotPoint(n, ReadInt16, ReadInt16)
                ' Two coods set to -1 mean invalid point
                If Not (point.X = -1 And point.Y = -1) Then
                    points.Add(point)
                End If
            Next
            Return points.ToArray()
        End Function

        Public Function ReadUnusedPaletteGamma() As Byte()
            Return ReadBytes(ReservedBytesSize)
        End Function

        Public Function ReadPivotPointsNumber() As Integer
            Dim flags = ReadUInt16()
            Dim numberPivotPoints = Convert.ToInt16(flags And NumberOfControlPointsBitMask)
            Return numberPivotPoints
        End Function

        Public Function ReadGlyphInfo() As GlyphInfo
            Return New GlyphInfo(
                    ReadInt32,
                    ReadInt32,
                    ReadInt32,
                    ReadInt32)
        End Function
    End Class
End Namespace