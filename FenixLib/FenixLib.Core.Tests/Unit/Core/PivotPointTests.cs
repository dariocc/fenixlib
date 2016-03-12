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
using System;
using FenixLib.Core;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture ( Category = "Unit" )]
    class PivotPointTests
    {
        private PivotPoint pivotPoint;
        private PivotPoint pivotPointSameId;
        private PivotPoint differentPivotPoint;

        [SetUp]
        public void SetUp ()
        {
            pivotPoint = new PivotPoint ( 1, 10, 10 );
            pivotPointSameId = new PivotPoint ( 1, 100, 100 );
            differentPivotPoint = new PivotPoint ( 2, 10, 100 );
        }

        [Test]
        public void Equals_SameId_True ()
        {
            Assert.AreEqual ( pivotPoint, pivotPointSameId );
        }

        [Test]
        public void Equals_DifferentId_False ()
        {
            Assert.AreNotEqual ( pivotPoint, differentPivotPoint );
        }

        [Test]
        public void GetHashCode_SameId_SameHashCode ()
        {
            Assert.AreEqual ( pivotPointSameId.GetHashCode (), 
                pivotPointSameId.GetHashCode () );
        }

        [Test]
        public void EqualsToOperator_SameId_True()
        {
            Assert.IsTrue ( pivotPoint == pivotPointSameId );
        }

        [Test]
        public void EqualsToOperator_DifferentId_False()
        {
            Assert.IsFalse ( pivotPoint == differentPivotPoint );
        }

        [Test]
        public void DifferentThanOperator_SameId_False()
        {
            Assert.IsFalse ( pivotPoint != pivotPointSameId );
        }

        [Test]
        public void DifferentThanOperator_DifferentId_True()
        {
            Assert.IsTrue ( pivotPoint != differentPivotPoint );
        }
    }
}
