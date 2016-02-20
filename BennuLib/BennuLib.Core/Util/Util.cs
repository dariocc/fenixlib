using System;
using System.Runtime.InteropServices;

namespace Bennu.Util
{
    static class Unsafe
    {
        internal static unsafe byte[] StructureToBytes<T> ( T st ) where T : struct
        {
            var bytes = new byte[Marshal.SizeOf ( st )];
            fixed ( byte* ptr = bytes ) Marshal.StructureToPtr ( st, new IntPtr ( ptr ), true );
            return bytes;
        }
    }
}
