using System.IO;

namespace BennuLib.IO
{
    /// <summary>
    /// Provides static methods for the opening and creation of Bennu native file
    /// format.
    /// </summary>
	public static class File
	{

		public static Sprite LoadMap(string fileName)
		{
			MapSpriteDecoder decoder = new MapSpriteDecoder();

			using (var stream = System.IO.File.Open(fileName, FileMode.Open)) {
				return decoder.Decode(stream);
			}
		}

		public static void SaveMap(Sprite sprite, string fileName)
		{
			MapSpriteEncoder encoder = new MapSpriteEncoder();
			using (var output = System.IO.File.Open(fileName, FileMode.Create)) {
				encoder.Encode(sprite, output);
			}
		}

        public static SpriteAsset LoadFpg(string fileName)
        {
            FpgSpriteAssetDecoder decoder = new FpgSpriteAssetDecoder();

            using (var stream = System.IO.File.Open(fileName, FileMode.Open))
            {
                return decoder.Decode(stream);
            }
        }

        public static Palette LoadPal(string fileName)
        {
            DivFormatPaletteDecoder decoder = new DivFormatPaletteDecoder();

            using (var stream = System.IO.File.Open(fileName, FileMode.Open))
            {
                return decoder.Decode(stream);
            }
        }

        public static void SavePal(Palette palette, string fileName)
        {
            PalPaletteEncoder encoder = new PalPaletteEncoder();

            using (var stream = System.IO.File.Open(fileName, FileMode.Open))
            {
                encoder.Encode(palette, stream);
            }
        }
    }
}
