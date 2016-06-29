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
using System.IO;
using FenixLib.Core;
using FenixLib.IO;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit" )]
    class FntAbstractBitmapFontDecoderTests : FntAbstractBitmapFontDecoder
    {
        // Following member variables control the return values of the 
        //     FntAstractBitmapFontDecoder abstract methods and properties.   
        private int[] validBitsPerPixelDepths;
        private string[] knownFileMagics;
        private NativeFormatReader createNativeFormatReaderValue;
        private int parseBitsPerPixelValue;
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

            var streamStub = MockRepository.GenerateStub<Stream> ();
            streamStub.Stub ( _ => _.CanRead ).Return ( true );

            var headerStub = new NativeFormat.Header ( "abc", new byte[1], 0 );
            var readerStub = MockRepository.GenerateStub<NativeFormatReader> 
                ( streamStub );

            Assert.Catch<UnsuportedFileFormatException> (
                () => ReadBody ( headerStub, readerStub ) );
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

            var headerStub = new NativeFormat.Header ( "abc", new byte[1], 0 );

            var streamStub = MockRepository.GenerateStub<Stream> ();
            streamStub.Stub ( _ => _.CanRead ).Return ( true );

            // The mocked INativeFormatReader to interact with ReadBody ()
            var readerMock = MockRepository.GenerateStrictMock<NativeFormatReader> 
                ( streamStub );
            createNativeFormatReaderValue = readerMock;

            using ( readerMock.GetMockRepository ().Ordered () )
            {
                readerMock.Expect ( _ => _.ReadPalette () ).Return ( null )
                    .WhenCalled ( _ =>
                {
                    _.ReturnValue = new Palette ();
                } );

                readerMock.Expect ( _ => _.ReadPaletteGammas () )
                    .Return ( null );

                readerMock.Expect ( _ => _.ReadInt32 () )
                    .Return ( 1 ); // Font Info

                // 256 calls to ReadGlyphInfo 
                // will be loged in readGlyphInfoCount member variable

                readerMock.Expect ( _ => _.ReadPixels ( null, 0, 0 ) )
                    .IgnoreArguments ().Repeat.Times ( 256 )
                    .Return ( new byte[] { 0 } );
            }

            // Act
            ReadBody ( headerStub, readerMock );

            // Assert
            readerMock.VerifyAllExpectations ();
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

        protected override void ProcessFontInfoField ( int codePageType )
        {
            // do nothing
        }

        protected override GlyphInfo ReadGlyphInfo ( NativeFormatReader reader )
        {
            readGlyphInfoCount++;
            var properties = MockRepository.GenerateStub<IGlyphInfoProperties> ();
            properties.Stub ( _ => _.Width ).Return ( 1 );
            properties.Stub ( _ => _.Height ).Return ( 1 );
            properties.Stub ( _ => _.FileOffset ).Return ( 1 );

            return new GlyphInfo ( properties );
        }

        #endregion
    }
}
