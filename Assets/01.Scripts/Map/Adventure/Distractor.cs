using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Distractor
{
    [Tooltip("������ �ؽ�Ʈ")]
    [TextArea(1, 1)]
    public string text;

    [Tooltip("�����Լ�")]
    public AdventureFunc func;
}
