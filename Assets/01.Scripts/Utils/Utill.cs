using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
    /// <summary>
    /// 자식을 찾아주는 함수!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go">자식을 찾을 오브젝트</param>
    /// <param name="name">찾을 자식의 이름</param>
    /// <param name="recursive">순환 여부</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T componet in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || componet.name == name)
                    return componet;
            }
        }
        return null;
    }
}
