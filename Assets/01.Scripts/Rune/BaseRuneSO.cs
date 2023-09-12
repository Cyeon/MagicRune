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
    public int Count = 1;

    [ConditionalField(nameof(EffectType), false, EffectType.Status, EffectType.DestroyStatus)]
    public StatusName StatusName;
}

public enum EffectDirection
{
    Enemy,
    Player,
}

public enum DiscoveryType
{
    Unknwon,
    Find,
    Known
}

[CreateAssetMenu(menuName = "SO/Rune/BaseRuneSO")]
public class BaseRuneSO : ScriptableObject
{
    public string RuneName;
    [ShowAssetPreview(32, 32)]
    public Sprite RuneSprite;
    [ResizableTextArea, SerializeField]
    private string _runeDescription;

    public bool IsUseEnhancedDesc = false;
    [ConditionalField(nameof(IsUseEnhancedDesc))]
    public string EnhancedRuneDescription;

    public bool IsSeeAbilityValue = true;

    public string RuneDescription(in KeywordName[] keywordList, bool isEnhance = false)
    {
        string desc = IsUseEnhancedDesc ? isEnhance ? EnhancedRuneDescription : _runeDescription : _runeDescription;
        desc = desc.Replace("(dmg)", GetAbillityValue(EffectType.Attack, isEnhance: isEnhance) + " 데미지");
        desc = desc.Replace("(status)", GetAbillityValue(EffectType.Status, isEnhance: isEnhance));
        desc = desc.Replace("(status1)", GetAbillityValue(EffectType.Status, 1, isEnhance: isEnhance));
        desc = desc.Replace("(def)", GetAbillityValue(EffectType.Defence, isEnhance: isEnhance) + " 방어");
        desc = desc.Replace("(dStatus)", GetAbillityValue(EffectType.DestroyStatus, isEnhance: isEnhance));
        desc = desc.Replace("(etc)", GetAbillityValue(EffectType.Etc, isEnhance: isEnhance));



        for (int i = 0; i < keywordList.Length; i++)
        {
            if (Managers.Keyword.GetKeyword(KeywardList[i]).IsAddDesc)
                desc += " <color=#FFE951>" + Managers.Keyword.GetKeyword(keywordList[i]).KeywardName + "</color>";
        }

        return desc;
    }

    public DiscoveryType DiscoveryType = DiscoveryType.Unknwon;
    public AttributeType AttributeType;
    public GameObject RuneEffect;

    [SerializeField]
    private RuneRarity _rarity = RuneRarity.Normal;
    public RuneRarity Rarity => _rarity;

    public int CoolTime;
    public int EnhancedCoolTime = -1;
    public EffectDirection Direction;
    public AudioClip RuneSound;

    // Ability Parameta

    // 吏꾩쭨 媛꾨떒?섍쾶 ?λ젰移??뺤쓽. 議곌굔 ???꾩슂?놁뼱 ?대뼸嫄??섍??붿?留?吏꾩쭨
    // 怨듦꺽 5, 鍮숆껐 2, 諛⑹뼱 10 ?대젃寃뚮쭔. 吏꾩쭨 ?λ젰移섎쭔
    public List<AbilityValue> AbilityList;
    public List<AbilityValue> EnhancedAbilityList;

    public KeywordName[] KeywardList;
    public KeywordName[] EnhancedKeywardList;

    public string GetAbillityValue(EffectType type, int index = 0, bool isEnhance = false)
    {
        List<AbilityValue> abilities = (isEnhance ? EnhancedAbilityList : AbilityList).Where(x => x.EffectType == type).ToList();
        if (abilities.Count == 0) return "";

        float? value = abilities[index].Value;
        Managers.StatModifier.GetStatModifierValue(type, ref value);

        string text = value.ToString();
        if (type == EffectType.Status || type == EffectType.DestroyStatus)
        {
            if (abilities[index].StatusName != StatusName.Null)
            {
                string status = Resources.Load("Prefabs/Status/Status_" + abilities[index].StatusName).GetComponent<Status>().debugName;
                text = (IsSeeAbilityValue ? value.ToString() : "") + " <color=#FFE951>" + status + "</color>";
                if(abilities[index].Count > 1)
                {
                    text = (IsSeeAbilityValue ? value.ToString() : "") + " <color=#FFE951>" + status + "</color>" + "을 " + abilities[index].Count + "번";
                }
            }
        }

        return text;
    }
}
