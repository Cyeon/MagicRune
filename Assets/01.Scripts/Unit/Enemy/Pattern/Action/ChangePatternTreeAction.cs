using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePatternTreeAction : PatternAction
{
    public string treeName;
    [SerializeField] private Sprite _changeSprite;

    public override void TurnAction()
    {
        if(_changeSprite != null)
            Enemy.spriteRenderer.sprite = _changeSprite;
        Enemy.PatternManager.ChangeTree(treeName);
        base.TurnAction();
    }
}
