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
using FenixLib.Core;
using Moq;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture ( Category = "Unit" )]
    public class FontGlyphTests
    {
        private Mock<IGlyph> fakeGlyph;
        private FontGlyph aFontGlyph;
        private FontGlyph equivalentFontGlyph;

        [SetUp]
        public void SetUp ()
        {
            fakeGlyph = new Mock<IGlyph> ();
            fakeGlyph.Setup ( x => x.Width ).Returns ( 1 );
            fakeGlyph.Setup ( x => x.Height ).Returns ( 1 );
            fakeGlyph.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format32bppArgb );
            fakeGlyph.Setup ( x => x.PixelData ).Returns ( new byte[10] );

            aFontGlyph = new FontGlyph ( 'a', fakeGlyph.Object );
            equivalentFontGlyph = new FontGlyph ( aFontGlyph.Character, fakeGlyph.Object );
        }

        [Test]
        public void XAdvanceGet_SameAsBaseGlyph ()
        {
            fakeGlyph.Setup ( x => x.XAdvance ).Returns ( 10 );
            Assert.AreEqual ( 10, aFontGlyph.XAdvance );
        }

        [Test]
        public void YAdvanceGet_SameAsBaseGlyph ()
        {
            fakeGlyph.Setup ( x => x.YAdvance ).Returns ( 10 );
            Assert.AreEqual ( 10, aFontGlyph.YAdvance );
        }

        [Test]
        public void XOffsetGet_SameAsBaseGlyph ()
        {
            fakeGlyph.Setup ( x => x.XOffset ).Returns ( 10 );
            Assert.AreEqual ( 10, aFontGlyph.XOffset );
        }


        [Test]
        public void YOffsetGet_SameAsBaseGlyph ()
        {
            fakeGlyph.Setup ( x => x.YOffset ).Returns ( 10 );
            Assert.AreEqual ( 10, aFontGlyph.YOffset );
        }

        [Test]
        public void PixelDataGet_SameAsBaseGlyph ()
        {
            Assert.AreSame ( aFontGlyph.PixelData, equivalentFontGlyph.PixelData );
        }

        [Test]
        public void XAdvanceSet_ChangeReflectedInBaseGlyph ()
        {
            fakeGlyph.SetupProperty( x => x.XAdvance );
            aFontGlyph.XAdvance = 10;
            Assert.AreEqual ( 10, fakeGlyph.Object.XAdvance );
        }

        [Test]
        public void YAdvanceSet_ChangeReflectedInBaseGlyph ()
        {
            fakeGlyph.SetupProperty( x => x.YAdvance );
            aFontGlyph.YAdvance = 10;
            Assert.AreEqual ( 10, fakeGlyph.Object.YAdvance );
        }

        [Test]
        public void XOffsetSet_ChangeReflectedInBaseGlyph ()
        {
            fakeGlyph.SetupProperty( x => x.XOffset );
            aFontGlyph.XOffset = 10;
            Assert.AreEqual ( 10, fakeGlyph.Object.XOffset );
        }


        [Test]
        public void YOffsetSet_ChangeReflectedInBaseGlyph ()
        {
            fakeGlyph.SetupProperty( x => x.YOffset );
            aFontGlyph.YOffset = 10;
            Assert.AreEqual ( 10, fakeGlyph.Object.YOffset );
        }

        // Equality and hashing

        [Test]
        public void Equals_NullGlyph_ReturnsFalse ()
        {
            Assert.That ( aFontGlyph.Equals ( null ), Is.False );
        }

        [Test]
        public void Equals_FontGlyphWithSameCharacter_ReturnsTrue ()
        {
            Assert.That ( aFontGlyph.Equals ( equivalentFontGlyph ), Is.True );
        }

        [Test]
        public void GetHashCode_TwoFontGlyphsWithSameCharacter_AreEqual ()
        {

            Assert.AreEqual ( aFontGlyph.GetHashCode (),
                equivalentFontGlyph.GetHashCode () );
        }
    }
}