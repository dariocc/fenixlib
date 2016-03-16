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
using FenixLib.Core;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture ( Category = "Unit" )]
    public class SpriteAssortmentSpriteTests
    {
        private ISprite fakeSprite;
        private SpriteAssortmentSprite spriteAssortmentSprite;
        private SpriteAssortmentSprite equivalentSprite;

        [SetUp]
        public void SetUp ()
        {
            fakeSprite = MockRepository.GenerateStub<ISprite> ();
            fakeSprite.Stub ( x => x.Width ).Return ( 1 );
            fakeSprite.Stub ( x => x.Height ).Return ( 1 );
            fakeSprite.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.Format32bppArgb );

            spriteAssortmentSprite = new SpriteAssortmentSprite ( 10, fakeSprite );
            equivalentSprite = new SpriteAssortmentSprite ( 10, fakeSprite );
        }

        [Test]
        public void DescriptionGetter_DescriptionModifierInBaseSprite_ReturnsSameDescriptionAsBaseSprite ()
        {
            fakeSprite.Description = "A description";
            Assert.That ( spriteAssortmentSprite.Description, Is.EqualTo ( "A description" ) );
        }

        [Test]
        public void DescriptionSetter_PropertyIsModified_ChangeIsReflectedInBaseSprite ()
        {
            spriteAssortmentSprite.Description = "A description";
            Assert.That ( fakeSprite.Description, Is.EqualTo ( "A description" ) );
        }

        [Test]
        public void PivotPointsGetter_SameAsBaseSprite ()
        {
            fakeSprite.Stub ( x => x.PivotPoints ).Return ( new PivotPoint[0] );
            Assert.That ( spriteAssortmentSprite.PivotPoints, Is.EqualTo ( fakeSprite.PivotPoints ) );
        }

        [Test]
        public void Equals_NullSprite_ReturnsFalse ()
        {
            Assert.That ( spriteAssortmentSprite.Equals ( null ), Is.False );
        }

        [Test]
        public void Equals_SpriteWithSameId_ReturnsTrue ()
        {
            Assert.That ( spriteAssortmentSprite.Equals ( equivalentSprite ), Is.True );
        }

        [Test]
        public void GetHashCode_TwoSpritesWithSameId_AreEqual ()
        {

            Assert.AreEqual ( spriteAssortmentSprite.GetHashCode (),
                equivalentSprite.GetHashCode () );
        }
    }
}