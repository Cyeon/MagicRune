using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Dictionary<int, List<BaseRuneUI>> _runeDict;
    private List<DialElement> _dialElementList;
    public List<DialElement> DialElementList => _dialElementList;

    #endregion

    private bool _isAttack;
    private Resonance _resonance;

    private void Awake()
    {
        #region Initialization 
        _resonance = GetComponent<Resonance>();

        _runeDict = new Dictionary<int, List<BaseRuneUI>>(3);
        for (int i = 1; i <= 3; i++)
        {
            _runeDict.Add(i, new List<BaseRuneUI>());
        }
        _dialElementList = new List<DialElement>();
        #endregion

        _isAttack = false;
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            DialElement d = this.transform.GetChild(i).GetComponent<DialElement>();
            d.SetLineID(3 - i);

            _dialElementList.Add(d);
        }
    }

    public void AddCard(BaseRuneUI card, int tier)
    {
        if (card != null)
        {
            if (_runeDict.ContainsKey(tier))
            {
                _runeDict[tier].Add(card);
            }
            else
            {
                _runeDict.Add(tier, new List<BaseRuneUI> { card });
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

    public void ResetDial()
    {
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].transform.eulerAngles = Vector3.zero;
        }
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

    public bool MagicEmpty()
    {
        return _dialElementList[0].SelectCard == null && _dialElementList[1].SelectCard == null && _dialElementList[2].SelectCard == null;
    }

    public void Attack()
    {
        if (MagicEmpty() == true) return;

        if (_isAttack == true) return;
        if (BattleManager.Instance.GameTurn == GameTurn.Player)
        {
            _isAttack = true;

            StartCoroutine(AttackCoroutine());
            _isAttack = false;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        Define.DialScene?.CardDescDown();

        // 이 부분 예외처리 필요
        AttributeType compareAttributeType = _dialElementList[0].SelectCard.Rune.BaseRuneSO.AttributeType;
        bool isResonanceCheck = true;
        for (int i = _dialElementList.Count - 1; i >= 0; i--)
        {
            if (_dialElementList[i].SelectCard != null)
            {
                // 방의 효과 발동
                int index = i;
                _dialElementList[i].IsGlow = true;
                BezierMissile b = Managers.Resource.Instantiate("BezierMissile", this.transform.parent).GetComponent<BezierMissile>();
                if (_dialElementList[i].SelectCard.Rune.BaseRuneSO.RuneEffect != null)
                {
                    b.SetEffect(_dialElementList[i].SelectCard.Rune.BaseRuneSO.RuneEffect);
                }

                if (isResonanceCheck)
                    isResonanceCheck = _dialElementList[i].SelectCard.Rune.BaseRuneSO.AttributeType == compareAttributeType;

                switch (_dialElementList[i].SelectCard.Rune.BaseRuneSO.AttributeType)
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
                switch (_dialElementList[i].SelectCard.Rune.BaseRuneSO.Direction)
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
                b.Init(_dialElementList[i].SelectCard.transform, pos, 1.5f, bendValue, bendValue, () =>
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
