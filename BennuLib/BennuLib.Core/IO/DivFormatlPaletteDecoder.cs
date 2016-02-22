using static Bennu.IO.NativeFormat;

namespace Bennu.IO
{
    public class DivFormatPaletteDecoder : NativeDecoder<Palette>
    {

        public override int MaxSupportedVersion { get; }

        protected override string[] KnownFileExtensions { get; }

        protected override string[] KnownFileMagics { get; }

        protected override Palette ReadBody ( Header header, NativeFormatReader reader )
        {
            // Map files have the Palette data in a different position than the rest of the files
            if ( header.Magic == "map" )
                reader.ReadBytes ( 40 );

            return reader.ReadPalette ();
        }
    }
}
