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
	public class PalPaletteEncoder : NativeEncoder<Palette>
	{

		protected override byte Version { get; }

		protected override void WriteNativeFormat(Palette palette, NativeFormatWriter writer)
		{
			writer.Write(palette);
			writer.WriteReservedPaletteGammaSection();
		}

		protected override string GetFileId(Palette obj)
		{
			return "pal";
		}
	}
}
