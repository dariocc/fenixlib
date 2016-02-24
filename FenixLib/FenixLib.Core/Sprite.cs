/*  Copyright 2016 Darío Cutillas Carrillo
*
*   Licensed under the Apache License, Version 2.0 (the "License");
*   you may not use this file except in compliance with the License.
*   You may obtain a copy of the License at
*
*       http://www.apache.org/licenses/LICENSE-2.0
*
*   Unless required by applicable law or agreed to in writing, software
*   distributed under the License is distributed on an "AS IS" BASIS,
*   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*   See the License for the specific language governing permissions and
*   limitations under the License.
*/
using System;
using System.Collections.Generic;

namespace FenixLib.Core
{

    /// <summary>
    /// A sprite is SpritePocket concept of image data (pixel information) and
    /// pivot points information grouped.
    ///
    /// Sprites can be collected in <see cref="SpriteAsset"></see>s and given a code
    /// from which it is possible to be retrieved later on.
    /// </summary>
    [Serializable ()]
    public partial class Sprite
    {

        private byte[] _pixelData;
        private Palette _palette;
        private SpriteAsset _parent;

        // TODO: Limit max pivot point ID Max pivot point ID is 999 (checked with bennu).

        private IDictionary<int, PivotPoint> _pivotPoints = 
            new SortedDictionary<int, PivotPoint> ();
        private const int MaxPivotPointId = 999;
        private const int MinPivotPointId = 0;
        private static bool IsValidPivotPointId ( int id )
        {
            return id < MaxPivotPointId & id >= MinPivotPointId;
        }


        public static Sprite Create ( DepthMode depth, int width, int height, 
            byte[] pixelData, Palette palette = null )
        {
            if ( width <= 0 || height <= 0 )
                throw new ArgumentOutOfRangeException (); // TODO: Customize

            // TODO: Validate the size of pixelData array based on the depth

            if ( ( depth == DepthMode.RgbIndexedPalette ) != ( palette != null ) )
                throw new ArgumentException (); // TODO: Customize

            return new Sprite ( width, height, ( int ) depth, pixelData, palette );
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
        public int? Id
        {
            get
            {
                if ( _parent == null )
                    return null;
                else
                    return _parent.IdOf ( this );
            }
        }

        /// <summary>
        /// A descriptive string.
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }

        public Palette Palette
        {
            get
            {
                if ( IsInAsset )
                {
                    return ParentAsset.Palette;
                }
                else
                {
                    return _palette;
                }
            }
        }

        public int Depth { get; }

        protected Sprite ( int width, int height, int depth, byte[] pixelData, 
            Palette palette = null )
        {
            Width = width;
            Height = height;
            _palette = palette;
            _pixelData = pixelData;
            Depth = depth;
        }

        public void DefinePivotPoint ( int id, int x, int y )
        {
            var pivotPoint = new PivotPoint ( id, x, y );

            if ( _pivotPoints.ContainsKey ( pivotPoint.Id ) )
            {
                _pivotPoints.Remove ( pivotPoint.Id );
            }

            if ( !( x == -1 && y == -1 ) )
            {
                _pivotPoints.Add ( pivotPoint.Id, pivotPoint );
            }
        }

        public void DeletePivotPoint ( int id )
        {
            if ( _pivotPoints.ContainsKey ( id ) )
            {
                _pivotPoints.Remove ( id );
            }
        }

        public void ClearPivotPoints ()
        {
            _pivotPoints.Clear ();
        }

        public ICollection<PivotPoint> PivotPoints
        {
            get { return _pivotPoints.Values; }
        }

        public bool IsInAsset
        {
            get { return _parent == null; }
        }

        public SpriteAsset ParentAsset
        {
            get { return _parent; }
            internal set
            {
                // Is this check necessary? It is like not trusting in 
                // the internal code... it'd be better covered with a test
                if ( !value.Sprites.Contains ( this ) )
                {
                    throw new InvalidOperationException (); // TODO: Customize
                }

                _palette = ParentAsset.Palette;
                _parent = value;
            }
        }

        /// <summary>
        /// Checks if a pivot point id has been defined.
        /// </summary>
        /// <param name="id">the id of the pivot point</param>
        /// <returns>True if the pivot point has been defined.</returns>
        public bool IsPivotPointDefined ( int id )
        {
            return _pivotPoints.ContainsKey ( id );
        }

        public int FindFreePivotPointId ( int start = 0, 
            SearchDirection direction = SearchDirection.Fordward )
        {
            if ( direction == SearchDirection.Fordward )
            {
                // TODO: What happens if all Pivot Points are defined
                for ( var n = start ; n <= _pivotPoints.Count - 1 ; n++ )
                {
                    if ( _pivotPoints[n].Id != n )
                        return n;
                }

                return _pivotPoints.Count;
            }
            else if ( direction == SearchDirection.Backward )
            {
                for ( var n = start ; n <= 0 ; n++ )
                {
                    if ( _pivotPoints[n].Id != n )
                        return n;
                }

                return -1;
            }

            return -1;
        }

        public byte[] PixelData
        {
            get { return _pixelData; }
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
