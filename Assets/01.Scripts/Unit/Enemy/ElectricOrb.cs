using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOrb : MonoBehaviour
{
    private Enemy _enemy;
    private bool _isShield = false;

    private void Start()
    {
        _enemy = transform.parent.parent.GetComponent<Enemy>();
        _enemy.OnGetDamage += Shield;
        _isShield = false;
    }

    private void Shield()
    {
        if (!_isShield)
        {
            if(_enemy.HP / _enemy.MaxHP <= 0.5f)
            {
                _isShield = true;
                _enemy.AddShield(20);
                _enemy.OnGetDamage -= Shield;
            }
        }
    }
}
