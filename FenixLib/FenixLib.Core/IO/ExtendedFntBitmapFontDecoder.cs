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
    public sealed class ExtendedFntBitmapFontDecoder : FntAbstractBitmapFontDecoder
    {
        /// <summary>
        /// Extended fonts (fnx) fonts do not have a version byte so this 
        /// information is not used.
        /// </summary>
        public override int MaxSupportedVersion { get; } = 0x00;

        protected override int[] KnownCodePageTypes { get; } = { 0, 1 };

        protected override int[] ValidBitPerPixelDepths { get; } = { 1, 8, 16, 32 };

        protected override string[] KnownFileMagics { get; } = { "fnx" };

        protected override FontCodePage ParseCodePageType ( int codePageType )
        {
            FontCodePage codePage;

            if ( codePageType == 0 )
                codePage = FontCodePage.CP850;
            else if ( codePageType == 1 )
                codePage = FontCodePage.ISO85591;
            else
                throw new ArgumentException (); // TODO: Customize message

            return codePage;
        }

        protected override int ParseBitsPerPixel ( NativeFormat.Header header )
        {
            return header.LastByte;
        }

        protected override NativeFormat.GlyphInfo ReadGlyphInfo (NativeFormatReader reader)
        {
            return reader.ReadExtendedFntGlyphInfo();
        }

        /// <summary>
        /// Validates the version information of the <paramref name="header"/>. Extended fonts
        /// do not store version information and therefore this function always returns True.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="header"></param>
        /// <returns>True</returns>
        protected override bool ValidateHeaderVersion ( int version, 
            NativeFormat.Header header )
        {
            return true;
        }
    }
}
