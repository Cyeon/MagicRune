using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityValue
{
    public EffectType EffectType;
    public int Value;
}

public class BaseCardSO : ScriptableObject
{
    public string RuneName;
    public Sprite RuneImage;
    public string RuneDescription; // 필요한가..?

    // Ability Parameta

    // 진짜 간단하게 능력 정의. 조건 다 필요없어 어떻거 나가는지만 진짜
    // 공격 5, 빙결 2, 방어 10 이렇게만
    public List<AbilityValue> AbilityList;
}
