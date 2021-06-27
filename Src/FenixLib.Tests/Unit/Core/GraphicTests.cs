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
using System;
using FenixLib.Core;
using Moq;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture ( Category = "Unit" )]
    class GraphicTests
    {

        private byte[] argbPixels;
        private byte[] indexedPixels;

        [SetUp]
        public void SetUp ()
        {
            argbPixels = new byte[GraphicFormat.Format32bppArgb.PixelsBytesForSize ( 1, 1 )];
            indexedPixels = new byte[GraphicFormat.Format8bppIndexed.PixelsBytesForSize ( 1, 1 )];
        }

        [TestCase ( -1, 1 )]
        [TestCase ( 0, 1 )]
        [TestCase ( 1, -1 )]
        [TestCase ( 1, 0 )]
        public void Construct_NegativeOrZeoWidthOrHeight_ThrowsException ( int w, int h )
        {
            TestDelegate createGraphic = () => 
            {
                new Graphic ( GraphicFormat.Format32bppArgb, w, h, argbPixels );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentOutOfRangeException> () );

        }

        [Test]
        public void Construct_GraphicFormatIsIndexedAndPaletteIsNull_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                new Graphic ( GraphicFormat.Format8bppIndexed, 1, 1, indexedPixels, null );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentException> () );
        }

        [Test]
        public void Construct_PaletteIsPassedButFormatIsNotIndexed_PaletteIsSetToNull ()
        {
            var palette = new Mock<Palette> ();

            // A non indexed graphic with a non null palette
            var g = new Graphic ( GraphicFormat.Format32bppArgb, 1, 1, argbPixels, palette.Object );

            // Graphic palette still needs to be null
            Assert.That ( g.Palette, Is.Null );
        }

        [Test]
        public void Construct_NullPixelData_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                new Graphic ( GraphicFormat.Format32bppArgb, 1, 1, null );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentNullException> () );
        }

        [Test]
        public void Construct_NullGraphicFormat_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                new Graphic ( null, 1, 1, argbPixels );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentNullException> () );
        }

        [Test]
        public void Construct_InvalidPixelDataLength_ThrowsException ()
        {
            TestDelegate createGraphic = () =>
            {
                // The pixel data should 4bytes but we pass instead 100
                new Graphic ( GraphicFormat.Format32bppArgb, 1, 1, new byte[100] );
            };

            Assert.That ( createGraphic, Throws.InstanceOf<ArgumentException> () );
        }
    }
}