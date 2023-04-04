using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakingDecision : PatternDecision
{
    public override bool MakeADecision()
    {
        return BattleManager.Instance.enemy.HP <= BattleManager.Instance.enemy.MaxHealth / 2;
    }
}
