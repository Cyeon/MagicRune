using DG.Tweening;
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

            if (_health > _maxHealth) _health = _maxHealth;
            if (Managers.Scene.CurrentScene == Define.DialScene) UpdateHealthUI();
            if(this is Player) _userInfoUI.UpdateHealthText();

            if (_health <= 0) Die();
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

    public int currentDmg = 0;
    public AudioClip attackSound = null;
    [HideInInspector] public bool isTurnSkip = false;

    #region UI
    protected Slider _healthSlider;
    protected Slider _shieldSlider;
    protected Slider _healthFeedbackSlider;
    protected TextMeshProUGUI _healthText;
    private UserInfoUI _userInfoUI;
    #endregion

    private bool _isDie = false;
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

    private void Start() {
        _statusManager = new StatusManager(this);
        statusTrm = transform.Find("Status");

        _statusManager.Reset();
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");
    }

    /// <summary>
    /// 데미지 받는 함수
    /// </summary>
    /// <param name="damage"></param>
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

        if(isTrueDamage == false)
            OnTakeDamage?.Invoke(currentDmg);
        OnTakeDamageFeedback?.Invoke();

        if (this is Enemy)
        {
            Define.DialScene?.DamageUIPopup(currentDmg, Define.MainCam.WorldToScreenPoint(transform.position), status);
        }
    }

    public virtual void Attack(float danage)
    {
        currentDmg = danage.RoundToInt();
        StatusManager.OnAttack();
        Managers.Sound.PlaySound(attackSound, SoundType.Effect);
    }

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

    public void AddHP(float value)
    {
        if (_isDie == false)
        {
            HP += value.RoundToInt();
        }
    }

    public void RemTrueHP(float value)
    {
        if(_isDie == false)
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
            _userInfoUI.UpdateHealthText();
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
        Shield = 0;
    }

    public void ResetHealth()
    {
        _isDie = false;
        _health = _maxHealth;
    }

    public virtual void Die()
    {
        _isDie = true;
        OnDieEvent?.Invoke();
    }

    public virtual void UpdateHealthUI()
    {
        Define.DialScene?.UpdateHealthbar(true);
    }

    public virtual void UpdateShieldUI()
    {
        Define.DialScene?.UpdateShieldText(Shield);
    }

}