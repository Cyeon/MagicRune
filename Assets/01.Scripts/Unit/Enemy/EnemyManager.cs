using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public EnemySO currentEnemy;

    public Enemy SpawnEnemy(EnemySO enemy = null)
    {
        if (enemy == null)
            enemy = currentEnemy;

        currentEnemy = enemy;
        Enemy e = Instantiate(enemy.prefab).GetComponent<Enemy>();
        UIManager.Instance.enemyImage.sprite = enemy.icon;
        e.transform.position = Vector3.zero;
        e.Init(enemy);
        return e;
    }
}
