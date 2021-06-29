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
using FenixLib.Core;
using FenixLib.Imaging.Color;

namespace FenixLib.Imaging
{
    internal class PixelWriterRgbIndexed : PixelWriter
    {
        private HslColor[] _hslPalette;
        private IDictionary<RgbColor, int> ColorCache = new Dictionary<RgbColor, int> ();

        public PixelWriterRgbIndexed ( Palette destPalette )
        {
            _hslPalette = PrepareHslPalette ( destPalette );
        }

        public override void Write ( int alpha, int r, int g, int b )
        {
            int paletteIndex;

            if ( alpha < 128 )
            {
                paletteIndex = 0;
            }
            else
            {
                paletteIndex = FindNearestInPalette ( r, g, b );
            }

            Writer.Write ( ( byte ) paletteIndex );
        }

        private int FindNearestInPalette ( int r, int g, int b )
        {
            RgbColor c = new RgbColor ( r, g, b );

            if ( ColorCache.ContainsKey ( c ) )
            {
                return ColorCache[c];
            }

            HslColor hslColor = HslColor.FromRgb ( c );

            HslWeightFactor weights;

            if ( hslColor.L > 0.8 | hslColor.L < 0.2 )
            {
                weights = new HslWeightFactor ( 0.1, 0.2, 0.7 );
            }
            else
            {
                weights = new HslWeightFactor ( 0.4, 0.4, 0.2 );
            }

            int index = -1;
            double minDistance = 360 * 360;
            int minDistanceIndex = 0;
            foreach ( HslColor paletteColor in _hslPalette )
            {
                index += 1;
                double distance = SquaredDistance ( hslColor, paletteColor, weights );
                if ( distance < minDistance )
                {
                    minDistance = distance;
                    minDistanceIndex = index;
                }
            }

            ColorCache[c] = minDistanceIndex;

            return minDistanceIndex;
        }

        private double SquaredDistance ( HslColor color1, HslColor color2, HslWeightFactor weights )
        {
            double hdiff = Math.Abs ( color1.H - color2.H ) / 360;
            double sdiff = Math.Abs ( color1.S - color2.S );
            double ldiff = Math.Abs ( color1.L - color2.L );

            return hdiff * hdiff * weights.H
                + sdiff * sdiff * weights.S
                + ldiff * ldiff * weights.L;
        }

        private static HslColor[] PrepareHslPalette ( Palette palette )
        {
            HslColor[] hslColors = new HslColor[palette.Colors.Length];

            for ( int i = 0 ; i < palette.Colors.Length ; i++ )
            {
                Core.PaletteColor paletteColor = palette[i];
                RgbColor c = new RgbColor( paletteColor.R, paletteColor.G, paletteColor.B );

                hslColors[i] = HslColor.FromRgb ( c );
            }

            return hslColors;
        }


        private struct HslWeightFactor
        {
            public double H { get; }
            public double S { get; }
            public double L { get; }

            public HslWeightFactor ( double wh, double ws, double wl )
            {
                H = wh;
                S = ws;
                L = wl;
            }
        }
    }

}
