using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDebufferThreeToOneAction : StatusAction
{
    [Header("Condition 조건")]
    [SerializeField] private StatusName _checkStatusName = StatusName.ChillinessZip;

    public override void StartAction()
    {
        if(Enemy.StatusManager.GetStatus(_checkStatusName) != null)
        {
            value = Mathf.RoundToInt(Enemy.StatusManager.GetStatus(_checkStatusName).TypeValue * 0.3f) + 1;
            Enemy.PatternManager.CurrentPattern.desc = value.ToString();
            Enemy.PatternManager.UpdatePatternUI();
        }

        base.StartAction();
    }

    public override void TurnAction()
    {
        Enemy.StatusManager.DeleteStatus(_checkStatusName);
        base.TurnAction();
    }
}
