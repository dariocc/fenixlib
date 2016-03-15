using System.Drawing;
using FenixLib.Core;

namespace FenixLib.Gdip
{
    internal interface IBitmapConverter
    {
        Bitmap SourceBitmap { get; set; }

        IGraphic GetGraphic ();
    }
}