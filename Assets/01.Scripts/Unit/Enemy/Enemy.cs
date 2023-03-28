using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemySO enemyInfo;
    public float atkDamage;
    public Pattern pattern;

    public AudioClip attackSound = null;

    private Sequence idleSequence = null;

    private SpriteRenderer _spriteRenderer;
    
    public Vector3 enemyScaleVec = Vector3.one;

    public void Init(EnemySO so)
    {
        enemyInfo = so;
        _maxHealth = enemyInfo.health;
        HP = enemyInfo.health;
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = so.icon;

        if (_dialScene == null)
            _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
        _dialScene?.HealthbarInit(false, _maxHealth);
        
        enemyScaleVec = enemyInfo.prefab.transform.localScale;
        _spriteRenderer.transform.localScale = enemyScaleVec;

    }

    public void TurnStart()
    {
        pattern.Turn();
    }

    public void Attack()
    {
        currentDmg = atkDamage;

        InvokeStatus(StatusInvokeTime.Attack);

        BattleManager.Instance.player.TakeDamage(currentDmg);
        SoundManager.Instance.PlaySound(attackSound, SoundType.Effect);
    }

    public void Idle()
    {
        idleSequence = DOTween.Sequence();
        idleSequence.Append(_dialScene?.EnemyIcon.transform.DOScaleY(enemyScaleVec.y + 0.1f, 0.5f));
        idleSequence.Append(_dialScene?.EnemyIcon.transform.DOScaleY(enemyScaleVec.y, 0.5f));
        idleSequence.AppendInterval(0.3f);
        idleSequence.SetLoops(-1);
    }

    public void StopIdle()
    {
        idleSequence.Kill();
        _dialScene?.EnemyIcon.transform.DORewind();
        if (_dialScene != null)
        {
            _dialScene.EnemyIcon.transform.localScale = Vector3.one;
        }
    }

    protected override void Die()
    {
        base.Die();

        REGold reward = new REGold();
        reward.gold = MapManager.Instance.CurrentChapter.Gold;
        reward.AddRewardList();

        //BattleManager.Instance.OnEnemyDie?.Invoke();
        //OnDieEvent?.Invoke();
    }

    private void OnDestroy()
    {
        //DOTween.KillAll();
        transform.DOKill();
    }
}
