using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;


    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    public abstract void Clear();
}
