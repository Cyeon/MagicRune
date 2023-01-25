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
/// 룬 정보 클래스
/// </summary>
[Serializable]
public class RuneProperty
{
    [Tooltip("카드 이름")]
    public string Name;
    // 유형 Enum ? 필요한가?
    [ResizableTextArea, Tooltip("카드 효과 텍스트")]
    public string Effect;
    [Tooltip("속성")]
    public AttributeType Attribute;
    [Min(0), Tooltip("코스트")]
    public int Cost; // 코스트
    [Min(0), Tooltip("재사용 가능 턴")]
    public int DelayTurn; // 재사용 가능 턴

    // 연걸이 안됨
    //public Card Effect; // 효과 함수를 갇고 있는 카드
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class CardSO : ScriptableObject
{
    [ShowAssetPreview(32, 32), Tooltip("카드에 들어갈 이미지")]
    public Sprite CardImage;
    [ShowAssetPreview(32, 32), Tooltip("마법진에 들어갈 룬 이미지")]
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
