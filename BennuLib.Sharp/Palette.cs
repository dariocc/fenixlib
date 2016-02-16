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
	public class Palette : IEnumerable
	{

		public struct Color
		{
			public readonly int r;
			public readonly int g;

			public readonly int b;
			public Color(int r, int g, int b)
			{
				this.r = r;
				this.g = g;
				this.b = b;
			}
		}


		private Color[] _colors;
		public static Palette Create(Color[] colors)
		{
			return new Palette(colors);
		}

		private Palette(Color[] colors)
		{
			_colors = colors;
		}

		public Color this[int index] {
			get { return _colors[index]; }
			set { _colors[index] = value; }
		}

		public Color[] Colors {
			get { return _colors; }
		}

		public Palette GetCopy()
		{
			Color[] colors = new Color[_colors.Length];
			_colors.CopyTo(colors, 0);
			return Create(colors);
		}

		public IEnumerator GetEnumerator()
		{
			return _colors.GetEnumerator();
		}
	}
}
