using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPortal : Portal
{
    public EnemySO enemy;

    public override void Execute()
    {
        MapManager.Instance.selectEnemy = enemy;
        SceneManagerEX.Instance.LoadScene("DialScene");
    }

    public override void Init()
    {

    }
}
