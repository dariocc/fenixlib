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
