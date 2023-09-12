using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAwake : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(FireAwake).Name);
        base.Init();
    }

    public override void AbilityAction()
    {
        int value = BattleManager.Instance.Enemy.StatusManager.GetStatusValue(StatusName.Fire);
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, value);
    }

    public override object Clone()
    {
        FireAwake fireAwake = new FireAwake();
        fireAwake.Init();
        fireAwake.UnEnhance();
        return fireAwake;
    }
}