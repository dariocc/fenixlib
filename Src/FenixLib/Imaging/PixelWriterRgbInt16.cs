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
    internal class PixelWriterRgbInt16 : PixelWriter
    {
        public override void Write ( int alpha, int r, int g, int b )
        {
            ushort value;

            if ( alpha < 128 )
            {
                value = 0;
            }
            else
            {
                value = ( ushort ) ( ( ( r >> 3 ) << 11 ) | ( ( g >> 2 ) << 5 ) | ( b >> 3 ) );
            }

            Writer.Write ( value );
        }
    }
}
