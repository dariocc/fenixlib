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

namespace FenixLib.Image
{
    internal abstract class PixelReader : IDisposable
    {
        protected Stream BaseStream { get; private set; }
        protected BinaryReader Reader { get; private set; }
        protected IGraphic Graphic { get; private set; }

        public int r { get; protected set; }
        public int g { get; protected set; }
        public int b { get; protected set; }
        public int alpha { get; protected set; }

        public abstract bool HasPixels { get; }

        public abstract void Read ();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose ( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Reader.Dispose ();
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

        public static PixelReader Create ( IGraphic graphic )
        {
            PixelReader pixelReader;

            if ( graphic.GraphicFormat == GraphicFormat.Format32bppArgb )
                pixelReader = new PixelReaderArgbInt32 ();
            else if ( graphic.GraphicFormat == GraphicFormat.Format8bppIndexed )
                pixelReader = new PixelReaderRgbInt16 ();
            else if ( graphic.GraphicFormat == GraphicFormat.Format8bppIndexed )
                pixelReader = new PixelReaderRgbIndexed ();
            else if ( graphic.GraphicFormat == GraphicFormat.Format1bppMonochrome )
                pixelReader = new PixelReaderMonochrome ();
            else
                throw new ArgumentOutOfRangeException ( "Unsupported GraphicFormat" );

            pixelReader.BaseStream = new MemoryStream ( graphic.PixelData );
            pixelReader.Reader = new BinaryReader ( pixelReader.BaseStream );
            pixelReader.Graphic = graphic;

            return pixelReader;
        }
    }
}
