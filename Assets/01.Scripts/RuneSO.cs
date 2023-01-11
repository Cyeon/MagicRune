using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EffectType
{
    Damage, // 데미지 주는 거
    Defence, // 방어
    Utill, // 유틸
    BufAndDebuf, // 버프&디버프
}

/// <summary>
/// 조건 클래스
/// </summary>
[Serializable]
public class Condition
{
    public UnityEvent OnConditionEvent;
}

/// <summary>
/// 효과 클래스
/// </summary>
[Serializable]
public class Effect
{
    public UnityEvent OnEffectEvent;
}

[Serializable]
public class EffectPair
{
    // 많은 개선 필요

    public string ConditionString; // 조건
    public bool Not; // 조건 반대로 하기
    public string EffectString; // 효과
}

/// <summary>
/// 룬 정보 클래스
/// </summary>
[Serializable]
public class RuneProperty
{
    // 유형 Enum
    // 속성 Enum
    [Min(0)]
    public int Cost; // 코스트
    [Min(0)]
    public int DelayTurn; // 재사용 가능 턴

    public List<EffectPair> EffectPair; // 효과와 조건을 담은 리스트
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class RuneSO : ScriptableObject
{
    public string CardName; // 카드 이름
    public string CardDescription; // 카드 설명

    public RuneProperty MainRune;
    public RuneProperty AssistRune;
}
