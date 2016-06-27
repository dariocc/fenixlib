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
    public class NativeFormatReader : BinaryReader, INativeFormatReader
    {
        private static readonly Encoding encoding = Encoding.GetEncoding ( 850 );

        public NativeFormatReader ( Stream input ) : base ( input, encoding )
        {
        }

        public string ReadAsciiZ ( int length )
        {
            byte[] bytes = ReadBytes ( length );

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

        public Header ReadHeader ()
        {
            // 3 first bytes describe the graphic format of the MAP
            var fileType = encoding.GetString ( ReadBytes ( 3 ) );
            // Next 4 bytes are MS-DOS termination, and last is the MAP version
            var terminator = ReadBytes ( 4 );
            // Last byte of the header is the version number
            var lastByte = ReadByte ();

            return new Header ( fileType, terminator, lastByte );
        }

        public Palette ReadPalette ()
        {
            var paletteColors = ReadBytes ( PaletteBytesSize );

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

        public PivotPoint[] ReadPivotPoints ( int number )
        {
            List<PivotPoint> points = new List<PivotPoint> ();
            for ( int n = 0 ; n < number ; n++ )
            {
                var x = ReadInt16 ();
                var y = ReadInt16 ();
                PivotPoint point = new PivotPoint ( n, x, y );

                // If the X and Y are -1, there is no need to add
                if ( !( point.X == -1 & point.Y == -1 ) )
                {
                    points.Add ( point );
                }
            }
            return points.ToArray ();
        }

        public byte[] ReadPaletteGammas ()
        {
            var bytes = ReadBytes ( PaletteGammaBytesSize );
            if (bytes.Length < PaletteGammaBytesSize)
            {
                throw new EndOfStreamException ();
            }

            return bytes;
        }

        public int ReadPivotPointsMaxIdUint16 ()
        {
            var flags = ReadUInt16 ();
            var numberPivotPoints = Convert.ToInt16 ( flags & PivotPointsNumberBitMask );
            return numberPivotPoints;
        }

        public int ReadPivotPointsMaxIdInt32 ()
        {
            var flags = ReadUInt32 ();
            var numberPivotPoints = Convert.ToInt16 ( flags & PivotPointsNumberBitMask );
            return numberPivotPoints;
        }

        public GlyphInfo ReadLegacyFntGlyphInfo ()
        {
            return new LegacyGlyphInfoBuilder ()
                .Width ( ReadInt32 () )
                .Height ( ReadInt32 () )
                .YOffset ( ReadInt32 () )
                .FileOffset ( ReadInt32 () )
                .Build ();
        }

        public GlyphInfo ReadExtendedFntGlyphInfo ()
        {
            return new ExtendedGlyphInfoBuilder ()
                .Width ( ReadInt32 () )
                .Height ( ReadInt32 () )
                .XAdvance ( ReadInt32 () )
                .YAdvance ( ReadInt32 () )
                .XOffset ( ReadInt32 () )
                .YOffset ( ReadInt32 () )
                .FileOffset ( ReadInt32 () )
                .Build ();
        }


        public byte[] ReadPixels ( GraphicFormat format, int width, int height )
        {
            var size = format.PixelsBytesForSize ( width, height );
            var bytes = ReadBytes ( size );

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

        int INativeFormatReader.ReadUInt16AsInt32 ()
        {
            return ReadUInt16 ();
        }
    }
}
