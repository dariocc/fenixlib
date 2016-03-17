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

namespace ConvertImageToMap
{
    class Program
    {
        static void Main ( string[] args )
        {
            // The FenixLib.Gdip assembly provides helper methods to load raster images
            var graphic = BitmapFile.Load ( "pig.png" );

            // You can also use Bitmap > Graphic conversion facilities provided by 
            // extension methods for the Bitmap class.
            using ( var bitmap = new Bitmap ( "pig.png" ) )
            {
                var anotherGraphic = bitmap.ToGraphic ();
            }


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
