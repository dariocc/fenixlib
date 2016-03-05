using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenixLib.Core
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
            ( ( IDictionary<K, E> ) decorated ).Add ( item );
        }

        public void Add ( K key, E value )
        {
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
