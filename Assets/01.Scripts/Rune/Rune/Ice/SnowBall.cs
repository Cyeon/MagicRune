using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(SnowBall).Name);
        base.Init();
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValue(EffectType.Attack), IsIncludeKeyword(KeywordName.Penetration));
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, 2);
    }

    public override object Clone()
    {
        SnowBall snowBall = new SnowBall();
        snowBall.Init();
        snowBall.UnEnhance();
        return snowBall;
    }
}
