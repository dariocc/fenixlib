using NUnit.Framework;
using Rhino.Mocks;
using System;
using FenixLib.Core;
using FenixLib.Image;

namespace FenixLib.Core.Tests.Image
{
    [TestFixture ( typeof ( UpstreamConverter ) )]
    [TestFixture ( typeof ( DitherConverter ) )]
    class IFormatConverterContract<T> where T : IFormatConverter, new()
    {
        protected IFormatConverter CreateConverter ()
        {
            return new T ();
        }

        [Test]
        public void Convert_NullGraphicPassed_ThrowsNullArgumentException ()
        {
            IFormatConverter converter = CreateConverter ();

            Assert.Throws<ArgumentNullException>(() => 
                converter.Convert ( null, GraphicFormat.ArgbInt32 ));

        }

        [Test]
        public void Convert_NullFormatPassed_ThrowsNullArgumentException ()
        {
            IGraphic stubGraphic = MockRepository.GenerateStub<IGraphic> ();
            IFormatConverter converter = CreateConverter ();

            Assert.Throws<ArgumentNullException> ( () =>
                  converter.Convert ( stubGraphic, null ) );
        }
    }

    [TestFixture]
    class GraphicFormatConverterTests
    {

    }
}
