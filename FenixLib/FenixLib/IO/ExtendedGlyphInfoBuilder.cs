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

namespace FenixLib.IO
{
    public sealed class ExtendedGlyphInfoBuilder :
        IExtendedGlyphInfoBuilder,
        IGlyphInfoProperties
    {
        private int width;
        private int height;
        private int xOffset;
        private int yOffset;
        private int xAdvance;
        private int yAdvance;
        private int fileOffset;

        int IGlyphInfoProperties.Width => width;

        int IGlyphInfoProperties.Height => height;

        int IGlyphInfoProperties.XAdvance => xAdvance;

        int IGlyphInfoProperties.YAdvance => yAdvance;

        int IGlyphInfoProperties.XOffset => xOffset;

        int IGlyphInfoProperties.YOffset => yOffset;

        int IGlyphInfoProperties.FileOffset => fileOffset;

        public IExtendedGlyphInfoBuilder Width ( int width )
        {
            this.width = width;
            return this;
        }

        public IExtendedGlyphInfoBuilder YAdvance ( int yAdvance )
        {
            this.yAdvance = yAdvance;
            return this;
        }

        public IExtendedGlyphInfoBuilder Height ( int height )
        {
            this.height = height;
            return this;
        }

        public IExtendedGlyphInfoBuilder YOffset ( int yOffset )
        {
            this.yOffset = yOffset;
            return this;
        }

        public IExtendedGlyphInfoBuilder XOffset ( int xOffset )
        {
            this.xOffset = xOffset;
            return this;
        }

        public IExtendedGlyphInfoBuilder XAdvance ( int xAdvance )
        {
            this.xAdvance = xAdvance;
            return this;
        }

        public IExtendedGlyphInfoBuilder FileOffset ( int fileOffset )
        {
            this.fileOffset = fileOffset;
            return this;
        }

        public GlyphInfo Build ()
        {
            return new GlyphInfo ( this );
        }
    }
}
