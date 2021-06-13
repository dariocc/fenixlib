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
using FenixLib.Core;
using Moq;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture ( Category = "Unit" )]
    public class SpriteAssortmentSpriteTests
    {
        private Mock<ISprite> fakeSprite;
        private SpriteAssortmentSprite spriteAssortmentSprite;
        private SpriteAssortmentSprite equivalentSprite;

        private ISprite FakeSprite { get => fakeSprite.Object; }

        [SetUp]
        public void SetUp ()
        {
            fakeSprite = new Mock<ISprite> ();
            fakeSprite.Setup ( x => x.Width ).Returns ( 1 );
            fakeSprite.Setup ( x => x.Height ).Returns ( 1 );
            fakeSprite.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format32bppArgb );

            spriteAssortmentSprite = new SpriteAssortmentSprite ( 10, FakeSprite );
            equivalentSprite = new SpriteAssortmentSprite ( 10, FakeSprite );
        }

        // Verification of propeties delegation

        [Test]
        public void DescriptionGet_DescriptionModifierInBaseSprite_ReturnssSameDescriptionAsBaseSprite ()
        {
            FakeSprite.Description = "A description";
            Assert.That ( spriteAssortmentSprite.Description, Is.EqualTo ( "A description" ) );
        }

        [Test]
        public void DescriptionSet_PropertyIsModified_ChangeIsReflectedInBaseSprite ()
        {
            spriteAssortmentSprite.Description = "A description";
            Assert.That ( FakeSprite.Description, Is.EqualTo ( "A description" ) );
        }

        [Test]
        public void PivotPointsGet_SameAsBaseSprite ()
        {
            fakeSprite.Setup ( x => x.PivotPoints ).Returns ( new PivotPoint[0] );
            Assert.That ( spriteAssortmentSprite.PivotPoints, Is.SameAs ( FakeSprite.PivotPoints ) );
        }

        [Test]
        public void PixelDataGet_SameAsBaseSprite ()
        {
            var bytes = new byte[1];

            fakeSprite.Setup ( x => x.PixelData ).Returns ( bytes );
            Assert.That ( spriteAssortmentSprite.PixelData, Is.SameAs ( bytes ) );
        }

        [Test]
        public void PaletteGet_SameAsBaseSprite ()
        {
            var palette = new Palette ();

            fakeSprite.Setup ( x => x.Palette ).Returns ( palette );
            Assert.That ( spriteAssortmentSprite.Palette, Is.SameAs ( palette ) );
        }

        [Test]
        public void GraphicFormatGet_SameAsBaseSprite ()
        {
            Assert.That ( spriteAssortmentSprite.GraphicFormat, 
                Is.SameAs ( FakeSprite.GraphicFormat ) );
        }

        // Verification of delegated methods invocation

        [Test]
        public void ClearPivotPoints_CallsMethodInBaseSprite ()
        {
            var mock = new Mock<ISprite> ();

            var s = new SpriteAssortmentSprite ( 0, mock.Object );
            s.ClearPivotPoints ();

            mock.Verify ( x => x.ClearPivotPoints () );
        }

        [Test]
        public void DefinePivotPoint_CallsMethodInBaseSprite ()
        {
            var mock = new Mock<ISprite> ();

            var s = new SpriteAssortmentSprite ( 0, mock.Object );
            s.DefinePivotPoint ( 2, 100, 200 );

            mock.Verify ( x => x.DefinePivotPoint (
                It.Is<int> ( i => i == 2 ),
                It.Is<int> ( i  => i == 100 ),
                It.Is<int> (  i => i == 200 ) )
                );
        }

        [Test]
        public void DeletePivotPoint_CallsMethodInBaseSprite ()
        {
            var mock = new Mock<ISprite> ();
            var s = new SpriteAssortmentSprite ( 0, mock.Object );
            s.DeletePivotPoint ( 2 );

            mock.Verify ( x => x.DeletePivotPoint (
                It.Is<int>( i => i == 2 )
                ) );
        }

        [Test]
        public void GetPivotPoint_CallsMethodInBaseSprite ()
        {
            var mock = new Mock<ISprite> ();
            var s = new SpriteAssortmentSprite ( 0, mock.Object );
            s.GetPivotPoint ( 2 );

            mock.Verify ( x => x.GetPivotPoint (
                It.Is<int>( i => i == 2 )) );
        }

        [Test]
        public void FindFreePivotPointId_CallsMethodInBaseSprite ()
        {
            var mock = new Mock<ISprite> ();
            var s = new SpriteAssortmentSprite ( 0, mock.Object );
            s.FindFreePivotPointId ( 2, Sprite.SearchDirection.Backward );

            mock.Verify ( x => x.FindFreePivotPointId (
                It.Is<int>( i => i == 2),
                It.Is<Sprite.SearchDirection>( d => d == Sprite.SearchDirection.Backward )
                ) == null);
        }

        [Test]
        public void IsPivotPointDefined_CallsMethodInBaseSprite ()
        {
            var mock = new Mock<ISprite> ();
            var s = new SpriteAssortmentSprite ( 0, mock.Object );
            s.IsPivotPointDefined ( 2 );

            mock.Verify ( x => x.IsPivotPointDefined (
                It.Is<int>( i => i== 2 ) ) == false );
        }

        // Equality & Hash

        [Test]
        public void Equals_NullSprite_ReturnssFalse ()
        {
            Assert.That ( spriteAssortmentSprite.Equals ( null ), Is.False );
        }

        [Test]
        public void Equals_SpriteWithSameId_ReturnssTrue ()
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