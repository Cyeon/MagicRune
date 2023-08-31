using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : Passive
{
    [SerializeField] private float _bloodPercent = 50;

    public override void Disable()
    {
        Player.OnTakeDamage.RemoveListener(BloodFunc);
    }

    public override void Init()
    {
        Player.OnTakeDamage.AddListener(BloodFunc);
    }

    private void BloodFunc(float dmg)
    {
        dmg = dmg * (_bloodPercent / 100);
        if (dmg.RoundToInt() > 0)
        {
            Enemy.AddHP(dmg, true);
        }
    }
}
