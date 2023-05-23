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
        foreach (var runeList in _runeDict)
        {
            for (int i = 0; i < runeList.Value.Count; i++)
            {
                Managers.Resource.Destroy(runeList.Value[i].gameObject);
            }
        }
        _runeDict.Clear();

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
            if (_runeDict.ContainsKey(i))
            {
                int count = _runeDict[i].Count;
                for (int k = 0; k < _copyCount; k++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        BaseRuneUI r = Managers.Resource.Instantiate("Rune/BaseRune", _dialElementList[3 - i].transform).GetComponent<BaseRuneUI>();
                        r.SetRune(_runeDict[i][j].Rune);
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
                BezierMissile b = Managers.Resource.Instantiate("BezierMissile", this.transform.parent).GetComponent<BezierMissile>();
                if (_dialElementList[i].SelectElement.Rune.BaseRuneSO.RuneEffect != null)
                {
                    b.SetEffect(_dialElementList[i].SelectElement.Rune.BaseRuneSO.RuneEffect);
                }

                if (isResonanceCheck)
                    isResonanceCheck = _dialElementList[i].SelectElement.Rune.BaseRuneSO.AttributeType == compareAttributeType;

                switch (_dialElementList[i].SelectElement.Rune.BaseRuneSO.AttributeType)
                {
                    case AttributeType.None:
                        break;
                    case AttributeType.NonAttribute:
                        b.SetTrailColor(Color.gray);
                        break;
                    case AttributeType.Fire:
                        b.SetTrailColor(Color.red);
                        break;
                    case AttributeType.Ice:
                        b.SetTrailColor(Color.cyan);
                        break;
                    case AttributeType.Wind:
                        b.SetTrailColor(new Color(0, 1, 0));
                        break;
                    case AttributeType.Ground:
                        b.SetTrailColor(new Color(0.53f, 0.27f, 0));
                        break;
                    case AttributeType.Electric:
                        b.SetTrailColor(Color.yellow);
                        break;
                }

                Transform pos = null;
                float bendValue = 0f;
                switch (_dialElementList[i].SelectElement.Rune.BaseRuneSO.Direction)
                {
                    case EffectDirection.Enemy:
                        pos = BattleManager.Instance.Enemy.transform;
                        bendValue = 7f;
                        break;
                    case EffectDirection.Player:
                        pos = this.transform;
                        bendValue = 15f;
                        break;
                }
                Managers.Sound.PlaySound(_dialElementList[i].SelectElement.Rune.BaseRuneSO.RuneSound, SoundType.Effect);
                b.Init(_dialElementList[i].SelectElement.transform, pos, 1.5f, bendValue, bendValue, () =>
                {
                    _dialElementList[index].Attack();
                    //_dialElementList[i] = null;
                });

                BattleManager.Instance.missileCount += 1;
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
