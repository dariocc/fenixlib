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

using FenixLib.Core;
using System;
using System.Collections.Generic;

namespace FenixLib.Tests.IntegrationTests
{
    [TestFixture, Category ( "Integration" )]
    public class FpgDecoding
    {

        [Test, TestCaseSource ( typeof ( TestCasesFactory ), "TestCases" )]
        public void FpgFileCanBeDecoded ( string resourceName, ISpriteAsset referenceAsset )
        {
            var assembly = Assembly.GetExecutingAssembly ();
            string folder = Path.GetDirectoryName ( assembly.Location );
            string path = Path.Combine ( folder, "TestFiles", "Fpg", resourceName );

            SpriteAsset actualAsset = IO.File.LoadFpg ( path );

            Assert.AreEqual ( referenceAsset.GraphicFormat, actualAsset.GraphicFormat );

            // Validate sprite
            foreach ( var sprite in actualAsset )
            {
                CompareSprites ( referenceAsset[sprite.Id], sprite );
            }

            // Validate the BitsPerPixel

            // Validate the sprites have been correctly loaded


            // Assert.AreEqual ( depth, (int) fpg.GraphicFormat );
            /*Assert.AreEqual ( 3, fpg.Sprites.Count );
            Assert.IsNotNull ( fpg.Sprites );

            // Validate the dimensions
            Assert.AreEqual ( 304, fpg[1].Width );
            Assert.AreEqual ( 256, fpg[100].Width );
            Assert.AreEqual ( 256, fpg[500].Width );
            Assert.AreEqual ( 315, fpg[1].Height );
            Assert.AreEqual ( 256, fpg[100].Height );
            Assert.AreEqual ( 256, fpg[500].Height );

            // Validate description
            Assert.AreEqual ( "hippo", fpg[1].Description );
            Assert.AreEqual ( "parrot", fpg[100].Description );
            Assert.AreEqual ( "penguin", fpg[500].Description );*/

            // Validate control points
            //Assert.AreEqual ( 2, fpg[500].PivotPoints.Count );
        }

        private static void CompareSprites ( ISprite expected, ISprite actual )
        {
            Assert.AreEqual ( expected.Width, actual.Width );
            Assert.AreEqual ( expected.Height, actual.Height );
            Assert.AreEqual ( expected.Description, actual.Description );
        }

        public class TestCasesFactory
        {
            private class AnimalAsset : ISpriteAsset
            {
                private ISprite hippo;
                private ISprite penguin;
                private ISprite parrot;

                private GraphicFormat format;

                public AnimalAsset ( int bpp )
                {
                    format = ( GraphicFormat ) bpp;

                    hippo = MockRepository.GenerateStub<ISprite> ();
                    hippo.Stub ( x => x.Width ).Return ( 304 );
                    hippo.Stub ( x => x.Height ).Return ( 315 );
                    hippo.Description = "hippo";

                    parrot = MockRepository.GenerateStub<ISprite> ();
                    parrot.Stub ( x => x.Width ).Return ( 256 );
                    parrot.Stub ( x => x.Height ).Return ( 256 );
                    parrot.Description = "parrot";

                    penguin = MockRepository.GenerateStub<ISprite> ();
                    penguin.Stub ( x => x.Width ).Return ( 256 );
                    penguin.Stub ( x => x.Height ).Return ( 256 );
                    penguin.Description = "penguin";
                }

                public ISprite this[int id]
                {
                    get
                    {
                        if ( id == 1 )
                            return hippo;
                        else if ( id == 100 )
                            return parrot;
                        else if ( id == 500 )
                            return penguin;
                        return null;
                    }
                }

                public GraphicFormat GraphicFormat => format;

                public IEnumerable<int> Ids
                {
                    get
                    {
                        throw new NotImplementedException ();
                    }
                }

                public Palette Palette
                {
                    get
                    {
                        throw new NotImplementedException ();
                    }
                }

                public ICollection<SpriteAssetElement> Sprites
                {
                    get
                    {
                        throw new NotImplementedException ();
                    }
                }

                public void Add ( int id, ISprite sprite )
                {
                    throw new NotImplementedException ();
                }

                public IEnumerator<SpriteAssetElement> GetEnumerator ()
                {
                    throw new NotImplementedException ();
                }

                public int GetFreeId ()
                {
                    throw new NotImplementedException ();
                }

                public void Update ( int id, ISprite sprite )
                {
                    throw new NotImplementedException ();
                }

                IEnumerator IEnumerable.GetEnumerator ()
                {
                    throw new NotImplementedException ();
                }
            }

            private class AssetDecorator : ISpriteAsset
            {
                ISpriteAsset decorated;

                public AssetDecorator ( ISpriteAsset decorated)
                {
                    this.decorated = decorated;
                }

                public ISprite this[int id]
                {
                    get
                    {
                        return decorated[id];
                    }
                }

                public GraphicFormat GraphicFormat
                {
                    get
                    {
                        return decorated.GraphicFormat;
                    }
                }

                public IEnumerable<int> Ids
                {
                    get
                    {
                        return decorated.Ids;
                    }
                }

                public Palette Palette
                {
                    get
                    {
                        return decorated.Palette;
                    }
                }

                public ICollection<SpriteAssetElement> Sprites
                {
                    get
                    {
                        return decorated.Sprites;
                    }
                }

                public void Add ( int id, ISprite sprite )
                {
                    decorated.Add ( id, sprite );
                }

                public IEnumerator<SpriteAssetElement> GetEnumerator ()
                {
                    return decorated.GetEnumerator ();
                }

                public int GetFreeId ()
                {
                    return decorated.GetFreeId ();
                }

                public void Update ( int id, ISprite sprite )
                {
                    decorated.Update ( id, sprite );
                }

                IEnumerator IEnumerable.GetEnumerator ()
                {
                    return decorated.GetEnumerator ();
                }
            }
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData ( "8bpp-uncompressed.fpg", FakeAnimalAsset ( 8 ) );
                    yield return new TestCaseData ( "8bpp-compressed.fpg", FakeAnimalAsset ( 8 ) );
                    yield return new TestCaseData ( "16bpp-uncompressed.fpg", FakeAnimalAsset ( 16 ) );
                    yield return new TestCaseData ( "16bpp-compressed.fpg", FakeAnimalAsset ( 16 ) );
                    yield return new TestCaseData ( "32bpp-compressed.fpg", FakeAnimalAsset ( 32 ) );
                    yield return new TestCaseData ( "32bpp-compressed.fpg", FakeAnimalAsset ( 32 ) );
                }
            }

            private static ISpriteAsset FakeAnimalAsset ( int bpp )
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
                asset.Stub ( x => x[1] ).Return ( hippo );
                asset.Stub ( x => x[100] ).Return ( parrot );
                asset.Stub ( x => x[500] ).Return ( penguin );

                return new AssetDecorator ( asset );
            }
        }
        /*
        [Test]
        public void DecodeFpg_1bppCompressed ()
        {
            SpriteAsset fpg = File.LoadFpg ( "./Fpg/1bpp-compressed.fpg" );
            Assert.AreEqual ( fpg[1].Width, 10 );
            Assert.AreEqual ( fpg[1].Height, 10 );
            Assert.AreEqual ( fpg.GraphicFormat, 1);
            Assert.AreEqual ( fpg.Sprites.Count, 1 );
        }*/
    }
}
