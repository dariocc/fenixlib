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

namespace FenixLib
{
    [Serializable ()]
    public class Glyph
    {
        public int Width { get; }
        public int Height { get; }
        public Palette Palette { get; } = null;
        public int YOffset { get; set; } = 0;
        public int XOffset { get; set; } = 0;
        public int XAdvance { get; set; } = 0;
        public int YAdavance { get; set; } = 0;
        public byte[] PixelData { get; }
        public int Depth { get; }

        private Glyph ( int width, int height, int depth, byte[] pixelData, 
            Palette palette = null )
        {
            Width = width;
            Height = height;
            Palette = palette;
            Depth = depth;
            PixelData = pixelData;
        }

        public static Glyph Create (DepthMode depth, int width, int height, byte[] pixelData, 
            Palette palette = null )
        {
            if ( width <= 0 || height <= 0 )
                throw new ArgumentOutOfRangeException (); // TODO: Customize

            // TODO: Validate the size of pixelData array
            if ( ( depth == DepthMode.RgbIndexedPalette ) != ( palette != null ) )
                throw new ArgumentException (); // TODO: Customize

            return new Glyph ( width, height, (int) depth, pixelData, palette );
        }
    }
}
