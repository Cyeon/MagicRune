using MyBox;
using System.Collections;
using UnityEngine;

public class Arson : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(Arson).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, GetAbliltiValue(EffectType.Status, StatusName.Fire, index: 0).RoundToInt());
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.Fire, GetAbliltiValue(EffectType.Status, StatusName.Fire, index: 1).RoundToInt());
    }

    public override object Clone()
    {
        Arson arson = new Arson();
        arson.Init();
        arson.UnEnhance();
        return arson;
    }
}