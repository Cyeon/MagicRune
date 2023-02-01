using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Unit : MonoBehaviour
{
    protected bool isPlayer = false;

    [SerializeField] protected float _maxHealth;
    [SerializeField] private float _health = 10f;
    public float HP
    {
        get => _health;
        set
        {
            _health = value;
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            if (_health <= 0)
            {
                EventManager.TriggerEvent(Define.GAME_END);
                if (isPlayer)
                    EventManager.TriggerEvent(Define.GAME_LOSE);
                else
                    EventManager.TriggerEvent(Define.GAME_WIN);
            }
        }
    }

    [SerializeField] private float _shield = 0f;
    public float Shield 
    { 
        get => _shield; 
        set
        {
            _shield = value;
            UIManager.Instance.UpdateShieldText(isPlayer, _shield);
        }
    }

    

    #region  ?ÅÌÉú?¥ÏÉÅ Í¥Ä??Î≥Ä??

    public float currentDmg = 0;

    #endregion

    [field:SerializeField] public UnityEvent<float> OnTakeDamage {get;set;}

    [field:SerializeField] public UnityEvent OnTakeDamageFeedback {get;set;}
    [field: SerializeField] public Dictionary<StatusInvokeTime, List<Status>> unitStatusDic = new Dictionary<StatusInvokeTime, List<Status>>();

    private void OnEnable() {
        unitStatusDic.Add(StatusInvokeTime.Start, new List<Status>());
        unitStatusDic.Add(StatusInvokeTime.Attack, new List<Status>());
        unitStatusDic.Add(StatusInvokeTime.GetDamage, new List<Status>());
        unitStatusDic.Add(StatusInvokeTime.End, new List<Status>());
    }
    
    /// <summary>
    /// ?∞Î?ÏßÄ Î∞õÎäî ?®Ïàò
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        currentDmg = damage;
        
        InvokeStatus(StatusInvokeTime.GetDamage);

        // ÎßåÏïΩ ?¥ÎìúÍ∞Ä ?àÎã§Î©?
        if(Shield > 0)
        {
            // Î∞õÎäî ?∞Î?ÏßÄÍ∞Ä ?¥ÎìúÎ≥¥Îã§ ?¨Îã§Î©?
            if (Shield - currentDmg >= 0)
                Shield -= currentDmg; // ?¥Îìú ÍπéÍ∏∞
            else // ?ÑÎãàÎ©?
            {
                // ?¥Îìú ?ÜÏï†Í≥??®Ï? ?∞Î?ÏßÄÎßåÌÅº Ï≤¥Î†• ÍπéÍ∏∞
                currentDmg -= Shield;
                HP -= currentDmg;
            }
        }
        else
            HP -= currentDmg;

        OnTakeDamage?.Invoke(currentDmg);
        OnTakeDamageFeedback?.Invoke();

        UIManager.Instance.UpdateEnemyHealthbar();
        UIManager.Instance.UpdatePlayerHealthbar();
    }

    public bool IsHealthAmount(float amount, HealthType type)
    {
        switch (type)
        {
            case HealthType.MoreThan:
                return HP >= amount;
            case HealthType.LessThan:
                return HP <= amount;
        }

        return false;
    }

    public void InvokeStatus(StatusInvokeTime time)
    {
        List<Status> status = new List<Status>();

        if(unitStatusDic.TryGetValue(time, out status))
        {
            if(status.Count > 0)
            {
                StatusManager.Instance.StatusFuncInvoke(status);
            }
        }
    }
}