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
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.IO;
using System.Linq;
using FenixLib.Core;
using FenixLib.IO;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit" )]
    public class NativeFormatReaderTests
    {

        /* 
            NOTICE FOR DEVELOPERS

            The basic idea for the test methods in this test fixture are:
            - Arrange an array of bytes initialized to known values.
            - Create a NativeFormatReader via MemoryStream that backs that byte array.
            - Test function invokes method under test of the NativeFormatReader class.
            - The objects returned by the function under test is compared against what it
              is expected, based on the fact that we know how the MemoryStream was initialized.
        */

        private NativeFormatReader formatReader;

        private readonly static byte[] samplePaletteBytes = new byte[]
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,

            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,

            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,

            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,

            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
        };

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
        public void ReadHeader_Works ()
        {
            var bytes = new byte[]
            {
                0x61, 0x62, 0x63,
                0x01, 0x02, 0x03, 0x04,
                0x99
            };

            formatReader = CreateFormatReader ( bytes );
            var header = formatReader.ReadHeader ();

            Assert.That ( header.Magic, Is.EqualTo ( "abc" ) );
            Assert.That ( header.Terminator,
                Is.EquivalentTo ( new byte[] { 0x01, 0x02, 0x03, 0x04 } ) );
            Assert.That ( header.LastByte, Is.EqualTo ( 0x99 ) );
        }

        [Test]
        [Ignore ( "WIP" )]
        public void ReadPalette_ValidStream_Works ()
        {
            var bytes = new byte[256 * 3];
            formatReader = CreateFormatReader ( bytes );

            throw new NotImplementedException ();
        }

        [Test]
        [Ignore ( "WIP" )]
        public void ReadPalette_EntriesGreaterThan64_Throws ()
        {
            var bytes = new byte[256 * 3];
            formatReader = CreateFormatReader ( bytes );

            throw new NotImplementedException ();
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

        private NativeFormatReader CreateFormatReader ( byte[] bytes )
        {
            var stream = new MemoryStream ( bytes, false );
            stream.Flush ();
            return new NativeFormatReader ( stream );
        }
    }
}
