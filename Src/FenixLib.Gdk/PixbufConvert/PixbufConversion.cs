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
using Gdk;
using FenixLib.Core;
using FenixLib.Core;

namespace FenixLib.Gdk
{
    public static class PixbufConversion
    {
        public static IGraphic ToGraphic ( this Pixbuf pixbuf )
        {
            if ( pixbuf == null )
            {
                throw new ArgumentNullException ( nameof ( pixbuf ) );
            }

            throw new NotImplementedException ();
        }

        public static Pixbuf ToPixBuf ( this IGraphic graphic )
        {
            byte[] destData = new byte[GraphicFormat
                .Format32bppArgb.PixelsBytesForSize ( graphic.Width, graphic.Height )];

            if ( graphic == null )
            {
                throw new ArgumentNullException ( nameof ( graphic ) );
            }


            using ( var reader = PixelReader.Create ( graphic ) )
            {
                int index = -1;
                do
                {
                    index++;
                    reader.Read ();
                    destData[index * 4] = ( byte ) reader.R;
                    destData[index * 4 + 1] = ( byte ) reader.G;
                    destData[index * 4 + 2] = ( byte ) reader.B;
                    destData[index * 4 + 3] = ( byte ) reader.Alpha;

                } while ( reader.HasPixels );
            }

            return new Pixbuf ( destData, 
                Colorspace.Rgb, 
                true, 
                8, 
                graphic.Width, 
                graphic.Height, 
                graphic.Width * 4 );
        }

    }
}
