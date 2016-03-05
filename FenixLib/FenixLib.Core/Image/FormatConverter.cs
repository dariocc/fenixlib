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
using FenixLib.Core;

namespace FenixLib.Image
{
    public class FormatConverter : IFormatConverter
    {
        public byte[] Convert ( IGraphic graphic, GraphicFormat format )
        {
            if ( graphic == null )
                throw new ArgumentNullException ( "Parameter 'graphic' cannot be null." );

            if ( format == null )
                throw new ArgumentNullException ( "Parameter 'format' cannot be null." );

            byte[] destBuffer;

            using ( var source = PixelReader.Create ( graphic ) )
            {
                using ( var dest = PixelWriter.Create ( format, graphic.Width, graphic.Height,
                    graphic.Palette ) )
                {
                    while ( source.HasPixels )
                    {
                        source.Read ();
                        dest.Write ( source.alpha, source.r, source.g, source.b );
                    }

                    destBuffer = dest.GetPixels ();
                }
            }
            return destBuffer;
        }
    }
}
