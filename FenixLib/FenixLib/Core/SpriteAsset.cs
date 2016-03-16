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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FenixLib.Core
{
    public partial class SpriteAsset : ISpriteAsset
    {
        private const int DefaultCapacity = 100;
        private UniformFormatGraphicDictionary<int, SpriteAssetSprite> sprites;

        public SpriteAsset ( GraphicFormat format, Palette palette = null ) :
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

        public SpriteAsset ( Palette palette ) : this ( GraphicFormat.Format8bppIndexed, palette ) { }

        // Injecting the collection increases testability 
        private SpriteAsset ( UniformFormatGraphicDictionary<int, SpriteAssetSprite> sprites )
        {
            this.sprites = sprites;
        }

        public Palette Palette { get; }

        public ICollection<SpriteAssetSprite> Sprites => sprites.Values;

        public IEnumerable<int> Ids => sprites.Select ( x => x.Value.Id ).OrderBy ( x => x );

        public GraphicFormat GraphicFormat => sprites.GraphicFormat;

        public SpriteAssetSprite this[int id]
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

        public int GetFreeId ()
        {
            return Ids.Max ( x => x ) + 1;
        }

        public IEnumerator<SpriteAssetSprite> GetEnumerator ()
        {
            return sprites.Values.OrderBy ( x => x.Id ).GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        // Convenience method
        private static UniformFormatGraphicDictionary<int, SpriteAssetSprite>
            CreateSpriteCollection ( GraphicFormat format )
        {
            return new UniformFormatGraphicDictionary<int, SpriteAssetSprite> (
                format, DefaultCapacity );
        }

        /// <summary>
        /// Returns a <see cref="SpriteAssetSprite"/> whose palette is that of the 
        /// <see cref="SpriteAsset"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sprite"></param>
        /// <returns></returns>
        private SpriteAssetSprite PrepareSprite ( int id, ISprite sprite )
        {
            if ( sprite == null )
            {
                throw new ArgumentNullException ();
            }

            return new SpriteAssetSprite ( id, new ChildSprite ( this, sprite ) );
        }


    }
}
