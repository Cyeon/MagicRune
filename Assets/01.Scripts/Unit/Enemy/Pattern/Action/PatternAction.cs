using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternAction : MonoBehaviour
{
    protected Enemy Enemy => BattleManager.Instance.Enemy;

    private Pattern _pattern;
    protected Pattern Pattern
    {
        get
        {
            if(_pattern == null)
            {
                _pattern = Enemy.PatternManager.CurrentPattern;
            }
            return _pattern;
        }
    }

    public virtual string Description => "";

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
