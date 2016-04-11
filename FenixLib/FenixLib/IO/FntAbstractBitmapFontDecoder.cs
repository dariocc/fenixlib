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
using System.Linq;
using System.IO;

using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    /// <summary>
    /// Bennu supports two types of font formats: Bennu's own format, called 'Fnx' Fonts, and
    /// the legacy DIV Games Studio font format, called 'Fnt'. 
    /// Much of the process of decoding those formats is equivalent. This class 
    /// </summary>
    public abstract class FntAbstractBitmapFontDecoder : NativeDecoder<IBitmapFont>
    {

        protected override string[] KnownFileExtensions { get; } = { "fnt" };

        protected abstract int[] ValidBitPerPixelDepths { get; }

        //protected abstract int[] KnownFontFlags { get; }

        protected abstract int ParseBitsPerPixel ( Header header );

        protected abstract void ProcessFontInfoField ( int codePageType );

        protected abstract FontEncoding Encoding { get; }

        protected abstract GlyphInfo ReadGlyphInfo ( NativeFormatReader reader );

        protected override IBitmapFont ReadBody ( Header header, NativeFormatReader reader )
        {
            int bpp = ParseBitsPerPixel ( header );

            if ( !ValidBitPerPixelDepths.Contains ( bpp ) )
            {
                throw new UnsuportedFileFormatException (); // TODO Customize
            }

            Palette palette = null;
            if ( bpp == 8 )
            {
                palette = reader.ReadPalette ();
                reader.ReadPaletteGamma ();
            }

            int fontInfoField = reader.ReadInt32 ();

            ProcessFontInfoField ( fontInfoField );

            GlyphInfo[] characters = new GlyphInfo[256];
            for ( var n = 0 ; n <= 255 ; n++ )
            {
                characters[n] = ReadGlyphInfo (reader);
            }

            // Create the font
            BitmapFont font = new BitmapFont ( Encoding, ( GraphicFormat) bpp, palette );

            Stream pixelsStream = GetSeekablePixelsStream ( reader.BaseStream );

            using ( var pixelsReader = new NativeFormatReader ( pixelsStream ) )
            {
                // Read Glyph's pixels
                int characterIndex = -1;
                foreach ( var character in characters )
                {
                    characterIndex += 1;
                    bool characterInvalid = character.Height <= 0
                        || character.Width <= 0
                        || character.FileOffset <= 0;

                    if ( characterInvalid )
                        continue;

                    pixelsStream.Seek ( character.FileOffset, SeekOrigin.Begin );

                    byte[] pixels = pixelsReader.ReadPixels ( bpp, character.Width,
                        character.Height );

                    IGraphic graphic = new Graphic ( ( GraphicFormat ) bpp, character.Width,
                        character.Height, pixels, palette );
                    IGlyph glyph = new Glyph ( graphic );
                    glyph.XAdvance = character.XAdvance;
                    glyph.YAdavance = character.YAdvance;
                    glyph.XOffset = character.XOffset;
                    glyph.YOffset = character.YOffset;

                    font[characterIndex] = glyph;
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

}
