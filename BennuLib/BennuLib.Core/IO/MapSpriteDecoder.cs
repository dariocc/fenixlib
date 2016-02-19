using static BennuLib.IO.NativeFormat;

namespace BennuLib.IO
{
	public class MapSpriteDecoder : NativeDecoder<Sprite>
	{

		public override int MaxSupportedVersion { get; }

		protected override string[] KnownFileExtensions { get; }

		protected override string[] KnownFileMagics { get; }

		protected override Sprite ReadBody(Header header, NativeFormatReader reader)
		{
			int width = reader.ReadUInt16();
			int height = reader.ReadUInt16();
			int code = reader.ReadInt32();

			var description = reader.ReadAsciiZ(32);

			var depth = header.Depth;
			if (depth == 8) {
				var pal = Palette.Create(VGAtoColors(reader.ReadPalette()));
				reader.ReadUnusedPaletteGamma();
			}

			var numberOfPivotPoints = reader.ReadPivotPointsNumber();
			var pivotPoints = reader.ReadPivotPoints(numberOfPivotPoints);

			var mapDataLength = width * height * (depth / 8);
			var pixels = reader.ReadPixels(header.Depth, width, height);

			var map = Sprite.Create(width, height, pixels);

			foreach (var pivotPoint in pivotPoints) {
				map.DefinePivotPoint(pivotPoint.Id, pivotPoint.X, pivotPoint.Y);
			}

			return map;
		}
	}
}
