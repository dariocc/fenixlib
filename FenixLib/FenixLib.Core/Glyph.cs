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
using System;

namespace FenixLib.Core
{
    [Serializable ()]
    public class Glyph : IGlyph
    {
        public int YOffset { get; set; } = 0;
        public int XOffset { get; set; } = 0;
        public int XAdvance { get; set; } = 0;
        public int YAdavance { get; set; } = 0;

        private IGraphic graphic;

        public GraphicFormat GraphicFormat => graphic.GraphicFormat;

        public int Height => graphic.Height;

        public Palette Palette => graphic.Palette;

        public int Width => graphic.Width;

        public byte[] PixelData => graphic.PixelData;

        public Glyph ( IGraphic graphic )
        {
            if (graphic == null)
            {
                throw new ArgumentNullException ( "graphic" );
            }
        }
    }
}
