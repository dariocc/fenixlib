using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;

using static Bennu.IO.NativeFormat;

namespace Bennu.IO
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
        /// Decodes the body of the native format and returns a <see cref="Bennu"/> base 
        /// type.
        /// </summary>
        /// <param name="header">A header object containing information of the magic, terminator
        /// and version.</param>
        /// <param name="reader">A <see cref="NativeFormatReader"/> that is used to read the
        /// stream</param>
        /// <returns></returns>
        protected abstract T ReadBody ( Header header, NativeFormatReader reader );

        /// <summary>
        /// The list of magic
        /// </summary>
        protected abstract string[] KnownFileMagics { get; }

        /// <summary>
        /// The list of extensions that the Decoder expects to be capable of reading.
        /// </summary>
        protected abstract string[] KnownFileExtensions { get; }

        public IEnumerable<string> SupportedExtensions
        {
            get { return KnownFileExtensions; }
        }

        /// <summary>
        /// GZip files start always with bytes {31, 139}. This function
        /// will check if the argument matches that criteria.
        /// </summary>
        /// <param name="header">An array of bytes containing at least two elements</param>
        /// <returns>True if the first two bytes <paramref name="header"/> are 
        /// that of GZip format.</returns>
		protected static bool HeaderIsGZip ( byte[] header )
        {
            return header.Length >= 2 & header[0] == 31 & header[1] == 139;
        }


        protected virtual bool ValidateHeaderMagic ( string magic, Header header )
        {
            return KnownFileMagics.Contains ( magic );
        }

        protected virtual bool ValidateHeaderTerminator ( byte[] terminator, Header header )
        {
            return header.IsTerminatorValid ();
        }

        protected virtual bool ValidateHeaderVersion ( int version, Header header )
        {
            return version <= MaxSupportedVersion;
        }

        /// <summary>
        /// Decodes the stream and returns a <see cref="Bennu"/> base type.
        /// </summary>
        /// <param name="input">The stream from which to read.</param>
        /// <returns>A <see cref="Bennu"/> base type.</returns>
		public T Decode ( Stream input )
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

            Header header;
            Stream bodyStream = null;

            byte[] headerBytes = new byte[8];

            if ( input.Read ( headerBytes, 0, 2 ) != 2 )
                throw new UnsuportedFileFormatException (); // TODO: Customize

            if ( HeaderIsGZip ( headerBytes ) )
            {
                // Concatenate a memory stream containing a GZipHeader with the input stream
                // so as to be able to use a GZipStream to read the bytes of the body
                using ( var GzipHeaderStream = new MemoryStream ( new byte[] { 31, 139 } ) )
                {
                    Stream concatenated = new ConcatenatedStream ( GzipHeaderStream, input );
                    bodyStream = new GZipStream ( concatenated, CompressionMode.Decompress );

                    if ( bodyStream.Read ( headerBytes, 0, 8 ) != 8 )
                        throw new UnsuportedFileFormatException ();
                }
            }
            else
            {
                // Read the remaining 6 bytes of the header
                if ( input.Read ( headerBytes, 2, 6 ) != 6 )
                    throw new UnsuportedFileFormatException ();

                bodyStream = input;
            }

            using ( MemoryStream headerStream = new MemoryStream ( headerBytes ) )
            {
                using ( NativeFormatReader reader = new NativeFormatReader ( headerStream ) )
                {
                    header = reader.ReadHeader ();

                    if ( !ValidateHeaderMagic ( header.Magic, header ) )
                        throw new UnsuportedFileFormatException (); // TODO: Customize

                    if ( !ValidateHeaderTerminator ( header.Terminator, header ) ) // TODO: Customize
                        throw new UnsuportedFileFormatException ();

                    if ( !ValidateHeaderVersion ( header.LastByte, header ) ) // TODO: Customize
                        throw new UnsuportedFileFormatException ();
                }
            }

            using ( NativeFormatReader reader = new NativeFormatReader ( bodyStream ) )
            {
                return ReadBody ( header, reader );
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
        public bool TryDecode ( Stream input, out T decoded )
        {
            try
            {
                decoded = Decode ( input );
                return true;
            }
            catch ( Exception )
            {
                decoded = default ( T );
                return false;
            }
        }

        private class ConcatenatedStream : Stream
        {
            private Stream stream1;
            private Stream stream2;
            private int _position;
            private int _stream1Length;

            public override bool CanRead { get; } = true;

            public override bool CanSeek { get; } = false;

            public override bool CanWrite { get; } = false;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stream1">Stream1 needs to support the Length property and it
            /// cannot be longer than sizeof(int). Stream 2.</param>
            /// <param name="stream2"></param>
            public ConcatenatedStream ( Stream stream1, Stream stream2 )
            {
                _position = 0;

                if ( stream1.Length > sizeof ( int ) )
                    throw new ArgumentException (); // TODO:Customize

                _stream1Length = ( int ) stream1.Length;

                this.stream1 = stream1;
                this.stream2 = stream2;
            }

            public override long Length
            {
                get
                {
                    throw new NotSupportedException ();
                }
            }

            public override long Position
            {
                get
                {
                    return _position;
                }

                set
                {
                    throw new NotSupportedException ();
                }
            }

            public override void Flush ()
            {
                throw new NotSupportedException ();
            }

            public override int Read ( byte[] buffer, int offset, int count )
            {
                byte[] bytes = new byte[count];

                int bytesToReadFromStream1 = 0;
                int bytesReadFromStream1 = 0;
                int bytesReadFromStream2 = 0;

                if ( Position < _stream1Length )
                {
                    if ( stream1.Length != _stream1Length )
                        throw new InvalidOperationException (); // TODO: Customize: redimensionable stream1 is not supported

                    int bytesLeftFromStream1 = _stream1Length - ( int ) stream1.Position;
                    bytesToReadFromStream1 = Math.Min ( bytesLeftFromStream1, count );
                    bytesReadFromStream1 = stream1.Read ( bytes, 0, bytesToReadFromStream1 );
                }

                if ( bytesReadFromStream1 < count && bytesReadFromStream1 == bytesToReadFromStream1 )
                {
                    bytesReadFromStream2 = stream2.Read ( bytes, bytesReadFromStream1,
                        count - bytesReadFromStream1 );
                }

                bytes.CopyTo ( buffer, offset );

                _position = bytesReadFromStream1 + bytesReadFromStream2;
                return _position;
            }

            public override long Seek ( long offset, SeekOrigin origin )
            {
                throw new NotSupportedException ();
            }

            public override void SetLength ( long value )
            {
                throw new NotSupportedException ();
            }

            public override void Write ( byte[] buffer, int offset, int count )
            {
                throw new NotSupportedException ();
            }
        }
    }
}
