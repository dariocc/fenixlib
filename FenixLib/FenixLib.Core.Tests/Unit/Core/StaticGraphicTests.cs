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
using FenixLib.Core;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture ( Category = "Unit" )]
    class StaticGraphicTests
    {

        private byte[] argbPixels;
        private byte[] indexedPixels;

        [SetUp]
        public void SetUp ()
        {
            argbPixels = new byte[GraphicFormat.ArgbInt32.PixelsBytesForSize ( 1, 1 )];
            indexedPixels = new byte[GraphicFormat.RgbIndexedPalette.PixelsBytesForSize ( 1, 1 )];
        }

        [TestCase ( -1, 1 )]
        [TestCase ( 0, 1 )]
        [TestCase ( 1, -1 )]
        [TestCase ( 1, 0 )]
        public void Construct_NegativeOrZeoWidthOrHeight_ThrowsException ( int w, int h )
        {
            TestDelegate createGraphic = () => 
            {
                new StaticGraphic ( GraphicFormat.ArgbInt32, w, h, argbPixels );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentOutOfRangeException> () );

        }

        [Test]
        public void Construct_GraphicFormatIsIndexedAndPaletteIsNull_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                new StaticGraphic ( GraphicFormat.RgbIndexedPalette, 1, 1, indexedPixels, null );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentException> () );
        }

        [Test]
        public void Construct_PaletteIsPassedButFormatIsNotIndexed_PaletteIsSetToNull ()
        {
            var palette = MockRepository.GenerateStub<Palette> ();

            // A non indexed graphic with a non null palette
            var g = new StaticGraphic ( GraphicFormat.ArgbInt32, 1, 1, argbPixels, palette );

            // Graphic palette still needs to be null
            Assert.That ( g.Palette, Is.Null );
        }

        [Test]
        public void Construct_NullPixelData_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                new StaticGraphic ( GraphicFormat.ArgbInt32, 1, 1, null );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentNullException> () );
        }

        [Test]
        public void Construct_NullGraphicFormat_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                new StaticGraphic ( null, 1, 1, argbPixels );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentNullException> () );
        }

        [Test]
        public void Construct_InvalidPixelDataLength_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                // The pixel data should 4bytes but we pass instead 100
                new StaticGraphic ( GraphicFormat.ArgbInt32, 1, 1, new byte[100] );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentException> () );
        }
    }
}