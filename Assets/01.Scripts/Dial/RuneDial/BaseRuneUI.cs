using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRuneUI : MonoBehaviour
{
    private SpriteRenderer _runeSpriteRenderer;

    private BaseRune _rune;
    public BaseRune Rune => _rune;

    private SpriteRenderer _enhanceSr;

    void Start()
    {
        _runeSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _enhanceSr = transform.Find("Enhance").GetComponent<SpriteRenderer>();
        //_enhanceSr.gameObject.SetActive(false);
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
        if(_enhanceSr == null)
        {
            _enhanceSr = transform.Find("Enhance").GetComponent<SpriteRenderer>();
        }
        _enhanceSr.gameObject.SetActive(rune.IsEnhanced);
    }

    public void RuneColor(Color color)
    {
        _runeSpriteRenderer.color = color;
        _enhanceSr.color = color;
    }
}
