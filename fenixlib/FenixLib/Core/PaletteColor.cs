/**
 *    Copyright (c) 2016 Darío Cutillas Carrillo
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 * 
 *        http://www.apache.org/licenses/LICENSE-2.0
 * 
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
using System;

namespace FenixLib
{
    public struct PaletteColor
    {
        // DIV / Bennu palettes are VGA 6-bit RGB. Stored as 8-bit values
        // via << 2, so each channel lives in {0, 4, ..., 252}.
        private const int BitsPerComponent = 6;
        private const int ComponentShift = 8 - BitsPerComponent;

        public int R { get; }
        public int G { get; }
        public int B { get; }

        public PaletteColor ( int r, int g, int b )
        {
            R = ToVgaComponent ( r );
            G = ToVgaComponent ( g );
            B = ToVgaComponent ( b );
        }

        public bool Equals ( PaletteColor color )
        {
            return R == color.R && G == color.G && B == color.B;
        }

        public override bool Equals ( object obj )
        {
            if ( !( obj != null && obj is PaletteColor ) )
                return false;

            return Equals ( ( PaletteColor ) obj );
        }

        public override int GetHashCode ()
        {
            return R << 16 & G << 8 & B;
        }

        public static bool operator == ( PaletteColor colorA, PaletteColor colorB )
        {
            if ( ReferenceEquals ( colorA, null ) )
                return ReferenceEquals ( colorB, null );

            return colorA.Equals ( colorB );
        }

        public static bool operator != ( PaletteColor colorA, PaletteColor colorB )
        {
            return !( colorA == colorB );
        }

        private static int ToVgaComponent ( int component )
        {
            if ( component < 0 | component > 255 )
            {
                throw new ArgumentOutOfRangeException ( "component", component,
                    "Color component out of allowed range 0..255." );
            }

            return ( component >> ComponentShift ) << ComponentShift;
        }
    }
}
