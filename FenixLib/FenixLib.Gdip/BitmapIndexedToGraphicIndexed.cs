using System;
using System.Drawing.Imaging;
using FenixLib.Core;
using System.Runtime.InteropServices;

namespace FenixLib.Gdip
{
    internal class BitmapIndexedToGraphicIndexed : Bitmap2GraphicConverter
    {
        protected override PixelFormat InputReadFormat
        {
            get
            {
                return PixelFormat.Format8bppIndexed;
            }
        }

        protected override IGraphic GetGraphicCore ( BitmapData data )
        {
            if ( SourceBitmap.Palette == null )
            {
                throw new InvalidOperationException ();
            }

            if ( SourceBitmap.Palette.Entries.Length > 256 )
            {
                throw new InvalidOperationException ();
            }

            var colors = new PaletteColor[256];
            int n = -1;
            foreach (var entry in SourceBitmap.Palette.Entries)
            {
                n++;
                // TODO: Alpha?
                colors[n] = new PaletteColor ( entry.R, entry.G, entry.B );
            }

            
            var pixelData = new byte[SourceBitmap.Height * SourceBitmap.Width];
            for (int i = 0 ; i < data.Height ; i++ )
            {
                Marshal.Copy ( IntPtr.Add(data.Scan0, i * data.Stride), pixelData, 
                    i * SourceBitmap.Width, SourceBitmap.Width );
            }


            var palette = new Palette ( colors );
            IGraphic graphic = new StaticGraphic ( GraphicFormat.RgbIndexedPalette, 
                SourceBitmap.Width, SourceBitmap.Height, pixelData, palette );

            return graphic;
        }
    }
}