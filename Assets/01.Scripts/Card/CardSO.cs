using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using MyBox;
using MinValue = NaughtyAttributes.MinValueAttribute;

/// <summary>
/// 속성 Enum
/// </summary>
public enum AttributeType
{
    None, // 없음
    NonAttribute, // 무속성
    Fire, // 불
    Ice, // 얼음
    Wind, // 바람
}

public enum EffectType
{
    Attack, // 공격하는거 예) 5데미지
    Defence, // 방어하는거 예) 5방어
    Status,
    Destroy,
    Draw,
    Etc, // 기타 효과 예) 1장 드로우, 화상효과 부여 등...
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
    MoreThan, // 이상
    LessThan, // 이하
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

    // true : 적이다, 메인 룬이다, false : 나다, 보조룬이다. 
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
    [Tooltip("조건")]
    public Condition Condition;
    [Tooltip("효과 간단 속성?")]
    public EffectType EffectType;
    [Tooltip("상태이상 속성, EffectType == Status면 사용"), ConditionalField(nameof(EffectType), false, EffectType.Status, EffectType.Destroy)]
    public StatusName StatusType;
    [Tooltip("true면 적, false면 나한태 씀"), ConditionalField(nameof(EffectType), false, EffectType.Attack, EffectType.Status, EffectType.Destroy)]
    public bool IsEnemy = true;
    [ConditionalField(nameof(EffectType), true, EffectType.Destroy, EffectType.Draw)]
    public AttackType AttackType;
    [ConditionalField(nameof(AttackType), false, AttackType.Double)]
    public AttributeType AttributeType;

    [ResizableTextArea, Tooltip("카드 효과 텍스트")]
    public string Effect;
}

/// <summary>
/// 룬 정보 클래스
/// </summary>
[Serializable]
public class RuneProperty
{
    [Tooltip("카드 이름")]
    public string Name;
    [ShowAssetPreview(32, 32), Tooltip("이미지")]
    public Sprite CardImage;
    [Tooltip("카드 설명"), ResizableTextArea]
    public string CardDescription;
    // 유형 Enum ? 필요한가?
    public List<Pair> EffectDescription;
    [Tooltip("속성")]
    public AttributeType Attribute;
    [Min(0), Tooltip("코스트")]
    public int Cost; // 코스트
    [Min(0), Tooltip("재사용 가능 턴")]
    public int DelayTurn; // 재사용 가능 턴

    // 나중에 공격 범위도
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class CardSO : ScriptableObject
{
    
    [ShowAssetPreview(32, 32), Tooltip("마법진에 들어갈 룬 이미지")]
    public Sprite RuneImage;
    [ShowAssetPreview(32, 32), Tooltip("마법진 위에 띄울 이펙트")]
    public GameObject RuneEffect;
    [Min(1), Tooltip("보조 룬 개수")]
    public int AssistRuneCount = 5;

    [Tooltip("메인 룬의 속성")]
    public RuneProperty MainRune;
    [Tooltip("보조 룬의 속성")]
    public RuneProperty AssistRune;
}
