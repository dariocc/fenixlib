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
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using FenixLib.Core.Tests.IntegrationTests.Comparison;
using static FenixLib.IO.File;

namespace FenixLib.Core.Tests.IntegrationTests
{
    [TestFixture, Category ( "Integration" )]
    public class FntDecoding
    {

        [TestCaseSource("TestCases")]
        public void FntFileCanBeDecoded ( string fontFile, IBitmapFont referenceFont )
        {
            var assembly = Assembly.GetExecutingAssembly ();
            string folder = Path.GetDirectoryName ( assembly.Location );
            string path = Path.Combine ( folder, "TestFiles", "Fnt", fontFile );

            IBitmapFont actualFont = LoadFnt ( path );

            Assert.IsTrue ( referenceFont.Equals ( actualFont ) );
            // TODO: Assert.AreEqual ( referenceFont, actualFont ) ; will not work ecause of equality compare
        }

        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData ( "8bpp-div-simbolos.fnt", new Fake8bppDivSimbolosFnt () );
            }
        }

        private class Fake8bppDivSimbolosFnt : ComparableBitmapFont
        {
            public Fake8bppDivSimbolosFnt () : base ( CreateFontStub () )
            {
                CompareFormat = true;
                CompareGlyphs = true;

                GlyphsComparer = new GraphicComparerByDimensions ();
            }

            private static IBitmapFont CreateFontStub ()
            {
                FakeGlyphInfo[] glyphsInfo = new FakeGlyphInfo[] {
                    new FakeGlyphInfo(15, 36), new FakeGlyphInfo(21, 21), new FakeGlyphInfo(33, 34),
                    new FakeGlyphInfo(34, 44), new FakeGlyphInfo(41, 36), new FakeGlyphInfo(37, 36),
                    new FakeGlyphInfo(13, 21), new FakeGlyphInfo(23, 44), new FakeGlyphInfo(23, 44),
                    new FakeGlyphInfo(21, 22), new FakeGlyphInfo(28, 28), new FakeGlyphInfo(15, 21),
                    new FakeGlyphInfo(19, 15), new FakeGlyphInfo(14, 15), new FakeGlyphInfo(33, 44),
                    new FakeGlyphInfo(14, 24), new FakeGlyphInfo(15, 30), new FakeGlyphInfo(26, 33),
                    new FakeGlyphInfo(28, 22), new FakeGlyphInfo(26, 33), new FakeGlyphInfo(32, 36),
                    new FakeGlyphInfo(38, 36), new FakeGlyphInfo(20, 44), new FakeGlyphInfo(33, 44),
                    new FakeGlyphInfo(20, 44), new FakeGlyphInfo(28, 23), new FakeGlyphInfo(29, 15),
                    new FakeGlyphInfo(18, 15), new FakeGlyphInfo(22, 44), new FakeGlyphInfo(16, 43),
                    new FakeGlyphInfo(22, 44), new FakeGlyphInfo(28, 18), new FakeGlyphInfo(17, 36)
                };

                FontGlyph[] glyphs = new FontGlyph[glyphsInfo.Length - 1];
                for ( int i = 0 ; i < glyphs.Length ; i++ )
                {
                    Encoding encoding = System.Text.Encoding.GetEncoding ( FontEncoding.CP850.CodePage );
                    char character = encoding.GetChars ( new byte[] { ( byte ) i } )[0];
                    glyphs[i] = new FontGlyph(character, CreateStubGlyph ( glyphsInfo[i].Width,
                        glyphsInfo[i].Height ));
                }

                IBitmapFont stub = MockRepository.GenerateStub<IBitmapFont> ();
                stub.Stub ( x => x.Glyphs ).Return ( glyphs );
                stub.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.RgbIndexedPalette );

                return stub;
            }

            private static IGlyph CreateStubGlyph ( int width, int height )
            {
                IGlyph glyph = MockRepository.GenerateStub<IGlyph> ();
                glyph.Stub ( x => x.Width ).Return ( width );
                glyph.Stub ( x => x.Height ).Return ( height );
                glyph.Stub ( x => x.Palette ).Return ( null );

                return glyph;
            }

            private struct FakeGlyphInfo
            {
                public int Width { get; }
                public int Height { get; }

                public FakeGlyphInfo ( int width, int height )
                {
                    Width = width;
                    Height = height;
                }
            }
        }
    }
}
