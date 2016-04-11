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
            - Test function initialize memory field with binary data.
            - Test function invokes method under test of the NativeFormatReader class.
            - Test function compares the result of the read method against what was expected.

            The NativeFormatReader is initialized with a stubbed stream that will read
            data from the memory byte array.

            Yes, this is kind of using a MemoryStream with the particularity that the stubbed
            stream only contains minimal behaviour to make it work with NativeFormatReader.
        */

        private NativeFormatReader formatReader;
        private byte[] memory;
        private int position = 0;
        private int lastReadLength = 0;

        // The strings are encoded according to CP850
        private static byte[] sampleAsciiZString = new byte[]
        {
            0x61, // a
            0x62, // b
            0x86, // å
            0x00, // Null Char --> Should be the end of the string
            0x63, // c
            0x64, // d
        };

        [SetUp]
        public void SetUp ()
        {
            // Stream Stub where Read() and ReadByte() calls will return
            // data from the memory private field.
            var streamStub = MockRepository.GenerateStub<Stream> ();

            streamStub.Stub ( _ => _.CanRead ).Return ( true );

            streamStub.Stub ( _ => _.Read (
                  Arg<byte[]>.Out ( new byte[1] ).Dummy,
                  Arg<int>.Is.GreaterThanOrEqual ( 0 ),
                  Arg<int>.Is.GreaterThan ( 0 ) )
                )
            .WhenCalled ( _ =>
            {
                var offset = ( int ) _.Arguments[1];
                var length = ( int ) _.Arguments[2];

                var tmp = new byte[length];
                Array.Copy ( memory, offset + position, tmp, 0, length );

                position += length;
                _.Arguments[0] = tmp;
                _.ReturnValue = length; // TODO: Does this make any difference?
                lastReadLength = length;

            } ).Return ( lastReadLength );

            streamStub.Stub ( _ => _.ReadByte () )
            .WhenCalled ( _ =>
            {
                _.ReturnValue = ( int ) memory[position];
                position++;
                lastReadLength = 1;
            } ).Return ( ( int ) ( 1 ) );

            formatReader = new NativeFormatReader ( streamStub );
        }

        [TearDown]
        public void TearDown ()
        {
            memory = null;
            position = 0;
            lastReadLength = 0;
        }

        [Test]
        public void ReadAsciiZ_NullCharIsAfterLength_ReturnsCorrectString ()
        {
            memory = sampleAsciiZString;
            Assert.That ( formatReader.ReadAsciiZ ( 3 ), Is.EqualTo ( "abå" ) );
        }

        [Test]
        public void ReadAsciiZ_NullCharBeforeMaxLengthOfString_NoCharacterAfterNullCharIsReturned ()
        {
            memory = sampleAsciiZString;
            Assert.That ( formatReader.ReadAsciiZ ( 5 ), Is.EqualTo ( "abå" ) );
        }

        [Test]
        public void ReadHeader_Works ()
        {
            memory = new byte[]
            {
                0x61, 0x62, 0x63,
                0x01, 0x02, 0x03, 0x04,
                0x99
            };

            var header = formatReader.ReadHeader ();

            Assert.That ( header.Magic, Is.EqualTo ( "abc" ) );
            Assert.That ( header.Terminator,
                Is.EquivalentTo ( new byte[] { 0x01, 0x02, 0x03, 0x04 } ) );
            Assert.That ( header.LastByte, Is.EqualTo ( 0x99 ) );
        }

        [Test]
        [Ignore ("WIP")]
        public void ReadPalette_ValidStream_Works ()
        {
            memory = new byte[256 * 3];

            throw new NotImplementedException ();
        }

        [Test]
        [Ignore("WIP")]
        public void ReadPalette_EntriesGreaterThan64_Throws ()
        {
            memory = new byte[256 * 3];

            throw new NotImplementedException ();
        }
    }
}
