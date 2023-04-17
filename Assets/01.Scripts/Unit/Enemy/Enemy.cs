using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    public string enemyName;
    public AudioClip attackSound = null;
    private Sequence idleSequence = null;
    public bool isEnter = false;
    private SpriteRenderer _spriteRenderer;
    
    public SpriteRenderer SpriteRenderer
    { 
        get
        {
            if(_spriteRenderer == null)
            {
                _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }

    public Vector3 enemyScaleVec = Vector3.one;
    private PatternManager _patternManager;
    public PatternManager PatternManager => _patternManager;

    public virtual void Init()
    {
        if(_patternManager == null)
            _patternManager = GetComponentInChildren<PatternManager>();

        HP = MaxHP;
        Define.DialScene?.HealthbarInit(false, MaxHP);

        enemyScaleVec = SpriteRenderer.transform.localScale;
        Define.DialScene?.EnemyIconSetting(SpriteRenderer);
        transform.localPosition = new Vector3(0, 6, 0);
    }

    public void Attack(int damage)
    {
        currentDmg = damage;
        StatusManager.OnAttack();

        BattleManager.Instance.player.TakeDamage(currentDmg);
        Managers.Sound.PlaySound(attackSound, SoundType.Effect);
    }

    public void Idle()
    {
        idleSequence = DOTween.Sequence();
        idleSequence.Append(Define.DialScene?.EnemyIcon.transform.DOScaleY(enemyScaleVec.y + 0.1f, 0.5f));
        idleSequence.Append(Define.DialScene?.EnemyIcon.transform.DOScaleY(enemyScaleVec.y, 0.5f));
        idleSequence.AppendInterval(0.3f);
        idleSequence.SetLoops(-1);
    }

    public void StopIdle()
    {
        idleSequence.Kill();
        Define.DialScene?.EnemyIcon.transform.DORewind();
    }

    protected override void Die()
    {
        base.Die();
        Managers.Resource.Destroy(gameObject);
        //BattleManager.Instance.OnEnemyDie?.Invoke();
        //OnDieEvent?.Invoke();
    }

    private void OnDestroy()
    {
        //DOTween.KillAll();
        transform.DOKill();
    }
}
