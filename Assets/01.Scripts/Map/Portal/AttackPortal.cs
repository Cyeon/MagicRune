using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackPortal : Portal
{
    private Enemy _portalEnemy;
    public Enemy PortalEnemy => _portalEnemy;

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
        _spriteRenderer.sprite = enemy.spriteRenderer.sprite;
        _titleText.text = enemy.enemyName;
        Init(pos);
    }
}
