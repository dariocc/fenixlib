using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

using System.IO;

namespace BennuLib.Bennu
{
	public static class Map
	{
		public static Sprite Load(string fileName)
		{
			IO.MapSpriteDecoder decoder = new IO.MapSpriteDecoder();

			using (Stream == File.Open(fileName, FileMode.Open)) {
				return decoder.Decode(Stream);
			}

			return null;
		}

		public static void Save(Sprite sprite, string fileName)
		{
			IO.MapSpriteEncoder encoder = new IO.MapSpriteEncoder();
			using (output == System.IO.File.Open(fileName, FileMode.Create)) {
				encoder.Encode(sprite, output);
			}
		}
	}

	public static class Fpg
	{
		public static SpriteAsset Load(string fileName)
		{
			IO.FpgSpriteAssetDecoder decoder = new IO.FpgSpriteAssetDecoder();

			using (Stream == File.Open(fileName, FileMode.Open)) {
				return decoder.Decode(Stream);
			}

			return null;
		}
	}

	public static class Pal
	{
		public static Palette Load(string fileName)
		{
			IO.DivFormatPaletteDecoder decoder = new IO.DivFormatPaletteDecoder();

			using (Stream == File.Open(fileName, FileMode.Open)) {
				return decoder.Decode(Stream);
			}

			return null;
		}
	}
}
