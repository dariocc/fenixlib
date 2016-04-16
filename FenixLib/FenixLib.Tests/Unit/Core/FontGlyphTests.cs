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
using FenixLib.Core;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture ( Category = "Unit" )]
    public class FontGlyphTests
    {
        private IGlyph fakeGlyph;
        private FontGlyph aFontGlyph;
        private FontGlyph equivalentFontGlyph;

        [SetUp]
        public void SetUp ()
        {
            fakeGlyph = MockRepository.GenerateStub<IGlyph> ();
            fakeGlyph.Stub ( x => x.Width ).Return ( 1 );
            fakeGlyph.Stub ( x => x.Height ).Return ( 1 );
            fakeGlyph.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.Format32bppArgb );
            fakeGlyph.Stub ( x => x.PixelData ).Return ( new byte[10] );

            aFontGlyph = new FontGlyph ( 'a', fakeGlyph );
            equivalentFontGlyph = new FontGlyph ( aFontGlyph.Character, fakeGlyph );
        }

        [Test]
        public void XAdvanceGet_SameAsBaseGlyph()
        {
            fakeGlyph.XAdvance = 10;
            Assert.AreEqual ( 10, aFontGlyph.XAdvance );
        }

        [Test]
        public void YAdvanceGet_SameAsBaseGlyph ()
        {
            fakeGlyph.YAdavance = 10;
            Assert.AreEqual ( 10, aFontGlyph.YAdavance );
        }

        [Test]
        public void XOffsetGet_SameAsBaseGlyph ()
        {
            fakeGlyph.XOffset = 10;
            Assert.AreEqual ( 10, aFontGlyph.XOffset );
        }


        [Test]
        public void YOffsetGet_SameAsBaseGlyph ()
        {
            fakeGlyph.YOffset = 10;
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
            aFontGlyph.XAdvance = 10;
            Assert.AreEqual ( 10, fakeGlyph.XAdvance );
        }

        [Test]
        public void YAdvanceSet_ChangeReflectedInBaseGlyph ()
        {
            aFontGlyph.YAdavance = 10;
            Assert.AreEqual ( 10, fakeGlyph.YAdavance );
        }

        [Test]
        public void XOffsetSet_ChangeReflectedInBaseGlyph ()
        {
            aFontGlyph.XOffset = 10;
            Assert.AreEqual ( 10, fakeGlyph.XOffset );
        }


        [Test]
        public void YOffsetSet_ChangeReflectedInBaseGlyph ()
        {
            aFontGlyph.YOffset = 10;
            Assert.AreEqual ( 10, fakeGlyph.YOffset );
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