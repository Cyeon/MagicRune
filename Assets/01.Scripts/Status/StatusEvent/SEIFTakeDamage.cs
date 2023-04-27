using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEIFTakeDamage : SETakeDamage
{
    [SerializeField] private GameTurn _invoekTurn = GameTurn.Player;

    public override void Invoke()
    {
        if(BattleManager.Instance.GameTurn == _invoekTurn)
            base.Invoke();
    }
}
