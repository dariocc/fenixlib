using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static BennuLib.IO.NativeFormat;

namespace BennuLib.IO
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
			var descriptor = ReadBytes(4);
			var version = ReadByte();

			return new Header(fileType, version, descriptor);
		}

		public byte[] ReadPalette()
		{
			var paletteColors = ReadBytes(NativeFormat.PaletteSize - 1);
			return paletteColors;
		}

		public PivotPoint[] ReadPivotPoints(int number)
		{
			List<PivotPoint> points = new List<PivotPoint>();
			for (int n = 0; n <= number - 1; n++) {
				PivotPoint point = new PivotPoint(n, ReadInt16(), ReadInt16());
				// Two coods set to -1 mean invalid point
				if (!(point.X == -1 & point.Y == -1)) {
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

		public GlyphInfo ReadGlyphInfo()
		{
			return new GlyphInfo(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
		}
	}
}
