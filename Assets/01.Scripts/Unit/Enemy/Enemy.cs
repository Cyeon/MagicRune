using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemySO enemyInfo;

    public void Init(EnemySO so)
    {
        enemyInfo = so;
        _maxHealth = HP = enemyInfo.health;
    }
}
