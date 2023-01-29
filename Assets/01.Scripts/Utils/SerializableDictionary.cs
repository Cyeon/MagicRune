using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableDictionary
{
    // 만드는 중 입니다~~!

    [Serializable]
    public class SerializableDictionary<TKey, TValue> where TKey : class, new() where TValue : class, new() // 제약 조건에서 불발
    {
        // 그냥 딕셔너리 변수 하나를 써야겠다.

        [SerializeField]
        private List<TKey> _keys;
        [SerializeField]
        private List<TValue> _valuse;

        public List<TKey> Keys => _keys;
        public List<TValue> Values => _valuse;

        public int Count => _keys.Count; // 그냥 키의 개수로 할까 따로 변수를 뺄서 관리할까?

        public void Clear()
        {
            _keys.Clear();
            _valuse.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            for(int i = 0; i < _keys.Count; i++)
            {
                if (_keys[i] == key)
                    return true;
            }

            return false;
        }

        public bool ContainsValue(TValue value)
        {
            for(int i = 0; i < _valuse.Count; i++)
            {
                if (_valuse[i] == value)
                    return true;
            }

            return false;
        }

        public void Add(TKey key, TValue value)
        {
            // 만약 키가 이미 있으면 에러
            if (ContainsKey(key))
            {
                Debug.LogError($"{key} Key already exists.");
                return;
            }

            _keys.Add(key);
            _valuse.Add(value);
        }

        private int GetKeyIndex(TKey key)
        {
            for(int i = 0; i < _keys.Count; i++)
            {
                if (_keys[i] == key)
                    return i;
            }

            return -1;
        }

        private int GetValueIndex(TValue value)
        {
            for (int i = 0; i < _valuse.Count; i++)
            {
                if (_valuse[i] == value)
                    return i;
            }

            return -1;
        }

        public bool Remove(TKey key)
        {
            int index = GetKeyIndex(key);
            if (index == -1)
            {
                return false;
            }
            else
            {
                _keys.RemoveAt(index);
                _valuse.RemoveAt(index);
                return true;
            }
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                return false;
            }

            _keys.Add(key);
            _valuse.Add(value);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (ContainsKey(key))
            {
                int index = GetKeyIndex(key);
                value = _valuse[index];
                return true;
            }

            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                int index = GetKeyIndex(key);
                return _valuse[index];
            }
            set
            {
                int index = GetKeyIndex(key);
                _valuse[index] = value;
            }
        }
    }
}