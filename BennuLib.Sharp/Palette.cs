using System;
using System.Collections;
using System.Linq;

namespace BennuLib
{
	[Serializable()]
	public partial class Palette : IEnumerable
	{
        private Color[] _colors;

        private Palette(Color[] colors)
        {
            _colors = colors;
        }

        public Color[] Colors
        {
            get { return _colors; }
        }
        public Color this[int index]
        {
            get { return _colors[index]; }
            set { _colors[index] = value; }
        }

        public static Palette Create(Color[] colors)
		{
            if (colors.Length < 256)
                throw new ArgumentException(); // TODO: Customize

			return new Palette(colors);
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            // Should we use reference comparison?
            Palette palette = obj as Palette;
            if (ReferenceEquals(palette, null))
            {
                return false;
            }

            return Equals(palette);
        }

        public bool Equals(Palette palette)
        {
            // TODO: does it matter if we have the (object)?
            // Should we use reference comparison?
            if ( ReferenceEquals(palette, null) )
            {
                return false;
            }

            return ( palette.Colors.SequenceEqual(Colors) );
        }

        public override int GetHashCode()
        {
            // TODO: We might want to make a better one...
            return _colors[0].r.GetHashCode() ^ _colors[100].r.GetHashCode();
        }

        public static bool operator == (Palette paletteA, Palette paletteB)
        {
            return paletteA.Equals(paletteB);
        }

        public static bool operator !=(Palette paletteA, Palette paletteB)
        {
            return !paletteA.Equals(paletteB);
        }
    }
}
