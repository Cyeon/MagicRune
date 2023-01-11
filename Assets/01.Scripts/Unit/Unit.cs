using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    public int health;

    [field:SerializeField] public UnityEvent<float> OnTakeDamage {get;set;}
    [field:SerializeField] public UnityEvent<float> OnAttack {get;set;}

    [field:SerializeField] public UnityEvent OnTakeDamageFeedback {get;set;}
    [field:SerializeField] public UnityEvent OnAttackFeedback {get;set;}

    protected virtual void Attack() { }
    
    protected void TakeDamage(float damage)
    {
        OnTakeDamage?.Invoke(damage);
        OnTakeDamageFeedback?.Invoke();
    }
}