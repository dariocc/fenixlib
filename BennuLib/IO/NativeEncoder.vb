Option Infer On

Imports System.IO.Compression
Imports System.IO
Imports PkSprite.Core

Namespace Bennu.IO
    Public MustInherit Class NativeEncoder(Of T)
        Implements IEncoder(Of T)

        Protected MustOverride ReadOnly Property Version As Byte
        Protected MustOverride Sub WriteNativeFormat(obj As T, writer As NativeFormatWriter)
        Protected MustOverride Function GetFileId(obj As T) As String

        Public ReadOnly Compression As CompressionOptions

        Public Sub New()
            Me.New(CompressionOptions.Uncompressed)
        End Sub

        Public Sub New(compressionOptions As CompressionOptions)
            Compression = compressionOptions
        End Sub

        Public Sub Encode(obj As T, output As Stream) Implements IEncoder(Of T).Encode
            Dim nativeStream As Stream

            If Compression = CompressionOptions.Uncompressed Then
                nativeStream = output
            Else
                nativeStream = New GZipStream(output, DirectCast(Compression, CompressionLevel))
            End If
            Using writer As New NativeFormatWriter(output)
                writer.WriteAsciiZ(GetFileId(obj).Substring(0, 3), 3)
                writer.Write(NativeDescriptor)
                writer.Write(Version)
                WriteNativeFormat(obj, writer)
            End Using
        End Sub

        Public Enum CompressionOptions
            Uncompressed = -1
            Fastest = CompressionLevel.Fastest
            Optimal = CompressionLevel.Optimal
            NoCompression = CompressionLevel.NoCompression ' Todo: Is it the same as Uncompressed?
        End Enum
    End Class
End Namespace