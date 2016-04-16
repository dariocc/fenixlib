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

        // Verification of propeties delegation

        [Test]
        public void DescriptionGet_DescriptionModifierInBaseSprite_ReturnsSameDescriptionAsBaseSprite ()
        {
            fakeSprite.Description = "A description";
            Assert.That ( spriteAssortmentSprite.Description, Is.EqualTo ( "A description" ) );
        }

        [Test]
        public void DescriptionSet_PropertyIsModified_ChangeIsReflectedInBaseSprite ()
        {
            spriteAssortmentSprite.Description = "A description";
            Assert.That ( fakeSprite.Description, Is.EqualTo ( "A description" ) );
        }

        [Test]
        public void PivotPointsGet_SameAsBaseSprite ()
        {
            fakeSprite.Stub ( x => x.PivotPoints ).Return ( new PivotPoint[0] );
            Assert.That ( spriteAssortmentSprite.PivotPoints, Is.SameAs ( fakeSprite.PivotPoints ) );
        }

        [Test]
        public void PixelDataGet_SameAsBaseSprite ()
        {
            var bytes = new byte[1];

            fakeSprite.Stub ( x => x.PixelData ).Return ( bytes );
            Assert.That ( spriteAssortmentSprite.PixelData, Is.SameAs ( bytes ) );
        }

        [Test]
        public void PaletteGet_SameAsBaseSprite ()
        {
            var palette = new Palette ();

            fakeSprite.Stub ( x => x.Palette ).Return ( palette );
            Assert.That ( spriteAssortmentSprite.Palette, Is.SameAs ( palette ) );
        }

        [Test]
        public void GraphicFormatGet_SameAsBaseSprite ()
        {
            Assert.That ( spriteAssortmentSprite.GraphicFormat, 
                Is.SameAs ( fakeSprite.GraphicFormat ) );
        }

        // Verification of delegated methods invocation

        [Test]
        public void ClearPivotPoints_CallsMethodInBaseSprite ()
        {
            var mock = MockRepository.GenerateMock<ISprite> ();
            mock.Expect ( x => x.ClearPivotPoints () );

            var s = new SpriteAssortmentSprite ( 0, mock );
            s.ClearPivotPoints ();

            mock.VerifyAllExpectations ();
        }

        [Test]
        public void DefinePivotPoint_CallsMethodInBaseSprite ()
        {
            var mock = MockRepository.GenerateMock<ISprite> ();
            mock.Expect ( x => x.DefinePivotPoint (
                Arg<int>.Is.Equal ( 2 ),
                Arg<int>.Is.Equal ( 100 ),
                Arg<int>.Is.Equal ( 200 ) )
                );

            var s = new SpriteAssortmentSprite ( 0, mock );
            s.DefinePivotPoint ( 2, 100, 200 );

            mock.VerifyAllExpectations ();
        }

        [Test]
        public void DeletePivotPoint_CallsMethodInBaseSprite ()
        {
            var mock = MockRepository.GenerateMock<ISprite> ();
            mock.Expect ( x => x.DeletePivotPoint (
                Arg<int>.Is.Equal ( 2 )
                ) );

            var s = new SpriteAssortmentSprite ( 0, mock );
            s.DeletePivotPoint ( 2 );

            mock.VerifyAllExpectations ();
        }

        [Test]
        public void GetPivotPoint_CallsMethodInBaseSprite ()
        {
            var mock = MockRepository.GenerateMock<ISprite> ();
            mock.Expect ( x => x.GetPivotPoint (
                Arg<int>.Is.Equal ( 2 ) )
                ).Return ( new PivotPoint () );

            var s = new SpriteAssortmentSprite ( 0, mock );
            s.GetPivotPoint ( 2 );

            mock.VerifyAllExpectations ();
        }

        [Test]
        public void FindFreePivotPointId_CallsMethodInBaseSprite ()
        {
            var mock = MockRepository.GenerateMock<ISprite> ();
            mock.Expect ( x => x.FindFreePivotPointId (
                Arg<int>.Is.Equal ( 2 ),
                Arg<Sprite.SearchDirection>.Is.Equal ( Sprite.SearchDirection.Backward )
                ) ).Return ( null );

            var s = new SpriteAssortmentSprite ( 0, mock );
            s.FindFreePivotPointId ( 2, Sprite.SearchDirection.Backward );

            mock.VerifyAllExpectations ();
        }

        [Test]
        public void IsPivotPointDefined_CallsMethodInBaseSprite ()
        {
            var mock = MockRepository.GenerateMock<ISprite> ();
            mock.Expect ( x => x.IsPivotPointDefined (
                Arg<int>.Is.Equal ( 2 ) ) ).Return ( false );

            var s = new SpriteAssortmentSprite ( 0, mock );
            s.IsPivotPointDefined ( 2 );

            mock.VerifyAllExpectations ();
        }

        // Equality & Hash

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