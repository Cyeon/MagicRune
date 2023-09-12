using MyBox;
using System.Collections;
using UnityEngine;

public class MagneticField : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Electric/" + typeof(MagneticField).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.MagneticField, GetAbliltiValue(EffectType.Status).RoundToInt());
    }

    public override object Clone()
    {
        MagneticField magneticField = new MagneticField();
        magneticField.Init();
        magneticField.UnEnhance();
        return magneticField;
    }
}