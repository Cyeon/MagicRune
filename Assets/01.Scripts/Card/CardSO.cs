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
    Draw,
    Etc, // ��Ÿ ȿ�� ��) 1�� ��ο�, ȭ��ȿ�� �ο� ��...
}

public enum ConditionType
{
    None,
    HeathComparison,
    AttributeComparison,
    StatusComparison,
    AssistRuneCount,
}

public enum ComparisonType
{
    MoreThan, // �̻�
    LessThan, // ����
}

public enum AttackType
{
    Single,
    Double,
}

[Serializable]
public class Condition
{
    public ConditionType ConditionType;
    [ConditionalField(nameof(ConditionType), false, ConditionType.AttributeComparison)]
    public AttributeType AttributeType;
    [ConditionalField(nameof(ConditionType), false, ConditionType.StatusComparison)]
    public StatusName StatusType;

    // true : ���̴�, ���� ���̴�, false : ����, �������̴�. 
    [ConditionalField(nameof(ConditionType), false, ConditionType.HeathComparison, ConditionType.AttributeComparison, ConditionType.StatusComparison)]
    public ComparisonType HeathType;
    [MinValue(0f), ConditionalField(nameof(ConditionType), false, ConditionType.HeathComparison, ConditionType.AttributeComparison, ConditionType.StatusComparison, ConditionType.AssistRuneCount)]
    public float Value;

    [ConditionalField(nameof(ConditionType), false, ConditionType.HeathComparison, ConditionType.StatusComparison)]
    public bool IsEnemy = true;
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
    [Tooltip("true�� ��, false�� ������ ��"), ConditionalField(nameof(EffectType), false, EffectType.Attack, EffectType.Status, EffectType.Destroy)]
    public bool IsEnemy = true;
    [ConditionalField(nameof(EffectType), true, EffectType.Destroy, EffectType.Draw)]
    public AttackType AttackType;
    [ConditionalField(nameof(AttackType), false, AttackType.Double)]
    public AttributeType AttributeType;

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

    // ���߿� ���� ������
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class CardSO : ScriptableObject
{
    
    [ShowAssetPreview(32, 32), Tooltip("�������� �� �� �̹���")]
    public Sprite RuneImage;
    [ShowAssetPreview(32, 32), Tooltip("������ ���� ��� ����Ʈ")]
    public GameObject RuneEffect;
    [Min(1), Tooltip("���� �� ����")]
    public int AssistRuneCount = 5;

    [Tooltip("���� ���� �Ӽ�")]
    public RuneProperty MainRune;
    [Tooltip("���� ���� �Ӽ�")]
    public RuneProperty AssistRune;
}
