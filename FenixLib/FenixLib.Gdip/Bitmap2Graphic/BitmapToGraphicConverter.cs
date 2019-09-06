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
using System.Runtime.InteropServices;
using System.Linq;
using FenixLib.Core;

namespace FenixLib.BitmapConvert
{
    /// <summary>
    /// Common behvaiour of several Bitmap2Graphic converter subclasses.
    /// </summary>
    public abstract class Bitmap2GraphicConverter : IConverter
    {
        protected Bitmap2GraphicConverter ( Bitmap src )
        {
            if ( src == null )
            {
                throw new ArgumentNullException ( nameof ( src ) );
            }

            if ( !AcceptedFormats.Contains ( src.PixelFormat ) )
            {
                throw new ArgumentException ("PixelFormat of source Bitmap is not " +
                    "accepted by this converter.");
            }

            Src = src;
        }

        protected Bitmap Src { get; }
        protected abstract PixelFormat[] AcceptedFormats { get; }
        protected abstract PixelFormat LockBitsFormat { get; }
        protected abstract GraphicFormat DestFormat { get; }

        public IGraphic Convert ()
        {
            var rect = new Rectangle ( 0, 0, Src.Width, Src.Height );

            // Optional preparation step.
            Prepare ();

            BitmapData bitmapData;
            try
            {
                bitmapData = Src.LockBits ( rect, ImageLockMode.ReadOnly, LockBitsFormat );
            }
            catch ( Exception e )
            {
                throw new InvalidOperationException (
                    "Failed to perform lock bits operation.", e );
            }

            IGraphic graphic;
            try
            {
                var pixelData = CreatePixelDataBuffer ( bitmapData );
                graphic = CreateGraphic ( pixelData );
            }
            catch ( Exception e )
            {
                throw new InvalidOperationException ( "Conversion operation failed.", e );
            }
            finally
            {
                Src.UnlockBits ( bitmapData );
            }

            return graphic;
        }

        /// <summary>
        /// Optional preparatio step, before conversion LockBits function is called
        /// for BitmapData.
        /// </summary>
        protected virtual void Prepare () { }

        /// <summary>
        /// Creates the PixelData buffer suitable for the DestFormat.
        /// </summary>
        /// <param name="bitmapData">A <see cref="BitmapData"/> resulting from calling 
        /// <see cref="Bitmap.LockBits(Rectangle, ImageLockMode, PixelFormat)"/>.</param>
        /// <returns></returns>
        protected virtual byte[] CreatePixelDataBuffer ( BitmapData bitmapData )
        {
            var destBuffer = new byte[DestFormat.PixelsBytesForSize (
                Src.Width, Src.Height )];
            var destStride = DestFormat.StrideForWidth ( Src.Width );

            for ( int y = 0 ; y < Src.Height ; y++ )
            {
                Marshal.Copy ( IntPtr.Add ( bitmapData.Scan0, y * bitmapData.Stride ),
                    destBuffer,
                    y * destStride,
                    destStride );
            }

            return destBuffer;
        }

        /// <summary>
        /// Creates the <see cref="IGraphic"/> instance based on the PixelData buffer
        /// returned by <see cref="CreatePixelDataBuffer(BitmapData)"/>. The default 
        /// implementation returns a <see cref="Graphic"/> without palette.
        /// </summary>
        /// <param name="pixelData"></param>
        /// <returns></returns>
        protected virtual IGraphic CreateGraphic ( byte[] pixelData )
        {
            return new Graphic ( DestFormat, Src.Width, Src.Height, pixelData );
        }
    }
}