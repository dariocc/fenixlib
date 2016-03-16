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
using System;
using FenixLib.Core;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture (Category = "Unit")]
    public class SpriteAssortmentTests
    {

        private SpriteAssortment argb32Assortment;
        private SpriteAssortment palettizedAssortment;

        private ISprite fakeSpriteArgb32;
        private ISprite fakeSpriteIndexed;

        [SetUp]
        public void SetUp ()
        {
            argb32Assortment = new SpriteAssortment ( GraphicFormat.Format32bppArgb );
            palettizedAssortment = new SpriteAssortment ( MockRepository.GenerateStub<Palette> () );

            fakeSpriteArgb32 = MockRepository.GenerateStub<ISprite> ();
            fakeSpriteArgb32.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.Format32bppArgb );

            fakeSpriteIndexed = MockRepository.GenerateStub<ISprite> ();
            fakeSpriteIndexed.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.Format8bppIndexed );
        }

        [Test]
        public void Construct_NullPaletteWhenFormatIsRgbIndexed_ThrowsException ()
        {
            Assert.That ( () => new SpriteAssortment ( GraphicFormat.Format8bppIndexed, null ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void SpriteAssortment_NotNullPaletteWhenFormatIsNotRgbIndexed_PaletteIsNotAssigned ()
        {
            var palette = MockRepository.GenerateStub<Palette> ();
            var assortment = new SpriteAssortment ( GraphicFormat.Format32bppArgb, palette );

            Assert.That ( assortment.Palette, Is.Null );
        }

        [Test]
        public void Add_GraphicFormatOfSpriteIsNotThatOfTheAssortment_ThrowsException ()
        {
            Assert.That ( () => argb32Assortment.Add ( 1, fakeSpriteIndexed ),
                Throws.InstanceOf<FormatMismatchException> () );
        }

        [Test]
        public void Add_IdExistsInAssortment_ThrowsException ()
        {
            argb32Assortment.Add ( 1, fakeSpriteArgb32 );

            Assert.That ( () => argb32Assortment.Add ( 1, fakeSpriteArgb32 ),
                Throws.InstanceOf<ArgumentException> () );
        }

        [Test]
        public void Add_AllInputOk_SpriteIsAdded ()
        {
            argb32Assortment.Add ( 100, fakeSpriteArgb32 );
            Assert.That ( argb32Assortment[100], Is.Not.Null );
        }

        [Test]
        public void Add_NullSprite_ThrowsException ()
        {
            Assert.That ( () => argb32Assortment.Add ( 100, null ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void Update_GraphicFormatOfSpriteIsNotThatOfTheAssortment_ThrowsException ()
        {
            Assert.That ( () => argb32Assortment.Update ( 1, fakeSpriteIndexed ),
                Throws.InstanceOf<FormatMismatchException> () );
        }

        [Test]
        public void Update_AllInputOk_SpriteIsUpdated ()
        {
            fakeSpriteArgb32.Description = "If I read this is wrong";
            argb32Assortment.Add ( 100, fakeSpriteArgb32 );

            const string aDescription = "Another sprite";
            var anotherFakeSprite = MockRepository.GenerateStub<ISprite> ();
            anotherFakeSprite.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.Format32bppArgb );
            anotherFakeSprite.Description = aDescription;
            anotherFakeSprite.Stub ( x => x.PivotPoints ).Return ( new PivotPoint[0] );

            // Assign an sprite to an id and then assign another
            argb32Assortment.Update ( 100, anotherFakeSprite );

            Assert.That ( argb32Assortment[100].Description, Is.EqualTo ( aDescription ) );
        }

        [Test]
        public void Update_NullSprite_ThrowsException ()
        {
            Assert.That ( () => argb32Assortment.Update ( 100, null ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void GetFreeId_AssortmentWithSprites_ReturnedIdIsNotInAssortment ()
        {
            argb32Assortment.Add ( 1, fakeSpriteArgb32 );
            argb32Assortment.Add ( 2, fakeSpriteArgb32 );
            argb32Assortment.Add ( 3, fakeSpriteArgb32 );
            argb32Assortment.Add ( 4, fakeSpriteArgb32 );
            argb32Assortment.Add ( 100, fakeSpriteArgb32 );
            argb32Assortment.Add ( 333, fakeSpriteArgb32 );

            Assert.That ( argb32Assortment.Ids, Has.No.Member ( argb32Assortment.GetFreeId () ) );
        }

        [Test]
        public void Ids_AssortmentWithSprites_ReturnsDefinedIds ()
        {
            argb32Assortment.Add ( 1, fakeSpriteArgb32 );
            argb32Assortment.Add ( 2, fakeSpriteArgb32 );
            argb32Assortment.Add ( 3, fakeSpriteArgb32 );
            argb32Assortment.Add ( 4, fakeSpriteArgb32 );
            argb32Assortment.Add ( 100, fakeSpriteArgb32 );
            argb32Assortment.Add ( 333, fakeSpriteArgb32 );

            CollectionAssert.AreEquivalent (
                new int[] { 1, 2, 3, 4, 100, 333 },
                argb32Assortment.Ids );
        }
    }
}