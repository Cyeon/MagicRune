using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private List<Enemy> _enemyList = new List<Enemy>();
    private Enemy _currentEnemy;
    public Enemy CurrentEnemy => _currentEnemy;

    private int _index = 0;

    public void BattleSetting()
    {
        List<Enemy> spawnEnemyList = new List<Enemy>();
        for(int i = 0; i < _enemyList.Count; i++)
        {
            if (_enemyList[i] == null) continue;

            Enemy enemy = Managers.Resource.Instantiate("Enemy/" + _enemyList[i].enemyType + "/" + _enemyList[i].name).GetComponent<Enemy>();
            enemy.Init();
            enemy.OnDieEvent.RemoveAllListeners();
            enemy.OnDieEvent.AddListener(() => EnemyDie());
            enemy.OnDieEvent.AddListener(() => enemy.PatternManager.passive?.Disable());
            enemy.gameObject.SetActive(false);
            spawnEnemyList.Add(enemy);
        }

        _index = 0;
        _enemyList = spawnEnemyList;
        _currentEnemy = _enemyList[_index];
        _currentEnemy.gameObject.SetActive(true);
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
        _index++;
        if (_enemyList.Count == _index)
        {
            RewardPopup();
            _currentEnemy.StopAllCoroutines();
            return;
        }
        else
        {
            _currentEnemy = _enemyList[_index];
            _currentEnemy.gameObject.SetActive(true);
            _currentEnemy.PatternManager.NextPattern();
            BattleManager.Instance.TurnChange();
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
