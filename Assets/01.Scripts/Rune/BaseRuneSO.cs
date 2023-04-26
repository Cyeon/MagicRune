using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityValue
{
    public EffectType EffectType;
    public int Value;
}

[CreateAssetMenu(menuName = "SO/Rune/BaseRuneSO")]
public class BaseRuneSO : ScriptableObject
{
    public string RuneName;
    [ShowAssetPreview(32, 32)]
    public Sprite RuneSprite;
    [ResizableTextArea]
    public string RuneDescription; // 필요한가..?
    public AttributeType AttributeType;
    public GameObject RuneEffect;
    public RuneRarity Rarity;
    public int CoolTime;

    // Ability Parameta

    // 진짜 간단하게 능력치 정의. 조건 다 필요없어 어떻거 나가는지만 진짜
    // 공격 5, 빙결 2, 방어 10 이렇게만. 진짜 능력치만
    public List<AbilityValue> AbilityList;


    //private BaseRune을 가지고 잇음

    public void AddRune()
    {
        // BaseRune.Excute 같은 거 실행함 
    }

    ///BaseRune
    // 대충 virtual 함수로 Excute건 뭐건 있음 

    /// 룬 : BaseRune
    // 대충 Excute를 재정의 함 {
    // 덱 매니저.Add(new 대충 룬) 
    // }
}
