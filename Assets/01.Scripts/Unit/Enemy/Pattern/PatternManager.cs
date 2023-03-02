using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternManager : MonoSingleton<PatternManager>
{
    public List<Pattern> patterns = new List<Pattern>();
    public List<Pattern> codiPatterns = new List<Pattern>();

    public PatternFuncList funcList;

    private void Awake()
    {
        funcList = GetComponent<PatternFuncList>();
    }

    public void PatternInit(List<Pattern> patterns)
    {
        this.patterns = patterns;
    }

    public Pattern GetPattern()
    {
        int index = Random.Range(0, patterns.Count);
        return patterns[index];
    }

    public Pattern GetCodiPattern(string name)
    {
        return codiPatterns.Where(e => e.info.patternName == name).FirstOrDefault();
    }

    public void FuncInovke(List<string> funcList)
    {
        foreach(var name in funcList)
        {
            this.funcList.FuncInvoke(name);
        }
    }
}
