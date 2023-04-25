using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAction : PatternAction
{
    [SerializeField] private Sprite _awakingIcon;

    public override void StartAction()
    {
        BattleManager.Instance.Enemy.spriteRenderer.sprite = _awakingIcon;
        BattleManager.Instance.Enemy.PatternManager.ChangeTree("Awaking");
        base.StartAction();
    }
}
