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
    public class SpriteAsset : ISpriteAsset
    {
        private const int DefaultCapacity = 100;
        private UniformFormatGraphicDictionary<int, SpriteAssetElement> sprites;

        public SpriteAsset ( GraphicFormat format, Palette palette = null )
        {
            if ( format == GraphicFormat.RgbIndexedPalette )
            {
                if ( palette == null )
                {
                    throw new ArgumentNullException ( "palette", 
                        "A palette is required for RgbIndexedPalette format." );
                }
                Palette = palette;
            }

            sprites = new UniformFormatGraphicDictionary<int, SpriteAssetElement> ( 
                format, DefaultCapacity );
        }

        public SpriteAsset (Palette palette) : this(GraphicFormat.RgbIndexedPalette, palette) { }

        public Palette Palette { get; }

        public ICollection<SpriteAssetElement> Sprites => sprites.Values;

        public IEnumerable<int> Ids => sprites.Select ( x => x.Value.Id ).OrderBy(x => x);

        public GraphicFormat GraphicFormat => sprites.GraphicFormat; 

        public SpriteAssetElement this[int id]
        {
            get
            {
                return sprites[id];
            }
        }

        public void Add ( int id, ISprite sprite )
        {
            sprites.Add ( id, PrepareSprite(id, sprite) );
        }

        public void Update ( int id, ISprite sprite )
        {
            sprites[id] = PrepareSprite ( id, sprite );
        }

        public int GetFreeId ()
        {
            return Ids.Max(x => x ) + 1;
        }

        public IEnumerator<SpriteAssetElement> GetEnumerator ()
        {
            return sprites.Values.OrderBy(x => x.Id).GetEnumerator ();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }

        /// <summary>
        /// Returns a <see cref="SpriteAssetElement"/> whose palette is that of the 
        /// <see cref="SpriteAsset"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sprite"></param>
        /// <returns></returns>
        private SpriteAssetElement PrepareSprite (int id, ISprite sprite)
        {
            return new SpriteAssetElement ( id, new ChildSprite ( this, sprite ) );
        }

        /// <summary>
        /// An <see cref="ISprite"/> decorator that replaces the Palette with the palette of a
        /// <see cref="SpriteAsset"/> which is considered the parent.
        /// </summary>
        private class ChildSprite : ISprite
        {
            private ISprite Sprite { get; }
            private SpriteAsset ParentAsset { get; }

            public ChildSprite ( SpriteAsset parentAsset, ISprite sprite )
            {
                ParentAsset = parentAsset;
                Sprite = sprite;
            }

            public GraphicFormat GraphicFormat => Sprite.GraphicFormat;

            public int Height => Sprite.Height;

            public Palette Palette => ParentAsset.Palette;

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
        }


        /*  private bool IsIdValid ( int x )
 {
     return x >= MinCode & x <= MaxCode;
 }*/

        /*  public ISprite this[int code]
          {
              get { return sprites[code]; }
          }

          public Palette Palette { get; private set; }

          public GraphicFormat GraphicFormat => graphicsCollection.Format

          ICollection<SpriteWithId<int>> IGenericSpriteAsset<int>.Sprites
          {
              get
              {
                  throw new NotImplementedException ();
              }
          }

          public void Add ( int code, ISprite sprite )
          {
              if ( sprite.GraphicFormat != GraphicFormat )
              {
                  throw new InvalidOperationException ();
              }

              graphicsCollection.Add
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
          }*/
        /*
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
        */
    }
}
