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

namespace FenixLib.Core.Tests.UnitTests
{
    abstract class IGraphicTests<T> where T : IGraphic
    {
        private IGraphic graphic;

        protected abstract IGraphic CreateSampleInstance ();

        [SetUp]
        public void SetUp()
        {
            graphic = CreateSampleInstance ();
        }

        [Test]
        public void PixelData_NotNull ()
        {
            Assert.IsNotNull ( graphic.PixelData );
        }

        [Test]
        public void Width_GreaterThan0 ()
        {
            Assert.IsNotNull ( graphic.Width > 0 );
        }

        [Test]
        public void Height_GreaterThan0 ()
        {
            Assert.IsNotNull ( graphic.Height > 0 );
        }

        public void GraphicFormat_NotNull ()
        {
            Assert.IsNotNull ( graphic.GraphicFormat );
        }
    }
}
