using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
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

            // The sprite can now be encoded to a Map file. Convience static methods
            // are defined in the NativeFile static class:
            NativeFile.SaveAsMap ( sprite, "pig.map" );

            // Which can also be used as an extension method for ISprite
            sprite.SaveAsMap ( "same-pig.map" );

            // NOTE: Check the bin folder to find the generated files

            // Alternatively, you can gain flexibility by using directly one of the 
            // encoders defined in the FenixLib.IO namespace.
            using ( var stream = new MemoryStream () )
            {
                var encoder = new MapSpriteEncoder ();
                encoder.Encode ( sprite, stream );
            }
        }
    }
}
