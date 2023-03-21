using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;

[System.Serializable]
public class Distractor
{

    [Tooltip("선택지 텍스트")]
    [TextArea(1, 3)]
    public string text;

    [Tooltip("실행함수")]
    public ButtonClickedEvent function;
}
