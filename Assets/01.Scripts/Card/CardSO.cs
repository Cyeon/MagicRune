using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// �Ӽ� Enum
/// </summary>
public enum AttributeType
{
    Fire, // ��
    Ice, // ����
    Wind, // �ٶ�
    None, // ���Ӽ�
}

/// <summary>
/// �� ���� Ŭ����
/// </summary>
[Serializable]
public class RuneProperty
{
    [Tooltip("ī�� �̸�")]
    public string Name;
    // ���� Enum ? �ʿ��Ѱ�?
    [ResizableTextArea, Tooltip("ī�� ȿ�� �ؽ�Ʈ")]
    public string Effect;
    [Tooltip("�Ӽ�")]
    public AttributeType Attribute;
    [Min(0), Tooltip("�ڽ�Ʈ")]
    public int Cost; // �ڽ�Ʈ
    [Min(0), Tooltip("���� ���� ��")]
    public int DelayTurn; // ���� ���� ��

    // ������ �ȵ�
    //public Card Effect; // ȿ�� �Լ��� ���� �ִ� ī��
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class CardSO : ScriptableObject
{
    [ShowAssetPreview(32, 32), Tooltip("ī�忡 �� �̹���")]
    public Sprite CardImage;
    [ShowAssetPreview(32, 32), Tooltip("�������� �� �� �̹���")]
    public Sprite RuneImage;
    [Tooltip("ī�� ����"), ResizableTextArea]
    public string CardDescription;
    [Min(1), Tooltip("���� �� ����")]
    public int AssistRuneCount = 5;

    [Tooltip("���� ���� �Ӽ�")]
    public RuneProperty MainRune;
    [Tooltip("���� ���� �Ӽ�")]
    public RuneProperty AssistRune;
}
