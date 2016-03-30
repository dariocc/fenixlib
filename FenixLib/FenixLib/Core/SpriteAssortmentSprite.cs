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
    public sealed class SpriteAssortmentSprite : ISprite
    {

        internal SpriteAssortmentSprite ( int id, ISprite sprite )
        {
            Id = id;

            baseSprite = sprite;
        }

        private readonly ISprite baseSprite;

        public int Id { get; }

        public string Description
        {
            get
            {
                return baseSprite.Description;
            }

            set
            {
                baseSprite.Description = value;
            }
        }

        public ICollection<PivotPoint> PivotPoints => baseSprite.PivotPoints;

        public GraphicFormat GraphicFormat => baseSprite.GraphicFormat;

        public int Height => baseSprite.Height;

        public Palette Palette => baseSprite.Palette;

        public int Width => baseSprite.Width;

        public byte[] PixelData => baseSprite.PixelData;

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( obj, null ) )
            {
                return false;
            }

            SpriteAssortmentSprite sprite = obj as SpriteAssortmentSprite;
            if ( ReferenceEquals ( sprite, null ) )
            {
                return false;
            }

            return Equals ( sprite );
        }

        public bool Equals ( SpriteAssortmentSprite sprite )
        {
            if ( ReferenceEquals ( sprite, null ) )
            {
                return false;
            }

            return ( sprite.Id.Equals ( Id ) );
        }

        public override int GetHashCode () => Id.GetHashCode ();

        public void ClearPivotPoints () => baseSprite.ClearPivotPoints ();

        public void DefinePivotPoint ( int id, int x, int y ) => baseSprite.DefinePivotPoint ( id, x, y );

        public void DeletePivotPoint ( int id ) => baseSprite.DeletePivotPoint ( id );

        public PivotPoint GetPivotPoint ( int id ) => baseSprite.GetPivotPoint ( id );

        public int? FindFreePivotPointId ( int start = 0, 
            Sprite.SearchDirection direction = Sprite.SearchDirection.Fordward )
        {
            return baseSprite.FindFreePivotPointId ( start, direction );
        }

        public bool IsPivotPointDefined ( int id ) => baseSprite.IsPivotPointDefined ( id );
    }
}
