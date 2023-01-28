using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemySO enemyInfo;
    public float atkDamage;
    public Pattern pattern;

    public bool isSkip = false;

    public void Init(EnemySO so)
    {
        enemyInfo = so;
        _maxHealth = enemyInfo.health;
        HP = enemyInfo.health;

        UIManager.Instance.EnemyHealthbarInit(_maxHealth);
    }

    public void TurnStart()
    {
        if (isSkip)
        {
            isSkip = false;
            GameManager.Instance.TurnChange();
            return;
        }

        pattern.Turn();
    }

    public void Attack()
    {
        currentDmg = atkDamage;

        InvokeStatus(StatusInvokeTime.Attack);

        GameManager.Instance.player.TakeDamage(currentDmg);
    }
}
