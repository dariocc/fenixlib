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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenixLib
{
    public sealed class FontCodePage
    {
        private readonly int _codePage;
        public int Value { get { return _codePage; } }

        private FontCodePage(int codePage)
        {
            _codePage = codePage;
        }

        internal static FontCodePage FromEncoding(Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                case 850:
                    return CP850;
                case 28591:
                    return ISO85591;
                default:
                    throw new ArgumentException (); // TODO: Customize
            }
        }

        public static FontCodePage ISO85591 = new FontCodePage(28591);
        public static FontCodePage CP850 = new FontCodePage(850);
    }
}
