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
    Ground, // ��
    Electric, // ����
}

/// <summary>
/// �ൿ ����, �� ������� �ൿ�� �����ϱ� ���� �߿�!
/// </summary>
public enum EffectType
{
    Status,
    Defence, // ����ϴ°� ��) 5���
    Attack, // �����ϴ°� ��) 5������
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

public enum RuneRarity
{
    Normal,
    Rare,
    Epic,
    Legendary,
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
    public ComparisonType ComparisonType;
    [MinValue(0f), ConditionalField(nameof(ConditionType), false, ConditionType.HeathComparison, ConditionType.AttributeComparison, ConditionType.StatusComparison, ConditionType.AssistRuneCount)]
    public float Value;

    [ConditionalField(nameof(ConditionType), false, ConditionType.HeathComparison, ConditionType.StatusComparison)]
    public bool IsEnemy = true;

    public Condition(ConditionType conditionType = ConditionType.None, AttributeType attributeType = AttributeType.None,
        StatusName statusType = StatusName.Null, ComparisonType comparisonType = ComparisonType.MoreThan,
        float value = 0, bool isEnemy = true)
    {
        ConditionType = conditionType;
        AttributeType = attributeType;
        StatusType = statusType;
        ComparisonType = comparisonType;
        Value = value;
        IsEnemy = isEnemy;
    }
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

    [Tooltip("ī�� ȿ�� ���")]
    public float Effect;

    public Pair(Condition condition, EffectType effectType = EffectType.Attack,
        StatusName statusType = StatusName.Null,
        bool isEnemy = true, AttackType attackType = AttackType.Single,
        AttributeType attributeType = AttributeType.None, float effect = 0)
    {
        Condition = condition;
        EffectType = effectType;
        StatusType = statusType;
        IsEnemy = isEnemy;
        AttackType = attackType;
        AttributeType = attributeType;
        Effect = effect;
    }
}

/// <summary>
/// �� ���� Ŭ����
/// </summary>
[Serializable]
public class RuneProperty
{
    
    [Tooltip("ȿ�� ����"), TextArea]
    public string CardDescription;
    // ���� Enum ? �ʿ��Ѱ�?
    public List<Pair> EffectDescription;
    [Tooltip("�Ӽ�")]
    public AttributeType Attribute;
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class RuneSO : ScriptableObject
{
    [Tooltip("ī�� �̸�")]
    public string Name;
    [ShowAssetPreview(32, 32), Tooltip("�������� �� �� �̹���")]
    public Sprite RuneImage;
    [ShowAssetPreview(32, 32), Tooltip("������ ���� ��� ����Ʈ")]
    public GameObject RuneEffect;
    [Min(0), Tooltip("���� ���� ��")]
    public int CoolTime; // ���� ����
    public RuneRarity Rarity;
    [Tooltip("���� ���� �Ӽ�")]
    public RuneProperty MainRune;

    public List<keywordEnum> keywordList = new List<keywordEnum>();
}