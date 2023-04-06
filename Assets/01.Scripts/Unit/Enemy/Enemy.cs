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
        if (_dialScene == null)
            _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;

        HP = MaxHealth;
        _dialScene?.HealthbarInit(false, MaxHealth);

        enemyScaleVec = SpriteRenderer.transform.localScale;
        _dialScene?.EnemyIconSetting(SpriteRenderer);
        transform.localPosition = new Vector3(0, 6, 0);

        PatternManager.ChangePattern(PatternManager.patternList[0]);
    }

    public void Attack(int damage)
    {
        currentDmg = damage;
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
            _dialScene.EnemyIcon.transform.localScale = enemyScaleVec;
        }
    }

    protected override void Die()
    {
        base.Die();
        PoolManager.Instance.Push(transform.GetComponent<Poolable>());
        //BattleManager.Instance.OnEnemyDie?.Invoke();
        //OnDieEvent?.Invoke();
    }

    private void OnDestroy()
    {
        //DOTween.KillAll();
        transform.DOKill();
    }
}
