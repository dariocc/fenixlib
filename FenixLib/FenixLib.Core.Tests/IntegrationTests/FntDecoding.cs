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

            BitmapFont actualFont = LoadFnt ( path );

            Assert.IsTrue ( referenceFont.Equals ( actualFont ) );
        }

        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData ( "8bpp-div-simbolos.fnt", new FakeDivFont () );
            }
        }

        private class FakeDivFont : ComparableBitmapFont
        {
            public FakeDivFont () : base ( CreateFontStub () )
            {
                CompareFormat = true;
                CompareGlyphs = true;
            }

            private static IBitmapFont CreateFontStub ()
            {
                Dimension[] glyphDimensions = new Dimension[] {
                    new Dimension(15, 36),
                    new Dimension(21, 21),
                    new Dimension(33, 34),
                    new Dimension(34, 44),
                    new Dimension(41, 36),
                    new Dimension(37, 36),
                    new Dimension(13, 21),
                    new Dimension(23, 44),
                    new Dimension(23, 44),
                    new Dimension(21, 22),
                    new Dimension(28, 28),
                    new Dimension(15, 21),
                    new Dimension(19, 15),
                    new Dimension(14, 15),
                    new Dimension(33, 44),
                    new Dimension(14, 24),
                    new Dimension(15, 30),
                    new Dimension(26, 33),
                    new Dimension(28, 22),
                    new Dimension(26, 33),
                    new Dimension(32, 36),
                    new Dimension(38, 36),
                    new Dimension(20, 44),
                    new Dimension(20, 44),
                    new Dimension(33, 44),
                    new Dimension(20, 44),
                    new Dimension(28, 23),
                    new Dimension(29, 15),
                    new Dimension(18, 15),
                    new Dimension(22, 44),
                    new Dimension(16, 43),
                    new Dimension(22, 44),
                    new Dimension(28, 18),
                    new Dimension(17, 36),
                };

                FontGlyph[] glyphs = new FontGlyph[glyphDimensions.Length - 1];
                for ( int i = 0 ; i < glyphs.Length ; i++ )
                {
                    Encoding encoding = Encoding.GetEncoding ( FontEncoding.CP850.CodePage );
                    char character = encoding.GetChars ( new byte[] { ( byte ) i } )[0];
                    glyphs[i] = new FontGlyph(character, CreateStubGlyph ( glyphDimensions[i].Width,
                        glyphDimensions[i].Height ));
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

            private struct Dimension
            {
                public int Width { get; }
                public int Height { get; }

                public Dimension ( int width, int height )
                {
                    Width = width;
                    Height = height;
                }
            }

        }
    }
}
