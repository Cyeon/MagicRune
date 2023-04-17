using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakingDecision : PatternDecision
{
    public override bool MakeADecision()
    {
        return BattleManager.Instance.Enemy.HP <= BattleManager.Instance.Enemy.MaxHP / 2;
    }
}
