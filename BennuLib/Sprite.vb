Option Infer On

''' <summary>
''' A sprite is SpritePocket concept of image data (pixel information) and
''' pivot points information grouped.
'''
''' Sprites can be collected in <see cref="SpriteAsset"></see>s and given a code from which
''' it is possible to be retrieved later on.
''' </summary>
<Serializable>
Public Class Sprite

    Private _pixels() As IPixel
    Private _palette As Palette
    Private _parent As SpriteAsset
    ' TODO: Limit max pivot point ID Max pivot point ID is 999 (checked with bennu).
    Private _pivotPoints As IDictionary(Of Integer, PivotPoint) = New SortedDictionary(Of Integer, PivotPoint)

    Private Const MaxPivotPointId = 999
    Private Const MinPivotPointId = 0
    Private Shared Function IsValidPivotPointId(id As Integer) As Boolean
        Return id < MaxPivotPointId And id >= MinPivotPointId
    End Function

    ''' <summary>
    ''' Creates an standalone <c>Sprite</c> object.
    ''' </summary>
    ''' <param name="width">The width of the sprite</param>
    ''' <param name="height">The height of the sprite</param>
    ''' <param name="pixelBuffer">The data that defines the pixel of the sprite</param>
    ''' <returns></returns>
    Public Shared Function Create(width As Integer, height As Integer, pixelBuffer() As IPixel) As Sprite
        Return New Sprite(width, height, pixelBuffer)
    End Function

    ''' <summary>
    ''' The width.
    ''' </summary>
    ''' <returns>The width in pixels.</returns>
    Public ReadOnly Property Width As Integer

    ''' <summary>
    ''' The height.
    ''' </summary>
    ''' <returns>The height in pixels.</returns>
    Public ReadOnly Property Height As Integer

    ''' <summary>
    ''' The <see cref="Sprite"/> identifier.
    ''' </summary>
    ''' <returns>The identifier of this <see cref="Sprite"/> within its
    ''' parent <see cref="SpriteAsset"/>. <c>Nothing</c> if this object
    ''' is not contained in the <see cref="SpriteAsset"/></returns>
    Public ReadOnly Property Id As Integer?
        Get
            Return If(_parent Is Nothing, Nothing, _parent.IdOf(Me))
        End Get
    End Property

    ''' <summary>
    ''' A descriptive string.
    ''' </summary>
    ''' <returns></returns>
    Public Property Description As String

    Public ReadOnly Property Palette As Palette
        Get
            Return _palette
        End Get
    End Property

    Private Sub New(width As Integer, height As Integer, pixels() As IPixel)
        Me.Width = width
        Me.Height = height

        If pixels.Length <> width * height Then
            Throw New ArgumentException() ' TODO: Customize
        End If

        _pixels = pixels
    End Sub

    Public Sub DefinePivotPoint(id As Integer, x As Integer, y As Integer)
        Dim pivotPoint = New PivotPoint(id, x, y)

        If _pivotPoints.ContainsKey(pivotPoint.Id) Then
            _pivotPoints.Remove(pivotPoint.Id)
        End If

        If Not x = -1 And y = -1 Then
            _pivotPoints.Add(pivotPoint.Id, pivotPoint)
        End If
    End Sub

    Public Sub DeletePivotPoint(id As Integer)
        If _pivotPoints.ContainsKey(id) Then
            _pivotPoints.Remove(id)
        End If
    End Sub

    Public Sub ClearPivotPoints()
        _pivotPoints.Clear()
    End Sub

    Public ReadOnly Property PivotPoints() As ICollection(Of PivotPoint)
        Get
            Return _pivotPoints.Values
        End Get
    End Property

    Public ReadOnly Property BelongsToAsset As Boolean
        Get
            Return _parent Is Nothing
        End Get
    End Property

    Public Property ParentAsset() As SpriteAsset
        Get
            Return _parent
        End Get
        Friend Set(asset As SpriteAsset)
            If Not asset.Sprites.Contains(Me) Then
                Throw New InvalidOperationException() ' TODO: Customize
            End If

            _parent = asset
        End Set
    End Property

    ''' <summary>
    ''' Checks if a pivot point id has been defined.
    ''' </summary>
    ''' <param name="id">the id of the pivot point</param>
    ''' <returns>True if the pivot point has been defined.</returns>
    Public Function IsPivotPointDefined(id As Integer) As Boolean
        Return _pivotPoints.ContainsKey(id)
    End Function

    Public Function FindFreePivotPointId(Optional start As Integer = 0, Optional direction As SearchDirection = SearchDirection.Fordward) As Integer
        If direction = SearchDirection.Fordward Then
            ' TODO: What happens if all Pivot Points are defined
            For n = start To _pivotPoints.Count - 1
                If _pivotPoints(n).Id <> n Then Return n
            Next

            Return _pivotPoints.Count
        ElseIf direction = SearchDirection.Backward Then
            For n = start To 0
                If _pivotPoints(n).Id <> n Then Return n
            Next

            Return -1
        End If

        Return -1
    End Function

    Public ReadOnly Property Pixels As IPixel()
        Get
            Return _pixels
        End Get
    End Property

    ' TODO: Might not belong here
    'Public Sub RemoveTransparency()
    '    Dim i As Integer = -1
    '    For Each pixel As IPixel In _pixels
    '        i += 1
    '        ' TODO: Create method IsTransparent?
    '        If pixel.IsTransparent Then _pixels(i) = pixel.GetOpaqueCopy()
    '    Next
    'End Sub

    'Public Function GetCopy() As Sprite
    '    ' TODO: Should the pixelBuffers be encapsulated in a "PixelBuffer" object, that
    '    ' supports color spaces?
    '    Dim pixelBuffer(_pixels.Length - 1) As IPixel
    '    _pixels.CopyTo(pixelBuffer, 0)

    '    Dim sprite = Create(Me.Width, Me.Height, pixelBuffer)
    '    sprite.Description = Description

    '    For Each pivotPoint In PivotPoints
    '        sprite.DefinePivotPoint(pivotPoint.Id, pivotPoint.X, pivotPoint.Y)
    '    Next

    '    ' TODO: Palette

    '    Return sprite
    'End Function
End Class