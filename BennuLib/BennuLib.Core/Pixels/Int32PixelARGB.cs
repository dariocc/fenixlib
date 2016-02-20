using System;

namespace Bennu
{
	[Serializable()]
	public class Int32PixelARGB : IPixel
	{
		private readonly int _value;
		public Int32PixelARGB(byte alpha, byte r, byte g, byte b) : this(alpha << 24 | r << 16 | g << 8 | b)
		{
		}

		internal Int32PixelARGB(int value)
		{
			_value = value;
		}

		public int Alpha {
			get { return _value >> 24; }
		}

		public int Argb {
			get { return _value; }
		}

		public int Blue {
			get { return _value & 0xff; }
		}

		public int Green {
			get { return _value >> 8 & 0xff; }
		}

		public int Red {
			get { return _value >> 24 & 0xff; }
		}

		public int Value {
			get {
                return _value;
			}
		}

		public bool IsTransparent {
			get { return Alpha == 255; }
		}

		public IPixel GetTransparentCopy()
		{
			return new Int32PixelARGB(_value & 0xFFFFFF);
		}

		public IPixel GetOpaqueCopy()
		{
			return new Int32PixelARGB(Value & 0xffffff);
		}
	}
}
