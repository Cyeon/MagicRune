using MyBox;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class AbilityValue
{
    public EffectType EffectType;
    public int Value;

    [ConditionalField(nameof(EffectType), false, EffectType.Status, EffectType.DestroyStatus)]
    public StatusName StatusName;
}

public enum EffectDirection
{
    Enemy,
    Player,
}

[CreateAssetMenu(menuName = "SO/Rune/BaseRuneSO")]
public class BaseRuneSO : ScriptableObject
{
    public string RuneName;
    [ShowAssetPreview(32, 32)]
    public Sprite RuneSprite;
    [ResizableTextArea, SerializeField]
    private string _runeDescription;

    public string RuneDescription
    {
        get
        {
            string desc = _runeDescription;
            desc = desc.Replace("(dmg)", GetAbillityValue(EffectType.Attack) + "데미지");
            desc = desc.Replace("(status)", GetAbillityValue(EffectType.Status));
            desc = desc.Replace("(def)", GetAbillityValue(EffectType.Defence) + "방어");
            desc = desc.Replace("(dStatus)", GetAbillityValue(EffectType.DestroyStatus));
            return desc;
        }
    }

    public AttributeType AttributeType;
    public GameObject RuneEffect;
    public RuneRarity Rarity;
    public int CoolTime;
    public EffectDirection Direction;
    public AudioClip RuneSound;

    // Ability Parameta

    // 진짜 간단하게 능력치 정의. 조건 다 필요없어 어떻거 나가는지만 진짜
    // 공격 5, 빙결 2, 방어 10 이렇게만. 진짜 능력치만
    public List<AbilityValue> AbilityList;

    public KeywordType[] KeywardList;

    public string GetAbillityValue(EffectType type, int index = 0)
    {
        List<AbilityValue> abilities = AbilityList.Where(x => x.EffectType == type).ToList();
        if (abilities.Count == 0) return "";

        float? value = abilities[index].Value;
        Managers.StatModifier.GetStatModifierValue(type, ref value);

        string text = value.ToString();
        if(type == EffectType.Status || type == EffectType.DestroyStatus)
        {
            if (abilities[index].StatusName != StatusName.Null)
            {
                text += Resources.Load("Prefabs/Status/Status_" + abilities[index].StatusName).GetComponent<Status>().debugName;
            }
        }

        return text;
    }
}
