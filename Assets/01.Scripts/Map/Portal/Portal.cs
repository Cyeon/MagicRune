using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Portal : MonoBehaviour
{
    public string portalName;
    public Sprite icon;

    [HideInInspector]
    public bool isUse = false;

    public abstract void Init();

    public abstract void Execute();
}
