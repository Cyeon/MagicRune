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
    }
    
    /// <summary>
    /// 데미지 받는 함수
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        currentDmg = damage;
        
        InvokeStatus(StatusInvokeTime.GetDamage);

        // 만약 쉴드가 있다면
        if(Shield > 0)
        {
            // 받는 데미지가 쉴드보다 크다면
            if (Shield - currentDmg >= 0)
                Shield -= currentDmg; // 쉴드 깎기
            else // 아니면
            {
                // 쉴드 없애고 남은 데미지만큼 체력 깎기
                currentDmg -= Shield;
                HP -= currentDmg;
            }
        }
        else
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