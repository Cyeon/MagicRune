using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusByHealAction : PatternAction
{
    [SerializeField] private StatusName _status;
    [SerializeField] private bool _isRemoveStack = true;

    private int _value = 0;

    private void Start()
    {
        _value = Enemy.StatusManager.GetStatusValue(_status) / 2;
        Enemy.PatternManager.CurrentPattern.patternValue = _value.ToString();
        Enemy.PatternManager.UpdatePatternUI();
    }

    public override void TurnAction()
    {
        Enemy.AddHP(_value);
        if (_isRemoveStack)
            Enemy.StatusManager.DeleteStatus(_status);

        base.TurnAction();
    }
}
