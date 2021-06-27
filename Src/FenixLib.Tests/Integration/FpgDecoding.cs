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
using System.Reflection;
using System.IO;
using System.Collections;
using System.Linq;
using FenixLib.Tests.Integration.Comparison;
using FenixLib.Core;
using static FenixLib.IO.NativeFile;

namespace FenixLib.Tests.Integration
{
    [TestFixture, Category ( "Integration" )]
    public class FpgDecoding
    {

        [Test, TestCaseSource ( "TestCases" )]
        public void FpgFileCanBeDecoded ( string fpgFile, ISpriteAssortment referenceAssortment )
        {
            var assembly = Assembly.GetExecutingAssembly ();
            string folder = Path.GetDirectoryName ( assembly.Location );
            string path = Path.Combine ( folder, "TestFiles", "Fpg", fpgFile );

            ISpriteAssortment actualAssortment = LoadFpg ( path );

            Assert.IsTrue ( referenceAssortment.Equals ( actualAssortment ) );
        }

        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData ( "1bpp-compressed.fpg", new MonochromeSampleAssortment () );
                yield return new TestCaseData ( "8bpp-uncompressed.fpg", new AnimalsFakeAssortment ( 8 ) );
                yield return new TestCaseData ( "8bpp-compressed.fpg", new AnimalsFakeAssortment ( 8 ) );
                yield return new TestCaseData ( "16bpp-uncompressed.fpg", new AnimalsFakeAssortment ( 16 ) );
                yield return new TestCaseData ( "16bpp-compressed.fpg", new AnimalsFakeAssortment ( 16 ) );
                yield return new TestCaseData ( "32bpp-compressed.fpg", new AnimalsFakeAssortment ( 32 ) );
                yield return new TestCaseData ( "32bpp-compressed.fpg", new AnimalsFakeAssortment ( 32 ) );
            }
        }

        private class MonochromeSampleAssortment : ComparableSpriteAssortment
        {
            public MonochromeSampleAssortment () : base ( CreateStubAssortment () )
            {
                CompareFormat = true;
                CompareElements = true;
                ElementsComparer = new SpriteComparerByDescription ( new GraphicComparerByPixels (
                    new GraphicComparerByDimensions () ) );
            }

            private static ISpriteAssortment CreateStubAssortment ()
            {
                var sprite = MockRepository.GenerateStub<ISprite> ();
                sprite.Stub ( x => x.Width ).Return ( 10 );
                sprite.Stub ( x => x.Height ).Return ( 10 );
                sprite.Description = "";
                byte[] PixelData = Enumerable.Repeat<byte> ( 255, 2 * 10 ).ToArray ();
                sprite.Stub ( x => x.PixelData ).Return ( PixelData );

                var assortment = MockRepository.GenerateStub<ISpriteAssortment> ();
                assortment.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.Format1bppMonochrome );
                assortment.Stub ( x => x[1] ).Return ( new SpriteAssortmentSprite ( 1, sprite ) );

                assortment.Stub ( x => x.Sprites ).Return ( new SpriteAssortmentSprite[] { assortment[1] } );

                return assortment;
            }
        }

        private class AnimalsFakeAssortment : ComparableSpriteAssortment
        {
            public AnimalsFakeAssortment ( int bpp ) : base ( CreateStubAssortment ( bpp ) )
            {
                ComparePalette = false;
                CompareFormat = true;
                CompareElements = true;
                ElementsComparer = new SpriteComparerByDescription ( new GraphicComparerByDimensions () );
            }

            private static ISpriteAssortment CreateStubAssortment ( int bpp )
            {
                var hippo = MockRepository.GenerateStub<ISprite> ();
                hippo.Stub ( x => x.Width ).Return ( 304 );
                hippo.Stub ( x => x.Height ).Return ( 315 );
                hippo.Description = "hippo";

                var parrot = MockRepository.GenerateStub<ISprite> ();
                parrot.Stub ( x => x.Width ).Return ( 256 );
                parrot.Stub ( x => x.Height ).Return ( 256 );
                parrot.Description = "parrot";

                var penguin = MockRepository.GenerateStub<ISprite> ();
                penguin.Stub ( x => x.Width ).Return ( 256 );
                penguin.Stub ( x => x.Height ).Return ( 256 );
                penguin.Description = "penguin";

                var assortment = MockRepository.GenerateStub<ISpriteAssortment> ();
                assortment.Stub ( x => x.GraphicFormat ).Return ( ( GraphicFormat ) bpp );
                assortment.Stub ( x => x[1] ).Return ( new SpriteAssortmentSprite ( 1, hippo ) );
                assortment.Stub ( x => x[100] ).Return ( new SpriteAssortmentSprite ( 100, parrot ) );
                assortment.Stub ( x => x[500] ).Return ( new SpriteAssortmentSprite ( 500, penguin ) );

                SpriteAssortmentSprite[] allSprites = new SpriteAssortmentSprite[] {
                    assortment[1], assortment[100], assortment[500] };

                assortment.Stub ( x => x.Sprites ).Return ( allSprites );

                return assortment;
            }
        }

    }
}

