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

namespace FenixLib.Core.Tests.UnitTests
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
            fakeGlyph.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.ArgbInt32 );

            aFontGlyph = new FontGlyph ( 'a', fakeGlyph );
            equivalentFontGlyph = new FontGlyph ( aFontGlyph.Character, fakeGlyph );
        }

        [Test]
        public void XAdvanceGetter_PropertyModifiedInBaseGlyph_ReturnsSameAsBaseGlyph()
        {
            fakeGlyph.XAdvance = 10;
            Assert.AreEqual ( 10, aFontGlyph.XAdvance );
        }

        [Test]
        public void YAdvanceGetter_PropertyModifiedInBaseGlyph_ReturnsSameAsBaseGlyph ()
        {
            fakeGlyph.YAdavance = 10;
            Assert.AreEqual ( 10, aFontGlyph.YAdavance );
        }

        [Test]
        public void XOffsetGetter_PropertyModifiedInBaseGlyph_ReturnsSameAsBaseGlyph ()
        {
            fakeGlyph.XOffset = 10;
            Assert.AreEqual ( 10, aFontGlyph.XOffset );
        }


        [Test]
        public void YOffsetGetter_PropertyModifiedInBaseGlyph_ReturnsSameAsBaseGlyph ()
        {
            fakeGlyph.YOffset = 10;
            Assert.AreEqual ( 10, aFontGlyph.YOffset );
        }

        [Test]
        public void XAdvanceSetter_PropertyIsModified_ChangeReflectedInBaseGlyph ()
        {
            aFontGlyph.XAdvance = 10;
            Assert.AreEqual ( 10, fakeGlyph.XAdvance );
        }

        [Test]
        public void YAdvanceSetter_PropertyIsModified_ChangeReflectedInBaseGlyph ()
        {
            aFontGlyph.YAdavance = 10;
            Assert.AreEqual ( 10, fakeGlyph.YAdavance );
        }

        [Test]
        public void XOffsetSetter_PropertyIsModified_ChangeReflectedInBaseGlyph ()
        {
            aFontGlyph.XOffset = 10;
            Assert.AreEqual ( 10, fakeGlyph.XOffset );
        }


        [Test]
        public void YOffsetSetter_PropertyIsModified_ChangeReflectedInBaseGlyph ()
        {
            aFontGlyph.YOffset = 10;
            Assert.AreEqual ( 10, fakeGlyph.YOffset );
        }

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