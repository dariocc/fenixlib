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
using System.Drawing.Imaging;
using FenixLib.Core;
using System.Runtime.InteropServices;

namespace FenixLib.IO
{
    /// <summary>
    /// Creates Monochrome Graphics from a 1bppIndexed GDIP Bitmap
    /// </summary>
    internal class Bitmap1bppIndexedToGraphicMonochrome : BitmapToGraphicConverter
    {
        protected override PixelFormat InputReadFormat => PixelFormat.Format1bppIndexed;

        protected override IGraphic GetGraphicCore ( BitmapData data )
        {
            int bytesPerRow = GraphicFormat.Format1bppMonochrome.PixelsBytesForSize ( data.Width, 1 );
            byte[] pixelData = new byte[GraphicFormat.Format1bppMonochrome.PixelsBytesForSize (
                    data.Width, data.Height )];

            // Monochrome bitmaps are packed as the FenixLib 1bpp format, but they are differently
            // aligned

            for ( int y = 0 ; y < data.Height ; y++ )
            {
                Marshal.Copy ( IntPtr.Add ( data.Scan0, y * data.Stride ), pixelData,
                    y * bytesPerRow, bytesPerRow );
            }

            return new Graphic ( GraphicFormat.Format1bppMonochrome, data.Width, data.Height,
                pixelData );
        }
    }
}