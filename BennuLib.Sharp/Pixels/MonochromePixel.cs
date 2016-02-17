using System;

namespace BennuLib.Pixels
{
    class MonochromePixel : IPixel
    {
        public int Alpha
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Argb
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Blue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Green
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsTransparent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Red
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPixel GetOpaqueCopy()
        {
            throw new NotImplementedException();
        }

        public IPixel GetTransparentCopy()
        {
            throw new NotImplementedException();
        }
    }
}
