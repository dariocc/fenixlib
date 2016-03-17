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
using System;
using System.Collections.Generic;
using System.Linq;
using FenixLib.Core;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace FenixLib.IO
{
    /// <summary>
    /// A <see cref="IGraphic"/> decoder that can read GDI+ supported file formats such as 
    /// PNG, BMP, TIFF, an others. The number of formats depend on the target platform.
    /// </summary>
    public class BitmapGraphicDecoder : IDecoder<IGraphic>
    {
        public IEnumerable<string> SupportedExtensions
        {
            get
            {
                ImageCodecInfo[] myCodecs;
                myCodecs = ImageCodecInfo.GetImageEncoders ();

                return myCodecs.SelectMany(c => c.FilenameExtension.Split ( ';' ));
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
                    IBitmap2GraphicConverter converter = GetDefaultConverterForFormat ( bmp.PixelFormat );
                    converter.SourceBitmap = bmp;

                    return converter.Convert ();
                }
            }
            catch ( Exception e )
            {
                throw new ArgumentException ( "Failed to decode the stream.",
                    nameof ( input ), e );
            }
        }

        public bool TryDecode ( Stream input, out IGraphic decoded )
        {
            try
            {
                decoded = Decode ( input );
                return true;
            }
            catch
            {
                decoded = null;
                return false;
            }
        }

        // Acts as a static factory for getting the best Converter for the specified format
        private IBitmap2GraphicConverter GetDefaultConverterForFormat ( PixelFormat format )
        {
            IBitmap2GraphicConverter converter;

            switch ( format )
            {
                case PixelFormat.Format1bppIndexed:
                    converter = new Bitmap1bppIndexedToGraphicMonochrome ();
                    break;

                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    converter = new BitmapIndexedToGraphicIndexed ();
                    break;

                case PixelFormat.Format16bppArgb1555:
                    // TODO: This format should have special treatment
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    converter = new Bitmap16bppToGraphic16bpp ();
                    break;

                default: // Any other format shall be read as 32bpp format
                    converter = new Bitmap32bppToGraphic32bpp ();
                    break;
            }

            return converter;
        }
    }
}
