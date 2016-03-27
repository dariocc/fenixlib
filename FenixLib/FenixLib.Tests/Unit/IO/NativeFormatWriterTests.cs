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
        private Stream fakeStream;

        [SetUp]
        public void SetUp ()
        {
            fakeStream = MockRepository.GenerateMock<Stream> ();
        }

        [Test ()]
        public void NativeFormatWriter_Test ()
        {

        }

        [Test ()]
        public void WriteAsciiZ_Test ()
        {

        }

        [Test ()]
        public void WriteExtendedGlyphInfo_Test ()
        {

        }

        [Test ()]
        public void WriteLegacyFntGlyphInfo_Test ()
        {

        }

        [Test ()]
        public void Write_Test ()
        {

        }

        [Test ()]
        public void WritePivotPoints_TestCases ()
        {

        }

        [Test ()]
        public void WriteReservedPaletteGammaSection_Test ()
        {

        }

        private class StreamFake : Stream
        {
            #region implemented abstract members of Stream

            public override void Flush ()
            {
                throw new NotImplementedException ();
            }

            public override long Seek ( long offset, SeekOrigin origin )
            {
                throw new NotImplementedException ();
            }

            public override void SetLength ( long value )
            {
                throw new NotImplementedException ();
            }

            public override int Read ( byte[] buffer, int offset, int count )
            {
                throw new NotImplementedException ();
            }

            public override void Write ( byte[] buffer, int offset, int count )
            {
                throw new NotImplementedException ();
            }

            public override bool CanRead
            {
                get
                {
                    throw new NotImplementedException ();
                }
            }

            public override bool CanSeek
            {
                get
                {
                    throw new NotImplementedException ();
                }
            }

            public override bool CanWrite
            {
                get
                {
                    throw new NotImplementedException ();
                }
            }

            public override long Length
            {
                get
                {
                    throw new NotImplementedException ();
                }
            }

            public override long Position
            {
                get
                {
                    throw new NotImplementedException ();
                }
                set
                {
                    throw new NotImplementedException ();
                }
            }

            #endregion
			
        }
    }
}
