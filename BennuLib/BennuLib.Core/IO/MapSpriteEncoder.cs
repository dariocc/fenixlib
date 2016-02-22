using System;
using System.Data;
using System.Linq;

namespace Bennu.IO
{
    public class MapSpriteEncoder : NativeEncoder<Sprite>
    {
        private const int version = 0x00;

        protected override byte GetLastHeaderByte ( Sprite sprite ) => version;

        protected override void WriteNativeFormatBody ( Sprite sprite, NativeFormatWriter writer )
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
