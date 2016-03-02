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
    public class BitmapFont : IEnumerable<IGlyph>
    {
        private readonly Encoding encoding;
        private readonly IDictionary<char, IGlyph> glyphs =
            new SortedDictionary<char, IGlyph> ();

        public FontCodePage CodePage
        {
            get
            {
                return FontCodePage.FromEncoding ( encoding );
            }
        }

        protected BitmapFont ( GraphicFormat graphicFormat, Encoding encoding, 
            Palette palette = null )
        {
            this.encoding = encoding;
            GraphicFormat = graphicFormat;
            Palette = palette;
        }

        public IGlyph this[char character]
        {
            get
            {
                IGlyph glyph;
                glyphs.TryGetValue ( character, out glyph );

                return glyph;
            }
            set
            {
                if ( value.GraphicFormat != GraphicFormat )
                    throw new ArgumentException ("Glyph and font graphic formats "
                        + "need to match.");

                if (glyphs.ContainsKey( character ) )
                    glyphs[character] = value;
                else
                    glyphs.Add ( character, value );
            }
        }

        public IGlyph this[int index]
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

        public GraphicFormat GraphicFormat { get; }
        public Palette Palette { get; }

        public IEnumerable<IGlyph> Glyphs
        {
            get
            {
                return glyphs.Values.AsEnumerable ();
            }
        }

        public static BitmapFont Create ( GraphicFormat graphicFormat, FontCodePage codePage )
        {
            if ( graphicFormat == GraphicFormat.RgbIndexedPalette )
            {
                throw new InvalidOperationException (); // TODO: Customize
            }

            BitmapFont font = new BitmapFont ( graphicFormat,
                Encoding.GetEncoding ( codePage.Value ) );

            return font;
        }

        public static BitmapFont Create ( Palette palette, FontCodePage codePage )
        {
            if ( palette == null )
            {
                throw new ArgumentException (); // TODO: Customize
            }

            BitmapFont font = new BitmapFont ( (GraphicFormat) 8,
                Encoding.GetEncoding ( codePage.Value ),
                palette );

            return font;
        }

        public IEnumerator<IGlyph> GetEnumerator ()
        {
            return Glyphs.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return Glyphs.GetEnumerator ();
        }

        private char Index2Char ( int index )
        {
            return encoding.GetChars ( BitConverter.GetBytes ( index ) )[0];
        }
    }
}
