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
    class GdipImageGraphicDecoder : IDecoder<IGraphic>
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

            Bitmap bmp;

            try
            {
                bmp = new Bitmap ( input );
            }
            catch ( Exception e )
            {
                throw new ArgumentException ( "Cannot decode the stream.", nameof ( input ), e );
            }

            BitmapConverter converter;

            if ( bmp.PixelFormat == PixelFormat.Format1bppIndexed )
            {
                converter = new MonochromeBitmapToMonochromeGraphicConverter ();
            }
            else if ( bmp.PixelFormat == PixelFormat.Format4bppIndexed ||
                bmp.PixelFormat == PixelFormat.Format8bppIndexed )
            {
                converter = new IndexedBitmapToIndexedGraphicConverter ();
            }
            else if ( bmp.PixelFormat == PixelFormat.Format16bppArgb1555  
                || bmp.PixelFormat == PixelFormat.Format16bppRgb555
                || bmp.PixelFormat == PixelFormat.Format16bppRgb555 )
            {
                converter = new Bitmap16bppTo16bppGraphicConverter ();
            }
            else
            {
                    
            }

            converter.BitmapSource = bmp;
            return converter.GetConvertedGraphic ();
        }

        public bool TryDecode ( Stream input, out IGraphic decoded )
        {
            throw new NotImplementedException ();
        }
    }
}
