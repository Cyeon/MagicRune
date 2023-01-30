using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoSingleton<PatternManager>
{
    public List<Pattern> patterns = new List<Pattern>();

    public PatternFuncList funcList;

    private void Awake()
    {
        funcList = GetComponent<PatternFuncList>();
    }

    public Pattern GetPattern()
    {
        int index = Random.Range(0, patterns.Count);
        return patterns[index];
    }
}
