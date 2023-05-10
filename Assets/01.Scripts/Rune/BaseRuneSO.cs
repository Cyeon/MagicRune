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
    [ResizableTextArea]
    public string RuneDescription; // �ʿ��Ѱ�..?
    public AttributeType AttributeType;
    public GameObject RuneEffect;
    public RuneRarity Rarity;
    public int CoolTime;
    public EffectDirection Direction;
    public AudioClip RuneSound;

    // Ability Parameta

    // ��¥ �����ϰ� �ɷ�ġ ����. ���� �� �ʿ���� ��� ���������� ��¥
    // ���� 5, ���� 2, ��� 10 �̷��Ը�. ��¥ �ɷ�ġ��
    public List<AbilityValue> AbilityList;

    public KeywardType[] KeywardList;
}
