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
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.IO
{
    public sealed class MapSpriteDecoder : NativeDecoder<ISprite>
    {

        public override int MaxSupportedVersion => 0;

        protected override string[] KnownFileExtensions => new string[] { "map" };

        protected override string[] KnownFileMagics => new string[] 
        {
            "m01",
            "map",
            "m16",
            "m32"
        };

        protected override ISprite ReadBody ( Header header, AbstractNativeFormatReader reader )
        {
            int width = reader.ReadUInt16 ();
            int height = reader.ReadUInt16 ();
            int code = reader.ReadInt32 ();

            var description = reader.ReadAsciiZ ( 32 );

            var bpp = header.ParseBitsPerPixelFromMagic ();
			Palette palette = null;
            if ( bpp == 8 )
            {
                palette = reader.ReadPalette ();
				reader.ReadPaletteGammas ();
            }

            var numberOfPivotPoints = reader.ReadPivotPointsMaxIdUint16 ();
            var pivotPoints = reader.ReadPivotPoints ( numberOfPivotPoints );

            var mapDataLength = width * height * ( bpp / 8 );
            var format = ( GraphicFormat ) bpp;
			var pixelData = reader.ReadPixels ( format, width, height );

            IGraphic graphic = new Graphic ( format, width, height, pixelData, palette );
            ISprite sprite = new Sprite ( graphic );
			sprite.Description = description;

            foreach ( var pivotPoint in pivotPoints )
            {
                sprite.DefinePivotPoint ( pivotPoint.Id, pivotPoint.X, pivotPoint.Y );
            }

            return sprite;
        }
    }
}
