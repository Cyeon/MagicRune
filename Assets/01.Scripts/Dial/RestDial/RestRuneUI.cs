using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestRuneUI : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Sprite _sprite;
    private Action _action;

    private void Start()
    {
        TryGetComponent<SpriteRenderer>(out _spriteRenderer);
    }

    public void SetInfo(Sprite sprite, Action action)
    {
        _sprite = sprite;
        _action = action;

        if(_spriteRenderer == null)
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);

        _spriteRenderer.sprite = _sprite;
    }

    public Action ClickAction()
    {
        return _action;
    }
}
