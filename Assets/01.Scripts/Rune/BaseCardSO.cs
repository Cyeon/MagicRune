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
    public string RuneDescription; // �ʿ��Ѱ�..?

    // Ability Parameta

    // ��¥ �����ϰ� �ɷ� ����. ���� �� �ʿ���� ��� ���������� ��¥
    // ���� 5, ���� 2, ��� 10 �̷��Ը�
    public List<AbilityValue> AbilityList;
}
