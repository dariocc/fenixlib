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
using System.Drawing;
using System.Drawing.Imaging;
using FenixLib.Core;

namespace FenixLib.IO
{
    internal abstract class BitmapToGraphicConverter : IBitmapConverter
    {

        protected abstract PixelFormat InputReadFormat { get; }

        protected abstract IGraphic GetGraphicCore ( BitmapData data );

        public Bitmap SourceBitmap { get; set; }

        public IGraphic GetGraphic ()
        {
            if ( SourceBitmap == null )
            {
                throw new InvalidOperationException ();
            }

            var rect = new Rectangle ( 0, 0, SourceBitmap.Width, SourceBitmap.Height );

            BitmapData data;
            try
            {
                data = SourceBitmap.LockBits ( rect, ImageLockMode.ReadOnly, InputReadFormat );
            }
            catch ( Exception e )
            {
                // TODO: Write a better error message description
                throw new InvalidOperationException ( "LockBits failed.", e );
            }

            try
            {
                return GetGraphicCore ( data );
            }
            finally
            {
                SourceBitmap.UnlockBits ( data );
            }

        }
    }
}