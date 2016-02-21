using System;

namespace Bennu
{
    [Serializable ()]
    public class Glyph
    {
        public int Width { get; }
        public int Height { get; }
        public Palette Palette { get; } = null;
        public int YOffset { get; set; } = 0;
        public int XOffset { get; set; } = 0;
        public int XAdvance { get; set; } = 0;
        public int YAdavance { get; set; } = 0;
        public byte[] PixelData { get; }
        public int Depth { get; }

        private Glyph ( int width, int height, int depth, byte[] pixelData, Palette palette = null )
        {
            Width = width;
            Height = height;
            Palette = palette;
            Depth = depth;
            PixelData = pixelData;
        }

        public static Glyph Create (DepthMode depth, int width, int height, byte[] pixelData, 
            Palette palette = null )
        {
            if ( width <= 0 || height <= 0 )
                throw new ArgumentOutOfRangeException (); // TODO: Customize

            // TODO: Validate the size of pixelData array
            if ( ( depth == DepthMode.RgbIndexedPalette ) != ( palette != null ) )
                throw new ArgumentException (); // TODO: Customize

            return new Glyph ( width, height, (int) depth, pixelData, palette );
        }
    }
}
