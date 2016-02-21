using System;

namespace Bennu.Image
{
    public class Raster<T> where T : struct
    {

        public IDataBuffer PixelData { get; }
        public SampleModel<T> SampleModel;

        public Raster ( int width, int height, SampleModel<T> sampleModel, IDataBuffer pixelData )
        {

        }
    }
}
