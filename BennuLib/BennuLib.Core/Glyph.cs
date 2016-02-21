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
        public AbstractPixel[] Pixels { get; }

        public int Depth
        {
            get
            {
                return PixelArrays.GetDepth ( Pixels );
            }
        }

        private Glyph ( int width, int height, AbstractPixel[] pixels, Palette palette = null )
        {
            Width = width;
            Height = height;
            Palette = palette;
            Pixels = pixels;
        }

        public static Glyph Create ( int width, int height, Palette palette, AbstractPixel[] pixels )
        {
            if ( width <= 0 || height <= 0 )
                throw new ArgumentOutOfRangeException (); // TODO: Customize

            if ( width * height != pixels.Length )
                throw new InvalidOperationException (); // TODO: Customize

            if ( ( palette == null ) == ( pixels[0].IsPalettized () ) )
                throw new InvalidOperationException (); // TODO: Customize       

            return new Glyph ( width, height, pixels, palette );
        }

        public static Glyph Create ( int width, int height, AbstractPixel[] pixels )
        {
            return Create ( width, height, null, pixels );
        }
    }
}
