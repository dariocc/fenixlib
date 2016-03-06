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
        [TestCase ( "8bpp-div-extendid.fnt" )]
        [TestCase ( "8bpp-div-mayuscul.fnt" )]
        [TestCase ( "8bpp-div-minusc.fnt" )]
        void FntFileCanBeDecoded ( string fontFile, IBitmapFont referenceFont )
        {
            var assembly = Assembly.GetExecutingAssembly ();
            string folder = Path.GetDirectoryName ( assembly.Location );
            string path = Path.Combine ( folder, "TestFiles", "Fpg", fontFile );

            BitmapFont actualFont = LoadFnt ( path );

            Assert.IsTrue ( referenceFont.Equals ( actualFont ) );
        }

        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData ( "8bpp-div-extendid.fnt", new FakeDivFont () );
                yield return new TestCaseData ( "8bpp-div-mayuscul.fnt", new FakeDivFont () );
                yield return new TestCaseData ( "8bpp-div-minusc.fnt", new FakeDivFont () );
            }
        }

        private class FakeDivFont : ComparableBitmapFont
        {
            public FakeDivFont () : base ( CreateFontStub () ) { }

            private static IBitmapFont CreateFontStub ()
            {
                return null;
            }
        }
    }
}
