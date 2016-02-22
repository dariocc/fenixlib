using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;

using Bennu;

namespace Bennu.IO
{
    public class NativeFormatWriter : BinaryWriter
    {

        private static readonly Encoding _encoding = Encoding.GetEncoding ( 850 );

        public NativeFormatWriter ( Stream input ) : base ( input, _encoding ) { }

        private void WriteHeader ( string formatHeader, byte version )
        {
            if ( formatHeader.Length != 3 )
            {
                throw new ArgumentException (); // TODO: Customize
            }

            base.Write ( _encoding.GetBytes ( formatHeader ) );
            base.Write ( NativeFormat.Terminator );
            base.Write ( version );
        }

        public void WriteAsciiZ ( string text, int maxLength )
        {
            // Texts are encoded in ASCIZZ format
            var clippedText = text.Substring ( 0, Math.Min ( text.Length, maxLength ) );
            var bytes = _encoding.GetBytes ( clippedText.ToCharArray () );
            Array.Resize ( ref bytes, maxLength );
            base.Write ( bytes );
        }

        public void Write ( Palette palette )
        {
            byte[] bytes = new byte[palette.Colors.Length * 3];
            for ( var n = 0 ; n <= palette.Colors.Length ; n++ )
            {
                bytes[n * 3] = Convert.ToByte ( palette[n].r );
                bytes[n * 3 + 1] = Convert.ToByte ( palette[n].g );
                bytes[n * 3 + 2] = Convert.ToByte ( palette[n].b );
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
