using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    private SpriteRenderer _sr;

    public enum ClickStatus
    {
        Default,
        Press,
    }

    [System.Serializable]
    public struct SpriteStruct
    {
        public ClickStatus Status;
        public Sprite Sprite;
    }

    [SerializeField]
    private List<SpriteStruct> _spriteList;

    private bool _isPressed = false;

    public bool IsPressed
    {
        get => _isPressed;
        set
        {
            if( _isPressed != value )
            {
                _isPressed = value;
                _sr.sprite = _spriteList.Find(x => _isPressed
                ? x.Status == ClickStatus.Press
                : x.Status == ClickStatus.Default).Sprite;
            }
        }
    }
}
