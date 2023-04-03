using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : Enemy
{
    [SerializeField] private Sprite _awakingIcon;

    public void Awaking()
    {
        if(HP <= MaxHealth / 2)
        {
            SpriteRenderer.sprite = _awakingIcon;
        }
        patternM.CurrentPattern.NextPattern();
    }
}
