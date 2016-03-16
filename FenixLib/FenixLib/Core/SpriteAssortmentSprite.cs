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
    public sealed class SpriteAssortmentSprite : Sprite
    {
        public int Id { get; }

        internal SpriteAssortmentSprite ( int id, ISprite sprite ) : base ( sprite )
        {
            Id = id;

            baseSprite = sprite;
        }

        public override string Description
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

        public override ICollection<PivotPoint> PivotPoints
        {
            get
            {
                return baseSprite.PivotPoints;
            }
        }

        private readonly ISprite baseSprite;

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

        public override int GetHashCode ()
        {
            return Id.GetHashCode ();
        }
    }
}
