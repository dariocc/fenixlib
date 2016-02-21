using Bennu.Util;

namespace Bennu
{
	public abstract class AbstractPixel
	{
		public abstract int Argb { get; }
        public abstract int Red { get; }
        public abstract int Green { get; }
        public abstract int Blue { get; }
        public abstract int Alpha { get; }
        public abstract int Value { get; }
        public abstract AbstractPixel GetTransparentCopy ();
        public abstract AbstractPixel GetOpaqueCopy ();
        public abstract bool IsPalettized ();

        public abstract bool IsTransparent { get; }
        internal abstract byte[] GetRawValueBytes ();
    }
}
