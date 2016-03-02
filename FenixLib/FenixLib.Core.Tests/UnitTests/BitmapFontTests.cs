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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenixLib.Core.Tests
{
    [TestFixture, Category("Unit")]
    public class BitmapFontTests
    {
        private BitmapFont stubFont32;
        private BitmapFont stubFont16;

        private Glyph stubGlyph32;
        private Glyph stubGlyph16;

        [SetUp]
        public void SetUp ()
        {
            stubFont32 = BitmapFont.Create ( GraphicFormat.ArgbInt32,
                FontCodePage.ISO85591 );

            stubFont16 = BitmapFont.Create ( GraphicFormat.RgbInt16,
                FontCodePage.ISO85591 );

            stubGlyph32 = new Glyph ( GraphicFormat.ArgbInt32, 10, 10, new byte[100 * 4], null );
            stubGlyph16 = new Glyph ( GraphicFormat.RgbInt16, 10, 10, new byte[100 * 4], null );
        }

        [Test]
        public void IntIndexerGet_NonDefinedGlyph_Null ()
        {
            Assert.IsNull ( stubFont32[0] );
        }

        [Test]
        public void CharIndexerGet_NonDefinedGlyph_Null ()
        {
            Assert.IsNull ( stubFont32['a'] );
        }

        [Test]
        public void IntIndexerGet_DefinedGlyph_NotNull ()
        {
            stubFont32[10] = stubGlyph32;
            Assert.IsNotNull ( stubFont32[10] );
        }

        [Test]
        public void CharIndexerGet_DefinedGlyph_NotNull ()
        {
            stubFont32['a'] = stubGlyph32;
            Assert.IsNotNull ( stubFont32['a'] );
        }

        [Test]
        public void IntIndexerGet_DefinedGlypWithIndexerSetterChar_NotNull ()
        {
            stubFont32['æ'] = stubGlyph32;
            Assert.IsNotNull ( stubFont32[230] );
        }

        [Test]
        public void IntIndexerGet_DefinedGlyphWithIndexerSetterInt_NotNull ()
        {
            stubFont32[230] = stubGlyph32;
            Assert.IsNotNull ( stubFont32['æ'] );
        }

        [Test]
        public void CharIndexerSet_GlyphDepthDoesNotMatchFontDepth_RaisesArgumentException ()
        {
            ArgumentException ex = Assert.Throws<ArgumentException> (
                () => stubFont32['a'] = stubGlyph16 );

            Assert.AreEqual ( "Glyph and font graphic formats need to match.", ex.Message );
        }
    }
}
