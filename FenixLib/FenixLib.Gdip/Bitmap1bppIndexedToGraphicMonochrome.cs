using System;
using System.Drawing.Imaging;
using FenixLib.Core;
namespace FenixLib.Gdip
{
    internal class Bitmap1bppIndexedToGraphicMonochrome : Bitmap2GraphicConverter
    {
        protected override PixelFormat InputReadFormat
        {
            get
            {
                throw new NotImplementedException ();
            }
        }

        protected override IGraphic GetGraphicCore ( BitmapData data )
        {
            throw new NotImplementedException ();
        }
    }
}