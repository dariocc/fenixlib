using System;
using System.Collections.Generic;

namespace BennuLib
{

	/// <summary>
	/// A sprite is SpritePocket concept of image data (pixel information) and
	/// pivot points information grouped.
	///
	/// Sprites can be collected in <see cref="SpriteAsset"></see>s and given a code from which
	/// it is possible to be retrieved later on.
	/// </summary>
	[Serializable()]
	public partial class Sprite
	{

		private IPixel[] _pixels;
		private Palette _palette;
		private SpriteAsset _parent;
		// TODO: Limit max pivot point ID Max pivot point ID is 999 (checked with bennu).

		private IDictionary<int, PivotPoint> _pivotPoints = new SortedDictionary<int, PivotPoint>();
		private const int MaxPivotPointId = 999;
		private const int MinPivotPointId = 0;
		private static bool IsValidPivotPointId(int id)
		{
			return id < MaxPivotPointId & id >= MinPivotPointId;
		}

		/// <summary>
		/// Creates an standalone <c>Sprite</c> object.
		/// </summary>
		/// <param name="width">The width of the sprite</param>
		/// <param name="height">The height of the sprite</param>
		/// <param name="pixelBuffer">The data that defines the pixel of the sprite</param>
		/// <returns></returns>
		public static Sprite Create(int width, int height, IPixel[] pixelBuffer)
		{
			return new Sprite(width, height, pixelBuffer);
		}

		/// <summary>
		/// The width.
		/// </summary>
		/// <returns>The width in pixels.</returns>
		public int Width { get; }

		/// <summary>
		/// The height.
		/// </summary>
		/// <returns>The height in pixels.</returns>
		public int Height { get; }

		/// <summary>
		/// The <see cref="Sprite"/> identifier.
		/// </summary>
		/// <returns>The identifier of this <see cref="Sprite"/> within its
		/// parent <see cref="SpriteAsset"/>. <c>Nothing</c> if this object
		/// is not contained in the <see cref="SpriteAsset"/></returns>
		public int? Id {
			get {
                if (_parent == null)
                    return null;
                else
                    return _parent.IdOf(this);
            }
		}

		/// <summary>
		/// A descriptive string.
		/// </summary>
		/// <returns></returns>
		public string Description { get; set; }

		public Palette Palette {
			get { return _palette; }
		}

        /// <summary>
        /// The depth of the map, which is determined by the type of its <see cref="Pixels"/>.
        /// When the type of the pixels is undefined, the value of this property is 0.
        /// </summary>
        public int Depth
        {
            get {
                return PixelArrays.GetDepth(_pixels);
            }
        }

        protected Sprite(int width, int height, IPixel[] pixels)
		{
			Width = width;
			Height = height;

			if (pixels.Length != width * height) {
				throw new ArgumentException();
				// TODO: Customize
			}

			_pixels = pixels;
		}

		public void DefinePivotPoint(int id, int x, int y)
		{
			var pivotPoint = new PivotPoint(id, x, y);

			if (_pivotPoints.ContainsKey(pivotPoint.Id)) {
				_pivotPoints.Remove(pivotPoint.Id);
			}

			if ( ! (x == -1 && y == -1) ) {
				_pivotPoints.Add(pivotPoint.Id, pivotPoint);
			}
		}

		public void DeletePivotPoint(int id)
		{
			if (_pivotPoints.ContainsKey(id)) {
				_pivotPoints.Remove(id);
			}
		}

		public void ClearPivotPoints()
		{
			_pivotPoints.Clear();
		}

		public ICollection<PivotPoint> PivotPoints {
			get { return _pivotPoints.Values; }
		}

		public bool IsInAsset {
			get { return _parent == null; }
		}

		public SpriteAsset ParentAsset {
			get { return _parent; }
			internal set {
				if (!value.Sprites.Contains(this)) {
					throw new InvalidOperationException();
					// TODO: Customize
				}

				_parent = value;
			}
		}

		/// <summary>
		/// Checks if a pivot point id has been defined.
		/// </summary>
		/// <param name="id">the id of the pivot point</param>
		/// <returns>True if the pivot point has been defined.</returns>
		public bool IsPivotPointDefined(int id)
		{
			return _pivotPoints.ContainsKey(id);
		}

		public int FindFreePivotPointId(int start = 0, SearchDirection direction = SearchDirection.Fordward)
		{
			if (direction == SearchDirection.Fordward) {
				// TODO: What happens if all Pivot Points are defined
				for (var n = start; n <= _pivotPoints.Count - 1; n++) {
					if (_pivotPoints[n].Id != n)
						return n;
				}

				return _pivotPoints.Count;
			} else if (direction == SearchDirection.Backward) {
				for (var n = start; n <= 0; n++) {
					if (_pivotPoints[n].Id != n)
						return n;
				}

				return -1;
			}

			return -1;
		}

		public IPixel[] Pixels {
			get { return _pixels; }
		}

		// TODO: Might not belong here
		//Public Sub RemoveTransparency()
		//    Dim i As Integer = -1
		//    For Each pixel As IPixel In _pixels
		//        i += 1
		//        ' TODO: Create method IsTransparent?
		//        If pixel.IsTransparent Then _pixels(i) = pixel.GetOpaqueCopy()
		//    Next
		//End Sub

		//Public Function GetCopy() As Sprite
		//    ' TODO: Should the pixelBuffers be encapsulated in a "PixelBuffer" object, that
		//    ' supports color spaces?
		//    Dim pixelBuffer(_pixels.Length - 1) As IPixel
		//    _pixels.CopyTo(pixelBuffer, 0)

		//    Dim sprite = Create(Me.Width, Me.Height, pixelBuffer)
		//    sprite.Description = Description

		//    For Each pivotPoint In PivotPoints
		//        sprite.DefinePivotPoint(pivotPoint.Id, pivotPoint.X, pivotPoint.Y)
		//    Next

		//    ' TODO: Palette

		//    Return sprite
		//End Function


	}
}
