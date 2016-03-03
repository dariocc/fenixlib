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
using System;
using System.Collections;

namespace FenixLib.Core.Tests.UnitTests
{
    [TestFixture ( Category = "Unit" )]
    class PaletteTests
    {
        private static Palette palette;

        [SetUp]
        public static void SetUp ()
        {
            PaletteColor[] colors = CreateSampleColors ();
            palette = new Palette ( colors );
        }

        [Test]
        public void Construct_NullColors_ArgumentNullException ()
        {
            Assert.Throws<ArgumentNullException> ( () => new Palette ( null ) );
        }

        [TestCase ( 2 )]
        [TestCase ( 257 )]
        public void Construct_OtherThan256Colors_ArgumentException ( int numberOfColors )
        {
            ArgumentException e = Assert.Throws<ArgumentException> (
                () => new Palette ( new PaletteColor[numberOfColors] ) );
        }

        [TestCase ( -1 )]
        [TestCase ( 256 )]
        public void IndexerGetter_IndexOutOfRange_ColorIndexOutOfRangeException ( int index )
        {
            TestDelegate colorAtIndex = () => new Func<PaletteColor> ( () =>
            {
                return palette[index];
            } ) ();
            Assert.Throws<ColorIndexOutOfRangeException> ( colorAtIndex );
        }

        [TestCase ( -1 )]
        [TestCase ( 256 )]
        public void IndexerSetter_IndexOutOfRange_ColorIndexOutOfRangeException ( int index )
        {
            TestDelegate changeColorAtIndex = () => new Action ( () =>
            {
                palette[index] = new PaletteColor ( 0, 0, 0 );
            } ) ();
            Assert.Throws<ColorIndexOutOfRangeException> ( changeColorAtIndex );
        }

        [Test]
        public void GetCopy_ColorsAreEqualReferencesNot ()
        {
            Palette copy = palette.GetCopy ();
            for ( int i = 0 ; i < palette.Colors.Length ; i++ )
            {
                Assert.AreEqual ( palette.Colors[i], copy.Colors[i] );
            }
            Assert.AreNotSame ( palette, copy );
        }

        [Test, TestCaseSource ( typeof( PaletteComparisonCasesFactory), "TestCases" )]
        public bool Equals_ComparisonCases ( Palette paletteA, Palette paletteB)
        {
            return paletteA.Equals ( paletteB );
        }

        [Test, TestCaseSource ( typeof ( PaletteComparisonCasesFactory ), "TestCases" )]
        public bool LogicalEquality_ComparisonCases ( Palette paletteA, Palette paletteB )
        {
            return ( paletteA == paletteB );
        }

        [Test, TestCaseSource ( typeof ( PaletteComparisonCasesFactory ), "TestCases" )]
        public bool LogicalInequality_ComparisonCases ( Palette paletteA, Palette paletteB )
        {
            return !( paletteA != paletteB );
        }

        private static PaletteColor[] CreateSampleColors()
        {
            PaletteColor[] colors = new PaletteColor[256];
            // A palette with some color variation
            for ( int i = 0 ; i < colors.Length ; i++ )
            {
                colors[i] = new PaletteColor ( i, i / 2, i / 3 );
            }

            return colors;
        }

        private class PaletteComparisonCasesFactory
        {
            private static Palette palette;
            private static Palette equivalentPalette;
            private static Palette differentPalette;

            static PaletteComparisonCasesFactory()
            {
                PaletteColor[] colors = CreateSampleColors ();
                PaletteColor[] colorsCopy = new PaletteColor[256];
                Array.Copy ( colors, colorsCopy, 256 );


                palette = new Palette ( colors );
                equivalentPalette = new Palette ( colorsCopy );
                differentPalette = new Palette ( new PaletteColor[256] );
            }
            static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData ( palette, palette )
                        .Returns ( true );
                    yield return new TestCaseData ( palette, equivalentPalette )
                        .Returns ( true );
                    yield return new TestCaseData ( equivalentPalette, palette )
                        .Returns ( true );
                    yield return new TestCaseData ( palette, differentPalette )
                        .Returns ( false );
                }
            }
        }
    }
}
