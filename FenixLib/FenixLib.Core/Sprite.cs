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
    public partial class Sprite : ISprite
    {
        private SpriteAsset parent;
        private IGraphic graphic;

        private IDictionary<int, PivotPoint> pivotPoints = 
            new SortedDictionary<int, PivotPoint> ();

        public Sprite ( IGraphic graphic )
        {
            if ( graphic == null )
            {
                throw new ArgumentNullException ( "graphic" );
            }

            this.graphic = graphic;
        }

        public int Width => graphic.Width;

        public int Height => graphic.Height;

        public GraphicFormat GraphicFormat => graphic.GraphicFormat;

        public byte[] PixelData => graphic.PixelData;

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
                    return graphic.Palette; ;
                }
            }
        }

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
                if ( parent == null )
                {
                    return null;
                }
                else
                {
                    return parent.IdOf ( this );
                }
            }
        }

        /// <summary>
        /// A descriptive string.
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }

        public SpriteAsset ParentAsset
        {
            get { return parent; }
            set
            {
                // TODO: This design makes it impossible to detach an Sprite from its parent
                if ( !value.Sprites.Contains ( this ) )
                {
                    throw new InvalidOperationException (); // TODO: Customize
                }

                parent = value;
            }
        }

        public bool IsInAsset
        {
            get { return parent == null; }
        }

        public void DefinePivotPoint ( int id, int x, int y )
        {
            var pivotPoint = new PivotPoint ( id, x, y );

            if ( pivotPoints.ContainsKey ( pivotPoint.Id ) )
            {
                pivotPoints.Remove ( pivotPoint.Id );
            }

            if ( !( x == -1 && y == -1 ) )
            {
                pivotPoints.Add ( pivotPoint.Id, pivotPoint );
            }
        }

        public void DeletePivotPoint ( int id )
        {
            if ( pivotPoints.ContainsKey ( id ) )
            {
                pivotPoints.Remove ( id );
            }
        }

        public void ClearPivotPoints ()
        {
            pivotPoints.Clear ();
        }

        public ICollection<PivotPoint> PivotPoints
        {
            get { return pivotPoints.Values; }
        }

        /// <summary>
        /// Checks if a pivot point id has been defined.
        /// </summary>
        /// <param name="id">the id of the pivot point</param>
        /// <returns>True if the pivot point has been defined.</returns>
        public bool IsPivotPointDefined ( int id )
        {
            return pivotPoints.ContainsKey ( id );
        }

        public int FindFreePivotPointId ( int start = 0, 
            SearchDirection direction = SearchDirection.Fordward )
        {
            if ( direction == SearchDirection.Fordward )
            {
                // TODO: What happens if all Pivot Points are defined
                for ( var n = start ; n <= pivotPoints.Count - 1 ; n++ )
                {
                    if ( pivotPoints[n].Id != n )
                        return n;
                }

                return pivotPoints.Count;
            }
            else if ( direction == SearchDirection.Backward )
            {
                for ( var n = start ; n <= 0 ; n++ )
                {
                    if ( pivotPoints[n].Id != n )
                        return n;
                }

                return -1;
            }

            return -1;
        }

        private static bool IsValidPivotPointId ( int id )
        {
            const int MaxPivotPointId = 999;
            const int MinPivotPointId = 0;

            return id < MaxPivotPointId & id >= MinPivotPointId;
        }
    }
}
