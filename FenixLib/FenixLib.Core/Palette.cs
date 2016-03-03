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
using System.Linq;

namespace FenixLib.Core
{
    [Serializable ()]
    public partial class Palette : IEnumerable
    {
        private Color[] colors;

        protected Palette ( Color[] colors )
        {
            this.colors = colors;
        }

        public Color[] Colors
        {
            get { return colors; }
        }
        public Color this[int index]
        {
            get { return colors[index]; }
            set { colors[index] = value; }
        }

        public static Palette Create ( Color[] colors )
        {
            if ( colors.Length < 256 )
                throw new ArgumentException (); // TODO: Customize

            return new Palette ( colors );
        }

        public Palette GetCopy ()
        {
            Color[] colors = new Color[this.colors.Length];
            this.colors.CopyTo ( colors, 0 );
            return Create ( colors );
        }

        public IEnumerator GetEnumerator ()
        {
            return colors.GetEnumerator ();
        }

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( obj, null ) )
            {
                return false;
            }

            // Should we use reference comparison?
            Palette palette = obj as Palette;
            if ( ReferenceEquals ( palette, null ) )
            {
                return false;
            }

            return Equals ( palette );
        }

        public bool Equals ( Palette palette )
        {
            // TODO: does it matter if we have the (object)?
            // Should we use reference comparison?
            if ( ReferenceEquals ( palette, null ) )
            {
                return false;
            }

            return ( Colors.SequenceEqual ( Colors ) );
        }

        public override int GetHashCode ()
        {
            // TODO: Pending to do a proper hashcode :)
            return colors[0].r.GetHashCode () ^ colors[100].r.GetHashCode ();
        }

        public static bool operator == ( Palette paletteA, Palette paletteB )
        {
            if ( ReferenceEquals ( paletteA, null ) )
            {
                return ReferenceEquals ( paletteB, null );
            }

            return paletteA.Equals ( paletteB );
        }

        public static bool operator != ( Palette paletteA, Palette paletteB )
        {
            return !( paletteA == paletteB );
        }
    }
}
