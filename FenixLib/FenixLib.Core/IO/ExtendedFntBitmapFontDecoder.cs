using System;

namespace FenixLib.IO
{
    public sealed class ExtendedFntBitmapFontDecoder : FntAbstractBitmapFontDecoder
    {
        /// <summary>
        /// Extended fonts (fnx) fonts do not have a version byte so this information is not used.
        /// </summary>
        public override int MaxSupportedVersion { get; } = 0x00;

        protected override int[] KnownCodePageTypes { get; } = { 0, 1 };

        protected override int[] KnownDepths { get; } = { 1, 8, 16, 32 };

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

        protected override int ParseDepth ( NativeFormat.Header header )
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
        protected override bool ValidateHeaderVersion ( int version, NativeFormat.Header header )
        {
            return true;
        }
    }
}
