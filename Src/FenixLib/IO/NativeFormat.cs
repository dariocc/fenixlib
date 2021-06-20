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
using FenixLib.Core;
using System;

namespace FenixLib.IO
{
    public static class NativeFormat
    {
        /// <summary>
        /// Sequence of bytes that that follows first three bytes for all
        /// native formats.
        /// </summary>
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
		public const int PaletteGammaBytesSize = 576;

        /// <summary>
        /// Bit mask used to separate the number of pivot points from the animation flags
        /// </summary>
        public const int PivotPointsNumberBitMask = 0xfff;

        /// <summary>
        /// Bit mask used to get the animation bit flag. Animation is not supported.
        /// </summary>
        public const int AnimationFlagBitMask = 0x1000;

        /// <summary>
        /// Represents the header of the native formats, i.e. a section describing type
        /// type of file (graphic, graphic collection, font or palettes), and the bpp
        /// of the graphic information (1, 8, 16 or 32bpp).
        /// </summary>
		public sealed class Header
        {
            private readonly string magic;
            private readonly int lastByte;
            private readonly byte[] terminator = new byte[5];

            public string Magic { get { return magic; } }

            public int LastByte { get { return lastByte; } }

            public byte[] Terminator { get { return terminator; } }

            public Header ( string magic, byte[] terminator, int lastByte )
            {
                this.magic = magic.ToLower ();
                this.lastByte = lastByte;
                this.terminator = terminator;
            }

            public bool IsTerminatorValid ()
            {
                return StructuralEqualityComparer.Equals ( NativeFormat.Terminator,
                    Terminator );
            }

            /// <summary>
            /// Most native format's magic follow the pattern 'aXY' where XY indicates, for
            /// non 8bpp formats, the bpp (01, 16 or 32). For 8bpp formats, XY ar two 
            /// characters.
            /// </summary>
			public int ParseBitsPerPixelFromMagic ()
            {
                int bpp;

                if ( int.TryParse ( Magic.Substring ( 1, 2 ), out bpp ) )
                {
                    return bpp;
                }
                else
                {
                    return 8;
                }
            }

        }
    }
}
