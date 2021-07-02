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
using System;
using Moq;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit")]
    class DivFilePaletteDecoderTests :  DivFilePaletteDecoder
    {
        [Test]
        public void ReadBody_HeaderMagicIsMap_First40BytesAreSkipped ()
        {
            // Configure a stream with 40 bytes initialized to 1 and the rest to 0
            var streamData = new byte[40 + 256 * 3];
            for (int i = 0 ; i < 40 ;  i++)
            {
                streamData[i] = 0xFF;
            }

            var header = new NativeFormat.Header ( "map", null, 0 );
            var stream = new MemoryStream ( streamData );
            var reader = new BinaryNativeFormatReader ( stream );

            var palette = ReadBody ( header, reader );

            Assert.That ( palette, Is.EqualTo ( new Palette () ) );
        }

        [Test]
        public void ReadBody_ValidInput_PaletteIsRead ()
        {
            // A palette with some non-black colors
            var colors = new PaletteColor[256];
            for ( int i = 0 ; i < colors.Length ; i++ )
                colors[i] = new PaletteColor ( i, i, i );
            var palette = new Palette ( colors );

            var header = new NativeFormat.Header ( "abc", new byte[] { 0 }, 0 );
            var stubStream = new Mock<Stream> ();
            // Stub a NativeFormatReader that returns the generated palette
            var stubReader = new Mock<NativeFormatReader> ( stubStream.Object );
            stubReader.Setup ( _ => _.ReadPalette () ).Returns ( palette );

            // Act
            var readBodyResult = ReadBody ( header, stubReader.Object );

            // ReadBody should have returned a palette equivalent to the one that the reader
            // returned
            Assert.That ( readBodyResult, Is.EqualTo ( palette ) );
        }

        #region abstract implementation
        public override int MaxSupportedVersion => 0;

        protected override string[] KnownFileMagics => new string[] { "abc" };

        protected override string[] KnownFileExtensions
        {
            get
            {
                throw new NotImplementedException ();
            }
        }
        #endregion
    }
}
