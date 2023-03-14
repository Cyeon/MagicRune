using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MyBox;

[System.Serializable]
public class PRS
{
    public Vector3 Pos;
    public Quaternion Rot;
    public Vector3 Scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        Pos = pos;
        Rot = rot;
        Scale = scale;
    }
}

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
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

    private PRS _originPRS;
    public PRS OriginPRS
    {
        get => _originPRS;
        set => _originPRS = value;
    }

    private int _sortingIndex;
    public int SortingIndex => _sortingIndex;

    private CardCollector _collector;
    private MagicCircle _magicCircle;
    private bool _isRest = false;
    public bool IsRest {  get => _isRest; set => _isRest = value; }
    private bool _isClick;
    private float _clickTimer = 0f;
    private int _fingerId;

    //[System.Obsolete]
    //private int _coolTime;
    //[System.Obsolete]
    //public int CoolTime
    //{
    //    get
    //    {
    //        return _coolTime;
    //    }
    //    set
    //    {
    //        _coolTime = value;
    //        if (_coolTime <= 0)
    //        {
    //            _isRest = false;
    //        }
    //        else
    //        {
    //            _isRest = true;
    //        }
    //    }
    //}

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
    private TMP_Text _mainSubText;
    private Text _skillText;
    private TMP_Text _nameText;
    private Text _assistRuneCount;
    private Image _descriptionImage;
    private TMP_Text _descText;
    private UIOutline _outlineEffect;
    public UIOutline OutlineEffect => _outlineEffect;
    // Rune Area
    private Transform _runeAreaParent;
    public Transform RuneAreaParent => _runeAreaParent;
    private Image _runeImage;
    private Image _runeOutlineImage;

    // Keyword
    private Transform _keywardParent;
    public Transform KeyWardParent => _keywardParent;
    #endregion

    private AssistRune _assistRune;
    public AssistRune AssistRune => _assistRune;
    private RectTransform _rect;

    //private bool _isClick;

    private void Start()
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

        if(_rune != null)
        {
            //    GameObject assert = Instantiate(UIManager.Instance.cardAssistPanel);

            //    assert.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = string.Format("[보조] {0}", rune.AssistRune.Name);
            //    assert.transform.Find("Mana").GetComponent<TMP_Text>().text = rune.AssistRune.Cost.ToString();
            //    assert.transform.Find("Information").GetComponent<TextMeshProUGUI>().text = rune.AssistRune.CardDescription;
            //    assert.transform.SetParent(_keywardParent);
            //    assert.transform.localScale = Vector3.one;

            if (_keywardParent == null) Setting();
            foreach (var keyword in Rune.keywordList)
            {
                GameObject panel = UIManager.Instance.word.KeywordInit(keyword);
                panel.transform.SetParent(_keywardParent);
                panel.transform.localScale = new Vector3(0.4f, 0.8f, 1f);
            }
            _keywardParent.gameObject.SetActive(false);
        }

        if (_rune != null)
        {
            UpdateUI(_isFront);
        }
    }

    public void MoveTransform(PRS prs, bool useDotween = false, float dotweenTime = 0f)
    {
        if (useDotween)
        {
            _rect.DOAnchorPos(prs.Pos, dotweenTime);
            _rect.DORotateQuaternion(prs.Rot, dotweenTime);
            _rect.DOScale(prs.Scale, dotweenTime);
        }
        else
        {
            _rect.anchoredPosition = prs.Pos;
            _rect.rotation = prs.Rot;
            _rect.localScale = prs.Scale;
        }
    }

    public void UpdateUI(bool isFront)
    {
        if (!_nameText || !_skillImage || !_costText || !_runeImage || !_descText || !_mainSubText) { Setting(); } // || !_coolTimeText || !_mainSubText || !_skillText || !_runeImage) { Setting(); }
        if (isFront == true)
        {
            _nameText.text = _rune.MainRune.Name;
            _skillImage.sprite = _rune.MainRune.CardImage;
            _costText.text = _rune.MainRune.Cost.ToString();
            _mainSubText.text = "메인 룬";
            _descText.text = _rune.MainRune.CardDescription;
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
            _mainSubText.text = "보조 룬";
            _descText.text = _rune.AssistRune.CardDescription;
            //_coolTimeText.text = _rune.AssistRune.DelayTurn.ToString();
            //_mainSubText.text = "보조";
            //_skillText.text = _rune.AssistRune.CardDescription;
            //_assistRuneCount.text = "0";
        }
        _cardBase.sprite = _rune.CardBackground;
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

    //[System.Obsolete]
    //public void SetCoolTime(int coolTime)
    //{
    //    _coolTime = coolTime;
    //}

    public void SetRune(bool value)
    {
        if (_runeAreaParent == null || _cardAreaParent == null) { Setting(); }
        if (value == true)
        {
            _runeAreaParent.gameObject.SetActive(true);
            _cardAreaParent.gameObject.SetActive(false);
            _assistRune.gameObject.SetActive(false);
            _keywardParent.gameObject.SetActive(false);
        }
        else
        {
            _runeAreaParent.gameObject.SetActive(false);
            _cardAreaParent.gameObject.SetActive(true);
            _keywardParent.gameObject.SetActive(true);
        }
    }

    public void SetOutlineColor(Color color)
    {
        //_cardBase.material?.SetColor("_SolidOutline", color);
        //_outlineEffect.color
    }

    public void SetOutline(bool value)
    {
        _outlineEffect.gameObject.SetActive(value);
    }

    public void SetRuneOutlineActive(bool value)
    {
        _runeOutlineImage.gameObject.SetActive(value);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isEquipMagicCircle == false)
        {
            _collector.CardSelect(this);
            if(_collector.SelectCard != null)
            {
                _collector.SelectCard.transform.localScale = new Vector3(2f, 2f, 1f);
                _collector.SelectCard.transform.rotation = Quaternion.identity;
            }
            _collector.FingerID = eventData.pointerId;
            _fingerId = eventData.pointerId;
            transform.DOKill();
            _keywardParent.gameObject.SetActive(true);
            if(_isFront == true)
            {
                _assistRune.gameObject.SetActive(true);
            }
            else
            {
                _assistRune.gameObject.SetActive(false);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_collector.SelectCard != null && _isEquipMagicCircle == false)
        {
            _collector.SelectCard.transform.localScale = Vector3.one;
            _collector.SelectCard.transform.rotation = _originPRS.Rot;
            _collector.FingerID = -1;
            _collector.CardSelect(null);
            _fingerId = -1;
            transform.DOKill();
            _keywardParent.gameObject.SetActive(false);
            _assistRune.gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        float x = 300;
        if(transform.position.x >= 0)
        {
            if(_keywardParent.transform.position.x != -x)
                _keywardParent.transform.DOLocalMoveX(-x, 0);
        }
        else
        {
            if (_keywardParent.transform.position.x != x)
                _keywardParent.transform.DOLocalMoveX(x, 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //transform.DOKill();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isClick = true;
        _fingerId = eventData.pointerId;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isClick = false;
        if (_collector.SelectCard != null && _isEquipMagicCircle == false)
        {
            _collector.SelectCard.transform.localScale = Vector3.one;
            _collector.SelectCard.transform.rotation = _originPRS.Rot;
            _collector.FingerID = -1;
            _collector.CardSelect(null);
            _fingerId = -1;
            transform.DOKill();
            _keywardParent.gameObject.SetActive(false);
            _assistRune.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(_isClick == true)
        {
            _clickTimer += Time.deltaTime;
            if(_clickTimer >= 0.5f)
            {
                if (_isEquipMagicCircle == false)
                {
                    _collector.CardSelect(this);
                    if (_collector.SelectCard != null)
                    {
                        _collector.SelectCard.transform.localScale = new Vector3(2f, 2f, 1f);
                        _collector.SelectCard.transform.rotation = Quaternion.identity;
                    }
                    _collector.FingerID = _fingerId;
                    transform.DOKill();
                    _keywardParent.gameObject.SetActive(true);
                    if (_isFront == true)
                    {
                        _assistRune.gameObject.SetActive(true);
                    }
                    else
                    {
                        _assistRune.gameObject.SetActive(false);
                    }
                    _isClick = false;
                }
            }
        }
    }

    private void Setting()
    {
        _collector = BattleManager.Instance.MagicCircle.CardCollector;
        _rect = GetComponent<RectTransform>();
        _magicCircle = BattleManager.Instance.MagicCircle;

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
        _outlineEffect = _cardAreaParent.Find("Base_Image/Outline").GetComponent<UIOutline>();

        _nameText = _cardAreaParent.Find("Name_Text").GetComponent<TMP_Text>();
        _skillImage = _cardAreaParent.Find("Skill_Image").GetComponent<Image>();
        _costText = _cardAreaParent.Find("Cost_Text").GetComponent<TMP_Text>();
        _mainSubText = _cardAreaParent.Find("MainSub_Text").GetComponent<TMP_Text>();

        _descText = _cardAreaParent.Find("Desc_Text").GetComponent<TMP_Text>();
        //_descriptionImage = _cardAreaParent.Find("Description_Image").GetComponent<Image>();

        _runeAreaParent = transform.Find("Rune_Area");
        _runeImage = _runeAreaParent.Find("Rune_Image").GetComponent<Image>();
        _runeOutlineImage = _runeAreaParent.Find("Rune_Line_Image").GetComponent<Image>();

        _keywardParent = transform.Find("Keyword");

        _assistRune = transform.Find("Assist").GetComponent<AssistRune>();
        if(_rune != null)
            _assistRune.UpdateUI(_rune.AssistRune);
        _assistRune.gameObject.SetActive(false);

        IsFront = true;

        if (_rune != null)
        {
            _runeImage.sprite = _rune.RuneImage;
            _runeAreaParent.gameObject.SetActive(false);
        }
        SetRuneOutlineActive(false);

        SetOutlineColor(Color.cyan);
        SetOutline(true);
    }

    public void OnDestroy()
    {
        transform.DOKill();
    }
}
