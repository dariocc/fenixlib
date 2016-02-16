using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace BennuLib.Bennu.IO
{
	public class MapSpriteDecoder : NativeDecoder<Sprite>
	{

		public override int MaxSupportedVersion { get; }

		protected override string[] KnownFileExtensions { get; }

		protected override string[] KnownFileIds { get; }

		protected override Sprite ReadNativeFormat(Magic magic, NativeFormatReader reader)
		{
			int width = reader.ReadUInt16();
			int height = reader.ReadUInt16();
			int code = reader.ReadInt32();

			var description = reader.ReadDescription();

			var depth = magic.Depth;
			if (depth == 8) {
				var pal = Palette.Create(VGAtoColors(reader.ReadPalette()));
				reader.ReadUnusedPaletteGamma();
			}

			var numberOfPivotPoints = reader.ReadPivotPointsNumber();
			[] pivotPoints = reader.ReadPivotPoints(numberOfPivotPoints);

			var mapDataLength = width * height * (depth / 8);
			var graphicData = reader.ReadBytes(mapDataLength);
			var pixels = CreatePixelBuffer(magic.Depth, graphicData);

			var map = Sprite.Create(width, height, pixels);

			foreach (? pivotPoint_loopVariable in pivotPoints) {
				pivotPoint = pivotPoint_loopVariable;
				map.DefinePivotPoint(pivotPoint.Id, pivotPoint.X, pivotPoint.Y);
			}

			return map;
		}
	}
}
