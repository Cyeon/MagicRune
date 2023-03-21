using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager
{
    public static Dictionary<string, object> pool = new Dictionary<string, object>();
    public static Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    public static void CreatePool<T>(string name, GameObject parent, int count = 5) where T : MonoBehaviour
    {
        Queue<T> q = new Queue<T>();
        T prefab = Resources.Load<T>("Prefabs/" + name);

        for (int i = 0; i < count; i++)
        {
            GameObject g = GameObject.Instantiate(prefab.gameObject, parent.transform);

            g.SetActive(false);
            q.Enqueue(g.GetComponent<T>());
        }

        try
        {
            pool.Add(name, q);
            prefabDictionary.Add(name, prefab.gameObject);
        }
        catch (ArgumentException e)
        {
            Debug.Log(e);
            pool.Clear();
            prefabDictionary.Clear();
            pool.Add(name, q);
            prefabDictionary.Add(name, prefab.gameObject);
        }
    }

    public static T GetItem<T>(string name) where T : MonoBehaviour
    {
        T item = null;
        if (pool.ContainsKey(name))
        {
            Queue<T> q = (Queue<T>)pool[name];
            T firstItem = q.Peek();

            if (firstItem.gameObject.activeSelf)
            {  //첫번째 아이템이 이미 사용중이라면
                q.Enqueue(q.Dequeue());
                foreach (var poolObj in q)
                {
                    if (poolObj.gameObject.activeSelf == false)
                    {
                        item = poolObj;
                        item.gameObject.SetActive(true);
                        break;
                    }
                }
                if (item == null)
                {
                    GameObject prefab = prefabDictionary[name];
                    GameObject g = GameObject.Instantiate(prefab, firstItem.transform.parent);
                    item = g.GetComponent<T>();
                }
            }
            else
            {
                item = q.Dequeue();
                item.gameObject.SetActive(true);
            }
            IPoolable ipool = item.GetComponent<IPoolable>();
            if (ipool != null)
            {
                ipool.OnPool();
            }
            q.Enqueue(item);

        }
        return item;
    }
}
