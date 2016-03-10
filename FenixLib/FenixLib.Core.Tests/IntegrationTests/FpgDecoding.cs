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
using FenixLib.Core.Tests.IntegrationTests.Comparison;
using static FenixLib.IO.File;

namespace FenixLib.Core.Tests.IntegrationTests
{
    [TestFixture, Category ( "Integration" )]
    public class FpgDecoding
    {

        [Test, TestCaseSource ( "TestCases" )]
        public void FpgFileCanBeDecoded ( string fpgFile, ISpriteAsset referenceAsset )
        {
            var assembly = Assembly.GetExecutingAssembly ();
            string folder = Path.GetDirectoryName ( assembly.Location );
            string path = Path.Combine ( folder, "TestFiles", "Fpg", fpgFile );

            ISpriteAsset actualAsset = LoadFpg ( path );

            Assert.IsTrue ( referenceAsset.Equals ( actualAsset ) );
        }

        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData ( "1bpp-compressed.fpg", new MonochromeSampleAsset () );
                yield return new TestCaseData ( "8bpp-uncompressed.fpg", new AnimalsFakeAsset ( 8 ) );
                yield return new TestCaseData ( "8bpp-compressed.fpg", new AnimalsFakeAsset ( 8 ) );
                yield return new TestCaseData ( "16bpp-uncompressed.fpg", new AnimalsFakeAsset ( 16 ) );
                yield return new TestCaseData ( "16bpp-compressed.fpg", new AnimalsFakeAsset ( 16 ) );
                yield return new TestCaseData ( "32bpp-compressed.fpg", new AnimalsFakeAsset ( 32 ) );
                yield return new TestCaseData ( "32bpp-compressed.fpg", new AnimalsFakeAsset ( 32 ) );
            }
        }

        private class MonochromeSampleAsset : ComparableAsset
        {
            public MonochromeSampleAsset () : base ( CreateStubAsset () )
            {
                CompareFormat = true;
                CompareElements = true;
                ElementsComparer = new SpriteComparerByDescription ( new GraphicComparerByPixels (
                    new GraphicComparerByDimensions () ) );
            }

            private static ISpriteAsset CreateStubAsset ()
            {
                var sprite = MockRepository.GenerateStub<ISprite> ();
                sprite.Stub ( x => x.Width ).Return ( 10 );
                sprite.Stub ( x => x.Height ).Return ( 10 );
                sprite.Description = "";
                byte[] PixelData = Enumerable.Repeat<byte> ( 255, 2 * 10 ).ToArray ();
                sprite.Stub ( x => x.PixelData ).Return ( PixelData );

                var asset = MockRepository.GenerateStub<ISpriteAsset> ();
                asset.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.Monochrome );
                asset.Stub ( x => x[1] ).Return ( new SpriteAssetElement ( 1, sprite ) );

                asset.Stub ( x => x.Sprites ).Return ( new SpriteAssetElement[] { asset[1] } );

                return asset;
            }
        }

        private class AnimalsFakeAsset : ComparableAsset
        {
            public AnimalsFakeAsset ( int bpp ) : base ( CreateStubAsset ( bpp ) )
            {
                ComparePalette = false;
                CompareFormat = true;
                CompareElements = true;
                ElementsComparer = new SpriteComparerByDescription ( new GraphicComparerByDimensions () );
            }

            private static ISpriteAsset CreateStubAsset ( int bpp )
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

                var asset = MockRepository.GenerateStub<ISpriteAsset> ();
                asset.Stub ( x => x.GraphicFormat ).Return ( ( GraphicFormat ) bpp );
                asset.Stub ( x => x[1] ).Return ( new SpriteAssetElement ( 1, hippo ) );
                asset.Stub ( x => x[100] ).Return ( new SpriteAssetElement ( 100, parrot ) );
                asset.Stub ( x => x[500] ).Return ( new SpriteAssetElement ( 500, penguin ) );

                SpriteAssetElement[] allSprites = new SpriteAssetElement[] {
                    asset[1], asset[100], asset[500] };

                asset.Stub ( x => x.Sprites ).Return ( allSprites );

                return asset;
            }
        }

    }
}

