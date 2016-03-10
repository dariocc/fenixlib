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

namespace FenixLib.Core.Tests.UnitTests.IO
{
    [TestFixture]
    public abstract class GenericDecoderTests<D, E> where D : IDecoder<E>, new()
    {
        protected abstract D CreateInstance ();

        protected D decoder;

        [SetUp]
        public void SetUp()
        {
            decoder = CreateInstance ();
        }

        [Test]
        public void Decode_NullStream_ThrowsException ()
        {
            Assert.That ( () => decoder.Decode ( null ), Throws.ArgumentNullException );
        }

        [Test]
        public void TryDecode_NullStream_ReturnsFalse()
        {
            E decoded;
            Assert.That ( decoder.TryDecode ( null, out decoded ), Is.False );
        }
    }
}
