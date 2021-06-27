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
using FenixLib.Core;

namespace FenixLib.Tests.Integration.Comparison
{
    internal abstract class AbstractGraphicComparer<E> : IGraphicEqualityComparer<E>
        where E : IGraphic
    {
        private IGraphicEqualityComparer<E> decorated;

        public AbstractGraphicComparer ( IGraphicEqualityComparer<E> comparer = null )
        {
            decorated = comparer;
        }

        public abstract int CalculateHashCode ( E x );
        public abstract bool CompareCore ( E x, E y );

        public bool Equals ( E x, E y )
        {
            bool result = true;

            if ( decorated != null )
            {
                result = decorated.Equals ( x, y );
            }

            return result & CompareCore ( x, y );
        }

        public int GetHashCode ( E obj )
        {
            int hashCode = CalculateHashCode ( obj );

            if ( decorated != null )
            {
                hashCode = decorated.GetHashCode ( obj ) ^ CalculateHashCode ( obj );
            }
            else
            {
                hashCode = CalculateHashCode ( obj );
            }

            return hashCode;
        }
    }
}
