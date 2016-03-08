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
    public class BitmapFont : IBitmapFont
    {
        private const int DefaultCapacity = 256;
        private readonly Encoding encoding;
        private UniformFormatGraphicDictionary<char, FontGlyph> glyphs;

        public FontEncoding CodePage => FontEncoding.FromEncoding ( encoding );

        public BitmapFont ( FontEncoding encoding, GraphicFormat format,
            Palette palette = null )
        {
            if ( format == GraphicFormat.RgbIndexedPalette )
            {
                if ( palette == null )
                {
                    throw new ArgumentNullException ( "palette",
                        "A palette is required for RgbIndexedPalette format." );
                }
                Palette = palette;
            }

            this.encoding = Encoding.GetEncoding ( encoding.CodePage );
            glyphs = new UniformFormatGraphicDictionary<char, FontGlyph> (
                format, DefaultCapacity );
        }

        public BitmapFont ( FontEncoding encoding, Palette palette )
            : this ( encoding, GraphicFormat.RgbIndexedPalette, palette )
        { }

        public IGlyph this[char character]
        {
            get
            {
                FontGlyph glyph;
                glyphs.TryGetValue ( character, out glyph );

                return glyph;
            }
            set
            {
                if ( glyphs.ContainsKey ( character ) )
                {
                    glyphs[character] = new FontGlyph ( character, value );
                }
                else
                {
                    glyphs.Add ( character, new FontGlyph ( character, value ) );
                }
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

        public GraphicFormat GraphicFormat => glyphs.GraphicFormat;
        public Palette Palette { get; }

        public IEnumerable<FontGlyph> Glyphs
        {
            get
            {
                return glyphs.Values.AsEnumerable ();
            }
        }

        public IEnumerator<FontGlyph> GetEnumerator ()
        {
            return glyphs.Values.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        private char Index2Char ( int index )
        {
            return encoding.GetChars ( BitConverter.GetBytes ( index ) )[0];
        }
    }
}
