using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

using System.IO;

namespace BennuLib.Bennu.IO
{
	public class NativeFormatReader : BinaryReader
	{


		private static readonly System.Text.Encoding _encoding = System.Text.Encoding.GetEncoding(850);
		public NativeFormatReader(Stream input) : base(input, _encoding)
		{
		}

		public string ReadDescription()
		{
			return _encoding.GetString(ReadBytes(32));
		}

		public Magic ReadMagic()
		{
			// 3 first bytes describe the depth of the MAP
			var fileType = _encoding.GetString(ReadBytes(3));
			// Next 4 bytes are MS-DOS termination, and last is the MAP version
			var descriptor = ReadBytes(4);
			var version = ReadByte();

			return new Magic(fileType, version, descriptor);
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
			var numberPivotPoints = Convert.ToInt16(flags & NativeFormat.NumberOfControlPointsBitMask);
			return numberPivotPoints;
		}

		public GlyphInfo ReadGlyphInfo()
		{
			return new GlyphInfo(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());
		}
	}
}
