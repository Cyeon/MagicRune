using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShieldPassive : Passive
{
    [SerializeField] private int _addShieldValue;

    public override void Disable()
    {
        Enemy.OnTurnStart.RemoveListener(() => Enemy.AddShield(_addShieldValue));
    }

    public override void Init()
    {
        Enemy.OnTurnStart.AddListener(() => Enemy.AddShield(_addShieldValue));
    }
}
