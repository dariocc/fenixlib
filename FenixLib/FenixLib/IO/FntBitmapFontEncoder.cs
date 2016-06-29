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
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    public abstract class FntBitmapFontEncoder : NativeEncoder<IBitmapFont>
    {
        protected abstract int GlyphInfoBlockSize { get; }

        protected abstract int CodePageTypeForFont ( IBitmapFont font );

        protected abstract void WriteGlyphInfo ( ref GlyphInfo glypInfo,
            NativeFormatWriter writer );

        protected override void WriteNativeFormatBody ( IBitmapFont font,
            NativeFormatWriter writer )
        {
            if ( ( int ) font.GraphicFormat == 8 )
            {
                writer.Write ( font.Palette );
                writer.WritePaletteGammaSection ();
            }

            writer.Write ( CodePageTypeForFont ( font ) );

            // The character data section needs to know in advance the start position of the 
            // pixel data in the stream. Since the pixel of each glyph will be written 
            // consecutively, we can calculate the relative position to the beginning of the
            // Pixel data section by considering the width, height (and bpp) of the 
            // previous glyph.

            // This information is gathered by the GlyphInfo structure, which will then be
            // used by the WriteGlyphInfo function.

            GlyphInfo[] glyphsInfo = new GlyphInfo[256];

            int pixelsDataOffset = 0; // Relative to the end of the glyph info section
            for ( int i = 0 ; i < glyphsInfo.Length ; i++ )
            {
                IGlyph glyph;
                if ( font[i] != null )
                {
                    glyph = font[i];
                    glyphsInfo[i] = new ExtendedGlyphInfoBuilder ()
                        .Width ( glyph.Width )
                        .Height ( glyph.Height )
                        .XOffset ( glyph.XOffset )
                        .YOffset ( glyph.YOffset )
                        .XAdvance ( glyph.XAdvance )
                        .YAdvance ( glyph.YAdavance )
                        .FileOffset ( pixelsDataOffset + GlyphInfoBlockSize + 12 ) // 8 header + 4 font info
                        .Build (); 
                }

                pixelsDataOffset += font.GraphicFormat.PixelsBytesForSize (
                    glyphsInfo[i].Width,
                    glyphsInfo[i].Height );

                // Delegate the actual writing of the glyph to the child classes as it is
                // different for Fnt or Fnx.
                WriteGlyphInfo ( ref glyphsInfo[i], writer );
            }

            // Write pixel data for every glyph
            foreach ( IGlyph g in font )
            {
                writer.Write ( g.PixelData );
            }
        }
    }
}
