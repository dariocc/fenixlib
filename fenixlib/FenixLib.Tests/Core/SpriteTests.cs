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

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture (Category = "Unit")]
    public class SpriteTests
    {

        private Sprite sprite;

        [SetUp]
        public void SetUp ()
        {
            var fakeGraphic = new Mock<IGraphic> ();
            sprite = new Sprite ( fakeGraphic.Object );
        }

        [Test]
        public void Construct_NullGraphic_ThrowsException ()
        {
            Assert.That ( () => new Sprite ( null ), Throws.ArgumentNullException );
        }

        [Test]
        public void DefinePivotPoint_ExistingId_PivotPointIsReplaced ()
        {
            // Define pivot point with id 10
            sprite.DefinePivotPoint ( 10, 100, 100 );
            PivotPoint reference = new PivotPoint ( 10, 200, 200 );

            // Redefine pivot point with id 10
            sprite.DefinePivotPoint ( 10, reference.X, reference.Y );

            PivotPoint point = sprite.GetPivotPoint ( 10 );

            Assert.That ( point, Is.EqualTo ( reference ) );
        }

        [Test]
        public void DefinePivotPoint_CoordsAreMinusOne_PivotPointIsNotDefined ()
        {
            // Define pivot point with id 10
            sprite.DefinePivotPoint ( 10, -1, -1 );

            // The pivot point 
            Assert.IsFalse ( sprite.IsPivotPointDefined ( 10 ) );
        }

        [TestCase ( 1000 )]
        [TestCase ( -1 )]
        public void DefinePivotPoint_IdGreaterThan999OrLowerThan0_ThrowsException ( int id )
        {
            Assert.That ( () => sprite.DefinePivotPoint ( id, 100, 100 ),
                Throws.InstanceOf<ArgumentOutOfRangeException> () );
        }

        [Test]
        public void GetPivotPoint_InexistingId_ThrowsException ()
        {
            Assert.That ( () => sprite.GetPivotPoint ( 100 ),
                Throws.InstanceOf<ArgumentOutOfRangeException> () );
        }

        [Test]
        public void GetPivotPoint_ExistingId_IsNotNull ()
        {
            sprite.DefinePivotPoint ( 10, 100, 100 );
            Assert.That ( () => sprite.GetPivotPoint ( 10 ), Is.Not.Null );
        }

        [Test]
        public void DeletePivotPoint_ExistingId_PivotPointIsRemoved ()
        {
            sprite.DefinePivotPoint ( 10, 100, 100 );

            sprite.DeletePivotPoint ( 10 );

            // The pivot point should no longer exists
            Assert.That ( () => sprite.GetPivotPoint ( 10 ),
                Throws.InstanceOf<ArgumentOutOfRangeException> () );
        }

        [Test]
        public void DeletePivotPoint_NonExistingId_DoesNotThrow ()
        {
            Assert.DoesNotThrow ( () => sprite.DeletePivotPoint ( 150 ) );
        }

        [Test]
        public void ClearPivotPoints_SpriteContainsPivotPoints_PivotPointsAreRemoved ()
        {
            sprite.DefinePivotPoint ( 10, 100, 100 );
            sprite.DefinePivotPoint ( 20, 100, 100 );
            sprite.ClearPivotPoints ();

            Assert.That ( sprite.PivotPoints.Count, Is.EqualTo ( 0 ) );
        }

        [Test]
        public void IsPivotPointDefined_PivotPointExists_ReturnssTrue ()
        {
            sprite.DefinePivotPoint ( 10, 100, 100 );

            Assert.That ( sprite.IsPivotPointDefined ( 10 ), Is.True );
        }

        [Test]
        public void IsPivotPointDefined_PivotPointDoesNotExist_ReturnssFalse ()
        {
            Assert.That ( sprite.IsPivotPointDefined ( 10 ), Is.False );
        }

        [TestCase ( 0, ExpectedResult = 3 )]
        [TestCase ( 1, ExpectedResult = 3 )]
        [TestCase ( 2, ExpectedResult = 3 )]
        [TestCase ( 3, ExpectedResult = 3 )]
        [TestCase ( 4, ExpectedResult = 4 )]
        [TestCase ( 5, ExpectedResult = 6 )]
        [TestCase ( 6, ExpectedResult = 6 )]
        [TestCase ( 7, ExpectedResult = 7 )]
        [TestCase ( 999, ExpectedResult = null )]
        public int? FindFreePivotPointId_FordwardsSearch_AsTestCaseExpects ( int start )
        {
            sprite.DefinePivotPoint ( 0, 100, 100 );
            sprite.DefinePivotPoint ( 1, 100, 100 );
            sprite.DefinePivotPoint ( 2, 100, 100 );
            // 3 Is free
            // 4 Is free
            sprite.DefinePivotPoint ( 5, 100, 100 );
            // ...
            sprite.DefinePivotPoint ( 999, 100, 100 );

            return sprite.FindFreePivotPointId ( start );
        }

        [TestCase ( 7, ExpectedResult = 7 )]
        [TestCase ( 6, ExpectedResult = 6 )]
        [TestCase ( 5, ExpectedResult = 4 )]
        [TestCase ( 4, ExpectedResult = 4 )]
        [TestCase ( 3, ExpectedResult = 3 )]
        [TestCase ( 2, ExpectedResult = null )]
        [TestCase ( 1, ExpectedResult = null )]
        [TestCase ( 0, ExpectedResult = null )]
        public int? FindFreePivotPointId_BackWardsSearch_AsTestCaseExpects ( int start )
        {
            sprite.DefinePivotPoint ( 0, 100, 100 );
            sprite.DefinePivotPoint ( 1, 100, 100 );
            sprite.DefinePivotPoint ( 2, 100, 100 );
            // 3 Is free
            // 4 Is free
            sprite.DefinePivotPoint ( 5, 100, 100 );

            return sprite.FindFreePivotPointId ( start, Sprite.SearchDirection.Backward );
        }

        [TestCase ( -1 )]
        [TestCase ( 1000 )]
        public void FindFreePivotPointId_InvalidPivotPointIds_ThrowsException ( int start )
        {
            Assert.That ( () => sprite.FindFreePivotPointId ( start ), Throws.InstanceOf<ArgumentOutOfRangeException> () );
        }
    }
}
