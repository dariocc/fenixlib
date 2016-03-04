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
    public sealed class SpriteAssetElement : ISprite
    {
        public int Id { get; }
        private ISprite Sprite { get; }

        public SpriteAssetElement ( int id, ISprite sprite )
        {
            Id = id;
            Sprite = sprite;
        }

        public GraphicFormat GraphicFormat => Sprite.GraphicFormat;

        public int Height => Sprite.Height;

        public Palette Palette => Palette;

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

        public int FindFreePivotPointId ( int start = 0, 
            Sprite.SearchDirection direction = Core.Sprite.SearchDirection.Fordward )
        {
            return Sprite.FindFreePivotPointId ( start, direction );
        }

        public bool IsPivotPointDefined ( int id )
        {
            return Sprite.IsPivotPointDefined ( id );
        }

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( obj, null ) )
            {
                return false;
            }

            SpriteAssetElement sprite = obj as SpriteAssetElement;
            if ( ReferenceEquals ( sprite, null ) )
            {
                return false;
            }

            return Equals ( sprite );
        }

        public bool Equals ( SpriteAssetElement sprite )
        {
            if ( ReferenceEquals ( sprite, null ) )
            {
                return false;
            }

            return ( sprite.Id.Equals ( Id ) );
        }

        public override int GetHashCode ()
        {
            return Id.GetHashCode ();
        }
    }
}
