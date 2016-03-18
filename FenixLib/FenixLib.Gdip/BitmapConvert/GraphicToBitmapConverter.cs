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
using System.Runtime.InteropServices;

namespace FenixLib.BitmapConvert
{
    // TODO: Everything in here is very dirty...
    internal class GraphicToBitmapConverter
    {
        public IGraphic SourceGraphic { get; internal set; }

        public Bitmap GetBitmap ()
        {
            Bitmap bmp;
            var format = SourceGraphic.GraphicFormat;

            if ( format == GraphicFormat.Format1bppMonochrome )
            {
                bmp = GetBitmapForGraphic ( PixelFormat.Format1bppIndexed );
                bmp.Palette.Entries[0] = Color.FromArgb ( 0, 0, 0 );
                bmp.Palette.Entries[1] = Color.FromArgb ( 255, 255, 255 );
            }
            else if ( format == GraphicFormat.Format8bppIndexed )
            {
                bmp = GetBitmapForGraphic ( PixelFormat.Format8bppIndexed );
                for ( int i = 0 ; i < SourceGraphic.Palette.Colors.Length ; i++ )
                {
                    var c = SourceGraphic.Palette[i];
                    bmp.Palette.Entries[i] = Color.FromArgb ( c.R, c.G, c.B );
                }
            }
            else if ( format == GraphicFormat.Format16bppRgb565 )
            {
                bmp = GetBitmapForGraphic ( PixelFormat.Format16bppRgb565 );
            }
            else if ( format == GraphicFormat.Format32bppArgb )
            {
                bmp = GetBitmapForGraphic ( PixelFormat.Format32bppArgb );
            }
            else
            {
                throw new InvalidOperationException ();
            }

            return bmp;
        }

        private Bitmap GetBitmapForGraphic ( PixelFormat destPixelFormat )
        {
            int width = SourceGraphic.Width;
            int height = SourceGraphic.Height;

            var bitmap = new Bitmap ( width, height, destPixelFormat );
            var rect = new Rectangle ( 0, 0, width, height );

            var data = bitmap.LockBits ( rect, ImageLockMode.WriteOnly, destPixelFormat );
            try
            {
                int srcStrideSize = SourceGraphic.GraphicFormat.PixelsBytesForSize ( width, 1 );
                for ( int y = 0 ; y < height ; y++ )
                {
                    Marshal.Copy ( SourceGraphic.PixelData, 
                        y * srcStrideSize, 
                        data.Scan0 + y * data.Stride, 
                        srcStrideSize );
                }
            }
            catch ( Exception e )
            {
                bitmap.Dispose ();
                throw e;
            }
            finally
            {
                bitmap.UnlockBits ( data );
            }

            return bitmap;
        }
    }
}
