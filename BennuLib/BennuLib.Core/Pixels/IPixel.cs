using Bennu.Util;

namespace Bennu
{
	public abstract class IPixel
	{
		public abstract int Argb { get; }
        public abstract int Red { get; }
        public abstract int Green { get; }
        public abstract int Blue { get; }
        public abstract int Alpha { get; }
        public abstract int Value { get; }
        public abstract IPixel GetTransparentCopy ();
        public abstract IPixel GetOpaqueCopy ();

        public abstract bool IsTransparent { get; }
        internal abstract byte[] GetRawValueBytes ();
    }
}
