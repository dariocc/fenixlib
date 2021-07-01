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
using FenixLib.Core;

namespace FenixLib.IO
{
    public class DivFntBitmapFontEncoder : FntBitmapFontEncoder
    {
        private const byte Version = 0x00;

        protected override int GlyphInfoBlockSize { get; } = 16;

        protected override int CodePageTypeForFont ( IBitmapFont font )
        {
            if ( font.Encoding != FontEncoding.CP850 )
            { 
                throw new ArgumentException (); // TODO: Customize
            }

            return 0;
        }

        protected override void WriteNativeFormatHeader ( IBitmapFont font, 
            NativeFormatWriter writer )
        {
            if ( font.GraphicFormat.BitsPerPixel != 8 )
            { 
                throw new ArgumentException (); // Customize
            }

            base.WriteNativeFormatHeader ( font, writer );
        }

        protected override string GetFileMagic ( IBitmapFont font ) => "fnt";

        protected override byte GetLastHeaderByte ( IBitmapFont font ) => Version;

        protected override void WriteGlyphInfo ( ref GlyphInfo glypInfo,
            NativeFormatWriter writer )
        {
            writer.WriteLegacyGlyphInfo ( ref glypInfo );
        }
    }
}
