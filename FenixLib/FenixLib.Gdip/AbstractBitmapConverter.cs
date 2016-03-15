using System;
using System.Drawing;
using System.Drawing.Imaging;
using FenixLib.Core;

namespace FenixLib.Gdip
{
    internal abstract class Bitmap2GraphicConverter : IBitmapConverter
    {

        protected abstract PixelFormat InputReadFormat { get; }

        protected abstract IGraphic GetGraphicCore ( BitmapData data );

        public Bitmap SourceBitmap { get; set; }

        public IGraphic GetGraphic ()
        {
            if ( SourceBitmap == null )
            {
                throw new InvalidOperationException ();
            }

            var rect = new Rectangle ( 0, 0, SourceBitmap.Width, SourceBitmap.Height );
            var data = SourceBitmap.LockBits ( rect, ImageLockMode.ReadOnly, InputReadFormat );

            try
            {
                return GetGraphicCore ( data );
            }
            finally
            {
                SourceBitmap.UnlockBits ( data );
            }

        }
    }
}