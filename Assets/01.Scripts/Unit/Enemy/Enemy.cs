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


    public void Init(EnemySO so)
    {
        enemyInfo = so;
        _maxHealth = enemyInfo.health;
        HP = enemyInfo.health;
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = so.icon;

        UIManager.Instance.HealthbarInit(false, _maxHealth);
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
        idleSequence.Append(UIManager.Instance.enemyIcon.DOScaleY(1.1f, 0.5f));
        idleSequence.Append(UIManager.Instance.enemyIcon.DOScaleY(1f, 0.5f));
        idleSequence.AppendInterval(0.3f);
        idleSequence.SetLoops(-1);
    }

    public void StopIdle()
    {
        idleSequence.Kill();
        UIManager.Instance.enemyIcon.DORewind();
        UIManager.Instance.enemyIcon.localScale = Vector3.one;
    }

    protected override void Die()
    {
        base.Die();

        REGold reward = new REGold();
        reward.gold = MapManager.Instance.CurrentChapter.Gold;
        reward.AddRewardList();

        BattleManager.Instance.OnEnemyDie?.Invoke();
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
