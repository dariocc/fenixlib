using System;
using static System.Collections.StructuralComparisons;

namespace BennuLib.IO
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
		public const int PaletteSize = 768;

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
        /// Represents the header of the native formats, i.e. a section describing type
        /// type of file (graphic, graphic collection, font or palettes), and the depth
        /// of the graphic information (1, 8, 16 or 32bpp).
        /// </summary>
		public sealed class Header
		{

			public string Magic { get; }
			public int Version { get; }

			public readonly byte[] Descriptor = new byte[5];
			
			public Header(string magic, int version, byte[] terminator)
			{
				Magic = magic.ToLower();
				Version = version;
				Descriptor = terminator;
			}

			public bool IsTerminatorValid() {
                    return StructuralEqualityComparer.Equals(Terminator, Descriptor);
			}

            /// <summary>
            /// All native format's magic follow the pattern 'aXY' where XY indicates, for
            /// non 8bpp formats, the depth (01, 16 or 32). For 8bpp formats, XY ar two 
            /// characters.
            /// </summary>
			public int Depth
            {
                // TODO: Perhaps it is wiser to have a ParseDepth instead of a property...
                get
                {
                    int depth;

                    if (int.TryParse(Magic.Substring(1, 2), out depth))
                        return depth;
                    else
                        return 8;
                }
            }

		}

		public struct GlyphInfo
		{
			public int Width { get; }
			public int Height { get; }
			public int YOffset { get; }
			// Vertical displacement
			public int FileOffset { get; }
			// Offset of the graphic in the file

			public GlyphInfo(int width, int height, int yOffset, int fileOffset)
			{
				Width = width;
				Height = height;
				YOffset = yOffset;
				FileOffset = fileOffset;
			}
		}
	}
}
