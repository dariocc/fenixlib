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
using System.Drawing;
using System.Drawing.Imaging;
using FenixLib.Core;

namespace FenixLib.IO
{
    public static class GraphicExtensions
    {
        public static Bitmap ToBitmap ( IGraphic graphic )
        {
            var converter = new GraphicToBitmapConverter ();
            converter.SourceGraphic = graphic;
            return converter.GetBitmap ();
        }
    }

    public static class BitmapExtensions
    {
        public static IGraphic ToGraphic ( this Bitmap bitmap )
        {
            var converter = (new Bitmap2GraphicConverterFactory ()).Create( bitmap );
            return converter.Convert( bitmap );
        }
    }
}
