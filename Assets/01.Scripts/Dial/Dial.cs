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
    private List<Rune> _selectedDeck = null; // 사전에 설정해둔 다이얼 안쪽의 1번째 줄 덱. 
    [SerializeField]
    private GameObject tempCard;
    [SerializeField]
    private BezierMissile _bezierMissile;
    [SerializeField]
    private Transform _enemyPos;

    private Dictionary<int, List<RuneUI>> _magicDict;
    private Dictionary<EffectType, List<EffectObjectPair>> _effectDict = new Dictionary<EffectType, List<EffectObjectPair>>();
    private List<DialElement> _dialElementList;

    private bool _isAttack;

    private DialScene _dialScene;

    private void Awake()
    {
        _magicDict = new Dictionary<int, List<RuneUI>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _magicDict.Add(i, new List<RuneUI>());
        }
        _dialElementList = new List<DialElement>();

        //_deck = DeckManager.Instance.Deck;
        //_selectedDeck = DeckManager.Instance.SelectedDeck;

        _isAttack = false;
    }

    private void Start()
    {
        _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;

        List<int> indexList = new List<int> { 1, 2, 3 };
        for (int i = 0; i < DeckManager.Instance.Deck.Count; i++)
        {
            if(indexList.Count <= 0)
            {
                // 들어갈 수 있는 데가 아무곳도 없으면 어카지?
                break;
            }

            int index = Random.Range(0, indexList.Count);
            //GameObject g = Instantiate(tempCard, this.transform.GetChild(index - 1));
            RuneUI r = ResourceManager.Instance.Instantiate("Rune").GetComponent<RuneUI>();
            r.transform.SetParent(this.transform.GetChild(3 - index).transform);
            r.Dial = this;
            r.DialElement = this.transform.GetChild(3 - index).GetComponent<DialElement>();
            r.SetRune(DeckManager.Instance.Deck[i]);
            r.UpdateUI();
            r.Rune.SetCoolTime(0);
            r.SetCoolTime();
            AddCard(r, index);

            // 한 라인에 들어갈 수 있는 최대 룬 개수 처리 해야함. 이러면 되지 않았을 까?
            if(this.transform.GetChild(3 - index).GetComponent<DialElement>().IsFull == true) // 이거 지금은 무조건 조건 만족 안함
            {
                indexList.RemoveAt(index);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();

            List<RuneUI> runeList = new List<RuneUI>();
            for (int j = 0; j < _magicDict[3 - i].Count; j++)
            {
                runeList.Add(_magicDict[3 - i][j]);
            }
            d.SetCardList(runeList); // 여기서 룬 리스트를 넣어주어서 그럼. 이 구조 수정 필요
            _dialElementList.Add(d);
        }

        

        //for(int i = 0; i < DeckManager.Instance.Deck.Count; i++) //룬 개수 적어서 임시로 한 번 더 돌렸음 
        //{
        //    int index = Random.Range(1, 4);
        //    GameObject g = Instantiate(tempCard, this.transform.GetChild(index - 1));
        //    Rune r = g.GetComponent<Rune>();
        //    r.Dial = this;
        //    r.SetMagic(DeckManager.Instance._defaultRune[i]);
        //    r.UpdateUI();
        //    AddCard(r, index);
        //    _deck.Add(r);
        //}

        
    }

    public void AddCard(RuneUI card, int tier)
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
        foreach (var d in _magicDict)
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