using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum RuneType
{
    Assist, // 보조
    Main, // 메인
}

public class MagicCircle : MonoBehaviour
{
    private Dictionary<RuneType, List<Rune>> _runeDict;

    [SerializeField]
    private int _mainRuneCnt = 1;
    [SerializeField]
    private int _assistRuneCnt = 5;

    [SerializeField]
    private float _assistRuneDistance = 3f;

    [SerializeField]
    private float _cardAreaDistance = 5f;

    [SerializeField]
    private Rune _runeTemplate;
    [SerializeField]
    private Rune _mainRuneTemplate;

    public Enemy enemy;

    public void Awake()
    {
        _runeDict = new Dictionary<RuneType, List<Rune>>();
    }

    public void SortCard()
    {
        if (_runeDict.ContainsKey(RuneType.Assist))
        {
            float angle = 360f / _runeDict[RuneType.Assist].Count;

            for (int i = 0; i < _runeDict[RuneType.Assist].Count; i++)
            {
                _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(0, 0, angle * (i + 1) + 90);
                //_runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(_assistRuneDistance, 0, 0);
                _runeDict[RuneType.Assist][i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
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
        float distance = Vector2.Distance(card.GetComponent<RectTransform>().anchoredPosition, transform.position);
        if(distance <= _cardAreaDistance)
        {
            if (_runeDict.ContainsKey(RuneType.Main))
            {
                if (_runeDict[RuneType.Main].Count >= _mainRuneCnt)
                {
                    Debug.Log("메인 룬이 꽉차 있습니다.");
                    return false;
                }

                GameObject go = Instantiate(_mainRuneTemplate.gameObject, this.transform);
                Rune rune = go.GetComponent<Rune>();
                rune.SetRune(card.Rune);
                _runeDict[RuneType.Main].Add(rune);
            }
            else
            {
                GameObject go = Instantiate(_mainRuneTemplate.gameObject, this.transform);
                Rune rune = go.GetComponent<Rune>();
                rune.SetRune(card.Rune);
                _runeDict.Add(RuneType.Main, new List<Rune>() { rune });
            }
        }
        else
        {
            if (_runeDict.ContainsKey(RuneType.Assist))
            {
                if (_runeDict[RuneType.Assist].Count >= _assistRuneCnt)
                {
                    Debug.Log("보조  룬이 꽉차 있습니다.");
                    return false;
                }
                GameObject go = Instantiate(_runeTemplate.gameObject, this.transform);
                Rune rune = go.GetComponent<Rune>();
                rune.SetRune(card.Rune);
                _runeDict[RuneType.Assist].Add(rune);
            }
            else
            {
                GameObject go = Instantiate(_runeTemplate.gameObject, this.transform);
                Rune rune = go.GetComponent<Rune>();
                rune.SetRune(card.Rune);
                _runeDict.Add(RuneType.Assist, new List<Rune>() { rune });
            }
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
