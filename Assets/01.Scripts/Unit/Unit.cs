using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    protected float _maxHealth;
    private float _health = 0f;
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


    [field:SerializeField] public UnityEvent<float> OnTakeDamage {get;set;}

    [field:SerializeField] public UnityEvent OnTakeDamageFeedback {get;set;}

    public List<Status> OnTurnStartStatus = new List<Status>();
    public List<Status> OnAttackStatus = new List<Status>();
    public List<Status> OnTurnStopStatus = new List<Status>();
    
    protected void TakeDamage(float damage)
    {
        HP -= damage;
        OnTakeDamage?.Invoke(damage);
        OnTakeDamageFeedback?.Invoke();
    }
}