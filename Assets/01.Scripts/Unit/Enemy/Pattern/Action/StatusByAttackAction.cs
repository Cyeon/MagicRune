using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusByAttackAction : PatternAction
{
    [SerializeField] private StatusName _status;
    private int _damage;

    public override string Description => Define.DamageDesc(_damage);

    public override void StartAction()
    {
        _damage = Enemy.StatusManager.GetStatusValue(_status);

        Enemy.PatternManager.CurrentPattern.patternValue = _damage.ToString();
        Enemy.PatternManager.UpdatePatternUI();

        base.StartAction();
    }

    public override void TurnAction()
    {
        Enemy.Attack(_damage);
        Enemy.StatusManager.DeleteStatus(_status);

        base.TurnAction();
    }
}
