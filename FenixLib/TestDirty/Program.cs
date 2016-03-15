using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FenixLib.Gdip;
using FenixLib.Core;
using FenixLib.IO;
using System.IO;



namespace TestDirty
{
    class Program
    {
        static void Main ( string[] args )
        {
            var decoder = new GdipBitmapGraphicDecoder ();
            var graphic = decoder.Decode ( System.IO.File.Open ( "1bpp_8x1.bmp", FileMode.Open ) );

            Sprite s = new Sprite ( graphic );
            s.Description = "This is an example of a Map File";
            var encoder = new MapSpriteEncoder ();

            using ( var stream = new FileStream ( "saved.map", FileMode.Create ) )
            {
                encoder.Encode ( s, stream );
            }
        }
    }
}
