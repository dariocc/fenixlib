using System;

namespace BennuLib
{
	[Serializable()]
	public class Glyph
	{
		public int Width { get; }
		public int Height { get; }
        public int YOffset { get; }
        public int XOffset { get; }
        public int XAdvance { get; }
        public int YAdavance { get; }
		public IPixel[] Pixels { get; }
	}
}
