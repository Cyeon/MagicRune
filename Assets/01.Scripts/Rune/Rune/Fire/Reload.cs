using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : BaseRune
{
    public override void Init()
    {
        base.Init();
        _baseRuneSO = Managers.Resource.Load<BaseRuneSO>("SO/Rune/Fire/" + typeof(Reload).Name);
    }

    public override void Enhance()
    {
        base.Enhance();
        if (_keywordList.Contains(KeywordType.Consume))
        {
            _keywordList.Remove(KeywordType.Consume);
        }
    }

    public override void UnEnhance()
    {
        base.UnEnhance();
        if (_keywordList.Contains(KeywordType.Consume) == false)
        {
            _keywordList.Add(KeywordType.Consume);
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
