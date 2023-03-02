using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Pattern", menuName = "Unit/Enemy/Pattern")]
public class PatternSO : ScriptableObject
{
    public string patternName;
    [ShowAssetPreview(32, 32), Tooltip("������")] public Sprite icon;

    public List<string> startFunc;
    public List<string> turnFunc;
    public List<string> endFunc;
}
