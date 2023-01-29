using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableDictionary
{
    // ����� �� �Դϴ�~~!

    [Serializable]
    public class SerializableDictionary<TKey, TValue> where TKey : class, new() where TValue : class, new()
    {
        [SerializeField]
        private List<TKey> _keys;
        [SerializeField]
        private List<TValue> _valuse;

        public List<TKey> Keys => _keys;
        public List<TValue> Values => _valuse;

        public int Count => _keys.Count; // �׳� Ű�� ������ �ұ� ���� ������ ���� �����ұ�?

        public void Clear()
        {
            _keys.Clear();
            _valuse.Clear();

            Dictionary<int, int> d = new Dictionary<int, int>();
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
            // ���� Ű�� �̹� ������ ����
            if(ContainsKey(key))

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
    }
}