using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : PatternAction
{
    public int damage;
    public int count;

    public override void TakeAction()
    {
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        for(int i = 0; i < count; i++)
        {
            BattleManager.Instance.Enemy.Attack(damage);
            yield return new WaitForSeconds(0.2f);
        }
        base.TakeAction();
    }
}
