using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;

using static BennuLib.IO.NativeFormat;

namespace BennuLib.IO
{
    /// <summary>
    /// <see cref="NativeDecoder{T}"/> base class for all native formats decoders. It defines
    /// common behaviour and defines a set of template methods that derivated classes must
    /// implement.
    /// </summary>
    /// <remarks>
    /// All native formats for fonts, graphics, graphic collections and palettes have 
    /// similarities in how they are read from disk:
    /// <list type="bullet">
    ///     <item>Might or might not be compressed in GZip format</item>
    ///     <item>Have a magic indicating the type of the file</item>
    ///     <item>Have an specific sequence of bytes called terminator</item>
    ///     <item>Have a byte indicating the version of the file</item>
    /// </list>
    /// The <see cref="NativeDecoder{T}"/> takes care of the communalities and leave the
    /// read of the body to the derivated classes.
    /// </remarks>
    /// <typeparam name="T">The </typeparam>
	public abstract class NativeDecoder<T> : IDecoder<T>
	{
        /// <summary>
        /// The highest version number that the decoder expects to be capable of reading.
        /// </summary>
        public abstract int MaxSupportedVersion { get; }
        
        /// <summary>
        /// Decodes the body of the native format and returns a <see cref="BennuLib"/> base 
        /// type.
        /// </summary>
        /// <param name="header">A header object containing information of the magic, terminator
        /// and version.</param>
        /// <param name="reader">A <see cref="NativeFormatReader"/> that is used to read the
        /// stream</param>
        /// <returns></returns>
        protected abstract T ReadBody(Header header, NativeFormatReader reader);

        /// <summary>
        /// The list of magic
        /// </summary>
        protected abstract string[] KnownFileMagics { get; }
        
        /// <summary>
        /// The list of extensions that the Decoder expects to be capable of reading.
        /// </summary>
		protected abstract string[] KnownFileExtensions { get; }

		public IEnumerable<string> SupportedExtensions {
			get { return KnownFileExtensions; }
		}

        /// <summary>
        /// GZip files start always with bytes {31, 139}. This function
        /// will check if the argument matches that criteria.
        /// </summary>
        /// <param name="header">An array of bytes containing at least two elements</param>
        /// <returns>True if the first two bytes <paramref name="header"/> are 
        /// that of GZip format.</returns>
		protected static bool HeaderIsGZip(byte[] header)
		{
			return header.Length >= 2 & header[0] == 31 & header[1] == 139;
		}


        protected virtual bool ValidateMagic(string magic)
        {

        }

        protected virtual bool ValidateTerminator()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        protected virtual bool ValidateHeader(Header header)
        {
            return ( KnownFileMagics.Contains(header.Magic) 
                && header.IsTerminatorValid()
                && header.Version <= MaxSupportedVersion );
        }

        /// <summary>
        /// Decodes the stream and returns a <see cref="BennuLib"/> base type.
        /// </summary>
        /// <param name="input">The stream from which to read.</param>
        /// <returns>A <see cref="BennuLib"/> base type.</returns>
		public T Decode(Stream input)
		{

            /* Native formats support GZip compression transparently
             * This function will read the first two bytes of the file and determine if it 
             * is a GZip file, in which case it will use a GZipStream object to read it.
             */

            /* Remove when function is verified
			byte[] buff = new byte[2];

			if ( !(input.Read(buff, 0, 2) == 2) ) {
				throw new IOException();
			}
            
            input.Position = 0; // This will not work with every type of stream

			Stream stream = null;
			if ( HeaderIsGZip(buff) ) {
				stream = new GZipStream(input, CompressionMode.Decompress);
			} else {
				stream = input;
			}
            */

            Stream stream = null;
            Header header;
            using (var memory = new MemoryStream(8))
            {
                input.CopyTo(memory, 8); // input will remain at Position 8
                memory.Flush();

                memory.Position = 0;

                byte[] firstBytes = new byte[2];

                if (!(memory.Read(firstBytes, 0, 2) == 2))
                {
                    throw new IOException();
                }

                if (HeaderIsGZip(firstBytes))
                {
                    stream = new GZipStream(input, CompressionMode.Decompress);
                }
                else
                {
                    stream = input;
                }

                // The native format header is read from the buffered memory, since
                // we cannot be sure that the original input stream can be rewind.
                memory.Position = 0;
                using (NativeFormatReader reader = new NativeFormatReader(memory))
                {
                    header = reader.ReadHeader();

                    if ( ! ValidateHeader(header) )
                        throw new UnsuportedFileFormatException();
                }
            }

            using (NativeFormatReader reader = new NativeFormatReader(stream))
            {
                return ReadBody(header, reader);
            }

		}

        /// <summary>
        /// Attempts to decode <paramref name="input"/> into <paramref name="decoded"/>.
        /// Returns whether the operation succeded or not.
        /// </summary>
        /// <param name="input">The input from which to read.</param>
        /// <param name="decoded">When this method returns, the result of decoding the 
        /// <see cref="Stream"/>. If the decoding fails, <paramref name="decoded"/> will
        /// contain the default value of <typeparamref name="T"/>.</param>
        /// <returns>True if the decoding was successful. Otherwise false.</returns>
        public bool TryDecode(Stream input, out T decoded)
        {
            try
            {
                decoded = Decode(input);
                return true;
            }
            catch (Exception)
            {
                decoded = default(T);
                return false;
            }
        }

        // TODO: Probably needs to be moved outside this class
        protected static Palette.Color[] VGAtoColors(byte[] colorData)
		{
			Palette.Color[] colors = new Palette.Color[colorData.Length / 3];
			for (var n = 0; n <= colors.Length - 1; n++) {
				colors[n] = new Palette.Color(
                    colorData[n * 3] << 2, 
                    colorData[n * 3 + 1] << 2, 
                    colorData[n * 3 + 1] << 2);
			}
			return colors;
		}
	}
}
