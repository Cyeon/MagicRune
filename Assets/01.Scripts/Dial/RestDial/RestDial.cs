using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RestDial : MonoBehaviour
{
    #region Rotate Parameta
    [SerializeField]
    private int _maxRuneCount = 3;
    [SerializeField, Range(90f, 360f)]
    private float _runeAngle = 180f;
    public float RuneAngle => _runeAngle;
    [SerializeField, Range(0f, 360f)]
    private float _startAngle = 180;
    public float StartAngle => _startAngle;
    #endregion

    #region Rune Parameta
    [SerializeField]
    private int _copyCount = 2;

    [SerializeField]
    private float[] _lineDistanceArray = new float[3];
    public float[] LineDistanceArray => _lineDistanceArray;
    #endregion

    #region Rune Container
    private Dictionary<int, List<RestRuneUI>> _runeDict;
    private List<RestDialElement> _dialElementList;
    public List<RestDialElement> DialElementList => _dialElementList;

    #endregion

    [SerializeField]
    private Sprite[] _restSpriteArray = new Sprite[3];

    private bool _isAttack;

    private RestUI _restUI;

    [SerializeField]
    private TextMeshProUGUI _descText;

    private void Awake()
    {
        #region Initialization 

        _runeDict = new Dictionary<int, List<RestRuneUI>>(3);
        for (int i = 1; i <= _lineDistanceArray.Length; i++)
        {
            _runeDict.Add(i, new List<RestRuneUI>());
        }
        _dialElementList = new List<RestDialElement>();
        #endregion

        _isAttack = false;
    }

    private void Start()
    {
        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();

        for (int i = 0; i < _lineDistanceArray.Length; i++)
        {
            RestDialElement d = this.transform.GetChild(i).GetComponent<RestDialElement>();
            d.SetLineID(_lineDistanceArray.Length - i);

            _dialElementList.Add(d);
        }

        RestRuneUI enhanceRune1 = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
        enhanceRune1.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        enhanceRune1.SetInfo(_restSpriteArray[0], () =>
        {
            // 깅회 1
            // 맞는 UI 띄워ㅓ주기
            PopupText text = Managers.Resource.Instantiate("UI/PopupText").GetComponent<PopupText>();
            text.SetText("개발 중인 기능입니다.");
            Debug.Log("강화1");
        }, "같은 등급의\n다른 룬으로 바꾼다.");
        _dialElementList[0].AddRuneList(enhanceRune1);
        AddCard(enhanceRune1, 3);

        RestRuneUI restRune = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
        restRune.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        restRune.SetInfo(_restSpriteArray[1], () =>
        {
            // 회복
            StartCoroutine(RestActionCoroutine());
        }, "최대 체력의\n25%를 회복한다.");
        _dialElementList[0].AddRuneList(restRune);
        AddCard(restRune, 3);

        RestRuneUI enhanceRune2 = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
        enhanceRune2.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        enhanceRune2.SetInfo(_restSpriteArray[2], () =>
        {
            // 깅회 2
            // 맞는 UI 띄워ㅓ주기
            Debug.Log("강화2");
            PopupText text = Managers.Resource.Instantiate("UI/PopupText").GetComponent<PopupText>();
            text.SetText("개발 중인 기능입니다.");
        }, "여러개의 룬을 바쳐\n더 높은 등급의\n룬을 얻는다.");
        _dialElementList[0].AddRuneList(enhanceRune2);
        AddCard(enhanceRune2, 3);

        #region COPY
        for(int i = 0; i < _restSpriteArray.Length; i++)
        {
            RestRuneUI rune = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
            rune.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            rune.SetInfo(_runeDict[3][i].GetSprite(), _runeDict[3][i].ClickAction(), _runeDict[3][i].Desc);
            _dialElementList[0].AddRuneList(rune);
            AddCard(rune, 3);
        }
        #endregion

        RuneSort();
    }

    public void EditText(string text)
    {
        _descText.SetText(text);
    }

    private IEnumerator RestActionCoroutine()
    {
        GameObject effect = Managers.Resource.Instantiate("Effects/HealthParticle", Managers.Canvas.GetCanvas("Rest").transform);
        Managers.GetPlayer().AddHPPercent(25);
        yield return new WaitForSeconds(1.5f);
        _restUI.NextStage();
    }

    public void AddCard(RestRuneUI card, int tier)
    {
        if (card != null)
        {
            if (_runeDict.ContainsKey(tier))
            {
                _runeDict[tier].Add(card);
            }
            else
            {
                _runeDict.Add(tier, new List<RestRuneUI> { card });
            }

            RuneSort();
        }
    }

    private void RuneSort()
    {
        for (int i = 1; i <= 3; i++)
        {
            LineCardSort(i);
        }
    }

    public void LineCardSort(int line)
    {
        if (_runeDict.ContainsKey(line))
        {
            float angle = -1 * _runeAngle / _runeDict[line].Count * Mathf.Deg2Rad;

            for (int i = 0; i < _runeDict[line].Count; i++)
            {
                float radianValue = angle * i + (_startAngle * Mathf.Deg2Rad);

                float height = Mathf.Sin(radianValue) * _lineDistanceArray[3 - line];
                float width = Mathf.Cos(radianValue) * _lineDistanceArray[3 - line];
                _runeDict[line][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

                Vector2 direction = new Vector2(
                    _runeDict[line][i].transform.position.x - transform.position.x,
                    _runeDict[line][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _runeDict[line][i].transform.rotation = angleAxis;
            }
        }
    }

    public void Clear()
    {
        for(int i = 0; i < _runeDict[3].Count; i++)
        {
            Managers.Resource.Destroy(_runeDict[3][i].gameObject);
        }
        _dialElementList[0].SelectCard = null;
        _dialElementList[0].Clear();
        _runeDict.Clear();
    }

    public void ResetDial()
    {
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].transform.eulerAngles = Vector3.zero;
        }
    }

    public void PanelUI()
    {

    }

    public void AllMagicActive(bool value)
    {
        for (int i = 1; i <= 3; i++)
        {
            if (_runeDict.ContainsKey(i))
            {
                for (int j = 0; j < _runeDict[i].Count; j++)
                {
                    _runeDict[3][j].gameObject.SetActive(false);
                }
            }
        }
    }

    public void MagicCircleGlow(int index, bool value)
    {
        _dialElementList[index].IsGlow = value;
    }

    public bool MagicEmpty()
    {
        return _dialElementList[0].SelectCard == null && _dialElementList[1].SelectCard == null && _dialElementList[2].SelectCard == null;
    }

    public void Attack()
    {
        if (_dialElementList[0].SelectCard == null) return;

        if (_isAttack == true) return;
        _isAttack = true;

        Define.DialScene?.CardDescDown();

        _dialElementList[0].SelectCard.ClickAction()?.Invoke();
        _isAttack = false;
    }

    public void AllMagicCircleGlow(bool value)
    {
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].IsGlow = value;
        }
    }

    //public void AllMagicSetCoolTime()
    //{
    //    Debug.Log("쿨타임 감소");

    //    for (int i = 0; i < _cooltimeDeck.Count; i++)
    //    {
    //        if (_cooltimeDeck[i].IsCoolTime == true)
    //        {
    //            _cooltimeDeck[i].AddCoolTime(-1);
    //        }
    //    }
    //}
}
