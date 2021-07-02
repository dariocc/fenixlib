/*  Copyright 2016 Dar�o Cutillas Carrillo
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FenixLib
{
    public partial class SpriteAssortment : ISpriteAssortment
    {
        private const int DefaultCapacity = 100;
        private UniformFormatGraphicDictionary<int, SpriteAssortmentSprite> sprites;

        public SpriteAssortment ( GraphicFormat format, Palette palette = null ) :
            this ( CreateSpriteCollection ( format ) )
        {
            if ( format == GraphicFormat.Format8bppIndexed )
            {
                if ( palette == null )
                {
                    throw new ArgumentNullException ( nameof ( palette ),
                        "A palette is required for RgbIndexedPalette format." );
                }
                Palette = palette;
            }
        }

        public SpriteAssortment ( Palette palette ) : this ( GraphicFormat.Format8bppIndexed, palette ) { }

        // Injecting the collection increases testability 
        private SpriteAssortment ( UniformFormatGraphicDictionary<int, SpriteAssortmentSprite> sprites )
        {
            this.sprites = sprites;
        }

        public Palette Palette { get; }

        public ICollection<SpriteAssortmentSprite> Sprites => sprites.Values;

        public IEnumerable<int> Ids => sprites.Select ( x => x.Value.Id ).OrderBy ( x => x );

        public GraphicFormat GraphicFormat => sprites.GraphicFormat;

        public SpriteAssortmentSprite this[int id]
        {
            get
            {
                return sprites[id];
            }
        }

        public void Add ( int id, ISprite sprite )
        {
            sprites.Add ( id, PrepareSprite ( id, sprite ) );
        }

        public void Update ( int id, ISprite sprite )
        {
            sprites[id] = PrepareSprite ( id, sprite );
        }

        public void Remove ( int id )
        {
            sprites.Remove ( id );
        }

        public int GetFreeId ()
        {
            return Ids.Max ( x => x ) + 1;
        }

        public IEnumerator<SpriteAssortmentSprite> GetEnumerator ()
        {
            return sprites.Values.OrderBy ( x => x.Id ).GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        // Convenience method
        private static UniformFormatGraphicDictionary<int, SpriteAssortmentSprite>
            CreateSpriteCollection ( GraphicFormat format )
        {
            return new UniformFormatGraphicDictionary<int, SpriteAssortmentSprite> (
                format, DefaultCapacity );
        }

        /// <summary>
        /// Returns a <see cref="SpriteAssortmentSprite"/> whose palette is that of the 
        /// <see cref="SpriteAssortment"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sprite"></param>
        /// <returns></returns>
        private SpriteAssortmentSprite PrepareSprite ( int id, ISprite sprite )
        {
            if ( sprite == null )
            {
                throw new ArgumentNullException ();
            }

            return new SpriteAssortmentSprite ( id, new ChildSprite ( this, sprite ) );
        }


    }
}
