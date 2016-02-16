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
	public class NativeFormatWriter : BinaryWriter
	{


		private static readonly System.Text.Encoding _encoding = System.Text.Encoding.GetEncoding(850);
		public NativeFormatWriter(Stream input) : base(input, _encoding)
		{
		}

		private void WriteHeader(string formatHeader, byte version)
		{
			if (formatHeader.Length != 3) {
				throw new ArgumentException();
				// TODO: Customize
			}

			base.Write(_encoding.GetBytes(formatHeader));
			base.Write(NativeFormat.NativeDescriptor);
			base.Write(version);
		}

		public void WriteAsciiZ(string text, int maxLength)
		{
			// Texts are encoded in ASCIZZ format
			var clippedText = text.Substring(0, Math.Min(text.Length, maxLength));
			var bytes = _encoding.GetBytes(clippedText.ToCharArray());
			Array.Resize(ref bytes, maxLength);
			base.Write(bytes);
		}

		public void Write(Palette palette)
		{
			byte[] bytes = new byte[palette.Colors.Length * 3];
			for (n = 0; n <= palette.Colors.Length; n++) {
				bytes[n * 3] = Convert.ToByte(palette[n].r);
				bytes[n * 3 + 1] = Convert.ToByte(palette[n].g);
				bytes[n * 3 + 2] = Convert.ToByte(palette[n].b);
			}

			base.Write(bytes);
		}

		public void Write(IEnumerable<PivotPoint> pivotPoints)
		{
			var ids = from p in pivotPoints select p.Id;

			if (ids.Count() == 0)
				return;

			PivotPoint[] pivotPointsIncludingUndefined = new PivotPoint[ids.Max()];
			for (n = 0; n <= ids.Max(); n++) {
				var id = n;
				var p = pivotPoints.Where(x => x.Id == id).FirstOrDefault();
				pivotPointsIncludingUndefined[n] = p == null ? new PivotPoint(id, -1, -1) : new PivotPoint(id, p.Value.X, p.Value.Y);
			}

			foreach (PivotPoint pivotPoint_loopVariable in pivotPointsIncludingUndefined) {
				pivotPoint = pivotPoint_loopVariable;
				FileSystem.Write(Convert.ToInt16(pivotPoint.X));
				FileSystem.Write(Convert.ToInt16(pivotPoint.Y));
			}
		}

		public void Write(Int32PixelARGB[] pixels)
		{
			foreach (Int32PixelARGB pixel_loopVariable in pixels) {
				pixel = pixel_loopVariable;
				FileSystem.Write(pixel.Value);
			}
		}

		public void Write(Int16Pixel565[] pixels)
		{
			foreach (Int16Pixel565 pixel_loopVariable in pixels) {
				pixel = pixel_loopVariable;
				FileSystem.Write(Convert.ToUInt16(pixel.Value));
			}
		}

		public void Write(IndexedPixel[] pixels)
		{
			foreach (IndexedPixel pixel_loopVariable in pixels) {
				pixel = pixel_loopVariable;
				FileSystem.Write(Convert.ToByte(pixel.Value));
			}
		}

		public void WriteReservedPaletteGammaSection()
		{
			byte[] bytes = new byte[NativeFormat.ReservedBytesSize];
			base.Write(bytes);
		}
	}
}
