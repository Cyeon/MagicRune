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
        //UIManager.Instance?.Clear(); // 임시 주석 근데 나중에 풀어야함 근데 그러려만 UIManager를 다 갈아 없어야 함
        CanvasManager.Instance.SetCanvas();
    }

    public abstract void Clear();
}
