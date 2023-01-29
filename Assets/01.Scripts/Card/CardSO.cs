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

public enum EffectType
{
    Attack, // �����ϴ°� ��) 5������
    Defence, // ����ϴ°� ��) 5���
    Status,
    Etc, // ��Ÿ ȿ�� ��) 1�� ��ο�, ȭ��ȿ�� �ο� ��...
}

[Serializable]
public class Pair
{
    [Tooltip("ȿ�� ���� �Ӽ�?")]
    public EffectType EffectType;
    [Tooltip("�����̻� �Ӽ�, EffectType == Status�� ���")]
    public StatusName StatusType;
    [ResizableTextArea, Tooltip("ī�� ȿ�� �ؽ�Ʈ")]
    public string Effect;
}

/// <summary>
/// �� ���� Ŭ����
/// </summary>
[Serializable]
public class RuneProperty
{
    [Tooltip("ī�� �̸�")]
    public string Name;
    [ShowAssetPreview(32, 32), Tooltip("�̹���")]
    public Sprite CardImage;
    [Tooltip("ī�� ����"), ResizableTextArea]
    public string CardDescription;
    // ���� Enum ? �ʿ��Ѱ�?
    public List<Pair> EffectDescription;
    [Tooltip("�Ӽ�")]
    public AttributeType Attribute;
    [Min(0), Tooltip("�ڽ�Ʈ")]
    public int Cost; // �ڽ�Ʈ
    [Min(0), Tooltip("���� ���� ��")]
    public int DelayTurn; // ���� ���� ��
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class CardSO : ScriptableObject
{
    
    [ShowAssetPreview(32, 32), Tooltip("�������� �� �� �̹���")]
    public Sprite RuneImage;
    [Min(1), Tooltip("���� �� ����")]
    public int AssistRuneCount = 5;

    [Tooltip("���� ���� �Ӽ�")]
    public RuneProperty MainRune;
    [Tooltip("���� ���� �Ӽ�")]
    public RuneProperty AssistRune;
}
