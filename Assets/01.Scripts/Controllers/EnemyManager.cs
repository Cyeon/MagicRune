using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private List<Enemy> _enemyList = new List<Enemy>();
    private Enemy _currentEnemy;
    public Enemy CurrentEnemy => _currentEnemy;

    public void BattleSetting()
    {
        List<Enemy> spawnEnemyList = new List<Enemy>();
        for(int i = 0; i < _enemyList.Count; i++)
        {
            Enemy enemy = Managers.Resource.Instantiate("Enemy/" + _enemyList[i].name).GetComponent<Enemy>();
            enemy.Init();
            enemy.OnDieEvent.RemoveAllListeners();
            enemy.OnDieEvent.AddListener(() => EnemyDie());
            spawnEnemyList.Add(enemy);
        }

        _enemyList = spawnEnemyList;
        _currentEnemy = _enemyList.GetRandom();
    }

    public void ResetEnemy()
    {
        _enemyList.Clear();
    }
    
    public void AddEnemy(Enemy enemy)
    {
        _enemyList.Add(enemy);
    }

    public void EnemyDie()
    {
        _enemyList.Remove(CurrentEnemy);
        if(_enemyList.Count == 0)
        {
            RewardPopup();
            return;
        }
    }

    private void RewardPopup()
    {
        REGold reward = new REGold();
        reward.SetGold(Managers.Map.CurrentChapter.Gold);
        reward.AddRewardList();

        RERune rune = new RERune();
        rune.AddRewardList();

        Define.DialScene?.RewardUI.VictoryPanelPopup();
    }
}
