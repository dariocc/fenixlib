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
    [TestFixture, Category ( "Unit" )]
    public class BitmapFontTests
    {
        private BitmapFont stubFont32;
        private BitmapFont stubFont16;

        private IGlyph stubGlyph32;
        private IGlyph stubGlyph16;

        [SetUp]
        public void SetUp ()
        {
            stubFont32 = new BitmapFont ( FontEncoding.ISO85591, GraphicFormat.Format32bppArgb );

            stubFont16 = new BitmapFont ( FontEncoding.ISO85591, GraphicFormat.Format16bppRgb565  );

            stubGlyph32 = new Glyph ( CreateFakeGraphic ( 32 ) );
            stubGlyph16 = new Glyph ( CreateFakeGraphic ( 16 ) );
        }

        private IGraphic CreateFakeGraphic ( int bpp )
        {
            IGraphic stubGraphic = MockRepository.GenerateStub<IGraphic> ();
            stubGraphic.Stub ( x => x.GraphicFormat ).Return ( ( GraphicFormat ) bpp );

            return stubGraphic;
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
        public void IntIndexerGet_DefinedGlypWithIndexerSetChar_NotNull ()
        {
            stubFont32['æ'] = stubGlyph32;
            Assert.IsNotNull ( stubFont32[230] );
        }

        [Test]
        public void IntIndexerGet_DefinedGlyphWithIndexerSetInt_NotNull ()
        {
            stubFont32[230] = stubGlyph32;
            Assert.IsNotNull ( stubFont32['æ'] );
        }

        [Test]
        public void CharIndexerSet_GlyphFormatDoesNotMatchFontFormat_ThrowsException ()
        {
            var ex = Assert.Throws<FormatMismatchException> (
                () => stubFont32['a'] = stubGlyph16 );
        }
    }
}
