using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(SnowBall).Name);
    }
    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(GetAbliltiValaue(EffectType.Attack), IsIncludeKeyword(KeywordType.Penetration));
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, 2);
    }

    public override object Clone()
    {
        SnowBall snowBall = new SnowBall();
        snowBall.Init();
        return snowBall;
    }
}
