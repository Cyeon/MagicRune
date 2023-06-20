using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOrb : MonoBehaviour
{
    private Enemy _enemy;
    private bool _isShield = false;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.OnGetDamage += Shield;
        _isShield = false;
    }

    private void Shield()
    {
        if (!_isShield)
        {
            float hp = _enemy.HP;
            float maxHp = _enemy.MaxHP;

            if (hp / maxHp <= 0.5f)
            {
                _isShield = true;
                _enemy.AddShield(20);
                _enemy.OnGetDamage -= Shield;
            }
        }
    }
}
