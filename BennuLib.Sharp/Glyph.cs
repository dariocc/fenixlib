using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace BennuLib
{
	[Serializable()]
	public class Glyph
	{

		public int Width { get; }
		public int Height { get; }
		public IPixel[] Pixels { get; }
	}
}
