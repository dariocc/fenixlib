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
using System.IO;
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.Image
{
    public class DirectConverter : IFormatConverter
    {
        public byte[] Convert ( IGraphic graphic, GraphicFormat format )
        {
            if ( graphic == null )
                throw new ArgumentNullException ();

            if ( format == null )
                throw new ArgumentNullException ();

            if ( graphic.PixelData == null )
                throw new InvalidOperationException ( "Graphic has to have pixeldata" );

            if ( graphic.PixelData.Length != CalculatePixelBufferBytes (
                graphic.GraphicFormat.BitsPerPixel, graphic.Width, graphic.Height ) )
                throw new InvalidOperationException ();

            byte[] converted = new byte[CalculatePixelBufferBytes (
                format.BitsPerPixel, graphic.Width, graphic.Height )];

            using ( var source = new MemoryStream ( graphic.PixelData ) )
            {
                using ( var dest = new MemoryStream ( converted ) )
                {
                    var writer = new BinaryWriter ( dest );
                    
                }
            }

        }
    }
}
