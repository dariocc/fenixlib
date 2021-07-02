/**
 *    Copyright (c) 2016 Darío Cutillas Carrillo
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 * 
 *        http://www.apache.org/licenses/LICENSE-2.0
 * 
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
namespace FenixLib.Imaging
{
    internal class PixelReaderMonochrome : PixelReader
    {
        private int pixelIndex;
        private byte byteValue;

        public override bool HasPixels
        {
            get
            {
                return pixelIndex < Graphic.Width * Graphic.Height;
            }
        }

        public override void Read ()
        {
            var bytesPerStride = FenixLib.GraphicFormat.Format1bppMonochrome.StrideForWidth ( Graphic.Width );
            var usedBitsInStride = Graphic.Width;
            var totalBitsInStride = bytesPerStride * 8;

            var alignedBitIndex = pixelIndex + ( pixelIndex / usedBitsInStride ) * (totalBitsInStride - usedBitsInStride);
            var bitInByte = 7 - ( alignedBitIndex % totalBitsInStride ) % 8;

            if (alignedBitIndex % totalBitsInStride == 0 || alignedBitIndex % 8 == 0)
            {
                byte[] buff = new byte[1];
                BaseStream.Read ( buff, 0, 1 );
                byteValue = buff[0];
            }

            int bitValue = ( byteValue & ( 0x01 << bitInByte ) ) >> bitInByte;
            int componentValue = bitValue * 255;

            R = componentValue;
            G = componentValue;
            B = componentValue;
            Alpha = componentValue;

            pixelIndex++;
        }
    }
}
