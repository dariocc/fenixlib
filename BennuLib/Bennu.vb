Option Infer On

Imports System.IO
Imports PkSprite.Core

Namespace Bennu
    Public Module Map
        Function Load(fileName As String) As Sprite
            Dim decoder As New IO.MapSpriteDecoder()

            Using stream = File.Open(fileName, FileMode.Open)
                Return decoder.Decode(stream)
            End Using

            Return Nothing
        End Function

        Sub Save(sprite As Sprite, fileName As String)
            Dim encoder As New IO.MapSpriteEncoder
            Using output = System.IO.File.Open(fileName, FileMode.Create)
                encoder.Encode(sprite, output)
            End Using
        End Sub
    End Module

    Public Module Fpg
        Function Load(fileName As String) As SpriteAsset
            Dim decoder As New IO.FpgSpriteAssetDecoder()

            Using stream = File.Open(fileName, FileMode.Open)
                Return decoder.Decode(stream)
            End Using

            Return Nothing
        End Function
    End Module

    Public Module Pal
        Function Load(fileName As String) As Palette
            Dim decoder As New IO.DivFormatPaletteDecoder()

            Using stream = File.Open(fileName, FileMode.Open)
                Return decoder.Decode(stream)
            End Using

            Return Nothing
        End Function
    End Module
End Namespace