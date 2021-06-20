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
using System.Collections.Generic;
using System.IO;
using System.Text;
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    /// <summary>
    /// Reads primitive and native format data types in a little-endian format.
    /// </summary>
    /// <remarks>
    /// This class uses internally a <seealso cref="BinaryReader"/>.
    /// </remarks>
    public sealed class BinaryNativeFormatReader : NativeFormatReader
    {
        private static readonly Encoding encoding = CodePagesEncodingProvider.Instance.GetEncoding ( 850 );

        private BinaryReader binaryReader;

        public BinaryNativeFormatReader ( Stream input ) : base ( input )
        {
            binaryReader = new BinaryReader ( input );
        }

        public override string ReadAsciiZ ( int length )
        {
            byte[] bytes = binaryReader.ReadBytes ( length );

            if (bytes.Length < length )
            {
                throw new EndOfStreamException ();
            }

            int n = 0;
            for ( n = 0 ; n < length ; n++ )
            {
                if ( bytes[n] == 0 )
                    break;
            }

            string result;
            if ( n > 0 )
            {
                byte[] trimmedBytes = new byte[n];
                Array.Copy ( bytes, 0, trimmedBytes, 0, n );
                result = encoding.GetString ( trimmedBytes );
            }
            else
            {
                result = "";
            }

            return result;
        }

        public override Header ReadHeader ()
        {
            // 3 first bytes describe the graphic format of the MAP
            var fileType = encoding.GetString ( binaryReader.ReadBytes ( 3 ) );
            // Next 4 bytes are MS-DOS termination, and last is the MAP version
            var terminator = binaryReader.ReadBytes ( 4 );
            // Last byte of the header is the version number
            var lastByte = binaryReader.ReadByte ();

            return new Header ( fileType, terminator, lastByte );
        }

        public override Palette ReadPalette ()
        {
            var paletteColors = binaryReader.ReadBytes ( PaletteBytesSize );

            if (paletteColors.Length < PaletteBytesSize)
            {
                throw new EndOfStreamException ();
            }

            try
            { 
                Palette palette = new Palette ( Vga2PaleetteColors ( paletteColors ) );
                return palette;
            }
            catch (Exception e)
            {
                throw new IOException ( "Invalid palette data.", e );
            }
        }

        public override PivotPoint[] ReadPivotPoints ( int number )
        {
            List<PivotPoint> points = new List<PivotPoint> ();
            for ( int n = 0 ; n < number ; n++ )
            {
                var x = binaryReader.ReadInt16 ();
                var y = binaryReader.ReadInt16 ();
                PivotPoint point = new PivotPoint ( n, x, y );

                // If the X and Y are -1, there is no need to add
                if ( !( point.X == -1 & point.Y == -1 ) )
                {
                    points.Add ( point );
                }
            }
            return points.ToArray ();
        }

        public override byte[] ReadPaletteGammas ()
        {
            var bytes = binaryReader.ReadBytes ( PaletteGammaBytesSize );
            if (bytes.Length < PaletteGammaBytesSize)
            {
                throw new EndOfStreamException ();
            }

            return bytes;
        }

        public override int ReadPivotPointsMaxIdUint16 ()
        {
            var flags = binaryReader.ReadUInt16 ();
            var numberPivotPoints = Convert.ToInt16 ( flags & PivotPointsNumberBitMask );
            return numberPivotPoints;
        }

        public override int ReadPivotPointsMaxIdInt32 ()
        {
            var flags = binaryReader.ReadUInt32 ();
            var numberPivotPoints = Convert.ToInt16 ( flags & PivotPointsNumberBitMask );
            return numberPivotPoints;
        }

        public override GlyphInfo ReadLegacyFntGlyphInfo ()
        {
            return new LegacyGlyphInfoBuilder ()
                .Width ( binaryReader.ReadInt32 () )
                .Height ( binaryReader.ReadInt32 () )
                .YOffset ( binaryReader.ReadInt32 () )
                .FileOffset ( binaryReader.ReadInt32 () )
                .Build ();
        }

        public override GlyphInfo ReadExtendedFntGlyphInfo ()
        {
            return new ExtendedGlyphInfoBuilder ()
                .Width ( binaryReader.ReadInt32 () )
                .Height ( binaryReader.ReadInt32 () )
                .XAdvance ( binaryReader.ReadInt32 () )
                .YAdvance ( binaryReader.ReadInt32 () )
                .XOffset ( binaryReader.ReadInt32 () )
                .YOffset ( binaryReader.ReadInt32 () )
                .FileOffset ( binaryReader.ReadInt32 () )
                .Build ();
        }

        public override byte[] ReadPixels ( GraphicFormat format, int width, int height )
        {
            var size = format.PixelsBytesForSize ( width, height );
            var bytes = binaryReader.ReadBytes ( size );

            if ( bytes.Length < size )
            {
                throw new EndOfStreamException ();
            }

            return bytes;
        }

        private static PaletteColor[] Vga2PaleetteColors ( byte[] colorData )
        {
            PaletteColor[] colors = new PaletteColor[colorData.Length / 3];
            for ( var n = 0 ; n <= colors.Length - 1 ; n++ )
            {
                colors[n] = new PaletteColor (
                    colorData[n * 3] << 2,
                    colorData[n * 3 + 1] << 2,
                    colorData[n * 3 + 2] << 2 );
            }
            return colors;
        }

        public override int ReadUInt16 () => binaryReader.ReadUInt16 ();

        public override int ReadInt32 () => binaryReader.ReadInt32 ();

        public override short ReadInt16 () => binaryReader.ReadInt16 ();

        public override byte ReadByte () => binaryReader.ReadByte ();

        public override long ReadUInt32 () => binaryReader.ReadUInt32 ();

        public override byte[] ReadBytes ( int number )
        {
            var bytes = binaryReader.ReadBytes (number);
            if ( bytes.Length < number )
                throw new EndOfStreamException ();

            return bytes;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void Dispose ( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    binaryReader.Dispose ();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public override void Dispose ()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose ( true );
        }
        #endregion
    }
}
