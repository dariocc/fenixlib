Option Infer On

Imports System.Collections.Generic

<Serializable>
Public Class SpriteAsset
    Implements IEnumerable(Of Sprite)

    Private Const MinCode As Integer = 1
    Private Const MaxCode As Integer = 999

    <Obsolete>
    Private Function IsIdValid(x As Integer) As Boolean
        Return x >= MinCode And x <= MaxCode
    End Function

    Private _sprites As IDictionary(Of Integer, Sprite) = New SortedDictionary(Of Integer, Sprite)
    Private _palette As Palette

    Default Public ReadOnly Property Item(code As Integer) As Sprite
        Get
            Return _sprites(code)
        End Get
    End Property

    Friend ReadOnly Property Palette() As Palette
        Get
            Return _palette
        End Get
    End Property

    <Obsolete>
    Private _depth As Short

    <Obsolete>
    Public ReadOnly Property Depth() As Short
        Get
            Return _depth
        End Get
    End Property

    Public ReadOnly Property Count As Integer
        Get
            Return _sprites.Count
        End Get
    End Property

    Public ReadOnly Property Sprites As ICollection(Of Sprite)
        Get
            Return _sprites.Values
        End Get
    End Property

    Public Sub Add(code As Integer, ByRef sprite As Sprite)
        ' TODO: ensure palette constraint
        _sprites.Add(code, sprite)
        sprite.ParentAsset = Me
    End Sub

    Public Sub Update(code As Integer, map As Sprite)
        If _sprites.ContainsKey(code) Then
            _sprites.Remove(code)
        End If

        _sprites.Add(code, map)
    End Sub

    Friend Function IdOf(sprite As Sprite) As Integer
        For Each kvp As KeyValuePair(Of Integer, Sprite) In _sprites
            If kvp.Value Is sprite Then
                Return kvp.Key
            End If
        Next

        Throw New ArgumentException() ' TODO customize
    End Function

    Public Function FindFreeId(Optional startId As Integer = MinCode) As Integer

        If IsIdValid(startId) Then Throw New ArgumentException() ' TODO: Customize

        Dim found = False
        Dim code = startId - 1
        Do
            code += 1
            If Not _sprites.ContainsKey(code) Then
                found = True
            End If
        Loop Until code = MaxCode Or found

        If Not found Then Throw New InvalidOperationException() ' TODO: Customize 

        Return code
    End Function

    Public Function PreviousFreeCode(Optional startId As Integer = MaxCode) As Integer
        If IsIdValid(startId) Then Throw New ArgumentException()  ' TODO: Customize

        Dim found = False
        Dim code = startId + 1
        Do
            code -= 1
            If Not _sprites.ContainsKey(code) Then
                found = True
            End If
        Loop Until code = MinCode Or found

        If Not found Then Throw New InvalidOperationException() ' TODO: Customize 

        Return code
    End Function

    Public Function GetEnumerator() As IEnumerator(Of Sprite) Implements IEnumerable(Of Sprite).GetEnumerator
        Return _sprites.Values.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return _sprites.Values.GetEnumerator()
    End Function
End Class