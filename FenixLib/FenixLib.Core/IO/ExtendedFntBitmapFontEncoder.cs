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
using FenixLib.Core;

namespace FenixLib.IO
{
    public class ExtendedFntBitmapFontEncoder : FntAbstractBitmapFontEncoder
    {
        protected override int GlyphInfoBlockSize { get; } = 28;

        protected override int CodePageTypeForFont ( BitmapFont font )
        {
            if ( font.CodePage == FontEncoding.ISO85591 )
                return 1;
            else if ( font.CodePage == FontEncoding.CP850 )
                return 0;
            else
                throw new ArgumentException ();
        }

        protected override string GetFileMagic ( BitmapFont font ) => "fnx";

        protected override byte GetLastHeaderByte ( BitmapFont font ) => ( byte ) font.GraphicFormat;

        protected override void WriteGlyphInfo ( ref NativeFormat.GlyphInfo glypInfo,
            NativeFormatWriter writer )
        {
            writer.WriteExtendedGlyphInfo ( ref glypInfo );
        }
    }
}
