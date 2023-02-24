using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PatternEnum
{
    Attack,
    Defence,
    Beem,
    Ice
}

public class PatternManager : MonoSingleton<PatternManager>
{

    public List<Pattern> patterns = new List<Pattern>();
    public List<Pattern> codiPatterns = new List<Pattern>();

    public List<Pattern> enemyHavePatterns = new List<Pattern>();

    public PatternFuncList funcList;

    private void Awake()
    {
        funcList = GetComponent<PatternFuncList>();
    }

    public void PatternInit(List<PatternEnum> enumList)
    {
        enemyHavePatterns.Clear();
        for(int i = 0; i < enumList.Count; i++)
        {
            enemyHavePatterns.Add(patterns.Where(e => e.patternEnum == enumList[i]).FirstOrDefault());
        }
    }

    public Pattern GetPattern()
    {
        int index = Random.Range(0, enemyHavePatterns.Count);
        return enemyHavePatterns[index];
    }

    public Pattern GetCodiPattern(string name)
    {
        return codiPatterns.Where(e => e.patternName == name).FirstOrDefault();
    }
}
