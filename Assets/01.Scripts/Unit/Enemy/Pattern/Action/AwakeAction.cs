using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAction : PatternAction
{
    [SerializeField] private Sprite _awakingIcon;

    public override void TurnAction()
    {
        BattleManager.Instance.Enemy.spriteRenderer.sprite = _awakingIcon;
        base.TurnAction();
    }
}
