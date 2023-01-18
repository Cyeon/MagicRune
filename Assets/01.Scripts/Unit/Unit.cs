using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    protected float _maxHealth;
    [SerializeField] private float _health = 10f;

    public float HP 
    {
        get => _health;
        protected set
        {
            _health = value;
            if(_health > _maxHealth)
            {
                _health = _maxHealth;
            }
        }
    }

    #region  상태이상 관련 변수

    public float currentDmg = 0;

    #endregion

    [field:SerializeField] public UnityEvent<float> OnTakeDamage {get;set;}

    [field:SerializeField] public UnityEvent OnTakeDamageFeedback {get;set;}
    [field:SerializeField] public Dictionary<StatusInvokeTime, List<Status>> unitStatusDic = new Dictionary<StatusInvokeTime, List<Status>>();

    private void OnEnable() {
        unitStatusDic.Add(StatusInvokeTime.Start, new List<Status>());
        unitStatusDic.Add(StatusInvokeTime.Attack, new List<Status>());
        unitStatusDic.Add(StatusInvokeTime.GetDamage, new List<Status>());
        unitStatusDic.Add(StatusInvokeTime.End, new List<Status>());
        Debug.Log("Unit Status Dictionary Setting");
    }
    
    public void TakeDamage(float damage)
    {
        currentDmg = damage;
        
        InvokeStatus(StatusInvokeTime.GetDamage);

        HP -= currentDmg;
        OnTakeDamage?.Invoke(currentDmg);
        OnTakeDamageFeedback?.Invoke();
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