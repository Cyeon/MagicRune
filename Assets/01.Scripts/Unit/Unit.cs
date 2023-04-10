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
    public float MaxHealth => _maxHealth;

    [SerializeField] private float _health = 10f;
    public float HP
    {
        get => _health;
        protected set
        {
            if (_isDie) return;

            _health = value;
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            if (_dialScene == null)
            {
                _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
            }
            if (SceneManagerEX.Instance.CurrentScene == _dialScene)
            {
                _dialScene?.UpdateHealthbar(IsPlayer);
            }

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

            if (_dialScene == null)
            {
                _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
            }
            _dialScene?.UpdateShieldText(_isPlayer, _shield);
        }
    }

    protected Slider _healthSlider;
    protected Slider _shieldSlider;
    protected Slider _healthFeedbackSlider;
    protected TextMeshProUGUI _healthText;

    #region  ?�태?�상 관??변??

    public float currentDmg = 0;

    #endregion

    [field: SerializeField] public UnityEvent<float> OnTakeDamage { get; set; }

    [field: SerializeField] public UnityEvent OnTakeDamageFeedback { get; set; }
    //[field: SerializeField] public Dictionary<StatusInvokeTime, List<Status>> unitStatusDic = new Dictionary<StatusInvokeTime, List<Status>>();

    public UnityEvent OnDieEvent;
    protected DialScene _dialScene;

    [Header("Status")]
    public Transform statusTrm;
    private StatusManager _statusManager;
    public StatusManager StatusManager => _statusManager;

    private void Start() {
        _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
        _statusManager = new StatusManager(this, _dialScene);
        _statusManager.Reset();

        statusTrm = transform.Find("Status");
    }

    /// <summary>
    /// ������ �޴� �Լ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage, bool isFixed = false, Status status = null)
    {
        currentDmg = damage;
        _statusManager.OnGetDamage();
        currentDmg = Mathf.Floor(currentDmg);

        if (Shield > 0 && isFixed == false)
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

        if (_dialScene == null)
        {
            _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
        }

        if (_isPlayer == false)
        {
            if (_dialScene != null)
            {
                _dialScene.DamageUIPopup(currentDmg, Define.MainCam.WorldToScreenPoint(_dialScene.EnemyIcon.transform.position), status);
            }
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

    //public void InvokeStatus(StatusInvokeTime time)
    //{
    //    if (IsDie == true) return;

    //    List<Status> status;

    //    if (unitStatusDic.TryGetValue(time, out status))
    //    {
    //        if (status.Count > 0)
    //        {
    //            StatusManager.Instance.StatusFuncInvoke(status, this);
    //        }
    //    }
    //}

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
            if(_dialScene == null)
            {
                _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
            }
            _dialScene?.UpdateHealthbar(IsPlayer);
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

    protected virtual void Die()
    {
        _isDie = true;
        OnDieEvent?.Invoke();
    }
}