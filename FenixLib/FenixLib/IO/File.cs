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
    /// Provides static methods for the opening and creation of Bennu native file
    /// format.
    /// </summary>
	public static class File
    {

        /// <summary>
        /// Opens a Map file, reads all the information into a <see cref="Sprite"/> 
        /// object, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <returns>An instance of <see cref="Sprite"/> created from the file.</returns>
		public static ISprite LoadMap ( string path )
        {
            MapSpriteDecoder decoder = new MapSpriteDecoder ();

            using ( var stream = System.IO.File.Open ( path, FileMode.Open ) )
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
		public static void SaveMap ( Sprite sprite, string path )
        {
            MapSpriteEncoder encoder = new MapSpriteEncoder ();
            using ( var output = System.IO.File.Open ( path, FileMode.Create ) )
            {
                encoder.Encode ( sprite, output );
            }
        }

        /// <summary>
        /// Opens a Fpg file, reads all the information into a <see cref="SpriteAsset"/> 
        /// object, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <returns>An instance of <see cref="SpriteAsset"/> created from the file.</returns>
        public static ISpriteAsset LoadFpg ( string path )
        {
            FpgSpriteAssetDecoder decoder = new FpgSpriteAssetDecoder ();

            using ( var stream = System.IO.File.Open ( path, FileMode.Open ) )
            {
                return decoder.Decode ( stream );
            }
        }

        /// <summary>
        /// Creates a new Fpg file, writes the information of a <see cref="SpriteAsset"/>,
        /// and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="asset">The <see cref="SpriteAsset"/> to write to the file.</param>
        /// <param name="path">The file to write to.</param>
        public static void SaveFpg ( ISpriteAsset asset, string path )
        {
            FpgSpriteAssetEncoder encoder = new FpgSpriteAssetEncoder ();
            using ( var output = System.IO.File.Open ( path, FileMode.Create ) )
            {
                encoder.Encode ( asset, output );
            }
        }

        public static IBitmapFont LoadFnt ( string path )
        {
            FntAbstractBitmapFontDecoder decoder = new DivFntBitmapFontDecoder ();

            using ( var stream = System.IO.File.Open ( path, FileMode.Open ) )
            {
                return decoder.Decode ( stream );
            }
        } 

        public static void SaveFnt ( BitmapFont font, string path )
        {

        }

        /// <summary>
        /// Opens a 8bpp Pal, Map, Fpg or Fnt file, reads all the information into 
        /// a <see cref="Palette"/> object, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open.</param>
        /// <returns>An instance of <see cref="Palette"/> created from the file.</returns>
        public static Palette LoadPal ( string path )
        {
            DivFilePaletteDecoder decoder = new DivFilePaletteDecoder ();

            using ( var stream = System.IO.File.Open ( path, FileMode.Open ) )
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
        public static void SavePal ( Palette palette, string path )
        {
            PalPaletteEncoder encoder = new PalPaletteEncoder ();

            using ( var stream = System.IO.File.Open ( path, FileMode.Open ) )
            {
                encoder.Encode ( palette, stream );
            }
        }
    }
}
