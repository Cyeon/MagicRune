using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PatternAction : MonoBehaviour
{
    protected Enemy Enemy => BattleManager.Instance.Enemy;

    public virtual void StartAction()
    {
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();
    }

    public virtual void TurnAction()
    {
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();
    }

    public virtual void EndAction()
    {
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();
    }
}
