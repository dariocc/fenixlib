using System.IO.Compression;
using System.IO;

namespace Bennu.IO
{
    public abstract class NativeEncoder<T> : IEncoder<T>
    {

        protected abstract byte LastHeaderByte { get; }

        protected abstract void WriteNativeFormatBody ( T obj, NativeFormatWriter writer );

        protected abstract string GetFileId ( T obj );

        public CompressionOptions Compression { get; }

        public NativeEncoder () : this ( CompressionOptions.Uncompressed )
        {
        }

        public NativeEncoder ( CompressionOptions compressionOptions )
        {
            Compression = compressionOptions;
        }

        public void Encode ( T obj, Stream output )
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
                writer.WriteAsciiZ ( GetFileId ( obj ).Substring ( 0, 3 ), 3 );
                writer.Write ( NativeFormat.Terminator );
                writer.Write ( LastHeaderByte );
                WriteNativeFormatBody ( obj, writer );
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
