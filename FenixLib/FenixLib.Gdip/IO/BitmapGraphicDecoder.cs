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
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using FenixLib.Core;
using FenixLib.BitmapConvert;

namespace FenixLib.IO
{
    /// <summary>
    /// A <see cref="IGraphic"/> decoder that can read GDI+ supported image formats.
    /// </summary>
    public class BitmapGraphicDecoder : IDecoder<IGraphic>
    {
        public IEnumerable<string> SupportedExtensions
        {
            get
            {
                ImageCodecInfo[] myCodecs;
                myCodecs = ImageCodecInfo.GetImageEncoders ();

                return myCodecs.SelectMany ( c => c.FilenameExtension.Split ( ';' ) );
            }
        }

        public IGraphic Decode ( Stream input )
        {
            return Decode ( input, new BitmapToGraphicConverterCreator () );
        }

        internal IGraphic Decode ( Stream input,
            IBitmapToGraphicConverterCreator converterCreator )
        {
            if ( input == null )
            {
                throw new ArgumentNullException ( nameof ( input ) );
            }
            else if ( converterCreator == null )
            {
                throw new ArgumentNullException ( nameof ( converterCreator ) );
            }

            try
            {
                using ( Bitmap bmp = new Bitmap ( input ) )
                {
                    var converter = converterCreator.Create ( bmp );

                    return converter.Convert ();
                }
            }
            catch ( Exception e )
            {
                throw new ArgumentException ( "Failed to decode the stream.",
                    nameof ( input ), e );
            }
        }

        public bool TryDecode ( Stream input, out IGraphic decoded )
        {
            try
            {
                decoded = Decode ( input );
                return true;
            }
            catch
            {
                decoded = null;
                return false;
            }
        }
    }
}
