using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;

[System.Serializable]
public class Distractor
{
    [Tooltip("������ �ؽ�Ʈ")]
    [TextArea(1, 3)]
    public string text;

    [Tooltip("���� ���� �ٸ� ������ �̵��ϴ� �������� üũ ����")]
    public bool isNextStage = true;

    [Tooltip("�����Լ�")]
    public ButtonClickedEvent function;
}