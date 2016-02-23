using System;

using static Bennu.IO.NativeFormat;

namespace Bennu.IO
{
    public abstract class FntAbstractFontEncoder : NativeEncoder<BitmapFont>
    {
        protected abstract int CodePageTypeForFont ( BitmapFont font );

        protected abstract void WriteGlyphInfo ( ref GlyphInfo glypInfo, NativeFormatWriter writer );

        protected override void WriteNativeFormatBody ( BitmapFont font, NativeFormatWriter writer )
        {
            if ( font.Depth == 8 )
            {
                writer.Write ( font.Palette );
                writer.WriteReservedPaletteGammaSection ();
            }

            writer.Write ( CodePageTypeForFont ( font ) );

            GlyphInfo[] glyphsInfo = new GlyphInfo[256];
            int dataOffset = 0; // Relative to the end of the glyp info section
            for (int i = 0 ; i < glyphsInfo.Length ; i++ )
            {
                // TODO: Will fail if the character is not defined, in which case
                // an empty glyph info shall be created
                // TODO: Or might return null if this[] is configured as "element or default"
                Glyph glyph;
                try
                {
                    glyph = font[i];
                    glyphsInfo[i] = new GlyphInfo ( glyph.Width, glyph.Height,
                        glyph.XOffset, glyph.YOffset, glyph.XAdvance, glyph.YAdavance, dataOffset );
                } catch (Exception e)
                {
                    // Ignore missing characters
                }

                dataOffset += CalculatePixelBufferBytes(font.Depth, glyphsInfo[i].Width, 
                    glyphsInfo[i].Height);

                // Delegate the actual writing of the glyph to the child classes
                WriteGlyphInfo ( ref glyphsInfo[i], writer );
            }

            // Write Pixel data for every glyph
            foreach (Glyph g in font )
            {
                writer.Write ( g.PixelData );
            }
        }
    }
}
