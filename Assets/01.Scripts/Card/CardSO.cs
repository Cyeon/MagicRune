using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// �Ӽ� Enum
/// </summary>
public enum AttributeType
{
    Fire, // ��
    Ice, // ����
    Wind, // �ٶ�
    None, // ���Ӽ�
}

/// <summary>
/// ���� Ŭ����
/// </summary>
[Serializable]
public class Condition
{
    public string ConditionName;
    
    // ���⵵ ���ڰ� �ʿ��ϸ� �������
}

/// <summary>
/// ȿ�� Ŭ����
/// </summary>
[Serializable]
public class Effect
{
    public string effectName;

    public string paramater; // string�� ���̾�
}

[Serializable]
public class EffectPair
{
    // ���� ���� �ʿ�

    public Condition Condition;
    public bool Not; // ���� �ݴ�� �ϱ�
    public Effect Effect;
}

/// <summary>
/// �� ���� Ŭ����
/// </summary>
[Serializable]
public class RuneProperty
{
    // ���� Enum ? �ʿ��Ѱ�?
    public AttributeType Attribute;
    [Min(0)]
    public int Cost; // �ڽ�Ʈ
    [Min(0)]
    public int DelayTurn; // ���� ���� ��

    public List<EffectPair> EffectPair; // ȿ���� ������ ���� ����Ʈ
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class CardSO : ScriptableObject
{
    public string CardName; // ī�� �̸�
    public string CardDescription; // ī�� ����

    public RuneProperty MainRune;
    public RuneProperty AssistRune;
}
