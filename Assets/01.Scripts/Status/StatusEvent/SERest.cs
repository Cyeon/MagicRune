using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SERest : StatusEvent
{
    public override void Invoke()
    {
        _unit.isTurnSkip = true;
        StartCoroutine(TurnChange());
    }

    private IEnumerator TurnChange()
    {
        yield return new WaitForSeconds(0.5f);
        BattleManager.Instance.TurnChange();
    }
}
