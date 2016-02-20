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
        private IDictionary<byte, Glyph> _glyps = new SortedDictionary<byte, Glyph> ();

        protected BitmapFont ( Encoding encoding )
        {
            _encoding = encoding;
        }

        public Glyph this[char character]
        {
            get
            {
                return this[IndexForCharacter ( character )];
            }
            set
            {
                this[IndexForCharacter ( character )] = value;
            }
        }

        public Glyph this[byte index]
        {
            get
            {
                return _glyps.ElementAt ( index ).Value;
            }
            set
            {
                if ( index < 0 || index > 255 )
                    throw new IndexOutOfRangeException (); // TODO: Customize

                _glyps.Add ( index, value );
            }
        }

        public IEnumerable<Glyph> Glyphs
        {
            get
            {
                return _glyps.Values.AsEnumerable ();
            }
        }

        public static BitmapFont Create ( FontCodePage codepage )
        {
            return new BitmapFont ( Encoding.GetEncoding ( codepage.Value ) );
        }

        public IEnumerator<Glyph> GetEnumerator ()
        {
            return Glyphs.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return Glyphs.GetEnumerator ();
        }

        private byte IndexForCharacter ( char character )
        {
            byte[] bytes = new byte[1];
            _encoding.GetEncoder ().GetBytes ( new char[] { character }, 0, 0,
                bytes, 0, true );

            return bytes[0];
        }
    }
}
