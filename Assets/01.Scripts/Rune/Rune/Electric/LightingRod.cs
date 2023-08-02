using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LightingRod : BaseRune
{
    public override void Init()
    {
        Addressables.LoadAssetAsync<BaseRuneSO>("LightingRod").Completed += (AsyncOperationHandle<BaseRuneSO> obj) =>
        {
            _baseRuneSO = obj.Result;
            base.Init();
        };
        //_baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("LightingRod");
    }

    public override void AbilityAction()
    {
        Managers.GetPlayer().StatusManager.AddStatus(StatusName.Recharging, GetAbliltiValue(EffectType.Status, StatusName.Recharging).RoundToInt());
        Managers.GetPlayer().TakeDamage(GetAbliltiValue(EffectType.Attack));
    }

    public override object Clone()
    {
        LightingRod lightingRod = new LightingRod();
        lightingRod.Init();
        lightingRod.UnEnhance();
        return lightingRod;
    }
}
