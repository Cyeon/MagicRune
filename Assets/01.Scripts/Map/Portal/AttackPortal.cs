using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPortal : Portal
{
    private Enemy _portalEnemy;
    public Enemy PortalEnemy => _portalEnemy;
    public AttackMapListSO attackMap;

    public override void Execute()
    {
        Managers.Enemy.AddEnemy(_portalEnemy);
        _portalEnemy.isEnter = true;
        Managers.Scene.LoadScene(Define.Scene.DialScene);
        base.Execute();
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
        enemyList[idx].isEnter = true;
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
            if (attackMap.map[i].MinFloor <= Managers.Map.Floor + 1 && attackMap.map[i].MaxFloor >= Managers.Map.Floor + 1)
            {
                foreach (var enemy in attackMap.map[i].enemyList)
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

}
