using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestRuneUI : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Sprite _sprite;
    private Action _action;

    [TextArea]
    private string _desc;
    public string Desc => _desc;

    private void Start()
    {
        TryGetComponent<SpriteRenderer>(out _spriteRenderer);

        RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
    }

    public void SetInfo(Sprite sprite, Action action, string desc)
    {
        _sprite = sprite;
        _action = action;
        _desc = desc;

        if(_spriteRenderer == null)
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);

        _spriteRenderer.sprite = _sprite;
    }

    public Sprite GetSprite()
    {
        return _sprite;
    }

    public Action ClickAction()
    {
        return _action;
    }

    public void RuneColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
