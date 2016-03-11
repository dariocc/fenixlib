﻿/*  Copyright 2016 Darío Cutillas Carrillo
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
namespace FenixLib.Core
{
    public sealed class FontGlyph : Glyph
    {
        internal FontGlyph ( char character, IGlyph glyph ) : base( glyph )
        {
            Character = character;
        }

        public char Character { get; }

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( obj, null ) )
            {
                return false;
            }

            FontGlyph glyph = obj as FontGlyph;
            if ( ReferenceEquals ( glyph, null ) )
            {
                return false;
            }

            return Equals ( glyph );
        }

        public bool Equals ( FontGlyph glyph )
        {
            if ( ReferenceEquals ( glyph, null ) )
            {
                return false;
            }

            return ( glyph.Character.Equals ( Character ) );
        }

        public override int GetHashCode ()
        {
            return Character.GetHashCode ();
        }
    }
}
