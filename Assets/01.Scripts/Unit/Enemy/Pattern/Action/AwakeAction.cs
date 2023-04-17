using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeAction : PatternAction
{
    [SerializeField] private Sprite _awakingIcon;

    public override void TakeAction()
    {
        BattleManager.Instance.Enemy.SpriteRenderer.sprite = _awakingIcon;
        base.TakeAction();
    }
}
