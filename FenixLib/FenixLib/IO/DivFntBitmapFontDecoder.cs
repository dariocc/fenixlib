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

namespace FenixLib.IO
{
    public sealed class DivFntBitmapFontDecoder : FntBitmapFontDecoder
    {

        public override int MaxSupportedVersion { get; } = 0x00;

        protected override int[] ValidBitPerPixelDepths { get; } = { 8 };

        protected override string[] KnownFileMagics { get; } = { "fnt" };

        protected override FontEncoding Encoding
        {
            get
            {
                return FontEncoding.CP850;
            }
        }

        protected override void ProcessFontInfoField ( int fontInfoField )
        {
            // The field determines the group of characters present in the
            // font
            // +1 Numbers
            // +2 Upper Case
            // +4 Lower Case
            // +8 Simbols
            // +16 Extended

            // This information is however not necessary to read the font.

            // Only above values or BitField combinations are valid
            if ( fontInfoField <= 0 || fontInfoField > 0xF )
            {
                throw new ArgumentException (); // TODO: Customize message
            }
        }

        protected override int ParseBitsPerPixel ( NativeFormat.Header header )
        {
            return 8;
        }

        protected override GlyphInfo ReadGlyphInfo ( NativeFormatReader reader )
        {
            return reader.ReadLegacyFntGlyphInfo ();
        }

		protected override int GetPixelDataStart ( int bpp ) => 5452;
    }
}
