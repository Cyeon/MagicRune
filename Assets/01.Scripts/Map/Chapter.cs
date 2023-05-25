using MyBox;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Chapter
{
    public string chapterName = "";
    public int chapter = 0;

    [Header("Background Image")]
    public Sprite background;

    [Header("Reward Gold")]
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

    private List<Enemy> GetEnemyList()
    {
        List<Enemy> enemyList = new List<Enemy>();
        for (int i = 0; i < this.enemyList.Count; ++i)
        {
            if (this.enemyList[i].periodType == Managers.Map.CurrentPeriodType)
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