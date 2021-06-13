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

        public FontEncoding Encoding => FontEncoding.FromEncoding ( encoding );

        public BitmapFont ( FontEncoding encoding, GraphicFormat format,
            Palette palette = null )
        {
            if ( format == GraphicFormat.Format8bppIndexed )
            {
                if ( palette == null )
                {
                    throw new ArgumentNullException ( "palette",
                        "A palette is required for RgbIndexedPalette format." );
                }
                Palette = palette;
            }

            this.encoding = System.Text.Encoding.GetEncoding ( encoding.CodePage );
            glyphs = new UniformFormatGraphicDictionary<char, FontGlyph> (
                format, DefaultCapacity );
        }

        public BitmapFont ( FontEncoding encoding, Palette palette )
            : this ( encoding, GraphicFormat.Format8bppIndexed, palette )
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
                    glyphs[character] = PrepareGlyph ( character, value );
                }
                else
                {
                    glyphs.Add ( character, PrepareGlyph ( character, value ) );
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
        
        /// <summary>
        /// Returns a <see cref="FontGlyph"/> whose palette is that of the 
        /// <see cref="BitmapFont"/>.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="glyph"></param>
        /// <returns></returns>
        private FontGlyph PrepareGlyph (char character, IGlyph glyph)
        {
            return new FontGlyph ( character, new ChildGlyph ( this, glyph ) );
        }

        /// <summary>
        /// An <see cref="IGlyph"/> decorator that replaces the Palette with the palette of a
        /// <see cref="BitmapFont"/> which is considered the parent.
        /// </summary>
        private class ChildGlyph : IGlyph
        {
            private readonly BitmapFont parentFont;
            private readonly IGlyph glyph;

            public ChildGlyph ( BitmapFont parentFont, IGlyph glyph )
            {
                this.parentFont = parentFont;
                this.glyph = glyph;
            }

            public GraphicFormat GraphicFormat => glyph.GraphicFormat;

            public int Height => glyph.Height;

            public Palette Palette => parentFont.Palette;

            public byte[] PixelData => glyph.PixelData;

            public int Width => glyph.Width;

            public int XAdvance
            {
                get
                {
                    return glyph.XAdvance;
                }

                set
                {
                    glyph.XAdvance = value;
                }
            }

            public int XOffset
            {
                get
                {
                    return glyph.XOffset;
                }

                set
                {
                    glyph.XOffset = value;
                }
            }

            public int YAdavance
            {
                get
                {
                    return glyph.YAdavance;
                }

                set
                {
                    glyph.YAdavance = value;
                }
            }

            public int YOffset
            {
                get
                {
                    return glyph.YOffset;
                }

                set
                {
                    glyph.YOffset = value;
                }
            }

        }
    }
}
