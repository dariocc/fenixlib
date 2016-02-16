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
	public class DivFormatPaletteDecoder : NativeDecoder<Palette>
	{

		public override int MaxSupportedVersion { get; }

		protected override string[] KnownFileExtensions { get; }

		protected override string[] KnownFileIds { get; }

		protected override Palette ReadNativeFormat(Magic magic, NativeFormatReader reader)
		{
			// Map files have the Palette data in a different position than the rest of the files
			if (magic.FileType == "map")
				reader.ReadBytes(40);

			return Palette.Create(VGAtoColors(reader.ReadPalette()));
		}
	}
}
