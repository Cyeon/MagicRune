using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    private Pattern _currentPattern;
    public Pattern CurrentPattern => _currentPattern;
    public List<Pattern> patternList = new List<Pattern>();
    private int _index = -1;

    [Header("UI")]
    [SerializeField] private SpriteRenderer _patternSprite;
    [SerializeField] private TextMeshPro _patternText;

    private void Awake()
    {
        foreach (var pattern in transform.GetComponentsInChildren<Pattern>())
        {
            if(pattern.isIncluding)
                patternList.Add(pattern);
        }
    }

    public void ChangePattern(Pattern pattern)
    {
        _currentPattern = pattern;
        UpdatePatternUI();
    }

    public void NextPattern()
    {
        _index++;
        if(_index ==  patternList.Count)
        {
            _index = 0;
        }

        ChangePattern(patternList[_index]);
    }

    public void TurnAction()
    {
        if (BattleManager.Instance.Enemy.isTurnSkip == false)
            _currentPattern.TurnAction();
    }

    public void StartAction()
    {
        _currentPattern.StartAction();
    }

    public void EndAction()
    {
        _currentPattern.EndAction();
    }

    public void UpdatePatternUI()
    {
        _patternSprite.sprite = _currentPattern.icon;
        _patternText.text = _currentPattern.desc;
    }
}
