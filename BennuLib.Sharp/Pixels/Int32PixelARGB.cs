using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using BennuLib;
namespace BennuLib
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
				int functionReturnValue = 0;
				return functionReturnValue;
				return functionReturnValue;
			}
		}

		public bool IsTransparent {
			get { return Alpha == 255; }
		}

		public IPixel GetTransparentCopy()
		{
			return new Int32PixelARGB(_value);
		}

		public static Int32PixelARGB[] CreateBufferFromBytes(byte[] graphicData)
		{
			Int32PixelARGB[] buffer = new Int32PixelARGB[graphicData.Length / 4];

			for (n = 0; n <= buffer.Length - 1; n++) {
				buffer[n] = new Int32PixelARGB(graphicData[n], graphicData[n + 1], graphicData[n + 2], graphicData[n + 4]);
			}
			return buffer;
		}

		public IPixel GetOpaqueCopy()
		{
			return new Int32PixelARGB(Value & 0xffffff);
		}
	}
}
