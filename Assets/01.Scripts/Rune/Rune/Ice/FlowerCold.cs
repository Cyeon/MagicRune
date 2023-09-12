using MyBox;
using System.Collections;
using UnityEngine;

public class FlowerCold : BaseRune
{

    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ice/" + typeof(FlowerCold).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Frost, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        FlowerCold ice = new FlowerCold();
        ice.Init();
        ice.UnEnhance();
        return ice;
    }
}