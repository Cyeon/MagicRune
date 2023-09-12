using System.Collections;
using UnityEngine;

public class WearArmor : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(WearArmor).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.Armor, 2);
    }

    public override object Clone()
    {
        WearArmor armor = new WearArmor();
        armor.Init();
        armor.UnEnhance();
        return armor;
    }
}