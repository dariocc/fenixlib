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
using System.Drawing;
using System.Drawing.Imaging;
using FenixLib.Core;

namespace FenixLib.BitmapConvert
{
    public class BitmapToGraphicConverterCreator : IConverterFactory
    {
        public IConverter Create ( Bitmap src )
        {
            if ( src == null )
            {
                throw new ArgumentNullException ( nameof ( src ) );
            }

            IConverter converter;
            var format = src.PixelFormat;

            switch ( format )
            {
                case PixelFormat.Format1bppIndexed:
                    converter = new BitmapTo1bppGraphicConverter ( src );
                    break;

                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    converter = new BitmapTo8bppGraphicConverter ( src );
                    break;

                case PixelFormat.Format16bppArgb1555:
                // TODO: This format should have special treatment
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    converter = new BitmapTo16bppGraphicConverter ( src );
                    break;

                default: // Any other format shall be read as 32bpp format
                    converter = new BitmapTo32bppGraphicConverter ( src );
                    break;
            }

            return converter;
        }

        public IConverter Create ( Bitmap src, GraphicFormat destFormat )
        {
            IConverter converter;

            if (destFormat == GraphicFormat.Format1bppMonochrome)
            {
                converter = new BitmapTo1bppGraphicConverter ( src );
            }
            else if ( destFormat == GraphicFormat.Format8bppIndexed )
            {
                converter = new BitmapTo8bppGraphicConverter ( src );
            }
            else if ( destFormat == GraphicFormat.Format16bppRgb565 )
            {
                converter = new BitmapTo16bppGraphicConverter ( src );
            }
            else if ( destFormat == GraphicFormat.Format32bppArgb )
            {
                converter = new BitmapTo32bppGraphicConverter ( src );
            }
            else
            {
                throw new ArgumentOutOfRangeException ( nameof ( destFormat ) );
            }

            return converter;
        }
    }
}
