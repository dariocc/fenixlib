using System;
using FenixLib.Core;
using System.Drawing.Imaging;

namespace FenixLib.Gdip
{
    internal class Bitmap16bppToGraphic16bpp : Bitmap2GraphicConverter
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