using System;

namespace Bennu
{

    [Serializable ()]
    public class IndexedPixel : AbstractPixel
    {
        private readonly byte _index;

        public IndexedPixel ( byte index )
        {
            _index = index;
        }

        public override int Alpha
        {
            get { return _index == 0 ? 255 : 0; }
        }

        public override int Argb
        {
            get
            {
                if ( _index == 0 )
                {
                    return 0;
                }
                else {
                    return 255;
                }
            }
        }

        public override int Blue
        {
            get
            {
                throw new InvalidOperationException ();
            }
        }

        public override int Green
        {
            get
            {
                throw new InvalidOperationException ();
            }
        }

        public override int Red
        {
            get
            {
                throw new InvalidOperationException ();
            }
        }

        public override bool IsTransparent { get { return _index == 0; } }

        public override int Value
        {
            get
            {
                return _index;
            }
        }

        public override AbstractPixel GetOpaqueCopy ()
        {
            throw new NotImplementedException ();
        }

        public override AbstractPixel GetTransparentCopy ()
        {
            return new IndexedPixel ( 0 );
        }

        internal override byte[] GetRawValueBytes ()
        {
            return new byte[] { _index };
        }

        public override bool IsPalettized ()
        {
            return true;
        }
    }
}
