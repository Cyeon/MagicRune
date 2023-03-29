using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    public AudioClip attackSound = null;
    private Sequence idleSequence = null;
    public bool isEnter = false;
    public Sprite sprite;

    public PatternManager patternM;

    private void Awake()
    {
        sprite = GetComponentInChildren<Image>().sprite;
    }

    public void Init()
    {
        transform.localPosition = new Vector3(0, 6, 0);
        UIManager.Instance.HealthbarInit(false, _maxHealth);
        patternM.currentPattern = patternM.patternList[0];
    }

    public void Attack()
    {
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

        BattleManager.Instance.OnEnemyDie.Invoke();
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
