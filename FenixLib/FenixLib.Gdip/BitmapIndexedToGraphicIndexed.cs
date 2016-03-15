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
    internal class BitmapIndexedToGraphicIndexed : BitmapToGraphicConverter
    {
        protected override PixelFormat InputReadFormat => PixelFormat.Format8bppIndexed;

        protected override IGraphic GetGraphicCore ( BitmapData data )
        {
            if ( SourceBitmap.Palette.Entries.Length > 256 )
            {
                throw new InvalidOperationException ();
            }

            var colors = new PaletteColor[256];
            int n = -1;
            foreach (var entry in SourceBitmap.Palette.Entries)
            {
                n++;

                // TODO: This will not have consideration for alpha in the palette entries
                colors[n] = new PaletteColor ( entry.R, entry.G, entry.B );
            }

            
            var pixelData = new byte[SourceBitmap.Height * SourceBitmap.Width];
            for (int y = 0 ; y < data.Height ; y++ )
            {
                Marshal.Copy ( IntPtr.Add(data.Scan0, y * data.Stride), pixelData, 
                    y * SourceBitmap.Width, SourceBitmap.Width );
            }


            var palette = new Palette ( colors );
            IGraphic graphic = new StaticGraphic ( GraphicFormat.RgbIndexedPalette, 
                SourceBitmap.Width, SourceBitmap.Height, pixelData, palette );

            return graphic;
        }
    }
}