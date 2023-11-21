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
using Moq;
using FenixLib;

namespace FenixLibTests
{
    [TestFixture ( Category = "Unit" )]
    class GlyphTests
    {
        [Test]
        public void Constructor_NullGraphic_ThrowsException ()
        {
            var glyph = new Glyph ( CreateFakeGraphic () );
        }

        private IGraphic CreateFakeGraphic ()
        {
            Mock<IGraphic> fakeGraphic = new Mock<IGraphic>();
            fakeGraphic.Setup ( x => x.Width ).Returns ( 1 );
            fakeGraphic.Setup ( x => x.Height ).Returns ( 2 );
            fakeGraphic.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format32bppArgb );
            fakeGraphic.Setup ( x => x.PixelData ).Returns ( new byte[1] );

            return fakeGraphic.Object;
        }
    }
}
