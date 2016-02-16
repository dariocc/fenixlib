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
	public class FpgSpriteAssetDecoder : NativeDecoder<SpriteAsset>
	{

		public override int MaxSupportedVersion { get; }
		// Current readable Fpg version

		protected override string[] KnownFileExtensions { get; }

		protected override string[] KnownFileIds { get; }

		protected override SpriteAsset ReadNativeFormat(Magic magic, NativeFormatReader reader)
		{
			var depth = magic.Depth;
			if (depth == 8) {
				var pal = Palette.Create(VGAtoColors(reader.ReadPalette()));
				reader.ReadUnusedPaletteGamma();
			}

			SpriteAsset fpg = new SpriteAsset();

			try {
				do {
					var code = reader.ReadInt32();
					var maplen = reader.ReadInt32();
					var description = reader.ReadDescription();
					var name = reader.ReadChars(12).ToString();
					var width = reader.ReadInt32();
					var height = reader.ReadInt32();

					var mapDataLength = width * height * (depth / 8);

					var numberOfPivotPoints = reader.ReadPivotPointsNumber();
					[] pivotPoints = reader.ReadPivotPoints(numberOfPivotPoints);

					// Serves as checksum for FPGs created with non-standard tools such
					// as FPG Edit
					if (mapDataLength + 64 + numberOfPivotPoints * 4 != maplen) {
						break; // TODO: might not be correct. Was : Exit Do
					}


					var graphicData = reader.ReadBytes(mapDataLength);
					var pixels = CreatePixelBuffer(magic.Depth, graphicData);

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
