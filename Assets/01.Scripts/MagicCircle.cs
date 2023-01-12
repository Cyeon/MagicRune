using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public enum RuneType
{
    Assist, // 보조
    Main, // 메인
}

public class MagicCircle : MonoBehaviour
{
    private Dictionary<RuneType, List<Card>> _runeDict;

    [SerializeField]
    private int _mainRuneCnt = 1;
    [SerializeField]
    private int _assistRuneCnt = 5;

    [SerializeField]
    private float _assistRuneDistance = 3f;

    [SerializeField]
    private float _cardAreaDistance = 5f;

    [SerializeField]
    private Card _runeTemplate;
    [SerializeField]
    private Card _mainRuneTemplate;

    public Enemy enemy;

    public void Awake()
    {
        _runeDict = new Dictionary<RuneType, List<Card>>();
    }

    public void SortCard()
    {
        if (_runeDict.ContainsKey(RuneType.Assist))
        {
            float angle = 360f / _runeDict[RuneType.Assist].Count;

            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(0, 0, -1 * angle * i + 90);
                //_runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(_assistRuneDistance, 0, 0);
                _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                //float height = Mathf.Sin(angle * (i + 1)) * _assistRuneDistance;
                //float width = Mathf.Cos(angle * (i + 1)) *_assistRuneDistance;
                //_runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(width, height, 0);
            }
        }

        if (_runeDict.ContainsKey(RuneType.Main))
        {
            _runeDict[RuneType.Main][0].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            _runeDict[RuneType.Main][0].GetComponentInParent<RectTransform>().transform.rotation = Quaternion.identity;
        }
    }

    public bool AddCard(Card card)
    {
        // 미리 보여준 보조 룬 근체에서 손가락을 때면 그 보조룬에 장착시키기
        if(_runeDict.ContainsKey(RuneType.Main) == false || (_runeDict[RuneType.Main].Count == 0))
        {
            if (_runeDict.ContainsKey(RuneType.Main))
            {
                if (_runeDict[RuneType.Main].Count >= _mainRuneCnt)
                {
                    Debug.Log("메인 룬이 꽉차 있습니다.");
                    return false;
                }

                GameObject go = Instantiate(_mainRuneTemplate.gameObject, this.transform);
                Card rune = go.GetComponent<Card>();
                rune.SetRune(card.Rune);
                rune.SetCoolTime(card.Rune.MainRune.DelayTurn);
                _runeDict[RuneType.Main].Add(rune);

                for(int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                {
                    GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                    Card grune = ggo.GetComponent<Card>();
                    grune.SetRune(null);
                    if (_runeDict.ContainsKey(RuneType.Assist))
                    {
                        _runeDict[RuneType.Assist].Add(grune);
                    }
                    else
                    {
                        _runeDict.Add(RuneType.Assist, new List<Card> { grune });
                    }
                }
            }
            else
            {
                GameObject go = Instantiate(_mainRuneTemplate.gameObject, this.transform);
                Card rune = go.GetComponent<Card>();
                rune.SetRune(card.Rune);
                rune.SetCoolTime(card.Rune.MainRune.DelayTurn);
                _runeDict.Add(RuneType.Main, new List<Card>() { rune });

                for (int i = 0; i < _runeDict[RuneType.Main][0].Rune.AssistRuneCount; i++)
                {
                    GameObject ggo = Instantiate(_runeTemplate.gameObject, this.transform);
                    Card grune = ggo.GetComponent<Card>();
                    grune.SetRune(null);
                    if (_runeDict.ContainsKey(RuneType.Assist))
                    {
                        _runeDict[RuneType.Assist].Add(grune);
                    }
                    else
                    {
                        _runeDict.Add(RuneType.Assist, new List<Card> { grune });
                    }
                }
            }
        }
        else
        {
            /*
            if (_runeDict.ContainsKey(RuneType.Assist))
            {
                if (_runeDict[RuneType.Assist].Count >= _assistRuneCnt)
                {
                    Debug.Log("보조 룬이 꽉차 있습니다.");
                    return false;
                }
                GameObject go = Instantiate(_runeTemplate.gameObject, this.transform);
                Card rune = go.GetComponent<Card>();
                rune.SetRune(card.Rune);
                rune.SetCoolTime(card.Rune.AssistRune.DelayTurn);
                _runeDict[RuneType.Assist].Add(rune);
            }
            else
            {
                GameObject go = Instantiate(_runeTemplate.gameObject, this.transform);
                Card rune = go.GetComponent<Card>();
                rune.SetRune(card.Rune);
                rune.SetCoolTime(card.Rune.AssistRune.DelayTurn);
                _runeDict.Add(RuneType.Assist, new List<Card>() { rune });
            }
            */

            // 근처에 있는 보조 룬으로 들어가기

            // 전체 탐색 후 자깃과의 거리 비교 가장 가까운 애로 교체

            int changeIndex = -1;
            for(int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                if (_runeDict[RuneType.Assist][i].Rune == null)
                {
                    changeIndex = i;
                    break;
                }
            }

            if (changeIndex == -1) return false;
            _runeDict[RuneType.Assist][changeIndex].SetRune(card.Rune);
        }

        SortCard();

        return true;
    }

    public void Damage()
    {
        int damage = 0;

        if (_runeDict.ContainsKey(RuneType.Assist))
        {
            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                //damage += _runeDict[RuneType.Assist][i]._runeSO.assistRuneValue;
                //damage += _runeDict[RuneType.Assist][i].Rune.AssistRune.EffectPair
            }
        }

        if (_runeDict.ContainsKey(RuneType.Main))
        {
            for (int i = 0; i < _runeDict[RuneType.Main].Count; i++)
            {
                //damage += _runeDict[RuneType.Main][i]._runeSO.mainRuneValue;
            }
        }

        enemy.Damage(damage);

        if (_runeDict.ContainsKey(RuneType.Assist))
        {
            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                Destroy(_runeDict[RuneType.Assist][i].gameObject);
            }
        }

        if (_runeDict.ContainsKey(RuneType.Main))
        {
            for (int i = 0; i < _runeDict[RuneType.Main].Count; i++)
            {
                Destroy(_runeDict[RuneType.Main][i].gameObject);
            }
        }

        _runeDict.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _cardAreaDistance);
        Gizmos.color = Color.white;
    }
#endif
}
