using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static Bennu.IO.NativeFormat;

namespace Bennu.IO
{
	public class NativeFormatReader : BinaryReader
	{
		private static readonly Encoding _encoding = Encoding.GetEncoding(850);

		public NativeFormatReader(Stream input) : base(input, _encoding)
		{
		}

        public string ReadAsciiZ(int length)
        {
            byte[] bytes = ReadBytes(length);

            int n = 0;
            for ( n = 0; n < length; n++ )
            { 
                if (bytes[n] == 0)
                    break;
            }

            string result;
            if (n > 0) {
                byte[] trimmedBytes = new byte[n];
                Array.Copy(bytes, 0, trimmedBytes, 0, n);
                result = _encoding.GetString(trimmedBytes);
            } else
            {
                result = "";
            }

            return result;
        }

		public Header ReadHeader()
		{
			// 3 first bytes describe the depth of the MAP
			var fileType = _encoding.GetString(ReadBytes(3));
			// Next 4 bytes are MS-DOS termination, and last is the MAP version
			var terminator = ReadBytes(4);
            // Last byte of the header is the version number
			var version = ReadByte();

			return new Header(fileType, terminator, version);
		}

		public Palette ReadPalette()
		{
			var paletteColors = ReadBytes(PaletteBytesSize);
            Palette palette = Palette.Create ( Vga2PaleetteColors ( paletteColors ) );

            return palette;
		}

		public PivotPoint[] ReadPivotPoints(int number)
		{
			List<PivotPoint> points = new List<PivotPoint>();
			for (int n = 0; n < number; n++)
            {
				PivotPoint point = new PivotPoint(n, ReadInt16(), ReadInt16());
				
                // If the X and Y are -1, there is no need to add
				if ( !(point.X == -1 & point.Y == -1) )
                {
					points.Add(point);
				}
			}
			return points.ToArray();
		}

		public byte[] ReadUnusedPaletteGamma()
		{
			return ReadBytes(NativeFormat.ReservedBytesSize);
		}

		public int ReadPivotPointsNumber()
		{
			var flags = ReadUInt16();
			var numberPivotPoints = Convert.ToInt16(flags & PivotPointsNumberBitMask);
			return numberPivotPoints;
		}

        public int ReadPivotPointsNumberLong()
        {
            var flags = ReadUInt32();
            var numberPivotPoints = Convert.ToInt16(flags & PivotPointsNumberBitMask);
            return numberPivotPoints;
        }

        public GlyphInfo ReadLegacyFntGlyphInfo()
        {
            return new GlyphInfo(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
        }

        public GlyphInfo ReadExtendedFntGlypInfo()
        {
            return new GlyphInfo ( ReadInt32 (), ReadInt32 (), ReadInt32 (),
                ReadInt32 (), ReadInt32 (), ReadInt32 (), ReadInt32 () );
        }

        
        public byte[] ReadPixels(int depth, int width, int height)
        {
            int byteLength;

            if (depth == 1)
            {
                int rowByteSize = ( width + ( ( width - ( width % 8 ) ) & 7 ) ) / 8;
                byteLength = rowByteSize * height;
            }
            else
            {
                byteLength = width * height * depth / 8;
            }
            
            return ReadBytes(byteLength);
        }

        private static Color[] Vga2PaleetteColors ( byte[] colorData )
        {
            Color[] colors = new Color[colorData.Length / 3];
            for ( var n = 0 ; n <= colors.Length - 1 ; n++ )
            {
                colors[n] = new Color (
                    colorData[n * 3] << 2,
                    colorData[n * 3 + 1] << 2,
                    colorData[n * 3 + 1] << 2 );
            }
            return colors;
        }
    }
}
