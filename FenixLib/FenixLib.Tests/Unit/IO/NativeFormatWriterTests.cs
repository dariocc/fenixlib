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

        private NativeFormatWriter fakeNativeFormatWriter;
        // Keeps track of the bytes written to the fake stream
        private byte [] currentMemory;

        [SetUp]
        public void SetUp ()
        {

            var streamStub = MockRepository.GenerateStub<Stream> ();

            streamStub.Stub ( _ => _.CanWrite ).Return ( true );

            streamStub.Stub ( _ => _.Write (
                Arg<byte []>.Is.NotNull, 
                Arg<int>.Is.GreaterThanOrEqual ( 0 ), 
                Arg<int>.Is.GreaterThan ( 0 ) ) )
            .WhenCalled ( a =>
            {
                var bytes = a.Arguments[0] as byte [];
                ResizeMemory ( bytes );
            } );

            streamStub.Stub ( _ => _.WriteByte ( Arg<byte>.Is.Anything ) )
            .WhenCalled ( _ =>
            {
                var bytes = new byte[] { ( byte ) _.Arguments[0] };
                ResizeMemory ( bytes );
            } );

            fakeNativeFormatWriter = new NativeFormatWriter ( streamStub );
        }

        [Test ()]
        public void Construct_NullArgument_Throws ()
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

        // Resizes currentMemory to hold bytes and copies the contents
        // of bytes to it
        private void ResizeMemory ( byte [] bytes )
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
                currentMemory = new byte[currentMemory.Length + bytes.Length];
            }

            Array.Copy ( bytes, 0, currentMemory, destIndex, bytes.Length );
        }
    }
}
