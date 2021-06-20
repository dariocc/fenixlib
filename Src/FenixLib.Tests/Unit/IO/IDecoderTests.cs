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
using FenixLib.IO;
using System;
using FenixLib.Core;

namespace FenixLib.Tests.Unit.Core.IO
{
    [TestFixture ( TypeArgs = new Type[] { typeof ( FpgSpriteAssortmentDecoder ), typeof ( ISpriteAssortment ) }, Category = "Unit" ) ]
    [TestFixture ( TypeArgs = new Type[] { typeof ( DivFntBitmapFontDecoder ), typeof ( IBitmapFont ) }, Category = "Unit" ) ]
    [TestFixture ( TypeArgs = new Type[] { typeof ( ExtendedFntBitmapFontDecoder ), typeof ( IBitmapFont ) }, Category = "Unit" )]
    [TestFixture ( TypeArgs = new Type[] { typeof ( DivFntSpriteAssortmentDecoder ), typeof ( ISpriteAssortment ) }, Category = "Unit" )]
    [TestFixture ( TypeArgs = new Type[] { typeof ( MapSpriteDecoder ), typeof ( ISprite ) } , Category = "Unit" )]
    [TestFixture ( TypeArgs = new Type[] { typeof ( DivFilePaletteDecoder ), typeof ( Palette ) }, Category = "Unit")]
    public class IDecoderTests<D, E> where D : IDecoder<E>, new()
    {
        protected D CreateInstance ()
        {
            return new D ();
        }

        protected D decoder;

        [SetUp]
        public void SetUp ()
        {
            decoder = CreateInstance ();
        }

        [Test]
        public void Decode_NullStream_ThrowsException ()
        {
            Assert.That ( () => decoder.Decode ( null ), Throws.ArgumentNullException );
        }

        [Test]
        public void TryDecode_NullStream_ReturnsFalse ()
        {
            bool isDecoded;
            using ( var stream = new System.IO.MemoryStream () )
            {
                isDecoded = decoder.TryDecode ( stream, out E decoded );
            }
            Assert.That ( isDecoded, Is.False );
        }
    }
}
