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

    private void Awake()
    {
        gameObject.name = "FollowObject";
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
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetImage(Sprite sprite)
    {
        _runeImage.sprite = sprite;
    }
}