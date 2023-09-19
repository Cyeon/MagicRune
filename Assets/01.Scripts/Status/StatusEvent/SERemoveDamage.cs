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
    [SerializeField] private bool _isAttack = false; // 데미지 감소가 공격할땐지, 공격을 받을땐지 체크

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
            if(_isAttack)
                unit.attackDamage -= (int)(unit.attackDamage * (_percent / 100));
            else
                unit.takeDamage -= (int)(unit.takeDamage * (_percent / 100));
        }
        else
        {
            if (_isAttack)
                unit.attackDamage -= _value;
            else
                unit.takeDamage -= _value;
        }
    }
}
