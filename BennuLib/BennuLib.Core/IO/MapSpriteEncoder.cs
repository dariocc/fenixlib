using System;
using System.Data;
using System.Linq;

namespace Bennu.IO
{
	public class MapSpriteEncoder : NativeEncoder<Sprite>
	{

		protected override byte Version { get; }

		protected override void WriteNativeFormat(Sprite sprite, NativeFormatWriter writer)
		{
			writer.Write(Convert.ToUInt16(sprite.Width));
			writer.Write(Convert.ToUInt16(sprite.Height));
			writer.Write(Convert.ToUInt32(sprite.Id.GetValueOrDefault()));

			if ((sprite.Palette != null)) {
				writer.Write(sprite.Palette);
				writer.WriteReservedPaletteGammaSection();
			}

			var ids = sprite.PivotPoints.Select(p => p.Id);

			writer.Write(Convert.ToUInt16(ids.Count() > 0 ? ids.Max() : 0));
			writer.Write(sprite.PivotPoints);
			//TODO
			writer.Write((Int32PixelARGB[])sprite.Pixels);
		}

		protected override string GetFileId(Sprite obj)
		{

			if ((obj.Pixels[0]) is IndexedPixel) {
				return "map";
			} else if ((obj.Pixels[0]) is Int16Pixel565) {
				return "m16";
			} else if ((obj.Pixels[0]) is Int32PixelARGB) {
				return "m32";
			} else {
				throw new ArgumentException();
			}

		}

	}
}
