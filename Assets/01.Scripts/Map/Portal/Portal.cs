using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Portal : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float _effectingTime = 1f;
    private bool _isEffecting = false;
    
    private Vector3 _portalEffectScale = Vector3.one;
    protected SpriteRenderer _spriteRenderer;
    protected TextMeshPro _titleText;

    private List<Transform> _effecting = new List<Transform>();

    private void Awake()
    {
        _portalEffectScale = transform.Find("Effect").Find("Effect1").localScale;
        _spriteRenderer = transform.Find("Icon").GetComponent<SpriteRenderer>();
        _titleText = transform.Find("TitleText").GetComponent<TextMeshPro>();

        for(int i = 0; i < transform.Find("Effect").childCount; ++i)
        {
            _effecting.Add(transform.Find("Effect").GetChild(i));
        }

        PortalReset();
    }

    /// <summary>
    /// Æ÷Å»ÀÌ »ý¼ºµÉ ¶§
    /// </summary>
    public virtual void Init(Vector2 pos)
    {
        transform.position = pos;

        PortalEffectingSizeControl(_portalEffectScale, _effectingTime);
        transform.DOScale(1f, _effectingTime);
        StartCoroutine(Effecting());
    }

    /// <summary>
    /// Æ÷Å»À» ´­·¶À» ¶§
    /// </summary>
    public virtual void Execute()
    {
        Managers.Map.PortalSpawner.ResetPortal();
    }

    public void PortalEffectingSizeControl(Vector2 size, float time)
    {
        foreach(var effect in _effecting)
        {
            effect.DOScale(size, time);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int currentClickNum = eventData.clickCount;

        if (currentClickNum == 1)
        {
            if (_isEffecting) return;
            Managers.Map.PortalSpawner.SelectPortal(this);
            Execute();
        }
    }

    public IEnumerator Effecting()
    {
        _isEffecting = true;
        yield return new WaitForSeconds(_effectingTime);
        _isEffecting = false;
    }

    public Sprite GetSprite()
    {
        return _spriteRenderer.sprite;
    }

    public void PortalReset()
    {
        PortalEffectingSizeControl(Vector2.zero, 0);
        transform.localScale = Vector2.zero;
    }
}
