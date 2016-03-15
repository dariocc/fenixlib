using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenixLib.IO;
using FenixLib.Core;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace FenixLib.Gdip
{
    public class GdipImageGraphicDecoder : IDecoder<IGraphic>
    {
        public IEnumerable<string> SupportedExtensions
        {
            get
            {
                throw new NotImplementedException ();
            }
        }

        public IGraphic Decode ( Stream input )
        {
            if ( input == null )
            {
                throw new ArgumentNullException ( nameof ( input ) );
            }


            try
            {
                using ( Bitmap bmp = new Bitmap ( input ) )
                {
                    IBitmapConverter converter = GetBitmapConverter ( bmp.PixelFormat );
                    converter.SourceBitmap = bmp;

                    return converter.GetGraphic ();
                }
            }
            catch ( Exception e )
            {
                throw new ArgumentException ( "Cannot decode the stream.", nameof ( input ), e );
            }
        }

        private IBitmapConverter GetBitmapConverter ( PixelFormat format )
        {
            IBitmapConverter converter;

            if ( format == PixelFormat.Format1bppIndexed )
            {
                converter = new Bitmap1bppIndexedToGraphicMonochrome ();
            }
            else if ( format == PixelFormat.Format4bppIndexed ||
                format == PixelFormat.Format8bppIndexed )
            {
                converter = new BitmapIndexedToGraphicIndexed ();
            }
            else if ( format == PixelFormat.Format16bppArgb1555
                || format == PixelFormat.Format16bppRgb555
                || format == PixelFormat.Format16bppRgb555 )
            {
                converter = new Bitmap16bppToGraphic16bpp ();
            }
            else
            {
                converter = new Bitmap32bppToGraphic32bpp ();
            }

            return converter;
        }

        public bool TryDecode ( Stream input, out IGraphic decoded )
        {
            throw new NotImplementedException ();
        }
    }
}
