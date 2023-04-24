using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : Unit
{
    public string enemyName;
    public AudioClip attackSound = null;
    private Sequence idleSequence = null;
    public bool isEnter = false;

    public Vector3 enemyScaleVec = Vector3.one;
    private PatternManager _patternManager;
    public PatternManager PatternManager => _patternManager;

    #region UI

    [Header("UI")]
    public SpriteRenderer spriteRenderer;
    [SerializeField] private Transform _healthBar;
    [SerializeField] private Transform _shieldBar;
    [SerializeField] private Transform _healthFeedbackBar;
    [SerializeField] private Transform _shieldIcon;
    [SerializeField] private TextMeshPro _enemyHealthText;
    [SerializeField] private TextMeshPro _shieldText;

    #endregion

    public virtual void Init()
    {
        if(_patternManager == null)
            _patternManager = GetComponentInChildren<PatternManager>();
        
        HealthUIInit();

        enemyScaleVec = spriteRenderer.transform.localScale;
        transform.localPosition = new Vector3(0, 6, 0);
    }

    public void Attack(int damage)
    {
        currentDmg = damage;
        StatusManager.OnAttack();

        BattleManager.Instance.Player.TakeDamage(currentDmg);
        Managers.Sound.PlaySound(attackSound, SoundType.Effect);
    }

    public void Idle()
    {
        idleSequence = DOTween.Sequence();
        idleSequence.Append(spriteRenderer.transform.DOScaleY(enemyScaleVec.y + 0.1f, 0.5f));
        idleSequence.Append(spriteRenderer.transform.DOScaleY(enemyScaleVec.y, 0.5f));
        idleSequence.AppendInterval(0.3f);
        idleSequence.SetLoops(-1);
    }

    public void StopIdle()
    {
        idleSequence.Kill();
        spriteRenderer.DORewind();
    }

    protected override void Die()
    {
        base.Die();
        Managers.Resource.Destroy(gameObject);
        //BattleManager.Instance.OnEnemyDie?.Invoke();
        //OnDieEvent?.Invoke();
    }

    private void HealthUIInit()
    {
        _healthBar.DOScaleX(HP / MaxHP, 0);
        _enemyHealthText.text = string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString());

        _shieldBar.DOScaleX(0, 0);
        _healthFeedbackBar.DOScaleX(0, 0);
    }

    public override void UpdateHealthUI()
    {
        _healthFeedbackBar.DOScaleX(_healthBar.localScale.x, 0);
        _healthBar.DOScaleX(HP / MaxHP, 0);
        _enemyHealthText.text = string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString());

        if (Shield > 0)
        {
            _shieldBar.DOScaleX(Mathf.Clamp((HP + Shield) / MaxHP, 0, 1), 0);
        }
        else
        {
            _shieldBar.DOScaleX(0, 0);
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(_healthFeedbackBar.DOScaleX(HP / MaxHP, 0.2f));

        Sequence vibrateSeq = DOTween.Sequence();
        vibrateSeq.Append(_healthFeedbackBar.parent.DOShakeScale(0.1f));
        vibrateSeq.Append(_healthFeedbackBar.parent.DOScale(1f, 0));
    }

    public override void UpdateShieldUI() 
    {
        _shieldText.SetText(Shield.ToString());
        UpdateHealthUI();

        Sequence seq = DOTween.Sequence();
        seq.Append(_shieldText.transform.parent.DOScale(1.2f, 0.1f));
        seq.Append(_shieldText.transform.parent.DOScale(1f, 0.1f));
    }

    private void OnDestroy()
    {
        //DOTween.KillAll();
        transform.DOKill();
    }
}
