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

    [Tooltip("전투 등의 다른 씬으로 이동하는 선택지는 체크 해제")]
    public bool isNextStage = true;

    [Tooltip("실행함수")]
    public ButtonClickedEvent function;
}