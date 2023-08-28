using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : MonoBehaviour
{
    protected Enemy Enemy => Managers.Enemy.CurrentEnemy;

    public Sprite passiveIcon;
    public string passiveName;
    [TextArea(0, 10)]
    public string passiveDescription;

    public abstract void Init();
    public abstract void Disable();
}
