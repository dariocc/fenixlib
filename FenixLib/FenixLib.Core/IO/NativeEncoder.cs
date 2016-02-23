using System.IO.Compression;
using System.IO;

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

            using ( NativeFormatWriter writer = new NativeFormatWriter ( output ) )
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
