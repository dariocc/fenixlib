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
using static System.Collections.StructuralComparisons;

namespace FenixLib.IO
{
	public static class NativeFormat
	{
		public static readonly byte[] Terminator = {
			0x1a,
			0xd,
			0xa,
			0x0
		};
	    
        /// <summary>
        /// The size of the color palette area, in bytes
        /// </summary>
		public const int PaletteBytesSize = 768;

        /// <summary>
        /// The size of the gamma color area, in bytes
        /// </summary>
		public const int ReservedBytesSize = 576;
		
        /// <summary>
        /// Bit mask used to separate the number of pivot points from the animation flags
        /// </summary>
		public const int PivotPointsNumberBitMask = 0xfff;
		
        /// <summary>
        /// Bit mask used to get the animation bit flag. Animation is not supported.
        /// </summary>
		public const int AnimationFlagBitMask = 0x1000;

        /// <summary>
        /// Computes the size in bytes of the area containing the pixel data for the specified 
        /// depth
        /// </summary>
        /// <param name="bpp"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int CalculatePixelBufferBytes(int bpp, int width, int height)
        {
            int byteLength;

            if ( bpp == 1 )
            {
                int rowByteSize = ( width + ( 8 - ( ( ( width % 8 ) ) & 7 ) ) ) / 8;
                byteLength = rowByteSize * height;
            }
            else
            {
                byteLength = width * height * bpp / 8;
            }

            return byteLength;
        }

        /// <summary>
        /// Represents the header of the native formats, i.e. a section describing type
        /// type of file (graphic, graphic collection, font or palettes), and the bpp
        /// of the graphic information (1, 8, 16 or 32bpp).
        /// </summary>
		public sealed class Header
		{

            private string _magic;
			public string Magic { get { return _magic; } }

            private int _lastByte;
			public int LastByte { get { return _lastByte; } }

			private readonly byte[] _terminator = new byte[5];
            public byte[] Terminator { get { return _terminator;  } }
			
			public Header(string magic, byte[] terminator, int lastByte)
			{
				_magic = magic.ToLower();
				_lastByte = lastByte;
				_terminator = terminator;
			}

			public bool IsTerminatorValid()
            {
                    return StructuralEqualityComparer.Equals(NativeFormat.Terminator, Terminator);
			}

            /// <summary>
            /// All native format's magic follow the pattern 'aXY' where XY indicates, for
            /// non 8bpp formats, the bpp (01, 16 or 32). For 8bpp formats, XY ar two 
            /// characters.
            /// </summary>
			public int BitsPerPixel
            {
                get
                {
                    int bpp;

                    if (int.TryParse(Magic.Substring(1, 2), out bpp ) )
                        return bpp;
                    else
                        return 8;
                }
            }

		}

		public struct GlyphInfo
		{
            /// <summary>
            /// The width of the character's glyph
            /// </summary>
			public int Width { get; }
            /// <summary>
            /// The height of the characters's glyph
            /// </summary>
			public int Height { get; }
            /// <summary>
            /// Displacement in the x-axis from the left side
            /// </summary>
            public int XOffset { get; }
            /// <summary>
            /// Displacement in the Y-axis from the top side
            /// </summary>
			public int YOffset { get; }
            /// <summary>
            /// 
            /// </summary>
            public int XAdvance { get; }
            /// <summary>
            /// 
            /// </summary>
            public int YAdvance { get; }
            /// <summary>
            /// The byte-location of the glyph's graphic data (pixels) in the
            /// file.
            /// </summary>
            public int FileOffset { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="yOffset"></param>
            /// <param name="fileOffset"></param>
            public GlyphInfo(int width, int height, int yOffset, int fileOffset)
			{
                Width = width;
                Height = height;
                XOffset = 0;
                YOffset = yOffset;
                XAdvance = width;
                YAdvance = height + yOffset;
                FileOffset = fileOffset;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <param name="xOffset"></param>
            /// <param name="yOffset"></param>
            /// <param name="xAdvance"></param>
            /// <param name="yAdvance"></param>
            /// <param name="fileOffset"></param>
            public GlyphInfo(int width, int height, int xOffset, 
                int yOffset, int xAdvance, int yAdvance, int fileOffset)
            {
                Width = width;
                Height = height;
                XOffset = xOffset;
                YOffset = yOffset;
                XAdvance = xAdvance;
                YAdvance = yAdvance; 
                FileOffset = fileOffset;
            }
		}
	}
}
