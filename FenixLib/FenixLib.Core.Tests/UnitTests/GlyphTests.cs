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

namespace FenixLib.Core.Tests.UnitTests
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
            IGraphic fakeGraphic = MockRepository.GenerateStub<IGraphic> ();
            fakeGraphic.Stub ( x => x.Width ).Return ( 1 );
            fakeGraphic.Stub ( x => x.Height ).Return ( 1 );
            fakeGraphic.Stub ( x => x.GraphicFormat ).Return ( GraphicFormat.ArgbInt32 );
            fakeGraphic.Stub ( x => x.PixelData ).Return ( new byte[1] );

            return fakeGraphic;
        }
    }
}
