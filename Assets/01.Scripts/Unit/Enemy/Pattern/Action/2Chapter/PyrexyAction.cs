using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PyrexyAction : PatternAction
{
    public override void StartAction()
    {
        BattleManager.Instance.Enemy.StatusManager.OnAddStatus += Pyrexy;
        base.StartAction();
    }

    public override void EndAction()
    {
        BattleManager.Instance.Enemy.StatusManager.OnAddStatus -= Pyrexy;
        base.EndAction();
    }

    public void Pyrexy(Status status, int count)
    {
        if(status.statusName == StatusName.Fire)
        {
            BattleManager.Instance.Enemy.AddHP(count);
        }
    }
}
