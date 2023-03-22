using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dial : MonoBehaviour
{
    [SerializeField]
    private List<Rune> _deck; // �����ϰ� �ִ� ��� �� 
    [SerializeField]
    private List<Rune> _selectedDeck = null; // ������ �����ص� ���̾� ������ 1��° �� ��. 
    [SerializeField]
    private GameObject tempCard;
    [SerializeField]
    private BezierMissile _bezierMissile;
    [SerializeField]
    private Transform _enemyPos;

    private Dictionary<int, List<Rune>> _magicDict;
    private Dictionary<EffectType, List<EffectObjectPair>> _effectDict;
    private List<DialElement> _dialElementList;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    private bool _isAttack;

    private void Awake()
    {
        _magicDict = new Dictionary<int, List<Rune>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _magicDict.Add(i, new List<Rune>());
        }
        _dialElementList = new List<DialElement>();

        _deck = DeckManager.Instance.Deck;
        _selectedDeck = DeckManager.Instance.SelectedDeck;
    }

    private void Start()
    {
        // �� ó�� �� ������ �ִ� �� 
        //// �׳� 3���� �ص��Ǵµ� �׳� �̷��� ��
        //for (int i = 1; i <= _deck.List.Count; i++)
        //{
        //    for (int j = 1; j <= _deck.List[i - 1].Count; j++)
        //    {
        //        GameObject g = Instantiate(tempCard, this.transform.GetChild(i - 1));
        //        Rune c = g.GetComponent<Rune>();
        //        c.Dial = this;
        //        c.SetMagic(_deck.List[i - 1].List[j - 1]);
        //        c.UpdateUI();
        //        AddCard(c, 4 - i);
        //    }
        //}

        for(int i = 0; i < _deck.Count; i++)
        {
            int index = Random.Range(1, 4);
            GameObject g = Instantiate(tempCard, this.transform.GetChild(index - 1));
            Rune r = g.GetComponent<Rune>();
            r.Dial = this;
            r.SetMagic(_deck[i].Magic);
            r.UpdateUI();
            AddCard(r, index);

        }

        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();
            d.SetCardList(_magicDict[3 - i]);
            _dialElementList.Add(d);
        }
    }

    public void AddCard(Rune card, int tier)
    {
        if (card != null)
        {
            _magicDict[tier].Add(card);

            CardSort();
        }
    }

    private void CardSort()
    {
        if (_magicDict.ContainsKey(3))
        {
            float angle = -2 * Mathf.PI / _magicDict[3].Count;

            for (int i = 0; i < _magicDict[3].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 4; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 4; // 450
                _magicDict[3][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

                Vector2 direction = new Vector2(
                    _magicDict[3][i].transform.position.x - transform.position.x,
                    _magicDict[3][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _magicDict[3][i].transform.rotation = angleAxis;
            }
        }
        if (_magicDict.ContainsKey(2))
        {
            float angle = -2 * Mathf.PI / _magicDict[2].Count;

            for (int i = 0; i < _magicDict[2].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 2.9f; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 2.9f; // 450
                _magicDict[2][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                //_magicDict[2][i].transform.localScale = new Vector3(0.0133f, 0.0133f, 1);

                Vector2 direction = new Vector2(
                    _magicDict[2][i].transform.position.x - transform.position.x,
                    _magicDict[2][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _magicDict[2][i].transform.rotation = angleAxis;
            }
        }
        if (_magicDict.ContainsKey(1))
        {
            float angle = -2 * Mathf.PI / _magicDict[1].Count;

            for (int i = 0; i < _magicDict[1].Count; i++)
            {
                float height = Mathf.Sin(angle * i + (90 * Mathf.Deg2Rad)) * 1.7f; // 470
                float width = Mathf.Cos(angle * i + (90 * Mathf.Deg2Rad)) * 1.7f; // 450
                _magicDict[1][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                //_magicDict[1][i].transform.localScale = new Vector3(0.02f, 0.02f, 1);

                Vector2 direction = new Vector2(
                    _magicDict[1][i].transform.position.x - transform.position.x,
                    _magicDict[1][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _magicDict[1][i].transform.rotation = angleAxis;
            }
        }
    }

    public void AllMagicActive(bool value)
    {
        if (_magicDict.ContainsKey(1))
        {
            for (int i = 0; i < _magicDict[1].Count; i++)
            {
                _magicDict[1][i].gameObject.SetActive(false);
            }
        }
        if (_magicDict.ContainsKey(2))
        {
            for (int i = 0; i < _magicDict[2].Count; i++)
            {
                _magicDict[2][i].gameObject.SetActive(false);
            }
        }
        if (_magicDict.ContainsKey(3))
        {
            for (int i = 0; i < _magicDict[3].Count; i++)
            {
                _magicDict[3][i].gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        if (_isAttack == true) return;
        if (BattleManager.Instance.GameTurn == GameTurn.Player)
        {
            _isAttack = true;
            _effectDict = new Dictionary<EffectType, List<EffectObjectPair>>();
            foreach (DialElement d in _dialElementList)
            {
                if (d.SelectCard != null && d.SelectCard.IsCoolTime == false)
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

            // ����� ��
#if UNITY_EDITOR
            foreach (var d in _effectDict)
            {
                Debug.Log($"{d.Key} : {d.Value}");
            }
#endif


            GameObject g = null;
            for (int i = _dialElementList.Count - 1; i >= 0; i--)
            {
                if (_dialElementList[i].SelectCard != null && _dialElementList[i].SelectCard.IsCoolTime == false)
                {
                    g = _dialElementList[i].SelectCard.Magic.RuneEffect;
                    break;
                }
            }
            for (int i = _dialElementList.Count - 1; i >= 0; i--)
            {
                if (_dialElementList[i].SelectCard != null && _dialElementList[i].SelectCard.IsCoolTime == false)
                {
                    _dialElementList[i].SelectCard.SetCoolTime();
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
            b.Init(this.transform, _enemyPos, 1.5f, 0, 0, () =>
            {
                for (int i = 0; i < (int)EffectType.Etc; i++)
                {
                    AttackFunction((EffectType)i);
                }

                BattleManager.Instance.PlayerTurnEnd();
                _effectDict.Clear();
            });
            _isAttack = false;
        }
    }

    public void AttackFunction(EffectType effectType)
    {
        if (_effectDict.ContainsKey(effectType))
        {
            foreach (var e in _effectDict[effectType])
            {
                Unit target = e.pair.IsEnemy == true ? BattleManager.Instance.enemy : GameManager.Instance.player;
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
                        action = () => BattleManager.Instance.player.Attack(int.Parse(e.Effect));
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Attack(int.Parse(e.Effect) * c);
                        action = () => BattleManager.Instance.player.Attack(int.Parse(e.Effect));
                        break;
                }
                break;
            case EffectType.Defence:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => BattleManager.Instance.player.Shield += int.Parse(e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Shield += int.Parse(e.Effect) * c;
                        action = () => BattleManager.Instance.player.Shield += int.Parse(e.Effect);
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
                // ������ �ϴ� �ּ�...
                //action = () => _cardCollector.CardDraw(int.Parse(e.Effect));
                break;
            case EffectType.Etc:
                action = null;
                break;
        }

        return action;
    }

    public void AllMagicSetCoolTime()
    {
        foreach(var d in _magicDict)
        {
            foreach(Rune m in d.Value)
            {
                if(m.GetCoolTime() > 0)
                {
                    m.SetCoolTime(m.GetCoolTime() - 1);
                }
            }
        }
    }

    public void AllMagicSetCoolTime(int value)
    {
        foreach (var d in _magicDict)
        {
            foreach (Rune m in d.Value)
            {
                if (m.GetCoolTime() > 0)
                {
                    m.SetCoolTime(value);
                }
            }
        }
    }
}