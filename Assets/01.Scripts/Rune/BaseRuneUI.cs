using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRuneUI : MonoBehaviour
{
    private SpriteRenderer _runeSpriteRenderer;

    private BaseRune _rune;
    public BaseRune Rune => _rune;

    void Start()
    {
        _runeSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
    }

    public void SetRune(BaseRune rune)
    {
        _rune = rune;

        if (_runeSpriteRenderer == null)
        {
            _runeSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        }

        _runeSpriteRenderer.sprite = _rune.BaseRuneSO.RuneSprite;
    }

    public void RuneColor(Color color)
    {
        _runeSpriteRenderer.color = color;
    }
}
