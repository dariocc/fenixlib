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
	public class IndexedPixel : IPixel
	{


		private readonly int _index;

		public IndexedPixel(int index)
		{
			_index = index;
		}

		public int Alpha {
			get { return _index == 0 ? 255 : 0; }
		}

		public int Argb {
			get {
				if (_index == 0) {
					return 0;
				} else {
					return 255;
				}
			}
		}

		public int Blue {
			get {
				// TODO: Might need to keep a reference to a color table
				throw new InvalidOperationException();
			}
		}

		public int Green {
			get {
				// TODO: Might need to keep a reference to a color table
				throw new InvalidOperationException();
			}
		}

		public bool IsTransparent {
			get { return _index == 0; }
		}

		public int Red {
			get {
				throw new InvalidOperationException();
			}
		}

		public int Value {
			get { return _index; }
		}

		public IPixel GetOpaqueCopy()
		{
			// TODO: Should find the closest color in the color space that is not in index 0
			throw new InvalidOperationException();
		}

		public IPixel GetTransparentCopy()
		{
			return new IndexedPixel(0);
		}

		// TODO: Might not belong here
		public static IndexedPixel[] CreateBufferFromBytes(byte[] graphicData)
		{
			IndexedPixel[] buffer = new IndexedPixel[graphicData.Length];
			for (n = 0; n <= buffer.Length - 1; n++) {
				buffer[n] = new IndexedPixel(graphicData[n]);
			}
			return buffer;
		}
	}
}
