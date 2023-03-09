using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dial : MonoBehaviour
{
    [SerializeField]
    private DeckSO _deck;

    [SerializeField]
    private GameObject tempCard;
    [SerializeField]
    private BezierMissile _bezierMissile;

    private Dictionary<int, List<TestCard>> _cardDict;
    private Dictionary<EffectType, List<EffectObjectPair>> _effectDict;
    private List<DialElement> _dialElementList;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    private void Start()
    {
        _cardDict = new Dictionary<int, List<TestCard>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _cardDict.Add(i, new List<TestCard>());
        }
        _dialElementList = new List<DialElement>();

        // 그냥 3으로 해도되는데 그냥 이렇게 함
        for (int i = 1; i <= _deck.list.Count; i++)
        {
            for (int j = 1; j <= _deck.list[i - 1].list.Count; j++)
            {
                GameObject g = Instantiate(tempCard, this.transform.GetChild(i - 1));
                TestCard c = g.GetComponent<TestCard>();
                c.Dial = this;
                c.SetMagic(_deck.list[i - 1].list[j - 1]);
                c.UpdateUI();
                AddCard(c, 4 - i);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();
            d.SetCardList(_cardDict[3 - i]);
            _dialElementList.Add(d);
        }
    }

    public void AddCard(TestCard card, int tier)
    {
        if (card != null)
        {
            _cardDict[tier].Add(card);

            CardSort();
        }
    }

    private void CardSort()
    {
        if (_cardDict.ContainsKey(3))
        {
            float angle = -2 * Mathf.PI / _cardDict[3].Count;

            for (int i = 0; i < _cardDict[3].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 695; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 695; // 450
                _cardDict[3][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

                Vector2 direction = new Vector2(
                    _cardDict[3][i].transform.position.x - transform.position.x,
                    _cardDict[3][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _cardDict[3][i].GetComponent<RectTransform>().rotation = angleAxis;
            }
        }
        if (_cardDict.ContainsKey(2))
        {
            float angle = -2 * Mathf.PI / _cardDict[2].Count;

            for (int i = 0; i < _cardDict[2].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 510; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 510; // 450
                _cardDict[2][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

                Vector2 direction = new Vector2(
                    _cardDict[2][i].transform.position.x - transform.position.x,
                    _cardDict[2][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _cardDict[2][i].GetComponent<RectTransform>().rotation = angleAxis;
            }
        }
        if (_cardDict.ContainsKey(1))
        {
            float angle = -2 * Mathf.PI / _cardDict[1].Count;

            for (int i = 0; i < _cardDict[1].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 328; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 328; // 450
                _cardDict[1][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);

                Vector2 direction = new Vector2(
                    _cardDict[1][i].transform.position.x - transform.position.x,
                    _cardDict[1][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _cardDict[1][i].GetComponent<RectTransform>().rotation = angleAxis;
            }
        }
    }

    public void AllMagicActive(bool value)
    {
        if (_cardDict.ContainsKey(1))
        {
            for (int i = 0; i < _cardDict[1].Count; i++)
            {
                _cardDict[1][i].gameObject.SetActive(false);
            }
        }
        if (_cardDict.ContainsKey(2))
        {
            for (int i = 0; i < _cardDict[2].Count; i++)
            {
                _cardDict[2][i].gameObject.SetActive(false);
            }
        }
        if (_cardDict.ContainsKey(3))
        {
            for (int i = 0; i < _cardDict[3].Count; i++)
            {
                _cardDict[3][i].gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        _effectDict = new Dictionary<EffectType, List<EffectObjectPair>>();
        foreach (DialElement d in _dialElementList)
        {
            if (d.SelectCard != null)
            {
                foreach (Pair p in d.SelectCard.Magic.MainRune.EffectDescription)
                {
                    if (_effectDict.ContainsKey(p.EffectType))
                    {
                        _effectDict[p.EffectType].Add(new EffectObjectPair(p, d.SelectCard.Magic.RuneEffect));
                    }
                    else
                    {
                        _effectDict.Add(p.EffectType, new List<EffectObjectPair> { new EffectObjectPair(p, d.SelectCard.Magic.RuneEffect) });
                    }
                }
            }
        }

        // 디버그 용
#if UNITY_EDITOR
        foreach(var d in _effectDict)
        {
            Debug.Log($"{d.Key} : {d.Value}");
        }
#endif

        
        GameObject g = null;
        for(int i = _dialElementList.Count - 1; i >= 0; i--)
        {
            if(_dialElementList[i].SelectCard != null)
            {
                g = _dialElementList[i].SelectCard.Magic.RuneEffect;
                break;
            }
        }

        if (g == null)
        {
            _effectDict.Clear();
            return;
        }
        UIManager.Instance.CardDescDown();
        BezierMissile b = Instantiate(_bezierMissile, this.transform.parent);
        b.SetEffect(g);
        b.SetTrailColor(EffectType.Attack);
        b.Init(new Vector2(0, -1480), new Vector2(0, 607), 1.5f, 0, 0, () =>
        {
            for (int i = 0; i < (int)EffectType.Etc; i++)
            {
                AttackFunction((EffectType)i);
            }

            _effectDict.Clear();
        });
    }

    public void AttackFunction(EffectType effectType)
    {
        if (_effectDict.ContainsKey(effectType))
        {
            foreach (var e in _effectDict[effectType])
            {
                Unit target = e.pair.IsEnemy == true ? GameManager.Instance.enemy : GameManager.Instance.player;
                AttackEffectFunction(effectType, target, e.pair)?.Invoke();
            }
        }
    }

    public Action AttackEffectFunction(EffectType effectType, Unit target, Pair e)
    {
        Action action = null;
        //int c = 0;
        //for (int i = 0; i < _effectDict[RuneType.Assist].Count; i++)
        //{
        //    if (_runeTempDict[RuneType.Assist][i].Rune != null && _runeTempDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.AttributeType)
        //    {
        //        c++;
        //    }
        //}
        //if (_runeTempDict[RuneType.Main][0].Rune != null && _runeTempDict[RuneType.Main][0].Rune.AssistRune.Attribute == e.AttributeType)
        //    c++;

        switch (effectType)
        {
            case EffectType.Attack:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => GameManager.Instance.player.Attack(int.Parse(e.Effect));
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Attack(int.Parse(e.Effect) * c);
                        action = () => GameManager.Instance.player.Attack(int.Parse(e.Effect));
                        break;
                }
                break;
            case EffectType.Defence:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => GameManager.Instance.player.Shield += int.Parse(e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Shield += int.Parse(e.Effect) * c;
                        action = () => GameManager.Instance.player.Shield += int.Parse(e.Effect);
                        break;
                }
                break;
            case EffectType.Status:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                        break;
                    case AttackType.Double:
                        //action = () => StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect) * c);
                        action = () => StatusManager.Instance.AddStatus(target, e.StatusType, int.Parse(e.Effect));
                        break;
                }
                break;
            case EffectType.Destroy:
                action = () => StatusManager.Instance.RemStatus(target, e.StatusType);
                break;
            case EffectType.Draw:
                // 지금은 일단 주석...
                //action = () => _cardCollector.CardDraw(int.Parse(e.Effect));
                break;
            case EffectType.Etc:
                action = null;
                break;
        }

        return action;

        //switch (e.Condition.ConditionType)
        //{
        //    case ConditionType.None:
        //        return action;
        //    case ConditionType.HeathComparison:
        //        if (target.IsHealthAmount(e.Condition.Value, e.Condition.ComparisonType))
        //        {
        //            return action;
        //        }
        //        break;
        //    case ConditionType.AssistRuneCount:
        //        int count = 0;
        //        for (int i = 0; i < _runeTempDict[RuneType.Assist].Count; i++)
        //        {
        //            if (_runeTempDict[RuneType.Assist][i].Rune != null)
        //            {
        //                count++;
        //            }
        //        }
        //        if (count >= e.Condition.Value)
        //        {
        //            return action;
        //        }
        //        break;
        //    case ConditionType.AttributeComparison:
        //        int cnt = 0;
        //        if (_runeTempDict[RuneType.Main][0].Rune.MainRune.Attribute == e.Condition.AttributeType)
        //            cnt++;
        //        for (int i = 0; i < _runeTempDict[RuneType.Assist].Count; i++)
        //        {
        //            if (_runeTempDict[RuneType.Assist][i].Rune.AssistRune.Attribute == e.Condition.AttributeType)
        //            {
        //                cnt++;
        //            }
        //        }

        //        switch (e.Condition.ComparisonType)
        //        {
        //            case ComparisonType.MoreThan:
        //                if (cnt >= e.Condition.Value)
        //                {
        //                    return action;
        //                }
        //                break;
        //            case ComparisonType.LessThan:
        //                if (cnt <= e.Condition.Value)
        //                {
        //                    return action;
        //                }
        //                break;
        //        }
        //        break;
        //    case ConditionType.StatusComparison:
        //        switch (e.Condition.ComparisonType)
        //        {
        //            case ComparisonType.MoreThan:
        //                if (StatusManager.Instance.GetUnitStatusValue(target, e.Condition.StatusType) >= e.Condition.Value)
        //                {
        //                    return action;
        //                }
        //                break;
        //            case ComparisonType.LessThan:
        //                if (StatusManager.Instance.GetUnitStatusValue(target, e.Condition.StatusType) <= e.Condition.Value)
        //                {
        //                    return action;
        //                }
        //                break;
        //        }
        //        break;
        //}

        //return null;
    }
}