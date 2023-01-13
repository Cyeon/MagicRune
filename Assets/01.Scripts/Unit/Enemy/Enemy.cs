using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemySO enemyInfo;
    
    private void Awake() {
        if(enemyInfo != null)
        {
            _maxHealth = HP = enemyInfo.health;
        }
    }
}
