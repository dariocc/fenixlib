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
namespace FenixLib.Imaging
{
    internal class PixelReaderMonochrome : PixelReader
    {
        private int x = 0, y = 0;
        byte byteValue;

        public override bool HasPixels
        {
            get
            {
                return ( ( x + 1 ) * ( y + 1 ) <= ( Graphic.Width - 1 ) * ( Graphic.Height - 1 ) );
            }
        }

        public override void Read ()
        {
            int bytesPerRow =
                Core.GraphicFormat.Format1bppMonochrome.StrideForWidth ( Graphic.Width );

            if ( x % 8 == 0 )
            {
                byte[] buff = new byte[1];
                BaseStream.Read ( buff, 0, 1 );
                byteValue = buff[0];
            }

            int bitInByte = ( x % 8 );

            int bitValue = ( byteValue & ( 0x01 << ( 7 - bitInByte ) ) ) >> ( 7 - bitInByte );
            int componentValue = bitValue * 255;

            if ( Core.GraphicFormat.Format1bppMonochrome.StrideForWidth ( x + 1 ) >= bytesPerRow )
            {
                x = 0;
                y++;
            }
            else
            {
                x++;
            }

            R = componentValue;
            G = componentValue;
            B = componentValue;
            Alpha = componentValue;
        }
    }
}
