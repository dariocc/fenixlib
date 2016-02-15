Namespace Bennu.IO
    Public Module NativeFormat
        Public ReadOnly NativeDescriptor As Byte() = {&H1A, &H0D, &H0A, &H0}
        Public Const PaletteSize As Short = 768 ' Size in bytes of a palette
        Public Const ReservedBytesSize As Integer = 576
        <Obsolete>
        Public Const NumberOfControlPointsBitMask As Integer = &HFFF ' Bit mask for the number of control points of
        <Obsolete>
        Public Const AnimationFlagBitMask As Integer = &H1000 ' Bit mask for enabled/disabled animation of

        NotInheritable Class Magic
            ' TODO: Think of a different way to handle this. Does not seem good that we need to
            ' repeat this in the native decoders and in the Magic class
            Public Shared ReadOnly Property ValidFormats As String() = {"map", "m16", "m32", "f16", "fpg", "f32", "fnt", "pal"}

            Public ReadOnly Property FileType As String
            Public ReadOnly Property Version As Integer
            Public ReadOnly Descriptor(4) As Byte

            ' TODO: Check for known types?
            Public Sub New(fileType As String, version As Integer, descriptor() As Byte)
                Me.FileType = fileType.ToLower
                Me.Version = version
                Me.Descriptor = descriptor
            End Sub

            Public ReadOnly Property IsDescriptorValid As Boolean
                Get
                    Return StructuralComparisons.StructuralEqualityComparer.Equals(NativeDescriptor, Descriptor)
                End Get
            End Property

            Public ReadOnly Property Depth As Integer
                Get
                    If Integer.TryParse(FileType.Substring(1, 2), Depth) Then
                        Return Depth
                    Else
                        Return 8
                    End If
                End Get
            End Property

            Public ReadOnly Property IsRecognizedFileType As Boolean
                Get
                    Return Array.IndexOf(ValidFormats, FileType) >= 0
                End Get
            End Property

            Public ReadOnly Property IsValid As Boolean
                Get
                    Return IsRecognizedFileType And IsDescriptorValid
                End Get
            End Property

        End Class

        Structure GlyphInfo
            Public ReadOnly Property Width As Integer
            Public ReadOnly Property Height As Integer
            Public ReadOnly Property YOffset As Integer ' Vertical displacement
            Public ReadOnly Property FileOffset As Integer ' Offset of the graphic in the file

            Public Sub New(width As Integer, height As Integer, yOffset As Integer, fileOffset As Integer)
                Me.Width = width
                Me.Height = height
                Me.YOffset = yOffset
                Me.FileOffset = fileOffset
            End Sub
        End Structure
    End Module
End Namespace
