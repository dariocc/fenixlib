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
using FenixLib.Core;
using FenixLib.IO;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit" )]
    public class NativeFormatWriterTests
    {

        private NativeFormatWriter fakeWriter;
        // Keeps track of the bytes written to the fake stream
        // NOTE: As per https://msdn.microsoft.com/en-us/library/24e33k1w%28v=vs.110%29.aspx
        // BinaryWriter implementation (superclass to fakeWriter) uses little-endian format
        private byte[] currentMemory;

        [SetUp]
        public void SetUp ()
        {
            // Stream stub that memorizes the bytes written to the field every
            // time Write() overloads are called
            var streamStub = MockRepository.GenerateStub<Stream> ();

            streamStub.Stub ( _ => _.CanWrite ).Return ( true );

            streamStub.Stub ( _ => _.Write (
                Arg<byte[]>.Is.NotNull,
                Arg<int>.Is.GreaterThanOrEqual ( 0 ),
                Arg<int>.Is.GreaterThan ( 0 ) ) )
            .WhenCalled ( a =>
            {
                var bytes = a.Arguments[0] as byte[];
                var tmp = new byte[( int ) a.Arguments[2]];
                Array.Copy ( bytes, tmp, tmp.Length );
                ResizeMemory ( tmp );
            } );

            streamStub.Stub ( _ => _.WriteByte ( Arg<byte>.Is.Anything ) )
            .WhenCalled ( _ =>
            {
                var bytes = new byte[] { ( byte ) _.Arguments[0] };
                ResizeMemory ( bytes );
            } );

            fakeWriter = new NativeFormatWriter ( streamStub );
        }

        [Test]
        public void Construct_NullArgument_Throws ()
        {
            Assert.That ( () => new NativeFormatWriter ( null ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void WriteAsciiZ_StringSorterThanMaxLength_ByteAffterCharacterIs0 ()
        {
            fakeWriter.WriteAsciiZ ( "a", 2 );
            Assert.That ( currentMemory[1], Is.EqualTo ( 0 ) );
        }

        [Test]
        public void WriteAsciiZ_NullString_Throws ()
        {
            Assert.That ( () => fakeWriter.WriteAsciiZ ( null, 2 ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void WriteAsciiZ_NegativeMaxLength_Throws ()
        {
            Assert.That ( () => fakeWriter.WriteAsciiZ ( "a text", -2 ),
                Throws.InstanceOf<ArgumentOutOfRangeException> () );
        }


        [Test]
        public void WriteExtendedGlyphInfo_Test ()
        {
            throw new NotImplementedException ();
        }

        [Test]
        public void WriteLegacyFntGlyphInfo_Test ()
        {
            throw new NotImplementedException ();
        }

        [Test]
        public void WritePalette_NullPalette_Throws ()
        {
            Assert.That ( () => fakeWriter.Write ( null as Palette ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void WritePalette_ValidPalette_Works ()
        {
            // TODO: Create a Palette2Bytes converter class so as WritePalette
            // becomes more easily testable?
            throw new NotImplementedException ();
        }


        [Test]
        public void WritePivotPoints_ValidPivotPoint_Works ()
        {
            fakeWriter.Write ( new PivotPoint ( 0, 0x01AA, 0x02BB ) );
            var expected = new byte[] { 0xAA, 0x01, 0xBB, 0x02 };
            Assert.That ( () => currentMemory, Is.EqualTo ( expected ) );
        }

        [Test]
        public void WriteReservedPaletteGammaSection_Test ()
        {
            throw new NotImplementedException ();
        }

        // Resizes currentMemory to hold bytes and copies the contents
        // of bytes to it
        private void ResizeMemory ( byte[] bytes )
        {
            int destIndex;

            if ( currentMemory == null )
            {
                destIndex = 0;
                currentMemory = new byte[bytes.Length];
            }
            else
            {
                destIndex = currentMemory.Length;
                var oldMemory = currentMemory;
                currentMemory = new byte[currentMemory.Length + bytes.Length];
                Array.Copy ( oldMemory, currentMemory, oldMemory.Length );
            }

            Array.Copy ( bytes, 0, currentMemory, destIndex, bytes.Length );
        }
    }
}
