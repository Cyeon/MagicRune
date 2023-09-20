using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAddStatusOnStackOverflow : StatusEvent
{
    [Header("Condition 조건")]
    [SerializeField]
    private StatusName _conditionStatus = StatusName.Null;
    [SerializeField]
    public int _conditionCount;
    [SerializeField]
    public bool isConditionStatusRem = true;

    [Header("Result 결과")]
    [SerializeField]
    private StatusName _addStatus = StatusName.Null;

    public override void Invoke()
    {
        base.Invoke();

        if (_unit.StatusManager.GetStatus(_conditionStatus)?.TypeValue >= _conditionCount)
        {
            if(isConditionStatusRem == true) _unit.StatusManager.RemoveStatus(_conditionStatus, _conditionCount);
            _unit.StatusManager.AddStatus(_addStatus, 1);

            if(BattleManager.Instance.GameTurn == GameTurn.Enemy)
            {
                _unit.StatusManager.GetStatus(_addStatus).isFirst = true;
            }
        }
    }
}
