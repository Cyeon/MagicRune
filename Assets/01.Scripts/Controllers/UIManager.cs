using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Linq;

public enum UIType
{
    DestroyUI,
    DontDestroyUI,
}

public class UIManager
{
    private Dictionary<Type, List<UnityEngine.Object>> _destroyUIDict = new Dictionary<Type, List<UnityEngine.Object>>();
    private Dictionary<Type, List<UnityEngine.Object>> _dontDestroyUIDict = new Dictionary<Type, List<UnityEngine.Object>>();

    #region Bind

    /// <summary>
    /// UI ���ε� �Լ�
    /// </summary>
    /// <typeparam name="T">UI�� Ÿ��</typeparam>
    /// <param name="name">���ǵ� UI �̸�</param>
    /// <param name="gameObject">ã�� ĵ����</param>
    public void Bind<T>(string name, GameObject gameObject = null, UIType uiType = UIType.DestroyUI) where T : UnityEngine.Object
    {
        switch (uiType)
        {
            case UIType.DestroyUI:
                BindOfDestroyUI<T>(name, gameObject);
                break;
            case UIType.DontDestroyUI:
                BindOfDontDestroyUI<T>(name, gameObject);
                break;
        }
    }

    private void BindOfDestroyUI<T>(string name, GameObject gameObject = null) where T : UnityEngine.Object
    {
        UnityEngine.Object objects = null;
        objects = Utill.FindChild<T>(gameObject, name, true);
        if (objects == null)
        {
            Debug.LogWarning($"Failed to bind {name}");
        }
        if (_destroyUIDict.ContainsKey(typeof(T)))
        {
            if (_destroyUIDict[typeof(T)].Contains(objects) == true)
            {
                return;
            }
        }

        if (_destroyUIDict.ContainsKey(typeof(T)) == false)
        {
            _destroyUIDict.Add(typeof(T), new List<UnityEngine.Object> { objects });
        }
        else
        {
            _destroyUIDict[typeof(T)].Add(objects);
        }
    }

    private void BindOfDontDestroyUI<T>(string name, GameObject gameObject = null) where T : UnityEngine.Object
    {
        UnityEngine.Object objects = null;
        objects = Utill.FindChild<T>(gameObject, name, true);
        if (objects == null)
        {
            Debug.LogWarning($"Failed to bind {name}");
        }
        if (_dontDestroyUIDict.ContainsKey(typeof(T)))
        {
            if (_dontDestroyUIDict[typeof(T)].Contains(objects) == true)
            {
                return;
            }
        }

        if (_dontDestroyUIDict.ContainsKey(typeof(T)) == false)
        {
            _dontDestroyUIDict.Add(typeof(T), new List<UnityEngine.Object> { objects });
        }
        else
        {
            _dontDestroyUIDict[typeof(T)].Add(objects);
        }
    }

    #endregion

    #region Get

    public T Get<T>(string name, UIType uiType = UIType.DestroyUI) where T : UnityEngine.Object
    {
        switch (uiType)
        {
            case UIType.DestroyUI:
                return GetOfDestroyUI<T>(name);
            case UIType.DontDestroyUI:
                return GetOfDontDestroyUI<T>(name);
            default:
                return null;
        }
    }

    private T GetOfDestroyUI<T>(string name) where T : UnityEngine.Object
    {

        if (_destroyUIDict.ContainsKey(typeof(T)))
        {
            List<UnityEngine.Object> objects = null;
            objects = new List<UnityEngine.Object>(_destroyUIDict[typeof(T)]);
            return objects.Find(x => x.name == name) as T;
        }
        else
        {
            return null;
        }
    }

    private T GetOfDontDestroyUI<T>(string name) where T : UnityEngine.Object
    {

        if (_dontDestroyUIDict.ContainsKey(typeof(T)))
        {
            List<UnityEngine.Object> objects = null;
            objects = new List<UnityEngine.Object>(_dontDestroyUIDict[typeof(T)]);
            return objects.Find(x => x.name == name) as T;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region Remove

    public bool Remove<T>(string name, UIType type = UIType.DestroyUI) where T : UnityEngine.Object
    {
        switch (type)
        {
            case UIType.DestroyUI:
                return RemoveOfDestroyUI<T>(name);
            case UIType.DontDestroyUI:
                return RemoveOfDontDestroyUI<T>(name);
            default:
                return false;
        }
    }

    private bool RemoveOfDestroyUI<T>(string name) where T : UnityEngine.Object
    {
        if (_destroyUIDict.ContainsKey(typeof(T)))
        {
            UnityEngine.Object objects = _destroyUIDict[typeof(T)].Find(x => x.name == name);
            _destroyUIDict[typeof(T)].Remove(objects);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool RemoveOfDontDestroyUI<T>(string name) where T : UnityEngine.Object
    {
        if (_destroyUIDict.ContainsKey(typeof(T)))
        {
            UnityEngine.Object objects = _destroyUIDict[typeof(T)].Find(x => x.name == name);
            _destroyUIDict[typeof(T)].Remove(objects);
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    public void Clear()
    {
        _destroyUIDict.Clear();
    }
}
