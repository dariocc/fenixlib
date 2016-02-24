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
    public class SpriteAsset : IEnumerable<Sprite>
    {

        private const int MinCode = 1;
        private const int MaxCode = 999;

        protected SpriteAsset ( int depth, Palette palette = null)
        {
            Depth = depth;
            Palette = palette;
        }

        [Obsolete ()]
        private bool IsIdValid ( int x )
        {
            return x >= MinCode & x <= MaxCode;
        }

        private IDictionary<int, Sprite> _sprites = new SortedDictionary<int, Sprite> ();

        public Sprite this[int code]
        {
            get { return _sprites[code]; }
        }

        public Palette Palette { get; private set; }

        public int Depth { get; private set; }

        public ICollection<Sprite> Sprites
        {
            get { return _sprites.Values; }
        }

        public void Add ( int code, ref Sprite sprite )
        {
            if ( sprite.Depth != Depth )
                throw new InvalidOperationException ();

            _sprites.Add ( code, sprite );
            sprite.ParentAsset = this;
        }

        public void Update ( int code, Sprite map )
        {
            if ( _sprites.ContainsKey ( code ) )
            {
                _sprites.Remove ( code );
            }

            _sprites.Add ( code, map );
        }

        internal int IdOf ( Sprite sprite )
        {
            foreach ( KeyValuePair<int, Sprite> kvp in _sprites )
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
                if ( !_sprites.ContainsKey ( code ) )
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
                if ( !_sprites.ContainsKey ( code ) )
                {
                    found = true;
                }
            } while ( !( code == MinCode | found ) );

            if ( !found )
                throw new InvalidOperationException ();
            // TODO: Customize 

            return code;
        }

        public IEnumerator<Sprite> GetEnumerator ()
        {
            return _sprites.Values.GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        public static SpriteAsset Create ( DepthMode depthMode )
        {
            return new SpriteAsset ( ( int ) depthMode );
        }

        public static SpriteAsset Create ( Palette palette )
        {
            return new SpriteAsset ( ( int ) DepthMode.RgbIndexedPalette, palette );
        }
    }
}
