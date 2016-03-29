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
using System.Text;
using System.Threading.Tasks;
using FenixLib.Core;

namespace FenixLib.IO
{
    public partial class NativeFormatWriter
    {
        internal sealed class ArrangedPivotPointsView
        {

            private int realCenterX;
            private int realCenterY;

            public ArrangedPivotPointsView( IEnumerable<PivotPoint> pivotPoints, 
                int spriteWidth, int spriteHeight )
            {
                ArrangedPivotPoints = Arrange ( new HashSet<PivotPoint>(pivotPoints) );
                realCenterX = spriteWidth / 2;
                realCenterY = spriteHeight / 2;
            }

            public IEnumerable<PivotPoint> ArrangedPivotPoints { get; }

            public int PivotPointsCount
            {
                get
                {
                    var last = ArrangedPivotPoints.Last ();

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

            // Creates an enumerable of PivotPoint objects based on a set of pivotPoints.
            // The returned enumerable is sorted by the PivotPoint.Id and contains PivotPoint 
            // defined for every id from 0 to max ( id of pivotPoints ) with the coordinates
            // set to -1, -1.
            // 
            private IEnumerable<PivotPoint> Arrange ( ISet<PivotPoint> pivotPoints )
            {
                if ( pivotPoints.Count == 0 )
                {
                    return null;
                }

                var maxId = pivotPoints.Max ( p => p.Id );
                var allIds = pivotPoints.Select ( p => p.Id ).OrderBy ( x => x );

                var arrangedPoints = new PivotPoint[maxId + 1];

                for ( int id = 0 ; id <= maxId ; id++ )
                {
                    int x = -1; int y = -1;

                    if ( allIds.Contains ( id ) )
                    {
                        var point = pivotPoints.Where ( p => p.Id == id ).First ();
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
