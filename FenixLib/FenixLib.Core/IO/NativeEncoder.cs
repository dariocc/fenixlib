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
using System.IO.Compression;
using System.IO;
using FenixLib.Core;

namespace FenixLib.IO
{
    public abstract class NativeEncoder<T> : IEncoder<T>
    {

        protected abstract byte GetLastHeaderByte ( T what );

        protected abstract void WriteNativeFormatBody ( T what, NativeFormatWriter writer );

        protected virtual void WriteNativeFormatHeader ( T what, NativeFormatWriter writer )
        {
            writer.WriteAsciiZ ( GetFileMagic ( what ).Substring ( 0, 3 ), 3 );
            writer.Write ( NativeFormat.Terminator );
            writer.Write ( GetLastHeaderByte ( what ) );
        }

        protected abstract string GetFileMagic ( T what );

        public CompressionOptions Compression { get; }

        public NativeEncoder () : this ( CompressionOptions.Uncompressed ) { }

        public NativeEncoder ( CompressionOptions compressionOptions )
        {
            Compression = compressionOptions;
        }

        public void Encode ( T what, Stream output )
        {
            Stream nativeStream = null;

            if ( Compression == CompressionOptions.Uncompressed )
            {
                nativeStream = output;
            }
            else
            {
                nativeStream = new GZipStream ( output, ( CompressionLevel ) Compression );
            }

            using ( NativeFormatWriter writer = new NativeFormatWriter ( nativeStream ) )
            {
                WriteNativeFormatHeader ( what, writer );
                WriteNativeFormatBody ( what, writer );
            }
        }

        public enum CompressionOptions
        {
            Uncompressed = -1,
            Fastest = CompressionLevel.Fastest,
            Optimal = CompressionLevel.Optimal,
            NoCompression = CompressionLevel.NoCompression
            // Todo: Is it the same as Uncompressed?
        }
    }
}
