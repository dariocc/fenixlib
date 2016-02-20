using System;
using System.Linq;
using System.IO;

using static Bennu.IO.NativeFormat;

namespace Bennu.IO
{
    /// <summary>
    /// Bennu supports two types of font formats: Bennu's own format, called 'Fnx' Fonts, and
    /// the legacy DIV Games Studio font format, called 'Fnt'. 
    /// Much of the process of decoding those formats is equivalent. This class 
    /// </summary>
    public abstract class FntAbstractFontDecoder : NativeDecoder<BitmapFont>
    {

        protected override string[] KnownFileExtensions { get; } = { "fnt" };

        protected abstract int[] KnownDepths { get; }

        protected abstract int[] KnownCodePageTypes { get; }

        protected abstract int ParseDepth ( Header header );

        protected abstract FontCodePage ParseCodePageType ( int codePageType );

        protected abstract GlyphInfo ReadGlyphInfo ( NativeFormatReader reader );

        protected override BitmapFont ReadBody ( Header header, NativeFormatReader reader )
        {
            int depth = ParseDepth ( header );

            if ( !KnownDepths.Contains ( depth ) )
            {
                throw new UnsuportedFileFormatException (); // TODO Customize
            }

            Palette palette = null;
            if ( depth == 8 )
            {
                palette = reader.ReadPalette ();
                reader.ReadUnusedPaletteGamma ();
            }

            int codePageType = reader.ReadInt32 ();

            if ( !KnownCodePageTypes.Contains ( codePageType ) )
            {
                throw new UnsuportedFileFormatException (); // TODO Customize
            }

            GlyphInfo[] characters = new GlyphInfo[256];
            for ( var n = 0 ; n <= 255 ; n++ )
            {
                characters[n] = ReadGlyphInfo (reader);
            }

            // Create the font
            BitmapFont font = BitmapFont.Create ( (DepthMode) depth, 
                ParseCodePageType ( codePageType ) );

            Stream pixelsStream = GetSeekablePixelsStream ( reader.BaseStream );

            using ( NativeFormatReader pixelsReader = new NativeFormatReader ( pixelsStream ) )
            {
                // Read Glyph's pixels
                byte characterIndex = 0;
                foreach ( var character in characters )
                {
                    bool characterValid = character.Height <= 0
                        || character.Width <= 0
                        || character.FileOffset <= 0;

                    if ( !characterValid )
                        continue;

                    pixelsStream.Seek ( character.FileOffset, SeekOrigin.Begin );

                    IPixel[] pixels = pixelsReader.ReadPixels ( depth, character.Width,
                        character.Height );

                    Glyph glyph = Glyph.Create ( character.Width, character.Height, pixels );
                    glyph.XAdvance = character.XAdvance;
                    glyph.YAdavance = character.YAdvance;
                    glyph.XOffset = character.XOffset;
                    glyph.YOffset = character.YOffset;

                    font[characterIndex] = glyph;

                    characterIndex += 1;
                }
            }

            return font;
        }

        private static Stream GetSeekablePixelsStream ( Stream stream )
        {
            if ( stream.CanSeek )
                return stream;

            // Estimation: Glyhps of 64x64, 32 bpp and 256 characters
            int InitialMemorySize = 64 * 64 * 4 * 256;

            MemoryStream memory = new MemoryStream ( InitialMemorySize );

            // Offset the position where the data will be buffered
            // to match the position of the original stream.
            memory.Seek ( stream.Position, SeekOrigin.Begin );

            stream.CopyTo ( memory );
            memory.Flush ();

            return memory;
        }
    }

    /*
    class FnxFntBitmapFontDecoder : NativeDecoder<BitmapFont>
    {
        public override int MaxSupportedVersion { get; } = 0x00;

        private int[] KnownDepths { get; } = { 1, 8, 16, 32 };

        private int[] KnownCharsets { get; } = { 0, 1 };

        protected override string[] KnownFileExtensions { get; } = { "fnt" };

        protected override string[] KnownFileMagics { get; } = { "fnx" };

        protected override bool ValidateHeaderVersion ( int version, Header header )
        {
            // FNX format uses the version byte to tell about the depth of the font. There is
            // no version number in the header and therefore this method shall always return true
            return true;
        }
    }*/

}
