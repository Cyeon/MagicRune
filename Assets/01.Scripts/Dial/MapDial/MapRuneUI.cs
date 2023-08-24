using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRuneUI : MonoBehaviour
{
    private Action _action;
    private SpriteRenderer _iconImage;
    public SpriteRenderer IconImage => _iconImage;

    public Sprite defaultIcon;
    public Sprite selectIcon;

    private Sequence _selectSeq;

    private void OnEnable()
    {
        _iconImage = transform.Find("Icon").GetComponent<SpriteRenderer>();
    }

    public void SetInfo(Action action)
    {
        _action = action;
    }

    public Action ClickAction()
    {
        return _action;
    }
    
    public void Select()
    {
        _iconImage.sprite = selectIcon;

        _selectSeq = DOTween.Sequence();
        _selectSeq.Append(transform.DOScale(0.2f, 0.2f));
        _selectSeq.Append(transform.DOScale(0.17f, 0.1f));
    }

    public void UnSelect()
    {
        _iconImage.sprite = defaultIcon;
        _selectSeq.Kill();
        transform.localScale = Vector3.one * 0.14f;
    }
}
