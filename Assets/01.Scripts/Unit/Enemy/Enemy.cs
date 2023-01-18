using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemySO enemyInfo;
    [SerializeField] private float atkDamage;

    public void Init(EnemySO so)
    {
        enemyInfo = so;
        _maxHealth = enemyInfo.health;
        HP = enemyInfo.health;

        UIManager.Instance.EnemyHealthbarInit(_maxHealth);
    }

    public void TurnStart()
    {
        Attack();
    }

    private void Attack()
    {
        currentDmg = atkDamage;

        InvokeStatus(StatusInvokeTime.Attack);

        GameManager.Instance.player.TakeDamage(currentDmg);
        Debug.Log("Enemy Attack");
    }
}
