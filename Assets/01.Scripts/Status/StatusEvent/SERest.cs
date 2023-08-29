using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SERest : StatusEvent
{
    public override void Invoke()
    {
        base.Invoke();

        _unit.isTurnSkip = true;
        if(_status.activeSound != null)
            Managers.Sound.PlaySound(_status.activeSound, SoundType.Effect);
        StartCoroutine(TurnChange());
    }

    private IEnumerator TurnChange()
    {
        yield return new WaitForSeconds(0.5f);
        BattleManager.Instance.TurnChange();
    }
}
