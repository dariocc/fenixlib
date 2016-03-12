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
namespace FenixLib.Image
{
    public abstract class SampleModel<T> where T : struct
    {
        public int Width { get; }
        public int Height { get; }

        protected SampleModel ( int width, int height )
        {
            Width = width;
            Height = height;
        }

        protected abstract T GetPixel ( int x, int y, DataBuffer<T> pixelsData );
    }

    public class SinglePixelPackedSampleModel<T> : SampleModel<T> where T : struct
    {
        public int[] BitMasks { get; }
        public int ScanLineStride { get; }

        public SinglePixelPackedSampleModel ( int width, int height, int scanLineStride, int[] bitMasks )
            : base ( width, height )
        {
            BitMasks = bitMasks;
            ScanLineStride = scanLineStride;
        }

        public int GetSample ( int x, int y, int band, DataBuffer<T> data )
        {
            int mask = BitMasks[band];
            return Convert.ToInt32 ( GetPixel ( x, y, data ) ) & mask;
        }

        protected override T GetPixel ( int x, int y, DataBuffer<T> data )
        {
            T pixel = data[y * ScanLineStride + x];
            return pixel;
        }
    }

    public class Int16Rgb565SampleModel : SinglePixelPackedSampleModel<short>
    {
        public Int16Rgb565SampleModel ( int width, int height ) :
            base ( width, height, width, new int[] { 0xF800, 0x7E0, 0x1F } )
        {

        }
    }

    public class Int32ArgbSampleModel : SinglePixelPackedSampleModel<int>
    {
        public Int32ArgbSampleModel ( int width, int height ) :
            base ( width, height, width, new int[] { 0xFF0000 << 8, 0xFF0000, 0xFF00, 0xFF } )
        {

        }
    }

    public class Int8IndexedSampleModel : SinglePixelPackedSampleModel<byte>
    {
        public Int8IndexedSampleModel ( int width, int height ) :
            base ( width, height, width, new int[] { 0xFF } )
        {

        }
    }

    public class MultiPixelPackedSampleModel<T> : SampleModel<T> where T : struct
    {
        protected int RowBytes { get; }
        protected int BlockBits { get; }

        protected MultiPixelPackedSampleModel ( int width, int height, int blockSize ) : base ( width, height )
        {
            BlockBits = blockSize * 8;
            RowBytes = GetColumn ( width );
        }

        protected override T GetPixel ( int x, int y, DataBuffer<T> pixelsData )
        {
            if ( x > Width || y > Height )
                throw new ArgumentOutOfRangeException ();

            int index = RowBytes * y + GetColumn ( x );

            return pixelsData[index];
        }

        private int GetColumn ( int x )
        {
            return ( x + ( ( BlockBits - ( x % BlockBits ) ) & ( BlockBits - 1 ) ) ) / BlockBits;
        }
    }
}
