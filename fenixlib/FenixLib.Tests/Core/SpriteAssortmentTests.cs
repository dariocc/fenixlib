/**
 *    Copyright (c) 2016 Darío Cutillas Carrillo
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 * 
 *        http://www.apache.org/licenses/LICENSE-2.0
 * 
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
using NUnit.Framework;
using System;
using Moq;
using FenixLib;

namespace FenixLibTests
{
    [TestFixture ( Category = "Unit" )]
    public class SpriteAssortmentTests
    {

        private SpriteAssortment argb32Assortment;
        private SpriteAssortment palettizedAssortment;

        private Mock<ISprite> fakeSpriteArgb32;
        private Mock<ISprite> fakeSpriteIndexed;

        private ISprite FakeSpriteArgb32 { get => fakeSpriteArgb32.Object; }
        private ISprite FakeSpriteIndexed { get => fakeSpriteIndexed.Object; }

        [SetUp]
        public void SetUp ()
        {
            argb32Assortment = new SpriteAssortment ( GraphicFormat.Format32bppArgb );
            palettizedAssortment = new SpriteAssortment ( new Mock<Palette> ().Object );

            fakeSpriteArgb32 = new Mock<ISprite> ();
            fakeSpriteArgb32.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format32bppArgb );

            fakeSpriteIndexed = new Mock<ISprite> ();
            fakeSpriteIndexed.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format8bppIndexed );
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
            var palette = new Mock<Palette> ();
            var assortment = new SpriteAssortment ( GraphicFormat.Format32bppArgb, palette.Object );

            Assert.That ( assortment.Palette, Is.Null );
        }

        [Test]
        public void Add_GraphicFormatOfSpriteIsNotThatOfTheAssortment_ThrowsException ()
        {
            Assert.That ( () => argb32Assortment.Add ( 1, FakeSpriteIndexed ),
                Throws.InstanceOf<FormatMismatchException> () );
        }

        [Test]
        public void Add_IdExistsInAssortment_ThrowsException ()
        {
            argb32Assortment.Add ( 1, FakeSpriteArgb32 );

            Assert.That ( () => argb32Assortment.Add ( 1, FakeSpriteArgb32 ),
                Throws.InstanceOf<ArgumentException> () );
        }

        [Test]
        public void Add_AllInputOk_SpriteIsAdded ()
        {
            argb32Assortment.Add ( 100, FakeSpriteArgb32 );
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
            Assert.That ( () => argb32Assortment.Update ( 1, FakeSpriteIndexed ),
                Throws.InstanceOf<FormatMismatchException> () );
        }

        [Test]
        public void Update_AllInputOk_SpriteIsUpdated ()
        {
            FakeSpriteArgb32.Description = "If I read this is wrong";
            argb32Assortment.Add ( 100, FakeSpriteArgb32 );

            const string aDescription = "Another sprite";
            var anotherFakeSprite = new Mock<ISprite> ();
            anotherFakeSprite.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format32bppArgb );
            anotherFakeSprite.Setup ( x => x.Description ).Returns ( aDescription );
            anotherFakeSprite.Setup ( x => x.PivotPoints ).Returns ( new PivotPoint[0] );

            // Assign an sprite to an id and then assign another
            argb32Assortment.Update ( 100, anotherFakeSprite.Object );

            Assert.That ( argb32Assortment[100].Description, Is.EqualTo ( aDescription ) );
        }

        [Test]
        public void Update_NullSprite_ThrowsException ()
        {
            Assert.That ( () => argb32Assortment.Update ( 100, null ),
                Throws.ArgumentNullException );
        }

        [Test]
        public void GetFreeId_AssortmentWithSprites_ReturnsedIdIsNotInAssortment ()
        {
            argb32Assortment.Add ( 1, FakeSpriteArgb32 );
            argb32Assortment.Add ( 2, FakeSpriteArgb32 );
            argb32Assortment.Add ( 3, FakeSpriteArgb32 );
            argb32Assortment.Add ( 4, FakeSpriteArgb32 );
            argb32Assortment.Add ( 100, FakeSpriteArgb32 );
            argb32Assortment.Add ( 333, FakeSpriteArgb32 );

            Assert.That ( argb32Assortment.Ids, Has.No.Member ( argb32Assortment.GetFreeId () ) );
        }

        [Test]
        public void Ids_AssortmentWithSprites_ReturnssDefinedIds ()
        {
            argb32Assortment.Add ( 1, FakeSpriteArgb32 );
            argb32Assortment.Add ( 2, FakeSpriteArgb32 );
            argb32Assortment.Add ( 3, FakeSpriteArgb32 );
            argb32Assortment.Add ( 4, FakeSpriteArgb32 );
            argb32Assortment.Add ( 100, FakeSpriteArgb32 );
            argb32Assortment.Add ( 333, FakeSpriteArgb32 );

            CollectionAssert.AreEquivalent (
                new int[] { 1, 2, 3, 4, 100, 333 },
                argb32Assortment.Ids );
        }
    }
}
