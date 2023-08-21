using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEHeating : StatusEvent
{
    public override void Invoke()
    {
        base.Invoke();
        Managers.Enemy.CurrentEnemy.StatusManager.AddStatus(StatusName.Fire, 1);
    }
}
