using System;
using System.Collections.Generic;
using FenixLib.Core;
using FenixLib.IO;
using FenixLib.BitmapConvert;
using System.Drawing;

namespace FpgFromFolder
{
    public class Files2FpgMaker : IFiles2FpgMaker
    {
        public event EventHandler<ConversionEventArgs> FileAdded;
        public event EventHandler<ConversionEventArgs> FileSkipped;

        public void Make(IEnumerable<string> paths, string outFile)
        {
            var fpg = new SpriteAssortment(GraphicFormat.Format32bppArgb);
            foreach (var path in paths)
            {
                var graphic = LoadBitmap(path);           
                var sprite = new Sprite(graphic);

                var args = new ConversionEventArgs(path);
                if ( TryParseId ( path, out int id ) )
                {
                    fpg.Add(id, sprite);
                    OnPathAdded(args);
                }
                else
                {
                    OnPathSkipped(args);
                }
            }

            fpg.SaveToFpg(outFile);
        }

        protected virtual IGraphic LoadBitmap(string path)
        {
            using ( var bitmap = new Bitmap ( path ) )
            {
                return bitmap.ToGraphic(GraphicFormat.Format32bppArgb);
            }       
        }

        protected virtual bool TryParseId(string path, out int id)
        {
            var fileTitle = System.IO.Path.GetFileNameWithoutExtension(path);
            if (!Int32.TryParse(fileTitle, out id))
            {
                return false;
            }

            return id >= 1 && id <= 999 ? true : false;
        }

        protected virtual void OnPathAdded(ConversionEventArgs args)
        {
            FileAdded?.Invoke(this, args);
        }

        protected virtual void OnPathSkipped(ConversionEventArgs args)
        {
            FileSkipped?.Invoke(this, args);
        }
    }


}
