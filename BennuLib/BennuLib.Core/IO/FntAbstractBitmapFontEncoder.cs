using static Bennu.IO.NativeFormat;

namespace Bennu.IO
{
    public abstract class FntAbstractBitmapFontEncoder : NativeEncoder<BitmapFont>
    {
        protected abstract int GlyphInfoBlockSize { get; }

        protected abstract int CodePageTypeForFont ( BitmapFont font );

        protected abstract void WriteGlyphInfo ( ref GlyphInfo glypInfo, 
            NativeFormatWriter writer );

        protected override void WriteNativeFormatBody ( BitmapFont font, 
            NativeFormatWriter writer )
        {
            if ( font.Depth == 8 )
            {
                writer.Write ( font.Palette );
                writer.WriteReservedPaletteGammaSection ();
            }

            writer.Write ( CodePageTypeForFont ( font ) );

            // The character data section needs to know in advance the start position of the 
            // pixel data in the stream. Since the pixel of each glyph will be written 
            // consecutively, we can calculate the relative position to the beginning of the
            // Pixel data section by considering the width, height (and depth) of the 
            // previous glyph.

            // This information is gathered by the GlyphInfo structure, which will then be
            // used by the WriteGlyphInfo function.

            GlyphInfo[] glyphsInfo = new GlyphInfo[256];

            int pixelsDataOffset = 0; // Relative to the end of the glyph info section
            for ( int i = 0 ; i < glyphsInfo.Length ; i++ )
            {
                Glyph glyph;
                if ( font[i] != null )
                {
                    glyph = font[i];
                    glyphsInfo[i] = new GlyphInfo ( glyph.Width, glyph.Height,
                        glyph.XOffset, glyph.YOffset, glyph.XAdvance, glyph.YAdavance,
                        pixelsDataOffset + GlyphInfoBlockSize + 12); // 12  = 8 bytes header + 4 font info
                }

                pixelsDataOffset += CalculatePixelBufferBytes ( font.Depth, 
                    glyphsInfo[i].Width,
                    glyphsInfo[i].Height );

                // Delegate the actual writing of the glyph to the child classes as it is
                // different for Fnt or Fnx.
                WriteGlyphInfo ( ref glyphsInfo[i], writer );
            }

            // Write pixel data for every glyph
            foreach ( Glyph g in font )
            {
                writer.Write ( g.PixelData );
            }
        }
    }
}
