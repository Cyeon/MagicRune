using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Unit : MonoBehaviour
{
    protected bool _isPlayer = false;
    public bool IsPlayer => _isPlayer;

    [SerializeField] protected float _maxHealth;
    public float MaxHealth => _maxHealth;
    [SerializeField] private float _health = 10f;

    private bool _isDie = false;

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

            if (_health <= 0) Die();
        }
    }

    [SerializeField] private float _shield = 0f;
    public float Shield
    { 
        get => _shield; 
        set
        {
            _shield = value;
            UIManager.Instance.UpdateShieldText(_isPlayer, _shield);
        }
    }

    #region  ?�태?�상 관??변??

    public float currentDmg = 0;

    #endregion

    [field:SerializeField] public UnityEvent<float> OnTakeDamage {get;set;}

    [field:SerializeField] public UnityEvent OnTakeDamageFeedback {get;set;}
    [field: SerializeField] public Dictionary<StatusInvokeTime, List<Status>> unitStatusDic = new Dictionary<StatusInvokeTime, List<Status>>();

    private void OnEnable() {
        //unitStatusDic.Add(StatusInvokeTime.Start, new List<Status>());
        //unitStatusDic.Add(StatusInvokeTime.Attack, new List<Status>());
        //unitStatusDic.Add(StatusInvokeTime.GetDamage, new List<Status>());
        //unitStatusDic.Add(StatusInvokeTime.End, new List<Status>());
    }
    
    /// <summary>
    /// ������ �޴� �Լ�
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage, bool isFixed = false, Status status = null)
    {
        currentDmg = damage;
        InvokeStatus(StatusInvokeTime.GetDamage);
        currentDmg = Mathf.Floor(currentDmg);

        if(Shield > 0 && isFixed == false)
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

        UIManager.Instance.UpdateHealthbar(false);
        UIManager.Instance.UpdateHealthbar(true);

        if(_isPlayer == false)
        {
            UIManager.Instance.DamageUIPopup(currentDmg, UIManager.Instance.enemyIcon.transform.position, status);
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

    public void InvokeStatus(StatusInvokeTime time)
    {
        List<Status> status;

        if(unitStatusDic.TryGetValue(time, out status))
        {
            if(status.Count > 0)
            {
                StatusManager.Instance.StatusFuncInvoke(status, this);
            }
        }
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
        _health += value;
        UIManager.Instance.UpdateHealthbar(true);
    }

    public void AddHPPercent(float value)
    {
        _health += value / 100 * _maxHealth;
    }

    public void AddMaxHp(float amount)
    {
        _maxHealth += amount;
    }
    
    protected virtual void Die()
    {
        _isDie = true;
    }
}