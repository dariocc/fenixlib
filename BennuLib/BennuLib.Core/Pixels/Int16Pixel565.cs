using System;

namespace BennuLib
{
	[Serializable()]
	public class Int16Pixel565 : IPixel
	{

		// TODO: It is certainly possible to speed up this colo conversions by 
		// keeping the list of all 65535 colors possible in memmory (65Kb)...

		private readonly ushort _value;
		public Int16Pixel565(byte r, byte g, byte b) : this(Convert.ToUInt16((r >> 3) << 11 | (g >> 2) << 5 | b >> 2))
		{
		}

		internal Int16Pixel565(ushort value)
		{
			_value = value;
		}

		public int Alpha {
			get { return _value == 0 ? 255 : 0; }
		}

		public int Argb {
			get { return (Alpha << 24 | Red << 16 | Green << 8 | Blue); }
		}

		public int Blue {
			get { return _value & 0x1f; }
		}

		public int Green {
			get { return _value >> 5 & 0x3f; }
		}

		public int Red {
			get { return _value >> 11 & 0x1f; }
		}

		public int Value {
			get { return _value; }
		}

		public bool IsTransparent {
			get { return Value == 0; }
		}

		public IPixel GetTransparentCopy()
		{
			return new Int16Pixel565(0);
		}

		public IPixel GetOpaqueCopy()
		{
			if (_value == 0) {
				return new Int16Pixel565(1);
			} else {
				return new Int16Pixel565(_value);
			}
		}
	}
}
