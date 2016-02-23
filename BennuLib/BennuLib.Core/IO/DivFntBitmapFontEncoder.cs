using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bennu.IO
{
    class DivFntBitmapFontEncoder : FntAbstractFontEncoder
    {
        private const byte Version = 0x00;

        protected override int GlyphInfoBlockSize { get; } = 16;

        protected override int CodePageTypeForFont ( BitmapFont font )
        {
            if ( font.CodePage != FontCodePage.CP850 )
                throw new ArgumentException (); // TODO: Customize

            return 0;
        }

        protected override string GetFileMagic ( BitmapFont font ) => "fnt";

        protected override byte GetLastHeaderByte ( BitmapFont font ) => Version;

        protected override void WriteGlyphInfo ( ref NativeFormat.GlyphInfo glypInfo,
            NativeFormatWriter writer )
        {
            writer.WriteLegacyFntGlyphInfo ( ref glypInfo );
        }
    }
}
