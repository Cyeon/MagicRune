using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Chapter
{
    public string chapterName = "";
    public int chapter = 0;
    public Sprite background;

    [Range(0, 100)]
    public float[] eventStagesChance = new float[9];

    [Header("몬스터 잡았을 때 지급될 골드")]
    public int minGold = 0;
    public int maxGold = 100;
    public int Gold => Random.Range(minGold, maxGold);

    [Header("Enemy")]
    public Enemy boss;
    public Enemy defaultEnemy;
    public List<AttackMapInfo> enemyList;

    public Enemy GetEnemy()
    {
        List<Enemy> list = GetEnemyList();   

        Enemy enemy = list.Count == 0 ? defaultEnemy : list.GetRandom();
        enemy.isEnter = true;

        return enemy;
    }

    public int GetEnemyCount()
    {
        return GetEnemyList().Count;
    }

    private List<Enemy> GetEnemyList()
    {
        List<Enemy> enemyList = new List<Enemy>();
        for (int i = 0; i < this.enemyList.Count; ++i)
        {
            if (this.enemyList[i].minStage <= Managers.Map.Stage && this.enemyList[i].maxStage >= Managers.Map.Stage)
            {
                foreach (var enemy in this.enemyList[i].enemyList)
                {
                    if (!enemy.isEnter)
                    {
                        enemyList.Add(enemy);
                    }
                }
            }
        }

        return enemyList;
    }

    public void EnemyReset()
    {
        enemyList.ForEach(x => x.Reset());
    }
}