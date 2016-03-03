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
    public struct Color
    {
        public readonly int r;
        public readonly int g;
        public readonly int b;

        public Color ( int r, int g, int b )
        {
            ValidateComponent ( r );
            ValidateComponent ( g );
            ValidateComponent ( b );

            this.r = r;
            this.g = g;
            this.b = b;
        }

        public bool Equals ( Color color )
        {
            return r == color.r && g == color.g && b == color.b;
        }

        public override bool Equals ( object obj )
        {
            if ( !( obj != null && obj is Color ) )
                return false;

            return Equals ( ( Color ) obj );
        }

        public override int GetHashCode ()
        {
            return r << 16 & g << 8 & b;
        }

        public static bool operator == ( Color colorA, Color colorB )
        {
            if ( ReferenceEquals ( colorA, null ) )
                return ReferenceEquals ( colorB, null );

            return colorA.Equals ( colorB );
        }

        public static bool operator != ( Color colorA, Color colorB )
        {
            return !( colorA == colorB );
        }

        private static void ValidateComponent( int component )
        {
            if ( component < 0 | component > 255 )
            {
                throw new ArgumentOutOfRangeException ("component", component, 
                    "Color component out of allowed range 0..255.");
            }
        }
    }
}
