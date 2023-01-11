using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EffectType
{
    Damage, // ������ �ִ� ��
    Defence, // ���
    Utill, // ��ƿ
    BufAndDebuf, // ����&�����
}

/// <summary>
/// ���� Ŭ����
/// </summary>
[Serializable]
public class Condition
{
    public UnityEvent OnConditionEvent;
}

/// <summary>
/// ȿ�� Ŭ����
/// </summary>
[Serializable]
public class Effect
{
    public UnityEvent OnEffectEvent;
}

[Serializable]
public class EffectPair
{
    // ���� ���� �ʿ�

    public string ConditionString; // ����
    public bool Not; // ���� �ݴ�� �ϱ�
    public string EffectString; // ȿ��
}

/// <summary>
/// �� ���� Ŭ����
/// </summary>
[Serializable]
public class RuneProperty
{
    // ���� Enum
    // �Ӽ� Enum
    [Min(0)]
    public int Cost; // �ڽ�Ʈ
    [Min(0)]
    public int DelayTurn; // ���� ���� ��

    public List<EffectPair> EffectPair; // ȿ���� ������ ���� ����Ʈ
}

[CreateAssetMenu(menuName = "SO/Rune")]
public class RuneSO : ScriptableObject
{
    public string CardName; // ī�� �̸�
    public string CardDescription; // ī�� ����

    public RuneProperty MainRune;
    public RuneProperty AssistRune;
}
