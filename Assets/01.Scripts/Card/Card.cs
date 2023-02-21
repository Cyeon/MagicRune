using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MyBox;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject cardPrefab = null;
    public GameObject CardPrefab => cardPrefab;

    [SerializeField]
    private CardSO _rune;
    [SerializeField]
    private RuneDesc _descPrefab;

    [SerializeField]
    private bool _isEquipMagicCircle = false;
    public bool IsEquipMagicCircle { get => _isEquipMagicCircle; set => _isEquipMagicCircle = value; }
    public CardSO Rune => _rune;

    private int _sortingIndex;
    public int SortingIndex => _sortingIndex;

    private CardCollector _collector;
    private MagicCircle _magicCircle;
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
            UpdateUI(_isFront);
        }
    }
    #region Card UI
    // Card Area
    private Transform _cardAreaParent;
    public Transform CardAreaParent => _cardAreaParent;
    private Transform _cardParent;
    private Image _cardBase;
    private Image _skillImage;
    private TMP_Text _costText;
    private Text _coolTimeText;
    private Text _mainSubText;
    private Text _skillText;
    private TMP_Text _nameText;
    private Text _assistRuneCount;
    private Image _descriptionImage;

    // Rune Area
    private Transform _runeAreaParent;
    public Transform RuneAreaParent => _runeAreaParent;
    private Image _runeImage;
    private Image _runeOutlineImage;
    #endregion

    private RectTransform _rect;
    [SerializeField]
    private Material _outlineMaterial;
    private Material _defaultMaterial;

    private bool _isClick;

    private void Awake()
    {
        Setting();
    }

    public void SetSortingIndex(int index)
    {
        _sortingIndex = index;
    }

    public void SetRune(CardSO rune)
    {
        _rune = rune;

        if (_rune != null)
        {
            UpdateUI(_isFront);
        }
    }

    public void UpdateUI(bool isFront)
    {
        if (!_nameText || !_skillImage || !_costText || !_runeImage || !_descriptionImage) { Setting(); } // || !_coolTimeText || !_mainSubText || !_skillText || !_runeImage) { Setting(); }
        if (isFront == true)
        {
            _nameText.text = _rune.MainRune.Name;
            _skillImage.sprite = _rune.MainRune.CardImage;
            _costText.text = _rune.MainRune.Cost.ToString();
            //_coolTimeText.text = _rune.MainRune.DelayTurn.ToString();
            //_mainSubText.text = "메인";
            //_skillText.text = _rune.MainRune.CardDescription;
            //_assistRuneCount.text = _rune.AssistRuneCount.ToString();

        }
        else
        {
            _nameText.text = _rune.AssistRune.Name;
            _skillImage.sprite = _rune.AssistRune.CardImage;
            _costText.text = _rune.AssistRune.Cost.ToString();
            //_coolTimeText.text = _rune.AssistRune.DelayTurn.ToString();
            //_mainSubText.text = "보조";
            //_skillText.text = _rune.AssistRune.CardDescription;
            //_assistRuneCount.text = "0";
        }
        _runeImage.sprite = _rune.RuneImage;
    }

    public void SetIsEquip(bool value)
    {
        _isEquipMagicCircle = value;

        if (_isEquipMagicCircle == true)
        {
            SetRune(true);
            //SetOutlineActive(true);
        }
        else
        {
            SetRune(false);
            //SetOutlineActive(false);
        }
    }

    public void SetCoolTime(int coolTime)
    {
        _coolTime = coolTime;
    }

    public void SetRune(bool value)
    {
        if (_runeAreaParent == null || _cardAreaParent == null) { Setting(); }
        if (value == true)
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

    public void SetOutlineColor(Color color)
    {
        _cardBase.material?.SetColor("_SolidOutline", color);
    }

    public void SetOutline(bool value)
    {
        if (value)
        {
            _cardBase.material = _outlineMaterial;
        }
        else
        {
            _cardBase.material = _defaultMaterial;
        }
    }

    public void SetOutlineActive(bool value)
    {
        _runeOutlineImage.gameObject.SetActive(value);
    }

    //private void Update()
    //{
    //    if (_isClick == true)
    //    {
    //        _clickTimer += Time.deltaTime;

    //        if (_clickTimer >= 0.5f)
    //        {
    //            _isClick = false;

    //            _collector.CardSelect(this);
    //            _collector.AllCardDescription(false);
    //        }
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_rune == null) return;

        if (_isEquipMagicCircle == false)
        {
            //_clickTimer += Time.deltaTime;

            //if(_clickTimer >= 0.5f)
            //{
            //    _collector.CardSelect(this);
            //    _clickTimer = 0f;
            //}

            _isClick = true;
            

            //transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_rune == null) return;

        if (_isEquipMagicCircle == false)
        {
            //if(_clickTimer < 0.5f)
            //{
            //    Debug.Log("섦여 띄우기");
            //}
            //else
            //{
            //    _collector.CardSelect(null);
            //}
            _isClick = false;

            //if (eventData.clickTime > 0.2f)
            //{
            //    _collector.AllCardDescription(false);
            //}
            //else
            //{
            //    if (_descriptionImage.color.a == 0)
            //    {
            //        _collector.AllCardDescription(false);
            //        SetDescription(true);
            //    }
            //    else
            //    {
            //        _collector.AllCardDescription(false);
            //    }
            //}
            //transform.localScale = Vector3.one;
        }
        else
        {
            // 나중에 수정

            //if (_rune == null) return;

            //RuneDesc go = Instantiate(_descPrefab, this.transform.parent).GetComponent<RuneDesc>();
            //Card mainCard = _collector.MagicCircle.RuneDict[RuneType.Main].Find(x => x == this);
            //if (mainCard != null)
            //{
            //    go.UpdateUI(_rune.MainRune);
            //}
            //else
            //{
            //    Card assistCard = _collector.MagicCircle.RuneDict[RuneType.Assist].Find(x => x == this);
            //    if (assistCard != null)
            //    {
            //        go.UpdateUI(_rune.AssistRune);
            //    }
            //    else
            //    {
            //        Destroy(go.gameObject);
            //    }
            //}

        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isEquipMagicCircle == false)
        {
            _collector.CardSelect(this);
            if(_collector.SelectCard != null)
            {
                _collector.SelectCard.transform.localScale = new Vector3(2f, 2f, 1f);
            }

            transform.DOKill();
            UIManager.Instance.CardDescDown();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_collector.SelectCard != null)
        {
            _collector.SelectCard.transform.localScale = Vector3.one;
            _collector.CardSelect(null);

            transform.DOKill();
            UIManager.Instance.CardDescDown();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOKill();
        UIManager.Instance.CardDescPopup(this);
    }

    private void Setting()
    {
        _collector = GameManager.Instance.MagicCircle.CardCollector;
        _rect = GetComponent<RectTransform>();
        _magicCircle = GameManager.Instance.MagicCircle;

        #region 예전 카드에서 세팅 필요했던 부분
        //_cardAreaParent = transform.Find("Card_Area");
        //_cardParent = _cardAreaParent.Find("Card_Add element");
        //_cardBase = _cardAreaParent.Find("Card_Basic/Card_Base").GetComponent<Image>();
        //_skillImage = _cardParent.Find("Skill_Image").GetComponent<Image>();
        //_costText = _cardParent.Find("Cost_Text").GetComponent<Text>();
        //_coolTimeText = _cardParent.Find("Cooltime_Text").GetComponent<Text>();
        //_mainSubText = _cardParent.Find("MainSub_Text").GetComponent<Text>();
        //_skillText = _cardParent.Find("Skill_Detail").GetComponent<Text>();
        //_nameText = _cardParent.Find("Skill_Name").GetComponent<Text>();
        //_assistRuneCount = _cardParent.Find("Rune_Count").GetComponent<Text>();
        //_descriptionImage = _cardParent.Find("Description").GetComponent<Image>();

        //_runeAreaParent = transform.Find("RuneArea");
        //_runeImage = _runeAreaParent.Find("Rune Image").GetComponent<Image>();
        #endregion

        _cardAreaParent = transform.Find("Card_Area");
        _cardBase = _cardAreaParent.Find("Base_Image/Card_Image").GetComponent<Image>();

        _nameText = _cardAreaParent.Find("Name_Text").GetComponent<TMP_Text>();
        _skillImage = _cardAreaParent.Find("Skill_Image").GetComponent<Image>();
        _costText = _cardAreaParent.Find("Cost_Text").GetComponent<TMP_Text>();

        _descriptionImage = _cardAreaParent.Find("Description_Image").GetComponent<Image>();

        _runeAreaParent = transform.Find("Rune_Area");
        _runeImage = _runeAreaParent.Find("Rune_Image").GetComponent<Image>();
        _runeOutlineImage = _runeAreaParent.Find("Rune_Line_Image").GetComponent<Image>();

        IsFront = true;
        if(_defaultMaterial == null)
        {
            _defaultMaterial = _cardBase.material;
        }

        if (_rune != null)
        {
            _runeImage.sprite = _rune.RuneImage;
            _runeAreaParent.gameObject.SetActive(false);
        }
        SetOutlineActive(false);

        SetOutlineColor(Color.cyan);
        SetOutline(true);
    }

    public void OnDestroy()
    {
        transform.DOKill();
    }

}
