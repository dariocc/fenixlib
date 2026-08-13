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
using System;
using System.IO;
using System.Linq;
using FenixLib.IO;
using Moq;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit" )]
    public class NativeFormatWriterTests
    {
        /* Note to developers:
        These tests write through NativeFormatWriter into a MemoryStream and then
        validate the little-endian bytes that BinaryWriter produced.
        */

        private NativeFormatWriter formatWriter;
        private MemoryStream stream;
        // Bytes written to the stream. BinaryWriter uses little-endian.
        private byte[] memory => stream.ToArray ();

        [SetUp]
        public void SetUp ()
        {
            stream = new MemoryStream ();
            formatWriter = new NativeFormatWriter ( stream );
        }

        [TearDown]
        public void TearDown ()
        {
            formatWriter.Dispose ();
            stream.Dispose ();
        }

        [Test]
        public void Construct_NullArgument_ThrowsException ()
        {
            Assert.That ( () => new NativeFormatWriter ( null ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void WriteAsciiZ_StringSorterThanMaxLength_ByteAffterCharacterIs0 ()
        {
            formatWriter.WriteAsciiZ ( "a", 2 );
            Assert.That ( memory[1], Is.EqualTo ( 0 ) );
        }

        [Test]
        public void WriteAsciiZ_StringLongerThanMaxLength_BytesWrittenDoNotExceedMaxLength ()
        {
            formatWriter.WriteAsciiZ ( "aaaa", 1 );
            Assert.That ( memory.Length, Is.EqualTo ( 1 ) );
        }

        [Test]
        public void WriteAsciiZ_NullString_WritesEmptyString ()
        {
            formatWriter.WriteAsciiZ ( null, 5 );
            Assert.That ( memory[0], Is.EqualTo ( 0 ) );
        }

        [Test]
        public void WriteAsciiZ_NegativeMaxLength_ThrowsException ()
        {
            Assert.That ( () => formatWriter.WriteAsciiZ ( "a text", -2 ),
                Throws.InstanceOf<ArgumentOutOfRangeException> () );
        }

        [Test]
        public void WriteExtendedGlyphInfo_Test ()
        {
            var stub = new Mock<IGlyphInfoProperties> ();
            stub.Setup ( _ => _.Width ).Returns ( 0x10 );
            stub.Setup ( _ => _.Height ).Returns ( 0x20 );
            stub.Setup ( _ => _.XAdvance ).Returns ( 0x30 );
            stub.Setup ( _ => _.YAdvance ).Returns ( 0x40 );
            stub.Setup ( _ => _.XOffset ).Returns ( 0x50 );
            stub.Setup ( _ => _.YOffset ).Returns ( 0x60 );
            stub.Setup ( _ => _.FileOffset ).Returns ( 0x70 );

            var glyphInfo = new GlyphInfo ( stub.Object );

            var expectedBytes = new byte[]
            {
                0x10, 0, 0, 0,
                0x20, 0, 0, 0,
                0x30, 0, 0, 0,
                0x40, 0, 0, 0,
                0x50, 0, 0, 0,
                0x60, 0, 0, 0,
                0x70, 0, 0, 0
            };

            formatWriter.WriteExtendedGlyphInfo ( ref glyphInfo );

            Assert.That ( memory, Is.EqualTo ( expectedBytes ) );
        }

        [Test]
        public void WriteLegacyFntGlyphInfo_ValidGlyph_Works ()
        {
            var stub = new Mock<IGlyphInfoProperties> ();
            stub.Setup ( _ => _.Width ).Returns ( 0x10 );
            stub.Setup ( _ => _.Height ).Returns ( 0x20 );
            stub.Setup ( _ => _.YOffset ).Returns ( 0x30 );
            stub.Setup ( _ => _.FileOffset ).Returns ( 0x40 );

            var glyphInfo = new GlyphInfo ( stub.Object );

            var expectedBytes = new byte[]
            {
                0x10, 0, 0, 0,
                0x20, 0, 0, 0,
                0x30, 0, 0, 0,
                0x40, 0, 0, 0
            };

            formatWriter.WriteLegacyGlyphInfo ( ref glyphInfo );

            Assert.That ( memory, Is.EqualTo ( expectedBytes ) );
        }

        [Test]
        public void WritePalette_NullPalette_ThrowsException ()
        {
            Assert.That ( () => formatWriter.Write ( null as Palette ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void WritePalette_ValidPalette_Works ()
        {
            var paletteStub = new Mock<Palette> ();
            var paletteColors = new PaletteColor[256];
            var expectedBytes = new byte[256 * 3];

            for ( int i = 0 ; i < 256 ; i++ )
            {
                // A sample color that will be in the stub palette
                var color = new PaletteColor ( i, i / 2, i / 3 );
                paletteColors[i] = color;
                paletteStub.Setup ( x => x[i] ).Returns ( color );

                // Color components are to be encoded in 6bits
                expectedBytes[i * 3 + 0] = ( byte ) ( ( i ) >> 2 );
                expectedBytes[i * 3 + 1] = ( byte ) ( ( i / 2 ) >> 2 );
                expectedBytes[i * 3 + 2] = ( byte ) ( ( i / 3 ) >> 2 );
            }
            // Stub the Colors property
            paletteStub.Setup ( _ => _.Colors ).Returns ( paletteColors );

            formatWriter.Write ( paletteStub.Object );

            Assert.That ( memory, Is.EqualTo ( expectedBytes ) );
        }

        [Test]
        public void WritePivotPoint_ValidPivotPoint_Works ()
        {
            var pivotPoint = new PivotPoint ( 0, 0x01AA, 0x02BB );
            var expectedBytes = new byte[] { 0xAA, 0x01, 0xBB, 0x02 };

            formatWriter.Write ( pivotPoint );

            Assert.That ( () => memory, Is.EqualTo ( expectedBytes ) );
        }

        [Test]
        public void WriteReservedPaletteGammaSection_Works ()
        {
            formatWriter.WritePaletteGammaSection ();

            // There are 16 gamma sections and they occupy 36bytes each.
            Assert.That ( memory.Length, Is.EqualTo ( 16 * 36 ) );
            // Every 16th byte needs to be 8, 16 or 32 for the format to be 
            // compatible with DIV games studio
            Assert.That ( memory.Where ( ( x, i ) => i % 36 == 0 ),
                Is.All.EqualTo ( 8 ).Or.EqualTo ( 16 ).Or.EqualTo ( 32 ) );
        }

    }
}
