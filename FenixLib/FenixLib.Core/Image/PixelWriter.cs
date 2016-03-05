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
    internal abstract class PixelWriter : IDisposable
    {
        private byte[] DestBuffer { get; set; }
        private Stream BaseStream { get; set; }
        protected BinaryWriter Writer { get; private set; }
        protected int Width { get; private set; }
        protected int Height { get; private set; }

        public abstract void Write ( int alpha, int r, int g, int b );

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose ( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Writer.Dispose ();
                    BaseStream.Dispose ();
                }

                disposedValue = true;
            }
        }

        public void Dispose ()
        {
            Dispose ( true );
        }
        #endregion

        public static PixelWriter Create ( GraphicFormat format, int width, int height,
            Palette destPalette = null )
        {
            if ( width <= 0 || height <= 0 )
                throw new ArgumentOutOfRangeException ( 
                    "Width and Height need to be greater than 0." );

            PixelWriter pixelWriter;

            if ( format == GraphicFormat.ArgbInt32 )
            {
                pixelWriter = new PixelWriterArgbInt32 ();
            }
            else if ( format == GraphicFormat.RgbInt16 )
            {
                pixelWriter = new PixelWriterRgbInt16 ();
            }
            else if ( format == GraphicFormat.RgbIndexedPalette )
            {
                if ( destPalette == null )
                {
                    throw new ArgumentNullException ( "Parameter 'destPalette' "
                        + "cannot be null for indexed palette graphic format." );
                }

                pixelWriter = new PixelWriterRgbIndexed ( destPalette );
            }
            else if ( format == GraphicFormat.Monochrome )
            {
                pixelWriter = new PixelWriterMonochrome ();
            }
            else
            {
                throw new ArgumentOutOfRangeException ( "Unsupported GraphicFormat." );
            }

            pixelWriter.Width = width;
            pixelWriter.Height = height;

            int destBufferLength = CalculatePixelBufferBytes ( format.BitsPerPixel,
                width, height );

            byte[] destBuffer = new byte[destBufferLength];

            pixelWriter.DestBuffer = destBuffer;
            pixelWriter.BaseStream = new MemoryStream ( destBuffer );
            pixelWriter.Writer = new BinaryWriter ( pixelWriter.BaseStream );

            return pixelWriter;
        }

        public byte[] GetPixels ()
        {
            BaseStream.Flush ();
            return DestBuffer;
        }

    }
}
