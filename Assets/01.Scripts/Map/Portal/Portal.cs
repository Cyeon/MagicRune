using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Portal : MonoBehaviour
{
    public string portalName;
    public Sprite icon;
    public bool isUse = false;

    /// <summary>
    /// 포탈이 맨처음 생성될 때
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 포탈을 눌렀을 때
    /// </summary>
    public abstract void Execute();
}
