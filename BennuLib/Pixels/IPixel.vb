Public Interface IPixel
    ReadOnly Property Argb() As Integer
    ReadOnly Property Red As Integer
    ReadOnly Property Green As Integer
    ReadOnly Property Blue As Integer
    ReadOnly Property Alpha As Integer
    ReadOnly Property Value As Integer
    Function GetTransparentCopy() As IPixel
    Function GetOpaqueCopy() As IPixel
    ReadOnly Property IsTransparent As Boolean
End Interface