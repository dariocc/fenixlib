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
using System.Collections.Generic;

namespace FenixLib.Core
{
    public partial class SpriteAssortment
    {
        /// <summary>
        /// An <see cref="ISprite"/> decorator that replaces the Palette with the palette of a
        /// <see cref="SpriteAssortment"/> which is considered the parent.
        /// </summary>
        private class ChildSprite : ISprite
        {
            private ISprite Sprite { get; }
            private SpriteAssortment ParentAssortment { get; }

            public ChildSprite ( SpriteAssortment parentAssortment, ISprite sprite )
            {
                ParentAssortment = parentAssortment;
                Sprite = sprite;
            }

            public GraphicFormat GraphicFormat => Sprite.GraphicFormat;

            public int Height => Sprite.Height;

            public Palette Palette => ParentAssortment.Palette;

            public byte[] PixelData => Sprite.PixelData;

            public int Width => Sprite.Width;

            public string Description
            {
                get
                {
                    return Sprite.Description;
                }

                set
                {
                    Sprite.Description = Description;
                }
            }

            public ICollection<PivotPoint> PivotPoints => Sprite.PivotPoints;

            public void ClearPivotPoints ()
            {
                Sprite.ClearPivotPoints ();
            }

            public void DefinePivotPoint ( int id, int x, int y )
            {
                Sprite.DefinePivotPoint ( id, x, y );
            }

            public void DeletePivotPoint ( int id )
            {
                Sprite.DeletePivotPoint ( id );
            }

            public int? FindFreePivotPointId ( int start = 0,
                Sprite.SearchDirection direction = Core.Sprite.SearchDirection.Fordward )
            {
                return Sprite.FindFreePivotPointId ( start, direction );
            }

            public bool IsPivotPointDefined ( int id )
            {
                return Sprite.IsPivotPointDefined ( id );
            }

            public PivotPoint GetPivotPoint ( int id )
            {
                return Sprite.GetPivotPoint ( id );
            }
        }
    }
}
