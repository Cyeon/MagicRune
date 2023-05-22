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
}

[Serializable]
public class Keyward
{
    public string KeywardName;
    public KeywordType TypeName;
    public string KeywardDescription;

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
