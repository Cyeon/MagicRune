using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주의사항
/// 패시브 이름 지을때 뒤에 (패시브) 붙이기 !!
/// </summary>
public abstract class Passive : MonoBehaviour
{
    protected Enemy Enemy => Managers.Enemy.CurrentEnemy;
    protected Player Player => Managers.GetPlayer();

    public Sprite passiveIcon;
    [Tooltip("** 뒤에 (패시브) 붙이기 **")]
    public string passiveName;
    [TextArea(0, 10)]
    public string passiveDescription;

    public abstract void Init();
    public abstract void Disable();
}
