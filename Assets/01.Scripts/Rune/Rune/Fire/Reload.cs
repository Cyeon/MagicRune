using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : BaseRune
{
    public override void Init()
    {
        _baseRuneSO = Managers.Addressable.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(Reload).Name);
        base.Init();
    }

    public override void Enhance()
    {
        base.Enhance();
        if (_keywordList.Contains(KeywordName.Consume))
        {
            _keywordList.Remove(KeywordName.Consume);
        }
    }

    public override void UnEnhance()
    {
        base.UnEnhance();
        if (_keywordList.Contains(KeywordName.Consume) == false)
        {
            _keywordList.Add(KeywordName.Consume);
        }
    }

    public override void AbilityAction()
    {
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.Penetration, 1);
    }

    public override object Clone()
    {
        Reload reload = new Reload();
        reload.Init();
        reload.UnEnhance();
        return reload;
    }
}
