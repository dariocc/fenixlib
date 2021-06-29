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

namespace FenixLib.Imaging
{
    public static class FormatConverter
    {
        public static IGraphic convert ( this IGraphic sourceGraphic, GraphicFormat destFormat, Palette destPalette = null)
        {
            if ( sourceGraphic == null )
                throw new ArgumentNullException ( "Parameter 'graphic' cannot be null." );

            if ( destFormat == null )
                throw new ArgumentNullException ( "Parameter 'format' cannot be null." );

            if ( destFormat == GraphicFormat.Format8bppIndexed && destPalette == null )
                throw new ArgumentException( $"{destFormat} requires a palette to be specified." );

            if ( destFormat != GraphicFormat.Format8bppIndexed && destPalette != null )
                throw new ArgumentException( $"{destFormat} doesn't support palettes." );

            byte[] destBuffer;

            using ( var source = PixelReader.Create ( sourceGraphic ) )
            {
                using ( var dest = PixelWriter.Create ( destFormat, sourceGraphic.Width, sourceGraphic.Height,
                    destPalette ))
                {
                    while ( source.HasPixels )
                    {
                        source.Read ();
                        dest.Write ( source.Alpha, source.R, source.G, source.B );
                    }

                    destBuffer = dest.GetPixels ();
                }
            }

            return new Graphic(destFormat, sourceGraphic.Width, sourceGraphic.Height, destBuffer, destPalette );
        }
    }
}
