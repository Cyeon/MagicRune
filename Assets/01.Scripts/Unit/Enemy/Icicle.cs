using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : Enemy
{
    [SerializeField] private Sprite _originalIcon;
    [SerializeField] private Sprite _awakingIcon;

    public void Awaking()
    {
        if(HP <= MaxHealth / 2)
        {
            SpriteRenderer.sprite = _awakingIcon;
        }
        PatternManager.CurrentPattern.NextPattern();
    }

    public override void Init()
    {
        SpriteRenderer.sprite = _originalIcon;
        base.Init();
    }
}
