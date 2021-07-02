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
using FenixLib;

namespace FenixLibTests
{
    [TestFixture ( Category = "Unit" )]
    class PaletteColorTests
    {
        private PaletteColor color;
        private PaletteColor colorDuplicate;
        private PaletteColor differentColor;

        [SetUp]
        public void SetUp()
        {
            color = new PaletteColor ( 10, 50, 100 );
            colorDuplicate = new PaletteColor ( 10, 50, 100 );
            differentColor = new PaletteColor ( 10, 10, 100 );
        }

        [TestCase ( -100, 0, 0 )]
        [TestCase ( 0, -100, 0 )]
        [TestCase ( 0, 0, -100 )]
        [TestCase ( 256, 0, 0 )]
        [TestCase ( 0, 256, 0 )]
        [TestCase ( 0, 0, 256 )]
        public void Construct_ComponentOutsideRange_ThrowsException ( int r, int g, int b )
        {
            Assert.Throws<ArgumentOutOfRangeException> ( () => new PaletteColor ( r, g, b ) );
        }

        [Test]
        public void Equals_SameComponents_True ()
        {
            Assert.IsTrue ( color.Equals ( colorDuplicate ) );
        }

        [Test]
        public void Equals_DifferentComponents_False ()
        {
            Assert.IsFalse ( color.Equals ( differentColor ) );
        }

        [Test]
        public void EqualsToOperator_SameComponents_True ()
        {
            Assert.IsTrue ( color == colorDuplicate );
        }

        [Test]
        public void EqualsToOperator_DifferentComponents_False ()
        {
            Assert.IsFalse ( color == differentColor );
        }

        [Test]
        public void GetHashCode_TwoObjectsEqual_SameHashCode ()
        {
            Assert.AreEqual ( color.GetHashCode(), colorDuplicate.GetHashCode() );
        }

        [Test]
        public void DifferentThanOperator_SameComponents_False ()
        {
            Assert.IsFalse ( color != colorDuplicate );
        }

        [Test]
        public void DifferentThanOperator_DifferentComponents_True ()
        {
            Assert.IsTrue ( color != differentColor );
        }
    }
}
