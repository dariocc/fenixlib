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

namespace FenixLib.Core.Tests.UnitTests
{
    [TestFixture ( Category = "Unit" )]
    public class SpriteAssetSpriteTests
    {
        private ISprite fakeSprite;
        private SpriteAssetSprite aSprite;
        private SpriteAssetSprite equivalentSprite;

        [SetUp]
        public void SetUp ()
        {
            fakeSprite = MockRepository.GenerateStub<ISprite> ();
            fakeSprite.Stub ( x => x.Width ).Return ( 1 );
            fakeSprite.Stub ( x => x.Height ).Return ( 1 );
            fakeSprite.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.ArgbInt32 );

            aSprite = new SpriteAssetSprite ( 10, fakeSprite );
            equivalentSprite = new SpriteAssetSprite ( 10, fakeSprite );
        }

        [Test]
        public void Equals_NullSprite_ReturnsFalse ()
        {
            Assert.That ( aSprite.Equals ( null ), Is.False );
        }

        [Test]
        public void Equals_SpriteWithSameId_ReturnsTrue ()
        {
            Assert.That ( aSprite.Equals ( equivalentSprite ), Is.True );
        }

        [Test]
        public void GetHashCode_TwoSpritesWithSameId_AreEqual ()
        {
            
            Assert.AreEqual ( aSprite.GetHashCode (), 
                equivalentSprite.GetHashCode () );
        }
    }
}