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
    public class MapSpriteDecoder : NativeDecoder<ISprite>
    {

        public override int MaxSupportedVersion { get; }

        protected override string[] KnownFileExtensions { get; }

        protected override string[] KnownFileMagics { get; }

        protected override ISprite ReadBody ( Header header, NativeFormatReader reader )
        {
            int width = reader.ReadUInt16 ();
            int height = reader.ReadUInt16 ();
            int code = reader.ReadInt32 ();

            var description = reader.ReadAsciiZ ( 32 );

            var bpp = header.BitsPerPixel;
			Palette palette = null;
            if ( bpp == 8 )
            {
                palette = reader.ReadPalette ();
				reader.ReadUnusedPaletteGamma ();
            }

            var numberOfPivotPoints = reader.ReadPivotPointsNumber ();
            var pivotPoints = reader.ReadPivotPoints ( numberOfPivotPoints );

            var mapDataLength = width * height * ( bpp / 8 );
			var pixelData = reader.ReadPixels ( header.BitsPerPixel, width, height );

            IGraphic graphic = new StaticGraphic ( ( GraphicFormat ) bpp, 
                width, height, pixelData, palette );
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
