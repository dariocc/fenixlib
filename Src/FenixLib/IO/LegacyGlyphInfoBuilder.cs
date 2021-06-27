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
    public sealed class LegacyGlyphInfoBuilder
        : ILegacyGlyphInfoBuilder, IGlyphInfoProperties
    {
        private int width;
        private int height;
        private int yOffset;
        private int fileOffset;

        int IGlyphInfoProperties.FileOffset => fileOffset;

        int IGlyphInfoProperties.Height => height;

        int IGlyphInfoProperties.Width => width;

        int IGlyphInfoProperties.XAdvance => width;

        int IGlyphInfoProperties.XOffset => 0;

        int IGlyphInfoProperties.YAdvance => height + yOffset;

        int IGlyphInfoProperties.YOffset => yOffset;

        public ILegacyGlyphInfoBuilder Width ( int width )
        {
            this.width = width;
            return this;
        }

        public ILegacyGlyphInfoBuilder Height ( int height )
        {
            this.height = height;
            return this;
        }

        public ILegacyGlyphInfoBuilder YOffset ( int yOffset )
        {
            this.yOffset = yOffset;
            return this;
        }

        public ILegacyGlyphInfoBuilder FileOffset ( int fileOffset )
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
