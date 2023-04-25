﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    private Pattern _currentPattern;
    public Pattern CurrentPattern => _currentPattern;

    private Pattern _beforePattern;
    public Pattern BeforePattern => _beforePattern;

    public List<Pattern> patternList = new List<Pattern>();
    private Dictionary<string, Pattern[]> patternTreeDic = new Dictionary<string, Pattern[]>();
    private int _index = -1;

    private string _treeName;
    private bool _treeChange = false;

    [Header("UI")]
    [SerializeField] private SpriteRenderer _patternSprite;
    [SerializeField] private TextMeshPro _patternText;

    public void Init()
    {
        foreach (var pattern in transform.GetComponentsInChildren<Transform>())
        {
            if (pattern.name.Contains("Tree"))
            {
                Pattern[] list = pattern.GetComponentsInChildren<Pattern>();
                patternTreeDic.Add(pattern.name, list);
            }
            else
            { 
                Pattern cPattern = pattern.GetComponent<Pattern>();
                if(cPattern != null)
                {
                    if (cPattern.isIncluding) patternList.Add(cPattern);
                }
            }
        }
    }

    /// <summary>
    /// 패턴 트리 변경하는 함수 (인자를 아무것도 안 넘기면 기본트리로 돌아옴)
    /// </summary>
    /// <param name="treeName"></param>
    public void ChangeTree(string treeName = "")
    {
        if(treeName == "")
        {
            _treeChange = false;
            return;
        }

        if(patternTreeDic.ContainsKey(treeName))
        {
            _treeChange = true;
            _treeName = treeName;

            _index = 0;
            ChangePattern(patternTreeDic[_treeName][_index]);
        }
    }

    public void ChangePattern(Pattern pattern)
    {
        _beforePattern = _currentPattern;
        _currentPattern = pattern;
        UpdatePatternUI();
    }

    public void NextPattern()
    {
        _index++;

        if (_treeChange)
        {
            if (_index == patternTreeDic[_treeName].Length)
            {
                _index = 0;
            }
            ChangePattern(patternTreeDic[_treeName][_index]);
            return;
        }

        if (_index == patternList.Count)
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

    public Pattern GetNextPattern()
    {
        Pattern p = (_index + 1 == patternList.Count) ? patternList[0] : patternList[_index + 1];
        return p;
    }
}
