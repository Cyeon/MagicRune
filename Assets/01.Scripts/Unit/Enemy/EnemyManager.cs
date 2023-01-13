using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    public List<EnemySO> enemyList;

    public Enemy SpawnEnemy()
    {
        if(enemyList.Count > 0)
        {
            GameObject obj = new GameObject();
            Enemy enemy = obj.AddComponent<Enemy>();
            enemy.Init(enemyList[0]);
            enemy.gameObject.name = enemy.enemyInfo.enemyName;
            return enemy;
        }

        return null;
    }
}
