using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : Enemy
{
    [SerializeField] private Sprite _originalIcon;

    public override void Init()
    {
        SpriteRenderer.sprite = _originalIcon;
        base.Init();
    }
}
