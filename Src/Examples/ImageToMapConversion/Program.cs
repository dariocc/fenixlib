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
using System.IO;
using System.Drawing;
using FenixLib.Core;
using FenixLib.IO;
using FenixLib.BitmapConvert;

namespace ConvertImageToMap
{
    class Program
    {
        static void Main ( string[] args )
        {
            /* The FenixLib.Gdip assembly provides useful classes to perform
               System.Drawing.Bitmap > FenixLib.Core.IGraphic conversion operations.

               This permits bridgin FenixLib with the System.Drawing API.
            */

            // Raster images supported by the Gdip may be loaded directly into IGraphic
            // instances by using helper methods of the BitmapFile static class:
            var graphic = BitmapFile.LoadAsGraphic ( "pig.png" );

            // If the Bitmap is not contained in a file, but comes from an stream you
            // may want to take advantage of the BitmapFileGraphicDecoder, which implements
            // FenixLib.IO.IDecoder, just like any other decoder function of the 
            // FenixLib.IO namespace
            using ( var stream = new FileStream ( "pig.png", FileMode.Open ) )
            {
                var decoder = new BitmapFileGraphicDecoder ();
                var aGraphic = decoder.Decode ( stream );

                // do something with aGraphic
            }

            // An alternative is to use FenixLib extension methods of the Bitmap class
            // to perform conversion from System.Drawing.Bitmap to FenixLib.Core.IGraphic
            using ( var bitmap = new Bitmap ( "pig.png" ) )
            {
                var aGraphic = bitmap.ToGraphic ();

                // do something with aGraphic
            }

            // Once you have a FenixLib.IGraphic, you might want to create an sprite
            // from it. It is as easy as passing the graphic to the constructor of the
            // FenixLib.Core.Sprite class
            var sprite = new Sprite ( graphic );

            // Modify some Sprite-specific properties
            sprite.Description = "My converted sprite";
            sprite.DefinePivotPoint ( 0, 100, 100 );

            // And save it as a Map file using the helper functions of the NativeFile
            // class
            NativeFile.SaveToMap ( sprite, "pig.map" );

            // Which conveniently are defined as extension methods, so you can just write:
            sprite.SaveToMap ( "same-pig.map" );

            // Or, for added flexibility, you can directly use any of the defined
            // IDecoder<Sprite> classes of the FenixLib.IO namespace. You can then choose
            // to output to any stream.
            using ( var stream = new MemoryStream () )
            {
                var encoder = new MapSpriteEncoder ();
                encoder.Encode ( sprite, stream );
            }
        }
    }
}
