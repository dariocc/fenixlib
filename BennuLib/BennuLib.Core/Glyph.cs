using System;

namespace BennuLib
{
	[Serializable()]
	public class Glyph
	{
		public int Width { get; }
		public int Height { get; }
        public int YOffset { get; set; } = 0;
        public int XOffset { get; set; } = 0;
        public int XAdvance { get; set; } = 0;
        public int YAdavance { get; set; } = 0;
		public IPixel[] Pixels { get; }

        public Glyph(int width, int height, IPixel[] pixels)
        {
            Width = width;
            Height = height;

            if ( width * height != pixels.Length )
                throw new InvalidOperationException (); // TODO: Customize

            Pixels = pixels;
        }

	}
}
