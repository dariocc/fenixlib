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
        public FontGlyph ( char character, IGlyph glyph )
        {
            Character = character;
            decorated = glyph;
        }

        public char Character { get; }

        public IGlyph decorated { get; }

        public int XAdvance
        {
            get
            {
                return decorated.XAdvance;
            }

            set
            {
                decorated.XAdvance = value;
            }
        }

        public int XOffset
        {
            get
            {
                return decorated.XOffset;
            }

            set
            {
                decorated.XOffset = value;
            }
        }

        public int YAdavance
        {
            get
            {
                return decorated.YAdavance;
            }

            set
            {
                decorated.YAdavance = value;
            }
        }

        public int YOffset
        {
            get
            {
                return decorated.YOffset;
            }

            set
            {
                decorated.YOffset = value;
            }
        }

        public GraphicFormat GraphicFormat => decorated.GraphicFormat;

        public int Height => decorated.Height;

        public Palette Palette => decorated.Palette; // TODO: Shall return the parent palette

        public int Width => decorated.Width;

        public byte[] PixelData => decorated.PixelData;
    }
}
