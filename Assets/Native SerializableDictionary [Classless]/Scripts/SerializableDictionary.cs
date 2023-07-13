using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NativeSerializableDictionary
{
    /// <summary>
    /// This SerializableDictionary works like a regular <seealso cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>.
    /// It does not require a new class for every data type.
    /// </summary>
    /// <typeparam name="K">This is the Key value. It must be unique.</typeparam>
    /// <typeparam name="V">This is the Value associated with the Key.</typeparam>
    /// <remarks> 
    /// It can be viewed within the Unity Inspector Window (Editor).
    /// It is JSON serializable (Tested with <seealso cref="Newtonsoft.Json"/>).
    /// </remarks>
    [Serializable]
    public class SerializableDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableKVP<K, V>> _keys = new List<SerializableKVP<K, V>>();

        public new V this[K Key] => _keys.First(kvp => EqualityComparer<K>.Default.Equals(kvp.Key, Key)).Value;

        public void Add(K Key, V Value)
        {
            //adding to both is necessary due to scripts not activating 
            //OnBeforeSerialize and OnAfterDeserialize
            base.Add(Key, Value);
            _keys.Add(new SerializableKVP<K, V>(Key, Value));
        }

        public void OnBeforeSerialize()
        {
            // This protects us from having
            // entries constantly added.
            if (!(Count > _keys.Count)) return;
            foreach (KeyValuePair<K, V> kvp in this)
            {
                _keys.Add(new SerializableKVP<K, V>(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            UpdateDictionaryInternal();
        }

        private void UpdateDictionaryInternal()
        {
            Clear();
            foreach (SerializableKVP<K, V> kvp in _keys)
            {
                if (kvp == null) continue;
                if (kvp.Key == null) continue;
                if (kvp.Value == null) continue;
                // This allows us to wait for duplicates
                // to be changed before adding them
                if (ContainsKey(kvp.Key)) continue;

                AddInternal(kvp.Key, kvp.Value);
            }
        }
        //this is necessary to avoid a System.InvalidOperationException 
        //in the UpdateDictionaryInternal foreach loop
        private void AddInternal(K Key, V Value)
        {
            base.Add(Key, Value);
        }
    }

    /// <summary>
    /// This SerializableDictionary works like a regular <seealso cref="System.Collections.Generic.Dictionary{TKey, TValue}"/>.
    /// It does not require a new class for every data type.
    /// </summary>
    /// <typeparam name="K">This is the Key value. It must be unique.</typeparam>
    /// <typeparam name="V">This is the Value associated with the Key.</typeparam>
    /// <remarks> 
    /// This alternate version boxes the values into a <seealso cref="System.Collections.Generic.List{T}"/>
    /// in the internal referenced custom class <seealso cref="NativeSerializableDictionary.SerializableDictionaryBoxed{K, V}"/>
    /// so we can bind the data to the List and effectively serialize types like
    /// (see: <seealso cref="UnityEngine.Vector3"/>).
    /// It can be viewed within the Unity Inspector Window (Editor).
    /// It is JSON serializable (Tested with <seealso cref="Newtonsoft.Json"/>).
    /// </remarks>
    [Serializable]
    public class SerializableDictionaryBoxed<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableKVPBoxed<K, V>> _keys = new List<SerializableKVPBoxed<K, V>>();

        public void Add(K Key, V Value)
        {
            //adding to both is necessary due to scripts not activating 
            //OnBeforeSerialize and OnAfterDeserialize
            base.Add(Key, Value);
            _keys.Add(new SerializableKVPBoxed<K, V>(Key, Value));
        }

        public void OnBeforeSerialize()
        {
            // This protects us from having
            // entries constantly added.
            if (!(Count > _keys.Count)) return;
            foreach (KeyValuePair<K, V> kvp in this)
            {
                _keys.Add(new SerializableKVPBoxed<K, V>(kvp.Key, kvp.Value));
            }
        }

        public void OnAfterDeserialize()
        {
            UpdateDictionaryInternal();
        }

        private void UpdateDictionaryInternal()
        {
            Clear();
            foreach (SerializableKVPBoxed<K, V> kvp in _keys)
            {
                if (kvp == null) continue;
                if (kvp.Key == null) continue;
                if (kvp.Values == null) continue;
                // This allows us to wait for duplicates
                // to be changed before adding them
                if (ContainsKey(kvp.Key)) continue;

                AddInternal(kvp.Key, kvp.Value);

                // we set the range of the class' inherited List
                // and then we copy over the contents, making sure
                // that the internal List is converted first so it
                // can use the CopyTo function to bind the data
                kvp.AddRange(kvp.Values);
                kvp.Values.CopyTo(kvp.ToArray());
            }
        }
        //this is necessary to avoid a System.InvalidOperationException 
        //in the UpdateDictionaryInternal foreach loop
        private void AddInternal(K Key, V Value)
        {
            base.Add(Key, Value);
        }
    }
}
