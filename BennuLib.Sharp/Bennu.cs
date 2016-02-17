using System.IO;

namespace BennuLib.Bennu
{
	public static class Map
	{
		public static Sprite Load(string fileName)
		{
			IO.MapSpriteDecoder decoder = new IO.MapSpriteDecoder();

			using (var stream = File.Open(fileName, FileMode.Open)) {
				return decoder.Decode(stream);
			}
		}

		public static void Save(Sprite sprite, string fileName)
		{
			IO.MapSpriteEncoder encoder = new IO.MapSpriteEncoder();
			using (var output = System.IO.File.Open(fileName, FileMode.Create)) {
				encoder.Encode(sprite, output);
			}
		}
	}

	public static class Fpg
	{
		public static SpriteAsset Load(string fileName)
		{
			IO.FpgSpriteAssetDecoder decoder = new IO.FpgSpriteAssetDecoder();

			using (var stream = File.Open(fileName, FileMode.Open)) {
				return decoder.Decode(stream);
			}
		}
	}

	public static class Pal
	{
		public static Palette Load(string fileName)
		{
			IO.DivFormatPaletteDecoder decoder = new IO.DivFormatPaletteDecoder();

			using (var stream = File.Open(fileName, FileMode.Open)) {
				return decoder.Decode(stream);
			}
		}
	}
}
