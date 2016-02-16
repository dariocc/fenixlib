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
using System.IO.Compression;

namespace BennuLib.Bennu.IO
{
	public abstract class NativeDecoder<T> : IDecoder<T>
	{

		public abstract int MaxSupportedVersion { get; }
		protected abstract T ReadNativeFormat(Magic magic, NativeFormatReader reader);
		protected abstract string[] KnownFileIds { get; }
		protected abstract string[] KnownFileExtensions { get; }

		public IEnumerable<string> SupportedExtensions {
			get { return KnownFileExtensions; }
		}

		protected static bool IsGZip(byte[] header)
		{
			return header.Length >= 2 & header[0] == 31 & header[1] == 139;
		}

		public T Decode(Stream input)
		{
			// TODO: Do we need to check if I can seek? Should we make the function protected?
			byte[] buff = new byte[2];
			if (!(input.Read(buff, 0, 2) == 2)) {
				throw new IOException();
			}

			input.Position = 0;

			Stream stream = null;
			if (IsGZip(buff)) {
				stream = new GZipStream(input, CompressionMode.Decompress);
			} else {
				stream = input;
			}

			using (NativeFormatReader reader = new NativeFormatReader(stream)) {
				var magic = reader.ReadMagic();

				if (!magic.IsValid)
					throw new UnsuportedFileFormatException();
				if (magic.Version > MaxSupportedVersion)
					throw new UnsuportedFileFormatException();

				return ReadNativeFormat(magic, reader);
			}

		}

		protected static IPixel[] CreatePixelBuffer(int depth, byte[] graphicData)
		{
			switch (depth) {
				case 8:
					return IndexedPixel.CreateBufferFromBytes(graphicData);
				case 16:
					return Int16Pixel565.CreateBufferFromBytes(graphicData);
				case 32:
					return Int32PixelARGB.CreateBufferFromBytes(graphicData);
				default:
					// TODO: Customize
					throw new ArgumentException();
			}
		}

		protected static Palette.Color[] VGAtoColors(byte[] colorData)
		{
			Palette.Color[] colors = new Palette.Color[colorData.Length / 3];
			for (n = 0; n <= colors.Length - 1; n++) {
				colors[n] = new Palette.Color(colorData[n * 3] << 2, colorData[n * 3 + 1] << 2, colorData[n * 3 + 1] << 2);
			}
			return colors;
		}

		public bool TryDecode(Stream input, ref T decoded)
		{
			try {
				decoded = Decode(input);
				return true;
			} catch (Exception ex) {
				return false;
			}
		}
	}
}
