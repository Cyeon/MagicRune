using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Pattern", menuName = "Unit/Enemy/Pattern")]
public class PatternSO : ScriptableObject
{
    public string patternName;
    [ShowAssetPreview(32, 32), Tooltip("æ∆¿Ãƒ‹")] public Sprite icon;

    public List<PatternFuncEnum> startFunc = new List<PatternFuncEnum>();
    public List<PatternFuncEnum> turnFunc = new List<PatternFuncEnum>();
    public List<PatternFuncEnum> endFunc = new List<PatternFuncEnum>();
}
