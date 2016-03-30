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
using System.Linq;
using FenixLib.Core;

namespace FenixLib.IO
{
    public class FpgSpriteAssortmentEncoder : NativeEncoder<ISpriteAssortment>
    {
        private const int version = 0x00;

        protected override byte GetLastHeaderByte ( ISpriteAssortment what ) => version;

        protected override void WriteNativeFormatBody ( ISpriteAssortment assortment,
            NativeFormatWriter writer )
        {
            // TODO: Test palette == null and GraphicFormat = indexed
            if ( assortment.GraphicFormat == GraphicFormat.Format8bppIndexed )
            {
                writer.Write ( assortment.Palette );
                writer.WriteReservedPaletteGammaSection ();
            }

            foreach ( var sprite in assortment )
            {
                var pixelDataSize = NativeFormat.CalculatePixelBufferBytes (
                    ( int ) assortment.GraphicFormat, sprite.Width, sprite.Height );

                var writablePoints = new NativeFormatWriter.WritablePivotPointsView (
                    sprite.PivotPoints, sprite.Width, sprite.Height );

                var maplen = Convert.ToUInt32 ( 64 + pixelDataSize + writablePoints.Count * 4 );

                writer.Write ( Convert.ToUInt32 ( sprite.Id ) );
                writer.Write ( maplen );
                writer.WriteAsciiZ ( sprite.Description, 32 );
                writer.WriteAsciiZ ( "FenixLib", 12 );
                writer.Write ( Convert.ToUInt32 ( sprite.Width ) );
                writer.Write ( Convert.ToUInt32 ( sprite.Height ) );
                writer.Write ( writablePoints,
                    NativeFormatWriter.PivotPointsCountFieldType.TypeUInt32 );
                writer.Write ( sprite.PixelData );
            }
        }

        protected override string GetFileMagic ( ISpriteAssortment assortment )
        {
            switch ( ( int ) assortment.GraphicFormat )
            {
                case 1:
                    return "f01";
                case 8:
                    return "fpg";
                case 16:
                    return "f16";
                case 32:
                    return "f32";
                default:
                    throw new ArgumentException ();
            }
        }
    }
}
