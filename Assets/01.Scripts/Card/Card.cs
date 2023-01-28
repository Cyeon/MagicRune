using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject cardPrefab = null;
    public GameObject CardPrefab => cardPrefab;

    [SerializeField]
    private CardSO _rune;

    [SerializeField]
    private bool _isEquipMagicCircle = false;
    public CardSO Rune => _rune;

    private CardCollector _collector;
    private bool _isRest = false;
    public bool IsRest => _isRest;

    private int _coolTime;
    public int CoolTime
    {
        get
        {
            return _coolTime;
        }
        set
        {
            _coolTime = value;
            if (_coolTime <= 0)
            {
                _isRest = false;
            }
            else
            {
                _isRest = true;
            }
        }
    }

    private bool _isFront = true;
    public bool IsFront
    {
        get => _isFront;
        set
        {
            //if (_isFront == value) return;

            _isFront = value;

            if (_rune == null) return;
            if(_isFront == true)
            {
                _skillImage.sprite = _rune.MainRune.CardImage;
                _costText.text = _rune.MainRune.Cost.ToString();
                _coolTimeText.text = _rune.MainRune.DelayTurn.ToString();
                _mainSubText.text = "메인";
                _skillText.text = _rune.MainRune.CardDescription;
            }
            else
            {
                _skillImage.sprite = _rune.AssistRune.CardImage;
                _costText.text = _rune.AssistRune.Cost.ToString();
                _coolTimeText.text = _rune.AssistRune.DelayTurn.ToString();
                _mainSubText.text = "보조";
                _skillText.text = _rune.AssistRune.CardDescription;
            }
        }
    }
    #region Card UI
    // Card Area
    private Transform _cardAreaParent;
    public Transform CardAreaParent => _cardAreaParent;
    private Transform _cardParent;
    private Image _skillImage;
    private Text _costText;
    private Text _coolTimeText;
    private Text _mainSubText;
    private Text _skillText;

    // Rune Area
    private Transform _runeAreaParent;
    public Transform RuneAreaParent => _runeAreaParent;
    private Image _runeImage;
    #endregion

    private RectTransform _rect;

    protected virtual void Start()
    {
        _collector = GetComponentInParent<CardCollector>();
        _rect = GetComponent<RectTransform>();

        _cardAreaParent = transform.Find("Card_Area");
        _cardParent = _cardAreaParent.Find("Card_Add element");
        _skillImage = _cardParent.Find("Skill_Image").GetComponent<Image>();
        _costText = _cardParent.Find("Cost_Text").GetComponent<Text>();
        _coolTimeText = _cardParent.Find("Cooltime_Text").GetComponent<Text>();
        _mainSubText = _cardParent.Find("MainSub_Text").GetComponent<Text>();
        _skillText = _cardParent.Find("Skill_Detail").GetComponent<Text>();

        _runeAreaParent = transform.Find("RuneArea");
        _runeImage = _runeAreaParent.Find("Rune Image").GetComponent<Image>();
        IsFront = true;

        if(_rune != null)
        {
            _runeImage.sprite = _rune.RuneImage;
            _runeAreaParent.gameObject.SetActive(false);
        }
    }

    public void SetRune(CardSO rune)
    {
        _rune = rune;

        if (_rune != null)
        {
            _runeImage.sprite = _rune.RuneImage;
        }
    }

    public void SetIsEquip(bool value)
    {
        _isEquipMagicCircle = value;

        if(_isEquipMagicCircle == true)
        {
            _runeAreaParent.gameObject.SetActive(true);
            _cardAreaParent.gameObject.SetActive(false);
        }
        else
        {
            _runeAreaParent.gameObject.SetActive(false);
            _cardAreaParent.gameObject.SetActive(true);
        }
    }

    public void SetCoolTime(int coolTime)
    {
        _coolTime = coolTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isEquipMagicCircle == false)
        {
            _collector.CardSelect(this);

            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isEquipMagicCircle == false)
        {
            _collector.CardSelect(null);
            transform.localScale = Vector3.one;
        }
    }
}
