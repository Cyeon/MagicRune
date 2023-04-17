using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PatternAction : MonoBehaviour
{
    public virtual void TakeAction()
    {
        BattleManager.Instance.Enemy.PatternManager.CurrentPattern.NextAction();
    }
}
