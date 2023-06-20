using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneWeiget : MonoBehaviour
{
    private Button _btn;
    private GameObject _emphasis;
    private bool _isEmphasis = false;
    private RuneWeigetPanel _parent;

    [SerializeField]
    private AttributeType _attributeType;
    public AttributeType AttributeType => _attributeType;

    public bool IsEmphasis
    {
        get => _isEmphasis;
        set
        {
            _isEmphasis = value;
            _emphasis.gameObject.SetActive(value);
        }
    }

    void Start()
    {
        _btn = GetComponent<Button>();
        _parent = GetComponentInParent<RuneWeigetPanel>();
        _emphasis = transform.Find("Emphasis").gameObject;

        _btn.onClick.AddListener(() => _parent.Select = this);
    }
}
