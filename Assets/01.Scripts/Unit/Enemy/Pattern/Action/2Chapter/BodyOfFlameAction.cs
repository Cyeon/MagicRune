using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyOfFlameAction : PatternAction
{
    public override void StartAction()
    {
        BattleManager.Instance.Enemy.StatusManager.OnAddStatus += BodyOfFlame;
        base.StartAction();
    }

    public override void EndAction()
    {
        BattleManager.Instance.Enemy.StatusManager.OnAddStatus -= BodyOfFlame;
        base.EndAction();
    }

    public void BodyOfFlame(Status status, int count)
    {
        if(status.statusName == StatusName.Fire)
        {
            BattleManager.Instance.Enemy.AddHP(count, true);
        }
    }
}
