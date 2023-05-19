using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapDial : MonoBehaviour
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
    private Dictionary<int, List<MapRuneUI>> _runeDict;
    private List<MapDialElement> _dialElementList;
    public List<MapDialElement> DialElementList => _dialElementList;

    #endregion

    private bool _isAttack;

    private void Awake()
    {
        #region Initialization 

        _runeDict = new Dictionary<int, List<MapRuneUI>>(3);
        for (int i = 1; i <= _lineDistanceArray.Length; i++)
        {
            _runeDict.Add(i, new List<MapRuneUI>());
        }
        _dialElementList = new List<MapDialElement>();
        #endregion

        _isAttack = false;
    }

    private void Start()
    {
        for (int i = 0; i < _lineDistanceArray.Length; i++)
        {
            MapDialElement d = this.transform.GetChild(i).GetComponent<MapDialElement>();
            d.SetLineID(_lineDistanceArray.Length - i);

            _dialElementList.Add(d);
        }
    }

    public void AddCard(MapRuneUI card, int tier)
    {
        if (card != null)
        {
            if (_runeDict.ContainsKey(tier))
            {
                _runeDict[tier].Add(card);
            }
            else
            {
                _runeDict.Add(tier, new List<MapRuneUI> { card });
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

    public void Clear()
    {
        if (_dialElementList.Count == 0) return;

        for(int i = _runeDict[3].Count - 1; i >= 0; i--)
        {
            if (_runeDict[3][i] != null)
                Managers.Resource.Destroy(_runeDict[3][i].gameObject);
        }

        _dialElementList[0].SelectCard = null;
        _dialElementList[0].Clear();
        _runeDict.Clear();
    }

    public void ResetDial()
    {
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].transform.eulerAngles = Vector3.zero;
        }
    }

    public void MapStageSpawn()
    {
        if (_dialElementList.Count == 0) return;

        for(int i = 0; i < Managers.Map.CurrentPeriodStageList.Count; i++)
        {
            StageType type = Managers.Map.CurrentPeriodStageList[i];
            MapRuneUI rune = Managers.Resource.Instantiate("Stage/" + type.ToString() + "Stage", _dialElementList[0].transform).GetComponent<MapRuneUI>();
            rune.transform.localScale = Vector3.one * 0.1f;

            rune.SetInfo(rune.GetComponent<Stage>().InStage);
            AddCard(rune, 3);
            _dialElementList[0].AddRuneList(rune);
        }

        RuneSort();
    }

    public void PanelUI()
    {

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

    public void MagicCircleGlow(int index, bool value)
    {
        _dialElementList[index].IsGlow = value;
    }

    public bool MagicEmpty()
    {
        return _dialElementList[0].SelectCard == null && _dialElementList[1].SelectCard == null && _dialElementList[2].SelectCard == null;
    }

    public void Attack()
    {
        if (_dialElementList[0].SelectCard == null) return;

        if (_isAttack == true) return;
        _isAttack = true;

        _dialElementList[0].SelectCard.ClickAction()?.Invoke();
        _isAttack = false;

        _runeDict[3].Remove(_dialElementList[0].SelectCard);
        Managers.Resource.Destroy(_dialElementList[0].SelectCard.gameObject);
        Managers.Map.MapScene.mapDial.gameObject.SetActive(false);
    }

    public void AllMagicCircleGlow(bool value)
    {
        for (int i = 0; i < _dialElementList.Count; i++)
        {
            _dialElementList[i].IsGlow = value;
        }
    }
}
