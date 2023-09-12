using DG.Tweening;
using MoreMountains.Feedbacks;
using MyBox;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] protected SpriteRenderer _healthBar;
    [SerializeField] protected SpriteRenderer _shieldBar;
    [SerializeField] protected SpriteRenderer _healthFeedbackBar;

    private Material _healthBarMat;
    private Material _shieldBarMat;
    private Material _healthFeedbackBarMat;

    [SerializeField] protected Transform _shieldIcon;
    [SerializeField] protected TextMeshPro _healthText;
    [SerializeField] protected TextMeshPro _shieldText;
    private const string MAT_POSITION_TEXT = "_Position";
    #endregion

    protected bool _isDie = false;
    public bool IsDie => _isDie;

    #region Event
    [field: SerializeField, Header("Event")] public UnityEvent<float> OnTakeDamage { get; set; }
    [field: SerializeField] public UnityEvent OnTakeDamageFeedback { get; set; }
    public UnityEvent OnDieEvent;
    public Action OnGetDamage;
    public UnityEvent OnTurnEnd;
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
        OnTurnEnd.AddListener(_statusManager.OnTurnEnd);

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

        Vector3 pos = transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        Define.DialScene?.DamageUIPopup(currentDmg, pos, status);

        if (this is Enemy)
        {
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
    }

    public virtual void Attack(float damage, ref bool isTrueDamage)
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

    public void AddHP(float value, bool isEffect = false)
    {
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
            value -= StatusManager.GetStatusValue(StatusName.Web) * 2;
            Shield += value.RoundToInt();
        }
    }

    public void AddShieldPerccent(float value)
    {
        if (_isDie == false)
        {
            Shield += (int)(value / 100 * Shield);
        }
    }

    public void ResetShield()
    {
        if (IsShiledReset)
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
        OnTurnEnd.RemoveAllListeners();

        _isDie = true;
        StopAllCoroutines();
        transform.DOKill();
        OnDieEvent?.Invoke();
    }

    public void UISetting()
    {
        if (_healthBar)
        {
            _healthBarMat = _healthBar.material;
            _shieldBarMat = _shieldBar.material;
            _healthFeedbackBarMat = _healthFeedbackBar.material;
        }

        _healthBarMat?.SetVector(MAT_POSITION_TEXT, new Vector4(1 - (float)HP / MaxHP, 0));
        _healthFeedbackBarMat?.SetVector(MAT_POSITION_TEXT, Vector4.one);

        _healthText.text = string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString());

        _shieldBarMat?.SetVector(MAT_POSITION_TEXT, Vector4.one);
        _shieldIcon.gameObject.SetActive(false);

        UpdateShieldUI();
    }

    public virtual void UpdateHealthUI()
    {
        float vectorX = _healthBarMat.GetVector(MAT_POSITION_TEXT).x;

        bool isChange = vectorX != 1 - (float)HP / MaxHP;

        _healthFeedbackBarMat?.SetVector(MAT_POSITION_TEXT, new Vector4(vectorX, 0));
        _healthBarMat?.SetVector(MAT_POSITION_TEXT, new Vector4(1 - (float)HP / MaxHP, 0));

        if (_healthText)
            _healthText?.SetText(string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString()));

        if (Shield > 0)
        {
            if (HP + Shield > MaxHP)
            {
                _shieldBarMat?.SetVector(MAT_POSITION_TEXT, Vector4.zero);
                _healthBarMat?.SetVector(MAT_POSITION_TEXT, new Vector4(1 - (float)HP / (MaxHP + Shield), 0));
            }
            else
            {
                _shieldBarMat?.SetVector(MAT_POSITION_TEXT, new Vector4(1 - (float)(HP + Shield) / MaxHP, 0));
            }

            if (_healthText)
                _healthText?.SetText(string.Format("{0}<color=#54D3CA>(+{1})</color>/{2}", HP.ToString(), Shield.ToString(), MaxHP.ToString()));
        }
        else
        {
            _shieldBarMat?.SetVector(MAT_POSITION_TEXT, Vector4.one);
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(_healthFeedbackBarMat?.DOVector(new Vector4(1 - (float)HP / MaxHP, 0), MAT_POSITION_TEXT, 0.2f));

        if (isChange)
        {
            Sequence vibrateSeq = DOTween.Sequence();
            vibrateSeq.Append(_healthFeedbackBar.transform.parent.parent?.DOShakeScale(0.1f));
            vibrateSeq.Append(_healthFeedbackBar.transform.parent.parent?.DOScale(1f, 0));
        }
    }

    public virtual void UpdateShieldUI()
    {
        if (_shield <= 0)
        {
            if (_shieldIcon.gameObject.activeSelf)
            {
                _shieldIcon.gameObject.SetActive(false);
                _shieldBarMat?.SetVector(MAT_POSITION_TEXT, Vector4.zero);
                UpdateHealthUI();
            }
            return;
        }
        else if (!_shieldIcon.gameObject.activeSelf)
        {
            _shieldIcon.gameObject.SetActive(true);
        }

        _shieldText.SetText(Shield.ToString());
        UpdateHealthUI();

        Sequence seq = DOTween.Sequence();
        seq.Append(_shieldText?.transform.parent.DOScale(1.2f, 0.1f));
        seq.Append(_shieldText?.transform.parent.DOScale(1f, 0.1f));
    }

    public void PlayAnimation(string name)
    {
        if (Animator != null)
        {
            Animator.Play(name);
        }
    }
}