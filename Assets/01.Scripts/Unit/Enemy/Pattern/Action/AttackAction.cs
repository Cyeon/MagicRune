using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : PatternAction
{
    public int damage = 10;
    public int count = 1;

    public override void TurnAction()
    {
        if (Managers.Enemy.CurrentEnemy.IsDie == false)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        BattleManager.Instance.Enemy.spriteRenderer.transform.DOMoveX(BattleManager.Instance.Enemy.spriteRenderer.transform.position.x - 1.5f, 0.1f);
        for (int i = 0; i < count; i++)
        {
            BattleManager.Instance.Enemy.Attack(damage);
            yield return new WaitForSeconds(0.2f);
        }
        BattleManager.Instance.Enemy.spriteRenderer.transform.DOMoveX(0, 0.1f);
        yield return new WaitForSeconds(0.1f);
        base.TurnAction();
    }
}
