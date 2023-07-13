using System;
using System.Collections.Generic;
using UnityEngine;

namespace NativeSerializableDictionary
{
    /// <summary>
    /// This SerializableKVP stores the Key and the Value(s) for the <seealso cref="NativeSerializableDictionary.SerializableDictionary{K, V}"/>.
    /// </summary>
    /// <typeparam name="K">This is the Key value. It must be unique.</typeparam>
    /// <typeparam name="V">This is the Value associated with the Key.</typeparam>
    /// <remarks>
    /// It is JSON serializable (Tested with <seealso cref="Newtonsoft.Json"/>).
    /// </remarks>
    [Serializable]
    public class SerializableKVP<K, V>
    {
        public V this[K Key] => Value;

        [SerializeField]
        private K _key = default;
        public K Key
        {
            get
            {
                return _key;
            }
        }
        [SerializeField]
        private V _value = default;
        public V Value
        {
            get
            {
                return _value;
            }
        }

        public SerializableKVP(K key, V value)
        {
            _key = key;
            _value = value;
        }
    }

    /// <summary>
    /// This SerializableKVP stores the Key and the Value(s) for the <seealso cref="NativeSerializableDictionary.SerializableDictionary{K, V}"/>.
    /// </summary>
    /// <typeparam name="K">This is the Key value. It must be unique.</typeparam>
    /// <typeparam name="V">This is the Value associated with the Key.</typeparam>
    /// <remarks>
    /// This alternate version boxes the values into a <seealso cref="System.Collections.Generic.List{T}"/>
    /// so we can bind the data to the List and effectively serialize types like
    /// (see: <seealso cref="UnityEngine.Vector3"/>).
    /// It is JSON serializable (Tested with <seealso cref="Newtonsoft.Json"/>).
    /// </remarks>
    [Serializable]
    public class SerializableKVPBoxed<K, V> : List<V>
    {
        public V this[K Key, int index] => Values[index];
        public List<V> this[K Key] => Values;

        [SerializeField] private K _key = default;
        public K Key
        {
            get
            {
                return _key;
            }
        }
        [SerializeField] private List<V> _values = new List<V>();
        public List<V> Values
        {
            get
            {
                return _values;
            }
        }

        /// <summary>
        /// This returns the first indexed value from the boxed List.
        /// </summary>
        public V Value
        {
            get
            {
                return _values[0];
            }
        }

        public SerializableKVPBoxed(K key, List<V> values)
        {
            _key = key;
            _values = values;
        }
        public SerializableKVPBoxed(K key, V value)
        {
            _key = key;
            List<V> container = new List<V>
            {
                value
            };
            _values = container;
        }
    }
}
