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

    public string RuneDescription(bool isEnhance = false)
    {
        string desc = _runeDescription;
        desc = desc.Replace("(dmg)", GetAbillityValue(EffectType.Attack, isEnhance:isEnhance) + " 데미지");
        desc = desc.Replace("(status)", GetAbillityValue(EffectType.Status, isEnhance: isEnhance));
        desc = desc.Replace("(def)", GetAbillityValue(EffectType.Defence, isEnhance: isEnhance) + " 방어");
        desc = desc.Replace("(dStatus)", GetAbillityValue(EffectType.DestroyStatus, isEnhance: isEnhance));

        for (int i = 0; i < KeywardList.Length; i++)
        {
            desc += " <color=#FFE951>" + Managers.Keyward.GetKeyward(KeywardList[i]).KeywardName + "</color>";
        }

        return desc;
    }

    public AttributeType AttributeType;
    public GameObject RuneEffect;
    public RuneRarity Rarity;
    public int CoolTime;
    public EffectDirection Direction;
    public AudioClip RuneSound;

    // Ability Parameta

    // 吏꾩쭨 媛꾨떒?섍쾶 ?λ젰移??뺤쓽. 議곌굔 ???꾩슂?놁뼱 ?대뼸嫄??섍??붿?留?吏꾩쭨
    // 怨듦꺽 5, 鍮숆껐 2, 諛⑹뼱 10 ?대젃寃뚮쭔. 吏꾩쭨 ?λ젰移섎쭔
    public List<AbilityValue> AbilityList;
    public List<AbilityValue> EnhancedAbilityList;

    public KeywordType[] KeywardList;

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
                text = value.ToString() + " <color=#FFE951>" + status + "</color>";
            }
        }

        return text;
    }
}
