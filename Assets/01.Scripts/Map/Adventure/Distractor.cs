using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Distractor
{
    [Tooltip("선택지 텍스트")]
    [TextArea(1, 1)]
    public string text;

    [Tooltip("실행함수")]
    public AdventureFunc func;
}
