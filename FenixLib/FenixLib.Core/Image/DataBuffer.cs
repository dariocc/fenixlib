using System;
using static FenixLib.Util.Unsafe;

namespace FenixLib.Image
{
    public interface IDataBuffer
    {
        byte[] GetBytes ();
        void SetBytes ( byte[] bytes );
    }

    public class DataBuffer<T> : IDataBuffer where T: struct
    {
        T[] data;

        public DataBuffer(int size)
        {
            if ( size <= 0 )
                throw new ArgumentOutOfRangeException ();

            data = new T[size];
        }

        public T this[int index]
        {
            get
            {
                return data[index];
            }
        }

        // TODO: Endianess-dependent?
        public byte[] GetBytes ()
        {
            return StructureToBytes<T> ( data );
        }

        // TODO: Endianess-dependent?
        public void SetBytes ( byte[] bytes )
        {
            if ( bytes.Length * sizeof ( ushort ) != data.Length )
            {
                throw new ArgumentException ();
            }

            data = BytesToStructure<T> ( bytes );
        }

        public static implicit operator int[](DataBuffer<T> data)
        {
            return new int[] { 2, 3 };
        }
    }
}
