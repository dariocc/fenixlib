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
    public struct GlyphInfo
    {
        /// <summary>
        /// The width of the character's glyph.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// The height of the characters's glyph.
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Displacement in the x-axis from the left side.
        /// </summary>
        public int XOffset { get; }
        /// <summary>
        /// Displacement in the Y-axis from the top side.
        /// </summary>
        public int YOffset { get; }
        /// <summary>
        /// Advance in pixels along the x-axis after the glyph is drawn.
        /// In other words: the origin of x-position calculation for the next
        /// glyph in a bitmap text.
        /// </summary>
        public int XAdvance { get; }
        /// <summary>
        /// Advance in pixels along the y-axis after the glyph is drawn.
        /// In other words: the origin of y-position calculation for the next
        /// glyph in a bitmap text.
        /// </summary>
        public int YAdvance { get; }
        /// <summary>
        /// The byte-location of the glyph's graphic data (pixels) in the
        /// file.
        /// </summary>
        public int FileOffset { get; }

        public GlyphInfo ( IGlyphInfoProperties properties )
        {
            Width = properties.Width;
            Height = properties.Height;
            XOffset = properties.XOffset;
            YOffset = properties.YOffset;
            XAdvance = properties.XAdvance;
            YAdvance = properties.YAdvance;
            FileOffset = properties.FileOffset;
        }
    }
}
