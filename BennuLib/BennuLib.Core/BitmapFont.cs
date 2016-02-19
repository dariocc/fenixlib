using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace BennuLib
{
    public class BitmapFont : IEnumerable<Glyph>
    {
        private Encoding _encoding;
        private IDictionary<byte, Glyph> _glyps = new SortedDictionary<byte, Glyph>();

        protected BitmapFont(Encoding encoding)
        {
            _encoding = encoding;
        }

        public Glyph this[char character]
        {
            get
            {
                byte[] bytes = new byte[1];
                _encoding.GetEncoder().GetBytes( new char[]{ character }, 0, 0, 
                    bytes, 0, true);

                return null;
            }
        }

        public Glyph this[int index]
        {
            get
            {
                return _glyps.ElementAt(index).Value;
            }
        }

        public IEnumerable<Glyph> Glyphs
        {
            get
            {
                return _glyps.Values.AsEnumerable();
            }
        }

        public static BitmapFont Create(int codepage)
        {
            // TODO: codepage should be encapsulated in an enum...

            if ( codepage != 850 || codepage != 28591)
            {
                throw new ArgumentException();
            }

            return new BitmapFont(Encoding.GetEncoding(codepage));
        }

        public IEnumerator<Glyph> GetEnumerator()
        {
            return _glyps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
