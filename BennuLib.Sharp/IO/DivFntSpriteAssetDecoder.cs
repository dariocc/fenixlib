using static BennuLib.IO.NativeFormat;

namespace BennuLib.IO
{
	public class DivFntFpgDecoder : NativeDecoder<SpriteAsset>
	{

		public override int MaxSupportedVersion { get; }

		protected override string[] KnownFileExtensions { get; }

		protected override string[] KnownFileMagics { get; }

		protected override SpriteAsset ReadBody(Header header, NativeFormatReader reader)
		{

			var pal = Palette.Create(VGAtoColors(reader.ReadPalette()));
			reader.ReadUnusedPaletteGamma();

			int fontInfo = reader.ReadInt32();

			GlyphInfo[] characters = new GlyphInfo[256];
			for (var n = 0; n <= 255; n++) {
				characters[n] = reader.ReadGlyphInfo();
			}

			SpriteAsset fpg = new SpriteAsset();
			foreach (var character in characters) {
				var dataLength = character.Height * character.Width;

				if (character.FileOffset == 0 | dataLength == 0)
					continue;

				var graphicData = reader.ReadBytes(dataLength);

				var pixels = IndexedPixel.CreateBufferFromBytes(graphicData);
				var map = Sprite.Create(character.Width, character.Height, pixels);
				fpg.Add(fpg.FindFreeId(), ref map);
			}

			return fpg;
		}
	}
}
