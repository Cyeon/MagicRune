using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager
{
    public void Init()
    {
        //Addressables.DownloadDependenciesAsync("SO");
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
            {
                name = name.Substring(index + 1);
            }

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
            {
                return go as T;
            }
        }

        return Addressables.LoadAssetAsync<T>(path).Result;
    }

    public void UnLoad<T>(string path) where T : Object
    {
        T obj = Load<T>(path);
        Addressables.Release(obj);
    }
}