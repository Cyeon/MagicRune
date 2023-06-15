using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeywordType
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
    SelfGeneration // 자가발전
}

[Serializable]
public class Keyward
{
    public string KeywardName;
    public KeywordType TypeName;
    [TextArea(1, 10)]
    public string KeywardDescription;
    public bool IsAddDesc = false;

    public Keyward()
    {
        KeywardName = "";
        TypeName = KeywordType.None;
        KeywardDescription = "";
    }
}

[CreateAssetMenu(menuName = "SO/Keyward/KeywardList")]
public class KeywardListSO : ScriptableObject
{
    public List<Keyward> KeywardList;
}
