/*  Copyright 2016 Darío Cutillas Carrillo
*
*   Licensed under the Apache License, Version 2.0 (the "License");
*   you may not use this file except in compliance with the License.
*   You may obtain a copy of the License at
*
*       http://www.apache.org/licenses/LICENSE-2.0
*
*   Unless required by applicable law or agreed to in writing, software
*   distributed under the License is distributed on an "AS IS" BASIS,
*   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*   See the License for the specific language governing permissions and
*   limitations under the License.
*/
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
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
        /// Decodes the body of the native format and returns a <see cref="FenixLib"/> base
        /// type.
        /// </summary>
        /// <param name="header">A header object containing information of the magic, terminator
        /// and version.</param>
        /// <param name="reader">A <see cref="BinaryNativeFormatReader"/> that is used to read the
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
        /// Decodes the stream and returns a <see cref="FenixLib"/> base type.
        /// </summary>
        /// <param name="input">The stream from which to read.</param>
        /// <returns>A <see cref="FenixLib"/> base type.</returns>
		public T Decode ( Stream input )
        {
            if ( input == null )
            {
                throw new ArgumentNullException ( nameof ( input ) );
            }

            /* Native formats support GZip compression transparently
             * This function will read the first two bytes of the file and determine if it
             * is a GZip file, in which case it will use a GZipStream object to read it.
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
                using ( var reader = CreateNativeFormatReader ( headerStream ) )
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

            using ( var reader = new BinaryNativeFormatReader ( bodyStream ) )
            {
                return ReadBody ( header, reader );
            }

        }

        /// <summary>
        /// Creates an <see cref="NativeFormatReader"/>.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method is necessary to avoid dependency on the <see cref="BinaryNativeFormatReader"/>
        /// class, which is not advisable for unit-testing.
        /// </remarks>
        protected virtual NativeFormatReader CreateNativeFormatReader ( Stream stream )
        {
            // An alternative to this template method would be an optional parameter
            // with a factory object that creates the NativeFormatReader
            return new BinaryNativeFormatReader ( stream );
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
    }
}
