Imports System.IO
Imports System.Collections.Generic


Public Interface IDecoder(Of T)
    ReadOnly Property SupportedExtensions As IEnumerable(Of String)
    Function TryDecode(input As Stream, ByRef decoded As T) As Boolean
    Function Decode(input As Stream) As T
End Interface

