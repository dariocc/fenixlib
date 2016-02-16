Option Infer On

Imports System.IO
Imports System.IO.Compression

Namespace Bennu.IO
    Public MustInherit Class NativeDecoder(Of T)
        Implements IDecoder(Of T)

        Public MustOverride ReadOnly Property MaxSupportedVersion As Integer
        Protected MustOverride Function ReadNativeFormat(magic As Magic, reader As NativeFormatReader) As T
        Protected MustOverride ReadOnly Property KnownFileIds As String()
        Protected MustOverride ReadOnly Property KnownFileExtensions As String()

        Public ReadOnly Property SupportedExtensions As IEnumerable(Of String) Implements IDecoder(Of T).SupportedExtensions
            Get
                Return KnownFileExtensions
            End Get
        End Property

        Protected Shared Function IsGZip(header As Byte()) As Boolean
            Return header.Length >= 2 And header(0) = 31 And header(1) = 139
        End Function

        Public Function Decode(input As Stream) As T Implements IDecoder(Of T).Decode
            ' TODO: Do we need to check if I can seek? Should we make the function protected?
            Dim buff(1) As Byte
            If Not input.Read(buff, 0, 2) = 2 Then
                Throw New IOException()
            End If

            input.Position = 0

            Dim stream As Stream
            If IsGZip(buff) Then
                stream = New GZipStream(input, CompressionMode.Decompress)
            Else
                stream = input
            End If

            Using reader As New NativeFormatReader(stream)
                Dim magic = reader.ReadMagic

                If Not magic.IsValid Then Throw New UnsuportedFileFormatException()
                If magic.Version > MaxSupportedVersion Then Throw New UnsuportedFileFormatException()

                Return ReadNativeFormat(magic, reader)
            End Using

        End Function

        Protected Shared Function CreatePixelBuffer(depth As Integer, graphicData() As Byte) As IPixel()
            Select Case depth
                Case 8
                    Return IndexedPixel.CreateBufferFromBytes(graphicData)
                Case 16
                    Return Int16Pixel565.CreateBufferFromBytes(graphicData)
                Case 32
                    Return Int32PixelARGB.CreateBufferFromBytes(graphicData)
                Case Else
                    Throw New ArgumentException() ' TODO: Customize
            End Select
        End Function

        Protected Shared Function VGAtoColors(colorData() As Byte) As Palette.Color()
            Dim colors(colorData.Length \ 3 - 1) As Palette.Color
            For n = 0 To colors.Length - 1
                colors(n) = New Palette.Color(colorData(n * 3) << 2, colorData(n * 3 + 1) << 2, colorData(n * 3 + 1) << 2)
            Next
            Return colors
        End Function

        Public Function TryDecode(input As Stream, ByRef decoded As T) As Boolean Implements IDecoder(Of T).TryDecode
            Try
                decoded = Decode(input)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
End Namespace
