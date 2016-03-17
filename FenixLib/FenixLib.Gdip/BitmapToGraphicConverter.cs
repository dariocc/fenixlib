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
    /// <summary>
    /// Common behvaiour of several Bitmap2Graphic converter subclasses.
    /// </summary>
    public abstract class Bitmap2GraphicConverter : IBitmap2GraphicConverter
    {
        protected abstract PixelFormat[] AcceptedFormats { get; }
        protected abstract PixelFormat ReadAsFormat { get; }

        protected abstract IGraphic GetGraphicCore ( Bitmap src, BitmapData lockedData );

        public IGraphic Convert (Bitmap src)
        {
            if ( src == null )
            {
                throw new ArgumentNullException (nameof( src ) );
            }

            var rect = new Rectangle ( 0, 0, src.Width, src.Height );

            BitmapData data;
            try
            {
                data = src.LockBits ( rect, ImageLockMode.ReadOnly, ReadAsFormat );
            }
            catch ( Exception e )
            {
                // TODO: Write a better error message description
                throw new InvalidOperationException ( "LockBits failed.", e );
            }

            try
            {
                return GetGraphicCore ( src, data );
            }
            finally
            {
                src.UnlockBits ( data );
            }
        }
    }
}