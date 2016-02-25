using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenixLib.Core.Tests
{
    [TestFixture]
    public class BitmapFontTests
    {
        private BitmapFont stubFont32;
        private BitmapFont stubFont16;

        private Glyph stubGlyph32;
        private Glyph stubGlyph16;

        [SetUp]
        public void Init ()
        {
            stubFont32 = BitmapFont.Create ( DepthMode.ArgbInt32,
                FontCodePage.ISO85591 );

            stubFont16 = BitmapFont.Create ( DepthMode.RgbInt16,
                FontCodePage.ISO85591 );

            stubGlyph32 = Glyph.Create ( DepthMode.ArgbInt32, 10, 10, new byte[100 * 4], null );
            stubGlyph16 = Glyph.Create ( DepthMode.RgbInt16, 10, 10, new byte[100 * 4], null );
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

            Assert.AreEqual ( "Glyph and font depths need to match.", ex.Message );
        }
    }
}
