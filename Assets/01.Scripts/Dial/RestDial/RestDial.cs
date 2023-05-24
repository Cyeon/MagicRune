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
            // ȸ��
            StartCoroutine(RestActionCoroutine());
        }, "�ִ� ü����\n25%�� ȸ���Ѵ�.");
        _dialElementList[0].AddRuneList(restRune);
        AddCard(restRune, 3);

        RestRuneUI enhanceRune1 = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
        enhanceRune1.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        enhanceRune1.SetInfo(_restSpriteArray[0], () =>
        {
            // ��ȸ 1
            // �´� UI ������ֱ�
            PopupText text = Managers.Resource.Instantiate("UI/PopupText").GetComponent<PopupText>();
            text.SetText("���� ���� ����Դϴ�.");
            Debug.Log("��ȭ1");
        }, "���� �����\n�ٸ� ������ �ٲ۴�.");
        _dialElementList[0].AddRuneList(enhanceRune1);
        AddCard(enhanceRune1, 3);

        RestRuneUI enhanceRune2 = Managers.Resource.Instantiate("Rune/" + typeof(RestRuneUI).Name, _dialElementList[0].transform).GetComponent<RestRuneUI>();
        enhanceRune2.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        enhanceRune2.SetInfo(_restSpriteArray[2], () =>
        {
            // ��ȸ 2
            // �´� UI ������ֱ�
            Debug.Log("��ȭ2");
            PopupText text = Managers.Resource.Instantiate("UI/PopupText").GetComponent<PopupText>();
            text.SetText("���� ���� ����Դϴ�.");
        }, "�������� ���� ����\n�� ���� �����\n���� ��´�.");
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
