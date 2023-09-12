using MyBox;
using System.Collections;
using UnityEngine;

public class SnowStorm : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(SnowStorm).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, GetAbliltiValue(EffectType.Status).RoundToInt());
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Frost, GetAbliltiValue(EffectType.Status, StatusName.Frost).RoundToInt());
    }

    public override object Clone()
    {
        SnowStorm ice = new SnowStorm();
        ice.Init();
        ice.UnEnhance();
        return ice;
    }
}