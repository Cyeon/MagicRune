using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeywordName
{
    None,
    Charge, // 충전
    Fire, // 화상 
    Impact, // 충격 
    Chilliness, // 한기
    Ice, // 빙결 
    Consume, // 소모 
    Penetration, // 관통
    Overheat, // 과열
    CantEnhance, // 강화불과
    GroundBeat, // 땅울림
    SelfGeneration, // 자가발전
    Bouncing, // 척력
    Frost, // 서리
    Armor // 갑옷
}

public enum KeywordType
{
    Status,
    Normal
}

[Serializable]
public class Keyword
{
    public string KeywardName;
    public KeywordName TypeName;
    public KeywordType KeywardType;

    [ConditionalField(nameof(KeywardType), false, KeywordType.Normal)]
    public bool IsAddDesc = false;
    [ConditionalField(nameof(KeywardType), false, KeywordType.Normal)]
    public string KeywardDescription;

    [ConditionalField(nameof(KeywardType), false, KeywordType.Status)]
    public StatusName KeywardStatus;


    public Keyword()
    {
        KeywardName = "";
        TypeName = KeywordName.None;
        KeywardType = KeywordType.Status;
        KeywardDescription = "";
    }
}

[CreateAssetMenu(menuName = "SO/Keyward/KeywardList")]
public class KeywardListSO : ScriptableObject
{
    public List<Keyword> KeywardList;
}
