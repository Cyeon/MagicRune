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
    private RectTransform rectTransform = null;
    private RectTransform canvasTransform = null;

    private void Awake()
    {
        gameObject.name = "FollowObject";
        rectTransform = GetComponent<RectTransform>();
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
        rectTransform.anchoredPosition = GetCanvasPosition(Input.mousePosition);
    }

    public void SetCanvasTrasform(RectTransform transform)
    {
        canvasTransform = transform;
    }

    private Vector2 GetCanvasPosition(Vector2 mousePosition)
    {
        Vector2 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
        return new Vector2((viewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f),
            (viewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f));
    }

    public void SetImage(Sprite sprite)
    {
        _runeImage.sprite = sprite;
    }
}