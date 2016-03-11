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
    public class Palette : IEnumerable
    {
        private PaletteColor[] colors;

        public Palette ()
        {
            colors = new PaletteColor[256];
        }

        public Palette ( PaletteColor[] colors )
        {
            if ( colors == null )
            {
                throw new ArgumentNullException ( nameof ( colors ) );
            }

            if ( colors.Length != 256 )
            {
                throw new ArgumentException (
                    "The number of colors must be 256.", "colors" );
            }

            this.colors = colors;
        }

        public virtual PaletteColor[] Colors
        {
            get { return colors; }
        }
        public virtual PaletteColor this[int index]
        {
            get
            {
                if ( index < 0 || index > 255 )
                    throw new ArgumentOutOfRangeException ();

                return colors[index];
            }
            set
            {
                if ( index < 0 || index > 255 )
                    throw new ArgumentOutOfRangeException ();

                colors[index] = value;
            }
        }

        public virtual Palette GetCopy ()
        {
            PaletteColor[] colors = new PaletteColor[this.colors.Length];
            this.colors.CopyTo ( colors, 0 );
            return new Palette ( colors );
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
            if ( ReferenceEquals ( palette, null ) )
            {
                return false;
            }

            return ( Colors.SequenceEqual ( palette.Colors ) );
        }

        public override int GetHashCode ()
        {
            // TODO: Pending to do a proper hashcode :)
            return colors[0].R.GetHashCode () ^ colors[100].R.GetHashCode ();
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
