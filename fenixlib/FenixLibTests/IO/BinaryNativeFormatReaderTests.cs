/**
 *    Copyright (c) 2016 Darío Cutillas Carrillo
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 * 
 *        http://www.apache.org/licenses/LICENSE-2.0
 * 
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
using NUnit.Framework;
using System.IO;
using FenixLib.IO;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit" )]
    public class BinaryNativeFormatReaderTests
    {

        /*
            NOTICE FOR DEVELOPERS

            The basic idea for the test methods in this test fixture are:
            - Arrange an array of bytes initialized to known values.
            - Create a NativeFormatReader via MemoryStream that backs that byte array.
            - Test function invokes method under test of the NativeFormatReader class..
        */

        private BinaryNativeFormatReader formatReader;

        [SetUp]
        public void SetUp ()
        {
            // Ensure tests are run in isolation
            formatReader = null;
        }

        [TearDown]
        public void TearDown ()
        {
            // Satisfied the contract of the IDisposable
            formatReader.Dispose ();
        }

        [Test]
        public void ReadAsciiZ_InsufficientStreamBytes_ThrowsException ()
        {
            var bytes = new byte[1] { 0xFF };
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadAsciiZ ( 3 ), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadAsciiZ_NullCharIsAfterLength_ReturnsCorrectString ()
        {
            // Encoding is CP850
            var bytes = new byte[]
            {
                0x61, // a
                0x62, // b
                0x86, // å
                0x00, // Null Char --> End of the string
                0x63, // c
                0x64, // d
            };
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( formatReader.ReadAsciiZ ( 3 ), Is.EqualTo ( "abå" ) );
        }

        [Test]
        public void ReadAsciiZ_NullCharBeforeMaxLengthOfString_NoCharacterAfterNullCharIsReturned ()
        {
            // Encoding is CP850
            var bytes = new byte[]
            {
                0x61, // a
                0x62, // b
                0x86, // å
                0x00, // Null Char --> End of the string
                0x63, // c
                0x64, // d
            };
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( formatReader.ReadAsciiZ ( 5 ), Is.EqualTo ( "abå" ) );
        }

        [Test]
        public void ReadHeader_InsufficientStreamBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadHeader (), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadHeader_Works ()
        {
            var bytes = new byte[]
            {
                0x61, 0x62, 0x63,           // a b c
                0x01, 0x02, 0x03, 0x04,     // Terminator
                0x99                        // Lastyte
            };

            formatReader = CreateFormatReader ( bytes );
            var header = formatReader.ReadHeader ();

            Assert.That ( header.Magic, Is.EqualTo ( "abc" ) );
            Assert.That ( header.Terminator,
                Is.EqualTo ( new byte[] { 0x01, 0x02, 0x03, 0x04 } ) );
            Assert.That ( header.LastByte, Is.EqualTo ( 0x99 ) );
        }

        [Test]
        public void ReadPalette_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadPalette (), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadPalette_ValidStream_Works ()
        {
            var bytes = new byte[256 * 3];
            bytes[100 * 3] = 63;
            bytes[100 * 3 + 1] = 20;
            bytes[100 * 3 + 2] = 12;
            formatReader = CreateFormatReader ( bytes );
            var palette = formatReader.ReadPalette ();

            Assert.That ( palette.Colors[100].R, Is.EqualTo ( 252 ) );
            Assert.That ( palette.Colors[100].G, Is.EqualTo ( 20 << 2 ) );
            Assert.That ( palette.Colors[100].B, Is.EqualTo ( 12 << 2 ) );
        }

        [Test]
        public void ReadPalette_EntriesGreaterThan64_ThrowsException ()
        {
            var bytes = new byte[256 * 3];
            bytes[100] = 64;
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadPalette (), Throws.InstanceOf<IOException> () );
        }

        [Test]
        public void ReadPivotPoints_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadPivotPoints (3), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadPivotPoints_CoordinatesAreMinus1_ZeroPivotPoints ()
        {
            var bytes = new byte[]
            {
                0xFF, 0xFF, 0xFF, 0xFF,  // (-1, -1)
                0xFF, 0xFF, 0xFF, 0xFF,  // (-1, -1)
                0xFF, 0xFF, 0xFF, 0xFF   // (-1, -1)
            };

            formatReader = CreateFormatReader ( bytes );
            var pivotPoints = formatReader.ReadPivotPoints ( 3 );

            Assert.That ( pivotPoints.Length, Is.EqualTo ( 0 ) );
        }

        [Test]
        public void ReadPivotPoints_SomePivotPointsHaveMinus1Coordinates_IdsAreCorrect ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x00, 0x0B, 0x00, // Id 0
                0x0C, 0x00, 0x0D, 0x00, // Id 1
                0xFF, 0xFF, 0xFF, 0xFF,  // Skipped (-1, -1)
                0xFF, 0xFF, 0xFF, 0xFF,  // Skipped (-1, -1)
                0x0E, 0x00, 0x0F, 0x00   // Id should be 4
            };

            formatReader = CreateFormatReader ( bytes );
            var pivotPoints = formatReader.ReadPivotPoints ( 5 );

            Assert.That ( pivotPoints.Length, Is.EqualTo ( 3 ) );
            Assert.That ( pivotPoints[0].Id, Is.EqualTo ( 0 ) );
            Assert.That ( pivotPoints[1].Id, Is.EqualTo ( 1 ) );
            Assert.That ( pivotPoints[2].Id, Is.EqualTo ( 4 ) );
        }

        [Test]
        public void ReadPivotPoints_ValidPivotPoints_CoordinatesAreCorrect ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x00, 0x0B, 0x00,
                0x0C, 0x00, 0x0D, 0x00
            };

            formatReader = CreateFormatReader ( bytes );
            var pivotPoints = formatReader.ReadPivotPoints ( 2 );

            Assert.That ( pivotPoints[0].X, Is.EqualTo ( 0xA ) );
            Assert.That ( pivotPoints[0].Y, Is.EqualTo ( 0xB ) );
            Assert.That ( pivotPoints[1].X, Is.EqualTo ( 0xC ) );
            Assert.That ( pivotPoints[1].Y, Is.EqualTo ( 0xD ) );
        }

        [Test]
        public void ReadPaletteGammas_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadPaletteGammas (), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadPivotPointsMaxIdUnit16_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadPivotPointsMaxIdUint16 (), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadPivotPointMaxIdUint16_FlagsGreaterThan0xFFF_BitMaskIsApplied ()
        {
            var bytes = new byte[] { 0xFF, 0xFF };
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( formatReader.ReadPivotPointsMaxIdUint16 (), Is.EqualTo ( 0xFFF ) );
        }

        [Test]
        public void ReadPivotPointsMaxIdInt32_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadPivotPointsMaxIdInt32 (), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadPivotPointMaxIdInt32_FlagsGreaterThan0xFFF_BitMaskIsApplied ()
        {
            var bytes = new byte[] { 0xFF, 0xFF, 0xFF, 0x01 };
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( formatReader.ReadPivotPointsMaxIdInt32 (), Is.EqualTo ( 0xFFF ) );
        }

        [Test]
        public void ReadLegacyGlyphInfo_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadLegacyFntGlyphInfo (), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadExtendedGlyphInfo_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadExtendedFntGlyphInfo (), Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadLegacyFntGlyphInfo_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x00, 0x00, 0x00,
                0x0B, 0x00, 0x00, 0x00,
                0x0C, 0x00, 0x00, 0x00,
                0x0D, 0x00, 0x00, 0x00
            };
            formatReader = CreateFormatReader ( bytes );
            var info = formatReader.ReadLegacyFntGlyphInfo ();

            Assert.That ( info.Width, Is.EqualTo ( 0xA ) );
            Assert.That ( info.Height, Is.EqualTo ( 0xB ) );
            Assert.That ( info.XAdvance, Is.EqualTo ( 0xA ) );
            Assert.That ( info.YAdvance, Is.EqualTo ( 0xB + 0xC ) );
            Assert.That ( info.XOffset, Is.EqualTo ( 0x0 ) );
            Assert.That ( info.YOffset, Is.EqualTo ( 0xC ) );
            Assert.That ( info.FileOffset, Is.EqualTo ( 0xD ) );
        }

        [Test]
        public void ReadPixels_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[1];
            formatReader = CreateFormatReader ( bytes );

            Assert.That ( () => formatReader.ReadPixels (GraphicFormat.Format32bppArgb, 3, 3),
                Throws.InstanceOf<EndOfStreamException> () );
        }

        [Test]
        public void ReadExtendedFntGlyphInfo_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x00, 0x00, 0x00,
                0x0B, 0x00, 0x00, 0x00,
                0x0C, 0x00, 0x00, 0x00,
                0x0D, 0x00, 0x00, 0x00,
                0x0E, 0x00, 0x00, 0x00,
                0x0F, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x00, 0x00
            };
            formatReader = CreateFormatReader ( bytes );
            var info = formatReader.ReadExtendedFntGlyphInfo ();

            Assert.That ( info.Width, Is.EqualTo ( 0xA ) );
            Assert.That ( info.Height, Is.EqualTo ( 0xB ) );
            Assert.That ( info.XAdvance, Is.EqualTo ( 0xC ) );
            Assert.That ( info.YAdvance, Is.EqualTo ( 0xD ) );
            Assert.That ( info.XOffset, Is.EqualTo ( 0xE ) );
            Assert.That ( info.YOffset, Is.EqualTo ( 0xF ) );
            Assert.That ( info.FileOffset, Is.EqualTo ( 0x10 ) );
        }

        [Test]
        public void ReadUInt16_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x0B
            };

            formatReader = CreateFormatReader ( bytes );
            var uint16 = formatReader.ReadUInt16 ();

            Assert.That ( uint16, Is.EqualTo ( 0x0B0A ) );
        }

        [Test]
        public void ReadUInt32_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x0B, 0x0C, 0x0D
            };

            formatReader = CreateFormatReader ( bytes );
            var uint32 = formatReader.ReadUInt32 ();

            Assert.That ( uint32, Is.EqualTo ( 0x0D0C0B0A ) );
        }

        [Test]
        public void ReadInt16_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x0B
            };

            formatReader = CreateFormatReader ( bytes );
            var int16 = formatReader.ReadInt16 ();

            Assert.That ( int16, Is.EqualTo ( 0x0B0A ) );
        }

        [Test]
        public void ReadInt32_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x0B, 0x0C, 0x0D
            };

            formatReader = CreateFormatReader ( bytes );
            var int32 = formatReader.ReadInt32 ();

            Assert.That ( int32, Is.EqualTo ( 0x0D0C0B0A ) );
        }

        [Test]
        public void ReadByte_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A
            };

            formatReader = CreateFormatReader ( bytes );
            var aByte = formatReader.ReadByte ();

            Assert.That ( aByte, Is.EqualTo ( 0x0A ) );
        }

        [Test]
        public void ReadBytes_ValidStreamData_Works ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x0B
            };

            formatReader = CreateFormatReader ( bytes );
            var readBytes = formatReader.ReadBytes (2);

            Assert.That ( readBytes, Is.EqualTo ( bytes ) );
        }

        [Test]
        public void ReadBytes_InsufficientBytes_ThrowsException ()
        {
            var bytes = new byte[]
            {
                0x0A, 0x0B
            };

            formatReader = CreateFormatReader ( bytes );
            Assert.That ( () => formatReader.ReadBytes ( 3 ),
                Throws.InstanceOf<EndOfStreamException> () );
        }

        private BinaryNativeFormatReader CreateFormatReader ( byte[] bytes )
        {
            var stream = new MemoryStream ( bytes, false );
            stream.Flush ();
            return new BinaryNativeFormatReader ( stream );
        }
    }
}
