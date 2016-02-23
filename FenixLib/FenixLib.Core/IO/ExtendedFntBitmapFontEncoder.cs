
using System;

namespace FenixLib.IO
{
    public class ExtendedFntBitmapFontEncoder : FntAbstractBitmapFontEncoder
    {
        protected override int GlyphInfoBlockSize { get; } = 28;

        protected override int CodePageTypeForFont ( BitmapFont font )
        {
            if ( font.CodePage == FontCodePage.ISO85591 )
                return 1;
            else if ( font.CodePage == FontCodePage.CP850 )
                return 0;
            else
                throw new ArgumentException ();
        }

        protected override string GetFileMagic ( BitmapFont font ) => "fnx";

        protected override byte GetLastHeaderByte ( BitmapFont font ) => ( byte ) font.Depth;

        protected override void WriteGlyphInfo ( ref NativeFormat.GlyphInfo glypInfo,
            NativeFormatWriter writer )
        {
            writer.WriteExtendedGlyphInfo ( ref glypInfo );
        }
    }
}
