/*  Copyright 2016 Dar�o Cutillas Carrillo
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
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    public class DivFilePaletteDecoder : NativeDecoder<Palette>
    {

        public override int MaxSupportedVersion { get; } = 0;

        protected override string[] KnownFileExtensions { get; } = { "pal", "map", "fpg", "fnt" };

        protected override string[] KnownFileMagics { get; } =
        {
            "fnt", "map", "fpg", "pal"
        };

        protected override Palette ReadBody ( Header header, NativeFormatReader reader )
        {
            // Map files have the Palette data in a different position than the
            // rest of the files
            if ( header.Magic == "map" )
                reader.ReadBytes ( 40 );

            return reader.ReadPalette ();
        }
    }
}
