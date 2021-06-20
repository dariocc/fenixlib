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
using System.IO;
using FenixLib.Core;

namespace FenixLib.IO
{
    /// <summary>
    /// Provides static methods for the opening and creation of native file
    /// formats.
    /// </summary>
	public static class NativeFile
    {

        /// <summary>
        /// Opens a Map file, reads all the information into a <see cref="Sprite"/> 
        /// object, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <returns>An instance of <see cref="Sprite"/> created from the file.</returns>
		public static ISprite LoadMap ( string path )
        {
            var decoder = new MapSpriteDecoder ();

            using ( var stream = File.Open ( path, FileMode.Open ) )
            {
                return decoder.Decode ( stream );
            }
        }

        /// <summary>
        /// Creates a new Map file, writes the information of a <see cref="Sprite"/>,
        /// and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="sprite">The <see cref="Sprite"/> to write to the file.</param>
        /// <param name="path">The file to write to.</param>
		public static void SaveToMap ( this ISprite sprite, string path )
        {
            var encoder = new MapSpriteEncoder ();
            using ( var output = File.Open ( path, FileMode.Create ) )
            {
                encoder.Encode ( sprite, output );
            }
        }

        /// <summary>
        /// Opens a Fpg file, reads all the information into a <see cref="SpriteAssortment"/> 
        /// object, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <returns>An instance of <see cref="SpriteAssortment"/> created from the file.</returns>
        public static ISpriteAssortment LoadFpg ( string path )
        {
            var decoder = new FpgSpriteAssortmentDecoder ();

            using ( var stream = File.Open ( path, FileMode.Open ) )
            {
                return decoder.Decode ( stream );
            }
        }

        /// <summary>
        /// Creates a new Fpg file, writes the information of a <see cref="ISpriteAssortment"/>,
        /// and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="assortment">The <see cref="SpriteAssortment"/> to write to the file.</param>
        /// <param name="path">The file to write to.</param>
        public static void SaveToFpg ( this ISpriteAssortment assortment, string path )
        {
            var encoder = new FpgSpriteAssortmentEncoder ();
            using ( var output = File.Open ( path, FileMode.Create ) )
            {
                encoder.Encode ( assortment, output );
            }
        }


        public static IBitmapFont LoadFnt ( string path )
        {
            // DivFont decoder is used by default
			var divFontDecoder = new DivFntBitmapFontDecoder ();

            using ( var stream = File.Open ( path, FileMode.Open ) )
            {
                IBitmapFont font;
                if ( divFontDecoder.TryDecode ( stream, out font ) )
                {
					return font;
                }
            }

			// Fallback to the ExtendedFontBitmapFontDecoder
			var extendedFontDecoder = new ExtendedFntBitmapFontDecoder ();

			using ( var stream = File.Open ( path, FileMode.Open ) )
			{
				return extendedFontDecoder.Decode ( stream );
			}
        }

        /// <summary>
        /// Creates a new Fnt file, writes the information of a <see cref="IBitmapFont"/>,
        /// and then closes the file. If the target file already exists, it is overwritten.
        /// Bitmap fonts are encoded by default using a
        /// <see cref="ExtendedFntBitmapFontEncoder"/>, except except when the font's 
        /// <see cref="BitmapFont.Encoding"/> property is set to 
        /// <seealso cref="FontEncoding.CP850"/> and the <see cref="GraphicFormat"/> 
        /// is <see cref="GraphicFormat.Format8bppIndexed" />, in which case the encoder
        /// used will be <see cref="DivFntBitmapFontEncoder"/>. It is possible to force the
        /// use of <see cref="ExtendedFntBitmapFontEncoder"/> by defining the 
        /// <paramref name="forceExtendedFontEncoding"/> parameter as true.
        /// </summary>
        /// <param name="font"></param>
        /// <param name="path"></param>
        /// <param name="forceExtendedFontEncoding"></param>
        public static void SaveToFnt ( this IBitmapFont font, string path, 
            bool forceExtendedFontEncoding = false )
        {
            FntBitmapFontEncoder encoder;

            if (font.GraphicFormat == GraphicFormat.Format8bppIndexed 
                && font.Encoding == FontEncoding.CP850
                && !forceExtendedFontEncoding)
            {
                encoder = new DivFntBitmapFontEncoder ();
            }
            else
            {
                encoder = new ExtendedFntBitmapFontEncoder ();
            }

            using ( var output = File.Open ( path, FileMode.Create ) )
            {
                encoder.Encode ( font, output );
            }
        }

        /// <summary>
        /// Opens a 8bpp Pal, Map, Fpg or Fnt file, reads all the information into 
        /// a <see cref="Palette"/> object, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <returns>An instance of <see cref="Palette"/> created from the file.</returns>
        public static Palette LoadPal ( string path )
        {
            var decoder = new DivFilePaletteDecoder ();

            using ( var stream = File.Open ( path, FileMode.Open ) )
            {
                return decoder.Decode ( stream );
            }
        }

        /// <summary>
        /// Creates a new Pal file, writes the information of a <see cref="Palette"/>,
        /// and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="palette">The <see cref="Palette"/> to write to the file.</param>
        /// <param name="path">The file to write to.</param>
        public static void SaveToPal ( this Palette palette, string path )
        {
            var encoder = new PalPaletteEncoder ();

            using ( var stream = File.Open ( path, FileMode.Open ) )
            {
                encoder.Encode ( palette, stream );
            }
        }
    }
}
