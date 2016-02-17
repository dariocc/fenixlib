using System;

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
