using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RuneDial : Dial<BaseRuneUI, BaseRune>
{
    protected override bool _isAttackCondition => BattleManager.Instance.GameTurn == GameTurn.Player;

    private Resonance _resonance;

    protected override void Awake()
    {
        base.Awake();
        _resonance = GetComponent<Resonance>();
    }

    public override void SettingDialRune(bool isReset)
    {
        #region Clear
        foreach (var runeList in _elementDict)
        {
            for (int i = 0; i < runeList.Value.Count; i++)
            {
                Managers.Resource.Destroy(runeList.Value[i].gameObject);
            }
        }
        _elementDict.Clear();

        for (int i = _usingDeck.Count - 1; i >= 0; i--)
        {
            _remainingDeck.Add(_usingDeck[i]);
            _usingDeck.RemoveAt(i);
        }
        _usingDeck.Clear();

        for (int i = _cooltimeDeck.Count - 1; i >= 0; i--)
        {
            if (_cooltimeDeck[i].IsCoolTime == false)
            {
                _remainingDeck.Add(_cooltimeDeck[i]);
                _cooltimeDeck.RemoveAt(i);
            }
        }

        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].ResetRuneList();
            _dialElementList[i].SelectElement = null;
        }

        if (isReset == true)
        {
            for (int i = 0; i < Managers.Deck.Deck.Count; i++)
            {
                Managers.Deck.Deck[i].SetCoolTime(0);
            }
            _remainingDeck.Clear();
            _remainingDeck = new List<BaseRune>(Managers.Deck.Deck);
            _cooltimeDeck.Clear();
        }
        #endregion

        #region Setting
        for (int i = 0; i < _maxCount * 3; i++)
        {
            if (_remainingDeck.Count <= 0)
            {
                break;
            }

            int runeIndex = Random.Range(0, _remainingDeck.Count);

            int index = 2 - (i % 3);
            BaseRune rune = _remainingDeck[runeIndex];
            BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune").GetComponent<BaseRuneUI>();
            r.SetRune(rune);
            r.transform.SetParent(_dialElementList[index].transform);
            r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
            r.gameObject.SetActive(true);
            _dialElementList[index].AddRuneList(r);
            AddCard(r, 3 - index);
            _remainingDeck.Remove(rune);
            _usingDeck.Add(rune);
        }
        #endregion

        #region Copy
        for (int i = 1; i <= 3; i++)
        {
            if (_elementDict.ContainsKey(i))
            {
                int count = _elementDict[i].Count;
                for (int k = 0; k < _copyCount; k++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune", _dialElementList[3 - i].transform).GetComponent<BaseRuneUI>();
                        r.SetRune(_elementDict[i][j].Rune);
                        r.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                        _dialElementList[3 - i].AddRuneList(r);

                        AddCard(r, i);
                    }
                }
            }
        }
        #endregion

        RuneSort();
    }

    protected override IEnumerator AttackCoroutine()
    {
        AttributeType compareAttributeType = _dialElementList[0].SelectElement.Rune.BaseRuneSO.AttributeType;
        bool isResonanceCheck = true;
        for (int i = _dialElementList.Count - 1; i >= 0; i--)
        {
            if (_dialElementList[i].SelectElement != null)
            {
                int index = i;
                MagicCircleGlow(i, true);
                BaseRune rune = _usingDeck.Find(x => x == _dialElementList[i].SelectElement.Rune);
                _usingDeck.Remove(rune);
                rune.SetCoolTime();
                _cooltimeDeck.Add(rune);

                if (isResonanceCheck)
                    isResonanceCheck = _dialElementList[i].SelectElement.Rune.BaseRuneSO.AttributeType == compareAttributeType;

                (_dialElementList[i] as RuneDialElement).EffectHandler.Attack(3 - i, () =>
                {
                    _dialElementList[index].Attack();

                    if (i == _dialElementList.Count - 1)
                    {
                        (_dialElementList[i] as RuneDialElement).EffectHandler.Clear();
                    }
                });

                yield return new WaitForSeconds(0.1f);
            }
        }

        if (isResonanceCheck)
        {
            _resonance.Invocation(compareAttributeType);
        }
    }

    public void AllMagicSetCoolTime()
    {
        for (int i = 0; i < _cooltimeDeck.Count; i++)
        {
            if (_cooltimeDeck[i].IsCoolTime == true)
            {
                _cooltimeDeck[i].AddCoolTime(-1);
            }
        }
    }

    public void CheckResonance()
    {
        if (MagicEmpty(false))
        {
            _resonance.ActiveAllEffectObject(false);
        }
        else
        {
            AttributeType criterionType = _dialElementList[0].SelectElement.Rune.BaseRuneSO.AttributeType;
            bool isSame = true;

            for (int i = 1; i < _dialElementList.Count; i++)
            {
                isSame = criterionType == _dialElementList[i].SelectElement.Rune.BaseRuneSO.AttributeType;
                if (!isSame)
                    break;
            }

            if (isSame)
                _resonance.ResonanceEffect(criterionType);
            else
                _resonance.ActiveAllEffectObject(false);
        }
    }
}
