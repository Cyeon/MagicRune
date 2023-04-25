using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 배출 패턴액션
public class EmissionAction : PatternAction
{
    public override void TurnAction()
    {
        int damage;
        int.TryParse(BattleManager.Instance.Enemy.PatternManager.CurrentPattern.desc, out damage);
        BattleManager.Instance.Enemy.Attack(damage);
        base.TurnAction();
    }
}
