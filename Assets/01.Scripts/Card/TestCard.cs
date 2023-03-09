using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestCard : MonoBehaviour
{
    public Dial Dial;
    private DialElement _dialElement;

    private CardSO _magic;
    public CardSO Magic => _magic;

    #region UI
    private Transform _magicArea;
    private Image _magicImage;
    private GameObject _outline;
    #endregion

    private int _coolTime;
    public bool IsCoolTime => _coolTime > 0;

    private void Awake()
    {
        _dialElement = GetComponentInParent<DialElement>();
        _magicArea = transform.Find("MagicArea");
        _magicImage = _magicArea.Find("MagicImage").GetComponent<Image>();
        _outline = _magicArea.Find("Outline").gameObject;
    }

    public void SetActiveOutline(bool value)
    {
        _outline.SetActive(value);
    }

    public void SetMagic(CardSO magic)
    {
        _magic = magic;
    }

    public void UpdateUI()
    {
        _magicImage.sprite = _magic.RuneImage;
    }

    public void SetCoolTime()
    {
        _coolTime = _magic.MainRune.CoolTime;
        _magicImage.color = Color.gray;
    }

    public void SetCoolTime(int value)
    {
        _coolTime = value;

        if(_coolTime > 0)
        {
            _magicImage.color = Color.gray;
        }
        else
        {
            _magicImage.color = Color.white;
        }
    }

    public int GetCoolTime()
    {
        return _coolTime;
    }
}