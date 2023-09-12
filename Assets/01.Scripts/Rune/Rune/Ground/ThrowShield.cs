using System.Collections;
using UnityEngine;

public class ThrowShield : BaseRune
{

    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Ground/" + typeof(ThrowShield).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().Attack(BattleManager.Instance.Player.Shield);
        
        if (!IsEnhanced)
        {
            BattleManager.Instance.Player.AddShieldPerccent(-25);
        }
    }

    public override object Clone()
    {
        ThrowShield shield = new ThrowShield();
        shield.Init();
        shield.UnEnhance();
        return shield;
    }
}