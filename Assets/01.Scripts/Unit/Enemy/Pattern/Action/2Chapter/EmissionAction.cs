using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 배출 패턴액션
public class EmissionAction : PatternAction
{
    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.Attack(BattleManager.Instance.Enemy.StatusManager.GetStatusValue(StatusName.Absorption));
        BattleManager.Instance.Enemy.StatusManager.DeleteStatus(BattleManager.Instance.Enemy.StatusManager.GetStatus(StatusName.Absorption));
        base.TurnAction();
    }
}
