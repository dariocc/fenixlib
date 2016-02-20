using System;

namespace BennuLib
{
    public sealed class DepthMode
    {
        public int Value { get; }

        private DepthMode ( int value )
        {
            Value = value;
        }

        public static explicit operator DepthMode ( int value )
        {
            switch ( value )
            {
                case 1:
                    return Monochrome;
                case 8:
                    return RgbIndexedPalette;
                case 16:
                    return RgbInt16;
                case 32:
                    return ArgbInt32;
            }

            throw new ArgumentException ();
        }

        public static explicit operator int (DepthMode depthMode)
        {
            return depthMode.Value;
        }

        public static DepthMode Monochrome = new DepthMode ( 1 );
        public static DepthMode RgbIndexedPalette = new DepthMode ( 8 );
        public static DepthMode RgbInt16 = new DepthMode ( 16 );
        public static DepthMode ArgbInt32 = new DepthMode ( 32 );
    }
}
