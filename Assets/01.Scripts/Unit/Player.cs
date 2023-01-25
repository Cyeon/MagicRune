using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public int cost = 10; // 마나

    private void Awake()
    {
        isPlayer = true;
    }

    public void Attack(float dmg)
    {
        currentDmg = dmg;

        InvokeStatus(StatusInvokeTime.Attack);

        GameManager.Instance.enemy.TakeDamage(currentDmg);
    }
}
