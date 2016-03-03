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

namespace FenixLib.Core
{
	public struct PivotPoint
	{
		public readonly int Id;
		public readonly int X;
		public readonly int Y;

		internal PivotPoint(int id, int x, int y)
		{
			Id = id;
			X = x;
			Y = y;
		}

        public bool Equals(PivotPoint point)
        {
            if (ReferenceEquals(point, null))
            {
                return false;
            }

            return point.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if ( !(obj is PivotPoint) )
            {
                return false;
            }

            return Equals((PivotPoint) obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Id.GetHashCode();
        }

        public static bool operator == (PivotPoint pointA, PivotPoint pointB)
        {
            return pointA.Equals(pointB);
        }

        public static bool operator != (PivotPoint pointA, PivotPoint pointB)
        {
            return !pointA.Equals(pointB);
        }
    }
}
