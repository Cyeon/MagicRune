using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public int cost = 10; // 마나
    public AudioClip attackSound = null;

    private void Awake()
    {
        _isPlayer = true;

        var obj = FindObjectsOfType<Managers>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Attack(float dmg)
    {
        currentDmg = dmg;

        InvokeStatus(StatusInvokeTime.Attack);

        BattleManager.Instance.enemy.TakeDamage(currentDmg);
        SoundManager.Instance.PlaySound(attackSound, SoundType.Effect);

    }
}
