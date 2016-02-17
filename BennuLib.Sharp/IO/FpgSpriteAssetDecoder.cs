using static BennuLib.IO.NativeFormat;

namespace BennuLib.IO
{
	public class FpgSpriteAssetDecoder : NativeDecoder<SpriteAsset>
	{

		public override int MaxSupportedVersion { get; }
		// Current readable Fpg version

		protected override string[] KnownFileExtensions { get; }

		protected override string[] KnownFileMagics { get; }

		protected override SpriteAsset ReadBody(Header header, NativeFormatReader reader)
		{
			var depth = header.Depth;
			if (depth == 8) {
				var pal = Palette.Create(VGAtoColors(reader.ReadPalette()));
				reader.ReadUnusedPaletteGamma();
			}

			SpriteAsset fpg = new SpriteAsset();

			try {
				do {
					var code = reader.ReadInt32();
					var maplen = reader.ReadInt32();
					var description = reader.ReadAsciiZ(32);
					var name = reader.ReadAsciiZ(12);
					var width = reader.ReadInt32();
					var height = reader.ReadInt32();

					var mapDataLength = width * height * (depth / 8);

					var numberOfPivotPoints = reader.ReadPivotPointsNumber();
					var pivotPoints = reader.ReadPivotPoints(numberOfPivotPoints);

					// Serves as checksum for FPGs created with non-standard tools such
					// as FPG Edit
					if (mapDataLength + 64 + numberOfPivotPoints * 4 != maplen) {
						break; // TODO: might not be correct. Was : Exit Do
					}


					var graphicData = reader.ReadBytes(mapDataLength);
					var pixels = CreatePixelBuffer(header.Depth, graphicData);

					var map = Sprite.Create(width, height, pixels);
					map.Description = description;

					fpg.Update(code, map);
				} while (true);
			} catch (System.IO.EndOfStreamException exception) {
				// Do nothing (the reading of the map
			}

			return fpg;
		}
	}
}
