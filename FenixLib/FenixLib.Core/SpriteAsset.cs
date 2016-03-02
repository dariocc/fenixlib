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

namespace FenixLib.Core
{
    [Serializable ()]
    public class SpriteAsset : IEnumerable<ISprite>
    {

        private const int MinCode = 1;
        private const int MaxCode = 999;

        protected SpriteAsset ( GraphicFormat graphicFormat, Palette palette = null)
        {
            GraphicFormat = graphicFormat;
            Palette = palette;
        }

        [Obsolete ()]
        private bool IsIdValid ( int x )
        {
            return x >= MinCode & x <= MaxCode;
        }

        private IDictionary<int, ISprite> sprites = new SortedDictionary<int, ISprite> ();

        public ISprite this[int code]
        {
            get { return sprites[code]; }
        }

        public Palette Palette { get; private set; }

        public GraphicFormat GraphicFormat { get; private set; }

        public ICollection<ISprite> Sprites
        {
            get { return sprites.Values; }
        }

        // TODO: Why ref?
        public void Add ( int code, ref ISprite sprite )
        {
            if ( sprite.GraphicFormat != GraphicFormat )
                throw new InvalidOperationException ();

            sprites.Add ( code, sprite );
            sprite.ParentAsset = this;
        }

        public void Update ( int code, ISprite map )
        {
            if ( sprites.ContainsKey ( code ) )
            {
                sprites.Remove ( code );
            }

            sprites.Add ( code, map );
        }

        internal int IdOf ( ISprite sprite )
        {
            foreach ( KeyValuePair<int, ISprite> kvp in sprites )
            {
                if ( object.ReferenceEquals ( kvp.Value, sprite ) )
                {
                    return kvp.Key;
                }
            }

            throw new ArgumentException ();
            // TODO customize
        }

        public int FindFreeId ( int startId = MinCode )
        {

            if ( IsIdValid ( startId ) )
                throw new ArgumentException ();
            // TODO: Customize

            var found = false;
            var code = startId - 1;
            do
            {
                code += 1;
                if ( !sprites.ContainsKey ( code ) )
                {
                    found = true;
                }
            } while ( !( code == MaxCode | found ) );

            if ( !found )
                throw new InvalidOperationException ();
            // TODO: Customize 

            return code;
        }

        [Obsolete]
        public int PreviousFreeCode ( int startId = MaxCode )
        {
            if ( IsIdValid ( startId ) )
                throw new ArgumentException ();
            // TODO: Customize

            var found = false;
            var code = startId + 1;
            do
            {
                code -= 1;
                if ( !sprites.ContainsKey ( code ) )
                {
                    found = true;
                }
            } while ( !( code == MinCode | found ) );

            if ( !found )
                throw new InvalidOperationException ();
            // TODO: Customize 

            return code;
        }

        public IEnumerator<ISprite> GetEnumerator ()
        {
            return sprites.Values.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        public static SpriteAsset Create ( GraphicFormat graphicFormat )
        {
            return new SpriteAsset ( graphicFormat );
        }

        public static SpriteAsset Create ( Palette palette )
        {
            return new SpriteAsset ( GraphicFormat.RgbIndexedPalette, palette );
        }
    }
}
