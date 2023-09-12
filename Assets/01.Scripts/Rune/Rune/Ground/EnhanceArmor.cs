using MyBox;
using System.Collections;
using UnityEngine;

public class EnhanceArmor : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(EnhanceArmor).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Player.AddShield(GetAbliltiValue(EffectType.Defence).RoundToInt());
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.Armor, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        EnhanceArmor armor = new EnhanceArmor();
        armor.Init();
        armor.UnEnhance();
        return armor;
    }
}