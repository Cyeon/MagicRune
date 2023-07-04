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
    Bouncing // 척력
}

public enum KeywardType
{
    Status,
    Noraml
}

[Serializable]
public class Keyward
{
    public string KeywardName;
    public KeywordName TypeName;
    public KeywardType KeywardType;

    [ConditionalField(nameof(KeywardType), false, KeywardType.Noraml)]
    public bool IsAddDesc = false;
    [ConditionalField(nameof(KeywardType), false, KeywardType.Noraml)]
    public string KeywardDescription;

    [ConditionalField(nameof(KeywardType), false, KeywardType.Status)]
    public StatusName KeywardStatus;

    public Keyward()
    {
        KeywardName = "";
        TypeName = KeywordName.None;
        KeywardType = KeywardType.Status;
        KeywardDescription = "";
    }
}

[CreateAssetMenu(menuName = "SO/Keyward/KeywardList")]
public class KeywardListSO : ScriptableObject
{
    public List<Keyward> KeywardList;
}
