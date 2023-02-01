using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MyBox;
using MinValue = NaughtyAttributes.MinValueAttribute;

/// <summary>
/// �Ӽ� Enum
/// </summary>
public enum AttributeType
{
    None, // ����
    NonAttribute, // ���Ӽ�
    Fire, // ��
    Ice, // ����
    Wind, // �ٶ�
}

public enum EffectType
{
    Attack, // �����ϴ°� ��) 5������
    Defence, // ����ϴ°� ��) 5���
    Status,
    Destroy,
    Etc, // ��Ÿ ȿ�� ��) 1�� ��ο�, ȭ��ȿ�� �ο� ��...
}

public enum ConditionType
{
    None,
    IfThereIs, // �þ� �ִٸ�
    IfNotThereIs,
    Heath,
    AssistRuneCount,
}

public enum HealthType
{
    None,
    MoreThan, // �̻�
    LessThan, // ����
}

[Serializable]
public class Condition
{
    public ConditionType ConditionType;
    [ConditionalField(nameof(ConditionType), false, ConditionType.IfThereIs, ConditionType.IfNotThereIs)]
    public AttributeType AttributeType;
    [ConditionalField(nameof(ConditionType), false, ConditionType.IfThereIs, ConditionType.IfNotThereIs, ConditionType.Heath)]
    public StatusName StatusType;

    // true : ���̴�, ���� ���̴�, false : ����, �������̴�. 
    [ConditionalField(nameof(ConditionType), false, ConditionType.Heath)]
    public HealthType HeathType;
    [MinValue(0f), ConditionalField(nameof(ConditionType), false, ConditionType.Heath, ConditionType.AssistRuneCount)]
    public float Value;

    [ConditionalField(nameof(ConditionType), true, ConditionType.None), ]
    public bool IsEnemyOrMain = true;
}

[Serializable]
public class Pair
{
    [Tooltip("����")]
    public Condition Condition;
    [Tooltip("ȿ�� ���� �Ӽ�?")]
    public EffectType EffectType;
    [Tooltip("�����̻� �Ӽ�, EffectType == Status�� ���"), ConditionalField(nameof(EffectType), false, EffectType.Status, EffectType.Destroy)]
    public StatusName StatusType;
    [Tooltip("true�� ��, false�� ������ ��"), ConditionalField(nameof(EffectType), false, EffectType.Status, EffectType.Destroy)]
    public bool IsEnemy = true;
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
    [Tooltip("�켱����, 0�� �������� ������ ����"), MinValue(0)]
    public int Priority;
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
