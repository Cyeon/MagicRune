using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 속성 Enum
/// </summary>
public enum AttributeType
{
    Fire, // 불
    Ice, // 얼음
    Wind, // 바람
    None, // 무속성
}

/// <summary>
/// 조건 클래스
/// </summary>
[Serializable]
public class Condition
{
    public string ConditionName;
    
    // 여기도 인자값 필요하면 넣으면됨
}

/// <summary>
/// 효과 클래스
/// </summary>
[Serializable]
public class Effect
{
    public string effectName;

    public string paramater; // string은 신이야
}

[Serializable]
public class EffectPair
{
    // 많은 개선 필요

    public Condition Condition;
    public bool Not; // 조건 반대로 하기
    public Effect Effect;
}

/// <summary>
/// 룬 정보 클래스
/// </summary>
[Serializable]
public class RuneProperty
{
    // 유형 Enum ? 필요한가?
    public AttributeType Attribute;
    [Min(0)]
    public int Cost; // 코스트
    [Min(0)]
    public int DelayTurn; // 재사용 가능 턴

    public List<EffectPair> EffectPair; // 효과와 조건을 담은 리스트
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class RuneSO : ScriptableObject
{
    [Tooltip("카드 이름")]
    public string CardName;
    [ShowAssetPreview(32, 32), Tooltip("카드 이미지")]
    public Sprite CardImage;
    [ShowAssetPreview(32, 32), Tooltip("룬 이미지")]
    public Sprite RuneImage;
    [Tooltip("카드 설명"), ResizableTextArea]
    public string CardDescription;
    [Min(1), Tooltip("보조 룬 개수")]
    public int AssistRuneCount = 5;

    [Tooltip("메인 룬의 속성")]
    public RuneProperty MainRune;
    [Tooltip("보조 룬의 속성")]
    public RuneProperty AssistRune;
}
