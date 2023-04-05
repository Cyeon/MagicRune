using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPortal : Portal
{
    private Enemy _portalEnemy;
    public AttackMapListSO attackMap;

    public override void Execute()
    {
        MapManager.Instance.selectEnemy = _portalEnemy;
        _portalEnemy.isEnter = true;
        SceneManagerEX.Instance.LoadScene("DialScene");
    }

    public void Init(Vector2 pos, Enemy enemy)
    {
        _portalEnemy = enemy;
        _spriteRenderer.sprite = enemy.SpriteRenderer.sprite;
        _titleText.text = enemy.enemyName;
        Init(pos);
    }

    public Enemy GetAttackEnemy()
    {
        List<Enemy> enemyList = GetAttackEnemyList();

        int idx = Random.Range(0, enemyList.Count);
        if (enemyList.Count == 0)
        {
            return attackMap.defaultEnemy;
        }

        return enemyList[idx];
    }

    public int GetAttackEnemyCount()
    {
        return GetAttackEnemyList().Count;
    }

    private List<Enemy> GetAttackEnemyList()
    {
        List<Enemy> enemyList = new List<Enemy>();
        for (int i = 0; i < attackMap.map.Count; ++i)
        {
            if (attackMap.map[i].MinFloor <= MapManager.Instance.Floor + 1 && attackMap.map[i].MaxFloor >= MapManager.Instance.Floor + 1)
            {
                foreach (var enemy in attackMap.map[i].enemyList)
                {
                    if (!enemy.isEnter) enemyList.Add(enemy);
                }
            }
        }

        return enemyList;
    }
}
