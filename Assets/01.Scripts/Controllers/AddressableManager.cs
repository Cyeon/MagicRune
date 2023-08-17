using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager
{
    public void Init()
    {
        Addressables.DownloadDependenciesAsync("SO");
        Addressables.LoadResourceLocationsAsync("SO");
    }

    public T Load<T>(string path) where T : Object
    {
        path = "Assets/Addressable/" + path;
        if (path.Contains("/SO/"))
        {
            path += ".asset";
        }
        return Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
    }

    public void UnLoad<T>(string path) where T : Object
    {
        T obj = Load<T>(path);
        Addressables.Release(obj);
    }
}