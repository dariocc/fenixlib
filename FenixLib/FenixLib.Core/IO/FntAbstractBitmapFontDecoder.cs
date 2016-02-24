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
using System;
using System.Linq;
using System.IO;

using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    /// <summary>
    /// Bennu supports two types of font formats: Bennu's own format, called 'Fnx' Fonts, and
    /// the legacy DIV Games Studio font format, called 'Fnt'. 
    /// Much of the process of decoding those formats is equivalent. This class 
    /// </summary>
    public abstract class FntAbstractBitmapFontDecoder : NativeDecoder<BitmapFont>
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

            using ( var pixelsReader = new NativeFormatReader ( pixelsStream ) )
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

                    byte[] pixels = pixelsReader.ReadPixels ( depth, character.Width,
                        character.Height );

                    Glyph glyph = Glyph.Create ( (DepthMode) depth, character.Width, 
                        character.Height, pixels );
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

}
