using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneWeiget : MonoBehaviour
{
    private Button _btn;
    private Image _runeImage;
    private bool _isSelect = false;
    private RuneWeigetPanel _parent;

    [SerializeField] private Sprite _onAttribute;
    [SerializeField] private Sprite _offAttribute;

    [SerializeField]
    private AttributeType _attributeType;
    public AttributeType AttributeType => _attributeType;

    public bool IsSelect
    {
        get => _isSelect;
        set
        {
            _isSelect = value;
            if(_runeImage)
            {
                if (_isSelect)
                    _runeImage.sprite = _onAttribute;
                else
                    _runeImage.sprite = _offAttribute;
            }
        }
    }

    void Start()
    {
        _btn = GetComponent<Button>();
        _parent = GetComponentInParent<RuneWeigetPanel>();
        _runeImage = transform.Find("Image").GetComponent<Image>();

        if(AttributeType == Managers.Rune.GetSelectAttribute())
        {
            IsSelect = true;
        }

        _btn.onClick.AddListener(() => _parent.Select = this);
    }
}
