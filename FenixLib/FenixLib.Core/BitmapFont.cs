/*  Copyright 2016 Darío Cutillas Carrillo
*
*   Licensed under the Apache License, Version 2.0 (the "License");
*   you may not use this file except in compliance with the License.
*   You may obtain a copy of the License at
*
*       http://www.apache.org/licenses/LICENSE-2.0
*
*   Unless required by applicable law or agreed to in writing, software
*   distributed under the License is distributed on an "AS IS" BASIS,
*   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*   See the License for the specific language governing permissions and
*   limitations under the License.
*/
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace FenixLib.Core
{
    public class BitmapFont : IEnumerable<Glyph>
    {
        private readonly Encoding _encoding;
        private readonly IDictionary<char, Glyph> _glyphs =
            new SortedDictionary<char, Glyph> ();

        public FontCodePage CodePage
        {
            get
            {
                return FontCodePage.FromEncoding ( _encoding );
            }
        }

        protected BitmapFont ( int depth, Encoding encoding, Palette palette = null )
        {
            _encoding = encoding;
            Depth = depth;
            Palette = palette;
        }

        public Glyph this[char character]
        {
            get
            {
                Glyph glyph;
                _glyphs.TryGetValue ( character, out glyph );

                return glyph;
            }
            set
            {
                if ( value.Depth != Depth )
                    throw new ArgumentException ("Glyph and font depths need to match");

                if (_glyphs.ContainsKey( character ) )
                    _glyphs[character] = value;
                else
                    _glyphs.Add ( character, value );
            }
        }

        public Glyph this[int index]
        {
            get
            {
                return this[Index2Char ( index )];
            }
            set
            {
                this[Index2Char ( index )] = value;
            }
        }

        public int Depth { get; }
        public Palette Palette { get; }

        public IEnumerable<Glyph> Glyphs
        {
            get
            {
                return _glyphs.Values.AsEnumerable ();
            }
        }

        public static BitmapFont Create ( DepthMode depthMode, FontCodePage codePage )
        {
            if ( depthMode == DepthMode.RgbIndexedPalette )
            {
                throw new InvalidOperationException (); // TODO: Customize
            }

            BitmapFont font = new BitmapFont ( ( int ) depthMode,
                Encoding.GetEncoding ( codePage.Value ) );

            return font;
        }

        public static BitmapFont Create ( Palette palette, FontCodePage codePage )
        {
            if ( palette == null )
            {
                throw new ArgumentException (); // TODO: Customize
            }

            BitmapFont font = new BitmapFont ( 8,
                Encoding.GetEncoding ( codePage.Value ),
                palette );

            return font;
        }

        public IEnumerator<Glyph> GetEnumerator ()
        {
            return Glyphs.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return Glyphs.GetEnumerator ();
        }

        private char Index2Char ( int index )
        {
            return _encoding.GetChars ( BitConverter.GetBytes ( index ) )[0];
        }
    }
}
