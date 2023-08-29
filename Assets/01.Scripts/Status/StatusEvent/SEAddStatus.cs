using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAddStatus : StatusEvent
{
    private enum AddStackType
    {
        Int,
        Dmg,
        Stack
    }

    [SerializeField] private StatusName _statusName = StatusName.Null;
    [SerializeField] private AddStackType _addStackType = AddStackType.Int;
    [SerializeField, ConditionalField(nameof(_addStackType), false, AddStackType.Int)] 
    private int _value = 0;
    [SerializeField] private bool _isSelf = false;

    public override void Invoke()
    {
        base.Invoke();

        Unit unit = _unit;
        if(!_isSelf)
        {
            if (_unit == Managers.GetPlayer()) unit = BattleManager.Instance.Enemy;
            else unit = Managers.GetPlayer();
        }

        if(_unit.currentDmg > 0)
        {
            switch(_addStackType)
            {
                case AddStackType.Dmg:
                    _value = Mathf.FloorToInt(_unit.currentDmg);
                    unit.StatusManager.AddStatus(_statusName, _value);
                    break;

                case AddStackType.Int:
                    unit.StatusManager.AddStatus(_statusName, _value);
                    break;

                case AddStackType.Stack:
                    unit.StatusManager.AddStatus(_statusName, _status.TypeValue);
                    break;
            }

        }
    }
}
