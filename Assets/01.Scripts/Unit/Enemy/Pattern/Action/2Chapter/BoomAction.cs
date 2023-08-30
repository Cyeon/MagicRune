using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomAction : PatternAction
{
    public override void StartAction()
    {
        base.StartAction();
        if(Enemy.StatusManager.GetStatusValue(StatusName.Boom) == 0)
        {
            Enemy.StatusManager.AddStatus(StatusName.Boom, 3);
        }

        Enemy.PatternManager.CurrentPattern.desc = Enemy.StatusManager.GetStatusValue(StatusName.Boom).ToString();
        Enemy.PatternManager.UpdatePatternUI();
    }

    public override void TurnAction()
    {
        Enemy.StatusManager.RemoveStatus(StatusName.Boom, 1);
        base.TurnAction();
    }
}
