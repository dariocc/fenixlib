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
using System.Collections.Generic;

namespace FenixLib.Core.Tests.IntegrationTests
{
    internal abstract class SpriteAssetElementComparer : IEqualityComparer<SpriteAssetElement>
    {
        private SpriteAssetElementComparer decorated;

        public SpriteAssetElementComparer ( SpriteAssetElementComparer comparer = null )
        {
            decorated = comparer;
        }

        public abstract bool CompareCore ( SpriteAssetElement x, SpriteAssetElement y );

        public abstract int CalculateHashCode ( SpriteAssetElement x );

        public bool Equals ( SpriteAssetElement x, SpriteAssetElement y )
        {
            bool result = true;

            if ( decorated != null )
            {
                result = decorated.CompareCore ( x, y ) ;
            }

            return result & CompareCore ( x, y );
        }

        public int GetHashCode ( SpriteAssetElement obj )
        {
            int hashCode = CalculateHashCode ( obj );

            if ( decorated != null )
            {
                hashCode = decorated.CalculateHashCode ( obj ) ^ CalculateHashCode ( obj );
            }
            else
            {
                hashCode = CalculateHashCode ( obj );
            }

            return hashCode ;
        }
    }
}
