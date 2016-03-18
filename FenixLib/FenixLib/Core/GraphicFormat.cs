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
    public sealed class GraphicFormat
    {
        private GraphicFormat ( int value, string name )
        {
            BitsPerPixel = value;
            this.name = name;
        }

        public int BitsPerPixel { get; }
        private string name;

        public int PixelsBytesForSize ( int width, int height )
        {
            int byteLength;

            if ( BitsPerPixel == 1 )
            {
                int rowByteSize = ( width + ( ( ( 8 - ( width % 8 ) ) & 7 ) ) ) / 8;
                byteLength = rowByteSize * height;
            }
            else
            {
                byteLength = width * height * BitsPerPixel / 8;
            }

            return byteLength;
        }

        public int StrideForWidth (int width)
        {
            return PixelsBytesForSize ( width, 1 );
        }

        public static explicit operator GraphicFormat ( int value )
        {
            switch ( value )
            {
                case 1:
                    return Format1bppMonochrome;
                case 8:
                    return Format8bppIndexed;
                case 16:
                    return Format16bppRgb565;
                case 32:
                    return Format32bppArgb;
            }

            throw new ArgumentException ();
        }

        public static explicit operator int ( GraphicFormat graphicFormat )
        {
            return graphicFormat.BitsPerPixel;
        }

        public override string ToString ()
        {
            return name;
        }

        public static GraphicFormat Format1bppMonochrome = new GraphicFormat ( 1, nameof ( Format1bppMonochrome ) );
        public static GraphicFormat Format8bppIndexed = new GraphicFormat ( 8, nameof ( Format8bppIndexed ) );
        public static GraphicFormat Format16bppRgb565 = new GraphicFormat ( 16, nameof ( Format16bppRgb565 ) );
        public static GraphicFormat Format32bppArgb = new GraphicFormat ( 32, nameof ( Format32bppArgb ) );
    }
}
