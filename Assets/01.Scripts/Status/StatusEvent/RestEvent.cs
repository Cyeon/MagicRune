using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestEvent : StatusEvent
{
    public override void Invoke()
    {
        StartCoroutine(TurnChange());
    }

    private IEnumerator TurnChange()
    {
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.TurnChange();
    }
}
