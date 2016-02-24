using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenixLib.IO;

namespace FenixLib.Core.Tests.IO
{
    [TestFixture]
    public class FpgSpriteAssetDecoderTests
    {
        [Test]
        public void Decode_NullStream_IOException ()
        {
            FpgSpriteAssetDecoder decoder = new FpgSpriteAssetDecoder ();
            decoder.Decode ( null );
        }
    }
}
