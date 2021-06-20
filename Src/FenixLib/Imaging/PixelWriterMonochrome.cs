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

namespace FenixLib.Imaging
{
    internal class PixelWriterMonochrome : PixelWriter
    {
        public override void Write ( int alpha, int r, int g, int b )
        {
            throw new NotImplementedException ();

            byte value;

            if ( alpha < 128 )
            {
                value = 0;
            }
            else
            {
                value = FindNearest ( r, g, b );
            }

            Writer.Write ( value );
        }

        private byte FindNearest ( int r, int g, int b )
        {
            throw new NotImplementedException ();

            double rn = r / 765.0;
            double gn = g / 765.0;
            double bn = b / 765.0;

            double avg = rn + gn + bn;

            if ( avg >= 0.5 )
                return 1;
            else
                return 0;
        }
    }
}
