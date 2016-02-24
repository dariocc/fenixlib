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
using System.Data;
using System.Linq;
using FenixLib.Core;

namespace FenixLib.IO
{
    public class MapSpriteEncoder : NativeEncoder<Sprite>
    {
        private const int version = 0x00;

        protected override byte GetLastHeaderByte ( Sprite sprite ) => version;

        protected override void WriteNativeFormatBody ( Sprite sprite, 
            NativeFormatWriter writer )
        {
            writer.Write ( Convert.ToUInt16 ( sprite.Width ) );
            writer.Write ( Convert.ToUInt16 ( sprite.Height ) );
            writer.Write ( Convert.ToUInt32 ( sprite.Id.GetValueOrDefault () ) );

            if ( ( sprite.Palette != null ) )
            {
                writer.Write ( sprite.Palette );
                writer.WriteReservedPaletteGammaSection ();
            }

            var ids = sprite.PivotPoints.Select ( p => p.Id );

            writer.Write ( Convert.ToUInt16 ( ids.Count () > 0 ? ids.Max () : 0 ) );
            writer.Write ( sprite.PivotPoints );
            writer.Write ( sprite.PixelData );
        }

        protected override string GetFileMagic ( Sprite sprite )
        {

            switch ( sprite.Depth )
            {
                case 1:
                    return "m01";
                case 8:
                    return "map";
                case 16:
                    return "m16";
                case 32:
                    return "m32";
                default:
                    throw new ArgumentException ();
            }
        }

    }
}
