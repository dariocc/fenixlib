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
	public class FpgSpriteAssetEncoder : NativeEncoder<SpriteAsset>
	{

		protected override byte Version { get; }

		protected override void WriteNativeFormat(SpriteAsset asset, NativeFormatWriter writer)
		{
			if (HasPalette(asset)) {
				// TODO: Write the palette
				// TODO: Write the unused bytes
			}

			foreach (Sprite sprite_loopVariable in asset) {
				sprite = sprite_loopVariable;
				// TODO: Will fail for no control points defined
				var maxPivotPointId = Convert.ToUInt16(sprite.PivotPoints.Max(p => p.Id));
				var maplen = Convert.ToUInt32(64 + Convert.ToUInt32(sprite.Width) * Convert.ToUInt32(sprite.Height) * asset.Depth / 8 + maxPivotPointId * 4);

				writer.Write(maplen);
				writer.WriteAsciiZ(sprite.Description, 32);
				writer.WriteAsciiZ("SpritePocket", 12);
				writer.Write(Convert.ToUInt32(sprite.Width));
				writer.Write(Convert.ToUInt32(sprite.Height));
				writer.Write(Convert.ToUInt32(maxPivotPointId));
				writer.Write(sprite.PivotPoints);
				// TODO: Fix
				writer.Write((Int32PixelARGB[])sprite.Pixels);
			}
		}

		private bool HasPalette(SpriteAsset asset)
		{
			return false;
			// TODO
		}

		protected override string GetFileId(SpriteAsset asset)
		{
			// TODO
			switch (asset.Depth) {
				case 8:
					return "fpg";
				case 16:
					return "f16";
				case 32:
					return "f32";
				default:
					// TODO more specific
					throw new ArgumentException();
			}
		}
	}
}
