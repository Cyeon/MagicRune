using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum OutlineType
{
    Default,
    Cyan,
    Red,
    COUNT
}

public class TestCard : MonoBehaviour
{
    public Dial Dial;
    private DialElement _dialElement;

    private CardSO _magic;
    public CardSO Magic => _magic;

    #region UI
    private Transform _magicArea;
    private SpriteRenderer _magicImage;
    //private GameObject _outline;
    private TextMeshPro _coolTimeText;
    #endregion

    [SerializeField]
    private Material[] _outlineMaterialArray;

    private int _coolTime;
    public bool IsCoolTime => _coolTime > 0;

    private void Awake()
    {
        _dialElement = GetComponentInParent<DialElement>();
        _magicArea = transform.Find("MagicArea");
        _magicImage = _magicArea.Find("MagicImage").GetComponent<SpriteRenderer>();
        //_outline = _magicArea.Find("Outline").gameObject;
        _coolTimeText = _magicArea.Find("CoolTimeText").GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        _coolTimeText.gameObject.SetActive(false);
    }

    public void SetActiveOutline(OutlineType type)
    {
        _magicImage.material = _outlineMaterialArray[(int)type];
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
        SetActiveOutline(OutlineType.Default);
        _coolTimeText.SetText(_coolTime.ToString());
        _coolTimeText.gameObject.SetActive(true);
    }

    public void SetCoolTime(int value)
    {
        _coolTime = value;

        if(_coolTime > 0)
        {
            _magicImage.color = Color.gray;
            SetActiveOutline(OutlineType.Default);
            _coolTimeText.SetText(_coolTime.ToString());
            _coolTimeText.gameObject.SetActive(true);
        }
        else
        {
            _magicImage.color = Color.white;
            _coolTimeText.gameObject.SetActive(false);
        }
    }

    public int GetCoolTime()
    {
        return _coolTime;
    }
}