using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAction : PatternAction
{
    private int _damage = 50;

    public override void StartAction()
    {
        _damage = 50;
        Enemy.PatternManager.CurrentPattern.desc = _damage.ToString();

        Enemy.OnTakeDamage.AddListener(ReduceDamage);
        base.StartAction();
    }

    public override void TurnAction()
    {
        Enemy.OnTakeDamage.RemoveListener(ReduceDamage);
        Enemy.Attack(_damage);
        base.TurnAction();
    }

    private void ReduceDamage(float damage)
    {
        this._damage -= damage.RoundToInt();
        Enemy.PatternManager.CurrentPattern.desc = _damage.ToString();
    }
}
