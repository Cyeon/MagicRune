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
            int index = Random.Range(0, enemyList.Count);
            GameObject obj = Instantiate(enemyList[0].prefab);
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.Init(enemyList[0]);
            return enemy;
        }

        return null;
    }
}
