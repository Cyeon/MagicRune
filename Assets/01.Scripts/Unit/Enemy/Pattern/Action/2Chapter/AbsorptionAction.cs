using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 흡수 패턴액션
public class AbsorptionAction : PatternAction
{
    public int absorptionDmg = 0; //흡수한 데미지

    public override void TakeAction()
    {
        BattleManager.Instance.Enemy.OnGetDamage += AbsorptionDamage;
        base.TakeAction();
    }

    public void AbsorptionDamage()
    {
        int damage = (int)(BattleManager.Instance.Enemy.currentDmg * 0.25f);
        absorptionDmg += damage;
        BattleManager.Instance.Enemy.currentDmg -= damage;
        BattleManager.Instance.Enemy.PatternManager.GetNextPattern().desc = absorptionDmg.ToString();
    }
}
