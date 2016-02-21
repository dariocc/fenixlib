using System;

namespace Bennu
{
	[Serializable()]
	public class Int32PixelARGB : AbstractPixel
	{
		private readonly int _value;

		public Int32PixelARGB(byte alpha, byte r, byte g, byte b) : this(alpha << 24 | r << 16 | g << 8 | b)
		{
		}

		internal Int32PixelARGB(int value)
		{
			_value = value;
		}

		public override int Alpha {
			get { return _value >> 24; }
		}

		public override int Argb {
			get { return _value; }
		}

		public override int Blue {
			get { return _value & 0xff; }
		}

		public override int Green {
			get { return _value >> 8 & 0xff; }
		}

		public override int Red {
			get { return _value >> 24 & 0xff; }
		}

		public override int Value {
			get {
                return _value;
			}
		}

		public override bool IsTransparent {
			get { return Alpha == 255; }
		}

        public override AbstractPixel GetTransparentCopy()
		{
			return new Int32PixelARGB(_value & 0xFFFFFF);
		}

		public override AbstractPixel GetOpaqueCopy()
		{
			return new Int32PixelARGB(Value & 0xffffff);
		}

        internal override byte[] GetRawValueBytes ()
        {
            return BitConverter.GetBytes( _value );
        }
    }
}
