using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utill
{
    /// <summary>
    /// �ڽ��� ã���ִ� �Լ�!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go">�ڽ��� ã�� ������Ʈ</param>
    /// <param name="name">ã�� �ڽ��� �̸�</param>
    /// <param name="recursive">��ȯ ����</param>
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
