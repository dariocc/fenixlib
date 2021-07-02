/**
 *    Copyright (c) 2016 Darío Cutillas Carrillo
 * 
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 * 
 *        http://www.apache.org/licenses/LICENSE-2.0
 * 
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 */
using System.Collections;
using System.Collections.Generic;

namespace FenixLib
{
    internal class UniformFormatGraphicDictionary<K, E> : IDictionary<K, E> where E : IGraphic
    {
        public GraphicFormat GraphicFormat { get; }
        private Dictionary<K, E> decorated;

        public UniformFormatGraphicDictionary ( GraphicFormat format )
        {
            decorated = new Dictionary<K, E> ();
            GraphicFormat = format;
        }

        public UniformFormatGraphicDictionary ( GraphicFormat format, int capacity )
        {
            decorated = new Dictionary<K, E> ( capacity );
            GraphicFormat = format;
        }

        public E this[K key]
        {
            get
            {
                return ( ( IDictionary<K, E> ) decorated )[key];
            }

            set
            {
                if ( ( value.GraphicFormat ) != GraphicFormat )
                {
                    throw new FormatMismatchException (
                        GraphicFormat, value.GraphicFormat );
                }

                ( ( IDictionary<K, E> ) decorated )[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return ( ( IDictionary<K, E> ) decorated ).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ( ( IDictionary<K, E> ) decorated ).IsReadOnly;
            }
        }

        public ICollection<K> Keys
        {
            get
            {
                return ( ( IDictionary<K, E> ) decorated ).Keys;
            }
        }

        public ICollection<E> Values
        {
            get
            {
                return ( ( IDictionary<K, E> ) decorated ).Values;
            }
        }

        public void Add ( KeyValuePair<K, E> item )
        {
            Add ( item.Key, item.Value );
        }

        public void Add ( K key, E value )
        {
            if ( ( value.GraphicFormat ) != GraphicFormat )
            {
                throw new FormatMismatchException (
                    GraphicFormat, value.GraphicFormat );
            }

            ( ( IDictionary<K, E> ) decorated ).Add ( key, value );
        }

        public void Clear ()
        {
            ( ( IDictionary<K, E> ) decorated ).Clear ();
        }

        public bool Contains ( KeyValuePair<K, E> item )
        {
            return ( ( IDictionary<K, E> ) decorated ).Contains ( item );
        }

        public bool ContainsKey ( K key )
        {
            return ( ( IDictionary<K, E> ) decorated ).ContainsKey ( key );
        }

        public void CopyTo ( KeyValuePair<K, E>[] array, int arrayIndex )
        {
            ( ( IDictionary<K, E> ) decorated ).CopyTo ( array, arrayIndex );
        }

        public IEnumerator<KeyValuePair<K, E>> GetEnumerator ()
        {
            return ( ( IDictionary<K, E> ) decorated ).GetEnumerator ();
        }

        public bool Remove ( KeyValuePair<K, E> item )
        {
            return ( ( IDictionary<K, E> ) decorated ).Remove ( item );
        }

        public bool Remove ( K key )
        {
            return ( ( IDictionary<K, E> ) decorated ).Remove ( key );
        }

        public bool TryGetValue ( K key, out E value )
        {
            return ( ( IDictionary<K, E> ) decorated ).TryGetValue ( key, out value );
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return ( ( IDictionary<K, E> ) decorated ).GetEnumerator ();
        }
    }
}
