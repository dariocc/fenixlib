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

namespace FenixLib.Imaging.Color
{
    internal class RgbColor
    {
        private int? cachedHashCode = null;

        public double R { get; }
        public double G { get; }
        public double B { get; }

        public RgbColor ( double r, double g, double b )
        {
            if ( r < 0 | r > 1 )
                throw new ArgumentOutOfRangeException ();

            if ( g < 0 | g > 1 )
                throw new ArgumentOutOfRangeException ();

            if ( b < 0 | b > 1 )
                throw new ArgumentOutOfRangeException ();

            R = r;
            G = g;
            B = b;
        }
        public RgbColor ( int r, int g, int b ) : this(r / 255.0, g / 255.0, b/ 255.0 ) {}

        public bool Equals ( RgbColor color )
        {
            return R == color.R && G == color.G && B == color.B;
        }

        public override bool Equals ( object obj )
        {
            if ( !( obj != null && obj is RgbColor ) )
                return false;

            return Equals ( ( RgbColor ) obj );
        }

        public override int GetHashCode ()
        {
            if ( cachedHashCode != null )
                return cachedHashCode.Value;

            int hash = 17;

            hash = hash * 23 + R.GetHashCode ();
            hash = hash * 23 + G.GetHashCode ();
            hash = hash * 23 + B.GetHashCode ();

            cachedHashCode = hash;

            return hash;
        }

        public static bool operator == ( RgbColor colorA, RgbColor colorB )
        {
            if ( ReferenceEquals ( colorA, null ) )
                return ReferenceEquals ( colorB, null );

            return colorA.Equals ( colorB );
        }

        public static bool operator != ( RgbColor colorA, RgbColor colorB )
        {
            return !( colorA == colorB );
        }
    }
}
