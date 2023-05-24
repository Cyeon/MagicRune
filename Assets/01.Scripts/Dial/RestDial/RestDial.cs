using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RestDial : Dial<RestRuneUI, RestRuneUI>
{
    [SerializeField]
    private Sprite[] _restSpriteArray = new Sprite[3];

    private RestUI _restUI;

    [SerializeField]
    private TextMeshProUGUI _descText;

    protected override void Start()
    {
        base.Start();

        _restUI = Managers.Canvas.GetCanvas("Rest").GetComponent<RestUI>();
    }

    public override void SettingDialRune(bool isReset)
    {
        RestRuneUI restRune = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
        restRune.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        restRune.SetInfo(_restSpriteArray[1], () =>
        {
            // 회복
            StartCoroutine(RestActionCoroutine());
        }, "최대 체력의\n25%를 회복한다.");
        _dialElementList[0].AddRuneList(restRune);
        AddCard(restRune, 3);

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
        for (int i = 0; i < _restSpriteArray.Length; i++)
        {
            RestRuneUI rune = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
            rune.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            rune.SetInfo(_elementDict[3][i].GetSprite(), _elementDict[3][i].ClickAction(), _elementDict[3][i].Desc);
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
        Managers.Sound.PlaySound("SFX/Healing", SoundType.Effect);
        yield return new WaitForSeconds(1.5f);
        _restUI.NextStage();
    }

    public void Clear()
    {
        for(int i = 0; i < _elementDict[3].Count; i++)
        {
            Managers.Resource.Destroy(_elementDict[3][i].gameObject);
        }
        _dialElementList[0].SelectElement = null;
        _dialElementList.Clear();
        _elementDict.Clear();
    }

    public override void Attack()
    {
        if (_dialElementList[0].SelectElement == null) return;

        if (_isAttack == true) return;
        _isAttack = true;

        Define.DialScene?.CardDescDown();

        _dialElementList[0].SelectElement.ClickAction()?.Invoke();

        AllMagicCircleGlow(false);
        _isAttack = false;
    }
}
