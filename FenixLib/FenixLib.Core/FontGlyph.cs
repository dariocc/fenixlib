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
namespace FenixLib.Core
{
    public sealed class FontGlyph : IGlyph
    {
        internal FontGlyph ( char character, IGlyph glyph )
        {
            Character = character;
            BaseGlyph = glyph;
        }

        public char Character { get; }

        private IGlyph BaseGlyph { get; }

        public int XAdvance
        {
            get
            {
                return BaseGlyph.XAdvance;
            }

            set
            {
                BaseGlyph.XAdvance = value;
            }
        }

        public int XOffset
        {
            get
            {
                return BaseGlyph.XOffset;
            }

            set
            {
                BaseGlyph.XOffset = value;
            }
        }

        public int YAdavance
        {
            get
            {
                return BaseGlyph.YAdavance;
            }

            set
            {
                BaseGlyph.YAdavance = value;
            }
        }

        public int YOffset
        {
            get
            {
                return BaseGlyph.YOffset;
            }

            set
            {
                BaseGlyph.YOffset = value;
            }
        }

        public GraphicFormat GraphicFormat => BaseGlyph.GraphicFormat;

        public int Height => BaseGlyph.Height;

        public Palette Palette => BaseGlyph.Palette;

        public int Width => BaseGlyph.Width;

        public byte[] PixelData => BaseGlyph.PixelData;

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( obj, null ) )
            {
                return false;
            }

            FontGlyph glyph = obj as FontGlyph;
            if ( ReferenceEquals ( glyph, null ) )
            {
                return false;
            }

            return Equals ( glyph );
        }

        public bool Equals ( FontGlyph glyph )
        {
            if ( ReferenceEquals ( glyph, null ) )
            {
                return false;
            }

            return ( glyph.Character.Equals ( Character ) );
        }

        public override int GetHashCode ()
        {
            return Character.GetHashCode ();
        }
    }
}
