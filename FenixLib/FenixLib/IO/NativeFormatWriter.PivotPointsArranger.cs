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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenixLib.Core;

namespace FenixLib.IO
{
    public partial class NativeFormatWriter
    {
        /// <summary>
        /// A collection of PivotPoints that is in a suitable format to be written to 
        /// disk by the NativeFormatWriter.
        /// This means that for a given IEnumerable of PivotPoints that is passed to the
        /// constructor, the <see cref="WritablePivotPointsView"/> will produce a collection
        /// of PivotPoints that take into account the following:
        /// 
        ///     - All PivotPoints whose coordinates are exactly (-1, -1) are discarded.
        ///     
        ///     - All PivotPoints from id = 0 to id = max of PivotPoints Id are defined.
        ///     
        ///     - The PivotPoints that were not defined in the original collection are
        ///       set to coordinates (-1, -1).
        ///       
        ///     - If there is only one pivot point, and the id is 0, the 
        ///       <see cref="WritablePivotPointsView"/> will only contain that pivot point,
        ///       unless the coordinates of that pivot point are equal to the center of the
        ///       dimension specified in the constructor via spriteWidth and spriteHeight.
        /// </summary>
        public sealed class WritablePivotPointsView : IEnumerable<PivotPoint>
        {

            private readonly int realCenterX;
            private readonly int realCenterY;
            private readonly PivotPoint[] arrangedPoints;

            public WritablePivotPointsView ( IEnumerable<PivotPoint> pivotPoints,
                int spriteWidth, int spriteHeight )
            {
                arrangedPoints = Arrange ( new HashSet<PivotPoint> ( pivotPoints ) );
                realCenterX = spriteWidth / 2;
                realCenterY = spriteHeight / 2;
            }

            public int Count
            {
                get
                {
                    if ( arrangedPoints == null )
                    {
                        return 0;
                    }

                    var last = arrangedPoints.Last ();

                    int count;

                    if ( last.Id == 0 && last.X == realCenterX && last.Y == realCenterY )
                    {
                        count = 0;
                    }
                    else
                    {
                        count = last.Id + 1;
                    }

                    return count;
                }
            }

            public IEnumerator<PivotPoint> GetEnumerator ()
            {
                if ( Count == 0 )
                {
                    return Enumerable.Empty<PivotPoint> ().GetEnumerator ();
                }

                return arrangedPoints.AsEnumerable ().GetEnumerator ();
            }

            IEnumerator IEnumerable.GetEnumerator ()
            {
                return GetEnumerator ();
            }

            // Creates an enumerable of PivotPoint objects based on a set of pivotPoints.
            // The returned enumerable is sorted by the PivotPoint.Id and contains PivotPoint 
            // defined for every id from 0 to max ( id of pivotPoints ) with the coordinates
            // set to -1, -1.
            // 
            private PivotPoint[] Arrange ( ISet<PivotPoint> pivotPoints )
            {
                // Discard pivot points with coordinates (-1, -1)
                var validPoints = pivotPoints.Where ( p => p.X != -1 && p.Y != -1 );

                if ( validPoints.Any () == false )
                {
                    return null;
                }

                var maxId = validPoints.Max ( p => p.Id );
                var allIds = validPoints.Select ( p => p.Id ).OrderBy ( x => x );

                var arrangedPoints = new PivotPoint[maxId + 1];

                for ( int id = 0 ; id <= maxId ; id++ )
                {
                    // Non-defined pivot points have coordinates -1, -1
                    int x = -1; int y = -1;

                    if ( allIds.Contains ( id ) )
                    {
                        var point = validPoints.Where ( p => p.Id == id ).First ();
                        x = point.X;
                        y = point.Y;
                    }

                    arrangedPoints[id] = new PivotPoint ( id, x, y );
                }

                return arrangedPoints;
            }
        }
    }
}
