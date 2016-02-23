using System;
using System.Runtime.InteropServices;

namespace FenixLib.Util
{
    static class Unsafe
    {
        internal static unsafe byte[] StructureToBytes<T> ( T[] st ) where T : struct
        {
            var bytes = new byte[Marshal.SizeOf ( st )];
            fixed ( byte* ptr = bytes ) Marshal.StructureToPtr ( st, new IntPtr ( ptr ), true );
            return bytes;
        }

        internal static unsafe T[] BytesToStructure<T> ( byte[] bytes ) where T : struct
        {
            fixed (byte* ptr = bytes)
                return ( T[] ) Marshal.PtrToStructure ( new IntPtr ( ptr ), typeof ( T ) );
        }
    }
}
