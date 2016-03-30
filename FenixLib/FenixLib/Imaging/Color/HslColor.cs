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
    public struct HslColor
    {
        public int H { get; }
        public double S { get; }
        public double L { get; }

        public HslColor ( int hue, double saturation, double luminosity )
        {
            if ( hue >= 360 || hue <= 0 )
                throw new ArgumentOutOfRangeException ( 
                    "Hue must bee between 0 and 360" );

            if ( saturation < 0 || saturation > 1 )
                throw new ArgumentOutOfRangeException (
                    "Saturation must be between 0 and 1" );

            if ( luminosity < 0 || luminosity > 1 )
                throw new ArgumentOutOfRangeException (
                    "Luminosity must be between 0 and 1" );

            H = hue;
            S = saturation;
            L = luminosity;
        }

        public static HslColor FromRgb ( RgbColor c )
        {
            return FromRgb ( c.R / 255.0, c.G / 255.0, c.B / 255.0 );
        }

        // This method is private to obligue consumers to use RgbColor constructor,
        // which will validate the RgbColor components
        private static HslColor FromRgb ( double r, double g, double b )
        {
            double max = Math.Max ( Math.Max ( r, g ), b );
            double min = Math.Min ( Math.Min ( r, g ), b );

            int h;
            if ( max == min )
            {
                h = 0;
            }
            else if ( max == r )
            {
                h = ( int ) ( ( 60 * ( g - b ) / ( max - min ) + 360 ) ) % 360;
            }
            else if ( max == g )
            {
                h = ( int ) ( 60 * ( b - r ) / ( max - min ) + 120 );
            }
            else
            {
                h = ( int ) ( 60 * ( r - g ) / ( max - min ) + 240 );
            }

            double l = 0.5 * ( max - min );

            double s;
            if ( max == min )
            {
                s = 0;
            }
            else if ( l <= 0.5 )
            {
                s = ( max - min ) / ( 2 * l );
            }
            else
            {
                s = ( max - min ) / ( 2 - 2L );
            }

            return new HslColor ( h, s, l );
        }
    }
}
