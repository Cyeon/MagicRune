using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Linq;

public class UIManager
{
    private Dictionary<Type, List<UnityEngine.Object>> _uiDict = new Dictionary<Type, List<UnityEngine.Object>>();

    #region Bind & Get

    /// <summary>
    /// UI 바인딩 함수
    /// </summary>
    /// <typeparam name="T">UI의 타입</typeparam>
    /// <param name="type">정의된 열거자</param>
    /// <param name="gameObject">찾을 캔버스</param>
    [Obsolete]
    public void Bind<T>(Type type, GameObject gameObject = null) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>(names.Length);

        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = Utill.FindChild<T>(gameObject, names[i], true);
            if (objects[i] == null)
            {
                Debug.LogWarning($"Failed to bind {names[i]}");
            }
        }
        _uiDict.Add(typeof(T), objects);
    }

    /// <summary>
    /// UI 바인딩 함수
    /// </summary>
    /// <typeparam name="T">UI의 타입</typeparam>
    /// <param name="name">정의된 UI 이름</param>
    /// <param name="gameObject">찾을 캔버스</param>
    public void Bind<T>(string name, GameObject gameObject = null) where T : UnityEngine.Object
    {
        UnityEngine.Object objects = null;
        objects = Utill.FindChild<T>(gameObject, name, true);
        if (objects == null)
        {
            Debug.LogWarning($"Failed to bind {name}");
        }
        if (_uiDict.ContainsKey(typeof(T)))
        {
            if (_uiDict[typeof(T)].Contains(objects) == true)
            {
                return;
            }
        }

        if (_uiDict.ContainsKey(typeof(T)) == false)
        {
            _uiDict.Add(typeof(T), new List<UnityEngine.Object> { objects });
        }
        else
        {
            _uiDict[typeof(T)].Add(objects);
        }

    }

    [Obsolete]
    public T Get<T>(int index) where T : UnityEngine.Object
    {
        List<UnityEngine.Object> objects = null;

        //if (_uiObject.TryGetValue(typeof(T), out objects) == false)
        //{
        //    return null;
        //}

        //if (objects.Count <= index)
        //{
        //    return null;
        //}

        //return objects[index] as T;

        if (_uiDict.ContainsKey(typeof(T)))
        {
            objects = new List<UnityEngine.Object>(_uiDict[typeof(T)]);
            return objects[index] as T;
        }
        else
        {
            return null;
        }
    }

    public T Get<T>(string name) where T : UnityEngine.Object
    {
        List<UnityEngine.Object> objects = null;

        if (_uiDict.ContainsKey(typeof(T)))
        {
            objects = new List<UnityEngine.Object>(_uiDict[typeof(T)]);
            return objects.Find(x => x.name == name) as T;
        }
        else
        {
            return null;
        }
    }

    public void Clear()
    {
        _uiDict.Clear();
    }

    #endregion
}
