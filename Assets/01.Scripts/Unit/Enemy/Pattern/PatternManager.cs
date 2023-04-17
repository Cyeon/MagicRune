using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    private Pattern _currentPattern;
    public Pattern CurrentPattern => _currentPattern;
    public List<Pattern> patternList = new List<Pattern>();
    private int _index = -1;

    private DialScene _disalScene;

    private void Awake()
    {
        _disalScene = Managers.Scene.CurrentScene as DialScene;

        foreach (var pattern in transform.GetComponentsInChildren<Pattern>())
        {
            if(pattern.isIncluding)
                patternList.Add(pattern);
        }
    }

    public void ChangePattern(Pattern pattern)
    {
        _currentPattern = pattern;
        _disalScene?.ReloadPattern(_currentPattern.icon, _currentPattern.desc);
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
        if (BattleManager.Instance.enemy.isTurnSkip == false)
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
}
