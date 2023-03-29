using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Dial : MonoBehaviour
{
    //[SerializeField]
    //private List<Rune> _deck; // 소지하고 있는 모든 룬 
    [SerializeField]
    private int _maxRuneCount = 3;
    [SerializeField, Range(90f, 360f)]
    private float _runeAngle = 180f;
    public float RuneAngle => _runeAngle;
    [SerializeField]
    private float _startAngle = 180;

    [SerializeField]
    private List<Rune> _selectedDeck = null; // 사전에 설정해둔 다이얼 안쪽의 1번째 줄 덱. 
    [SerializeField]
    private GameObject tempCard;
    [SerializeField]
    private BezierMissile _bezierMissile;
    [SerializeField]
    private Transform _enemyPos;

    private Dictionary<int, List<RuneUI>> _runeDict;
    private Dictionary<EffectType, List<EffectObjectPair>> _effectDict = new Dictionary<EffectType, List<EffectObjectPair>>();
    private List<DialElement> _dialElementList;

    private bool _isAttack;

    private DialScene _dialScene;

    private void Awake()
    {
        _runeDict = new Dictionary<int, List<RuneUI>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _runeDict.Add(i, new List<RuneUI>());
        }
        _dialElementList = new List<DialElement>();

        //_deck = DeckManager.Instance.Deck;
        //_selectedDeck = DeckManager.Instance.SelectedDeck;

        _isAttack = false;
    }

    private void Start()
    {
        _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;

        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();

            _dialElementList.Add(d);
        }

        SettingDialRune(true);  
    }

    public void SettingDialRune(bool isReset)
    {
        foreach (KeyValuePair<int, List<RuneUI>> runeList in _runeDict)
        {
            foreach(RuneUI rune in runeList.Value)
            {
                if(rune != null)
                {
                    ResourceManager.Instance.Destroy(rune.gameObject);
                }
            }
        }
        _runeDict.Clear();

        for(int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].ResetRuneList();
            _dialElementList[i].SelectCard = null;
        }

        DeckManager.Instance.UsingDeckSort();
        int maxRuneCount = DeckManager.Instance.GetUsingRuneCount();
        for (int i = 0; i < _maxRuneCount * 3; i++)
        {
            if (maxRuneCount <= 0)
            {
                break;
            }

            int runeIndex = Random.Range(0, maxRuneCount);

            RuneUI r = ResourceManager.Instance.Instantiate("Rune").GetComponent<RuneUI>();
            r.transform.SetParent(_dialElementList[2 - (i % 3)].transform);
            r.transform.localScale = new Vector3(0.1f, 0.1f);
            r.Dial = this;
            r.DialElement = _dialElementList[2 - (i % 3)];
            _dialElementList[2 - (i % 3)].AddRuneList(r);
            r.SetRune(DeckManager.Instance.Deck[runeIndex]);
            r.UpdateUI();
            if (isReset == true)
            {
                r.Rune.SetCoolTime(0);
            }
            r.SetCoolTime();
            AddCard(r, (i % 3) + 1);
            DeckManager.Instance.RuneSpawn(runeIndex, maxRuneCount - 1);

            maxRuneCount--;
        }
    }

    public void AddCard(RuneUI card, int tier)
    {
        if (card != null)
        {
            if (_runeDict.ContainsKey(tier))
            {
                _runeDict[tier].Add(card);
            }
            else
            {
                _runeDict.Add(tier, new List<RuneUI> { card });
            }

            CardSort();
        }
    }

    private void CardSort()
    {
        if (_runeDict.ContainsKey(3))
        {
            float angle = -1 * _runeAngle / _runeDict[3].Count;

            for (int i = 0; i < _runeDict[3].Count; i++)
            {
                float height = Mathf.Sin(angle * Mathf.Deg2Rad * i + (_startAngle * Mathf.Deg2Rad) + angle / 2f * Mathf.Deg2Rad) * 4;
                float width = Mathf.Cos(angle * Mathf.Deg2Rad * i + (_startAngle * Mathf.Deg2Rad) + angle / 2f * Mathf.Deg2Rad) * 4;
                _runeDict[3][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);

                Vector2 direction = new Vector2(
                    _runeDict[3][i].transform.position.x - transform.position.x,
                    _runeDict[3][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _runeDict[3][i].transform.rotation = angleAxis;
            }
        }
        if (_runeDict.ContainsKey(2))
        {
            float angle = -1 * _runeAngle / _runeDict[2].Count;

            for (int i = 0; i < _runeDict[2].Count; i++)
            {
                float height = Mathf.Sin(angle * Mathf.Deg2Rad * i + (_startAngle * Mathf.Deg2Rad) + angle / 2f * Mathf.Deg2Rad) * 2.9f; // 470
                float width = Mathf.Cos(angle * Mathf.Deg2Rad * i + (_startAngle * Mathf.Deg2Rad) + angle / 2f * Mathf.Deg2Rad) * 2.9f; // 450
                _runeDict[2][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                //_magicDict[2][i].transform.localScale = new Vector3(0.0133f, 0.0133f, 1);

                Vector2 direction = new Vector2(
                    _runeDict[2][i].transform.position.x - transform.position.x,
                    _runeDict[2][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _runeDict[2][i].transform.rotation = angleAxis;
            }
        }
        if (_runeDict.ContainsKey(1))
        {
            float angle = -1 * _runeAngle / _runeDict[1].Count;

            for (int i = 0; i < _runeDict[1].Count; i++)
            {
                float height = Mathf.Sin(angle * Mathf.Deg2Rad * i + (_startAngle * Mathf.Deg2Rad) + angle / 2f * Mathf.Deg2Rad) * 1.7f; // 470
                float width = Mathf.Cos(angle * Mathf.Deg2Rad * i + (_startAngle * Mathf.Deg2Rad) + angle / 2f * Mathf.Deg2Rad) * 1.7f; // 450
                _runeDict[1][i].transform.position = new Vector3(width + this.transform.position.x, height + this.transform.position.y, 0);
                //_magicDict[1][i].transform.localScale = new Vector3(0.02f, 0.02f, 1);

                Vector2 direction = new Vector2(
                    _runeDict[1][i].transform.position.x - transform.position.x,
                    _runeDict[1][i].transform.position.y - transform.position.y
                );

                float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion angleAxis = Quaternion.AngleAxis(ang - 90f, Vector3.forward);
                _runeDict[1][i].transform.rotation = angleAxis;
            }
        }
    }

    public void AllMagicActive(bool value)
    {
        if (_runeDict.ContainsKey(1))
        {
            for (int i = 0; i < _runeDict[1].Count; i++)
            {
                _runeDict[1][i].gameObject.SetActive(false);
            }
        }
        if (_runeDict.ContainsKey(2))
        {
            for (int i = 0; i < _runeDict[2].Count; i++)
            {
                _runeDict[2][i].gameObject.SetActive(false);
            }
        }
        if (_runeDict.ContainsKey(3))
        {
            for (int i = 0; i < _runeDict[3].Count; i++)
            {
                _runeDict[3][i].gameObject.SetActive(false);
            }
        }
    }

    public void Attack()
    {
        if (_isAttack == true) return;
        if (BattleManager.Instance.GameTurn == GameTurn.Player)
        {
            _isAttack = true;
            //_effectDict = new Dictionary<EffectType, List<EffectObjectPair>>();
            //foreach (DialElement d in _dialElementList)
            //{
            //    if (d.SelectCard != null && d.SelectCard.Rune.IsCoolTime == false)
            //    {
            //        foreach (Pair p in d.SelectCard.Rune.GetRune().MainRune.EffectDescription)
            //        {
            //            if (_effectDict.ContainsKey(p.EffectType))
            //            {
            //                _effectDict[p.EffectType].Add(new EffectObjectPair(p, d.SelectCard.Rune.GetRune().RuneEffect));
            //            }
            //            else
            //            {
            //                _effectDict.Add(p.EffectType, new List<EffectObjectPair> { new EffectObjectPair(p, d.SelectCard.Rune.GetRune().RuneEffect) });
            //            }
            //        }
            //    }
            //}


            GameObject g = null;
            for (int i = _dialElementList.Count - 1; i >= 0; i--)
            {
                if (_dialElementList[i].SelectCard != null && _dialElementList[i].SelectCard.Rune.IsCoolTime == false)
                {
                    g = _dialElementList[i].SelectCard.Rune.GetRune().RuneEffect;
                    break;
                }
            }
            for (int i = _dialElementList.Count - 1; i >= 0; i--)
            {
                if (_dialElementList[i].SelectCard != null && _dialElementList[i].SelectCard.Rune.IsCoolTime == false)
                {
                    _dialElementList[i].SelectCard.Rune.GetRune(); // ...? 뭐하는 코드지?
                }
            }

            if (g == null)
            {
                _effectDict.Clear();
                return;
            }
            _dialScene?.CardDescDown();
            BezierMissile b = ResourceManager.Instance.Instantiate("BezierMissile", this.transform.parent).GetComponent<BezierMissile>();
            b.SetEffect(g);
            b.SetTrailColor(EffectType.Attack);
            b.Init(this.transform, _enemyPos, 1.5f, 0, 0, () =>
            {
                for(int i = 0; i < 3; i++)
                {
                    _dialElementList[i].Attack();
                }

                //for (int i = 0; i < (int)EffectType.Etc; i++)
                //{
                //    AttackFunction((EffectType)i);
                //}

                for (int i = 0; i < 3; i++)
                {
                    _dialElementList[i].SelectCard = null;
                }

                _effectDict.Clear();
                BattleManager.Instance.PlayerTurnEnd();
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
                        action = () => BattleManager.Instance.player.Attack(e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Attack(e.Effect * c);
                        action = () => BattleManager.Instance.player.Attack(e.Effect);
                        break;
                }
                break;
            case EffectType.Defence:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => BattleManager.Instance.player.AddShield(e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => GameManager.Instance.player.Shield += e.Effect * c;
                        action = () => BattleManager.Instance.player.AddShield(e.Effect);
                        break;
                }
                break;
            case EffectType.Status:
                switch (e.AttackType)
                {
                    case AttackType.Single:
                        action = () => StatusManager.Instance.AddStatus(target, e.StatusType, (int)e.Effect);
                        break;
                    case AttackType.Double:
                        //action = () => StatusManager.Instance.AddStatus(target, e.StatusType, (int)e.Effect * c);
                        action = () => StatusManager.Instance.AddStatus(target, e.StatusType, (int)e.Effect);
                        break;
                }
                break;
            case EffectType.Destroy:
                action = () => StatusManager.Instance.RemStatus(target, e.StatusType);
                break;
            case EffectType.Draw:
                // 지금은 일단 주석...
                //action = () => _cardCollector.CardDraw((int)e.Effect);
                break;
            case EffectType.Etc:
                action = null;
                break;
        }

        return action;
    }

    public void AllMagicSetCoolTime()
    {
        foreach (var d in _runeDict)
        {
            foreach (RuneUI m in d.Value)
            {
                if (m.Rune.GetCoolTime() > 0)
                {
                    m.Rune.SetCoolTime(m.Rune.GetCoolTime() - 1);
                    m.SetCoolTime();
                }
            }
        }
    }
}