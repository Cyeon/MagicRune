using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public Pattern currentPattern;
    public List<Pattern> patternList = new List<Pattern>();
    private int _index = 0;

    private void Awake()
    {
        foreach(var pattern in transform.GetComponentsInChildren<Pattern>())
        {
            if(pattern.isIncluding)
                patternList.Add(pattern);
        }
    }

    public void NextPattern()
    {
        _index++;
        if(_index ==  patternList.Count)
        {
            _index = 0;
        }

        currentPattern = patternList[_index];
    }

    public void TurnAction()
    {
        currentPattern.TurnAction();
    }

    public void StartAction()
    {
        currentPattern.StartAction();
    }

    public void EndAction()
    {
        currentPattern.EndAction();
    }
}
