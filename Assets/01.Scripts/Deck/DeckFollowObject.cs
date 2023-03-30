using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 따라다니는 오브젝트에 붙어있는 스크립트
/// </summary>
public class DeckFollowObject : MonoBehaviour
{
    private Image _runeImage = null;
    private RectTransform _rectTransform = null;
    private RectTransform _canvasTransform = null;

    private void Awake()
    {
        gameObject.name = "FollowObject";
        _rectTransform = GetComponent<RectTransform>();
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image item in images)
        {
            item.raycastTarget = false;
            if (item.name == "RuneImage")
            {
                _runeImage = item;
                break;
            }
        }
    }
    private void Update()
    {
        FollowMouse();
    }

    public void SetCanvasTrasform(RectTransform transform)
    {
        _canvasTransform = transform;
    }

    public void FollowMouse()
    {
        _rectTransform.anchoredPosition = GetCanvasPosition(Input.mousePosition);
    }

    private Vector2 GetCanvasPosition(Vector2 mousePosition)
    {
        Vector2 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
        return new Vector2((viewportPosition.x * _canvasTransform.sizeDelta.x) - (_canvasTransform.sizeDelta.x * 0.5f),
            (viewportPosition.y * _canvasTransform.sizeDelta.y) - (_canvasTransform.sizeDelta.y * 0.5f));
    }

    public void SetImage(Sprite sprite)
    {
        _runeImage.sprite = sprite;
    }
}