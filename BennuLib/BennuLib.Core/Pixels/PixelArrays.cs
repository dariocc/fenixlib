namespace Bennu
{
    /// <summary>
    /// Helper functions to work with Arrays of pixels
    /// </summary>
    public static class PixelArrays
    {
        /// <summary>
        /// Gets the depth of the Pixel Array based on the type of its elements
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns>1, 8, 16 or 32 if the <paramref name="pixels"/> array is of known
        /// type. Otherwise it returns 0.</returns>
        public static int GetDepth(AbstractPixel[] pixels)
        {
            if (pixels is IndexedPixel[])
                return 8;
            else if (pixels is Int16Pixel565[])
                return 16;
            else if (pixels is Int32PixelARGB[])
                return 32;

            return 0;
        }
    }
}
