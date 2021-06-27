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
    public class Glyph : IGlyph
    {
        public virtual int YOffset { get; set; } = 0;
        public virtual int XOffset { get; set; } = 0;
        public virtual int XAdvance { get; set; } = 0;
        public virtual int YAdvance { get; set; } = 0;

        // The implementation to satisfy the IGraphic interface will be delegated
        // to a graphic object that is injected in the constructor. This allows for 
        // reusability of the IGraphic implementations in other classes such as the
        // Sprite class.
        private IGraphic graphic;

        public GraphicFormat GraphicFormat => graphic.GraphicFormat;

        public virtual int Height => graphic.Height;

        public Palette Palette => graphic.Palette;

        public virtual int Width => graphic.Width;

        public virtual byte[] PixelData => graphic.PixelData;

        public Glyph ( IGraphic graphic )
        {
            if (graphic == null)
            {
                throw new ArgumentNullException ( "graphic" );
            }

            this.graphic = graphic;
        }
    }
}
