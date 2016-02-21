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
				// Two coods set to -1 mean invalid point
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

        
        public AbstractPixel[] ReadPixels(int depth, int width, int height)
        {
            int byteLength = width * height * depth / 8; // TODO: Shall e adjusted for monochrome
            byte[] graphicData = ReadBytes(byteLength);

            switch (depth)
            {
                case 1:
                    return ReadPixelMonochrome(graphicData);
                case 8:
                    return ReadPixelRgbIndexed(graphicData);
                case 16:
                    return ReadPixelsRgbInt16(graphicData);
                case 32:
                    return ReadPixelsArgbInt32(graphicData);
                default:
                    throw new ArgumentException(); // TODO: Customize
            }
        }

        private IndexedPixel[] ReadPixelMonochrome(byte[] graphicData)
        {
            throw new NotImplementedException(); // TODO
        }

        private IndexedPixel[] ReadPixelRgbIndexed(byte[] graphicData)
        {
            IndexedPixel[] pixels = new IndexedPixel[graphicData.Length];
            for (var n = 0; n <= pixels.Length - 1; n++)
            {
                pixels[n] = new IndexedPixel(graphicData[n]);
            }
            return pixels;
        }

        private Int16Pixel565[] ReadPixelsRgbInt16(byte[] graphicData)
        {
            Int16Pixel565[] pixels = new Int16Pixel565[graphicData.Length / 2];
            for (var n = 0; n <= pixels.Length - 1; n += 3)
            {
                pixels[n] = new Int16Pixel565(graphicData[n], 
                    graphicData[n + 1], 
                    graphicData[n + 2]);
            }
            return pixels;
        }

        private Int32PixelARGB[] ReadPixelsArgbInt32(byte[] graphicData)
        {
            Int32PixelARGB[] buffer = new Int32PixelARGB[graphicData.Length / 4];

            for (var n = 0; n <= buffer.Length - 1; n++)
            {
                buffer[n] = new Int32PixelARGB(graphicData[n], 
                    graphicData[n + 1], 
                    graphicData[n + 2],
                    graphicData[n + 4]);
            }
            return buffer;
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
