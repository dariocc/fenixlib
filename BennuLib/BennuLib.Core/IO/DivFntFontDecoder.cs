using System;

namespace BennuLib.IO
{
    public sealed class DivFntFontDecoder : FntAbstractFontDecoder
    {

        public override int MaxSupportedVersion { get; } = 0x00;

        protected override int[] KnownCodePageTypes { get; } = { 0 };

        protected override int[] KnownDepths { get; } = { 8 };

        protected override string[] KnownFileMagics { get; } = { "fnt" };

        protected override FontCodePage ParseCodePageType ( int codePageType )
        {
            FontCodePage codePage;

            if ( codePageType == 0 )
                codePage = FontCodePage.CP850;
            else
                throw new ArgumentException (); // TODO: Customize message

            return codePage;
        }

        protected override int ParseDepth ( NativeFormat.Header header )
        {
            return 8;
        }

        protected override NativeFormat.GlyphInfo ReadGlyphInfo ( NativeFormatReader reader )
        {
            return reader.ReadLegacyFntGlyphInfo ();
        }
    }
}
