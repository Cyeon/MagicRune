using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemoveType
{ 
    Int,
    Percent
}

public class SERemoveDamage : StatusEvent
{
    public RemoveType removeType = RemoveType.Int;
    [SerializeField, ConditionalField(nameof(removeType), false, RemoveType.Percent)]
    private float _percent = 0;
    [SerializeField, ConditionalField(nameof(removeType), false, RemoveType.Int)]
    private int _value = 0;

    [SerializeField] private bool _isSelf = false;

    public override void Invoke()
    {
        base.Invoke();

        Unit unit = _unit;
        if (!_isSelf)
        {
            if (_unit == Managers.GetPlayer()) unit = BattleManager.Instance.Enemy;
            else unit = Managers.GetPlayer();
        }

        if (removeType == RemoveType.Percent)
        {
            unit.attackDamage -= (int)(unit.attackDamage * (_percent / 100));
        }
        else
        {
            unit.attackDamage -= _value;
        }
    }
}
