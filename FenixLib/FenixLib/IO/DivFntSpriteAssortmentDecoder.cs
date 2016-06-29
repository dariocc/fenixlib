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
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    public class DivFntSpriteAssortmentDecoder : NativeDecoder<ISpriteAssortment>
    {

        public override int MaxSupportedVersion { get; }

        protected override string[] KnownFileExtensions { get; }

        protected override string[] KnownFileMagics { get; }

        protected override ISpriteAssortment ReadBody ( Header header, NativeFormatReader reader )
        {

            Palette palette = reader.ReadPalette ();
            reader.ReadPaletteGammas ();

            int fontInfo = reader.ReadInt32 ();

            GlyphInfo[] characters = new GlyphInfo[256];
            for ( var n = 0 ; n <= 255 ; n++ )
            {
                characters[n] = reader.ReadLegacyFntGlyphInfo ();
            }

            var bpp = header.ParseBitsPerPixelFromMagic ();
            SpriteAssortment fpg = new SpriteAssortment ( palette );

            foreach ( var character in characters )
            {
                var dataLength = character.Height * character.Width;

                if ( character.FileOffset == 0 | dataLength == 0 )
                    continue;

                var pixels = reader.ReadPixels ( (GraphicFormat) bpp, character.Width, 
                    character.Height );

                IGraphic graphic = new Graphic ( ( GraphicFormat ) bpp, 
                    character.Width, character.Height, pixels );

                ISprite sprite = new Sprite ( graphic );

                fpg.Add ( fpg.GetFreeId (), sprite );
            }

            return fpg;
        }
    }
}
