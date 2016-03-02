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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using FenixLib.Core;

namespace FenixLib.IO
{
    public class NativeFormatWriter : BinaryWriter
    {

        private static readonly Encoding encoding = Encoding.GetEncoding ( 850 );

        public NativeFormatWriter ( Stream input ) : base ( input, encoding ) { }

        private void WriteHeader ( string formatHeader, byte version )
        {
            if ( formatHeader.Length != 3 )
            {
                throw new ArgumentException (); // TODO: Customize
            }

            base.Write ( encoding.GetBytes ( formatHeader ) );
            base.Write ( NativeFormat.Terminator );
            base.Write ( version );
        }

        public void WriteAsciiZ ( string text, int maxLength )
        {
            // Texts are encoded in ASCIZZ format
            var clippedText = text.Substring ( 0, Math.Min ( text.Length, maxLength ) );
            var bytes = encoding.GetBytes ( clippedText.ToCharArray () );
            Array.Resize ( ref bytes, maxLength );
            base.Write ( bytes );
        }

        public void WriteExtendedGlyphInfo ( ref NativeFormat.GlyphInfo glypInfo )
        {
           Write ( ( int ) glypInfo.Width );
           Write ( ( int ) glypInfo.Height );
           Write ( ( int ) glypInfo.YOffset );
           Write ( ( int ) glypInfo.FileOffset);
        }

        public void WriteLegacyFntGlyphInfo ( ref NativeFormat.GlyphInfo glypInfo )
        {
            Write ( ( int ) glypInfo.Width );
            Write ( ( int ) glypInfo.Height );
            Write ( ( int ) glypInfo.XAdvance );
            Write ( ( int ) glypInfo.XOffset );
            Write ( ( int ) glypInfo.YOffset );
            Write ( ( int ) glypInfo.YAdvance );
            Write ( ( int ) glypInfo.FileOffset );
        }

        public void Write ( Palette palette )
        {
            byte[] bytes = new byte[palette.Colors.Length * 3];
            for ( var n = 0 ; n <= palette.Colors.Length ; n++ )
            {
                bytes[n * 3] = Convert.ToByte ( palette[n].R );
                bytes[n * 3 + 1] = Convert.ToByte ( palette[n].G );
                bytes[n * 3 + 2] = Convert.ToByte ( palette[n].B );
            }

            base.Write ( bytes );
        }

        public void Write ( IEnumerable<PivotPoint> pivotPoints )
        {
            var ids = from p in pivotPoints select p.Id;

            if ( ids.Count () == 0 )
                return;

            PivotPoint[] pivotPointsIncludingUndefined = new PivotPoint[ids.Max ()];
            for ( var n = 0 ; n <= ids.Max () ; n++ )
            {
                var id = n;
                PivotPoint? p = pivotPoints.Where ( x => x.Id == id ).FirstOrDefault ();
                pivotPointsIncludingUndefined[n] = p == null 
                    ? new PivotPoint ( id, -1, -1 ) 
                    : new PivotPoint ( id, p.Value.X, p.Value.Y );
            }

            foreach ( PivotPoint pivotPoint in pivotPointsIncludingUndefined )
            {
                Write ( Convert.ToInt16 ( pivotPoint.X ) );
                Write ( Convert.ToInt16 ( pivotPoint.Y ) );
            }
        }

        public void WriteReservedPaletteGammaSection ()
        {
            byte[] bytes = new byte[NativeFormat.ReservedBytesSize];
            base.Write ( bytes );
        }
    }
}
