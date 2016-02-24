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

namespace FenixLib.Core
{
    public sealed class DepthMode
    {
        public int Value { get; }

        private DepthMode ( int value )
        {
            Value = value;
        }

        public static explicit operator DepthMode ( int value )
        {
            switch ( value )
            {
                case 1:
                    return Monochrome;
                case 8:
                    return RgbIndexedPalette;
                case 16:
                    return RgbInt16;
                case 32:
                    return ArgbInt32;
            }

            throw new ArgumentException ();
        }

        public static explicit operator int (DepthMode depthMode)
        {
            return depthMode.Value;
        }

        public static DepthMode Monochrome = new DepthMode ( 1 );
        public static DepthMode RgbIndexedPalette = new DepthMode ( 8 );
        public static DepthMode RgbInt16 = new DepthMode ( 16 );
        public static DepthMode ArgbInt32 = new DepthMode ( 32 );
    }
}
