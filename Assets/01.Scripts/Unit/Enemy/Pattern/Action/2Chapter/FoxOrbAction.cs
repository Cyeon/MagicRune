using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class FoxOrbAction : PatternAction
{
    private bool _isGetDamage = false;

    public override string Description => "체력이 깎이지 않으면 " + Define.BENEFIC_DESC;

    public override void StartAction()
    {
        BattleManager.Instance.Enemy.OnTakeDamage.AddListener(GetDamageCheck);
        base.StartAction();
    }

    public override void TurnAction()
    {
        if(_isGetDamage == false)
        {
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.FoxOrb, 1);
        }

        base.TurnAction();
    }

    public override void EndAction()
    {
        BattleManager.Instance.Enemy.OnTakeDamage.RemoveListener(GetDamageCheck);
        _isGetDamage = false;
        base.EndAction();
    }

    private void GetDamageCheck(float dmg)
    {
        if(dmg == 0)
        {
            _isGetDamage = true;
        }
    }
}
