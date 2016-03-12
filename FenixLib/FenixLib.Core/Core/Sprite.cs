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
using System.Linq;

namespace FenixLib.Core
{

    /// <summary>
    /// A sprite is SpritePocket concept of image data (pixel information) and
    /// pivot points information grouped.
    ///
    /// Sprites can be collected in <see cref="SpriteAsset"></see>s and given a code
    /// from which it is possible to be retrieved later on.
    /// </summary>
    public partial class Sprite : ISprite
    {
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

        public virtual int Width => graphic.Width;

        public virtual int Height => graphic.Height;

        public virtual GraphicFormat GraphicFormat => graphic.GraphicFormat;

        public virtual byte[] PixelData => graphic.PixelData;

        public virtual Palette Palette => graphic.Palette;

        /// <summary>
        /// A descriptive string.
        /// </summary>
        /// <returns></returns>
        public virtual string Description { get; set; }

        public virtual void DefinePivotPoint ( int id, int x, int y )
        {
            if ( !IsValidPivotPointId ( id ) )
            {
                throw new ArgumentOutOfRangeException ();
            }

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

        public virtual PivotPoint GetPivotPoint ( int id )
        {
            if ( !pivotPoints.ContainsKey ( id ) )
            {
                throw new ArgumentOutOfRangeException ( nameof ( id ) );
            }

            return pivotPoints[id];
        }

        public virtual void DeletePivotPoint ( int id )
        {
            if ( pivotPoints.ContainsKey ( id ) )
            {
                pivotPoints.Remove ( id );
            }
        }

        public virtual void ClearPivotPoints ()
        {
            pivotPoints.Clear ();
        }

        public virtual ICollection<PivotPoint> PivotPoints
        {
            get { return pivotPoints.Values; }
        }

        /// <summary>
        /// Checks if a pivot point id has been defined.
        /// </summary>
        /// <param name="id">the id of the pivot point</param>
        /// <returns>True if the pivot point has been defined.</returns>
        public virtual bool IsPivotPointDefined ( int id )
        {
            return pivotPoints.ContainsKey ( id );
        }

        public virtual int? FindFreePivotPointId ( int start = 0,
            SearchDirection direction = SearchDirection.Fordward )
        {
            if ( !IsValidPivotPointId ( start ) )
            {
                throw new ArgumentOutOfRangeException ();
            }

            if ( pivotPoints.Count == 0 )
            {
                return start;
            }

            int firstAvailable;

            if ( direction == SearchDirection.Fordward )
            {
                int from = start;
                int count = MaxPivotPointId - start ;
                firstAvailable = Enumerable.Range ( from, count )
                                                .Except ( pivotPoints.Keys )
                                                .DefaultIfEmpty (MaxPivotPointId + 1)
                                                .First ();
            }
            else if ( direction == SearchDirection.Backward )
            {
                int from = MinPivotPointId;
                int count = start + 1;
                firstAvailable = Enumerable.Range ( from, count )
                                                .Reverse ()
                                                .Except ( pivotPoints.Keys )
                                                .DefaultIfEmpty (MinPivotPointId - 1)
                                                .First ();
            }
            else
            {
                throw new ArgumentOutOfRangeException ( nameof ( direction ) );
            }

            if ( IsValidPivotPointId ( firstAvailable ) )
                return firstAvailable;
            else
                return null;
        }

        private static bool IsValidPivotPointId ( int id )
        {
            return id <= MaxPivotPointId & id >= MinPivotPointId;
        }

        private const int MaxPivotPointId = 999;
        private const int MinPivotPointId = 0;
    }
}
