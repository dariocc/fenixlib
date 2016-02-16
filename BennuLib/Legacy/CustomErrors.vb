Module CustomErrors



    '===============================================================================
    ' MODULE OVERVIEW
    '===============================================================================
    ' SpritePocket error constants and utility functions.
    '===============================================================================

    ' Error messages are defined in the ./res/SpritePocket.res resource file and are
    ' related to these error constants.
    Public Const ERR_CANNOTOPENFILE As Integer = 101
    Public Const ERR_NOTANFPGFILE As Integer = 102
    Public Const ERR_UNSUPPORTEDVERSION As Integer = 103
    Public Const ERR_PALETTETRUNCATED As Integer = 104
    Public Const ERR_GAMMATRUNCATED As Integer = 105
    Public Const ERR_CPINFOTRUNCATED As Integer = 106
    Public Const ERR_ANIMATIONFLAGACTIVE As Integer = 107
    Public Const ERR_BITMAPDATATRUNCATED As Integer = 108
    ' Public Const ERR_OBJECTUNAVAILABLE As Long = 109
    Public Const ERR_NOTAMAPFILE As Integer = 110
    Public Const ERR_NOTAPALFILE As Integer = 111
    Public Const ERR_MAPINDEXOUTOFRANGE As Integer = 112
    Public Const ERR_DIFFERENTDEPTHS As Integer = 113
    Public Const ERR_INVALIDCODE As Integer = 114
    Public Const ERR_PALETTEREQUIRED As Integer = 115
    Public Const ERR_UNSUPPORTEDDEPTH As Integer = 116
    Public Const ERR_SETPALETTEINVALIDDEPTH As Integer = 117
    Public Const ERR_INVALIDCPID As Integer = 118
    Public Const ERR_INVALIDMAPWIDTH As Integer = 119
    Public Const ERR_INVALIDMAPHEIGHT As Integer = 120
    Public Const ERR_INVALIDCOLORINDEX As Integer = 121
    Public Const ERR_INSUFFICIENTARRAYELEMENTS As Integer = 122
    Public Const ERR_FILEDOESNOTCONTAINPALETTE As Integer = 123
    Public Const ERR_CANNOTCONVERTMAPINFPG As Integer = 124
    Public Const ERR_INVALIDDEPTH As Integer = 125
    Public Const ERR_NOTANFNTFILE As Integer = 126
    Public Const ERR_SERIALIZEDMAPINVALID As Integer = 127
    Public Const ERR_INVALIDCOMPRESSIONLEVEL As Integer = 128
    Public Const ERR_ZLIB As Integer = 129
    Public Const ERR_FILEWRITE As Integer = 130

    ' FreeImage error constants
    Public Const ERR_FREEIMAGE As Integer = 2000
    Public Const ERR_FREEIMAGE_NOTKNOWNFILE As Integer = 2001

    ' As opposed to errors, warnings do not represent a SpritePocket failure and don't
    ' need to be handled
    Public Const WARNING_DUPLICATEDCODE As Integer = 1001
    Public Const WARNING_UNSUPPORTEDOPERATION As Integer = 1002
    Public Const WARNING_CANNOTCREATEOBJECT As Integer = 1003
    Public Const WARNING_NOTSTANDARDDECODER As Integer = 1004

End Module