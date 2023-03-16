using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackPortal : Portal
{
    public EnemySO enemy;

    public override void Execute()
    {
        MapManager.Instance.selectEnemy = enemy;
        SceneManager.LoadScene("MainBattleScene");
    }

    public override void Init()
    {

    }
}
