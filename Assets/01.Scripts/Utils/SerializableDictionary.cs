using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SerializableDictionary
{
    // ���߿� ���� �����ؼ� �ٽ� ��������

    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {

        public List<TKey> g_InspectorKeys;
        public List<TValue> g_InspectorValues;

        public SerializableDictionary()
        {
            g_InspectorKeys = new List<TKey>();
            g_InspectorValues = new List<TValue>();
            SyncInspectorFromDictionary();
        }
        /// <summary>
        /// ���ο� KeyValuePair�� �߰��ϸ�, �ν����͵� ������Ʈ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            SyncInspectorFromDictionary();
        }
        /// <summary>
        /// KeyValuePair�� �����ϸ�, �ν����͵� ������Ʈ
        /// </summary>
        /// <param name="key"></param>
        public new void Remove(TKey key)
        {
            base.Remove(key);
            SyncInspectorFromDictionary();
        }

        public void OnBeforeSerialize()
        {
        }
        /// <summary>
        /// �ν����͸� ��ųʸ��� �ʱ�ȭ
        /// </summary>
        public void SyncInspectorFromDictionary()
        {
            //�ν����� Ű ��� ����Ʈ �ʱ�ȭ
            g_InspectorKeys.Clear();
            g_InspectorValues.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                g_InspectorKeys.Add(pair.Key); g_InspectorValues.Add(pair.Value);
            }
        }

        /// <summary>
        /// ��ųʸ��� �ν����ͷ� �ʱ�ȭ
        /// </summary>
        public void SyncDictionaryFromInspector()
        {
            //��ųʸ� Ű ��� ����Ʈ �ʱ�ȭ
            foreach (var key in Keys.ToList())
            {
                base.Remove(key);
            }

            for (int i = 0; i < g_InspectorKeys.Count; i++)
            {
                //�ߺ��� Ű�� �ִٸ� ���� ���
                if (this.ContainsKey(g_InspectorKeys[i]))
                {
                    Debug.LogError("�ߺ��� Ű�� �ֽ��ϴ�.");
                    break;
                }
                base.Add(g_InspectorKeys[i], g_InspectorValues[i]);
            }
        }

        public void OnAfterDeserialize()
        {
            Debug.Log(this + string.Format("�ν����� Ű �� : {0} �� �� : {1}", g_InspectorKeys.Count, g_InspectorValues.Count));

            //�ν������� Key Value�� KeyValuePair ���¸� �� ���
            if (g_InspectorKeys.Count == g_InspectorValues.Count)
            {
                SyncDictionaryFromInspector();
            }
        }
    }
}