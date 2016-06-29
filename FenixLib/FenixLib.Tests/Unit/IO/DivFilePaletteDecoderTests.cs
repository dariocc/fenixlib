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
using System;

namespace FenixLib.Tests.Unit.IO
{
    [TestFixture ( Category = "Unit")]
    class DivFilePaletteDecoderTests :  DivFilePaletteDecoder
    {
        private NativeFormatReader Reader { get; set; }

        [SetUp]
        public void SetUp ()
        {

        }

        [TearDown]
        public void TearDown ()
        {
        }

        [Test]
        public void ReadBody_HeaderMagicIsMap_First40BytesAreSkipped ()
        {
            var stubHeader = new NativeFormat.Header ( "map", null, 0 );
            var stubStream = new MemoryStream ( new byte[41] );
            var mockedReader = MockRepository.GenerateMock<NativeFormatReader>
                ( stubStream );

        }

        [Test]
        public void ReadBody_ValidInputs_ReadPaletteIsCalled ()
        {
            var stubHeader = new NativeFormat.Header ( "abc", new byte[] { 0 }, 0 );
            var stubStream = MockRepository.GenerateStub<Stream> ();
            var mockedReader = MockRepository.GenerateMock<NativeFormatReader>
                ( stubStream );

            var decoder = new DivFilePaletteDecoderTests ();
            decoder.Reader = mockedReader;

            mockedReader.Expect ( _ => _.ReadPalette () ).Return ( null );
            mockedReader.Stub ( _ => _.ReadHeader () ).Return ( null );

            decoder.Decode ( stubStream );

            mockedReader.VerifyAllExpectations ();
        }

        public override int MaxSupportedVersion => 0;

        protected override string[] KnownFileMagics => new string[] { "abc" };

        protected override string[] KnownFileExtensions
        {
            get
            {
                throw new NotImplementedException ();
            }
        }

        protected override Palette ReadBody ( NativeFormat.Header header,
            NativeFormatReader reader )
        {
            return base.ReadBody ( header, Reader );
        }

        protected override bool ValidateHeaderMagic ( string magic, NativeFormat.Header header )
        {
            return true;
        }

        protected override bool ValidateHeaderTerminator ( byte[] terminator, NativeFormat.Header header )
        {
            return true;
        }

        protected override bool ValidateHeaderVersion ( int version, NativeFormat.Header header )
        {
            return true;
        }

        protected override NativeFormatReader CreateNativeFormatReader ( Stream stream )
        {
            return Reader;
        }
    }
}
