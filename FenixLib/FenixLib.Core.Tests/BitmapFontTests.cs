using NUnit.Framework;
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
        private BitmapFont stubFontIso85591bpp32;
        private Glyph stubGlyph;

        [SetUp]
        public void Init()
        {
            stubFontIso85591bpp32 = BitmapFont.Create ( DepthMode.ArgbInt32, 
                FontCodePage.ISO85591 );
            stubGlyph = Glyph.Create ( DepthMode.ArgbInt32, 10, 10, new byte[10 * 10 * 8] );
        }

        [Test]
        public void IndexerGetInt_NonDefinedGlyph_Null ()
        {
            Assert.IsNull ( stubFontIso85591bpp32[0] );
        }

        [Test]
        public void IndexerGetChar_NonDefinedGlyph_Null ()
        {
            Assert.IsNull ( stubFontIso85591bpp32['a'] );
        }

        [Test]
        public void IndexerGetInt_DefinedGlyph_NotNull ()
        {
            stubFontIso85591bpp32[10] = stubGlyph;
            Assert.IsNotNull ( stubFontIso85591bpp32[10] );
        }

        [Test]
        public void IndexerGetChar_DefinedGlyph_NotNull ()
        {
            stubFontIso85591bpp32['a'] = stubGlyph;
            Assert.IsNotNull ( stubFontIso85591bpp32['a'] );
        }

        [Test]
        public void IndexerGetInt_DefinedGlypWithIndexerSetterChar_NotNull ()
        {
            stubFontIso85591bpp32['æ'] = stubGlyph;
            Assert.IsNotNull ( stubFontIso85591bpp32[230] );
        }

        [Test]
        public void IndexerGetInt_DefinedGlyphWithIndexerSetterInt_NotNull ()
        {
            stubFontIso85591bpp32[230] = stubGlyph;
            Assert.IsNotNull ( stubFontIso85591bpp32['æ'] );
        }
    }
}
