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
    public sealed class SpriteAssetSprite : ISprite
    {
        public int Id { get; }

        private ISprite BaseSprite { get; }

        internal SpriteAssetSprite ( int id, ISprite sprite )
        {
            Id = id;
            BaseSprite = sprite;
        }

        public GraphicFormat GraphicFormat => BaseSprite.GraphicFormat;

        public int Height => BaseSprite.Height;

        public Palette Palette => Palette;

        public byte[] PixelData => BaseSprite.PixelData;

        public int Width => BaseSprite.Width;

        public string Description
        {
            get
            {
                return BaseSprite.Description;
            }

            set
            {
                BaseSprite.Description = Description;
            }
        }

        public ICollection<PivotPoint> PivotPoints => BaseSprite.PivotPoints;

        public void ClearPivotPoints ()
        {
            BaseSprite.ClearPivotPoints ();
        }

        public void DefinePivotPoint ( int id, int x, int y )
        {
            BaseSprite.DefinePivotPoint ( id, x, y );
        }

        public void DeletePivotPoint ( int id )
        {
            BaseSprite.DeletePivotPoint ( id );
        }

        public int FindFreePivotPointId ( int start = 0, 
            Sprite.SearchDirection direction = Core.Sprite.SearchDirection.Fordward )
        {
            return BaseSprite.FindFreePivotPointId ( start, direction );
        }

        public bool IsPivotPointDefined ( int id )
        {
            return BaseSprite.IsPivotPointDefined ( id );
        }

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( obj, null ) )
            {
                return false;
            }

            SpriteAssetSprite sprite = obj as SpriteAssetSprite;
            if ( ReferenceEquals ( sprite, null ) )
            {
                return false;
            }

            return Equals ( sprite );
        }

        public bool Equals ( SpriteAssetSprite sprite )
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
