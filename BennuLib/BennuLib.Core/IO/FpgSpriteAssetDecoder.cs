using static Bennu.IO.NativeFormat;

namespace Bennu.IO
{
    public class FpgSpriteAssetDecoder : NativeDecoder<SpriteAsset>
    {

        public override int MaxSupportedVersion { get; } = 0x00;

        protected override string[] KnownFileExtensions { get; } = { "fpg" };

        protected override string[] KnownFileMagics { get; } = { "f16", "f32", "fpg", "f01" };

        protected override SpriteAsset ReadBody ( Header header, NativeFormatReader reader )
        {
            SpriteAsset fpg;

            if ( header.Depth == 8 )
            {
                Palette palette = reader.ReadPalette ();
                reader.ReadUnusedPaletteGamma ();
                fpg = SpriteAsset.Create ( palette );
            }
            else
            {
                fpg = SpriteAsset.Create ( ( DepthMode ) header.Depth );
            }

            try
            {
                do
                {
                    var code = reader.ReadInt32 ();
                    var maplen = reader.ReadInt32 ();
                    var description = reader.ReadAsciiZ ( 32 );
                    var name = reader.ReadAsciiZ ( 12 );
                    var width = reader.ReadInt32 ();
                    var height = reader.ReadInt32 ();
                    var numberOfPivotPoints = reader.ReadPivotPointsNumberLong ();
                    var pivotPoints = reader.ReadPivotPoints ( numberOfPivotPoints );

                    var mapDataLength = width * height * ( header.Depth / 8 );

                    // Some tools such as FPG Edit are non conformant with the standard
                    // FPG files and will add data at the end. 
                    if ( mapDataLength + 64 + numberOfPivotPoints * 4 != maplen )
                    {
                        // It can be that many tools generate this field with wrong
                        // information. I am not even sure in SmartFpgEditor!
                        // break; 
                        // TODO: Consider if for example, we shall generate some 
                        // kind of event
                    }

                    AbstractPixel[] pixels = reader.ReadPixels ( header.Depth, width, height );

                    var map = Sprite.Create ( width, height, pixels );
                    map.Description = description;
                    foreach ( var point in pivotPoints )
                    {
                        map.DefinePivotPoint ( point.Id, point.X, point.Y );
                    }

                    fpg.Update ( code, map );

                } while ( true );

            }
            catch ( System.IO.EndOfStreamException )
            {
                // Do nothing. The file is consumed until it is not possible to 
                // read any more data.
            }

            return fpg;
        }
    }
}
