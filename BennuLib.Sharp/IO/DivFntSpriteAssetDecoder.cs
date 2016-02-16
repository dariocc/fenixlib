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
	public class DivFntFpgDecoder : NativeDecoder<SpriteAsset>
	{

		public override int MaxSupportedVersion { get; }

		protected override string[] KnownFileExtensions { get; }

		protected override string[] KnownFileIds { get; }

		protected override SpriteAsset ReadNativeFormat(Magic magic, NativeFormatReader reader)
		{

			var pal = Palette.Create(VGAtoColors(reader.ReadPalette()));
			reader.ReadUnusedPaletteGamma();

			int fontInfo = reader.ReadInt32();

			GlyphInfo[] characters = new GlyphInfo[256];
			for (n = 0; n <= 255; n++) {
				characters(n) = reader.ReadGlyphInfo();
			}

			SpriteAsset fpg = new SpriteAsset();
			foreach (var character_loopVariable in characters) {
				character = character_loopVariable;
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
