/**
 *    Copyright (c) 2016 Darío Cutillas Carrillo
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 * 
 *        http://www.apache.org/licenses/LICENSE-2.0
 * 
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
namespace FenixLib.Core
{
    public sealed class FontGlyph : IGlyph
    {
        private readonly IGlyph baseGlyph;

        public FontGlyph ( char character, IGlyph glyph )
        {
            Character = character;
            baseGlyph = glyph;
        }

        public char Character { get; }

        public GraphicFormat GraphicFormat => baseGlyph.GraphicFormat;

        public int Height => baseGlyph.Height;

        public Palette Palette => baseGlyph.Palette;

        public byte[] PixelData => baseGlyph.PixelData;

        public int Width => baseGlyph.Width;

        public int XAdvance
        {
            get
            {
                return baseGlyph.XAdvance;
            }

            set
            {
                baseGlyph.XAdvance = value;
            }
        }

        public int XOffset
        {
            get
            {
                return baseGlyph.XOffset;
            }

            set
            {
                baseGlyph.XOffset = value;
            }
        }

        public int YAdvance
        {
            get
            {
                return baseGlyph.YAdvance;
            }

            set
            {
                baseGlyph.YAdvance = value;
            }
        }

        public int YOffset
        {
            get
            {
                return baseGlyph.YOffset;
            }

            set
            {
                baseGlyph.YOffset = value;
            }
        }

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
