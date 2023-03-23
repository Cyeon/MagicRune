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
    /// ��Ż�� ��ó�� ������ ��
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// ��Ż�� ������ ��
    /// </summary>
    public abstract void Execute();
}
