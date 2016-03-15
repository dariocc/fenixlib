﻿/*  Copyright 2016 Darío Cutillas Carrillo
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
using FenixLib.Core;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FenixLib.IO
{
    internal class Bitmap16bppToGraphic16bpp : BitmapToGraphicConverter
    {
        protected override PixelFormat InputReadFormat => PixelFormat.Format16bppRgb565;

        protected override IGraphic GetGraphicCore ( BitmapData data )
        {
            byte[] pixelData = new byte[GraphicFormat.RgbInt16.PixelsBytesForSize (
                data.Width, data.Height )];

            for ( int y = 0 ; y < data.Height ; y++ )
            {
                Marshal.Copy ( IntPtr.Add ( data.Scan0, y * data.Stride ), pixelData,
                    y * data.Width, data.Width );
            }

            return new StaticGraphic ( GraphicFormat.RgbInt16, data.Width, data.Height, 
                pixelData );
        }
    }
}