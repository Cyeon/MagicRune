using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RuneDial : Dial<BaseRuneUI, BaseRune>
{
    private List<BaseRune> _consumeDeck = new List<BaseRune>();
    public List<BaseRune> ConsumeDeck => _consumeDeck;
    protected override bool _isAttackCondition => BattleManager.Instance.GameTurn == GameTurn.Player;

    private Resonance _resonance;

    public Action OnDialAttack;

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
                if(runeList.Value[i] != null)
                {
                    runeList.Value[i].transform.DOKill();
                    Managers.Resource.Destroy(runeList.Value[i].gameObject);
                    runeList.Value[i] = null;
                }
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
        _isAttack = true;
        Managers.GetPlayer().PlayAnimation(Managers.GetPlayer().HashAttack);
        int outRuneIndex = -1;
        for(int i = 0; i < _dialElementList.Count; i++)
        {
            if (_dialElementList[i].SelectElement != null)
            {
                outRuneIndex = i;
                break;
            }
        }
        if (outRuneIndex == -1) yield return null;
        AttributeType compareAttributeType = _dialElementList[outRuneIndex].SelectElement.Rune.BaseRuneSO.AttributeType;
        bool isResonanceCheck = true;
        for (int i = _dialElementList.Count - 1; i >= 0; i--)
        {
            if (_dialElementList[i].SelectElement != null)
            {
                int index = i;
                MagicCircleGlow(index, true);
                BaseRune rune = _usingDeck.Find(x => x == _dialElementList[index].SelectElement.Rune);
                // ????筌ｋ똾寃?
                _usingDeck.Remove(rune);

                if (rune.IsIncludeKeyword(KeywordName.Consume))
                {
                    _consumeDeck.Add(rune);
                }
                else
                {
                    rune.SetCoolTime();
                    _cooltimeDeck.Add(rune);
                }

                if (isResonanceCheck)
                    isResonanceCheck = _dialElementList[index].SelectElement.Rune.BaseRuneSO.AttributeType == compareAttributeType;

                (_dialElementList[index] as RuneDialElement).EffectHandler.Attack(3 - index, () =>
                {
                    if ((_dialElementList[index].SelectElement.Rune is VariableRune) && (index - 1 >= 0))
                    {
                        (_dialElementList[index].SelectElement.Rune as VariableRune).nextRune = _dialElementList[index - 1].SelectElement.Rune;
                    }
                    _dialElementList[index].Attack();
                    OnDialAttack?.Invoke();

                    //if (index == 0)
                    //{
                    //    (_dialElementList[index] as RuneDialElement).EffectHandler.Clear();
                    //    _isAttack = false;
                    //}
                });

                yield return new WaitForSeconds(0.1f);
            }
        }

        if (isResonanceCheck)
        {
            _resonance.Invocation(compareAttributeType);
        }
        yield return new WaitUntil(() => BattleManager.Instance.missileCount <= 0);
        yield return new WaitForSeconds(0.1f);
        (_dialElementList[outRuneIndex] as RuneDialElement).EffectHandler.Clear();
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].SelectElement = null;
        }
        _isAttack = false;
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
