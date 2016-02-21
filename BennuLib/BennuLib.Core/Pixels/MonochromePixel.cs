using System;

namespace Bennu
{
    class MonochromePixel : AbstractPixel
    {
        private const int Black = 0xFFFFFF << 8 | 0xFF;
        private byte _value;

        public MonochromePixel ( byte value )
        {
            if ( value > 1 )
                throw new ArgumentException ();

            _value = value;
        }

        public override int Alpha { get { return _value * 255; }
        }

        public override int Argb
        {
            get
            {
                if ( _value == 1 )
                    return Black;

                return 0;
            }
        }

        public override int Blue { get { return _value * 255; } }


        public override int Green { get { return _value * 255; } }

        public override bool IsTransparent
        {
            get
            {
                throw new NotImplementedException ();
            }
        }

        public override int Red { get { return _value; } }

        public override int Value { get { return _value; } }

        public override AbstractPixel GetOpaqueCopy ()
        {
            return new MonochromePixel ( 0 );
        }

        public override AbstractPixel GetTransparentCopy ()
        {
            return new MonochromePixel ( 1 );
        }

        internal override byte[] GetRawValueBytes ()
        {
            throw new NotImplementedException ();
        }
    }
}
