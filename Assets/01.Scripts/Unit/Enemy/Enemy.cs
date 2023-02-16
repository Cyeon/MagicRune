using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemySO enemyInfo;
    public float atkDamage;
    public Pattern pattern;

    public bool isSkip = false;

    public AudioClip attackSound = null;

    private Sequence idleSequence = null;

    public void Init(EnemySO so)
    {
        enemyInfo = so;
        _maxHealth = enemyInfo.health;
        HP = enemyInfo.health;

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

        GameManager.Instance.player.TakeDamage(currentDmg);
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
    }
}
