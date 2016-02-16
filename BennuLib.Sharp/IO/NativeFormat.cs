using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace BennuLib.Bennu.IO
{
	public static class NativeFormat
	{
		public static readonly byte[] NativeDescriptor = {
			0x1a,
			0xd,
			0xa,
			0x0
		};
			// Size in bytes of a palette
		public const short PaletteSize = 768;
		public const int ReservedBytesSize = 576;
		[Obsolete()]
			// Bit mask for the number of control points of
		public const int NumberOfControlPointsBitMask = 0xfff;
		[Obsolete()]
			// Bit mask for enabled/disabled animation of
		public const int AnimationFlagBitMask = 0x1000;

		public sealed class Magic
		{
			// TODO: Think of a different way to handle this. Does not seem good that we need to
			// repeat this in the native decoders and in the Magic class
			public static string[] ValidFormats { get; }

			public string FileType { get; }
			public int Version { get; }

			public readonly byte[] Descriptor = new byte[5];
			// TODO: Check for known types?
			public Magic(string fileType, int version, byte[] descriptor)
			{
				this.FileType = fileType.ToLower();
				this.Version = version;
				this.Descriptor = descriptor;
			}

			public bool IsDescriptorValid {
				get { return StructuralComparisons.StructuralEqualityComparer.Equals(NativeFormat.NativeDescriptor, Descriptor); }
			}

			public int Depth {
				get {
					int functionReturnValue = 0;
					if (int.TryParse(FileType.Substring(1, 2), out Depth)) {
						return functionReturnValue;
					} else {
						return 8;
					}
					return functionReturnValue;
				}
			}

			public bool IsRecognizedFileType {
				get { return Array.IndexOf(ValidFormats, FileType) >= 0; }
			}

			public bool IsValid {
				get { return IsRecognizedFileType & IsDescriptorValid; }
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
				this.Width = width;
				this.Height = height;
				this.YOffset = yOffset;
				this.FileOffset = fileOffset;
			}
		}
	}
}
