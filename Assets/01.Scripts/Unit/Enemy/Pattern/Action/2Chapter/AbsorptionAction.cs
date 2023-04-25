using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 흡수 패턴액션
public class AbsorptionAction : PatternAction
{
    private int _absorptionDmg = 0; //흡수한 데미지

    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.OnGetDamage += AbsorptionDamage;
        base.TurnAction();
    }

    public override void EndAction()
    {
        BattleManager.Instance.Enemy.OnGetDamage -= AbsorptionDamage;
        base.EndAction();
    }

    public void AbsorptionDamage()
    {
        int damage = (int)(BattleManager.Instance.Enemy.currentDmg * 0.25f);
        _absorptionDmg += damage;
        BattleManager.Instance.Enemy.currentDmg -= damage;
        BattleManager.Instance.Enemy.PatternManager.GetNextPattern().desc = _absorptionDmg.ToString();
    }
}
