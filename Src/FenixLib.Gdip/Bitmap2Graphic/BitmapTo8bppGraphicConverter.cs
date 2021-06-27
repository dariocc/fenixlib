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
    internal class BitmapTo8bppGraphicConverter : Bitmap2GraphicConverter
    {
        public BitmapTo8bppGraphicConverter ( Bitmap src ) : base ( src ) { }

        protected override PixelFormat[] AcceptedFormats => new PixelFormat[] 
        {
            PixelFormat.Format1bppIndexed,
            PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed
        };

        protected override GraphicFormat DestFormat => GraphicFormat.Format8bppIndexed;
        protected override PixelFormat LockBitsFormat => PixelFormat.Format8bppIndexed;

        protected override IGraphic CreateGraphic ( byte[] pixelData )
        {
            var palette = CreatePalette ();
            return new Graphic (DestFormat, Src.Width, Src.Height, pixelData, palette );
        }
        protected virtual Palette CreatePalette ()
        {
            if ( Src.Palette == null )
            {
                throw new InvalidOperationException ( "Only bitmaps with palette can use " +
                    "this converter." );
            }

            if ( Src.Palette.Entries.Length > 256 )
            {
                throw new InvalidOperationException ( "Too many entries on Source " +
                    "bitmap palette." );
            }

            // TODO: Consider defining Alpha policies (A bitmap palette can have several
            // entries with alpha component and not just transparent/opaque of 1 index).
            // For the moment, the alpha component is ignored

            var colors = new PaletteColor[256];
            int n = -1;
            foreach ( var entry in Src.Palette.Entries )
            {
                n++;
                colors[n] = new PaletteColor ( entry.R, entry.G, entry.B );
            }

            return new Palette ( colors );
        }
    }
}