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
using System.IO;
using FenixLib.Core;
using FenixLib.IO;
using Moq;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit" )]
    class FntBitmapFontDecoderTests : FntBitmapFontDecoder
    {
        // Following member variables control the return values of the
        //     FntAstractBitmapFontDecoder abstract methods and properties.
        private int[] validBitsPerPixelDepths;
        private string[] knownFileMagics;
        private NativeFormatReader createNativeFormatReaderValue;
        private int parseBitsPerPixelValue;
        private int getPixelDataStartValue;
        private FontEncoding encoding;

        // Tracks the number of times the ReadGlyphInfo functions is called
        private int readGlyphInfoCount;

        [SetUp]
        public void SetUp ()
        {

        }

        [TearDown]
        public void TearDown ()
        {
            // Clean-up to avoid test interference
            validBitsPerPixelDepths = null;
            knownFileMagics = null;
            createNativeFormatReaderValue = null;
            parseBitsPerPixelValue = 0;
            encoding = null;

            readGlyphInfoCount = 0;
        }

        [Test]
        public void ReadBody_InvalidBitsPerPixels_ThrowsException ()
        {
            parseBitsPerPixelValue = 55;
            validBitsPerPixelDepths = new int[] { 10 };

            var streamSetup = new Mock<Stream> ();
            streamSetup.Setup ( _ => _.CanRead ).Returns ( true );

            var headerSetup = new NativeFormat.Header ( "abc", new byte[1], 0 );
            var readerSetup = new Mock<NativeFormatReader> ( streamSetup.Object );

            Assert.Catch<UnsupportedFileFormatException> (
                () => ReadBody ( headerSetup, readerSetup.Object ) );
        }

        [Test]
        public void ReadBody_BitsPerPixelIs8_ReadingOccursAsExpected ()
        {
            // Interaction testing of the ReadBody method through the
            // INativeFormatReader interface

            // Arrange
            parseBitsPerPixelValue = 8;
            validBitsPerPixelDepths = new int[] { 8 };
            encoding = FontEncoding.ISO85591;

            var headerSetup = new NativeFormat.Header ( "abc", new byte[1], 0 );

            var streamSetup = new Mock<Stream> ();
            streamSetup.Setup ( _ => _.CanRead ).Returns ( true );

            // The mocked INativeFormatReader to interact with ReadBody ()
            var readerMock = new Mock<NativeFormatReader>( streamSetup.Object );
            createNativeFormatReaderValue = readerMock.Object;

            // TODO: Verify called in order
            readerMock.Setup ( x => x.ReadPalette () ).Returns ( new Palette () );
            readerMock.Setup ( x => x.ReadPaletteGammas () ).Returns ( new byte[0] { } );
            readerMock.Setup ( x => x.ReadInt32 () ).Returns ( 1 ); // FontInfo
            readerMock.Setup ( x => x.ReadPixels ( It.IsAny<GraphicFormat> (), It.IsAny<int> (), It.IsAny<int> () ) ).Returns ( new byte[] { 0 } );

            // Act
            ReadBody ( headerSetup, readerMock.Object );

            // Assert
            readerMock.Verify ( x => x.ReadPaletteGammas () );
            readerMock.Verify ( x => x.ReadInt32 () );
            readerMock.Verify ( x => x.ReadPixels ( It.IsAny<GraphicFormat> (), It.IsAny<int> (), It.IsAny<int> () ), Times.Exactly(256));
            readerMock.Verify ( x => x.ReadPalette () );

            Assert.That ( readGlyphInfoCount, Is.EqualTo ( 256 ) );
        }

        #region virtual methods implementation

        protected override NativeFormatReader CreateNativeFormatReader ( Stream stream )
        {
            return createNativeFormatReaderValue;
        }

        protected override IBitmapFont ReadBody ( NativeFormat.Header header,
            NativeFormatReader reader )
        {
            return base.ReadBody ( header, reader );
        }

        public override int MaxSupportedVersion => 1;

        protected override FontEncoding Encoding => encoding;

        protected override string[] KnownFileMagics => knownFileMagics;

        protected override int[] ValidBitPerPixelDepths => validBitsPerPixelDepths;

        protected override int ParseBitsPerPixel ( NativeFormat.Header header )
        {
            return parseBitsPerPixelValue;
        }

        protected override int GetPixelDataStart ( int bpp )
        {
            return getPixelDataStartValue;
        }

        protected override void ProcessFontInfoField ( int codePageType )
        {
            // do nothing
        }

        protected override GlyphInfo ReadGlyphInfo ( NativeFormatReader reader )
        {
            readGlyphInfoCount++;
            var properties = new Mock<IGlyphInfoProperties> ();
            properties.Setup ( _ => _.Width ).Returns ( 1 );
            properties.Setup ( _ => _.Height ).Returns ( 1 );
            properties.Setup ( _ => _.FileOffset ).Returns ( 1 );

            return new GlyphInfo ( properties.Object );
        }

        #endregion
    }
}
