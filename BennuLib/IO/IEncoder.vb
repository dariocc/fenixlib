Imports System.IO

Public Interface IEncoder(Of T)
    Sub Encode(obj As T, output As Stream)
End Interface
