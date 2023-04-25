using DG.Tweening;
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
    protected bool _isPlayer = false;
    public bool IsPlayer => _isPlayer;

    private bool _isDie = false;
    public bool IsDie => _isDie;

    [SerializeField] protected float _maxHealth;
    public float MaxHP => _maxHealth;

    [SerializeField] private float _health = 10f;
    public float HP
    {
        get => _health;
        protected set
        {
            if (_isDie) return;

            _health = value;

            if (_health > _maxHealth) _health = _maxHealth;
            if (Managers.Scene.CurrentScene == Define.DialScene) UpdateHealthUI();
            if(_isPlayer) _userInfoUI.UpdateHealthText();

            if (_health <= 0) Die();
        }
    }

    [SerializeField] private float _shield = 0f;
    public float Shield
    {
        get => _shield;
        protected set
        {
            _shield = value;
            UpdateShieldUI();
        }
    }

    public bool isTurnSkip = false;

    protected Slider _healthSlider;
    protected Slider _shieldSlider;
    protected Slider _healthFeedbackSlider;
    protected TextMeshProUGUI _healthText;

    public float currentDmg = 0;

    public Action OnGetDamage;

    [field: SerializeField] public UnityEvent<float> OnTakeDamage { get; set; }
    [field: SerializeField] public UnityEvent OnTakeDamageFeedback { get; set; }
    public UnityEvent OnDieEvent;

    [HideInInspector]
    public Transform statusTrm;
    private StatusManager _statusManager;
    public StatusManager StatusManager => _statusManager;

    private UserInfoUI _userInfoUI;

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
        currentDmg = Mathf.Floor(damage);
        OnGetDamage?.Invoke();
        currentDmg = Mathf.Floor(currentDmg);

        if (Shield > 0 && isTrueDamage == false)
        {
            if (Shield - currentDmg >= 0)
                Shield -= currentDmg;
            else
            {
                currentDmg -= Shield;
                Shield = 0;
                HP -= currentDmg;
            }
        }
        else
            HP -= currentDmg;

        OnTakeDamage?.Invoke(currentDmg);
        OnTakeDamageFeedback?.Invoke();

        if (_isPlayer == false)
        {
            Define.DialScene?.DamageUIPopup(currentDmg, Define.MainCam.WorldToScreenPoint(transform.position), status);
        }
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
            HP += value;
        }
    }

    public void RemTrueHP(float value)
    {
        if(_isDie == false)
        {
            HP -= value;
        }
    }

    public void AddHPPercent(float value)
    {
        if (_isDie == false)
        {
            HP += value / 100 * _maxHealth;
        }
    }

    public void AddMaxHp(float amount)
    {
        if (_isDie == false)
        {
            _maxHealth += amount;
            _userInfoUI.UpdateHealthText();
        }
    }

    public void AddShield(float value)
    {
        if (_isDie == false)
        {
            Shield += value;
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
        Define.DialScene?.UpdateHealthbar(IsPlayer);
    }

    public virtual void UpdateShieldUI()
    {
        Define.DialScene?.UpdateShieldText(Shield);
    }

}