using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternManager : MonoSingleton<PatternManager>
{
    public List<Pattern> patterns = new List<Pattern>();

    public PatternFuncList funcList;

    private int _patternIndex = 0;

    private void Awake()
    {
        funcList = GetComponent<PatternFuncList>();
    }

    public void PatternInit(List<Pattern> patterns)
    {
        this.patterns = patterns;
        _patternIndex = 0;
    }

    public Pattern GetPattern()
    {
        if (_patternIndex >=  patterns.Count)
        {
            _patternIndex = 0;
        }
        return patterns[_patternIndex++];
    }

    public void FuncInovke(List<PatternFuncEnum> funcList)
    {
        foreach(var name in funcList)
        {
            this.funcList.FuncInvoke(name);
        }
    }
}
