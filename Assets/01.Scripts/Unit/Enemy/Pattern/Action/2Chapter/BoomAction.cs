using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomAction : PatternAction
{
    public override void StartAction()
    {
        if(Enemy.StatusManager.GetStatusValue(StatusName.Boom) == 0)
        {
            Enemy.StatusManager.AddStatus(StatusName.Boom, 3);
        }

        Enemy.PatternManager.CurrentPattern.desc = Enemy.StatusManager.GetStatusValue(StatusName.Boom).ToString();
        Enemy.PatternManager.UpdatePatternUI();

        base.StartAction();
    }

    public override void TurnAction()
    {
        Enemy.StatusManager.RemoveStatus(StatusName.Boom, 1);
        base.TurnAction();
    }
}
