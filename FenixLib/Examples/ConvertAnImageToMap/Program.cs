using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FenixLib.Core;
using FenixLib.IO;

namespace ConvertImageToMap
{
    class Program
    {
        static void Main ( string[] args )
        {
            // Common image raster formats can be loaded via the functionality exposed
            // by FenixLib.Gdip assembly (which uses System.Drawing)
            var graphic = BitmapFile.Load ( "pig.png" );

            // Create an sprite from that graphic
            var sprite = new Sprite ( graphic );
            sprite.Description = "My converted sprite";

            // The sprite can now be encoded to a Map file. The easiest is to use the
            // helper class NativeFile:
            NativeFile.SaveMap ( sprite, "pig.map" );
        }
    }
}
