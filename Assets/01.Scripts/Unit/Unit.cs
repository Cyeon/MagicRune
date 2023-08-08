using DG.Tweening;
using MoreMountains.Feedbacks;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;
    public int MaxHP => _maxHealth;

    [SerializeField] private int _health = 10;
    public int HP 
    {
        get => _health;
        protected set
        {
            if (_isDie) return;

            _health = value;

            if (_health < 0) _health = 0;
            if (_health > _maxHealth) _health = _maxHealth;

            if (Managers.Scene.CurrentScene == Define.DialScene) UpdateHealthUI();
            if (this is Player) userInfoUI.UpdateHealthText();

            if (_health == 0) Die();
        }
    }

    [SerializeField] private int _shield = 0;
    public int Shield
    {
        get => _shield;
        protected set
        {
            _shield = value;
            UpdateShieldUI();
        }
    }
    public bool IsShiledReset = true;

    public int currentDmg = 0;
    public AudioClip attackSound = null;
    [HideInInspector] public bool isTurnSkip = false;

    public UserInfoUI userInfoUI;

    #region UI
    [Header("UI")]
    [SerializeField] protected Transform _healthBar;
    [SerializeField] protected Transform _shieldBar;
    [SerializeField] protected Transform _healthFeedbackBar;
    [SerializeField] protected Transform _shieldIcon;
    [SerializeField] protected TextMeshPro _healthText;
    [SerializeField] protected TextMeshPro _shieldText;
    #endregion

    protected bool _isDie = false;
    public bool IsDie => _isDie;

    #region Event
    [field: SerializeField, Header("Event")] public UnityEvent<float> OnTakeDamage { get; set; }
    [field: SerializeField] public UnityEvent OnTakeDamageFeedback { get; set; }
    public UnityEvent OnDieEvent;
    public Action OnGetDamage;
    #endregion

    [HideInInspector]
    public Transform statusTrm;
    private StatusManager _statusManager;
    public StatusManager StatusManager => _statusManager;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Material _hitMat;
    private Material _defaultMat;

    private Coroutine _hitCoroutine;

    [SerializeField] private MMPositionShaker _hitShaker;
    public Animator Animator;

    #region Animation Name
    public readonly string HashAttack = "Attack";
    public readonly string HashHit = "Hit";
    #endregion

    private void Start()
    {
        _statusManager = new StatusManager(this);
        statusTrm = transform.Find("Status");
        _statusManager.Reset();

        if (_spriteRenderer != null)
        {
            _defaultMat = _spriteRenderer.material;
        }
        userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");
    }

    public void TakeDamage(float damage, bool isTrueDamage = false, Status status = null)
    {
        currentDmg = damage.RoundToInt();
        OnGetDamage?.Invoke();

        if (Shield > 0 && isTrueDamage == false)
        {
            if (Shield - currentDmg >= 0)
            {
                Shield -= currentDmg;
                currentDmg = 0;
            }
            else
            {
                currentDmg -= Shield;
                Shield = 0;
                HP -= currentDmg;
            }
        }
        else
            HP -= currentDmg;

        if (isTrueDamage == false)
            OnTakeDamage?.Invoke(currentDmg);
        OnTakeDamageFeedback?.Invoke();
        PlayAnimation(HashHit);

        if (this is Enemy)
        {
            Define.DialScene?.DamageUIPopup(currentDmg, Define.MainCam.WorldToScreenPoint(transform.position), status);
            if (_hitCoroutine != null)
            {
                StopCoroutine(_hitCoroutine);
            }

            if (this.gameObject.activeSelf == true)
            {
                _hitCoroutine = StartCoroutine(HitCoroutine());
            }
        }
    }

    public bool IsHitAnimationPlaying()
    {
        return _hitShaker.Shaking;
    }

    private IEnumerator HitCoroutine()
    {
        _spriteRenderer.material = _hitMat;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _defaultMat;
    }

    public virtual void Attack(float damage, bool isTrueDamage = false)
    {
        currentDmg = damage.RoundToInt();
        StatusManager.OnAttack();
        Managers.Sound.PlaySound(attackSound, SoundType.Effect);

        if (StatusManager.IsHaveStatus(StatusName.Penetration))
        {
            StatusManager.DeleteStatus(StatusName.Penetration);
            isTrueDamage = true;
        }
    }

    #region health & shield
    public bool IsHealthAmount(float amount, ComparisonType type)
    {
        switch (type)
        {
            case ComparisonType.MoreThan:
                return HP >= amount;
            case ComparisonType.LessThan:
                return HP <= amount;
        }

        return false;
    }

    public float GetMaxHP()
    {
        return _maxHealth;
    }

    public float GetHP()
    {
        return HP;
    }

    public void AddHP(float value, bool isEffect = false)
    {
        if (value <= 0) return;

        if (_isDie == false)
        {
            HP += value.RoundToInt();
            if (isEffect == true)
            {
                GameObject healingEffect = Managers.Resource.Instantiate("Effects/HealingEffect");
                healingEffect.transform.position = this.transform.position - Vector3.up;
            }
        }
    }

    public void RemTrueHP(float value)
    {
        if (_isDie == false)
        {
            HP -= value.RoundToInt();
        }
    }

    public void AddHPPercent(float value)
    {
        if (_isDie == false)
        {
            HP += (int)(value / 100 * _maxHealth);
        }
    }

    public void AddMaxHp(float amount)
    {
        if (_isDie == false)
        {
            _maxHealth += amount.RoundToInt();
            userInfoUI.UpdateHealthText();
        }
    }

    public void AddShield(float value)
    {
        if (_isDie == false)
        {
            Shield += value.RoundToInt();
        }
    }

    public float GetShield()
    {
        return _shield;
    }

    public void ResetShield()
    {
        if(IsShiledReset)
            Shield = 0;
    }

    public void ResetHealth()
    {
        _isDie = false;
        HP = _maxHealth;
    }
    #endregion

    public virtual void Die()
    {
        _isDie = true;
        StopAllCoroutines();
        OnDieEvent?.Invoke();
    }

    public void UISetting()
    {
        _healthBar.DOScaleX((float)HP / MaxHP, 0);
        _healthFeedbackBar.DOScaleX(0, 0);
        _healthText.text = string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString());

        _shieldBar.DOScaleX(0, 0);
        _shieldIcon.gameObject.SetActive(false);
    }

    public virtual void UpdateHealthUI()
    {
        bool isChange = _healthBar.localScale.x != (float)HP / MaxHP;

        _healthFeedbackBar.DOScaleX(_healthBar.localScale.x, 0);
        _healthBar.DOScaleX((float)HP / MaxHP, 0);

        _healthText.text = string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString());

        if (Shield > 0)
        {
            if (HP + Shield > MaxHP)
            {
                _shieldBar.DOScaleX(1, 0);
                _healthBar.DOScaleX((float)HP / (MaxHP + Shield), 0);
            }
            else
            {
                _shieldBar.DOScaleX((float)(HP + Shield) / MaxHP, 0);
            }
        }
        else
        {
            _shieldBar.DOScaleX(0, 0);
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(_healthFeedbackBar.DOScaleX((float)HP / MaxHP, 0.2f));

        if (isChange)
        {
            Sequence vibrateSeq = DOTween.Sequence();
            vibrateSeq.Append(_healthFeedbackBar.parent.DOShakeScale(0.1f));
            vibrateSeq.Append(_healthFeedbackBar.parent.DOScale(1f, 0));
        }
    }

    public virtual void UpdateShieldUI()
    {
        if(_shield <= 0)
        {
            if(_shieldIcon.gameObject.activeSelf)
            {
                _shieldIcon.gameObject.SetActive(false);
                _shieldBar.DOScaleX(0, 0);
                UpdateHealthUI();
            }
            return;
        }
        else if(!_shieldIcon.gameObject.activeSelf)
        {
            _shieldIcon.gameObject.SetActive(true);
        }

        _shieldText.SetText(Shield.ToString());
        UpdateHealthUI();

        Sequence seq = DOTween.Sequence();
        seq.Append(_shieldText.transform.parent.DOScale(1.2f, 0.1f));
        seq.Append(_shieldText.transform.parent.DOScale(1f, 0.1f));
    }

    public void PlayAnimation(string name)
    {
        if(Animator != null)
        {
            Animator.Play(name);
        }
    }
}