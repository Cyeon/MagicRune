using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SerializableDictionary
{
    // 나중에 내가 연구해서 다시 만들어야지

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
        /// 새로운 KeyValuePair을 추가하며, 인스펙터도 업데이트
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            SyncInspectorFromDictionary();
        }
        /// <summary>
        /// KeyValuePair을 삭제하며, 인스펙터도 업데이트
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
        /// 인스펙터를 딕셔너리로 초기화
        /// </summary>
        public void SyncInspectorFromDictionary()
        {
            //인스펙터 키 밸류 리스트 초기화
            g_InspectorKeys.Clear();
            g_InspectorValues.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                g_InspectorKeys.Add(pair.Key); g_InspectorValues.Add(pair.Value);
            }
        }

        /// <summary>
        /// 딕셔너리를 인스펙터로 초기화
        /// </summary>
        public void SyncDictionaryFromInspector()
        {
            //딕셔너리 키 밸류 리스트 초기화
            foreach (var key in Keys.ToList())
            {
                base.Remove(key);
            }

            for (int i = 0; i < g_InspectorKeys.Count; i++)
            {
                //중복된 키가 있다면 에러 출력
                if (this.ContainsKey(g_InspectorKeys[i]))
                {
                    Debug.LogError("중복된 키가 있습니다.");
                    break;
                }
                base.Add(g_InspectorKeys[i], g_InspectorValues[i]);
            }
        }

        public void OnAfterDeserialize()
        {
            Debug.Log(this + string.Format("인스펙터 키 수 : {0} 값 수 : {1}", g_InspectorKeys.Count, g_InspectorValues.Count));

            //인스펙터의 Key Value가 KeyValuePair 형태를 띌 경우
            if (g_InspectorKeys.Count == g_InspectorValues.Count)
            {
                SyncDictionaryFromInspector();
            }
        }
    }
}